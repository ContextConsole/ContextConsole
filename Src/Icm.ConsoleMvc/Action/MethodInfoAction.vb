Imports Icm.Reflection

Public Class MethodInfoAction
    Implements IAction


    Private ReadOnly minf_ As System.Reflection.MethodInfo
    Private synonyms_ As List(Of String)
    Private ReadOnly ctl_ As IController

    Public Sub New(minf As System.Reflection.MethodInfo, ctl As IController)
        minf_ = minf
        ctl_ = ctl
    End Sub

    Public ReadOnly Property GetMethod As System.Reflection.MethodInfo
        Get
            Return minf_
        End Get
    End Property

    Public ReadOnly Property Name As String Implements IAction.Name
        Get
            Return minf_.Name.ToLower
        End Get
    End Property

    Public ReadOnly Property Synonyms As System.Collections.Generic.IEnumerable(Of String) Implements IAction.Synonyms
        Get
            If synonyms_ Is Nothing Then
                synonyms_ = New List(Of String)
                Dim synAttrs = minf_.GetAttributes(Of SynonymAttribute)(True)
                For Each synAttr In synAttrs
                    synonyms_.AddRange(synAttr.Synonyms.Select(Function(str) str.ToLower))
                Next

            End If
            Return synonyms_
        End Get
    End Property


    Public Function Named(actionName As String) As Boolean Implements IAction.Named
        Return Name = actionName.ToLower OrElse Synonyms().Contains(actionName.ToLower)
    End Function

    Public Sub Execute() Implements IAction.Execute
        Dim parameters = minf_.GetParameters

        If parameters Is Nothing OrElse parameters.Count = 0 Then
            minf_.Invoke(ctl_, Nothing)
            Exit Sub
        End If

        Dim arguments() As Object
        ReDim arguments(parameters.Count - 1)

        For i = 0 To parameters.Count - 1

            arguments(i) = AskValue(parameters(i))
        Next

        minf_.Invoke(ctl_, arguments)
    End Sub

    Private Shared Function AskValue(parinfo As System.Reflection.ParameterInfo) As Object
        ' TODO
        Throw New NotImplementedException("")
        If parinfo.ParameterType Is GetType(String) Then

        ElseIf parinfo.ParameterType.IsGenericType AndAlso parinfo.ParameterType.GetGenericTypeDefinition().Equals(GetType(Lazy(Of ))) Then

        ElseIf parinfo.ParameterType Is GetType(String) Then
        ElseIf parinfo.ParameterType Is GetType(String) Then
        ElseIf parinfo.ParameterType Is GetType(String) Then
        ElseIf parinfo.ParameterType Is GetType(String) Then
        ElseIf parinfo.ParameterType Is GetType(String) Then
        ElseIf parinfo.ParameterType Is GetType(String) Then
        Else
            Throw New ArgumentException("Cannot ask the value of a parameter of type " & parinfo.ParameterType.FullName, "parinfo")
        End If
    End Function

End Class
