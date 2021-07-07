using System;
using System.Collections.Generic;
using System.Text;

namespace c_sharp_json_diff
{
    public class JsonDiffFailedException : Exception
    {
        public JsonDiffFailedException(string message, Exception exception) : base(message, exception) { }
    }
}
