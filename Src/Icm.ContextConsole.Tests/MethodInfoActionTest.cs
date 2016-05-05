
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using NUnit.Framework;
using Icm.ContextConsole;
using Moq;

[TestFixture()]
public class MethodInfoActionTest
{

	private class TestContext : IContext
	{

		public string Name()
		{
			throw new NotImplementedException();
		}

		public IEnumerable<string> Synonyms()
		{
			throw new NotImplementedException();
		}


		public string Value = "";
		[Synonym("OtherName", "Another")]
		public void TestAction()
		{
			Value = "OK";
		}


		public void Initialize(IApplication app)
		{
		}

		public IActionFinder ActionFinder {
			get {
				throw new NotImplementedException();
			}
		}
	}

	[Test()]
	public void NameTest()
	{
		TestContext ctl = new TestContext();

		dynamic mi = typeof(TestContext).GetMethod("TestAction");
		MethodInfoAction action = new MethodInfoAction(mi, ctl);

		Assert.That(action.Name, Is.EqualTo("testaction"));
	}

	[Test()]
	public void SynonymsTest()
	{
		TestContext ctl = new TestContext();

		dynamic mi = typeof(TestContext).GetMethod("TestAction");
		MethodInfoAction action = new MethodInfoAction(mi, ctl);

		Assert.That(action.Synonyms, Is.EquivalentTo({
			"othername",
			"another"
		}));
	}

	[Test()]
	public void NamedTest()
	{
		TestContext ctl = new TestContext();

		dynamic mi = typeof(TestContext).GetMethod("TestAction");
		MethodInfoAction action = new MethodInfoAction(mi, ctl);

		Assert.That(action.IsNamed("TestAction"));
		Assert.That(action.IsNamed("OtherName"));
		Assert.That(action.IsNamed("Another"));
	}

	[Test()]
	public void ValueTest()
	{
		TestContext ctl = new TestContext();

		dynamic mi = typeof(TestContext).GetMethod("TestAction");
		MethodInfoAction action = new MethodInfoAction(mi, ctl);

		action.Execute();

		Assert.That(ctl.Value, Is.EqualTo("OK"));
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
