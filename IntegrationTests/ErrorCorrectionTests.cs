using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assembly.Models;
using Assembly.Services;
using Xunit;

namespace Assembly.IntegrationTests
{
    public class ErrorCorrectionTests
    {

        [Fact]
        public void Correct_RealData()
        {
            var corrector = new ErrorCorrector(6);
            var fastaService = new FastaService(new FileService());
            Console.WriteLine("Enter .fasta file path");
            var path = Console.ReadLine();

            var histogram = corrector.BuildHistogram(fastaService.ParseFastaFile(path));

            Console.WriteLine("Before correction");
            WriteLowCountDistinctKmersCount(histogram, 12);

            var histogramCorrected = corrector.BuildHistogram(corrector.Correct(fastaService.ParseFastaFile(path), histogram, 5));

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