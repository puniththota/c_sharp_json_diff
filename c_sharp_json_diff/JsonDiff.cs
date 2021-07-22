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
                Dictionary<string, IJsonProcessor<object>> processors = new Dictionary<string, IJsonProcessor<object>>();
                processors.Add("Int Diff", new IntDiffProcessor());
                processors.Add("Double Diff", new DoubleDiffProcessor());
                InjectProcessors(processors, diffJsonObject);
                return diffJsonObject;
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

        private string JsonDiffBetweenTwo(string left, string right)
        {
            return new JsonDiffPatch(new Options { ArrayDiff = ArrayDiffMode.Efficient }).Diff(left, right);
        }

        private void InjectProcessors(Dictionary<string, IJsonProcessor<object>> processors, JObject jObject)
        {
            foreach(KeyValuePair<string, IJsonProcessor<object>> keyValuePair in processors)
            {
                keyValuePair.Value.Process(keyValuePair.Key, jObject);
            }
        }
    }
}
