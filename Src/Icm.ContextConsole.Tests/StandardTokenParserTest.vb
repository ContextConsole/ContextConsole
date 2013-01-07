Imports NUnit.Framework
Imports Icm.ContextConsole
Imports Moq

<TestFixture>
Public Class StandardTokenParserTest

    Private Shared Function ParseTestSource() As IEnumerable(Of TestCaseData)
        Return New List(Of TestCaseData) From {
            New TestCaseData(Nothing, New String() {}, New ParseError() {}),
            New TestCaseData("", New String() {}, New ParseError() {}),
            New TestCaseData("   " & vbTab, New String() {}, New ParseError() {}),
            New TestCaseData(vbTab & "command    ", {"command"}, New ParseError() {}),
            New TestCaseData("command -arg arg - arg2", {"command", "-arg", "arg", "-", "arg2"}, New ParseError() {}),
            New TestCaseData("command -arg ""arg   - arg2"" arg3  ", {"command", "-arg", "arg   - arg2", "arg3"}, New ParseError() {}),
            New TestCaseData("command -arg ""arg -  arg2", {"command", "-arg", "arg -  arg2"}, {New ParseError(1, 25, 13)})
        }
    End Function

    <Test>
    <TestCaseSource("ParseTestSource")>
    Public Sub ParseTest(line As String, tokens As IEnumerable(Of String), errors As IEnumerable(Of ParseError))
        Dim parser As New StandardTokenParser

        parser.Parse(line)

        Assert.That(parser.Tokens, [Is].EquivalentTo(tokens))
        Assert.That(parser.Errors, [Is].EquivalentTo(errors))

    End Sub
End Class
