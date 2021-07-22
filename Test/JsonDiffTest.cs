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
            JObject actualNumericDiff = jsonDiff.NumericDoubleDiff(left, right);
            JObject expectedNumericDiff = JObject.Parse(expected);

            Assert.AreEqual(expectedNumericDiff.Count, actualNumericDiff.Count);
        }

        [Test, Category("Unit")]
        [TestCaseSource("ProviderForNumericDiffTest")]
        public void Given_TwoJsonStrings_When_NumericDiff_Then_ExpectedNumberOfPropertiesReturned(string left, string right, string expected)
        {
            JsonDiff jsonDiff = new JsonDiff();
            JObject actualNumericDiff = jsonDiff.NumericDoubleDiff(left, right);
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
            JObject actualNumericDiff = jsonDiff.NumericDoubleDiff(left, right);
            JObject expectedNumericDiff = JObject.Parse(expected);

            Assert.IsTrue(JObject.DeepEquals(expectedNumericDiff, actualNumericDiff));

        }

        public static IEnumerable<object[]> ProviderForNumericDiffTest()
        {
            return new List<object[]>()
            {
                new object[] { @"{
                              'x': [
                                    { 'b': 15 },
                                    { 'c': 20 },
                                   ]
                                  }",
                               @"{
                               'x': [
                                     { 'b': 10 },
                                     { 'c': 25 },
                                    ]
                                 }",
                               @"{
                               'x': {
                                    '_t': 'a',
                                     '0': {
                                           'b': {
                                                   'Left': 15,
                                                   'Right': 10,
                                                    'Double Diff': {
                                                      'Subtraction': -5.0,
                                                      'Division': 0.66666666666666663
                                                      }
                                                }
                                                },
                                    '1': {
                                          'c': {
                                                   'Left': 20,
                                                   'Right': 25,
                                                    'Double Diff': {
                                                      'Subtraction': 5.0,
                                                      'Division': 1.25
                                                      }
                                               }
                                           }
                                        }
                                       }" },
                new object[]
                { @"{
                              'number': 5,
                              'something': {
                               'inside': 20
                                }
                             }", @"{
                              'number': 10,
                              'something': {
                               'inside': 10
                                }
                             }", @"{
                                 'number': {
                                   'Left': 5,
                                   'Right': 10,
                                   'Double Diff': {
                                                      'Subtraction': 5.0,
                                                      'Division': 2.0
                                                      }
                                  },
                                 'something': {
                                    'inside': 
                                       {
                                         'Left': 20,
                                         'Right': 10,
                                         'Double Diff': {
                                                      'Subtraction': -10.0,
                                                      'Division': 0.5
                                                      }
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
                                   'Left': 102679.894,
                                    'Right': 102680,
                                     'Double Diff': {
                                                      'Subtraction': 0.10599999999976717,
                                                      'Division': 1.000001032334529
                                                      }
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