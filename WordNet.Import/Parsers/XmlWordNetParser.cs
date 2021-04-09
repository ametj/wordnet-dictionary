using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using WordNet.Data.Model;
using WordNet.Model;
using WordNet.Util;

namespace WordNet.Import.Parsers
{
    public class XmlWordNetParser : IWordNetParser
    {
        public ICollection<LexicalEntry> Parse(string fileName)
        {
            var lexicalEntries = new Dictionary<string, LexicalEntry>();
            var senses = new Dictionary<string, Sense>();
            var synsets = new Dictionary<string, Synset>();

            var document = new XmlDocument();
            document.Load(Path.GetFullPath(fileName));

            foreach (XmlNode lexicon in document.DocumentElement.ChildNodes)
            {
                var language = lexicon.Attributes["language"].Value;

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

                        ParseLexicalEntryContent(lexiconChild, lexicalEntry, senses, synsets);
                    }
                    else if (lexiconChild.Name == "Synset")
                    {
                        var synset = synsets.GetOrAdd(
                            lexiconChild.Attributes["id"].Value,
                            id => new Synset { Id = id, });

                        synset.Ili = lexiconChild.Attributes["ili"].Value;
                        synset.PartOfSpeech = PartOfSpeechExtensions.Parse(lexiconChild.Attributes["partOfSpeech"].Value);

                        ParseSynsetContent(lexiconChild, synset, synsets);
                    }
                }
            }
            return lexicalEntries.Values;
        }

        private void ParseLexicalEntryContent(XmlNode lexiconChild, LexicalEntry lexicalEntry, IDictionary<string, Sense> senses, IDictionary<string, Synset> synsets)
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

                    foreach (XmlNode relationNode in lexicalEntryChild.ChildNodes)
                    {
                        var relation = new SenseRelation
                        {
                            Type = Enum.Parse<SenseRelationType>(relationNode.Attributes["relType"].Value),
                            Target = senses.GetOrAdd(
                                relationNode.Attributes["target"].Value,
                                id => new Sense { Id = id })
                        };
                        sense.Relations.Add(relation);
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

        private void ParseSynsetContent(XmlNode lexiconChild, Synset synset, Dictionary<string, Synset> synsets)
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
                else if (synsetChild.Name == "SynsetRelation")
                {
                    var relation = new SynsetRelation
                    {
                        Type = Enum.Parse<SynsetRelationType>(synsetChild.Attributes["relType"].Value),
                        Target = synsets.GetOrAdd(
                            synsetChild.Attributes["target"].Value,
                            id => new Synset { Id = id })
                    };
                    synset.Relations.Add(relation);
                }
            }
        }
    }
}
