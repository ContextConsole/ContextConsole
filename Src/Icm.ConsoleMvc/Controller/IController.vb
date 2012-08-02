''' <summary>
''' An interactive controller allows to ask for many kinds of data and to show lists of data.
''' </summary>
''' <remarks></remarks>
Public Interface IController

    Function Name() As String
    Function Named(controllerName As String) As Boolean

    Function GetActions() As IEnumerable(Of IAction)

    Function GetAction(actionName As String) As IAction
    Function Synonyms() As IEnumerable(Of String)

End Interface
