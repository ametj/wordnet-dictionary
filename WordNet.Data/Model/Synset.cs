using System.Collections.Generic;
using WordNet.Data.Model;

namespace WordNet.Model
{
    public class Synset
    {
        public string Id { get; set; }
        public string Ili { get; set; }
        public PartOfSpeech PartOfSpeech { get; set; }

        public virtual IList<Sense> Senses { get; set; }

        public virtual IList<string> Definitions { get; set; } = new List<string>();
        public virtual IList<string> Examples { get; set; } = new List<string>();
        public virtual IList<SynsetRelation> Relations { get; set; } = new List<SynsetRelation>();
    }
}