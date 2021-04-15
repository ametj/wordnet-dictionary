using System.Collections.Generic;
using WordNet.Model;

namespace WordNet.Import.Model
{
    public class ParseResult
    {
        public IEnumerable<LexicalEntry> LexicalEntries { get; set; }
        public IEnumerable<SenseRelation> SenseRelations { get; set; }
        public IEnumerable<SynsetRelation> SynsetRelations { get; set; }
    }
}