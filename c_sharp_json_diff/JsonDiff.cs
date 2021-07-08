using JsonDiffPatchDotNet;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;

namespace c_sharp_json_diff
{
    public class JsonDiff
    {
        private bool _isAbsoluteDiff;
        private bool _isDiffIndented;
        private const string skippableProperty = "_t";
        private const string propertyLeft = "left";
        private const string propertyRight = "right";
        private const string propertyDiff = "diff";

        public JsonDiff(bool isAbsoluteDiff, bool isDiffIndented)
        {
            _isAbsoluteDiff = isAbsoluteDiff;
            _isDiffIndented = isDiffIndented;
        }

        public JsonDiff()
        {
            _isAbsoluteDiff = true;
            _isDiffIndented = true;
        }

        /// <summary>
        /// Method do a numeric diff between two Json strings. Method throws a custom JsonDiffFailedException in case of any exception
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns name="diffReturnObject">Returns a numeric diff object of type JObject</returns>
        public JObject NumericDiff(string left, string right)
        {
            string diff = "";
            try
            {
                diff = JsonDiffBetweenTwo(left, right);
                if (diff == null)
                {
                    return JObject.Parse(@"{}");
                }
                JObject diffJsonObject = JObject.Parse(diff);
                JObject diffReturnObject = CalculateNumericDiff(diffJsonObject, _isAbsoluteDiff);
                return diffReturnObject;
            }
            catch (Exception e)
            {
                throw new JsonDiffFailedException("Exception encountered while performing json numeric diff", e);
            }
        }

        /// <summary>
        /// Method do a numeric diff between two Json strings. Method throws a custom JsonDiffFailedException in case of any exception
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>Returns a numeric diff string between left and right</returns>
        public string NumericDiffAsString(string left, string right)
        {
            JObject diff = NumericDiff(left, right);
            return _isDiffIndented ? diff.ToString(Formatting.Indented) : diff.ToString();

        }

        private JObject CalculateNumericDiff(JObject jObject, bool isAbsoluteDiff)
        {
            foreach (KeyValuePair<string, JToken?> prop in jObject)
            {
                if (ShouldSkipProperty(prop.Key))
                {
                    continue;
                }
                if (prop.Value != null)
                {
                    if (prop.Value is JArray valuesAsArray)
                    {
                        if (valuesAsArray.Count >= 2)
                        {
                            if (valuesAsArray[0] != null && valuesAsArray[1] != null)
                            {
                                string leftValueAsString = valuesAsArray[0].ToString();
                                string rightValueAsString = valuesAsArray[1].ToString();
                                if (int.TryParse(leftValueAsString, out int leftValueAsInt) && int.TryParse(rightValueAsString, out int rightValueAsInt))
                                {
                                    int diffValue = isAbsoluteDiff ? Math.Abs(leftValueAsInt - rightValueAsInt) : leftValueAsInt - rightValueAsInt;
                                    var map = CreateDictionary(leftValueAsInt, rightValueAsInt, diffValue);
                                    JToken jToken = JToken.FromObject(map);
                                    prop.Value.Replace(jToken);
                                }
                                else if (double.TryParse(leftValueAsString, out double leftValueAsDouble) && double.TryParse(rightValueAsString, out double rightValueAsDouble))
                                {
                                    double diffValue = isAbsoluteDiff ? Math.Abs(leftValueAsDouble - rightValueAsDouble) : leftValueAsDouble - rightValueAsDouble;
                                    var map = CreateDictionary(leftValueAsDouble, rightValueAsDouble, diffValue);
                                    JToken jToken = JToken.FromObject(map);
                                    prop.Value.Replace(jToken);
                                }

                            }
                        }
                    }
                    else
                    {
                        if (prop.Value is JObject jsonObject)
                        {
                            CalculateNumericDiff(jsonObject, isAbsoluteDiff);
                        }
                    }
                }
            }
            return jObject;
        }

        private string JsonDiffBetweenTwo(string left, string right)
        {
            return new JsonDiffPatch(new Options { ArrayDiff = ArrayDiffMode.Efficient }).Diff(left, right);
        }

        private Dictionary<object, object> CreateDictionary(object left, object right, object diff)
        {
            Dictionary<object, object> map = new Dictionary<object, object>();
            map.Add(propertyLeft, left);
            map.Add(propertyRight, right);
            map.Add(propertyDiff, diff);
            return map;
        }

        private bool ShouldSkipProperty(string property)
        {
            return property == skippableProperty;
        }
    }
}
