Imports Icm.ContextConsole
Imports System.Runtime.CompilerServices
Imports Icm.Tree

Public Module App

    Sub Main()
        Dim app = StandardApplication.Create(Of MainContext)(extLocRepo:=My.Resources.ResourceManager.ToRepository)
        app.DependencyResolver = New CustomDependencyResolver()
        app.ApplicationPrompt = "EXAMPLE"
        app.Run()

        Dim root As New TreeNode(Of Type)(GetType(MainContext))

        root.AddEnter(Of MathContext) _
                .AddEnter(Of MathContext) _
                    .Add(Of MathContext) _
                    .Add(Of MathContext) _
                    .Parent _
                .Parent _
            .Add(Of MathContext)()

        Dim sct As New TypeContextTreeBuilder(root)

        app = New StandardApplication(root, extLocRepo:=My.Resources.ResourceManager.ToRepository)

        app.ApplicationPrompt = "EXAMPLE2"
        app.Run()

    End Sub

    <Extension>
    Public Function Add(Of T As IContext)(tn As TreeNode(Of Type)) As TreeNode(Of Type)
        Dim child = tn.AddChild(GetType(T))
        Return tn
    End Function

    <Extension>
    Public Function AddEnter(Of T As IContext)(tn As TreeNode(Of Type)) As TreeNode(Of Type)
        Dim child = tn.AddChild(GetType(T))
        Return child
    End Function

End Module

Public Class CustomDependencyResolver
    Inherits StandardDependencyResolver

    Private Counter As Integer = 0

    Protected Overrides Function CreateService(service As Type) As Object
        If service.Equals(GetType(MathContext)) Then
            Counter += 1
            Return New MathContext() With {.PostFix = "Injected" + Counter.ToString}
        End If
        Return MyBase.CreateService(service)
    End Function

End Class