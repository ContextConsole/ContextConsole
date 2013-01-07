Imports Icm.ContextConsole
Imports System.Runtime.CompilerServices


Public Module App

    Sub Main()
        ApplicationFactory.Start(Of MainContext)(My.Resources.ResourceManager)

        Dim root As New TreeNode(Of Type)(GetType(MainContext))


        root.Add(Of MathContext) _
                .Add(Of MathContext) _
                    .Add(Of MathContext) _
                    .Add(Of MathContext) _
                    .Parent _
                .Parent _
            .Add(Of MathContext)()

        Dim sct As New TypeContextTreeBuilder(root)

    End Sub

    <Extension>
    Public Function Add(Of T As IContext)(tn As TreeNode(Of Type)) As TreeNode(Of Type)
        Dim child = tn.AddChild(GetType(T))
        Return child
    End Function

End Module
