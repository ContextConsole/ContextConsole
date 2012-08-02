Public Class ReflectionControllerManager
    Implements IControllerManager

    Public Function GetAllControllers() As IEnumerable(Of IBackController) Implements IControllerManager.GetAllControllers
        Return Icm.Ninject.Instances(Of IBackController)()
    End Function

    Public Function GetActions(ctl As IController) As System.Collections.Generic.IEnumerable(Of IAction) Implements IControllerManager.GetActions
        Dim methods = ctl.GetType.GetMethods().Where(Function(minf) _
                                                minf.ReturnType.Name = "Void" AndAlso _
                                                minf.GetParameters().Count = 0 AndAlso _
                                                minf.IsPublic AndAlso _
                                                Not minf.IsStatic)

        Return methods.Select(Function(minf) New MethodInfoAction(minf, ctl))
    End Function

    Public Function GetName(ctl As IController) As String Implements IControllerManager.GetName
        Dim ctlTypeName = ctl.GetType.Name
        Return ctlTypeName.Substring(0, ctlTypeName.Length - "Controller".Length).ToLower

    End Function
End Class
