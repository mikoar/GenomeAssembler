using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assembly.Models;
using Assembly.Services;
using Xunit;

namespace Assembly.UnitTests
{
    public class ErrorCorrectorTests

    {
        [Fact]
        public void GenerateNeighbors()
        {
            var corrector = new ErrorCorrector(3);

            var neighbors = corrector.GenerateNeighbors("CAT").ToHashSet();

            var expected = new HashSet<string>
            {
                "CAA",
                "CAC",
                "CAG",
                "CCT",
                "CGT",
                "CTT",
                "AAT",
                "GAT",
                "TAT"
            };
            Assert.Equal(expected, neighbors);
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

        [Fact]
        public void Correct()
        {
            var corrector = new ErrorCorrector(3);

            var reads = new List<string>
            {
                "CATCGTCATCATGATCATCATCAGCATCATCAT"

            };

            var histogram = corrector.BuildHistogram(reads);

            var corrected = corrector.Correct(reads, histogram, 5).ToList();

            var expected = new List<string>
            {
                string.Join("", Enumerable.Repeat("CAT", 11))
            };

            Assert.Equal(expected, corrected);
            Assert.Equal(3, corrector.CorrectedKmersCount);
        }

    }
}