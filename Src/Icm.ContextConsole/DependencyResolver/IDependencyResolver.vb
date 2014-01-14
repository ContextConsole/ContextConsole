''' <summary>
''' Dependency resolvers
''' </summary>
''' <remarks></remarks>
Public Interface IDependencyResolver

    ''' <summary>
    ''' Resolves a type by giving an instance of that type.
    ''' </summary>
    ''' <param name="service"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetService(service As Type) As Object

    Sub ClearRequestScope()

End Interface
