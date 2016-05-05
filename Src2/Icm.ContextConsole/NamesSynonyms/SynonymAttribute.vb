''' <summary>
''' This attribute provides synonyms for the given item.
''' </summary>
''' <remarks></remarks>
Public Class SynonymAttribute
    Inherits Attribute

    Public Synonyms() As String

    Public Sub New(ParamArray synonyms As String())
        Me.Synonyms = synonyms
    End Sub

End Class
