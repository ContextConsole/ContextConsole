''' <summary>
''' A context provides access to its actions for them to be executed. It has a name and optional synonyms.
''' </summary>
''' <remarks></remarks>
Public Interface IContext
    Inherits INamedWithSynonyms

    Sub Initialize(app As IApplication)
    ReadOnly Property ActionFinder As IActionFinder

End Interface
