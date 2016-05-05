
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using Icm.Localization;
using Icm.Reflection;

/// <summary>
/// A base context implements many of the methods of IContext based on 4 of them.
/// </summary>
/// <remarks></remarks>
public abstract class BaseContext : IContext
{


	private readonly IActionFinder _actionFinder;
	protected ILocalizationRepository locRepo;

	protected ILocalizationRepository extLocRepo;
	public IInteractor Interactor { get; set; }

	public BaseContext()
	{
		_actionFinder = new ReflectionActionFinder();
	}

	public BaseContext(IActionFinder actionFinder)
	{
		_actionFinder = actionFinder;
	}


	protected virtual void BaseInitialize(IApplication app)
	{
		locRepo = app.InternalLocRepo;
		extLocRepo = app.ExternalLocRepo;
		Interactor = app.Interactor;
		Initialize();
	}
	void IContext.Initialize(IApplication app)
	{
		BaseInitialize(app);
	}

	protected virtual void Initialize()
	{
	}

	public virtual string Name()
	{
		var ctlTypeName = this.GetType().Name;
		return ctlTypeName.Substring(0, ctlTypeName.Length - "Context".Length).ToLower();
	}


	public virtual IEnumerable<string> Synonyms()
	{
		var synAttr = this.GetType().GetAttribute<SynonymAttribute>(true);
		List<string> result = new List<string>();
		if (synAttr != null) {
			result.AddRange(synAttr.Synonyms.Select(str => str.ToLower()));
		}
		return result;
	}

	public bool Named(string contextName)
	{
		return Name() == contextName || Synonyms().Contains(contextName);
	}

	public IActionFinder ActionFinder {
		get { return _actionFinder; }
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
