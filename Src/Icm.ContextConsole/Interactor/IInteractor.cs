
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Icm.Localization;

/// <summary>
/// An interactor asks values and shows results, abstracting the data source and the target of the operations.
/// </summary>
/// <remarks></remarks>
public interface IInteractor
{


	void SetLocalizationRepo(ILocalizationRepository locRepo);
	string PromptSeparator { get; set; }
	Queue<string> TokenQueue { get; set; }

	string CommandPromptSeparator { get; set; }
	string GetPropertyDescription(System.Reflection.PropertyInfo pi, object obj);

	string AskString(string prompt, Func<string, bool> validation, string defaultValue);
	T AskOne<T>(string prompt, IEnumerable<T> values, Func<T, string> toString, T defaultValue, List<T> selectedList) where T : class;
	T AskOneByKey<T>(string prompt, IEnumerable<T> values, Func<T, string> key, Func<T, string> toString, T defaultValue, List<T> selectedList) where T : class;
	List<T> AskMany<T>(string generalPrompt, string prompt, IEnumerable<T> values, Func<T, string> toString, List<T> initialList) where T : class;
	string AskCommand(string prompt);

	void ShowList<T>(string title, IEnumerable<T> values, Func<T, string> toString, bool hideIfEmpty) where T : class;
	void ShowNumberedList<T>(string title, IEnumerable<T> values, Func<T, string> toString, bool hideIfEmpty) where T : class;
	void ShowKeyedList<T>(string title, IEnumerable<T> values, Func<T, string> key, Func<T, string> toString, bool hideIfEmpty) where T : class;
	void ShowErrors(IEnumerable<string> list);
	void ShowMessage(string msg);
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
