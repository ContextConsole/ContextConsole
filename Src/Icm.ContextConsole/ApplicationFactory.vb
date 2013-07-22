Imports Icm.Localization
Imports System.Resources
Imports Icm.Tree

''' <summary>
''' Various methods for building and running an Icm.ContextConsole application.
''' </summary>
''' <remarks></remarks>
Public Module ApplicationFactory

    Public Function Create(Of TMain As IContext)() As IApplication
        Return Create(GetType(TMain))
    End Function

    Public Function Create(fctlType As Type) As IApplication
        Initialize(fctlType, locRepo:=Nothing, loadAllBackContexts:=True)
        Return CreateApp()
    End Function

    Public Function Create(fctl As IContext) As IApplication
        Initialize(fctl, locRepo:=Nothing, loadAllBackContexts:=True)
        Return CreateApp()
    End Function

    Public Function Create(fctl As ITreeNode(Of IContext)) As IApplication
        Initialize(fctl, locRepo:=Nothing)
        Return CreateApp()
    End Function

    Public Function Create(Of TMain As IContext)(locRepo As ILocalizationRepository) As IApplication
        Initialize(GetType(TMain), locRepo, True)
        Return CreateApp()
    End Function

    Public Function Create(Of TMain As IContext)(resourceManager As ResourceManager) As IApplication
        Return Create(Of TMain)(New ResourceLocalizationRepository(resourceManager))
    End Function

    Public Function Create(fctlType As Type, resourceManager As ResourceManager) As IApplication
        Initialize(fctlType, New ResourceLocalizationRepository(resourceManager), True)
        Return CreateApp()
    End Function

    Public Function Create(fctl As IContext, resourceManager As ResourceManager) As IApplication
        Initialize(fctl, New ResourceLocalizationRepository(resourceManager), True)
        Return CreateApp()
    End Function

    Public Function Create(fctl As ITreeNode(Of IContext), resourceManager As ResourceManager) As IApplication
        Initialize(fctl, New ResourceLocalizationRepository(resourceManager))
        Return CreateApp()
    End Function

    Public Sub Start(Of TMain As IContext)(locRepo As ILocalizationRepository)
        Initialize(GetType(TMain), locRepo, True)
        Run(CreateApp)
    End Sub

    Public Sub Start(Of TMain As IContext)(resourceManager As ResourceManager)
        Start(Of TMain)(New ResourceLocalizationRepository(resourceManager))
    End Sub

    Public Sub Start(fctlType As Type, resourceManager As ResourceManager)
        Initialize(fctlType, New ResourceLocalizationRepository(resourceManager), True)
        Run(CreateApp)
    End Sub

    Public Sub Start(fctl As IContext, resourceManager As ResourceManager)
        Initialize(fctl, New ResourceLocalizationRepository(resourceManager), True)
        Run(CreateApp)
    End Sub

    Public Sub Start(fctl As ITreeNode(Of IContext), resourceManager As ResourceManager)
        Initialize(fctl, New ResourceLocalizationRepository(resourceManager))
        Run(CreateApp)
    End Sub

    Public Sub Initialize(
                          frontContextType As Type,
                          locRepo As ILocalizationRepository,
                          loadAllBackContexts As Boolean)
        Icm.Ninject.Kernel.Load({New StandardNinjectModule(locRepo, loadAllBackContexts, frontContextType)})
    End Sub

    Public Sub Initialize(
                          frontContext As IContext,
                          locRepo As ILocalizationRepository,
                          loadAllBackContexts As Boolean)
        Icm.Ninject.Kernel.Load({New StandardNinjectModule(locRepo, loadAllBackContexts, frontContext)})
    End Sub

    Public Sub Initialize(
                          contextTree As ITreeNode(Of IContext))
        Icm.Ninject.Kernel.Load({New StandardNinjectModule(contextTree)})
    End Sub

    Public Sub Initialize(
                          contextTree As ITreeNode(Of IContext),
                          locRepo As ILocalizationRepository)
        Icm.Ninject.Kernel.Load({New StandardNinjectModule(locRepo, contextTree)})
    End Sub

    Public Function CreateApp() As IApplication
        Return Icm.Ninject.Instance(Of IApplication)()
    End Function

    Public Sub Run(app As IApplication)
        Do Until app.ExecuteCommand()
        Loop
    End Sub

End Module
