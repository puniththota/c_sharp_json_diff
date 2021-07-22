using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace c_sharp_json_diff
{
    class DoubleDiffProcessor : DiffProcessor
    {
        public override Dictionary<string, object> PerformDiffProcess(JToken left, JToken right)
        {
            string leftValueAsString = left.ToString();
            string rightValueAsString = right.ToString();
            Dictionary<string, object> dict = new Dictionary<string, object>();

            if (!IsIntable(leftValueAsString, rightValueAsString))
            {
                NumericDiff diff = new NumericDiff();

                if (double.TryParse(leftValueAsString, out double leftValueAsDouble) && double.TryParse(rightValueAsString, out double rightValueAsDouble))
                {
                    diff.Subtraction = leftValueAsDouble - rightValueAsDouble;
                    diff.Division = rightValueAsDouble == 0 ? -1 : leftValueAsDouble / rightValueAsDouble;
                }


                if (diff.Subtraction != null && diff.Division != null)
                {
                    dict.Add("Subtraction", diff.Subtraction);
                    dict.Add("Division", diff.Division);
                }
            }

            return dict;
        }

        private bool IsIntable(string left, string right)
        {
            return int.TryParse(left, out int leftAsInt) && int.TryParse(right, out int rightAsInt);
        }
    }
}
