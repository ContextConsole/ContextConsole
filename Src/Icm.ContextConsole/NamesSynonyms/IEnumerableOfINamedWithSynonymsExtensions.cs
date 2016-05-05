
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

public static class IEnumerableOfINamedWithSynonymsExtensions
{

	/// <summary>
	/// Gets an item by name, taking into account its synonyms.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="list"></param>
	/// <param name="name"></param>
	/// <returns></returns>
	/// <remarks></remarks>
	public static T GetNamedItem<T>(this IEnumerable<T> list, string name) where T : INamedWithSynonyms
	{
		return list.SingleOrDefault(ctrl => ctrl.Name() == name || ctrl.Synonyms().Contains(name));
	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
