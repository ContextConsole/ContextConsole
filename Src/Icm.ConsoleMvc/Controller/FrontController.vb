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

    Private ReadOnly _controllers As New List(Of IController)
    Private ReadOnly _tokenParser As ITokenParser
    Private ReadOnly _environmentArgsTaken As Boolean
    Private _usedController As IController

    Protected FrontInteractor As IFrontInteractor


    Protected Sub New(ctlman As IControllerManager, asker As IFrontInteractor, tokenParser As ITokenParser)
        MyBase.New(ctlman, asker)
        FrontInteractor = asker
        _tokenParser = tokenParser
        _controllers.Add(Me) ' First the front controller (so that its actions get presented first at help)
        _controllers.AddRange(_ctlman.GetAllControllers())
        _usedController = Me

        Dim environmentArgs = Environment.GetCommandLineArgs
        If environmentArgs.Count > 1 Then
            _environmentArgsTaken = True
        End If
        Interactor.EnqueueToken(environmentArgs.Skip(1).Select(Function(arg) If(arg.Contains(" "), """" & arg & """", arg)).JoinStr(" "))
    End Sub

    Public Sub Help() Implements IFrontController.Help
        For Each ctl In _controllers
            ShowActions(ctl)
        Next
    End Sub

    Public Sub Controllers() Implements IFrontController.Controllers
        Interactor.ShowList(Of IController)("Controller list", _controllers, Function(obj) obj.Name)
    End Sub

    Public Sub Actions() Implements IFrontController.Actions
        ShowActions(Me)
    End Sub

    Public Sub Use()
        Dim ctl = FrontInteractor.AskController("Controller (leave blank for Main)", Me)
        If ctl IsNot Nothing Then
            Interactor.ShowMessage("Using controller " & ctl.Name)
            _usedController = ctl
        End If
    End Sub

    Public Overridable Sub Quit() Implements IFrontController.Quit
        Interactor.ShowMessage("Bye!")
    End Sub

    Protected MustOverride Sub ShowTitles() Implements IFrontController.ShowTitles

    Protected Overridable Function Initialize() As Boolean
        Return True
    End Function

    Protected Function InitializeApplication() As Boolean Implements IFrontController.InitializeApplication
        If Not _environmentArgsTaken Then
            ShowTitles()
        End If
        return Initialize()
    End Function

    Protected Overridable Function CommandPrompt() As String
        Return ""
    End Function

    Private Function ExecuteCommand() As Boolean Implements IFrontController.ExecuteCommand

        Dim command = FrontInteractor.AskCommand(CommandPrompt)
        _tokenParser.Initialize()
        _tokenParser.Parse(command)

        If _tokenParser.Errors.Count > 0 Then
            Interactor.ShowErrors(_tokenParser.Errors)
            Return False
        ElseIf _tokenParser.Tokens.Count = 0 Then
            Return False
        End If

        Dim saTokens = _tokenParser.Tokens

        For i = 1 To saTokens.Count - 1
            Interactor.EnqueueToken(saTokens(i))
        Next
        Dim sa = saTokens(0).Split(CtlActionSeparator)
        Dim controllerName As String
        Dim ctl As IController

        If sa.Length = 1 Then
            ctl = _usedController
            controllerName = ctl.Name
        Else
            controllerName = sa(0).ToLower
            ctl = _controllers.SingleOrDefault(Function(ctrl) _
                                                   ctrl.Name = controllerName OrElse _
                                                   ctrl.Synonyms().Contains(controllerName))
        End If

        If ctl Is Nothing Then
            Interactor.ShowErrors({String.Format("Could not find controller {0}", controllerName)})
        Else
            Dim actionName As String

            actionName = sa.Last.ToLower
            Dim action = ctl.GetAction(actionName)

            If action Is Nothing Then
                Interactor.ShowErrors({String.Format("Could not find action {1} inside controller {0}", controllerName, sa.Last)})
            Else
                action.Execute()
                Interactor.ShowMessage("")
                Return (ctl Is Me AndAlso action.Name = "quit") OrElse _environmentArgsTaken
            End If
        End If
        Return False
    End Function

    Private Sub ShowActions(ByVal ctl As IController)
        Interactor.ShowList(String.Format("{0} actions{1}", ctl.Name, If(_usedController Is ctl, " (CURRENT)", "")),
                 ctl.GetActions,
                 Function(action) _
                     String.Format("{0}{1}: {2}", _
                        If(_usedController Is ctl, "", ctl.Name & CtlActionSeparator), _
                        action.Name.ToLower, _
                        If( _
                            My.Resources.ResourceManager.GetString(String.Format("{0}_{1}", ctl.Name, action.Name)), _
                            action.Name)))
    End Sub

End Class
