
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Icm.Localization;
using Icm.Tree;

public interface IApplication
{


	string ApplicationPrompt { get; set; }
	ITreeNode<IContext> CurrentContextNode { get; }

	ITreeNode<IContext> RootContextNode { get; }
	IEnumerable<IContext> GetAllContexts();

	bool ExecuteCommand();


	IInteractor Interactor { get; }
	ILocalizationRepository InternalLocRepo { get; }

	ILocalizationRepository ExternalLocRepo { get; }

	void Run();

	IDependencyResolver DependencyResolver { get; set; }
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
