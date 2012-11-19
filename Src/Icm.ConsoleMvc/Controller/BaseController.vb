Imports System.Linq
Imports Icm.Reflection

''' <summary>
''' A base controller implements many of the methods of IController based on 4 of them.
''' </summary>
''' <remarks></remarks>
Public MustInherit Class BaseController
    Implements IController

    Protected Friend ReadOnly _ctlman As IControllerManager

    Protected Property Interactor As IInteractor

    Protected Sub New(ctlman As IControllerManager, asker As IInteractor)
        _ctlman = ctlman
        Interactor = asker
    End Sub

    Public Function GetActions() As System.Collections.Generic.IEnumerable(Of IAction) Implements IController.GetActions
        Return _ctlman.GetActions(Me)
    End Function

    Public Function Name() As String Implements IController.Name
        Return _ctlman.GetName(Me)
    End Function

    Public Function GetAction(actionName As String) As IAction Implements IController.GetAction
        Return GetActions().SingleOrDefault(Function(act) act.Name = actionName OrElse act.Synonyms.Contains(actionName))
    End Function


    Public Function Synonyms() As System.Collections.Generic.IEnumerable(Of String) Implements IController.Synonyms
        Dim synAttr = Me.GetType().GetAttribute(Of SynonymAttribute)(True)
        Dim result As New List(Of String)
        If synAttr IsNot Nothing Then
            result.AddRange(synAttr.Synonyms.Select(Function(str) str.ToLower))
        End If
        Return result
    End Function

    Public Function Named(controllerName As String) As Boolean Implements IController.Named
        Return Name() = controllerName OrElse Synonyms().Contains(controllerName)
    End Function
End Class