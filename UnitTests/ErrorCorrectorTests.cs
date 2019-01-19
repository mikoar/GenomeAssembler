using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assembly.DeBruijn;
using Xunit;

namespace Assembly.UnitTests
{
    public class ErrorCorrectorTests

    {
        [Theory]
        [InlineData("CA", 15)]
        [InlineData("CAT", 36)]
        public void GenerateNeighbors(string kmer, int count)
        {
            var corrector = new ErrorCorrector(3);

            var neighbors = corrector.GenerateNeighbors(kmer).ToHashSet();

            Assert.Equal(count, neighbors.Count);
        }

        [Fact]
        public void Histogram()
        {
            var corrector = new ErrorCorrector(3);

            var reads = new List<string>
            {
                string.Join("", Enumerable.Repeat("CAT", 10))
            };

            var histogram = corrector.BuildHistogram(reads);

            var expected = new Dictionary<string, int>
                { { "ATC", 9 },
                    { "CAT", 10 },
                    { "TCA", 9 }
                };

            Assert.Equal(expected, histogram);
        }

        [Theory]
        [InlineData(
            "CATCGTCATCATGATCATCATCAGCATCATCAT",
            "CATCATCATCATCATCATCATCATCATCATCAT",
            3)]
        [InlineData(
            "GGTCATCATCATCATCATCATCATCATCATCAT",
            "CATCATCATCATCATCATCATCATCATCATCAT",
            1)]
        public void Correct(string read, string expectedRead, int numberOfErrors)
        {
            var corrector = new ErrorCorrector(3);

            var reads = new List<string> { read };
            var expectedReads = new List<string> { expectedRead };

            corrector.BuildHistogram(reads);

            var expectedKmers = new TestErrorCorrector(3).CorrectReadsAndSplitToKmers(expectedReads).ToList();
            var correctedKmers = corrector.CorrectReadsAndSplitToKmers(reads).ToList();

            Assert.Equal(expectedKmers, correctedKmers);
            Assert.Equal(numberOfErrors, corrector.CorrectedKmersCount);
        }

    }
}