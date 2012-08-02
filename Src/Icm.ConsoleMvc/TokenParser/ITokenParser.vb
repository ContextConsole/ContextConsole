''' <summary>
''' Generic parser interface.
''' </summary>
''' <remarks>
''' Function Parse() parses the string and appends the obtained tokens to the Tokens list. It also appends
''' any error to the Errors list. If you need to start from scratch with an already used ITokenParser,
''' you should use Initialize before (this will clear the Tokens and Errors lists).
''' </remarks>
Public Interface ITokenParser

    ' Tokens recognized so far
    ReadOnly Property Tokens As IEnumerable(Of String)

    ReadOnly Property Errors As IEnumerable(Of String)

    Sub Initialize()

    Sub Parse(line As String)

End Interface
