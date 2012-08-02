Public Interface IAction
    ReadOnly Property Name() As String
    Function Named(actionName As String) As Boolean
    ReadOnly Property Synonyms() As IEnumerable(Of String)
    Sub Execute()
End Interface
