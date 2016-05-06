using System.Linq;

public static class INamedWithSynonymsExtensions
{
	public static bool IsNamed(this INamedWithSynonyms obj, string name)
	{
		var lowname = name.ToLower();
		return obj.Name() == lowname || obj.Synonyms().Contains(lowname);
	}

}
