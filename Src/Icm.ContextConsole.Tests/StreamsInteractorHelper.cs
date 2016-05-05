
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using NUnit.Framework;
using Icm.ContextConsole;
using System.IO;
using Icm.IO;

public static class StreamsInteractorHelper
{

	private static string GetInputstring(IEnumerable<string> input)
	{
		string inputstring = null;
		using (StringWriter inputgen = new StringWriter()) {
			if (input != null) {
				foreach (void line_loopVariable in input) {
					line = line_loopVariable;
					inputgen.WriteLine(line);
				}
			}
			inputstring = inputgen.ToString;
		}
		return inputstring;
	}

	public static StreamsInteractor BuildInteractor(IEnumerable<string> input)
	{
		return new StreamsInteractor(new Icm.Localization.DictionaryLocalizationRepository(), TextReaderFactory.FromString(GetInputstring(input)), new StringWriter(), new StringWriter(), new Queue<string>());
	}

	public static StreamsInteractor BuildInteractor(IEnumerable<string> input, StringWriter output, StringWriter errorstr)
	{
		return new StreamsInteractor(new Icm.Localization.DictionaryLocalizationRepository(), TextReaderFactory.FromString(GetInputstring(input)), output, errorstr, new Queue<string>());
	}

	public static StreamsInteractor BuildInteractor(IEnumerable<string> input, StringWriter output, StringWriter errorstr, Queue<string> tokenQueue)
	{
		return new StreamsInteractor(new Icm.Localization.DictionaryLocalizationRepository(), TextReaderFactory.FromString(GetInputstring(input)), output, errorstr, tokenQueue);
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
