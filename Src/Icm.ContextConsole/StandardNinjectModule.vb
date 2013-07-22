Imports Icm.Reflection
Imports Icm.Collections
Imports Icm.Localization
Imports System.Resources
Imports Icm.Tree

Public Class StandardNinjectModule
    Inherits Global.Ninject.Modules.NinjectModule

    Private ReadOnly _loadAllBackContexts As Boolean
    Private ReadOnly _frontContextType As Type
    Private ReadOnly _frontContext As IContext
    Private ReadOnly _initialLocRepo As ILocalizationRepository
    Private ReadOnly _contextRootNode As ITreeNode(Of IContext)
    Private ReadOnly _typeRootNode As TreeNode(Of Type)

    Public Sub New(contextRootNode As ITreeNode(Of IContext))
        _contextRootNode = contextRootNode
        _initialLocRepo = New DictionaryLocalizationRepository
    End Sub

    Public Sub New(locRepo As ILocalizationRepository, contextRootNode As ITreeNode(Of IContext))
        _contextRootNode = contextRootNode
        _initialLocRepo = locRepo
    End Sub


    Public Sub New(locRepo As ILocalizationRepository, typeRootNode As TreeNode(Of Type))
        _typeRootNode = typeRootNode
        _initialLocRepo = locRepo
    End Sub

    Public Sub New(loadAllBackContexts As Boolean, frontContextType As Type)
        _loadAllBackContexts = loadAllBackContexts
        _frontContextType = frontContextType
        _initialLocRepo = New DictionaryLocalizationRepository
    End Sub

    Public Sub New(loadAllBackContexts As Boolean, frontContext As IContext)
        _loadAllBackContexts = loadAllBackContexts
        _frontContext = frontContext
        _initialLocRepo = New DictionaryLocalizationRepository
    End Sub

    Public Sub New(locRepo As ILocalizationRepository, loadAllBackContexts As Boolean, frontContextType As Type)
        _loadAllBackContexts = loadAllBackContexts
        _frontContextType = frontContextType
        _initialLocRepo = locRepo
    End Sub

    Public Sub New(locRepo As ILocalizationRepository, loadAllBackContexts As Boolean, frontContext As IContext)
        _loadAllBackContexts = loadAllBackContexts
        _frontContext = frontContext
        _initialLocRepo = locRepo
    End Sub

    Public Overrides Sub Load()
        Bind(Of ILocalizationRepository).ToConstant(New ResourceLocalizationRepository(My.Resources.ResourceManager)).WhenTargetHas(Of IcmContextConsoleLocalizationAttribute)()
        If _initialLocRepo IsNot Nothing Then
            Bind(Of ILocalizationRepository).ToConstant(_initialLocRepo)
        End If
        Bind(Of IInteractor).To(Of StreamsInteractor).InSingletonScope()
        Bind(Of ITokenParser).To(Of StandardTokenParser).InSingletonScope()
        If _contextRootNode Is Nothing Then
            Bind(Of IContextTreeBuilder).To(Of ReflectionContextTreeBuilder).InSingletonScope()
            If _frontContext IsNot Nothing Then
                Bind(Of IContext).ToConstant(_frontContext).WhenInjectedInto(Of ReflectionContextTreeBuilder).InSingletonScope()
            ElseIf _frontContextType IsNot Nothing Then
                Bind(Of IContext).To(_frontContextType).WhenInjectedInto(Of ReflectionContextTreeBuilder).InSingletonScope()
            End If
        ElseIf _typeRootNode Is Nothing Then
            Bind(Of IContextTreeBuilder).ToConstant(New TypeContextTreeBuilder(_typeRootNode)).InSingletonScope()
        Else
            Bind(Of IContextTreeBuilder).ToConstant(New SimpleContextTreeBuilder(_contextRootNode)).InSingletonScope()
        End If
        Bind(Of IApplication).To(Of StandardApplication).InSingletonScope().WithConstructorArgument("rootNode", _contextRootNode)

        If _loadAllBackContexts Then
            ' By default, use reflection to get contexts and actions
            Dim backContextTypes = GetAllImplementors(Of IContext)()

            For Each backContextType In backContextTypes
                Bind(Of IContext).To(backContextType).InSingletonScope()
            Next
        End If
    End Sub
End Class
