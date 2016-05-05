
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Icm.ContextConsole;

public class MainContext : RootContext
{

	public void Test()
	{
		dynamic tup2 = Interactor.AskOneByKey("Choose", {
			Tuple.Create(1, "a"),
			Tuple.Create(2, "b")
		}, tup => tup.Item1.ToString);
		Interactor.ShowMessage("Chosen: " + tup2.Item2);
	}

	public override void Credits()
	{
		Interactor.ShowMessage("Icm.ContextConsole Example");
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
