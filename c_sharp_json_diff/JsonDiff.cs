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
        private bool _isDiffIndented;
        private List<IJsonProcessor<object>> Processors = new List<IJsonProcessor<object>>();

        public JsonDiff(bool isDiffIndented)
        {
            _isDiffIndented = isDiffIndented;
        }

        public JsonDiff()
        {
            _isDiffIndented = true;
        }

        /// <summary>
        /// Method do a numeric diff between two Json strings. Method throws a custom JsonDiffFailedException in case of any exception
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns name="diffReturnObject">Returns a numeric diff object of type JObject</returns>
        public JObject NumericDoubleDiff(string left, string right)
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
                new DoubleDiffProcessor().Process(diffJsonObject);
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
        public string NumericDoubleDiffAsString(string left, string right)
        {
            JObject diff = NumericDoubleDiff(left, right);
            return _isDiffIndented ? diff.ToString(Formatting.Indented) : diff.ToString();

        }

        private string JsonDiffBetweenTwo(string left, string right)
        {
            return new JsonDiffPatch(new Options { ArrayDiff = ArrayDiffMode.Efficient }).Diff(left, right);
        }

        private void InjectProcessors(List<IJsonProcessor<object>> processors, JObject jObject)
        {
            foreach(IJsonProcessor<object> processor in processors)
            {
                processor.Process(jObject);
            }
        }
    }
}
