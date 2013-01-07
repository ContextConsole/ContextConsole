Imports NUnit.Framework
Imports Icm.ContextConsole
Imports Moq

<TestFixture>
Public Class FrontContextTest

    <Test>
    Public Sub ConstructorTest()
        Assert.That(Sub()
                        Dim fctl As IContext = New TestFrontContext(New StreamsInteractor)
                    End Sub,
                    Throws.Nothing)
    End Sub

    <TestCase("asdf", "executecommand_actionnotfound", False)>
    <TestCase("help", Nothing, False)>
    <TestCase("quit", Nothing, True)>
    Public Sub ExecuteCommandTest(input As String, errorKey As String, expectedMustQuit As Boolean)
        Dim tw = New System.IO.StringWriter()
        Dim twerr = New System.IO.StringWriter()
        Dim inter = BuildInteractor({input}, tw, twerr)
        Dim mustQuit As Boolean
        Assert.That(Sub()
                        Dim fctl As IApplication = New StandardApplication(New TestFrontContext(inter), inter)
                        mustQuit = fctl.ExecuteCommand()
                    End Sub,
                    Throws.Nothing)

        Assert.That(mustQuit, [Is].EqualTo(expectedMustQuit))
        If errorKey IsNot Nothing Then
            Assert.That(twerr.ToString, [Is].StringContaining(errorKey))
        End If
    End Sub

End Class
