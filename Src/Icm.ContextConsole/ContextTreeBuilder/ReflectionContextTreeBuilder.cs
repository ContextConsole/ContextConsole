using System;
using System.Linq;
using Icm.Tree;
using static Icm.Reflection.ActivatorTools;

/// <summary>
/// Builds a two-level context tree by reflection, based on a root IContext.
/// </summary>
/// <remarks>
/// The first level is formed by the root context. The second level is formed by all the instantiable types (using Ninject)
/// of IContext, except for the type of the root context itself.) <see cref="Action(Of Integer)"></see>
/// </remarks>
public class ReflectionContextTreeBuilder : TypeContextTreeBuilder
{

	public ReflectionContextTreeBuilder(Type rootContextType) : base()
	{
		var typeRootNode = new TreeNode<Type>(rootContextType);
		var childContextTypes = GetAllImplementors<IContext>().Where(ctxType => !(rootContextType == ctxType) && !ctxType.IsNestedPrivate);
		typeRootNode.AddChildren(childContextTypes);
		Root = typeRootNode;
	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
