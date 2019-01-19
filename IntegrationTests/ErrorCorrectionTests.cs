using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assembly.DeBruijn;
using Assembly.Fasta;
using Xunit;

namespace Assembly.IntegrationTests
{
    public class ErrorCorrectionTests
    {

        [Fact]
        public void Correct_RealData()
        {
            var kmerLength = 19;
            var corrector = new ErrorCorrector(kmerLength);
            var fastaService = new FastaReader(new FileService());
            Console.WriteLine("Enter .fasta file path");
            var path = Console.ReadLine();
            var reads = fastaService.ParseFastaFile(path);

            var histogram = corrector.BuildHistogram(reads);

            Console.WriteLine("Before correction");
            WriteLowCountDistinctKmersCount(histogram, 12);

            var kmersCorrected = corrector.CorrectReadsAndSplitToKmers(reads);
            var histogramCorrected = corrector.BuildHistogram(kmersCorrected.Select(k => k.ToString()));

            Console.WriteLine("After correction");
            WriteLowCountDistinctKmersCount(histogramCorrected, 12);
            corrector.PrintResult();
        }

        private void WriteLowCountDistinctKmersCount(Dictionary<string, int> histogram, int threshold)
        {
            var kmerCounts = histogram.Values.Distinct().OrderBy(i => i).Take(threshold).ToArray();

            var distinctKmersWithGivenCount = new int[kmerCounts.Length];

            for (int i = 0; i < kmerCounts.Length; i++)
            {
                distinctKmersWithGivenCount[i] = histogram.Values.Count(v => v == kmerCounts[i]);
            }

            Console.WriteLine("#\tk-mers");
            for (int i = 0; i < kmerCounts.Length; i++)
            {
                Console.WriteLine($"{ kmerCounts[i] }\t{ distinctKmersWithGivenCount[i] }");
            }
        }
    }
}