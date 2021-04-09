using System.Collections.Generic;

namespace WordNet.Model
{
    public class Sense
    {
        public string Id { get; set; }
        public LexicalEntry LexicalEntry { get; set; }
        public Synset Synset { get; set; }

        public IList<SenseRelation> Relations { get; set; } = new List<SenseRelation>();
    }
}
