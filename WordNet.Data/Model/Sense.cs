using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WordNet.Model
{
    public class Sense
    {
        public string Id { get; set; }

        public string LexicalEntryId { get; set; }
        public virtual LexicalEntry LexicalEntry { get; set; }

        public string SynsetId { get; set; }
        public virtual Synset Synset { get; set; }

        public virtual IList<SenseRelation> Relations { get; set; } = new List<SenseRelation>();

        [NotMapped]
        public string PositionInSynset => Id[^2..];
    }
}