using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assembly.Models;
using Assembly.Services;
using Xunit;

namespace Assembly.Tests
{
    public class ErrorCorrectorTests

    {
        [Fact]
        public void GenerateNeighbors()
        {
            var corrector = new ErrorCorrector(3, new char[] { 'A', 'T', 'G', 'C' });

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

            var histogram = corrector.Histogram(reads);

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

            var histogram = corrector.Histogram(reads);

            var corrected = corrector.Correct(reads, histogram, 5).ToList();

            var expected = new List<string>
            {
                string.Join("", Enumerable.Repeat("CAT", 11))
            };

            Assert.Equal(expected, corrected);
            Assert.Equal(3, corrector.NumberOfCorrections);
        }

        [Fact(Skip = "requires file with reads")]
        public void Correct_RealData_BelieveItWorks()
        {
            var corrector = new ErrorCorrector(6);
            var fastaService = new FastaService(new FileReader());
            Console.WriteLine("Enter .fasta file path");
            var path = Console.ReadLine();

            var histogram = corrector.Histogram(fastaService.ParseFastaFile(path));
            var kmerCounts = histogram.Values.Distinct().OrderBy(i => i).ToArray();

            var distinctKmersWithGivenCount = new int[kmerCounts.Length];

            for (int i = 0; i < kmerCounts.Length; i++)
            {
                distinctKmersWithGivenCount[i] = histogram.Values.Count(v => v == kmerCounts[i]);
            }

            var corrected = corrector.Correct(fastaService.ParseFastaFile(path), histogram, 5).ToList();
        }
    }
}