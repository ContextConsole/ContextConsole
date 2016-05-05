
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Runtime.CompilerServices;

public static class IDependencyResolverExtensions
{

	[Extension()]
	public static T GetService<T>(IDependencyResolver resolver)
	{
		return (T)resolver.GetService(typeof(T));
	}

	[Extension()]
	public static T GetService<T>(IDependencyResolver resolver, Type service)
	{
		return (T)resolver.GetService(service);
	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
