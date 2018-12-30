using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assembly.Models;
using Assembly.Services;
using Xunit;

namespace Assembly.IntegrationTests
{
    public class GraphTests
    {
        [Fact]
        public void BuildGraphAndGenerateDotFile_RealData()
        {
            var fileService = new FileService();
            var kmerLength = 19;
            var errorCorrector = new ErrorCorrector(kmerLength);
            var fastaService = new FastaService(fileService);
            Console.WriteLine("Enter .fasta file path");
            var fastaPath = Console.ReadLine();

            var reads = fastaService.ParseFastaFile(fastaPath);
            var histogram = errorCorrector.BuildHistogram(reads);
            var correctedKmers = errorCorrector.CorrectReadsAndSplitToKmers(reads, histogram);

            var graphService = new DeBruijnGraphService(kmerLength, errorCorrector, histogram);
            var graph = graphService.Build(correctedKmers.Select(k => k.ToString()));
            errorCorrector.PrintResult();

            var dotFilePath = Path.Combine(Path.GetDirectoryName(fastaPath), "graphs", Path.GetFileNameWithoutExtension(fastaPath) + ".dot");
            graphService.ToDot(fileService, dotFilePath, graph);
        }
    }
}