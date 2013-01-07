Imports NUnit.Framework
Imports Icm.ContextConsole
Imports System.IO
Imports Icm.IO

Public Module StreamsInteractorHelper

    Private Function GetInputstring(ByVal input As IEnumerable(Of String)) As String
        Dim inputstring As String
        Using inputgen As New StringWriter()
            If input IsNot Nothing Then
                For Each line In input
                    inputgen.WriteLine(line)
                Next
            End If
            inputstring = inputgen.ToString
        End Using
        Return inputstring
    End Function

    Public Function BuildInteractor(input As IEnumerable(Of String)) As StreamsInteractor
        Return New StreamsInteractor(TextReaderFactory.FromString(GetInputstring(input)), New StringWriter, New StringWriter, New Queue(Of String))
    End Function

    Public Function BuildInteractor(input As IEnumerable(Of String),
        output As StringWriter,
        errorstr As StringWriter) As StreamsInteractor
        Return New StreamsInteractor(TextReaderFactory.FromString(GetInputstring(input)), output, errorstr, New Queue(Of String))
    End Function

    Public Function BuildInteractor(input As IEnumerable(Of String),
        output As StringWriter,
        errorstr As StringWriter,
        tokenQueue As Queue(Of String)) As StreamsInteractor
        Return New StreamsInteractor(TextReaderFactory.FromString(GetInputstring(input)), output, errorstr, tokenQueue)
    End Function
End Module
