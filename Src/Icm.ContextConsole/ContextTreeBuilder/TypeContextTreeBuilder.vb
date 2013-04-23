Imports Icm.Ninject
Imports Icm.Tree

''' <summary>
''' This builder accepts an already built tree of types.
''' </summary>
''' <remarks> The returned context tree instantiate each type on demand,
''' based on the corresponding item of the type tree.</remarks>
Public Class TypeContextTreeBuilder
    Implements IContextTreeBuilder


    Private ReadOnly _root As ITreeNode(Of Type)

    Public Sub New(root As TreeNode(Of Type))
        _root = root
    End Sub

    Public Function GetTree() As ITreeNode(Of IContext) Implements IContextTreeBuilder.GetTree
        Return _root.Select(Function(typ) DirectCast(Instance(typ), IContext))
    End Function
End Class
