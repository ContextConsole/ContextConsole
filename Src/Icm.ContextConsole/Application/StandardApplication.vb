Imports Icm.Localization
Imports Icm.Collections
Imports Icm.Tree

Public Class StandardApplication
    Implements IApplication
    Private ReadOnly _internalLocRepo As ILocalizationRepository
    Private ReadOnly _externalLocRepo As ILocalizationRepository
    Private ReadOnly _interactor As IInteractor
    Private _currentContextNode As ITreeNode(Of IContext)
    Private _rootContextNode As ITreeNode(Of IContext)
    Private _contextsCache As IEnumerable(Of IContext)
    Private ReadOnly _tokenParser As ITokenParser

    Public Sub New(rootContextType As Type,
                  Optional tokenParser As ITokenParser = Nothing,
                  Optional interactor As IInteractor = Nothing,
                  Optional intLocRepo As ILocalizationRepository = Nothing,
                  Optional extLocRepo As ILocalizationRepository = Nothing
                  )
        MyClass.New(
            New ReflectionContextTreeBuilder(rootContextType),
            tokenParser,
            interactor,
            intLocRepo,
            extLocRepo
        )
    End Sub

    Public Sub New(rootContextNode As TreeNode(Of Type),
                  Optional tokenParser As ITokenParser = Nothing,
                  Optional interactor As IInteractor = Nothing,
                  Optional intLocRepo As ILocalizationRepository = Nothing,
                  Optional extLocRepo As ILocalizationRepository = Nothing
                  )
        MyClass.New(
            New TypeContextTreeBuilder(rootContextNode),
            tokenParser,
            interactor,
            intLocRepo,
            extLocRepo
        )
    End Sub

    Public Sub New(
                  treeBuilder As IContextTreeBuilder,
                  Optional tokenParser As ITokenParser = Nothing,
                  Optional interactor As IInteractor = Nothing,
                  Optional intLocRepo As ILocalizationRepository = Nothing,
                  Optional extLocRepo As ILocalizationRepository = Nothing
                  )

        If tokenParser Is Nothing Then
            _tokenParser = New StandardTokenParser
        Else
            _tokenParser = tokenParser
        End If
        If intLocRepo Is Nothing Then
            intLocRepo = New ResourceLocalizationRepository(My.Resources.ResourceManager)
        End If
        If extLocRepo Is Nothing Then
            extLocRepo = New DictionaryLocalizationRepository()
        End If
        If interactor Is Nothing Then
            interactor = New StreamsInteractor(intLocRepo)
        End If
        If tokenParser Is Nothing Then
            tokenParser = New StandardTokenParser()
        End If
        treeBuilder.Application = Me
        _interactor = interactor
        _internalLocRepo = intLocRepo
        _externalLocRepo = extLocRepo
        ApplicationPrompt = System.Reflection.Assembly.GetExecutingAssembly.CodeBase
        DependencyResolver = New StandardDependencyResolver
        ' Stablish root context in the last place so that the IContext.Initialize routine
        ' can access the former application values.
        SetRootContextNode(treeBuilder.GetTree)
    End Sub

    Property ApplicationPrompt As String Implements IApplication.ApplicationPrompt

    Property DependencyResolver As IDependencyResolver Implements IApplication.DependencyResolver

    ReadOnly Property ExternalLocRepo() As ILocalizationRepository Implements IApplication.ExternalLocRepo
        Get
            Return _externalLocRepo
        End Get
    End Property

    ReadOnly Property InternalLocRepo() As ILocalizationRepository Implements IApplication.InternalLocRepo
        Get
            Return _internalLocRepo
        End Get
    End Property

    ReadOnly Property Interactor() As IInteractor Implements IApplication.Interactor
        Get
            Return _interactor
        End Get
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <typeparam name="TMain"></typeparam>
    ''' <returns></returns>
    ''' <remarks>This builder is here because there is no such thing as generic constructors.</remarks>
    Public Shared Function Create(Of TMain As IContext)(
            Optional tokenParser As ITokenParser = Nothing,
            Optional interactor As IInteractor = Nothing,
            Optional intLocRepo As ILocalizationRepository = Nothing,
            Optional extLocRepo As ILocalizationRepository = Nothing
            ) As IApplication
        Dim app As New StandardApplication(GetType(TMain),
            tokenParser,
            interactor,
            intLocRepo,
            extLocRepo)
        Return app
    End Function

    ReadOnly Property CurrentContext As IContext
        Get
            Return CurrentContextNode.Value
        End Get
    End Property

    ReadOnly Property CurrentContextNode() As ITreeNode(Of IContext) Implements IApplication.CurrentContextNode
        Get
            Return _currentContextNode
        End Get
    End Property


    Private Sub SetRootContextNode(value As ITreeNode(Of IContext))
        If Not _rootContextNode Is value Then
            _rootContextNode = value
        End If
        _currentContextNode = value
    End Sub

    ReadOnly Property RootContextNode() As ITreeNode(Of IContext) Implements IApplication.RootContextNode
        Get
            Return _rootContextNode
        End Get
    End Property

    Public Function GetAllContexts() As IEnumerable(Of IContext) Implements IApplication.GetAllContexts
        If _contextsCache Is Nothing Then
            _contextsCache = CurrentContextNode.DepthPreorderTraverse()
        End If
        Return _contextsCache
    End Function

    Public Sub Run() Implements IApplication.Run
        Dim action = RootContextNode.Value.GetActions().GetNamedItem("credits")
        action.Execute()
        Do Until ExecuteCommand()
            DependencyResolver.ClearRequestScope()
        Loop
    End Sub

    Private Function ExecuteCommand() As Boolean Implements IApplication.ExecuteCommand
        ' Ask command line
        Dim command As String

        Dim prompt =
            {ApplicationPrompt}.Concat(
                CurrentContextNode.
                Ancestors.
                Reverse.
                Skip(1).
                Select(Function(node) node.Name)).
            JoinStr(" ")

        command = Interactor.AskCommand(prompt)

        ' Parse command line
        _tokenParser.Initialize()
        _tokenParser.Parse(command)

        If _tokenParser.Errors.Count > 0 Then
            Interactor.ShowErrors(_tokenParser.Errors.Select(
                                  Function(parseErr)
                                      Return InternalLocRepo.TransF("executecommand_parseerror", parseErr.Index, parseErr.StartIndex)
                                  End Function))
            Return False
        End If

        Dim saTokens = _tokenParser.Tokens

        If saTokens.Count = 0 Then
            Return False
        End If

        Dim i = 0

        ' Context retrieval (recursive)
        Dim executionCtxNode = CurrentContextNode
        Dim nextCtxNode As ITreeNode(Of IContext) = Nothing
        Do
            Dim contextName = saTokens(i)

            If contextName = ".." Then
                nextCtxNode = executionCtxNode.GetParent
                i += 1
            Else
                nextCtxNode = executionCtxNode.GetChildNodes.SingleOrDefault(Function(ctrl) ctrl.Value.IsNamed(contextName)).As(Of ITreeNode(Of IContext))()
            End If
            If nextCtxNode IsNot Nothing Then
                executionCtxNode = nextCtxNode
                i += 1
            End If
        Loop Until nextCtxNode Is Nothing OrElse i = saTokens.Count

        If i < saTokens.Count Then
            ' Action retrieval
            Dim actionName = saTokens(i).ToLower
            Dim action As IAction
            Dim ancestorExecutionCtx = executionCtxNode
            Do
                action = ancestorExecutionCtx.Value.GetActions().GetNamedItem(actionName)

                If action Is Nothing Then
                    ancestorExecutionCtx = ancestorExecutionCtx.GetParent
                    If ancestorExecutionCtx Is Nothing Then
                        Exit Do
                    End If
                Else
                    Exit Do
                End If
            Loop

            If action Is Nothing Then
                Interactor.ShowErrors({InternalLocRepo.TransF("executecommand_actionnotfound", actionName, executionCtxNode.Ancestors.Select(Function(ctx) ctx.Name).JoinStr(","))})
                Return False
            Else
                i += 1
                For j = i To saTokens.Count - 1
                    Interactor.TokenQueue.Enqueue(saTokens(j))
                Next
                action.Execute()
                Interactor.ShowMessage("")
                Return (action.Name = "quit")
            End If
        Else
            Interactor.ShowMessage(InternalLocRepo.TransF("use_message", executionCtxNode.Value.Name))
            _currentContextNode = executionCtxNode
            Return False
        End If
    End Function

End Class
