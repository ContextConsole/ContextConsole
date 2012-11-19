Imports System.Linq
Imports Icm.Reflection

''' <summary>
''' The base interactor provide some trivial implementations of some of the overloads of IInteractor.
''' </summary>
''' <remarks></remarks>
Public MustInherit Class BaseInteractor
    Implements IInteractor

    Public Function AskString(ByVal prompt As String) As String Implements IInteractor.AskString
        Return AskString(prompt, Nothing, Nothing)
    End Function

    Public MustOverride Function AskString(ByVal prompt As String, ByVal defaultValue As String) As String Implements IInteractor.AskString

    Public Function AskString(ByVal prompt As String, ByVal validation As Func(Of String, Boolean)) As String Implements IInteractor.AskString
        Return AskString(prompt, validation, Nothing)
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

    Public Function AskInteger(ByVal prompt As String, Optional ByVal defaultValue As Integer? = Nothing) As Integer? Implements IInteractor.AskInteger
        Return AskLiteral(Of Integer?)(prompt, Function(obj) CInt(obj), defaultValue)
    End Function

    Public Function AskLiteral(Of T)(ByVal prompt As String, convert As Func(Of String, T), Optional ByVal defaultValue As T = Nothing) As T Implements IInteractor.AskLiteral
        Dim response = AskString(prompt, AddressOf IsNumeric, If(defaultValue IsNot Nothing, defaultValue.ToString, ""))
        If String.IsNullOrEmpty(response) Then
            Return defaultValue
        Else
            Return convert(response)
        End If
    End Function

    Public Function AskDate(ByVal prompt As String, Optional ByVal defaultValue As Date? = Nothing) As Date? Implements IInteractor.AskDate
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

    Public Function AskBoolean(ByVal prompt As String, Optional ByVal defaultValue As Boolean? = Nothing) As Boolean? Implements IInteractor.AskBoolean
        Dim response = AskString(prompt, AddressOf IsBoolean, If(defaultValue.HasValue, CStr(defaultValue.Value), ""))
        If String.IsNullOrEmpty(response) Then
            Return defaultValue
        Else
            Return CBool(response)
        End If
    End Function

    MustOverride Function GetPropertyDescription(pi As System.Reflection.PropertyInfo, obj As Object) As String

    Public Sub ShowProperties(Of T As Class)(ByVal title As String, obj As T) Implements IInteractor.ShowProperties
        ShowList(Of System.Reflection.PropertyInfo)(title, obj.GetType.GetProperties(), Curry2(Of System.Reflection.PropertyInfo, Object, String)(AddressOf GetPropertyDescription, obj))
    End Sub

    Public Sub ShowList(Of T As Class)(ByVal title As String, ByVal values As IEnumerable(Of T)) Implements IInteractor.ShowList
        ShowList(title, values, Function(obj) obj.ToString)
    End Sub

    Public Sub ShowNumberedList(Of T As Class)(ByVal title As String, ByVal values As IEnumerable(Of T)) Implements IInteractor.ShowNumberedList
        ShowNumberedList(title, values, Function(obj) obj.ToString)
    End Sub

    Public Sub ShowKeyedList(Of T As Class)(ByVal title As String, ByVal values As IEnumerable(Of T), key As Func(Of T, String)) Implements IInteractor.ShowKeyedList
        ShowKeyedList(title, values, key, Function(obj) obj.ToString)
    End Sub

    Public MustOverride Sub ShowList(Of T As Class)(ByVal title As String, ByVal values As IEnumerable(Of T), ByVal toString As Func(Of T, String)) Implements IInteractor.ShowList
    Public MustOverride Sub ShowNumberedList(Of T As Class)(ByVal title As String, ByVal values As IEnumerable(Of T), ByVal toString As Func(Of T, String)) Implements IInteractor.ShowNumberedList
    Public MustOverride Sub ShowKeyedList(Of T As Class)(ByVal title As String, ByVal values As IEnumerable(Of T), key As Func(Of T, String), ByVal toString As Func(Of T, String)) Implements IInteractor.ShowKeyedList

    Public Function AskOne(Of T As Class)(ByVal prompt As String, ByVal values As IEnumerable(Of T)) As T Implements IInteractor.AskOne
        Return AskOne(prompt, values, Function(obj) obj.ToString)
    End Function

    Public Function AskOne(Of T As Class)(ByVal prompt As String, ByVal values As IEnumerable(Of T), ByVal toString As Func(Of T, String)) As T Implements IInteractor.AskOne
        Return AskOne(prompt, values, toString, Nothing, New List(Of T)())
    End Function

    Public MustOverride Function AskOne(Of T As Class)(ByVal prompt As String, ByVal values As IEnumerable(Of T), ByVal toString As Func(Of T, String), ByVal defaultValue As T, ByVal selectedList As List(Of T)) As T Implements IInteractor.AskOne

    Public Function AskOneByKey(Of T As Class)(ByVal prompt As String, ByVal values As IEnumerable(Of T), key As Func(Of T, String)) As T Implements IInteractor.AskOneByKey
        Return AskOneByKey(prompt, values, key, Function(obj) obj.ToString)
    End Function

    Public Function AskOneByKey(Of T As Class)(ByVal prompt As String, ByVal values As IEnumerable(Of T), key As Func(Of T, String), ByVal toString As Func(Of T, String)) As T Implements IInteractor.AskOneByKey
        Return AskOneByKey(prompt, values, key, toString, Nothing, New List(Of T)())
    End Function

    Public MustOverride Function AskOneByKey(Of T As Class)(ByVal prompt As String, ByVal values As IEnumerable(Of T), key As Func(Of T, String), ByVal toString As Func(Of T, String), ByVal defaultValue As T, ByVal selectedList As List(Of T)) As T Implements IInteractor.AskOneByKey

    Public Function AskMany(Of T As Class)(ByVal generalPrompt As String, ByVal prompt As String, ByVal values As IEnumerable(Of T)) As List(Of T) Implements IInteractor.AskMany
        Return AskMany(generalPrompt, prompt, values, Function(obj) obj.ToString, New List(Of T)())
    End Function

    Public Function AskMany(Of T As Class)(ByVal generalPrompt As String, ByVal prompt As String, ByVal values As IEnumerable(Of T), ByVal initialList As List(Of T)) As List(Of T) Implements IInteractor.AskMany
        Return AskMany(generalPrompt, prompt, values, Function(obj) obj.ToString, initialList)
    End Function

    Public Function AskMany(Of T As Class)(ByVal generalPrompt As String, ByVal prompt As String, ByVal values As IEnumerable(Of T), ByVal toString As Func(Of T, String)) As List(Of T) Implements IInteractor.AskMany
        Return AskMany(generalPrompt, prompt, values, toString, New List(Of T)())
    End Function

    Public MustOverride Function AskMany(Of T As Class)(ByVal generalPrompt As String, ByVal prompt As String, ByVal values As IEnumerable(Of T), ByVal toString As Func(Of T, String), ByVal initialList As List(Of T)) As List(Of T) Implements IInteractor.AskMany

    Public MustOverride Sub ShowErrors(list As IEnumerable(Of String)) Implements IInteractor.ShowErrors

    Public MustOverride Sub ShowMessage(msg As String) Implements IInteractor.ShowMessage

    Public MustOverride Sub EnqueueToken(tok As String) Implements IInteractor.EnqueueToken

End Class
