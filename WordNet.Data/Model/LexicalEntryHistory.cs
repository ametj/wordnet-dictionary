using System;

namespace WordNet.Data.Model
{
    public class LexicalEntryHistory
    {
        public string Lemma { get; set; }
        public string Language { get; set; }
        public DateTime LastAccessed { get; set; }
    }
}