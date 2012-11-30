Imports NUnit.Framework
Imports Icm.ConsoleMvc
Imports Moq
Imports System.IO


<TestFixture>
Public Class StreamsInteractorTest

    Private Function BuildInteractor(inputstr As String) As StreamsInteractor
        Dim input As New StringReader(inputstr)
        Dim output As New StringWriter
        Dim errorstr As New StringWriter

        Return New StreamsInteractor(input, output, errorstr)
    End Function

    Shared Iterator Function GetTestCases() As IEnumerable
        Dim empty As New List(Of Tuple(Of Integer, String))

        Yield New TestCaseData({"2"}, empty, Tuple.Create(2, "b")).Throws(GetType(ArgumentException))
        Yield New TestCaseData({"2"}, {Tuple.Create(1, "a"), Tuple.Create(2, "b")}, Tuple.Create(2, "b"))
    End Function

    <Test>
    <TestCaseSource(GetType(StreamsInteractorTest), "GetTestCases")>
    Public Sub AskOneByKeyTest(a() As String, values As IEnumerable(Of Tuple(Of Integer, String)), expected As Tuple(Of Integer, String))
        Dim inputgen As New StringWriter()
        For Each line In a
            inputgen.WriteLine(line)
        Next

        Dim inter = BuildInteractor(inputgen.ToString)

        Dim result = inter.AskOneByKey("", values, Function(tup) tup.Item1.ToString)
        Assert.That(result, [Is].EqualTo(expected))
    End Sub
End Class