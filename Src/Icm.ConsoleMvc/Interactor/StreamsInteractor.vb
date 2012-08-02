Imports System.IO


''' <summary>
''' Implementation of BaseInteractor using <see cref="TextReader">TextReaders</see> for input and
''' <see cref="TextWriter">TextWriters</see> for output and error.
''' </summary>
''' <remarks></remarks>
Public Class StreamsInteractor
    Inherits BaseInteractor

    Protected Shared Property TokenQueue As New Queue(Of String)

    Protected Property Reader As TextReader
    Protected Property Writer As TextWriter
    Protected Property ErrorWriter As TextWriter

    Private _indentLevel As Integer
    Private _indentString As String = ""

    Public Sub New()
        MyBase.New()

        Reader = Console.In
        Writer = Console.Out
        ErrorWriter = Console.Error
    End Sub

    Public Sub New(inputtr As TextReader, outputtw As TextWriter, errortw As TextWriter)
        MyBase.New()

        Reader = inputtr
        Writer = outputtw
        ErrorWriter = errortw
    End Sub

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

    Public Overrides Function AskString(prompt As String, defaultValue As String) As String
        Dim response As String
        If TokenQueue.Count <> 0 Then
            response = TokenQueue.Dequeue
        Else
            If String.IsNullOrEmpty(defaultValue) Then
                Writer.Write("{0}: ", prompt)
            Else
                Writer.Write("{0} (default: {1}): ", prompt, defaultValue)
            End If
            response = Reader.ReadLine
        End If
        Return response
    End Function

    Public Overrides Sub ShowList(Of T As Class)(ByVal title As String, ByVal values As IEnumerable(Of T), ByVal toString As Func(Of T, String))
        Writer.WriteLine(title)
        Writer.WriteLine(New String("-"c, title.Length))
        For i = 1 To values.Count
            Writer.WriteLine(IndentString & toString(values(i - 1)))
        Next
    End Sub

    Public Overrides Sub ShowKeyedList(Of T As Class)(ByVal title As String, ByVal values As IEnumerable(Of T), key As Func(Of T, String), ByVal toString As Func(Of T, String))
        Writer.WriteLine(title)
        Writer.WriteLine(New String("-"c, title.Length))
        For i = 0 To values.Count - 1
            Writer.WriteLine("{0}. {1}", key(values(i)), toString(values(i)))
        Next
    End Sub

    Public Overrides Function AskOneByKey(Of T As Class)(ByVal prompt As String, ByVal values As IEnumerable(Of T), key As Func(Of T, String), ByVal toString As Func(Of T, String), ByVal defaultValue As T, ByVal selectedList As List(Of T)) As T
        If TokenQueue.Count = 0 Then
            For i = 0 To values.Count - 1
                If selectedList.Contains(values(i)) Then
                    Writer.WriteLine(">{0}. {1}", key(values(i)), toString(values(i)))
                Else
                    Writer.WriteLine(" {0}. {1}", key(values(i)), toString(values(i)))
                End If
            Next
        End If
        Dim response As String
        If defaultValue Is Nothing Then
            response = AskString(prompt)
        Else
            response = AskString(prompt, key(defaultValue))
        End If
        If response IsNot Nothing Then
            Return values.Single(Function(obj) key(obj) = response)
        Else
            Return defaultValue
        End If
    End Function

    Public Overrides Sub ShowNumberedList(Of T As Class)(title As String, values As IEnumerable(Of T), toString As Func(Of T, String))
        Writer.WriteLine(title)
        Writer.WriteLine(New String("-"c, title.Length))
        For i = 1 To values.Count
            Writer.WriteLine("{0}. {1}", i, toString(values(i - 1)))
        Next
    End Sub

    Public Overrides Function AskOne(Of T As Class)(prompt As String, values As IEnumerable(Of T), toString As Func(Of T, String), defaultValue As T, selectedList As List(Of T)) As T
        Dim defaultIndex As Integer? = Nothing
        For i = 1 To values.Count
            If selectedList.Contains(values(i - 1)) Then
                Writer.WriteLine(">{0}. {1}", i, toString(values(i - 1)))
            Else
                Writer.WriteLine(" {0}. {1}", i, toString(values(i - 1)))
            End If
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

    Public Overrides Function AskMany(Of T As Class)(generalPrompt As String, prompt As String, values As IEnumerable(Of T), toString As Func(Of T, String), initialList As List(Of T)) As List(Of T)
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

    Public Overrides Function GetPropertyDescription(pi As System.Reflection.PropertyInfo, obj As Object) As String
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
                    value = vbCrLf & Icm.Collections.JoinStr(list, vbCrLf)
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

    Public Overrides Sub ShowErrors(list As IEnumerable(Of String))
        For Each errorString In list
            ErrorWriter.WriteLine(errorString)
        Next
        ErrorWriter.WriteLine()
    End Sub

    Public Overrides Sub ShowMessage(msg As String)
        Writer.WriteLine(msg)
    End Sub
End Class
