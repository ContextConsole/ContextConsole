using System;
using System.Linq;
using System.Runtime.CompilerServices;

public static class TypeExtensions
{
	[Extension()]
	public static T[] GetAttributes<T>(Type type, bool inherit) where T : Attribute
	{
		return (T[])type.GetCustomAttributes(typeof(T), inherit);
	}
	[Extension()]
	public static bool HasAttribute<T>(Type type, bool inherit) where T : Attribute
	{
		return ((T[])type.GetCustomAttributes(typeof(T), inherit)).Count() > 0;
	}
	[Extension()]
	public static T GetAttribute<T>(Type type, bool inherit) where T : Attribute
	{
		return ((T[])type.GetCustomAttributes(typeof(T), inherit)).SingleOrDefault();
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
