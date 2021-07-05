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
        public static JObject NumericDiff(string left, string right)
        {
            string diff = JsonDiffBetweenTwo(left, right);
            if(diff == null)
            {
                return JObject.Parse(@"{}");
            }
            JObject diffJsonObject = JObject.Parse(diff);
            JObject diffReturnObject = CalculateNumericDiff(diffJsonObject);
            return diffReturnObject;
        }

        private static JObject CalculateNumericDiff(JObject jObject)
        {
            foreach (KeyValuePair<string, JToken?> prop in jObject)
            {
                if (prop.Value != null)
                {
                    if (!(prop.Value is JArray))
                    {
                        CalculateNumericDiff((JObject)prop.Value);
                    }
                    else
                    {
                        JArray valuesAsArray = (JArray)prop.Value;
                        if (valuesAsArray[0] != null && valuesAsArray[1] != null)
                        {
                            string leftValueAsString = valuesAsArray[0].ToString();
                            string rightValueAsString = valuesAsArray[1].ToString();
                            if (int.TryParse(leftValueAsString, out int leftValueAsInt) && int.TryParse(rightValueAsString, out int rightValueAsInt))
                            {
                                int diffValue = Math.Abs(leftValueAsInt - rightValueAsInt);
                                valuesAsArray.Add(diffValue);
                            }
                            else if (float.TryParse(leftValueAsString, out float leftValueAsFloat) && float.TryParse(rightValueAsString, out float rightValueAsFloat))
                            {
                                float diffValue = Math.Abs(leftValueAsFloat - rightValueAsFloat);
                                valuesAsArray.Add(diffValue);
                            }
                            else if (double.TryParse(leftValueAsString, out double leftValueAsDouble) && double.TryParse(rightValueAsString, out double rightValueAsDouble))
                            {
                                double diffValue = Math.Abs(leftValueAsDouble - rightValueAsDouble);
                                valuesAsArray.Add(diffValue);
                            }
                        }
                    }
                }
            }
            return jObject;
        }

        public static string NumericDiffAsString(string left, string right)
        {
            JObject diff = NumericDiff(left, right);
            return diff.ToString(Formatting.Indented);

        }

        private static string JsonDiffBetweenTwo(string left, string right)
        {
            return new JsonDiffPatch().Diff(left, right);
        }
    }
}
