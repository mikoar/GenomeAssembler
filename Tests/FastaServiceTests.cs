using System;
using System.Collections.Generic;
using System.Linq;
using Assembly.Services;
using Xunit;

namespace Assembly.Tests
{
    public class FastaServiceTests
    {
        [Fact]
        public void ParseFastaFile_ReturnsCorrectSequences()
        {
            var fileReader = new TestFileReader();
            var fastaService = new FastaService(fileReader);

            var sequences = fastaService.ParseFastaFile("path").ToList();

            Assert.Equal(10, sequences.Count);
            Assert.All(sequences, s => Assert.Equal(80, s.Length));
        }

        [Fact]
        public void ParseFastaFile_EmptyFile_ThrowsArgumentException()
        {
            var fileReader = new TestFileReader();
            fileReader.fileLines = new List<string>();
            var fastaService = new FastaService(fileReader);

            Assert.Throws<ArgumentException>(() =>
                fastaService.ParseFastaFile("path").ToList());

        }

        [Fact]
        public void ParseFastaFile_NoSequencesInFile_ThrowsArgumentException()
        {
            var fileReader = new TestFileReader();
            fileReader.fileLines = new List<string>()
            {
                "this is definitely not a fasta file",
                "ATCGCTGJGJGNVBCACABJP",
                "dvdfvdf fsdfs dfsdfsgdf"
            };
            var fastaService = new FastaService(fileReader);

            Assert.Throws<ArgumentException>(() =>
                fastaService.ParseFastaFile("path").ToList());

        }
    }
}