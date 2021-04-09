using System.Collections.Generic;
using WordNet.Data.Model;

namespace WordNet.Model
{
    public class Synset
    {
        public string Id { get; set; }
        public string Ili { get; set; }
        public PartOfSpeech PartOfSpeech { get; set; }

        public IList<string> Definitions { get; set; } = new List<string>();
        public IList<string> Examples { get; set; } = new List<string>();
        public IList<SynsetRelation> Relations { get; set; } = new List<SynsetRelation>();
    }
}
