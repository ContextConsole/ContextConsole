
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Icm.ContextConsole;
using System.Runtime.CompilerServices;
using Icm.Tree;

public static class App
{

	public static void Main()
	{
		dynamic app = StandardApplication.Create<MainContext>(extLocRepo: My.Resources.ResourceManager.ToRepository);
		app.DependencyResolver = new CustomDependencyResolver();
		app.ApplicationPrompt = "EXAMPLE";
		app.Run();

		TreeNode<Type> root = new TreeNode<Type>(typeof(MainContext));

		root.AddEnter<MathContext>.AddEnter<MathContext>.Add<MathContext>.Add<MathContext>.Parent.Parent.Add<MathContext>();

		TypeContextTreeBuilder sct = new TypeContextTreeBuilder(root);

		app = new StandardApplication(root, extLocRepo: My.Resources.ResourceManager.ToRepository);

		app.ApplicationPrompt = "EXAMPLE2";
		app.Run();

	}

	[Extension()]
	public static TreeNode<Type> Add<T>(TreeNode<Type> tn) where T : IContext
	{
		dynamic child = tn.AddChild(typeof(T));
		return tn;
	}

	[Extension()]
	public static TreeNode<Type> AddEnter<T>(TreeNode<Type> tn) where T : IContext
	{
		dynamic child = tn.AddChild(typeof(T));
		return child;
	}

}

public class CustomDependencyResolver : StandardDependencyResolver
{


	private int Counter = 0;
	protected override object CreateService(Type service)
	{
		if (service.Equals(typeof(MathContext))) {
			Counter += 1;
			return new MathContext { PostFix = "Injected" + Counter.ToString };
		}
		return base.CreateService(service);
	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
