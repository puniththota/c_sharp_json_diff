using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;

namespace c_sharp_json_diff
{
    public class Result {
        public List<Diff> Diffs { get; set; } = new List<Diff>();

    }
    public class Diff
    {
        [JsonProperty("previous")]
        public double previous { get; set; }
        [JsonProperty("current")]
        public double current { get; set; }
        [JsonProperty("difference")]
        public double difference { get; set; }
    }
    public class JsonDiff
    {
        public static Result GetDiff(IDictionary<string, double> first, IDictionary<string, double> second)
        {
           ICollection<String> firstKeys = first.Keys;
            ICollection<String> secondKeys = second.Keys;
            List<String> matches = new List<string>();
            Result result = new Result();
            foreach (string key in secondKeys)
            {
                if (firstKeys.Contains(key))
                {
                    matches.Add(key);
                }
            }
            foreach(string key in matches)
            {
                first.TryGetValue(key, out double firstValue);
                second.TryGetValue(key, out double secondValue);
                if(firstValue != secondValue)
                {
                    Diff item = new Diff();
                    item.previous = firstValue;
                    item.current = secondValue;
                    item.difference = Math.Abs(firstValue - secondValue);
                    result.Diffs.Add(item);
                }
            }
            return result;
        }
    }
}
