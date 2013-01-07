Imports NUnit.Framework

<TestFixture>
Public Class StreamsInteractorTest

    Shared Function GetTestCases() As IEnumerable
        Dim empty As New List(Of Tuple(Of Integer, String))

        Return {
            New TestCaseData(Nothing, empty, Nothing).Throws(GetType(ArgumentException)),
            New TestCaseData(Nothing, {Tuple.Create(1, "a"), Tuple.Create(2, "b")}, Nothing),
            New TestCaseData({"2"}, {Tuple.Create(1, "a"), Tuple.Create(2, "b")}, Tuple.Create(2, "b"))
        }
    End Function

    <Test()>
    <Timeout(2000)>
    <TestCaseSource("GetTestCases")>
    Public Sub AskOneByKeyTest(input() As String, values As IEnumerable(Of Tuple(Of Integer, String)), expected As Tuple(Of Integer, String))
        Dim inter = BuildInteractor(input)

        Dim result = inter.AskOneByKey("", values, Function(tup) tup.Item1.ToString)
        Assert.That(result, [Is].EqualTo(expected))
    End Sub
End Class