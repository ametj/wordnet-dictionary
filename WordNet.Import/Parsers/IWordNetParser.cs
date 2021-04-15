using WordNet.Import.Model;

namespace WordNet.Import.Parsers
{
    public interface IWordNetParser
    {
        ParseResult Parse(string fileName, bool loadRelations);
    }
}