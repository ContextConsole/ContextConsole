Imports System.Linq
Imports Icm.Collections
Imports Icm.Localization
Imports Icm.Tree

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
        ' Actions of current context
        ShowActions(Application.CurrentContextNode.Value)

        ' Available subcontexts
        Interactor.ShowList(
            "Subcontexts",
            Application.CurrentContextNode,
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
