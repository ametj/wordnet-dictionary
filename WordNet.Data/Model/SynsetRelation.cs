namespace WordNet.Data.Model
{
    public enum SynsetRelationType
    {
        agent,
        also,
        attribute,
        be_in_state,
        causes,
        classified_by,
        classifies,
        co_agent_instrument,
        co_agent_patient,
        co_agent_result,
        co_instrument_agent,
        co_instrument_patient,
        co_instrument_result,
        co_patient_agent,
        co_patient_instrument,
        co_result_agent,
        co_result_instrument,
        co_role,
        direction,
        domain_region,
        domain_topic,
        exemplifies,
        entails,
        eq_synonym,
        has_domain_region,
        has_domain_topic,
        is_exemplified_by,
        holo_location,
        holo_member,
        holo_part,
        holo_portion,
        holo_substance,
        holonym,
        hypernym,
        hyponym,
        in_manner,
        instance_hypernym,
        instance_hyponym,
        instrument,
        involved,
        involved_agent,
        involved_direction,
        involved_instrument,
        involved_location,
        involved_patient,
        involved_result,
        involved_source_direction,
        involved_target_direction,
        is_caused_by,
        is_entailed_by,
        location,
        manner_of,
        mero_location,
        mero_member,
        mero_part,
        mero_portion,
        mero_substance,
        meronym,
        similar,
        other,
        patient,
        restricted_by,
        restricts,
        result,
        role,
        source_direction,
        state_of,
        target_direction,
        subevent,
        is_subevent_of,
        antonym
    }

    public class SynsetRelation
    {
        public int Id { get; set; }
        public SynsetRelationType Type { get; set; }

        public string SourceId { get; set; }
        public virtual Synset Source { get; set; }

        public string TargetId { get; set; }
        public virtual Synset Target { get; set; }
    }
}