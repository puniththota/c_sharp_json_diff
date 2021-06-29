using c_sharp_json_diff;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

namespace Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            IDictionary<string, double> first = new Dictionary<string, double>();
            first.Add("first", 1);
            IDictionary<string, double> second = new Dictionary<string, double>();
            second.Add("first", 2);
            Result expected = new Result();
            Diff expectedDiff = new Diff();
            expectedDiff.previous = 1;
            expectedDiff.current = 2;
            expectedDiff.difference = 1;
            expected.Diffs.Add(expectedDiff);
            Result actual = JsonDiff.GetDiff(first, second);
            Assert.AreEqual(expected.Diffs.Count, actual.Diffs.Count);
            Assert.AreEqual(expected.Diffs[0].previous, actual.Diffs[0].previous);
            Assert.AreEqual(expected.Diffs[0].current, actual.Diffs[0].current);
            Assert.AreEqual(expected.Diffs[0].difference, actual.Diffs[0].difference);
        }
    }
}