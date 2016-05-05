
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

public static class INamedWithSynonymsExtensions
{
	public static bool IsNamed(this INamedWithSynonyms obj, string name)
	{
		var lowname = name.ToLower();
		return obj.Name() == lowname || obj.Synonyms().Contains(lowname);
	}

}
