using System;
using Icm.Localization;
using System.Resources;

/// <summary>
/// Various methods for building and running an Icm.ContextConsole application.
/// </summary>
/// <remarks></remarks>
public static class ApplicationStarter
{

	public static void Start<TMain>(ILocalizationRepository locRepo) where TMain : IContext
	{
		StandardApplication.Create<TMain>(extLocRepo: locRepo).Run();
	}

	public static void Start<TMain>(ResourceManager resourceManager) where TMain : IContext
	{
		Start<TMain>(resourceManager.ToRepository());
	}

	public static void Start(Type rootContextType, ResourceManager resourceManager)
	{
		var app = new StandardApplication(rootContextType, extLocRepo: resourceManager.ToRepository());
		app.Run();
	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
