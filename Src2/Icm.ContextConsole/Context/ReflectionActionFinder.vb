''' <summary>
''' Finds actions by using reflection on the context.
''' </summary>
''' <remarks>It finds all public non-static parameterless procedures.</remarks>
Public Class ReflectionActionFinder
    Implements IActionFinder

    Public Function GetActions(ctl As IContext) As IEnumerable(Of IAction) Implements IActionFinder.GetActions
        Dim methods = ctl.GetType.GetMethods().Where(Function(minf) _
                                minf.ReturnType.Name = "Void" AndAlso _
                                minf.GetParameters().Count = 0 AndAlso _
                                minf.IsPublic AndAlso _
                                Not minf.IsStatic)

#If Framework = "net35" Then
        Return methods.Select(Function(minf) New MethodInfoAction(minf, ctl).As(Of IAction)())
#Else
        Return methods.Select(Function(minf) New MethodInfoAction(minf, ctl))
#End If
    End Function
End Class