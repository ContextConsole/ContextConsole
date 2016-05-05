Public Class StandardDependencyResolver
    Implements IDependencyResolver


    Private store As New Dictionary(Of Type, Object)

    Public Overridable Function GetService(service As Type) As Object Implements IDependencyResolver.GetService
        Dim result As Object = Nothing
        If Not store.TryGetValue(service, result) Then
            result = CreateService(service)
            store.Add(service, result)
        End If
        Return result
    End Function

    Protected Overridable Function CreateService(service As Type) As Object
        Return Activator.CreateInstance(service)
    End Function

    Public Sub ClearRequestScope() Implements IDependencyResolver.ClearRequestScope
        store.Clear()
    End Sub
End Class
