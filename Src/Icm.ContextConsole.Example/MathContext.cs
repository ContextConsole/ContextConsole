
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Icm.ContextConsole;

public class MathContext : BaseContext
{

	public string PostFix { get; set; }

	public override string Name()
	{
		return base.Name() + PostFix;
	}

	public void Add()
	{
		dynamic num1 = Interactor.AskInteger("Number 1");
		dynamic num2 = Interactor.AskInteger("Number 2");

		Interactor.ShowMessage(string.Format("{0} + {1} = {2}", num1, num2, num1 + num2));
	}

	public void Multiply()
	{
		dynamic num1 = Interactor.AskInteger("Number 1");
		dynamic num2 = Interactor.AskInteger("Number 2");

		Interactor.ShowMessage(string.Format("{0} * {1} = {2}", num1, num2, num1 * num2));
	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
