Imports Icm.Localization
Imports Icm.Tree

Public Interface IContextTreeBuilder
    Function GetTree() As ITreeNode(Of IContext)
End Interface
