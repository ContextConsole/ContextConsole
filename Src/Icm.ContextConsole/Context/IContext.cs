
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
/// <summary>
/// A context provides access to its actions for them to be executed. It has a name and optional synonyms.
/// </summary>
/// <remarks></remarks>
public interface IContext : INamedWithSynonyms
{

	void Initialize(IApplication app);

	IActionFinder ActionFinder { get; }
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
