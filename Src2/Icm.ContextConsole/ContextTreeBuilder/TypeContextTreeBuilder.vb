Imports Icm.Tree

''' <summary>
''' This builder accepts an already built tree of types.
''' </summary>
''' <remarks> The returned context tree instantiate each type on demand,
''' based on the corresponding item of the type tree.</remarks>
Public Class TypeContextTreeBuilder
    Implements IContextTreeBuilder

    Private _root As ITreeNode(Of Type)

    Protected Sub New()
    End Sub

    Public Sub New(root As TreeNode(Of Type))
        _root = root
    End Sub

    Property Application As IApplication Implements IContextTreeBuilder.Application

    Protected Property Root As ITreeNode(Of Type)
        Get
            Return _root
        End Get
        Set(value As ITreeNode(Of Type))
            _root = value
        End Set
    End Property

    Public Function GetTree() As ITreeNode(Of IContext) Implements IContextTreeBuilder.GetTree
        If Application Is Nothing Then
            Throw New InvalidOperationException("Cannot get context tree if the application is not set")
        End If

        Dim contextRootNode = New TransformTreeNode(Of Type, IContext)(_root, Function(ctxType)
                                                                                  Dim context = Application.DependencyResolver.GetService(Of IContext)(ctxType)
                                                                                  context.Initialize(Application)
                                                                                  Return context
                                                                              End Function)
        Return contextRootNode
    End Function
End Class
