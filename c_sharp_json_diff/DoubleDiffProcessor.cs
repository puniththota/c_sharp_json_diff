using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace c_sharp_json_diff
{
    class DoubleDiffProcessor : DiffProcessor
    {
        /// <summary>
        /// Perform a double diff on the left and right tokens
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>Dictionary of type NumericDiff with two properties - subtraction and division. An empty or null dictionary means the diff will be skipped.
        /// The Division property will be -1(error case) if the denominator (left token) value is 0</returns>
        public override Dictionary<string, object> PerformDiffProcess(JToken left, JToken right)
        {
            string leftValueAsString = left.ToString();
            string rightValueAsString = right.ToString();
            Dictionary<string, object> dict = new Dictionary<string, object>();

            NumericDiff diff = new NumericDiff();

            if (double.TryParse(leftValueAsString, out double leftValueAsDouble) && double.TryParse(rightValueAsString, out double rightValueAsDouble))
            {
                diff.Subtraction = rightValueAsDouble - leftValueAsDouble;
                diff.Division = leftValueAsDouble == 0 ? -1 : rightValueAsDouble / leftValueAsDouble;
            }


            if (diff.Subtraction != null && diff.Division != null)
            {
                dict.Add("Subtraction", diff.Subtraction);
                dict.Add("Division", diff.Division);
            }

            return dict;
        }
    }
}
