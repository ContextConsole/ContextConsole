''' <summary>
''' An interactor asks values and shows results, abstracting the data source and the target of the operations.
''' </summary>
''' <remarks></remarks>
Public Interface IInteractor
    Function AskString(ByVal prompt As String) As String
    Function AskString(ByVal prompt As String, ByVal defaultValue As String) As String
    Function AskString(ByVal prompt As String, ByVal validation As Func(Of String, Boolean)) As String
    Function AskString(ByVal prompt As String, ByVal validation As Func(Of String, Boolean), ByVal defaultValue As String) As String
    Function AskInteger(ByVal prompt As String, Optional ByVal defaultValue As Integer? = Nothing) As Integer?
    Function AskLiteral(Of T)(ByVal prompt As String, ByVal convert As Func(Of String, T), Optional ByVal defaultValue As T = Nothing) As T
    Function AskDate(ByVal prompt As String, Optional ByVal defaultValue As Date? = Nothing) As Date?
    Function AskBoolean(ByVal prompt As String, Optional ByVal defaultValue As Boolean? = Nothing) As Boolean?
    Function AskOne(Of T As Class)(ByVal prompt As String, ByVal values As IEnumerable(Of T)) As T
    Function AskOne(Of T As Class)(ByVal prompt As String, ByVal values As IEnumerable(Of T), ByVal toString As Func(Of T, String)) As T
    Function AskOne(Of T As Class)(ByVal prompt As String, ByVal values As IEnumerable(Of T), ByVal toString As Func(Of T, String), ByVal defaultValue As T, ByVal selectedList As List(Of T)) As T
    Function AskOneByKey(Of T As Class)(ByVal prompt As String, ByVal values As IEnumerable(Of T), ByVal key As Func(Of T, String)) As T
    Function AskOneByKey(Of T As Class)(ByVal prompt As String, ByVal values As IEnumerable(Of T), ByVal key As Func(Of T, String), ByVal toString As Func(Of T, String)) As T
    Function AskOneByKey(Of T As Class)(ByVal prompt As String, ByVal values As IEnumerable(Of T), ByVal key As Func(Of T, String), ByVal toString As Func(Of T, String), ByVal defaultValue As T, ByVal selectedList As List(Of T)) As T
    Function AskMany(Of T As Class)(ByVal generalPrompt As String, ByVal prompt As String, ByVal values As IEnumerable(Of T)) As List(Of T)
    Function AskMany(Of T As Class)(ByVal generalPrompt As String, ByVal prompt As String, ByVal values As IEnumerable(Of T), ByVal initialList As List(Of T)) As List(Of T)
    Function AskMany(Of T As Class)(ByVal generalPrompt As String, ByVal prompt As String, ByVal values As IEnumerable(Of T), ByVal toString As Func(Of T, String)) As List(Of T)
    Function AskMany(Of T As Class)(ByVal generalPrompt As String, ByVal prompt As String, ByVal values As IEnumerable(Of T), ByVal toString As Func(Of T, String), ByVal initialList As List(Of T)) As List(Of T)

    Sub ShowProperties(Of T As Class)(ByVal title As String, ByVal obj As T)
    Sub ShowList(Of T As Class)(ByVal title As String, ByVal values As IEnumerable(Of T))
    Sub ShowNumberedList(Of T As Class)(ByVal title As String, ByVal values As IEnumerable(Of T))
    Sub ShowKeyedList(Of T As Class)(ByVal title As String, ByVal values As IEnumerable(Of T), ByVal key As Func(Of T, String))
    Sub ShowList(Of T As Class)(ByVal title As String, ByVal values As IEnumerable(Of T), ByVal toString As Func(Of T, String))
    Sub ShowNumberedList(Of T As Class)(ByVal title As String, ByVal values As IEnumerable(Of T), ByVal toString As Func(Of T, String))
    Sub ShowKeyedList(Of T As Class)(ByVal title As String, ByVal values As IEnumerable(Of T), ByVal key As Func(Of T, String), ByVal toString As Func(Of T, String))
    Sub ShowErrors(list As IEnumerable(Of String))
    Sub ShowMessage(msg As String)

    Sub EnqueueToken(tok As String)
End Interface
