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
                return JObject.Parse(diff + e.StackTrace);
            }
        }

        public string NumericDiffAsString(string left, string right)
        {
            JObject diff = NumericDiff(left, right);
            return _isDiffIndented ? diff.ToString(Formatting.Indented) : diff.ToString();

        }

        private JObject CalculateNumericDiff(JObject jObject, bool isAbsoluteDiff)
        {
            foreach (KeyValuePair<string, JToken?> prop in jObject)
            {
                if (prop.Key == "_t")
                {
                    continue;
                }
                if (prop.Value != null)
                {
                    if (prop.Value is JArray)
                    {
                        JArray valuesAsArray = (JArray)prop.Value;
                        if (valuesAsArray.Count >= 2)
                        {
                            if (valuesAsArray[0] != null && valuesAsArray[1] != null)
                            {
                                string leftValueAsString = valuesAsArray[0].ToString();
                                string rightValueAsString = valuesAsArray[1].ToString();
                                if (int.TryParse(leftValueAsString, out int leftValueAsInt) && int.TryParse(rightValueAsString, out int rightValueAsInt))
                                {
                                    int diffValue = isAbsoluteDiff ? Math.Abs(leftValueAsInt - rightValueAsInt) : leftValueAsInt - rightValueAsInt;
                                    Dictionary<string, int> map = CreateDictionary<string, int>("left", "right", "diff", leftValueAsInt, rightValueAsInt, diffValue);
                                    valuesAsArray.RemoveAll();
                                    valuesAsArray.Add(JToken.FromObject(map));
                                }
                                else if (float.TryParse(leftValueAsString, out float leftValueAsFloat) && float.TryParse(rightValueAsString, out float rightValueAsFloat))
                                {
                                    float diffValue = isAbsoluteDiff ? Math.Abs(leftValueAsFloat - rightValueAsFloat) : leftValueAsFloat - rightValueAsFloat;
                                    Dictionary<string, float> map = CreateDictionary<string, float>("left", "right", "diff", leftValueAsFloat, rightValueAsFloat, diffValue);
                                    valuesAsArray.RemoveAll();
                                    valuesAsArray.Add(JToken.FromObject(map));
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

        private Dictionary<TKey, TValue> CreateDictionary<TKey, TValue>(TKey leftKey, TKey rightKey, TKey diffKey, TValue left, TValue right, TValue diff)
        {
            Dictionary<TKey, TValue> map = new Dictionary<TKey, TValue>();
            map.Add(leftKey, left);
            map.Add(rightKey, right);
            map.Add(diffKey, diff);
            return map;
        }
    }
}
