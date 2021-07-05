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

        [Test, Category("Unit")]
        [TestCaseSource("ProviderForNumericDiffTest")]
        public void Given_TwoJsonStrings_When_NumericDiff_Then_ExpectedResultReturned(string left, string right, string expected)
        {
            JObject actualNumericDiff = JsonDiff.NumericDiff(left, right);
            JObject expectedNumericDiff = JObject.Parse(expected);

            Assert.AreEqual(expectedNumericDiff.Count, actualNumericDiff.Count);

            IEnumerable<JProperty> actualProperties = actualNumericDiff.Properties();
            IEnumerable<JProperty> expectedProperties = expectedNumericDiff.Properties();
            Assert.AreEqual(expectedProperties, actualProperties);

            if (expectedNumericDiff.ContainsKey("number") && actualNumericDiff.ContainsKey("number"))
            {
                JArray expectedArray = (JArray)expectedNumericDiff.Value<JToken>("number");
                JArray actualArray = (JArray)actualNumericDiff.Value<JToken>("number");
                Assert.AreEqual(expectedArray[2], actualArray[2]);
            }
        }

        public static IEnumerable<object[]> ProviderForNumericDiffTest()
        {
            return new List<object[]>()
            {
                new object[] { @"{
                              'number': 10,
                              'sommething': 20
                             }", @"{
                              'number': 5,
                              'sommething': 10
                             }", @"{
                              'number': [
                                 10,
                                  5,
                                  5
                                 ],
                             'something': [
                                 20,
                                 10,
                                 10
                                 ]
                                }" },
                new object[]
                { @"{
                              'number': 10,
                              'sommething': {
                               'inside': 20
                                }
                             }", @"{
                              'number': 5,
                              'sommething': {
                               'inside': 10
                                }
                             }", @"{
                                 'number': [
                                    10,
                                     5,
                                     5
                                    ],
                                'sommething': {
                                   'inside': [
                                      20,
                                      10,
                                      10
                                       ]
                                    }
                                   }"

                },
                new object[]
                {
                    @"{
	                 'number': 10
                    }", 
                    @"{
	                 'number': 10
                    }",
                    @"{}"
                },
                new object[]
                {
                    @"{
                              'number': 10,
                              'sommething': 20
                             }", @"{
                              'number': 5,
                              'sommething': {
                               'inside': 10
                                }
                             }", @"{
                                 'number': [
                                    10,
                                     5,
                                     5
                                    ],
                                'sommething': [
                                      20,
                                      {'inside': 10}
                             ]}"
                }
            };
        }
    }
}