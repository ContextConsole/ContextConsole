Imports NUnit.Framework
Imports Icm.ContextConsole
Imports Moq

<TestFixture>
Public Class MethodInfoActionTest

    Private Class TestContext
        Implements IContext


        Public Function Name() As String Implements IContext.Name
            Throw New NotImplementedException
        End Function

        Public Function Synonyms() As IEnumerable(Of String) Implements IContext.Synonyms
            Throw New NotImplementedException
        End Function

        Public Value As String = ""

        <Synonym("OtherName", "Another")>
        Public Sub TestAction()
            Value = "OK"
        End Sub

        Public Sub Initialize(app As IApplication) Implements IContext.Initialize

        End Sub

        Public ReadOnly Property ActionFinder As IActionFinder Implements IContext.ActionFinder
            Get
                Throw New NotImplementedException
            End Get
        End Property
    End Class

    <Test>
    Public Sub NameTest()
        Dim ctl As New TestContext

        Dim mi = GetType(TestContext).GetMethod("TestAction")
        Dim action As New MethodInfoAction(mi, ctl)

        Assert.That(action.Name, [Is].EqualTo("testaction"))
    End Sub

    <Test>
    Public Sub SynonymsTest()
        Dim ctl As New TestContext

        Dim mi = GetType(TestContext).GetMethod("TestAction")
        Dim action As New MethodInfoAction(mi, ctl)

        Assert.That(action.Synonyms, [Is].EquivalentTo({"othername", "another"}))
    End Sub

    <Test>
    Public Sub NamedTest()
        Dim ctl As New TestContext

        Dim mi = GetType(TestContext).GetMethod("TestAction")
        Dim action As New MethodInfoAction(mi, ctl)

        Assert.That(action.IsNamed("TestAction"))
        Assert.That(action.IsNamed("OtherName"))
        Assert.That(action.IsNamed("Another"))
    End Sub

    <Test>
    Public Sub ValueTest()
        Dim ctl As New TestContext

        Dim mi = GetType(TestContext).GetMethod("TestAction")
        Dim action As New MethodInfoAction(mi, ctl)

        action.Execute()

        Assert.That(ctl.Value, [Is].EqualTo("OK"))
    End Sub
End Class
