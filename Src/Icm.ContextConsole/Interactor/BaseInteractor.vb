Imports Icm.Localization
Imports Icm.Ninject

''' <summary>
''' The base interactor provide some trivial implementations of some of the overloads of IInteractor.
''' </summary>
''' <remarks></remarks>
Public MustInherit Class BaseInteractor
    Implements IInteractor

    Property PromptSeparator As String = ": " Implements IInteractor.PromptSeparator

    Protected locRepo As ILocalizationRepository

    Protected Sub New()
        locRepo = TryInstance(Of ILocalizationRepository)("ConsoleMvcInternalResources")
        If locRepo Is Nothing Then
            locRepo = New DictionaryLocalizationRepository
        End If
    End Sub

    Protected MustOverride Function AskStringWithoutTokenQueue(ByVal prompt As String, ByVal defaultValue As String, promptSeparator As String) As String

    Public Overridable Function AskStringWithTokenQueue(ByVal prompt As String, ByVal defaultValue As String, promptSeparator As String) As String
        Dim response As String
        If TokenQueue.Count <> 0 Then
            response = TokenQueue.Dequeue
        Else
            response = AskStringWithoutTokenQueue(prompt, defaultValue, promptSeparator)
        End If
        Return response
    End Function

    Public Function AskString(prompt As String, validation As Func(Of String, Boolean), defaultValue As String) As String Implements IInteractor.AskString
        Dim response As String
        Do
            response = AskString(prompt, defaultValue)
            If String.IsNullOrEmpty(response) Then
                Return defaultValue
            End If
            If validation Is Nothing Then
                Exit Do
            ElseIf validation.Invoke(response) Then
                Exit Do
            End If
        Loop

        Return response
    End Function

    Public Function AskInteger(ByVal prompt As String, ByVal defaultValue As Integer?) As Integer? Implements IInteractor.AskInteger
        Return AskLiteral(Of Integer?)(prompt, Function(obj) CInt(obj), defaultValue)
    End Function

    Public Function AskLiteral(Of T)(ByVal prompt As String, convert As Func(Of String, T), ByVal defaultValue As T) As T Implements IInteractor.AskLiteral
        Dim response = AskString(prompt, AddressOf IsNumeric, If(defaultValue IsNot Nothing, defaultValue.ToString, ""))
        If String.IsNullOrEmpty(response) Then
            Return defaultValue
        Else
            Return convert(response)
        End If
    End Function

    Public Function AskDate(ByVal prompt As String, ByVal defaultValue As Date?) As Date? Implements IInteractor.AskDate
        Dim response = AskString(prompt, AddressOf IsDate, If(defaultValue.HasValue, CStr(defaultValue.Value), ""))
        If String.IsNullOrEmpty(response) Then
            Return defaultValue
        Else
            Return CDate(response)
        End If
    End Function

    Private Function IsBoolean(s As String) As Boolean
        Return {"0", "1", "true", "false"}.Contains(s.ToLower)
    End Function

    Public Function AskBoolean(ByVal prompt As String, ByVal defaultValue As Boolean?) As Boolean? Implements IInteractor.AskBoolean
        Dim response = AskString(prompt, AddressOf IsBoolean, If(defaultValue.HasValue, CStr(defaultValue.Value), ""))
        If String.IsNullOrEmpty(response) Then
            Return defaultValue
        Else
            Return CBool(response)
        End If
    End Function

    MustOverride Function GetPropertyDescription(ByVal pi As System.Reflection.PropertyInfo, ByVal obj As Object) As String Implements IInteractor.GetPropertyDescription

    Public MustOverride Sub ShowList(Of T As Class)(ByVal title As String, ByVal values As IEnumerable(Of T), ByVal toString As Func(Of T, String), hideIfEmpty As Boolean) Implements IInteractor.ShowList
    Public MustOverride Sub ShowNumberedList(Of T As Class)(ByVal title As String, ByVal values As IEnumerable(Of T), ByVal toString As Func(Of T, String), hideIfEmpty As Boolean) Implements IInteractor.ShowNumberedList
    Public MustOverride Sub ShowKeyedList(Of T As Class)(ByVal title As String, ByVal values As IEnumerable(Of T), key As Func(Of T, String), ByVal toString As Func(Of T, String), hideIfEmpty As Boolean) Implements IInteractor.ShowKeyedList

    Public MustOverride Function AskOne(Of T As Class)(ByVal prompt As String, ByVal values As IEnumerable(Of T), ByVal toString As Func(Of T, String), ByVal defaultValue As T, ByVal selectedList As List(Of T)) As T Implements IInteractor.AskOne

    Public MustOverride Function AskOneByKey(Of T As Class)(ByVal prompt As String, ByVal values As IEnumerable(Of T), key As Func(Of T, String), ByVal toString As Func(Of T, String), ByVal defaultValue As T, ByVal selectedList As List(Of T)) As T Implements IInteractor.AskOneByKey

    Public MustOverride Function AskMany(Of T As Class)(ByVal generalPrompt As String, ByVal prompt As String, ByVal values As IEnumerable(Of T), ByVal toString As Func(Of T, String), ByVal initialList As List(Of T)) As List(Of T) Implements IInteractor.AskMany

    Public MustOverride Sub ShowErrors(list As IEnumerable(Of String)) Implements IInteractor.ShowErrors

    Public MustOverride Sub ShowMessage(msg As String) Implements IInteractor.ShowMessage

    Public Function AskContext(ByVal prompt As String, caller As IContext, contexts As IEnumerable(Of IContext)) As IContext Implements IInteractor.AskContext
        If contexts Is Nothing Then
            Dim ctls = New List(Of IContext)

            ctls.Add(Instance(Of IContext))
            ctls.AddRange(Instances(Of IContext))
            contexts = ctls
        End If

        Dim contextName = AskString(prompt)

        Dim ctl As IContext
#If Framework = "net35" Then
        If String.IsNullOrEmpty(contextName.Trim) Then
#Else
        If String.IsNullOrWhiteSpace(contextName) Then
#End If
            Return caller
        Else
            ctl = contexts.SingleOrDefault(Function(ctrl) _
                                                   ctrl.Name = contextName OrElse _
                                                   ctrl.Synonyms().Contains(contextName))
        End If

        If ctl Is Nothing Then
            ShowErrors({String.Format("Could not find context {0}", contextName)})
            Return Nothing
        Else
            Return ctl
        End If
    End Function

    Public Property TokenQueue As Queue(Of String) Implements IInteractor.TokenQueue


    Property CommandPromptSeparator As String = "> " Implements IInteractor.CommandPromptSeparator


    Public Function AskCommand(prompt As String) As String Implements IInteractor.AskCommand
        Return AskStringWithTokenQueue(prompt, Nothing, CommandPromptSeparator)
    End Function

End Class
