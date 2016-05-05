
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
/// <summary>
/// This attribute provides synonyms for the given item.
/// </summary>
/// <remarks></remarks>
public class SynonymAttribute : Attribute
{


	public string[] Synonyms;
	public SynonymAttribute(params string[] synonyms)
	{
		this.Synonyms = synonyms;
	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
