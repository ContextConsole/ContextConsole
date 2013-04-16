Imports Icm.Ninject
Imports Icm.Tree

''' <summary>
''' Builds a two-level context tree by reflection, based on a root IContext.
''' </summary>
''' <remarks>
''' The first level is formed by the root context. The second level is formed by all the instantiable types (using Ninject)
''' of IContext, except for the type of the root context itself.)
''' </remarks>
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
