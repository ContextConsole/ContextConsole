
Public Structure ParseError

    Private ReadOnly _index As Integer
    Private ReadOnly _startIndex As Integer
    Private ReadOnly _code As Integer

    Sub New(code As Integer, index As Integer, errorStartIndex As Integer)
        _code = code
        _index = index
        _startIndex = errorStartIndex
    End Sub

    ReadOnly Property Code As Integer
        Get
            Return _code
        End Get
    End Property

    ReadOnly Property Index As Integer
        Get
            Return _index
        End Get
    End Property

    ReadOnly Property StartIndex As Integer
        Get
            Return _startIndex
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return String.Format("Parse error code {0} at {1}, starting at {2}", Code, Index, StartIndex)
    End Function

End Structure
