using System.Collections.Generic;

public interface INamedWithSynonyms
{

	string Name();
	IEnumerable<string> Synonyms();

}
