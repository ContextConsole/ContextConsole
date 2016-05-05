
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;

/// <summary>
/// Finds actions by using reflection on the context.
/// </summary>
/// <remarks>It finds all public non-static parameterless procedures.</remarks>
public class ReflectionActionFinder : IActionFinder
{

	public IEnumerable<IAction> GetActions(IContext ctl)
	{
		var methods = ctl.GetType().GetMethods().Where(minf => minf.ReturnType.Name == "Void" && minf.GetParameters().Count == 0 && minf.IsPublic && !minf.IsStatic);
        return methods.Select(minf => new MethodInfoAction(minf, ctl));
    }
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
