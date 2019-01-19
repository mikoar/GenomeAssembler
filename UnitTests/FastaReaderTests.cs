using System;
using System.Collections.Generic;
using System.Linq;
using Assembly.Fasta;
using Xunit;

namespace Assembly.UnitTests
{
    public class FastaReaderTests
    {
        [Fact]
        public void ParseFastaFile_ReturnsCorrectSequences()
        {
            var fileReader = new TestFileService();
            var fastaReader = new FastaReader(fileReader);

            var sequences = fastaReader.ParseFastaFile("path").ToList();

            Assert.Equal(10, sequences.Count);
            Assert.All(sequences, s => Assert.Equal(80, s.Length));
        }

        [Fact]
        public void ParseFastaFile_EmptyFile_ThrowsArgumentException()
        {
            var fileReader = new TestFileService();
            fileReader.fileLines = new List<string>();
            var fastaReader = new FastaReader(fileReader);

            Assert.Throws<ArgumentException>(() =>
                fastaReader.ParseFastaFile("path").ToList());

        }

        [Fact]
        public void ParseFastaFile_NoSequencesInFile_ThrowsArgumentException()
        {
            var fileReader = new TestFileService();
            fileReader.fileLines = new List<string>()
            {
                "this is definitely not a fasta file",
                "ATCGCTGJGJGNVBCACABJP",
                "dvdfvdf fsdfs dfsdfsgdf"
            };
            var fastaReader = new FastaReader(fileReader);

            Assert.Throws<ArgumentException>(() =>
                fastaReader.ParseFastaFile("path").ToList());

        }
    }
}