Imports Icm.Tree

Imports Icm.Localization

''' <summary>
''' This base root context provides implementation of Help action and Quit action.
''' </summary>
''' <remarks></remarks>
Public MustInherit Class RootContext
    Inherits BaseContext

    Private Application As IApplication

    Protected Overrides Sub BaseInitialize(app As IApplication)
        MyBase.BaseInitialize(app)
        Application = app
    End Sub

    Public MustOverride Sub Credits()

    Public Sub Help()
        ' Actions of current context
        ShowActions(Application.CurrentContextNode.Value)

        ' Available subcontexts
        Interactor.ShowList(
            "Subcontexts",
            Application.CurrentContextNode.GetChildNodes,
            Function(ctx)
                Return String.Format(My.Resources.Resources.root_use, _
                        ctx.Value.Name.ToLower)
            End Function,
            hideIfEmpty:=True)

        ' Inherited actions from ancestors
        For Each ctx In Application.CurrentContextNode.ProperAncestors
            ShowActions(ctx)
        Next
    End Sub

    Public Sub Contexts()
        For Each contextAndLevel In Application.RootContextNode.DepthPreorderTraverseWithLevel
            Interactor.ShowMessage(New String(" "c, contextAndLevel.Level) & contextAndLevel.Result.Name)
        Next
    End Sub

    Public Overridable Sub Quit()
        Interactor.ShowMessage(locRepo.Trans("quit_bye"))
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
            Return locRepo.Trans(action.LocalizationKey)
        Else
            Return extLocRepo.Trans(action.LocalizationKey)
        End If
    End Function

End Class
