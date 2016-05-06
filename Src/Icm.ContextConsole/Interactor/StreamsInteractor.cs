using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Icm.Collections;
using Icm.Localization;

public static class TextWriterExtensions
{
    public static void WriteLines(this TextWriter writer, IEnumerable<string> lines)
    {
        foreach (var line in lines)
        {
            writer.Write(line);
        }
    }
}

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

    public StreamsInteractor(ILocalizationRepository locRepo)
        : this(locRepo, Console.In, Console.Out, Console.Error, new Queue<string>())
    {
    }

    public StreamsInteractor(ILocalizationRepository locRepo, TextReader inputtr, TextWriter outputtw,
        TextWriter errortw, Queue<string> tokenQueue) : base()
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
        if (TokenQueue.Count != 0)
        {
            response = TokenQueue.Dequeue();
        }
        else
        {
            response = AskStringWithoutTokenQueue(prompt, defaultValue, promptSeparator);
        }
        return response;
    }

    public string AskString(string prompt, Func<string, bool> validation, string defaultValue)
    {
        string response = null;
        do
        {
            response = AskStringWithTokenQueue(prompt, defaultValue, PromptSeparator);
            if (string.IsNullOrEmpty(response))
            {
                return defaultValue;
            }
            if (validation == null)
            {
                break; // TODO: might not be correct. Was : Exit Do
            }
            else if (validation.Invoke(response))
            {
                break; // TODO: might not be correct. Was : Exit Do
            }
        } while (true);

        return response;
    }

    public int IndentLevel
    {
        get { return _indentLevel; }
        set
        {
            _indentLevel = value;
            _indentString = new string(' ', _indentLevel * 2);
        }
    }

    protected string IndentString => _indentString;


    protected string AskStringWithoutTokenQueue(string prompt, string defaultValue, string promptSeparator)
    {
        if (string.IsNullOrEmpty(defaultValue))
        {
            Writer.Write("{0}{1}", prompt, promptSeparator);
        }
        else
        {
            Writer.Write("{0} (default: {1}){2}", prompt, defaultValue, promptSeparator);
        }
        return Reader.ReadLine();
    }

    private void ShowListAux(string title, IEnumerable<string> list, bool hideIfEmpty = false)
    {
        var array = list as string[] ?? list.ToArray();
        if (hideIfEmpty && !array.Any())
        {
            return;
        }
        Writer.WriteLine();
        Writer.WriteLine(title);
        Writer.WriteLine(new string('-', title.Length));
        Writer.WriteLines(array.Select(x => IndentString + x));
    }

    public void ShowList<T>(string title, IEnumerable<T> values, Func<T, string> toString, bool hideIfEmpty)
        where T : class
    {
        ShowListAux(title, values.Select(toString), hideIfEmpty);
    }

    private void WriteList<T>(IEnumerable<T> values, Func<T, string> key, Func<T, string> toString, List<T> selectedList)
        where T : class
    {
        Writer.WriteLines(
            selectedList == null
                ? values.Select(value => GetListItem(value, key, toString, selected: false))
                : values.Select(value => GetListItem(value, key, toString, selected: selectedList.Contains(value))));
    }

    public void ShowKeyedList<T>(string title, IEnumerable<T> values, Func<T, string> key, Func<T, string> toString,
        bool hideIfEmpty) where T : class
    {
        ShowListAux(title, values.Select(item => GetListItem(item, key, toString, selected: false)), hideIfEmpty);
    }

    private string GetListItem<T>(T value, Func<T, string> key, Func<T, string> toString, bool selected) where T : class
    {
        return selected
            ? $">{key(value)}. {toString(value)}"
            : $" {key(value)}. {toString(value)}";
    }

    public T AskOneByKey<T>(string prompt, IEnumerable<T> values, Func<T, string> key, Func<T, string> toString,
        T defaultValue, List<T> selectedList) where T : class
    {
        string response;
        var array = values as T[] ?? values.ToArray();
        if (!array.Any())
        {
            throw new ArgumentException("Values collection cannot be empty");
        }

        if (defaultValue != null && array.All(obj => key(obj) != key(defaultValue)))
        {
            throw new ArgumentException(
                $"The default value (key {key(defaultValue)}) cannot be found in the values collection");
        }

        if (TokenQueue.Count != 0)
        {
            response = TokenQueue.Dequeue();
        }
        else if (array.Length == 1)
        {
            return array.Single();
        }

        else
        {
            WriteList(array, key, toString, selectedList);
            response = defaultValue == null
                ? this.AskString(prompt)
                : this.AskString(prompt, key(defaultValue));
        }

        return response != null
            ? array.Single(obj => key(obj) == response)
            : defaultValue;
    }

    public void ShowNumberedList<T>(string title, IEnumerable<T> values, Func<T, string> toString, bool hideIfEmpty)
        where T : class
    {
        var array = values as T[] ?? values.ToArray();
        if (hideIfEmpty && !array.Any())
        {
            return;
        }

        Writer.WriteLine(title);
        Writer.WriteLine(new string('-', title.Length));
        Writer.WriteLines(
            array.Select((item, i) => GetListItem(item, obj => (i + 1).ToString(), toString, selected: false)));
    }

    public T AskOne<T>(string prompt, IEnumerable<T> values, Func<T, string> toString, T defaultValue,
        List<T> selectedList) where T : class
    {
        int? defaultIndex = null;
        var array = values as T[] ?? values.ToArray();

        foreach (var result in array.Select((item, i) => new
        {
            Number = i + 1,
            Value = item,
            Index = i
        }))
        {
            Func<T, string> key = obj => result.Number.ToString();
            WriteList(array, key, toString, selectedList);
            if (ReferenceEquals(defaultValue, result.Value))
            {
                defaultIndex = result.Index;
            }
        }

        var response = this.AskInteger(prompt, defaultIndex);
        return response.HasValue
            ? array[response.Value - 1]
            : defaultValue;
    }

    public List<T> AskMany<T>(string generalPrompt, string prompt, IEnumerable<T> values, Func<T, string> toString,
        List<T> initialList) where T : class
    {
        var result = initialList;
        Writer.WriteLine(generalPrompt);
        do
        {
            var item = AskOne(prompt, values, toString, null, initialList);
            if (item == null)
            {
                return result;
            }

            if (result.Contains(item))
            {
                result.Remove(item);
            }
            else
            {
                result.Add(item);
            }
        } while (true);
    }

    public string GetPropertyDescription(System.Reflection.PropertyInfo pi, object obj)
    {
        string value;
        if (pi.GetIndexParameters().Any())
        {
            value = "INDEXED";
        }
        else
        {
            object val = null;
            try
            {
                val = pi.GetValue(obj, null);
            }
            catch (System.Reflection.TargetInvocationException)
            {
                value = "ERROR";
            }

            if (val != null)
            {
                if (val.GetType().Name == "String")
                {
                    value = $"\"{val}\"";
                }
                else if (val is IEnumerable)
                {
                    var enumerable = (IEnumerable) val;
                    List<string> list = enumerable.Cast<object>().Select(obj2 => "   " + obj2).ToList();
                    value = "\r\n" + list.JoinStr("\r\n");
                }
                else
                {
                    value = val.ToString();
                }
            }
            else
            {
                value = "NULL";
            }
        }

        var result = pi.PropertyType.IsPrimitive 
            ? $"{pi.Name}: {value} ({pi.PropertyType.Name})"
            : $"{pi.Name}: {value}";
        return result;
    }

    public void ShowErrors(IEnumerable<string> list)
    {
        foreach (var errorString in list)
        {
            ErrorWriter.WriteLine(errorString);
        }

        ErrorWriter.WriteLine();
    }

    public void ShowMessage(string msg)
    {
        Writer.WriteLine(msg);
    }
}