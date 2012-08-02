Public Interface IControllerManager
    Function GetAllControllers() As IEnumerable(Of IBackController)
    Function GetName(ctl As IController) As String
    Function GetActions(ctl As IController) As IEnumerable(Of IAction)
End Interface
