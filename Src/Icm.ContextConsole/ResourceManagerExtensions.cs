using Icm.Localization;
using System.Resources;

public static class ResourceManagerExtensions
{
	public static ILocalizationRepository ToRepository(this ResourceManager manager)
	{
		return new ResourceLocalizationRepository(manager);
	}

}
