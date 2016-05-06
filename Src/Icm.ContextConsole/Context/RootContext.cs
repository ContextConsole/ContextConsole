using Icm.Tree;

using Icm.Localization;
using static Icm.Localization.PhraseFactory;

/// <summary>
/// This base root context provides implementation of Help action and Quit action.
/// </summary>
/// <remarks></remarks>
public abstract class RootContext : BaseContext
{


	private IApplication _application;
	protected override void BaseInitialize(IApplication app)
	{
		base.BaseInitialize(app);
		_application = app;
	}

	public abstract void Credits();

	public void Help()
	{
		// Actions of current context
		ShowActions(_application.CurrentContextNode.Value);

		// Available subcontexts
		Interactor.ShowList("Subcontexts", 
            _application.CurrentContextNode.GetChildNodes(), 
            ctx => string.Format(My.Resources.Resources.root_use, ctx.Value.Name().ToLower()), 
            hideIfEmpty: true);

		// Inherited actions from ancestors
		foreach (var ctx in _application.CurrentContextNode.ProperAncestors()) {
			ShowActions(ctx);
		}
	}

	public void Contexts()
	{
		foreach (var contextAndLevel in _application.RootContextNode.DepthPreorderTraverseWithLevel()) {
			Interactor.ShowMessage(new string(' ', contextAndLevel.Level) + contextAndLevel.Result.Name());
		}
	}

	public virtual void Quit()
	{
		Interactor.ShowMessage(locRepo.Trans("quit_bye"));
	}

	private void ShowActions(IContext ctl)
	{
		bool isCurrent = _application.CurrentContextNode.Value.Name() == ctl.Name();

		string title;
		title = isCurrent 
            ? locRepo.TransF("help_title", ctl.Name(), PhrF("help_current")) 
            : locRepo.TransF("help_title", ctl.Name());

		Interactor.ShowList(title, ctl.GetActions(), action =>
		    $"{action.Name().ToLower()}: {TranslateActionDescription(action) ?? action.Name()}");
	}

	private string TranslateActionDescription(IAction action)
	{
		if (action.IsInternal()) {
			return locRepo.Trans(action.LocalizationKey());
		} else {
			return extLocRepo.Trans(action.LocalizationKey());
		}
	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
