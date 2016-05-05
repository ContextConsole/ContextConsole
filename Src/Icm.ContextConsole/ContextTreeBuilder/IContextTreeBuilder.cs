
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Icm.Tree;

/// <summary>
/// Interface for builders of context trees.
/// </summary>
/// <remarks>It has one method that returns the root context of the tree.</remarks>
public interface IContextTreeBuilder
{
	IApplication Application { get; set; }
	ITreeNode<IContext> GetTree();
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
