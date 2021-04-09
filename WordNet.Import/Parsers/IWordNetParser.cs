using System.Collections.Generic;
using WordNet.Model;

namespace WordNet.Import.Parsers
{
    public interface IWordNetParser
    {
        ICollection<LexicalEntry> Parse(string fileName);
    }
}
