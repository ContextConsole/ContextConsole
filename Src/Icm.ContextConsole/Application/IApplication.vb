Imports Icm.Localization

Public Interface IApplication

    Property CurrentContextNode As ITreeNode(Of IContext)
    Property RootContextNode As ITreeNode(Of IContext)

    Function GetAllContexts() As IEnumerable(Of IContext)

    Function ExecuteCommand() As Boolean

    Property TokenQueue As Queue(Of String)

    Property Interactor As IInteractor

    Property InternalLocRepo As ILocalizationRepository
    Property ExternalLocRepo As ILocalizationRepository

End Interface
