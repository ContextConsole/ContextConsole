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

    Property ApplicationPrompt As String Implements IApplication.ApplicationPrompt

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

    Public Sub New(rootContext As IContext)
        MyClass.New(
            New SimpleContextTreeBuilder(New TreeNode(Of IContext)(rootContext)),
            New StandardTokenParser,
            New StreamsInteractor,
            New DictionaryLocalizationRepository,
            New DictionaryLocalizationRepository)
        Interactor.SetLocalizationRepo(InternalLocRepo)
    End Sub

    Public Sub New(rootContext As IContext, inter As IInteractor)
        MyClass.New(
            New SimpleContextTreeBuilder(New TreeNode(Of IContext)(rootContext)),
            New StandardTokenParser,
            inter,
            New DictionaryLocalizationRepository,
            New DictionaryLocalizationRepository)
    End Sub

    <Global.Ninject.Inject>
    Public Sub New(
                  treeBuilder As IContextTreeBuilder,
                  tokenParser As ITokenParser,
                  interactor As IInteractor,
                  <IcmContextConsoleLocalization> intLocRepo As ILocalizationRepository,
                  extLocRepo As ILocalizationRepository
                  )

        If tokenParser Is Nothing Then
            _tokenParser = New StandardTokenParser
        Else
            _tokenParser = tokenParser
        End If
        _interactor = interactor
        _internalLocRepo = intLocRepo
        _externalLocRepo = extLocRepo
        ApplicationPrompt = System.Reflection.Assembly.GetExecutingAssembly.CodeBase
        ' Stablish root context in the last place so that the IContext.Initialize routine
        ' can access the former application values.
        SetRootContextNode(treeBuilder.GetTree)

    End Sub

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
            ' Give reference to the application to all the contexts
            For Each ctl In value.DepthPreorderTraverse()
                ctl.Initialize(Me)
            Next
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

    Private Function ExecuteCommand() As Boolean Implements IApplication.ExecuteCommand
        ' Ask command line
        Dim command As String
        If CurrentContextNode Is RootContextNode Then
            command = Interactor.AskCommand(ApplicationPrompt)
        Else
            command = Interactor.AskCommand(ApplicationPrompt & " " & CurrentContextNode.Ancestors.Reverse.Skip(1).Select(Function(node) node.Name).JoinStr(" "))
        End If

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
