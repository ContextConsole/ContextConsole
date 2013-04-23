''' <summary>
''' Standard parser
''' </summary>
''' <remarks>
''' The standard token parser separates tokens by spaces, but recognizes strings inside
''' double quotes (") as single tokens and can also escape characters inside and outside quotes
''' using the backslash (\).
''' </remarks>
Public Class StandardTokenParser
    Implements ITokenParser

    Private ReadOnly _errors As New List(Of ParseError)
    Private ReadOnly _tokens As New List(Of String)

    Private Enum ParserState
        OutsideQuotesNotEscaping
        OutsideQuotesEscaping
        InsideQuotesNotEscaping
        InsideQuotesEscaping
    End Enum

    Public Sub Parse(line As String) Implements ITokenParser.Parse
        Dim currentToken As New System.Text.StringBuilder
        Dim index As Integer = 0
        Dim state = ParserState.OutsideQuotesNotEscaping
        Dim oldState As ParserState
        Dim errorStartIndex As Integer = 0
        If line Is Nothing Then
            Exit Sub
        End If
        For Each character In line
            oldState = state
            Select Case state
                Case ParserState.OutsideQuotesNotEscaping ' Outside quotes and not escaping
                    If Char.IsWhiteSpace(character) Then
                        If currentToken.Length > 0 Then
                            _tokens.Add(currentToken.ToString)
#If Framework = "net35" Then
                            currentToken = New System.Text.StringBuilder
#Else
                            currentToken.Clear()
#End If
                        End If
                    ElseIf character = "\" Then
                        state = ParserState.OutsideQuotesEscaping
                    ElseIf character = """" Then
                        state = ParserState.InsideQuotesNotEscaping
                    Else
                        currentToken.Append(character)
                    End If
                Case ParserState.OutsideQuotesEscaping ' Outside quotes and escaping
                    currentToken.Append(character)
                    state = ParserState.OutsideQuotesNotEscaping
                Case ParserState.InsideQuotesNotEscaping ' Inside quotes and not escaping
                    If character = "\" Then
                        state = ParserState.InsideQuotesEscaping
                    ElseIf character = """" Then
                        _tokens.Add(currentToken.ToString)
#If Framework = "net35" Then
                        currentToken = New System.Text.StringBuilder
#Else
                            currentToken.Clear()
#End If
                        state = ParserState.OutsideQuotesNotEscaping
                    Else
                        currentToken.Append(character)
                    End If
                Case ParserState.InsideQuotesEscaping ' Inside quotes and escaping
                    currentToken.Append(character)
                    state = ParserState.InsideQuotesNotEscaping
                Case Else
                    Throw New Exception("Bad state! Cannot happen!")
            End Select
            If oldState = ParserState.OutsideQuotesNotEscaping AndAlso
               state <> ParserState.OutsideQuotesNotEscaping Then
                ' transitions between initial state and any other mark a possible
                ' cause for an error
                errorStartIndex = index
            End If

            index += 1
        Next
        If currentToken.Length > 0 Then
            _tokens.Add(currentToken.ToString)
        End If
        If state <> ParserState.OutsideQuotesNotEscaping Then
            _errors.Add(New ParseError(1, index, errorStartIndex))
        End If
    End Sub


    Public ReadOnly Property Errors As IEnumerable(Of ParseError) Implements ITokenParser.Errors
        Get
            Return _errors
        End Get
    End Property

    Public ReadOnly Property Tokens As IEnumerable(Of String) Implements ITokenParser.Tokens
        Get
            Return _tokens
        End Get
    End Property

    Public Sub Initialize() Implements ITokenParser.Initialize
        _tokens.Clear()
        _errors.Clear()
    End Sub

End Class