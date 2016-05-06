using System.Collections.Generic;

/// <summary>
/// An Action Finder finds the actions a context implements
/// </summary>
/// <remarks></remarks>
public interface IActionFinder
{
	IEnumerable<IAction> GetActions(IContext ctl);
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
