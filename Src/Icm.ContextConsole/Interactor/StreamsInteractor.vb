Imports System.IO
Imports Icm.Collections
Imports Icm.Localization
Imports Icm.Tree

''' <summary>
''' Implementation of BaseInteractor using <see cref="TextReader">TextReaders</see> for input and
''' <see cref="TextWriter">TextWriters</see> for output and error.
''' </summary>
''' <remarks></remarks>
Public Class StreamsInteractor
    Implements IInteractor

    Protected Property Reader As TextReader
    Protected Property Writer As TextWriter
    Protected Property ErrorWriter As TextWriter

    Private _indentLevel As Integer
    Private _indentString As String = ""

    Protected locRepo As ILocalizationRepository

    Property PromptSeparator As String = ": " Implements IInteractor.PromptSeparator

    Property TokenQueue As Queue(Of String) Implements IInteractor.TokenQueue


    Property CommandPromptSeparator As String = "> " Implements IInteractor.CommandPromptSeparator

    Public Sub New(locRepo As ILocalizationRepository)
        MyClass.New(locRepo, Console.In, Console.Out, Console.Error, New Queue(Of String))
    End Sub

    Public Sub New(locRepo As ILocalizationRepository, inputtr As TextReader, outputtw As TextWriter, errortw As TextWriter, tokenQueue As Queue(Of String))
        MyBase.New()

        Reader = inputtr
        Writer = outputtw
        ErrorWriter = errortw
        Me.TokenQueue = tokenQueue
        Me.locRepo = locRepo
    End Sub


    Sub SetLocalizationRepo(locRepo As ILocalizationRepository) Implements IInteractor.SetLocalizationRepo
        Me.locRepo = locRepo
    End Sub


    Public Function AskCommand(prompt As String) As String Implements IInteractor.AskCommand
        Return AskStringWithTokenQueue(prompt, Nothing, CommandPromptSeparator)
    End Function

    Private Function AskStringWithTokenQueue(ByVal prompt As String, ByVal defaultValue As String, promptSeparator As String) As String
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
            response = AskStringWithTokenQueue(prompt, defaultValue, PromptSeparator)
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

    Property IndentLevel As Integer
        Get
            Return _indentLevel
        End Get
        Set(value As Integer)
            _indentLevel = value
            _indentString = New String(" "c, _indentLevel * 2)
        End Set
    End Property

    Protected ReadOnly Property IndentString() As String
        Get
            Return _indentString
        End Get

    End Property

    Protected Function AskStringWithoutTokenQueue(prompt As String, defaultValue As String, promptSeparator As String) As String
        If String.IsNullOrEmpty(defaultValue) Then
            Writer.Write("{0}{1}", prompt, promptSeparator)
        Else
            Writer.Write("{0} (default: {1}){2}", prompt, defaultValue, promptSeparator)
        End If
        Return Reader.ReadLine
    End Function

    Private Sub ShowListAux(ByVal title As String, ByVal list As IEnumerable(Of String), Optional hideIfEmpty As Boolean = False)
        If hideIfEmpty AndAlso list.Count = 0 Then
            Exit Sub
        End If
        Writer.WriteLine()
        Writer.WriteLine(title)
        Writer.WriteLine(New String("-"c, title.Length))
        For i = 0 To list.Count - 1
            Writer.WriteLine(IndentString & list(i))
        Next
    End Sub

    Public Sub ShowList(Of T As Class)(ByVal title As String, ByVal values As IEnumerable(Of T), ByVal toString As Func(Of T, String), hideIfEmpty As Boolean) Implements IInteractor.ShowList
        ShowListAux(title, values.Select(Function(item) toString(item)), hideIfEmpty)
    End Sub

    Private Sub WriteList(Of T As Class)(ByVal values As IEnumerable(Of T), ByVal key As Func(Of T, String), ByVal toString As Func(Of T, String), ByVal selectedList As List(Of T))
        If selectedList Is Nothing Then
            For Each value In values
                Writer.WriteLine(GetListItem(value, key, toString, selected:=False))
            Next
        Else
            For Each value In values
                Writer.WriteLine(GetListItem(value, key, toString, selected:=selectedList.Contains(value)))
            Next
        End If
    End Sub

    Public Sub ShowKeyedList(Of T As Class)(ByVal title As String, ByVal values As IEnumerable(Of T), key As Func(Of T, String), ByVal toString As Func(Of T, String), hideIfEmpty As Boolean) Implements IInteractor.ShowKeyedList
        ShowListAux(title, values.Select(Function(item) GetListItem(item, key, toString, selected:=False)), hideIfEmpty)
    End Sub

    Private Function GetListItem(Of T As Class)(ByVal value As T, ByVal key As Func(Of T, String), ByVal toString As Func(Of T, String), ByVal selected As Boolean) As String
        If selected Then
            Return String.Format(">{0}. {1}", key(value), toString(value))
        Else
            Return String.Format(" {0}. {1}", key(value), toString(value))
        End If
    End Function

    Public Function AskOneByKey(Of T As Class)(ByVal prompt As String, ByVal values As IEnumerable(Of T), key As Func(Of T, String), ByVal toString As Func(Of T, String), ByVal defaultValue As T, ByVal selectedList As List(Of T)) As T Implements IInteractor.AskOneByKey
        Dim response As String
        If values.Count = 0 Then
            Throw New ArgumentException("Values collection cannot be empty")
        End If
        If defaultValue IsNot Nothing AndAlso Not values.Any(Function(obj) key(obj) = key(defaultValue)) Then
            Throw New ArgumentException(String.Format("The default value (key {0}) cannot be found in the values collection", key(defaultValue)))
        End If

        If TokenQueue.Count <> 0 Then
            response = TokenQueue.Dequeue
        ElseIf values.Count = 1 Then
            Return values.Single
        Else
            WriteList(values, key, toString, selectedList)
            If defaultValue Is Nothing Then
                response = AskString(prompt)
            Else
                response = AskString(prompt, key(defaultValue))
            End If
        End If
        If response IsNot Nothing Then
            Return values.Single(Function(obj) key(obj) = response)
        Else
            Return defaultValue
        End If
    End Function

    Public Sub ShowNumberedList(Of T As Class)(title As String, values As IEnumerable(Of T), toString As Func(Of T, String), hideIfEmpty As Boolean) Implements IInteractor.ShowNumberedList
        If hideIfEmpty AndAlso values.Count = 0 Then
            Exit Sub
        End If
        Writer.WriteLine(title)
        Writer.WriteLine(New String("-"c, title.Length))
        For i = 1 To values.Count
            Dim iFor = i
            Writer.WriteLine(GetListItem(values(i - 1), Function(obj) iFor.ToString, toString, selected:=False))
        Next
    End Sub

    Public Function AskOne(Of T As Class)(prompt As String, values As IEnumerable(Of T), toString As Func(Of T, String), defaultValue As T, selectedList As List(Of T)) As T Implements IInteractor.AskOne
        Dim defaultIndex As Integer? = Nothing
        For i = 1 To values.Count
            Dim iFor = i
            Dim value = values(i - 1)
            Dim key = Function(obj As T) iFor.ToString
            WriteList(values, key, toString, selectedList)
            If defaultValue Is values(i - 1) Then
                defaultIndex = i
            End If
        Next
        Dim response = AskInteger(prompt, defaultIndex)
        If response.HasValue Then
            Return values(response.Value - 1)
        Else
            Return defaultValue
        End If
    End Function

    Public Function AskMany(Of T As Class)(generalPrompt As String, prompt As String, values As IEnumerable(Of T), toString As Func(Of T, String), initialList As List(Of T)) As List(Of T) Implements IInteractor.AskMany
        Dim result = initialList
        Writer.WriteLine(generalPrompt)
        Do
            Dim item = AskOne(prompt, values, toString, Nothing, initialList)
            If item Is Nothing Then
                Return result
            ElseIf result.Contains(item) Then
                result.Remove(item)
            Else
                result.Add(item)
            End If
        Loop
    End Function

    Private Function GetPropertyDescription(pi As System.Reflection.PropertyInfo, obj As Object) As String Implements IInteractor.GetPropertyDescription
        Dim result As String
        Dim value As String
        If pi.GetIndexParameters.Count > 0 Then
            value = "INDEXED"
        Else
            Dim val As Object = Nothing
            Try
                val = pi.GetValue(obj, Nothing)
            Catch ex As System.Reflection.TargetInvocationException
                value = "ERROR"
            End Try
            If val IsNot Nothing Then
                If val.GetType.Name = "String" Then
                    value = String.Format("""{0}""", val)
                ElseIf GetType(IEnumerable).IsAssignableFrom(val.GetType) Then
                    Dim enumerable = CType(val, IEnumerable)
                    Dim list As New List(Of String)()
                    For Each obj In enumerable
                        list.Add("   " & obj.ToString)
                    Next
                    value = vbCrLf & list.JoinStr(vbCrLf)
                Else
                    value = val.ToString
                End If
            Else
                value = "NULL"
            End If
        End If

        If pi.PropertyType.IsPrimitive Then
            result = String.Format("{0}: {1} ({2})", pi.Name, value, pi.PropertyType.Name)
        Else
            result = String.Format("{0}: {1}", pi.Name, value)
        End If
        Return result
    End Function

    Public Sub ShowErrors(list As IEnumerable(Of String)) Implements IInteractor.ShowErrors
        For Each errorString In list
            ErrorWriter.WriteLine(errorString)
        Next
        ErrorWriter.WriteLine()
    End Sub

    Public Sub ShowMessage(msg As String) Implements IInteractor.ShowMessage
        Writer.WriteLine(msg)
    End Sub

End Class
