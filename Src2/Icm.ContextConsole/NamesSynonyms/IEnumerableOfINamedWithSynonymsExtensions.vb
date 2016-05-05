Imports System.Runtime.CompilerServices

Public Module IEnumerableOfINamedWithSynonymsExtensions

    ''' <summary>
    ''' Gets an item by name, taking into account its synonyms.
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <param name="list"></param>
    ''' <param name="name"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension>
    Public Function GetNamedItem(Of T As INamedWithSynonyms)(list As IEnumerable(Of T), name As String) As T
        Return list.SingleOrDefault(
            Function(ctrl) _
                ctrl.Name = name OrElse _
                ctrl.Synonyms().Contains(name))
    End Function

End Module