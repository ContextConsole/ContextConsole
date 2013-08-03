Imports NUnit.Framework

<TestFixture>
Public Class StreamsInteractorTest

    Shared Function GetAskStringTestCases() As IEnumerable
        Dim valFunc As Func(Of String, Boolean) = Function(str As String) str = "qwer"
        Return {
            New TestCaseData(Nothing, Nothing, Nothing).Returns(Nothing).SetName("Empty list, no validation"),
            New TestCaseData(Nothing, {"2"}, Nothing).Returns("2"),
            New TestCaseData({"2"}, Nothing, Nothing).Returns("2"),
            New TestCaseData({"2"}, Nothing, valFunc).Returns(Nothing),
            New TestCaseData({"qwer"}, Nothing, valFunc).Returns("qwer")
        }
    End Function

    <Test()>
    <Timeout(2000)>
    <TestCaseSource("GetAskStringTestCases")>
    Public Function AskStringTest(tokenQueue() As String, input() As String, valFunc As Func(Of String, Boolean)) As String
        Dim inter = BuildInteractor(input)
        If tokenQueue IsNot Nothing Then
            Dim queue As New Queue(Of String)(tokenQueue)
            inter.TokenQueue = queue
        End If

        Dim result = inter.AskString("", valFunc)
        Return result
    End Function

    Shared Function GetAskOneByKeyTestCases() As IEnumerable
        Dim empty As New List(Of Tuple(Of Integer, String))
        Dim list = {Tuple.Create(1, "a"), Tuple.Create(2, "b"), Tuple.Create(3, "c")}
        Dim defaultValue = Tuple.Create(1, "a")
        Return {
            New TestCaseData(Nothing, Nothing, empty, Nothing).Throws(GetType(ArgumentException)).SetName("Empty list, no default value"),
            New TestCaseData(Nothing, Nothing, list, Nothing).Returns(Nothing),
            New TestCaseData(Nothing, {"2"}, list, Nothing).Returns(Tuple.Create(2, "b")),
            New TestCaseData({"2"}, Nothing, list, Nothing).Returns(Tuple.Create(2, "b")),
            New TestCaseData(Nothing, {"4"}, list, Nothing).Throws(GetType(InvalidOperationException)),
            New TestCaseData({"4"}, Nothing, list, Nothing).Throws(GetType(InvalidOperationException)),
            New TestCaseData(Nothing, Nothing, empty, defaultValue).Throws(GetType(ArgumentException)).SetName("Empty list, with default value"),
            New TestCaseData(Nothing, Nothing, list, defaultValue).Returns(defaultValue),
            New TestCaseData(Nothing, {"2"}, list, defaultValue).Returns(Tuple.Create(2, "b")),
            New TestCaseData({"2"}, Nothing, list, defaultValue).Returns(Tuple.Create(2, "b")),
            New TestCaseData(Nothing, {"4"}, list, defaultValue).Throws(GetType(InvalidOperationException)),
            New TestCaseData({"4"}, Nothing, list, defaultValue).Throws(GetType(InvalidOperationException))
        }
    End Function

    <Test()>
    <Timeout(2000)>
    <TestCaseSource("GetAskOneByKeyTestCases")>
    Public Function AskOneByKeyTest(tokenQueue() As String, input() As String, values As IEnumerable(Of Tuple(Of Integer, String)), defaultValue As Tuple(Of Integer, String)) As Tuple(Of Integer, String)
        Dim inter = BuildInteractor(input)
        If tokenQueue IsNot Nothing Then
            Dim queue As New Queue(Of String)(tokenQueue)
            inter.TokenQueue = queue
        End If
        Dim result = inter.AskOneByKey("", values, Function(tup) tup.Item1.ToString, toString:=Function(obj) obj.ToString, defaultValue:=defaultValue, selectedList:=New List(Of Tuple(Of Integer, String)))
        Return result
    End Function

End Class