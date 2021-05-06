using System;

namespace WordNet.Data.Model
{
    public enum PartOfSpeech
    {
        Noun = 0,
        Verb = 1,
        Adjective = 2,
        Adverb = 3,
        AdjectiveSatellite = 4,
        Other = 5
    }

    public static class PartOfSpeechExtensions
    {
        public static PartOfSpeech Parse(string partOfSpeech)
        {
            return partOfSpeech switch
            {
                "n" => PartOfSpeech.Noun,
                "v" => PartOfSpeech.Verb,
                "a" => PartOfSpeech.Adjective,
                "s" => PartOfSpeech.AdjectiveSatellite,
                "r" => PartOfSpeech.Adverb,
                "x" => PartOfSpeech.Other,
                _ => throw new ArgumentException("Bad part of speech value"),
            };
        }
    }
}