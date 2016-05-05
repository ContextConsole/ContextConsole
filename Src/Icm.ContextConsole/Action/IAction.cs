
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
/// <summary>
/// An action is a piece of executable code, with a name and optional synonyms.
/// It also provides a key for localizing the description of the action and a flag that
/// indicates if the action must be localized using Icm.ContextConsole.dll internal
/// resources or using external resources (provided by the client assemblies).
/// </summary>
/// <remarks></remarks>
public interface IAction : INamedWithSynonyms
{

	bool IsInternal();
	string LocalizationKey();

	void Execute();
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
