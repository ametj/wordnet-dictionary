namespace WordNet.Model
{
    public enum SenseRelationType
    {
        antonym,
        also,
        participle,
        pertainym,
        derivation,
        domain_topic,
        has_domain_topic,
        domain_region,
        has_domain_region,
        exemplifies,
        is_exemplified_by,
        similar,
        other
    }

    public class SenseRelation
    {
        public int Id { get; set; }
        public SenseRelationType Type { get; set; }
        public Sense Target { get; set; }
    }
}
