
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using Icm.Collections;
using Icm.Localization;
using Icm.Tree;

/// <summary>
/// Implementation of BaseInteractor using <see cref="TextReader">TextReaders</see> for input and
/// <see cref="TextWriter">TextWriters</see> for output and error.
/// </summary>
/// <remarks></remarks>
public class StreamsInteractor : IInteractor
{

	protected TextReader Reader { get; set; }
	protected TextWriter Writer { get; set; }
	protected TextWriter ErrorWriter { get; set; }

	private int _indentLevel;

	private string _indentString = "";

	protected ILocalizationRepository locRepo;
	public string PromptSeparator { get; set; }

	public Queue<string> TokenQueue { get; set; }


	public string CommandPromptSeparator { get; set; }

	public StreamsInteractor(ILocalizationRepository locRepo) : this(locRepo, Console.In, Console.Out, Console.Error, new Queue<string>())
	{
	}

	public StreamsInteractor(ILocalizationRepository locRepo, TextReader inputtr, TextWriter outputtw, TextWriter errortw, Queue<string> tokenQueue) : base()
	{

		Reader = inputtr;
		Writer = outputtw;
		ErrorWriter = errortw;
		this.TokenQueue = tokenQueue;
		this.locRepo = locRepo;
	}


	public void SetLocalizationRepo(ILocalizationRepository locRepo)
	{
		this.locRepo = locRepo;
	}


	public string AskCommand(string prompt)
	{
		return AskStringWithTokenQueue(prompt, null, CommandPromptSeparator);
	}

	private string AskStringWithTokenQueue(string prompt, string defaultValue, string promptSeparator)
	{
		string response = null;
		if (TokenQueue.Count != 0) {
			response = TokenQueue.Dequeue;
		} else {
			response = AskStringWithoutTokenQueue(prompt, defaultValue, promptSeparator);
		}
		return response;
	}

	public string AskString(string prompt, Func<string, bool> validation, string defaultValue)
	{
		string response = null;
		do {
			response = AskStringWithTokenQueue(prompt, defaultValue, PromptSeparator);
			if (string.IsNullOrEmpty(response)) {
				return defaultValue;
			}
			if (validation == null) {
				break; // TODO: might not be correct. Was : Exit Do
			} else if (validation.Invoke(response)) {
				break; // TODO: might not be correct. Was : Exit Do
			}
		} while (true);

		return response;
	}

	public int IndentLevel {
		get { return _indentLevel; }
		set {
			_indentLevel = value;
			_indentString = new string(' ', _indentLevel * 2);
		}
	}

	protected string IndentString {
		get { return _indentString; }
	}


	protected string AskStringWithoutTokenQueue(string prompt, string defaultValue, string promptSeparator)
	{
		if (string.IsNullOrEmpty(defaultValue)) {
			Writer.Write("{0}{1}", prompt, promptSeparator);
		} else {
			Writer.Write("{0} (default: {1}){2}", prompt, defaultValue, promptSeparator);
		}
		return Reader.ReadLine;
	}

	private void ShowListAux(string title, IEnumerable<string> list, bool hideIfEmpty = false)
	{
		if (hideIfEmpty && list.Count == 0) {
			return;
		}
		Writer.WriteLine();
		Writer.WriteLine(title);
		Writer.WriteLine(new string('-', title.Length));
		for (i = 0; i <= list.Count - 1; i++) {
			Writer.WriteLine(IndentString + list(i));
		}
	}

	public void ShowList<T>(string title, IEnumerable<T> values, Func<T, string> toString, bool hideIfEmpty) where T : class
	{
		ShowListAux(title, values.Select(item => toString(item)), hideIfEmpty);
	}

	private void WriteList<T>(IEnumerable<T> values, Func<T, string> key, Func<T, string> toString, List<T> selectedList) where T : class
	{
		if (selectedList == null) {
			foreach (void value_loopVariable in values) {
				value = value_loopVariable;
				Writer.WriteLine(GetListItem(value, key, toString, selected: false));
			}
		} else {
			foreach (void value_loopVariable in values) {
				value = value_loopVariable;
				Writer.WriteLine(GetListItem(value, key, toString, selected: selectedList.Contains(value)));
			}
		}
	}

	public void ShowKeyedList<T>(string title, IEnumerable<T> values, Func<T, string> key, Func<T, string> toString, bool hideIfEmpty) where T : class
	{
		ShowListAux(title, values.Select(item => GetListItem(item, key, toString, selected: false)), hideIfEmpty);
	}

	private string GetListItem<T>(T value, Func<T, string> key, Func<T, string> toString, bool selected) where T : class
	{
		if (selected) {
			return string.Format(">{0}. {1}", key(value), toString(value));
		} else {
			return string.Format(" {0}. {1}", key(value), toString(value));
		}
	}

	public T AskOneByKey<T>(string prompt, IEnumerable<T> values, Func<T, string> key, Func<T, string> toString, T defaultValue, List<T> selectedList) where T : class
	{
		string response = null;
		if (values.Count == 0) {
			throw new ArgumentException("Values collection cannot be empty");
		}
		if (defaultValue != null && !values.Any(obj => key(obj) == key(defaultValue))) {
			throw new ArgumentException(string.Format("The default value (key {0}) cannot be found in the values collection", key(defaultValue)));
		}

		if (TokenQueue.Count != 0) {
			response = TokenQueue.Dequeue;
		} else if (values.Count == 1) {
			return values.Single;
		} else {
			WriteList(values, key, toString, selectedList);
			if (defaultValue == null) {
				response = AskString(prompt);
			} else {
				response = AskString(prompt, key(defaultValue));
			}
		}
		if (response != null) {
			return values.Single(obj => key(obj) == response);
		} else {
			return defaultValue;
		}
	}

	public void ShowNumberedList<T>(string title, IEnumerable<T> values, Func<T, string> toString, bool hideIfEmpty) where T : class
	{
		if (hideIfEmpty && values.Count == 0) {
			return;
		}
		Writer.WriteLine(title);
		Writer.WriteLine(new string('-', title.Length));
		for (i = 1; i <= values.Count; i++) {
			var iFor = i;
			Writer.WriteLine(GetListItem(values(i - 1), obj => iFor.ToString, toString, selected: false));
		}
	}

	public T AskOne<T>(string prompt, IEnumerable<T> values, Func<T, string> toString, T defaultValue, List<T> selectedList) where T : class
	{
		int? defaultIndex = null;
		for (i = 1; i <= values.Count; i++) {
			var iFor = i;
			var value = values(i - 1);
			var key = (T obj) => iFor.ToString;
			WriteList(values, key, toString, selectedList);
			if (object.ReferenceEquals(defaultValue, values(i - 1))) {
				defaultIndex = i;
			}
		}
		var response = AskInteger(prompt, defaultIndex);
		if (response.HasValue) {
			return values(response.Value - 1);
		} else {
			return defaultValue;
		}
	}

	public List<T> AskMany<T>(string generalPrompt, string prompt, IEnumerable<T> values, Func<T, string> toString, List<T> initialList) where T : class
	{
		var result = initialList;
		Writer.WriteLine(generalPrompt);
		do {
			var item = AskOne(prompt, values, toString, null, initialList);
			if (item == null) {
				return result;
			} else if (result.Contains(item)) {
				result.Remove(item);
			} else {
				result.Add(item);
			}
		} while (true);
	}

	private string GetPropertyDescription(System.Reflection.PropertyInfo pi, object obj)
	{
		string result = null;
		string value = null;
		if (pi.GetIndexParameters.Count > 0) {
			value = "INDEXED";
		} else {
			object val = null;
			try {
				val = pi.GetValue(obj, null);
			} catch (System.Reflection.TargetInvocationException ex) {
				value = "ERROR";
			}
			if (val != null) {
				if (val.GetType.Name == "String") {
					value = string.Format("\"{0}\"", val);
				} else if (typeof(IEnumerable).IsAssignableFrom(val.GetType)) {
					var enumerable = (IEnumerable)val;
					List<string> list = new List<string>();
					foreach (void obj_loopVariable in enumerable) {
						obj = obj_loopVariable;
						list.Add("   " + obj.ToString);
					}
					value = Constants.vbCrLf + list.JoinStr(Constants.vbCrLf);
				} else {
					value = val.ToString;
				}
			} else {
				value = "NULL";
			}
		}

		if (pi.PropertyType.IsPrimitive) {
			result = string.Format("{0}: {1} ({2})", pi.Name, value, pi.PropertyType.Name);
		} else {
			result = string.Format("{0}: {1}", pi.Name, value);
		}
		return result;
	}

	public void ShowErrors(IEnumerable<string> list)
	{
		foreach (void errorString_loopVariable in list) {
			errorString = errorString_loopVariable;
			ErrorWriter.WriteLine(errorString);
		}
		ErrorWriter.WriteLine();
	}

	public void ShowMessage(string msg)
	{
		Writer.WriteLine(msg);
	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
