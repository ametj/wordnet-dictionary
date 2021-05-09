using System.Linq;
using WordNet.Data.Model;
using WordNet.Import.Parsers;
using Xunit;

namespace WordNet.Test
{
    public class WordNetParserTests
    {
        [Fact]
        public void XmlWordNetParser()
        {
            var parser = new XmlWordNetParser();

            var result = parser.Parse("TestData/wordnet.xml", true);
            
            Assert.NotNull(result);
            Assert.Equal(2, result.LexicalEntries.Count());

            var lexicalEntry = result.LexicalEntries.First();
            Assert.Equal("ewn-wordnet-n", lexicalEntry.Id);
            Assert.Equal("en", lexicalEntry.Language);
            Assert.Equal("wordnet", lexicalEntry.Lemma);
            Assert.Equal(PartOfSpeech.Noun, lexicalEntry.PartOfSpeech);
            Assert.Single(lexicalEntry.Senses);

            var sense = lexicalEntry.Senses.First();
            Assert.Equal("ewn-wordnet-n-06652077-01", sense.Id);
            Assert.Equal("01", sense.PositionInSynset);
            Assert.NotNull(sense.Synset);

            var synset = sense.Synset;
            Assert.Equal("ewn-06652077-n", synset.Id);
            Assert.Equal("any of the machine-readable lexical databases modeled after the Princeton WordNet", synset.Definitions.First());
            Assert.Equal(PartOfSpeech.Noun, synset.PartOfSpeech);

            Assert.Single(result.SenseRelations);
            Assert.Equal(4, result.SynsetRelations.Count());
        }
    }
}
