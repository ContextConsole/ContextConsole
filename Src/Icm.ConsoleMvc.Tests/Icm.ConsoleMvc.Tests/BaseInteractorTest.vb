Imports NUnit.Framework
Imports Icm.ConsoleMvc
Imports Moq

<TestFixture()>
Public Class BaseInteractorTest

    <Test()>
    Public Sub AskString3()
        Dim mock As New Mock(Of BaseInteractor)
        mock.Setup(Function(bi) bi.AskString("a", It.IsAny(Of String))).Returns("asdf")

        Assert.AreEqual(mock.Object.AskString("a"), "asdf")

        Assert.AreEqual(mock.Object.AskString("a", Function(str) IsNumeric(str)), "asdf")
    End Sub

End Class
