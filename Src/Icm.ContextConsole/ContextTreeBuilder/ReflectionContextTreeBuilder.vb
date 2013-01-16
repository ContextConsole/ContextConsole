Imports Icm.Localization
Imports Icm.Ninject
Imports Icm.Tree

Public Class ReflectionContextTreeBuilder
    Implements IContextTreeBuilder


    Private ReadOnly _root As IContext

    Public Sub New(root As IContext)
        _root = root
    End Sub

    Public Function GetTree() As ITreeNode(Of IContext) Implements IContextTreeBuilder.GetTree
        Dim rootnode = New TreeNode(Of IContext)(_root)

        rootnode.AddChildren(Instances(Of IContext)().Where(Function(ctlinst) Not _root.GetType.Equals(ctlinst.GetType)))

        Return rootnode
    End Function
End Class
