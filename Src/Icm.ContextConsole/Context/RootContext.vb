Imports System.Linq
Imports Icm.Collections
Imports Icm.Localization

''' <summary>
''' This base root context provides implementation of Help action and Quit action.
''' </summary>
''' <remarks></remarks>
Public MustInherit Class RootContext
    Inherits BaseContext

    Private Application As IApplication

    Public Sub New()

    End Sub

    Protected Overrides Sub BaseInitialize(app As IApplication)
        MyBase.BaseInitialize(app)
        Application = app
    End Sub

    Public Sub Help()
        ShowActions(Me)
        Interactor.ShowList(
            "Subcontexts",
            Application.CurrentContextNode.GetChildren,
            Function(ctx)
                Return String.Format("{0}: change to {0} subcontext", _
                        ctx.Value.Name.ToLower)
            End Function)

        For Each ctx In Application.CurrentContextNode.Ancestors
            ShowActions(ctx)
        Next
        'For Each ctl In Application.GetAllContexts
        '    ShowActions(ctl)
        'Next
    End Sub

    Public Sub Contexts()
        For Each item In Application.RootContextNode.DepthPreorderTraverseWithLevel
            Interactor.ShowMessage(New String(" "c, item.Item2) & item.Item1.Name)
        Next
    End Sub

    Public Overridable Sub Quit()
        Interactor.ShowMessage(locRepo("quit_bye"))
    End Sub

    Private Sub ShowActions(ByVal ctl As IContext)
        Dim IsCurrent = (Application.CurrentContextNode.Value.Name = ctl.Name)

        Dim title As String
        If IsCurrent Then
            title = locRepo.TransF("help_title", ctl.Name, PhrF("help_current"))
        Else
            title = locRepo.TransF("help_title", ctl.Name)
        End If

        Interactor.ShowList(
            title,
            ctl.GetActions(),
            Function(action) _
                     String.Format("{0}: {1}", _
                        action.Name.ToLower, _
                        If( _
                            TranslateActionDescription(action), _
                            action.Name)))
    End Sub

    Private Function TranslateActionDescription(action As IAction) As String
        If action.IsInternal Then
            Return locRepo(action.LocalizationKey)
        Else
            Return extLocRepo(action.LocalizationKey)
        End If
    End Function



End Class
