using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assembly.DeBruijn;
using Assembly.Fasta;
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
            var fastaService = new FastaReader(fileService);
            Console.WriteLine("Enter .fasta file path");
            var fastaPath = Console.ReadLine();

            var reads = fastaService.ParseFastaFile(fastaPath);
            var histogram = errorCorrector.BuildHistogram(reads);
            var correctedKmers = errorCorrector.CorrectReadsAndSplitToKmers(reads);

            var graphService = new DeBruijnGraphBuilder(kmerLength, errorCorrector);
            var graph = graphService.Build(correctedKmers.Select(k => k.ToString()));
            errorCorrector.PrintResult();

            var dotFilePath = Path.Combine(Path.GetDirectoryName(fastaPath), "graphs", Path.GetFileNameWithoutExtension(fastaPath) + ".dot");
            graphService.ToDot(fileService, dotFilePath, graph);
        }
    }
}