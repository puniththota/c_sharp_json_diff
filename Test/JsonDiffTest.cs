using c_sharp_json_diff;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

namespace Test
{
    public class JsonDiffTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void WhenJsonDiffOnTwoStringsThenCorrectValueReturned()
        {
            string left = @"{
                              'number': 10,
                              'sommething': 20
                             }";
            string right = @"{
                              'number': 5,
                              'sommething': 10
                             }";
            JObject diffObject = JsonDiff.NumericDiff(left, right);
            JArray array = (JArray)diffObject.Value<JToken>("number");
            Assert.IsNotNull(array[2]);
            Assert.AreEqual(array.Count, 3);
            Assert.AreEqual(int.Parse(array[2].ToString()), 5);
        }
    }
}