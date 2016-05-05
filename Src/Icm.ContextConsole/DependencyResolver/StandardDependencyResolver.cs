
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
public class StandardDependencyResolver : IDependencyResolver
{



	private Dictionary<Type, object> store = new Dictionary<Type, object>();
	public virtual object GetService(Type service)
	{
		object result = null;
		if (!store.TryGetValue(service, result)) {
			result = CreateService(service);
			store.Add(service, result);
		}
		return result;
	}

	protected virtual object CreateService(Type service)
	{
		return Activator.CreateInstance(service);
	}

	public void ClearRequestScope()
	{
		store.Clear();
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
