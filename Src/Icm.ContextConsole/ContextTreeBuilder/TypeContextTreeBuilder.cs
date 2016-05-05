
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Icm.Tree;

/// <summary>
/// This builder accepts an already built tree of types.
/// </summary>
/// <remarks> The returned context tree instantiate each type on demand,
/// based on the corresponding item of the type tree.</remarks>
public class TypeContextTreeBuilder : IContextTreeBuilder
{


	private ITreeNode<Type> _root;
	protected TypeContextTreeBuilder()
	{
	}

	public TypeContextTreeBuilder(TreeNode<Type> root)
	{
		_root = root;
	}

	public IApplication Application { get; set; }

	protected ITreeNode<Type> Root {
		get { return _root; }
		set { _root = value; }
	}

	public ITreeNode<IContext> GetTree()
	{
		if (Application == null) {
			throw new InvalidOperationException("Cannot get context tree if the application is not set");
		}

		var contextRootNode = new TransformTreeNode<Type, IContext>(_root, ctxType =>
		{
			var context = Application.DependencyResolver.GetService<IContext>(ctxType);
			context.Initialize(Application);
			return context;
		});
		return contextRootNode;
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
