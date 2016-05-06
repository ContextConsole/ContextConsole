using System.Collections.Generic;

public static class IContextExtensions
{
	public static IEnumerable<IAction> GetActions(this IContext ctx)
	{
		return ctx.ActionFinder.GetActions(ctx);
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
