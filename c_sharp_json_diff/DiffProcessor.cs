using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace c_sharp_json_diff
{
    abstract class DiffProcessor : IJsonProcessor<object>
    {
        private const string SkippableProperty = "_t";
        private const string KeyDiff = "Double Diff";
        private const string KeyLeft = "Left";
        private const string KeyRight = "Right";

        public void Process(JObject jObject)
        {
            foreach (KeyValuePair<string, JToken?> keyValuePair in jObject)
            {
                if (ShouldSkipProperty(keyValuePair.Key))
                {
                    continue;
                }

                if (keyValuePair.Value != null)
                {
                    if (keyValuePair.Value is JArray valuesAsArray)
                    {
                        if (valuesAsArray.Count == 2)
                        {
                            if (valuesAsArray[0] != null && valuesAsArray[1] != null)
                            {
                                Dictionary<string, object> newJsonObj =  PerformDiffProcess(valuesAsArray[0], valuesAsArray[1]);
                                Dictionary<object, object> dict = new Dictionary<object, object>();
                                if (newJsonObj != null && newJsonObj.Count != 0) 
                                {
                                    dict = CreateDictionary(KeyDiff, valuesAsArray[0], valuesAsArray[1], newJsonObj);
                                    keyValuePair.Value.Replace(JToken.FromObject(dict));
                                }
                            }
                        }
                    }
                    else
                    {
                        if(keyValuePair.Value is JObject jsonObj)
                        {
                            Process(jsonObj);
                        }
                    }
                }
            }
        }

        public abstract Dictionary<string, object> PerformDiffProcess(JToken left, JToken right);

        private Dictionary<object, object> CreateDictionary(string diffKey, object left, object right, object diff)
        {
            Dictionary<object, object> map = new Dictionary<object, object>();
            map.Add(KeyLeft, left);
            map.Add(KeyRight, right);
            map.Add(diffKey, diff);
            return map;
        }

        private bool ShouldSkipProperty(string property)
        {
            return property == SkippableProperty;
        }
    }
}
