using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace c_sharp_json_diff
{
    class IntDiffProcessor : DiffProcessor
    {

        public override Dictionary<string, object> PerformDiffProcess(JToken left, JToken right)
        {
            string leftValueAsString = left.ToString();
            string rightValueAsString = right.ToString();
            NumericDiff diff = new NumericDiff();

            if (int.TryParse(leftValueAsString, out int leftValueAsInt) && int.TryParse(rightValueAsString, out int rightValueAsInt))
            {
                diff.Subtraction = leftValueAsInt - rightValueAsInt;
                diff.Division = rightValueAsInt == 0 ? -1 : leftValueAsInt / rightValueAsInt;
            }

            Dictionary<string, object> dict = new Dictionary<string, object>();
            
            if (diff.Subtraction != null && diff.Division != null)
            {
                dict.Add("Subtraction", diff.Subtraction);
                dict.Add("Division", diff.Division);
            }

            return dict;
        }
    }
}
