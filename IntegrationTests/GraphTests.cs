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

            var errorCorrector = new ErrorCorrector();
            var fastaService = new FastaService(fileService);
            Console.WriteLine("Enter .fasta file path");
            var fastaPath = Console.ReadLine();
            var reads = fastaService.ParseFastaFile(fastaPath);
            var histogram = errorCorrector.BuildHistogram(reads);
            var correctedReads = errorCorrector.Correct(reads, histogram);

            var graphService = new DeBruijnGraphService(20);
            var graph = graphService.Build(correctedReads);
            errorCorrector.PrintResult();

            var dotFilePath = Path.Combine(Path.GetDirectoryName(fastaPath), Path.GetFileNameWithoutExtension(fastaPath) + ".dot");
            graphService.ToDot(fileService, dotFilePath, graph);
        }
    }
}