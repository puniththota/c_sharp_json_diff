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
        public void Given_TwoJsonStrings_When_NumericDiff_Then_ExpectedNumberOfChildrenReturned(string left, string right, string expected)
        {
            JsonDiff jsonDiff = new JsonDiff();
            JObject actualNumericDiff = jsonDiff.NumericDiff(left, right);
            JObject expectedNumericDiff = JObject.Parse(expected);

            Assert.AreEqual(expectedNumericDiff.Count, actualNumericDiff.Count);
        }

        [Test, Category("Unit")]
        [TestCaseSource("ProviderForNumericDiffTest")]
        public void Given_TwoJsonStrings_When_NumericDiff_Then_ExpectedNumberOfPropertiesReturned(string left, string right, string expected)
        {
            JsonDiff jsonDiff = new JsonDiff();
            JObject actualNumericDiff = jsonDiff.NumericDiff(left, right);
            JObject expectedNumericDiff = JObject.Parse(expected);

            IEnumerable<JProperty> actualProperties = actualNumericDiff.Properties();
            IEnumerable<JProperty> expectedProperties = expectedNumericDiff.Properties();

            Assert.AreEqual(expectedProperties, actualProperties);

        }

        [Test, Category("Unit")]
        [TestCaseSource("ProviderForNumericDiffTest")]
        public void Given_TwoJsonStrings_When_NumericDiff_Then_ExpectedValueReturned(string left, string right, string expected)
        {
            JsonDiff jsonDiff = new JsonDiff();
            JObject actualNumericDiff = jsonDiff.NumericDiff(left, right);
            JObject expectedNumericDiff = JObject.Parse(expected);

            Assert.IsTrue(JObject.DeepEquals(expectedNumericDiff, actualNumericDiff));

        }

        public static IEnumerable<object[]> ProviderForNumericDiffTest()
        {
            return new List<object[]>()
            {
                new object[] { @"{
                              'a': [
                                    { 'b': 15 },
                                    { 'c': 25 },
                                   ]
                                  }",
                               @"{
                               'a': [
                                     { 'b': 10 },
                                     { 'c': 20 },
                                    ]
                                 }", 
                               @"{
                               'a': {
                                    '_t': 'a',
                                     '0': {
                                           'b': {
                                                   'left': 15,
                                                   'right': 10,
                                                    'diff': 5
                                                }
                                                },
                                    '1': {
                                          'c': {
                                                   'left': 25,
                                                   'right': 20,
                                                    'diff': 5
                                               }
                                           }
                                        }
                                       }" },
                new object[]
                { @"{
                              'number': 10,
                              'something': {
                               'inside': 20
                                }
                             }", @"{
                              'number': 5,
                              'something': {
                               'inside': 10
                                }
                             }", @"{
                                 'number': {
                                   'left': 10,
                                   'right': 5,
                                   'diff': 5
                                  },
                                 'something': {
                                    'inside': 
                                       {
                                         'left': 20,
                                         'right': 10,
                                         'diff': 10
                                        }   
                                       }
                                      }"},
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
                              'number': 102679.894,
                              'something': 20
                             }", @"{
                              'number': 102680,
                              'something': {
                               'inside': 10
                                }
                             }", @"{
                                 'number': {
                                   'left': 102679.894,
                                    'right': 102680.0,
                                     'diff': 0.10599999999976717
                                    },
                                'something': [ 
                                      20,
                                      {'inside': 10}
                             ]}"
                }
            };
        }
    }
}