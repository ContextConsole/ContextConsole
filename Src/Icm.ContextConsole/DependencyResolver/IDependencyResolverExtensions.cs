using System;

public static class IDependencyResolverExtensions
{
	public static T GetService<T>(this IDependencyResolver resolver)
	{
		return (T)resolver.GetService(typeof(T));
	}
    
	public static T GetService<T>(this IDependencyResolver resolver, Type service)
	{
		return (T)resolver.GetService(service);
	}

}
