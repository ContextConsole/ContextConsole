
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using NUnit.Framework;
using Icm.ContextConsole;
using Icm.Localization;
using Moq;

[TestFixture()]
public class FrontContextTest
{

	[Test()]
	public void ConstructorTest()
	{
		Assert.That(() =>
		{
			IContext fctl = new TestFrontContext();
		}, Throws.Nothing);
	}

	[TestCase("asdf", "executecommand_actionnotfound", false)]
	[TestCase("help", null, false)]
	[TestCase("quit", null, true)]
	public void ExecuteCommandTest(string input, string errorKey, bool expectedMustQuit)
	{
		dynamic tw = new System.IO.StringWriter();
		dynamic twerr = new System.IO.StringWriter();
		dynamic inter = BuildInteractor({ input }, tw, twerr);
		bool mustQuit = false;
		Assert.That(() =>
		{
			IApplication fctl = StandardApplication.Create<TestFrontContext>(interactor: inter, intLocRepo: new DictionaryLocalizationRepository());
			mustQuit = fctl.ExecuteCommand();
		}, Throws.Nothing);

		Assert.That(mustQuit, Is.EqualTo(expectedMustQuit));
		if (errorKey != null) {
			Assert.That(twerr.ToString, Is.StringContaining(errorKey));
		}
	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
