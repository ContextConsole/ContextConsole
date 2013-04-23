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

End Module
