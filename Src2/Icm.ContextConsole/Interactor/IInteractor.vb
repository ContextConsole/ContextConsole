Imports Icm.Localization

''' <summary>
''' An interactor asks values and shows results, abstracting the data source and the target of the operations.
''' </summary>
''' <remarks></remarks>
Public Interface IInteractor

    Sub SetLocalizationRepo(locRepo As ILocalizationRepository)

    Property PromptSeparator As String
    Property TokenQueue As Queue(Of String)
    Property CommandPromptSeparator As String

    Function GetPropertyDescription(ByVal pi As System.Reflection.PropertyInfo, ByVal obj As Object) As String

    Function AskString(ByVal prompt As String, ByVal validation As Func(Of String, Boolean), ByVal defaultValue As String) As String
    Function AskOne(Of T As Class)(ByVal prompt As String, ByVal values As IEnumerable(Of T), ByVal toString As Func(Of T, String), ByVal defaultValue As T, ByVal selectedList As List(Of T)) As T
    Function AskOneByKey(Of T As Class)(ByVal prompt As String, ByVal values As IEnumerable(Of T), ByVal key As Func(Of T, String), ByVal toString As Func(Of T, String), ByVal defaultValue As T, ByVal selectedList As List(Of T)) As T
    Function AskMany(Of T As Class)(ByVal generalPrompt As String, ByVal prompt As String, ByVal values As IEnumerable(Of T), ByVal toString As Func(Of T, String), ByVal initialList As List(Of T)) As List(Of T)
    Function AskCommand(ByVal prompt As String) As String

    Sub ShowList(Of T As Class)(ByVal title As String, ByVal values As IEnumerable(Of T), ByVal toString As Func(Of T, String), hideIfEmpty As Boolean)
    Sub ShowNumberedList(Of T As Class)(ByVal title As String, ByVal values As IEnumerable(Of T), ByVal toString As Func(Of T, String), hideIfEmpty As Boolean)
    Sub ShowKeyedList(Of T As Class)(ByVal title As String, ByVal values As IEnumerable(Of T), ByVal key As Func(Of T, String), ByVal toString As Func(Of T, String), hideIfEmpty As Boolean)
    Sub ShowErrors(list As IEnumerable(Of String))
    Sub ShowMessage(msg As String)
End Interface
