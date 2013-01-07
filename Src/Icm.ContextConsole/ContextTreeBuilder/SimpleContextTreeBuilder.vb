Imports Icm.Localization
Imports Icm.Ninject


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
