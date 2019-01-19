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

        [Theory]
        [InlineData("TestData/reads0.fasta")]
        [InlineData("TestData/reads2.fasta")]
        [InlineData("TestData/reads3.fasta")]
        public void Correct_RealData(string fastaPath)
        {
            var assemblyName = "IntegrationTests";
            var projectPath = Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.IndexOf(assemblyName) + assemblyName.Length);
            fastaPath = Path.Combine(projectPath, fastaPath);
            var kmerLength = 19;
            var corrector = new ErrorCorrector(kmerLength);
            var fastaReader = new FastaReader(new FileService());
            var reads = fastaReader.ParseFastaFile(fastaPath);

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