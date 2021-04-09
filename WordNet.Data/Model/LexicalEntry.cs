using System.Collections.Generic;
using WordNet.Data.Model;

namespace WordNet.Model
{
    public class LexicalEntry
    {
        public string Id { get; set; }
        public string Lemma { get; set; }
        public string Language { get; set; }
        public PartOfSpeech PartOfSpeech { get; set; }

        public IList<string> Forms { get; set; } = new List<string>();
        public IList<Sense> Senses { get; set; } = new List<Sense>();
        public IList<SyntacticBehaviour> SyntacticBehaviours { get; set; } = new List<SyntacticBehaviour>();
    }
}
