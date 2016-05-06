using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class IInteractorExtensions
{
    public static string AskString(this IInteractor interactor, string prompt)
    {
        return interactor.AskString(prompt, null, null);
    }

    public static string AskString(this IInteractor interactor, string prompt, string defaultValue)
    {
        return interactor.AskString(prompt, null, defaultValue);
    }

    public static string AskString(this IInteractor interactor, string prompt, Func<string, bool> validation)
    {
        return interactor.AskString(prompt, validation, null);
    }

    public static int? AskInteger(this IInteractor interactor, string prompt)
    {
        return interactor.AskInteger(prompt, null);
    }

    public static T AskOne<T>(this IInteractor interactor, string prompt, IEnumerable<T> values) where T: class
    {
        return interactor.AskOne(prompt, values, obj => obj.ToString());
    }

    public static T AskOne<T>(this IInteractor interactor, string prompt, IEnumerable<T> values, Func<T, string> toString) where T : class
    {
        return interactor.AskOne(prompt, values, toString, null, new List<T>());
    }

    public static T AskOneByKey<T>(this IInteractor interactor, string prompt, IEnumerable<T> values, Func<T, string> key) where T : class
    {
        return interactor.AskOneByKey(prompt, values, key, obj => obj.ToString());
    }

    public static T AskOneByKey<T>(this IInteractor interactor, string prompt, IEnumerable<T> values, Func<T, string> key, Func<T, string> toString) where T : class
    {
        return interactor.AskOneByKey(prompt, values, key, toString, null, new List<T>());
    }

    public static List<T> AskMany<T>(this IInteractor interactor, string generalPrompt, string prompt, IEnumerable<T> values) where T : class
    {
        return interactor.AskMany(generalPrompt, prompt, values, obj => obj.ToString(), new List<T>());
    }

    public static List<T> AskMany<T>(this IInteractor interactor, string generalPrompt, string prompt, IEnumerable<T> values, List<T> initialList) where T : class
    {
        return interactor.AskMany(generalPrompt, prompt, values, obj => obj.ToString(), initialList);
    }

    public static List<T> AskMany<T>(this IInteractor interactor, string generalPrompt, string prompt, IEnumerable<T> values, Func<T, string> toString) where T : class
    {
        return interactor.AskMany(generalPrompt, prompt, values, toString, new List<T>());
    }

    public static void ShowProperties<T>(this IInteractor interactor, string title, T obj) where T : class
    {
        interactor.ShowList(title, obj.GetType().GetProperties(), Icm.CurryExtensions.Curry2<PropertyInfo, object, string>(interactor.GetPropertyDescription, obj), false);
    }

    public static void ShowList<T>(this IInteractor interactor, string title, IEnumerable<T> values) where T : class
    {
        interactor.ShowList(title, values, obj => obj.ToString(), false);
    }

    public static void ShowList<T>(this IInteractor interactor, string title, IEnumerable<T> values, bool hideIfEmpty) where T : class
    {
        interactor.ShowList(title, values, obj => obj.ToString(), hideIfEmpty);
    }

    public static void ShowList<T>(this IInteractor interactor, string title, IEnumerable<T> values, Func<T, string> toString) where T : class
    {
        interactor.ShowList(title, values, toString, false);
    }

    public static void ShowNumberedList<T>(this IInteractor interactor, string title, IEnumerable<T> values, Func<T, string> toString) where T : class
    {
        interactor.ShowNumberedList(title, values, toString, false);
    }

    public static void ShowKeyedList<T>(this IInteractor interactor, string title, IEnumerable<T> values, Func<T, string> key, Func<T, string> toString) where T : class
    {
        interactor.ShowKeyedList(title, values, key, toString, false);
    }

    public static void ShowNumberedList<T>(this IInteractor interactor, string title, IEnumerable<T> values, bool hideIfEmpty = false) where T : class
    {
        interactor.ShowNumberedList(title, values, obj => obj.ToString(), hideIfEmpty);
        // Warning!!! Optional parameters not supported
    }

    public static void ShowKeyedList<T>(this IInteractor interactor, string title, IEnumerable<T> values, Func<T, string> key, bool hideIfEmpty = false) where T: class
    {
        interactor.ShowKeyedList(title, values, key, obj => obj.ToString(), hideIfEmpty);
        // Warning!!! Optional parameters not supported
    }
    
    public static bool? AskBoolean(this IInteractor interactor, string prompt, bool? defaultValue)
    {
        return interactor.AskLiteral(prompt, bool.TryParse, defaultValue);
    }

    public static IContext AskContext(this IInteractor interactor, string prompt, IContext caller, IEnumerable<IContext> contexts)
    {
        var contextName = interactor.AskString(prompt);
        if (string.IsNullOrWhiteSpace(contextName))
        {
            return caller;
        }

        var ctl = contexts.SingleOrDefault(ctrl => ctrl.Name() == contextName || ctrl.Synonyms().Contains(contextName));

        if (ctl != null)
        {
            return ctl;
        }

        interactor.ShowErrors(new [] {$"Could not find context {contextName}"});
        return null;
    }
    
    public static int? AskInteger(this IInteractor interactor, string prompt, int? defaultValue)
    {
        return interactor.AskLiteral(prompt, int.TryParse, defaultValue);
    }

    public delegate bool TryParseDelegate<T>(string input, out T output);


    public static DateTime? AskDate(this IInteractor interactor, string prompt, DateTime? defaultValue)
    {
        return interactor.AskLiteral(prompt, DateTime.TryParse, defaultValue);
    }

    public static T? AskLiteral<T>(this IInteractor interactor, string prompt, TryParseDelegate<T> tryParse, T? defaultValue) where T: struct
    {
        T result = default(T);
        var response = interactor.AskString(prompt, obj => tryParse(obj, out result), Equals(defaultValue, default(T)) ? defaultValue.ToString() : "");
        return string.IsNullOrEmpty(response) 
            ? defaultValue 
            : result;
    }
}