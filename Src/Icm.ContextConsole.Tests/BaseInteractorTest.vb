Imports NUnit.Framework
Imports Icm.ContextConsole
Imports Moq
Imports Moq.Protected

''' <summary>
''' 
''' </summary>
''' <remarks>
''' Timeouts are set because AskString with validation but without defaultValue will ask forever if the validation does
''' not succeed.
''' </remarks>
<TestFixture()>
Public Class BaseInteractorTest
#If Framework <> "net35" Then
    <Test()>
    <Timeout(3000)>
    Public Sub AskString1()

        Dim mock As New Mock(Of BaseInteractor)
        mock.Setup(Function(bi) bi.AskStringWithTokenQueue("prompt", It.IsAny(Of String), It.IsAny(Of String))).Returns("asdf")

        Assert.That(mock.Object.AskString("prompt"), [Is].EqualTo("asdf"), "No validation")

    End Sub

    <Test()>
    <Timeout(3000)>
    Public Sub AskString2()
        Dim mock As New Mock(Of BaseInteractor)
        mock.SetupSequence(Function(bi) bi.AskStringWithTokenQueue("prompt", It.IsAny(Of String), It.IsAny(Of String))).Returns("asdf").Returns("23")

        Assert.That(mock.Object.AskString("prompt", Function(str) IsNumeric(str)), [Is].EqualTo("23"))
    End Sub
#End If

End Class
