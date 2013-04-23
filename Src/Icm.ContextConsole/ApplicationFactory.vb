Imports Icm.Localization
Imports System.Resources
Imports Icm.Tree

''' <summary>
''' Various methods for building and running an Icm.ContextConsole application.
''' </summary>
''' <remarks></remarks>
Public Module ApplicationFactory

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
                          initialLocRepo As ILocalizationRepository,
                          loadAllBackContexts As Boolean,
                          ParamArray additionalModules() As Global.Ninject.Modules.INinjectModule)
        Icm.Ninject.Kernel.Load({New StandardNinjectModule(initialLocRepo, loadAllBackContexts, frontContextType)})
        Icm.Ninject.Kernel.Load(additionalModules)
    End Sub

    Public Sub Initialize(
                          frontContext As IContext,
                          initialLocRepo As ILocalizationRepository,
                          loadAllBackContexts As Boolean,
                          ParamArray additionalModules() As Global.Ninject.Modules.INinjectModule)
        Icm.Ninject.Kernel.Load({New StandardNinjectModule(initialLocRepo, loadAllBackContexts, FrontContext)})
        Icm.Ninject.Kernel.Load(additionalModules)
    End Sub

    Public Sub Initialize(
                          contextTree As ITreeNode(Of IContext),
                          ParamArray additionalModules() As Global.Ninject.Modules.INinjectModule)
        Icm.Ninject.Kernel.Load({New StandardNinjectModule(contextTree)})
        Icm.Ninject.Kernel.Load(additionalModules)
    End Sub

    Public Sub Initialize(
                          contextTree As ITreeNode(Of IContext),
                          initialLocRepo As ILocalizationRepository,
                          ParamArray additionalModules() As Global.Ninject.Modules.INinjectModule)
        Icm.Ninject.Kernel.Load({New StandardNinjectModule(initialLocRepo, contextTree)})
        Icm.Ninject.Kernel.Load(additionalModules)
    End Sub

    Public Function CreateApp() As IApplication
        Return Icm.Ninject.Instance(Of IApplication)()
    End Function

    Public Sub Run(app As IApplication)
        Do Until app.ExecuteCommand()
        Loop
    End Sub

End Module
