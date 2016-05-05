
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using Icm.Reflection;
using System.Reflection;

public class MethodInfoAction : IAction
{

	private readonly MethodInfo _minfo;
	private List<string> _synonyms;
	private readonly IContext _ctl;
	private readonly string _locKey;

	private readonly bool _isInternal;
	public MethodInfoAction(MethodInfo minf, IContext ctl)
	{
		_minfo = minf;
		_ctl = ctl;
		_isInternal = object.ReferenceEquals(minf.DeclaringType.Assembly, this.GetType().Assembly);
		var declaringTypeName = minf.DeclaringType.Name;
		_locKey = string.Format("{0}_{1}", declaringTypeName.Substring(0, declaringTypeName.Length - "Context".Length).ToLower(), minf.Name.ToLower());
	}


	public string Name()
	{
		return _minfo.Name.ToLower();
	}

	public System.Collections.Generic.IEnumerable<string> Synonyms()
	{
		if (_synonyms == null) {
			_synonyms = new List<string>();
			var synAttrs = _minfo.GetAttributes<SynonymAttribute>(true);
			foreach (var synAttr in synAttrs) {
				_synonyms.AddRange(synAttr.Synonyms.Select(str => str.ToLower()));
			}

		}
		return _synonyms;
	}

	public void Execute()
	{
		var parameters = _minfo.GetParameters();

		if (!parameters.Any()) {
			_minfo.Invoke(_ctl, null);
			return;
		}

		object[] arguments = null;
		arguments = new object[parameters.Count()];

		for (var i = 0; i <= parameters.Count() - 1; i++) {
			arguments[i] = AskValue(parameters[i]);
		}

		_minfo.Invoke(_ctl, arguments);
	}

	private static object AskValue(ParameterInfo parinfo)
	{
		// TODO
		throw new NotImplementedException("");
		//If parinfo.ParameterType Is GetType(String) Then

		//ElseIf parinfo.ParameterType.IsGenericType AndAlso parinfo.ParameterType.GetGenericTypeDefinition().Equals(GetType(Lazy(Of ))) Then

		//ElseIf parinfo.ParameterType Is GetType(String) Then
		//ElseIf parinfo.ParameterType Is GetType(String) Then
		//ElseIf parinfo.ParameterType Is GetType(String) Then
		//ElseIf parinfo.ParameterType Is GetType(String) Then
		//ElseIf parinfo.ParameterType Is GetType(String) Then
		//ElseIf parinfo.ParameterType Is GetType(String) Then
		//Else
		//    Throw New ArgumentException("Cannot ask the value of a parameter of type " & parinfo.ParameterType.FullName, "parinfo")
		//End If
	}

	public string LocalizationKey()
	{
		return _locKey;
	}

	public bool IsInternal()
	{
		return _isInternal;
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
