using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace c_sharp_json_diff
{
    interface IJsonProcessor<T>
    {
        void Process(JObject jObject);
    }
}
