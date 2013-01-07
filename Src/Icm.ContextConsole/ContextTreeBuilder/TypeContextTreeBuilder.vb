Imports Icm.Localization
Imports Icm.Ninject

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
