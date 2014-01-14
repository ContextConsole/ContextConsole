Imports Icm.Tree
Imports Icm.Reflection

''' <summary>
''' Builds a two-level context tree by reflection, based on a root IContext.
''' </summary>
''' <remarks>
''' The first level is formed by the root context. The second level is formed by all the instantiable types (using Ninject)
''' of IContext, except for the type of the root context itself.)
''' </remarks>
Public Class ReflectionContextTreeBuilder
    Inherits TypeContextTreeBuilder

    Public Sub New(rootContextType As Type)
        MyBase.New()
        Dim typeRootNode = New TreeNode(Of Type)(rootContextType)
        Dim childContextTypes = GetAllImplementors(Of IContext)().Where(Function(ctxType) Not rootContextType.Equals(ctxType) AndAlso Not ctxType.IsNestedPrivate)
        typeRootNode.AddChildren(childContextTypes)
        Root = typeRootNode
    End Sub

End Class
