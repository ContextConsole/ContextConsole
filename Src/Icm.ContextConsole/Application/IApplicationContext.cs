using System.Collections.Generic;
using Icm.Localization;
using Icm.Tree;

public interface IApplicationContext
{

	ITreeNode<IContext> CurrentContextNode { get; }

	ITreeNode<IContext> RootContextNode { get; }
	IEnumerable<IContext> GetAllContexts();


	Queue<string> TokenQueue { get; }

	IInteractor Interactor { get; }
	ILocalizationRepository InternalLocRepo { get; }

	ILocalizationRepository ExternalLocRepo { get; }
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
