Imports Icm.Reflection

Public Class MethodInfoAction
    Implements IAction

    Private ReadOnly _minfo As System.Reflection.MethodInfo
    Private _synonyms As List(Of String)
    Private ReadOnly _ctl As IContext
    Private ReadOnly _locKey As String
    Private ReadOnly _isInternal As Boolean

    Public Sub New(minf As System.Reflection.MethodInfo, ctl As IContext)
        _minfo = minf
        _ctl = ctl
        _isInternal = minf.DeclaringType.Assembly Is Me.GetType.Assembly
        Dim declaringTypeName = minf.DeclaringType.Name
        _locKey = String.Format("{0}_{1}",
                                declaringTypeName.Substring(0, declaringTypeName.Length - "Context".Length).ToLower,
                                minf.Name.ToLower)
    End Sub


    Public Function Name() As String Implements IAction.Name
        Return _minfo.Name.ToLower
    End Function

    Public Function Synonyms() As System.Collections.Generic.IEnumerable(Of String) Implements IAction.Synonyms
        If _synonyms Is Nothing Then
            _synonyms = New List(Of String)
            Dim synAttrs = _minfo.GetAttributes(Of SynonymAttribute)(True)
            For Each synAttr In synAttrs
                _synonyms.AddRange(synAttr.Synonyms.Select(Function(str) str.ToLower))
            Next

        End If
        Return _synonyms
    End Function

    Public Sub Execute() Implements IAction.Execute
        Dim parameters = _minfo.GetParameters

        If parameters Is Nothing OrElse parameters.Count = 0 Then
            _minfo.Invoke(_ctl, Nothing)
            Exit Sub
        End If

        Dim arguments() As Object
        ReDim arguments(parameters.Count - 1)

        For i = 0 To parameters.Count - 1
            arguments(i) = AskValue(parameters(i))
        Next

        _minfo.Invoke(_ctl, arguments)
    End Sub

    Private Shared Function AskValue(parinfo As System.Reflection.ParameterInfo) As Object
        ' TODO
        Throw New NotImplementedException("")
        'If parinfo.ParameterType Is GetType(String) Then

        'ElseIf parinfo.ParameterType.IsGenericType AndAlso parinfo.ParameterType.GetGenericTypeDefinition().Equals(GetType(Lazy(Of ))) Then

        'ElseIf parinfo.ParameterType Is GetType(String) Then
        'ElseIf parinfo.ParameterType Is GetType(String) Then
        'ElseIf parinfo.ParameterType Is GetType(String) Then
        'ElseIf parinfo.ParameterType Is GetType(String) Then
        'ElseIf parinfo.ParameterType Is GetType(String) Then
        'ElseIf parinfo.ParameterType Is GetType(String) Then
        'Else
        '    Throw New ArgumentException("Cannot ask the value of a parameter of type " & parinfo.ParameterType.FullName, "parinfo")
        'End If
    End Function

    Public Function LocalizationKey() As String Implements IAction.LocalizationKey
        Return _locKey
    End Function

    Public Function IsInternal() As Boolean Implements IAction.IsInternal
        Return _isInternal
    End Function
End Class
