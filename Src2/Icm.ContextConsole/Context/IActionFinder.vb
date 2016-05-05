
''' <summary>
''' An Action Finder finds the actions a context implements
''' </summary>
''' <remarks></remarks>
Public Interface IActionFinder
    Function GetActions(ctl As IContext) As IEnumerable(Of IAction)
End Interface
