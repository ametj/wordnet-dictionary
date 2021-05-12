using System;
using System.Collections.Generic;

namespace WordNet.Data.Model
{
    public class LexicalEntry
    {
        public string Id { get; set; }
        public string Lemma { get; set; }
        public string Language { get; set; }
        public PartOfSpeech PartOfSpeech { get; set; }
        public DateTime? LastAccessed { get; set; }

        public IList<string> Forms { get; set; } = new List<string>();
        public virtual IList<Sense> Senses { get; set; } = new List<Sense>();
        public virtual IList<SyntacticBehaviour> SyntacticBehaviours { get; set; } = new List<SyntacticBehaviour>();
    }
}