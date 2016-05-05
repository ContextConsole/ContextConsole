Imports Icm.Localization
Imports Icm.Tree

Public Interface IApplication

    Property ApplicationPrompt As String

    ReadOnly Property CurrentContextNode As ITreeNode(Of IContext)
    ReadOnly Property RootContextNode As ITreeNode(Of IContext)

    Function GetAllContexts() As IEnumerable(Of IContext)

    Function ExecuteCommand() As Boolean

    ReadOnly Property Interactor As IInteractor

    ReadOnly Property InternalLocRepo As ILocalizationRepository
    ReadOnly Property ExternalLocRepo As ILocalizationRepository

    Sub Run()

    Property DependencyResolver As IDependencyResolver

End Interface
