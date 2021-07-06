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
        private string _leftJsonString = "";
        private string _rightJsonString = "";

        public JsonDiff(string left, string right)
        {
            _leftJsonString = left;
            _rightJsonString = right;
        }

        public JObject NumericDiff(bool isAbsoluteDiff)
        {
            string diff = "";
            try
            {
                diff = JsonDiffBetweenTwo(_leftJsonString, _rightJsonString);
                if (diff == null)
                {
                    return JObject.Parse(@"{}");
                }
                JObject diffJsonObject = JObject.Parse(diff);
                JObject diffReturnObject = CalculateNumericDiff(diffJsonObject, isAbsoluteDiff);
                return diffReturnObject;
            } catch(Exception e)
            {
                return JObject.Parse(diff + e.StackTrace);
            }
        }

        public string NumericDiffAsString(bool isAbsoluteDiff)
        {
            JObject diff = NumericDiff(isAbsoluteDiff);
            return diff.ToString(Formatting.Indented);

        }

        private JObject CalculateNumericDiff(JObject jObject, bool isAbsoluteDiff)
        {
            foreach (KeyValuePair<string, JToken?> prop in jObject)
            {
                if(prop.Key == "_t")
                {
                    continue;
                }
                if (prop.Value != null)
                {
                    if (prop.Value is JArray)
                    {
                        JArray valuesAsArray = (JArray)prop.Value;
                        if (valuesAsArray[0] != null && valuesAsArray[1] != null)
                        {
                            string leftValueAsString = valuesAsArray[0].ToString();
                            string rightValueAsString = valuesAsArray[1].ToString();
                            if (int.TryParse(leftValueAsString, out int leftValueAsInt) && int.TryParse(rightValueAsString, out int rightValueAsInt))
                            {
                                int diffValue = isAbsoluteDiff ? Math.Abs(leftValueAsInt - rightValueAsInt) : leftValueAsInt - rightValueAsInt;
                                Dictionary<string, int> map = new Dictionary<string, int>();
                                map.Add("left", leftValueAsInt);
                                map.Add("right", rightValueAsInt);
                                map.Add("diff", diffValue);
                                valuesAsArray.RemoveAll();
                                valuesAsArray.Add(JToken.FromObject(map));
                            }
                            else if (float.TryParse(leftValueAsString, out float leftValueAsFloat) && float.TryParse(rightValueAsString, out float rightValueAsFloat))
                            {
                                float diffValue = isAbsoluteDiff ? Math.Abs(leftValueAsFloat - rightValueAsFloat) : leftValueAsFloat - rightValueAsFloat;
                                Dictionary<string, float> map = new Dictionary<string, float>();
                                map.Add("left", leftValueAsFloat);
                                map.Add("right", rightValueAsFloat);
                                map.Add("diff", diffValue);
                                valuesAsArray.RemoveAll();
                                valuesAsArray.Add(JToken.FromObject(map));
                            }

                        }
                    }
                    else
                    {
                        CalculateNumericDiff((JObject)prop.Value, isAbsoluteDiff);
                    }
                }
            }
            return jObject;
        }

        private string JsonDiffBetweenTwo(string left, string right)
        {
            return new JsonDiffPatch(new Options { ArrayDiff = ArrayDiffMode.Efficient }).Diff(left, right);
        }
    }
}
