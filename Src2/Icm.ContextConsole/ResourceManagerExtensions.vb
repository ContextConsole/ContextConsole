Imports System.Runtime.CompilerServices
Imports Icm.Localization
Imports System.Resources

Public Module ResourceManagerExtensions

    <Extension>
    Public Function ToRepository(ByVal manager As ResourceManager) As ILocalizationRepository
        Return New ResourceLocalizationRepository(manager)
    End Function

End Module
