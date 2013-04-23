Imports Icm.Localization

''' <summary>
''' A base context implements many of the methods of IContext based on 4 of them.
''' </summary>
''' <remarks></remarks>
Public MustInherit Class BaseContext
    Implements IContext


    Private ReadOnly _actionFinder As IActionFinder
    Protected locRepo As ILocalizationRepository
    Protected extLocRepo As ILocalizationRepository

    Property Interactor As IInteractor

    Public Sub New()
        _actionFinder = New ReflectionActionFinder
    End Sub

    Public Sub New(actionFinder As IActionFinder)
        _actionFinder = actionFinder
    End Sub


    Protected Overridable Sub BaseInitialize(app As IApplication) Implements IContext.Initialize
        locRepo = app.InternalLocRepo
        extLocRepo = app.ExternalLocRepo
        Interactor = app.Interactor
        Initialize()
    End Sub

    Protected Overridable Sub Initialize()
    End Sub

    Public Function Name() As String Implements IContext.Name
        Dim ctlTypeName = Me.GetType.Name
        Return ctlTypeName.Substring(0, ctlTypeName.Length - "Context".Length).ToLower
    End Function


    Public Function Synonyms() As IEnumerable(Of String) Implements IContext.Synonyms
        Dim synAttr = Me.GetType().GetAttribute(Of SynonymAttribute)(True)
        Dim result As New List(Of String)
        If synAttr IsNot Nothing Then
            result.AddRange(synAttr.Synonyms.Select(Function(str) str.ToLower))
        End If
        Return result
    End Function

    Public Function Named(contextName As String) As Boolean
        Return Name() = contextName OrElse Synonyms().Contains(contextName)
    End Function

    Public ReadOnly Property ActionFinder As IActionFinder Implements IContext.ActionFinder
        Get
            Return _actionFinder
        End Get
    End Property
End Class
