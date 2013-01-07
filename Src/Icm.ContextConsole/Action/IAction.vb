''' <summary>
''' An action is a piece of executable code, with a name and optional synonyms.
''' It also provides a key for localizing the description of the action and a flag that
''' indicates if the action must be localized using Icm.ContextConsole.dll internal
''' resources or using external resources (provided by the client assemblies).
''' </summary>
''' <remarks></remarks>
Public Interface IAction
    Inherits INamedWithSynonyms

    Function IsInternal() As Boolean
    Function LocalizationKey() As String
    Sub Execute()

End Interface
