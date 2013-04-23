Imports Icm.Tree

''' <summary>
''' This tree builder does nothing, it returns the tree node given in the constructor.
''' </summary>
''' <remarks>
''' This builder is useful for testing purposes or if you want to control the creation of your context tree.
''' </remarks>
Public Class SimpleContextTreeBuilder
    Implements IContextTreeBuilder

    Private ReadOnly _root As ITreeNode(Of IContext)

    Public Sub New(root As ITreeNode(Of IContext))
        _root = root
    End Sub

    Public Function GetTree() As ITreeNode(Of IContext) Implements IContextTreeBuilder.GetTree
        Return _root
    End Function

End Class
