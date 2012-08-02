Imports System.Linq
Imports Icm.Collections

''' <summary>
''' Implementation of IFrontController using Console methods.
''' </summary>
''' <remarks></remarks>
Public MustInherit Class FrontController
    Inherits BaseController
    Implements IFrontController

    Private Const CtlActionSeparator = "."c

    Private ReadOnly controllers_ As New List(Of IController)
    Private ReadOnly tokenParser_ As ITokenParser
    Private usedController_ As IController

    Protected FrontAsker As IFrontInteractor
    Protected TokenQueue As New Queue(Of String)

    Public Sub New(ctlman As IControllerManager, asker As IFrontInteractor)
        MyBase.New(ctlman, asker)
        FrontAsker = asker
        tokenParser_ = New StandardTokenParser
        controllers_.Add(Me) ' First the front controller (so that its actions get presented first at help)
        controllers_.AddRange(ctlman_.GetAllControllers())
        usedController_ = Me
    End Sub
    Public Sub New(ctlman As IControllerManager, asker As IFrontInteractor, tokenParser As ITokenParser)
        MyBase.New(ctlman, asker)
        FrontAsker = asker
        tokenParser_ = tokenParser
        controllers_.Add(Me) ' First the front controller (so that its actions get presented first at help)
        controllers_.AddRange(ctlman_.GetAllControllers())
        usedController_ = Me
    End Sub

    Public Sub Help() Implements IFrontController.Help
        For Each ctl In controllers_
            ShowActions(ctl)
        Next
    End Sub

    Public Sub Controllers() Implements IFrontController.Controllers
        Asker.ShowList(Of IController)("Controller list", controllers_, Function(obj) obj.Name)
    End Sub

    Public Sub Actions() Implements IFrontController.Actions
        ShowActions(Me)
    End Sub

    Public Sub Use()
        Dim ctl = FrontAsker.AskController("Controller (leave blank for Main)", Me)
        If ctl IsNot Nothing Then
            Asker.ShowMessage("Using controller " & ctl.Name)
            usedController_ = ctl
        End If
    End Sub

    Public Overridable Sub Quit() Implements IFrontController.Quit
        Asker.ShowMessage("Bye!")
    End Sub

    Protected MustOverride Function ShowTitles() As Boolean Implements IFrontController.ShowTitles

    Protected MustOverride Function InitializeApplication() As Boolean Implements IFrontController.InitializeApplication

    Private Function ExecuteCommand() As Boolean Implements IFrontController.ExecuteCommand
        Dim command = Asker.AskString(">")
        tokenParser_.Initialize()
        tokenParser_.Parse(command)

        If tokenParser_.Errors.Count > 0 Then
            Asker.ShowErrors(tokenParser_.Errors)
            Return False
        ElseIf tokenParser_.Tokens.Count = 0 Then
            Return False
        End If

        Dim saTokens = tokenParser_.Tokens

        For i = 1 To saTokens.Count - 1
            TokenQueue.Enqueue(saTokens(i))
        Next
        Dim sa = saTokens(0).Split(CtlActionSeparator)
        Dim controllerName As String
        Dim ctl As IController

        If sa.Length = 1 Then
            ctl = usedController_
            controllerName = ctl.Name
        Else
            controllerName = sa(0).ToLower
            ctl = controllers_.SingleOrDefault(Function(ctrl) _
                                                   ctrl.Name = controllerName OrElse _
                                                   ctrl.Synonyms().Contains(controllerName))
        End If

        If ctl Is Nothing Then
            Asker.ShowErrors({String.Format("Could not find controller {0}", controllerName)})
        Else
            Dim actionName As String

            actionName = sa.Last.ToLower
            Dim action = ctl.GetAction(actionName)

            If action Is Nothing Then
                Asker.ShowErrors({String.Format("Could not find action {1} inside controller {0}", controllerName, sa.Last)})
            Else
                action.Execute()
                Asker.ShowMessage("")
                Return (ctl Is Me AndAlso action.Name = "quit")
            End If
        End If
        Return False
    End Function

    Private Sub ShowActions(ByVal ctl As IController)
        Asker.ShowList(String.Format("{0} actions{1}", ctl.Name, If(usedController_ Is ctl, " (CURRENT)", "")),
                 ctl.GetActions,
                 Function(action) _
                     String.Format("{0}{1}: {2}", _
                        If(usedController_ Is ctl, "", ctl.Name & CtlActionSeparator), _
                        action.Name.ToLower, _
                        If( _
                            My.Resources.ResourceManager.GetString(String.Format("{0}_{1}", ctl.Name, action.Name)), _
                            action.Name)))
    End Sub

End Class
