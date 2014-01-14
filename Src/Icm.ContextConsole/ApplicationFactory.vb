Imports Icm.Localization
Imports System.Resources
Imports Icm.Tree

''' <summary>
''' Various methods for building and running an Icm.ContextConsole application.
''' </summary>
''' <remarks></remarks>
Public Module ApplicationStarter

    Public Sub Start(Of TMain As IContext)(locRepo As ILocalizationRepository)
        StandardApplication.Create(Of TMain)(extLocRepo:=locRepo).Run()
    End Sub

    Public Sub Start(Of TMain As IContext)(resourceManager As ResourceManager)
        Start(Of TMain)(resourceManager.ToRepository)
    End Sub

    Public Sub Start(rootContextType As Type, resourceManager As ResourceManager)
        Dim app = New StandardApplication(rootContextType, extLocRepo:=resourceManager.ToRepository)
        app.Run()
    End Sub

End Module
