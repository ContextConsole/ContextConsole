Imports Icm.Localization


Public Interface IApplicationContext

    ReadOnly Property CurrentContextNode As ITreeNode(Of IContext)
    ReadOnly Property RootContextNode As ITreeNode(Of IContext)

    Function GetAllContexts() As IEnumerable(Of IContext)

    ReadOnly Property TokenQueue As Queue(Of String)

    ReadOnly Property Interactor As IInteractor

    ReadOnly Property InternalLocRepo As ILocalizationRepository
    ReadOnly Property ExternalLocRepo As ILocalizationRepository

End Interface
