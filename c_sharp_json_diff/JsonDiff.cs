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
            JObject diffJsonObject = JObject.Parse(diff);
            foreach(KeyValuePair<string, JToken?> prop in diffJsonObject)
            {
                if(prop.Value != null)
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
                        else
                        {
                            prop.Value.Remove();
                        }
                    }
                }
            }
            string result = diffJsonObject.ToString(Formatting.Indented);
            return diffJsonObject;
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
