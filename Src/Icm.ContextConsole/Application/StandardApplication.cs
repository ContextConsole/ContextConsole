using System;
using System.Collections.Generic;
using System.Linq;
using Icm;
using Icm.Localization;
using Icm.Collections;
using Icm.Tree;

public class StandardApplication : IApplication
{
	private readonly ILocalizationRepository _internalLocRepo;
	private readonly ILocalizationRepository _externalLocRepo;
	private readonly IInteractor _interactor;
	private ITreeNode<IContext> _currentContextNode;
	private ITreeNode<IContext> _rootContextNode;
	private IEnumerable<IContext> _contextsCache;

	private readonly ITokenParser _tokenParser;
	public StandardApplication(Type rootContextType, ITokenParser tokenParser = null, IInteractor interactor = null, ILocalizationRepository intLocRepo = null, ILocalizationRepository extLocRepo = null) : this(new ReflectionContextTreeBuilder(rootContextType), tokenParser, interactor, intLocRepo, extLocRepo)
	{
	}

	public StandardApplication(TreeNode<Type> rootContextNode, ITokenParser tokenParser = null, IInteractor interactor = null, ILocalizationRepository intLocRepo = null, ILocalizationRepository extLocRepo = null) : this(new TypeContextTreeBuilder(rootContextNode), tokenParser, interactor, intLocRepo, extLocRepo)
	{
	}


	public StandardApplication(IContextTreeBuilder treeBuilder, ITokenParser tokenParser = null, IInteractor interactor = null, ILocalizationRepository intLocRepo = null, ILocalizationRepository extLocRepo = null)
	{
		if (tokenParser == null) {
			_tokenParser = new StandardTokenParser();
		} else {
			_tokenParser = tokenParser;
		}
		if (intLocRepo == null) {
			intLocRepo = new ResourceLocalizationRepository(My.Resources.Resources.ResourceManager);
		}
		if (extLocRepo == null) {
			extLocRepo = new DictionaryLocalizationRepository();
		}
		if (interactor == null) {
			interactor = new StreamsInteractor(intLocRepo);
		}
		if (tokenParser == null) {
			tokenParser = new StandardTokenParser();
		}
		treeBuilder.Application = this;
		_interactor = interactor;
		_internalLocRepo = intLocRepo;
		_externalLocRepo = extLocRepo;
		ApplicationPrompt = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
		DependencyResolver = new StandardDependencyResolver();
		// Stablish root context in the last place so that the IContext.Initialize routine
		// can access the former application values.
		SetRootContextNode(treeBuilder.GetTree());
	}

	public string ApplicationPrompt { get; set; }

	public IDependencyResolver DependencyResolver { get; set; }

	public ILocalizationRepository ExternalLocRepo {
		get { return _externalLocRepo; }
	}

	public ILocalizationRepository InternalLocRepo {
		get { return _internalLocRepo; }
	}

	public IInteractor Interactor {
		get { return _interactor; }
	}

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TMain"></typeparam>
	/// <returns></returns>
	/// <remarks>This builder is here because there is no such thing as generic constructors.</remarks>
	public static IApplication Create<TMain>(ITokenParser tokenParser = null, IInteractor interactor = null, ILocalizationRepository intLocRepo = null, ILocalizationRepository extLocRepo = null) where TMain : IContext
	{
		StandardApplication app = new StandardApplication(typeof(TMain), tokenParser, interactor, intLocRepo, extLocRepo);
		return app;
	}

	public IContext CurrentContext {
		get { return CurrentContextNode.Value; }
	}

	public ITreeNode<IContext> CurrentContextNode {
		get { return _currentContextNode; }
	}


	private void SetRootContextNode(ITreeNode<IContext> value)
	{
		if ((!object.ReferenceEquals(_rootContextNode, value))) {
			_rootContextNode = value;
		}
		_currentContextNode = value;
	}

	public ITreeNode<IContext> RootContextNode {
		get { return _rootContextNode; }
	}

	public IEnumerable<IContext> GetAllContexts()
	{
		if (_contextsCache == null) {
			_contextsCache = CurrentContextNode.DepthPreorderTraverse();
		}
		return _contextsCache;
	}

	public void Run()
	{
		var action = RootContextNode.Value.GetActions().GetNamedItem("credits");
		action.Execute();
		while (!ExecuteCommand()) {
			DependencyResolver.ClearRequestScope();
		}
	}

	public bool ExecuteCommand()
	{
		// Ask command line
		string command = null;

		var prompt = new[] { ApplicationPrompt }
        .Concat(CurrentContextNode.Ancestors()
        .Reverse()
        .Skip(1)
        .Select(node => node.Name()))
        .JoinStr(" ");

		command = Interactor.AskCommand(prompt);

		// Parse command line
		_tokenParser.Initialize();
		_tokenParser.Parse(command);

		if (_tokenParser.Errors.Any()) {
			Interactor.ShowErrors(_tokenParser.Errors.Select(parseErr => InternalLocRepo.TransF("executecommand_parseerror", parseErr.Index, parseErr.StartIndex)));
			return false;
		}

		var saTokens = _tokenParser.Tokens.ToArray();

		if (!saTokens.Any()) {
			return false;
		}

		var i = 0;

		// Context retrieval (recursive)
		var executionCtxNode = CurrentContextNode;
		ITreeNode<IContext> nextCtxNode = null;
		do {
			var contextName = saTokens[i];

			if (contextName == "..") {
				nextCtxNode = executionCtxNode.GetParent();
				i += 1;
			} else {
				nextCtxNode = executionCtxNode.GetChildNodes().SingleOrDefault(ctrl => ctrl.Value.IsNamed(contextName)).As<ITreeNode<IContext>>();
			}
			if (nextCtxNode != null) {
				executionCtxNode = nextCtxNode;
				i += 1;
			}
		} while (!(nextCtxNode == null || i == saTokens.Count()));

		if (i < saTokens.Count()) {
			// Action retrieval
			var actionName = saTokens[i].ToLower();
			IAction action = default(IAction);
			var ancestorExecutionCtx = executionCtxNode;
			do {
				action = ancestorExecutionCtx.Value.GetActions().GetNamedItem(actionName);

				if (action == null) {
					ancestorExecutionCtx = ancestorExecutionCtx.GetParent();
					if (ancestorExecutionCtx == null) {
						break; // TODO: might not be correct. Was : Exit Do
					}
				} else {
					break; // TODO: might not be correct. Was : Exit Do
				}
			} while (true);

			if (action == null) {
				Interactor.ShowErrors(new [] { InternalLocRepo.TransF("executecommand_actionnotfound", actionName, executionCtxNode.Ancestors().Select(ctx => ctx.Name()).JoinStr(",")) });
				return false;
			} else {
				i += 1;
				for (var j = i; j <= saTokens.Count() - 1; j++) {
					Interactor.TokenQueue.Enqueue(saTokens[j]);
				}
				action.Execute();
				Interactor.ShowMessage("");
				return (action.Name() == "quit");
			}
		} else {
			Interactor.ShowMessage(InternalLocRepo.TransF("use_message", executionCtxNode.Value.Name()));
			_currentContextNode = executionCtxNode;
			return false;
		}
	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
