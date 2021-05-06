using System;
using System.Collections.Generic;
using System.Xml;
using WordNet.Data.Model;
using WordNet.Import.Model;
using WordNet.Util;

namespace WordNet.Import.Parsers
{
    public class XmlWordNetParser : IWordNetParser
    {
        public ParseResult Parse(string fileName, bool loadRelations)
        {
            var result = new ParseResult();
            var lexicalEntries = new Dictionary<string, LexicalEntry>();
            var senses = new Dictionary<string, Sense>();
            var synsets = new Dictionary<string, Synset>();

            var document = new XmlDocument();
            document.Load(fileName);

            foreach (XmlNode lexicon in document.DocumentElement.ChildNodes)
            {
                var language = lexicon.Attributes["language"].Value;

                Console.WriteLine($"Started processing lexicon \"{lexicon.Attributes["label"].Value} ({language})\".");

                foreach (XmlNode lexiconChild in lexicon.ChildNodes)
                {
                    if (lexiconChild.Name == "LexicalEntry")
                    {
                        var lexicalEntry = new LexicalEntry
                        {
                            Id = lexiconChild.Attributes["id"].Value,
                            Language = language,
                        };
                        lexicalEntries.Add(lexicalEntry.Id, lexicalEntry);

                        ParseLexicalEntryContent(lexiconChild, lexicalEntry, senses, synsets, loadRelations);
                    }
                    else if (lexiconChild.Name == "Synset")
                    {
                        var synset = synsets.GetOrAdd(
                            lexiconChild.Attributes["id"].Value,
                            id => new Synset { Id = id, });

                        synset.Ili = lexiconChild.Attributes["ili"].Value;
                        synset.PartOfSpeech = PartOfSpeechExtensions.Parse(lexiconChild.Attributes["partOfSpeech"].Value);

                        ParseSynsetContent(lexiconChild, synset, loadRelations);
                    }
                }
            }
            Console.WriteLine($"Loaded {lexicalEntries.Count} lexical entries, {senses.Count} senses and {synsets.Count} synsets.");

            result.LexicalEntries = lexicalEntries.Values;
            if (loadRelations)
            {
                var senseRelations = new List<SenseRelation>();
                var synsetRelations = new List<SynsetRelation>();

                foreach (var sense in senses.Values)
                {
                    senseRelations.AddRange(sense.Relations);
                    sense.Relations = null;
                }
                foreach (var synset in synsets.Values)
                {
                    synsetRelations.AddRange(synset.Relations);
                    synset.Relations = null;
                }
                result.SenseRelations = senseRelations;
                result.SynsetRelations = synsetRelations;
                Console.WriteLine($"Loaded {senseRelations.Count} sense relations, {synsetRelations.Count} synset relations.");
            }

            return result;
        }

        private static void ParseLexicalEntryContent(XmlNode lexiconChild, LexicalEntry lexicalEntry, IDictionary<string, Sense> senses, IDictionary<string, Synset> synsets, bool loadRelations)
        {
            foreach (XmlNode lexicalEntryChild in lexiconChild.ChildNodes)
            {
                if (lexicalEntryChild.Name == "Lemma")
                {
                    lexicalEntry.Lemma = lexicalEntryChild.Attributes["writtenForm"].Value;
                    lexicalEntry.PartOfSpeech = PartOfSpeechExtensions.Parse(lexicalEntryChild.Attributes["partOfSpeech"].Value);
                }
                else if (lexicalEntryChild.Name == "Form")
                {
                    lexicalEntry.Forms.Add(lexicalEntryChild.Attributes["writtenForm"].Value);
                }
                else if (lexicalEntryChild.Name == "Sense")
                {
                    var sense = senses.GetOrAdd(
                        lexicalEntryChild.Attributes["id"].Value,
                        id => new Sense { Id = id });

                    sense.LexicalEntry = lexicalEntry;
                    sense.Synset = synsets.GetOrAdd(
                        lexicalEntryChild.Attributes["synset"].Value,
                        id => new Synset { Id = id });

                    if (loadRelations)
                    {
                        foreach (XmlNode relationNode in lexicalEntryChild.ChildNodes)
                        {
                            var relation = new SenseRelation
                            {
                                Type = Enum.Parse<SenseRelationType>(relationNode.Attributes["relType"].Value),
                                SourceId = sense.Id,
                                TargetId = relationNode.Attributes["target"].Value
                            };
                            sense.Relations.Add(relation);
                        }
                    }

                    lexicalEntry.Senses.Add(sense);
                }
                else if (lexicalEntryChild.Name == "SyntacticBehaviour")
                {
                    lexicalEntry.SyntacticBehaviours.Add(new SyntacticBehaviour
                    {
                        Senses = lexicalEntryChild.Attributes["senses"].Value,
                        SubcategorizationFrame = lexicalEntryChild.Attributes["subcategorizationFrame"].Value
                    });
                }
            }
        }

        private static void ParseSynsetContent(XmlNode lexiconChild, Synset synset, bool loadRelations)
        {
            foreach (XmlNode synsetChild in lexiconChild.ChildNodes)
            {
                if (synsetChild.Name == "Definition")
                {
                    synset.Definitions.Add(synsetChild.InnerText);
                }
                else if (synsetChild.Name == "Example")
                {
                    synset.Examples.Add(synsetChild.InnerText);
                }
                else if (loadRelations && synsetChild.Name == "SynsetRelation")
                {
                    var relation = new SynsetRelation
                    {
                        Type = Enum.Parse<SynsetRelationType>(synsetChild.Attributes["relType"].Value),
                        SourceId = synset.Id,
                        TargetId = synsetChild.Attributes["target"].Value
                    };
                    synset.Relations.Add(relation);
                }
            }
        }
    }
}