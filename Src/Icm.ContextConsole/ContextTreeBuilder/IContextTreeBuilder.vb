Imports Icm.Tree

''' <summary>
''' Interface for builders of context trees.
''' </summary>
''' <remarks>It has one method that returns the root context of the tree.</remarks>
Public Interface IContextTreeBuilder
    Property Application As IApplication
    Function GetTree() As ITreeNode(Of IContext)
End Interface
