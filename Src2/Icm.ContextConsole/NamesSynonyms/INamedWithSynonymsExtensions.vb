Imports System.Runtime.CompilerServices

Public Module INamedWithSynonymsExtensions

    <Extension>
    Public Function IsNamed(obj As INamedWithSynonyms, name As String) As Boolean
        Dim lowname = name.ToLower
        Return obj.Name = lowname OrElse obj.Synonyms.Contains(lowname)
    End Function

End Module