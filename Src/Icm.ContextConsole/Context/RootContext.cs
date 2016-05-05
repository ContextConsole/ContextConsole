
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Icm.Tree;

using Icm.Localization;

/// <summary>
/// This base root context provides implementation of Help action and Quit action.
/// </summary>
/// <remarks></remarks>
public abstract class RootContext : BaseContext
{


	private IApplication Application;
	protected override void BaseInitialize(IApplication app)
	{
		base.BaseInitialize(app);
		Application = app;
	}

	public abstract void Credits();

	public void Help()
	{
		// Actions of current context
		ShowActions(Application.CurrentContextNode.Value);

		// Available subcontexts
		Interactor.ShowList("Subcontexts", 
            Application.CurrentContextNode.GetChildNodes(), 
            ctx => string.Format(My.Resources.Resources.root_use, ctx.Value.Name().ToLower()), 
            hideIfEmpty: true);

		// Inherited actions from ancestors
		foreach (var ctx in Application.CurrentContextNode.ProperAncestors()) {
			ShowActions(ctx);
		}
	}

	public void Contexts()
	{
		foreach (var contextAndLevel in Application.RootContextNode.DepthPreorderTraverseWithLevel()) {
			Interactor.ShowMessage(new string(' ', contextAndLevel.Level) + contextAndLevel.Result.Name());
		}
	}

	public virtual void Quit()
	{
		Interactor.ShowMessage(locRepo.Trans("quit_bye"));
	}

	private void ShowActions(IContext ctl)
	{
		bool IsCurrent = Application.CurrentContextNode.Value.Name() == ctl.Name();

		string title = null;
		if (IsCurrent) {
			title = locRepo.TransF("help_title", ctl.Name(), PhrF("help_current"));
		} else {
			title = locRepo.TransF("help_title", ctl.Name());
		}

		Interactor.ShowList(title, ctl.GetActions(), action => string.Format("{0}: {1}", action.Name().ToLower(), TranslateActionDescription(action) ?? action.Name()));
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
