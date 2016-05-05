
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Icm.ContextConsole;

public class StringContext : BaseContext
{

	public void Concat()
	{
		dynamic num1 = Interactor.AskString("String 1");
		dynamic num2 = Interactor.AskString("String 2");

		Interactor.ShowMessage(string.Format("{0} + {1} = {2}", num1, num2, num1 + num2));
	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
