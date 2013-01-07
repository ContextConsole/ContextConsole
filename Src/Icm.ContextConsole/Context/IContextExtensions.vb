Imports System.Runtime.CompilerServices

Public Module IContextExtensions

    <Extension>
    Public Function GetActions(ctx As IContext) As IEnumerable(Of IAction)
        Return ctx.ActionFinder.GetActions(ctx)
    End Function
End Module