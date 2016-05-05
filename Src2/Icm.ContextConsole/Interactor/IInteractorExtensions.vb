Imports System.Runtime.CompilerServices

Public Module IInteractorExtensions

    <Extension>
    Function AskString(interactor As IInteractor, ByVal prompt As String) As String
        Return interactor.AskString(prompt, Nothing, Nothing)
    End Function

    <Extension>
    Function AskString(interactor As IInteractor, ByVal prompt As String, ByVal defaultValue As String) As String
        Return interactor.AskString(prompt, Nothing, defaultValue)
    End Function

    <Extension>
    Function AskString(interactor As IInteractor, ByVal prompt As String, ByVal validation As Func(Of String, Boolean)) As String
        Return interactor.AskString(prompt, validation, Nothing)
    End Function

    <Extension>
    Function AskInteger(interactor As IInteractor, ByVal prompt As String) As Integer?
        Return interactor.AskInteger(prompt, Nothing)
    End Function

    <Extension>
    Function AskOne(Of T As Class)(interactor As IInteractor, ByVal prompt As String, ByVal values As IEnumerable(Of T)) As T
        Return interactor.AskOne(prompt, values, Function(obj) obj.ToString)
    End Function

    <Extension>
    Function AskOne(Of T As Class)(interactor As IInteractor, ByVal prompt As String, ByVal values As IEnumerable(Of T), ByVal toString As Func(Of T, String)) As T
        Return interactor.AskOne(prompt, values, toString, Nothing, New List(Of T)())
    End Function

    <Extension>
    Function AskOneByKey(Of T As Class)(interactor As IInteractor, ByVal prompt As String, ByVal values As IEnumerable(Of T), ByVal key As Func(Of T, String)) As T
        Return interactor.AskOneByKey(prompt, values, key, Function(obj) obj.ToString)
    End Function

    <Extension>
    Function AskOneByKey(Of T As Class)(interactor As IInteractor, ByVal prompt As String, ByVal values As IEnumerable(Of T), ByVal key As Func(Of T, String), ByVal toString As Func(Of T, String)) As T
        Return interactor.AskOneByKey(prompt, values, key, toString, Nothing, New List(Of T)())
    End Function

    <Extension>
    Function AskMany(Of T As Class)(interactor As IInteractor, ByVal generalPrompt As String, ByVal prompt As String, ByVal values As IEnumerable(Of T)) As List(Of T)
        Return interactor.AskMany(generalPrompt, prompt, values, Function(obj) obj.ToString, New List(Of T)())
    End Function

    <Extension>
    Function AskMany(Of T As Class)(interactor As IInteractor, ByVal generalPrompt As String, ByVal prompt As String, ByVal values As IEnumerable(Of T), ByVal initialList As List(Of T)) As List(Of T)
        Return interactor.AskMany(generalPrompt, prompt, values, Function(obj) obj.ToString, initialList)
    End Function

    <Extension>
    Function AskMany(Of T As Class)(interactor As IInteractor, ByVal generalPrompt As String, ByVal prompt As String, ByVal values As IEnumerable(Of T), ByVal toString As Func(Of T, String)) As List(Of T)
        Return interactor.AskMany(generalPrompt, prompt, values, toString, New List(Of T)())
    End Function

    <Extension>
    Sub ShowProperties(Of T As Class)(interactor As IInteractor, ByVal title As String, ByVal obj As T)
        interactor.ShowList(Of System.Reflection.PropertyInfo)(title, obj.GetType.GetProperties(), Curry2(Of System.Reflection.PropertyInfo, Object, String)(AddressOf interactor.GetPropertyDescription, obj), False)
    End Sub

    <Extension>
    Sub ShowList(Of T As Class)(interactor As IInteractor, ByVal title As String, ByVal values As IEnumerable(Of T))
        interactor.ShowList(title, values, Function(obj) obj.ToString, False)
    End Sub

    <Extension>
    Sub ShowList(Of T As Class)(interactor As IInteractor, ByVal title As String, ByVal values As IEnumerable(Of T), hideIfEmpty As Boolean)
        interactor.ShowList(title, values, Function(obj) obj.ToString, hideIfEmpty)
    End Sub

    <Extension>
    Sub ShowList(Of T As Class)(interactor As IInteractor, ByVal title As String, ByVal values As IEnumerable(Of T), ByVal toString As Func(Of T, String))
        interactor.ShowList(title, values, toString, False)
    End Sub

    <Extension>
    Sub ShowNumberedList(Of T As Class)(interactor As IInteractor, ByVal title As String, ByVal values As IEnumerable(Of T), ByVal toString As Func(Of T, String))
        interactor.ShowNumberedList(title, values, toString, False)
    End Sub

    <Extension>
    Sub ShowKeyedList(Of T As Class)(interactor As IInteractor, ByVal title As String, ByVal values As IEnumerable(Of T), ByVal key As Func(Of T, String), ByVal toString As Func(Of T, String))
        interactor.ShowKeyedList(title, values, key, toString, False)
    End Sub

    <Extension>
    Sub ShowNumberedList(Of T As Class)(interactor As IInteractor, ByVal title As String, ByVal values As IEnumerable(Of T), Optional hideIfEmpty As Boolean = False)
        interactor.ShowNumberedList(title, values, Function(obj) obj.ToString, hideIfEmpty)
    End Sub

    <Extension>
    Sub ShowKeyedList(Of T As Class)(interactor As IInteractor, ByVal title As String, ByVal values As IEnumerable(Of T), ByVal key As Func(Of T, String), Optional hideIfEmpty As Boolean = False)
        interactor.ShowKeyedList(title, values, key, Function(obj) obj.ToString, hideIfEmpty)
    End Sub


    Private Function IsBoolean(s As String) As Boolean
        Return {"0", "1", "true", "false"}.Contains(s.ToLower)
    End Function

    <Extension>
    Public Function AskBoolean(interactor As IInteractor, ByVal prompt As String, ByVal defaultValue As Boolean?) As Boolean?
        Dim response = interactor.AskString(prompt, AddressOf IsBoolean, If(defaultValue.HasValue, CStr(defaultValue.Value), ""))
        If String.IsNullOrEmpty(response) Then
            Return defaultValue
        Else
            Return CBool(response)
        End If
    End Function

    <Extension>
    Public Function AskContext(interactor As IInteractor, ByVal prompt As String, caller As IContext, contexts As IEnumerable(Of IContext)) As IContext
        Dim contextName = interactor.AskString(prompt)

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
            interactor.ShowErrors({String.Format("Could not find context {0}", contextName)})
            Return Nothing
        Else
            Return ctl
        End If
    End Function

    <Extension>
    Public Function AskInteger(interactor As IInteractor, ByVal prompt As String, ByVal defaultValue As Integer?) As Integer?
        Return interactor.AskLiteral(Of Integer?)(prompt, Function(obj) CInt(obj), defaultValue)
    End Function

    <Extension>
    Public Function AskLiteral(Of T)(interactor As IInteractor, ByVal prompt As String, convert As Func(Of String, T), ByVal defaultValue As T) As T
        Dim response = interactor.AskString(prompt, AddressOf IsNumeric, If(defaultValue IsNot Nothing, defaultValue.ToString, ""))
        If String.IsNullOrEmpty(response) Then
            Return defaultValue
        Else
            Return convert(response)
        End If
    End Function

    <Extension>
    Public Function AskDate(interactor As IInteractor, ByVal prompt As String, ByVal defaultValue As Date?) As Date?
        Dim response = interactor.AskString(prompt, AddressOf IsDate, If(defaultValue.HasValue, CStr(defaultValue.Value), ""))
        If String.IsNullOrEmpty(response) Then
            Return defaultValue
        Else
            Return CDate(response)
        End If
    End Function

End Module
