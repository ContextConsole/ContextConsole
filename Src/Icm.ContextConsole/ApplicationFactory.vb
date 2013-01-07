Imports Icm.Localization
Imports System.Resources


Public Module ApplicationFactory

    Public Sub Start(Of TMain As IContext)(locRepo As ILocalizationRepository)
        Initialize(GetType(TMain), locRepo, True)
        Run()
    End Sub

    Public Sub Start(Of TMain As IContext)(resourceManager As ResourceManager)
        Start(Of TMain)(New ResourceLocalizationRepository(resourceManager))
    End Sub

    Public Sub Start(fctlType As Type, resourceManager As ResourceManager)
        Initialize(fctlType, New ResourceLocalizationRepository(resourceManager), True)
        Run()
    End Sub

    Public Sub Initialize(
                          frontContextType As Type,
                          initialLocRepo As ILocalizationRepository,
                          loadAllBackContexts As Boolean,
                          ParamArray additionalModules() As Global.Ninject.Modules.INinjectModule)
        Icm.Ninject.Kernel.Load({New StandardNinjectModule(initialLocRepo, loadAllBackContexts, frontContextType)})
        Icm.Ninject.Kernel.Load(additionalModules)
    End Sub

    Public Sub Start(fctl As IContext, resourceManager As ResourceManager)
        Initialize(fctl, New ResourceLocalizationRepository(resourceManager), True)
        Run()
    End Sub

    Public Sub Initialize(
                          frontContext As IContext,
                          initialLocRepo As ILocalizationRepository,
                          loadAllBackContexts As Boolean,
                          ParamArray additionalModules() As Global.Ninject.Modules.INinjectModule)
        Icm.Ninject.Kernel.Load({New StandardNinjectModule(initialLocRepo, loadAllBackContexts, FrontContext)})
        Icm.Ninject.Kernel.Load(additionalModules)
    End Sub

    Public Sub Start(fctl As ITreeNode(Of IContext), resourceManager As ResourceManager)
        Initialize(fctl, New ResourceLocalizationRepository(resourceManager))
        Run()
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

    Public Sub Run()
        Dim application = Icm.Ninject.Instance(Of IApplication)()
        Do Until application.ExecuteCommand()
        Loop
    End Sub

End Module
