using System;

/// <summary>
/// Dependency resolvers
/// </summary>
/// <remarks></remarks>
public interface IDependencyResolver
{

	/// <summary>
	/// Resolves a type by giving an instance of that type.
	/// </summary>
	/// <param name="service"></param>
	/// <returns></returns>
	/// <remarks></remarks>
	object GetService(Type service);


	void ClearRequestScope();
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
