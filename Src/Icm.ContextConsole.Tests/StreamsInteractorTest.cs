
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using NUnit.Framework;

[TestFixture()]
public class StreamsInteractorTest
{

	public static IEnumerable GetAskStringTestCases()
	{
		Func<string, bool> valFunc = (string str) => str == "qwer";
		return {
			new TestCaseData(null, null, null).Returns(null).SetName("Empty list, no validation"),
			new TestCaseData(null, { "2" }, null).Returns("2"),
			new TestCaseData({ "2" }, null, null).Returns("2"),
			new TestCaseData({ "2" }, null, valFunc).Returns(null),
			new TestCaseData({ "qwer" }, null, valFunc).Returns("qwer")
		};
	}

	[Test()]
	[Timeout(2000)]
	[TestCaseSource("GetAskStringTestCases")]
	public string AskStringTest(string[] tokenQueue, string[] input, Func<string, bool> valFunc)
	{
		dynamic inter = BuildInteractor(input);
		if (tokenQueue != null) {
			Queue<string> queue = new Queue<string>(tokenQueue);
			inter.TokenQueue = queue;
		}

		dynamic result = inter.AskString("", valFunc);
		return result;
	}

	public static IEnumerable GetAskOneByKeyTestCases()
	{
		List<Tuple<int, string>> empty = new List<Tuple<int, string>>();
		dynamic list = {
			Tuple.Create(1, "a"),
			Tuple.Create(2, "b"),
			Tuple.Create(3, "c")
		};
		dynamic defaultValue = Tuple.Create(1, "a");
		return {
			new TestCaseData(null, null, empty, null).Throws(typeof(ArgumentException)).SetName("Empty list, no default value"),
			new TestCaseData(null, null, list, null).Returns(null),
			new TestCaseData(null, { "2" }, list, null).Returns(Tuple.Create(2, "b")),
			new TestCaseData({ "2" }, null, list, null).Returns(Tuple.Create(2, "b")),
			new TestCaseData(null, { "4" }, list, null).Throws(typeof(InvalidOperationException)),
			new TestCaseData({ "4" }, null, list, null).Throws(typeof(InvalidOperationException)),
			new TestCaseData(null, null, empty, defaultValue).Throws(typeof(ArgumentException)).SetName("Empty list, with default value"),
			new TestCaseData(null, null, list, defaultValue).Returns(defaultValue),
			new TestCaseData(null, { "2" }, list, defaultValue).Returns(Tuple.Create(2, "b")),
			new TestCaseData({ "2" }, null, list, defaultValue).Returns(Tuple.Create(2, "b")),
			new TestCaseData(null, { "4" }, list, defaultValue).Throws(typeof(InvalidOperationException)),
			new TestCaseData({ "4" }, null, list, defaultValue).Throws(typeof(InvalidOperationException))
		};
	}

	[Test()]
	[Timeout(2000)]
	[TestCaseSource("GetAskOneByKeyTestCases")]
	public Tuple<int, string> AskOneByKeyTest(string[] tokenQueue, string[] input, IEnumerable<Tuple<int, string>> values, Tuple<int, string> defaultValue)
	{
		dynamic inter = BuildInteractor(input);
		if (tokenQueue != null) {
			Queue<string> queue = new Queue<string>(tokenQueue);
			inter.TokenQueue = queue;
		}
		dynamic result = inter.AskOneByKey("", values, tup => tup.Item1.ToString, toString: obj => obj.ToString, defaultValue: defaultValue, selectedList: new List<Tuple<int, string>>());
		return result;
	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
