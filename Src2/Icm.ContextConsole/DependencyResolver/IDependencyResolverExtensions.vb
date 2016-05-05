Imports System.Runtime.CompilerServices

Public Module IDependencyResolverExtensions

    <Extension>
    Function GetService(Of T)(resolver As IDependencyResolver) As T
        Return DirectCast(resolver.GetService(GetType(T)), T)
    End Function

    <Extension>
    Function GetService(Of T)(resolver As IDependencyResolver, service As Type) As T
        Return DirectCast(resolver.GetService(service), T)
    End Function

End Module
