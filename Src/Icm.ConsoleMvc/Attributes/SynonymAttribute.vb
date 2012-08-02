
Public Class SynonymAttribute
    Inherits Attribute
    Public Synonyms() As String
    Public Sub New(ParamArray synonyms As String())
        Me.Synonyms = synonyms
    End Sub
End Class
