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
        [Theory]
        [InlineData("TestData/reads0.fasta")]
        [InlineData("TestData/reads1.fasta")]
        [InlineData("TestData/reads2.fasta")]
        [InlineData("TestData/reads3.fasta")]
        [InlineData("TestData/reads4.fasta")]
        [InlineData("TestData/reads5.fasta")]
        public void BuildGraph_GenerateDotFile_WriteContigs(string fastaPath)
        {
            var assemblyName = "IntegrationTests";
            var projectPath = Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.IndexOf(assemblyName) + assemblyName.Length);
            fastaPath = Path.Combine(projectPath, fastaPath);
            var fileService = new FileService();
            var kmerLength = 19;
            var errorCorrector = new ErrorCorrector(kmerLength);
            var fastaReader = new FastaReader(fileService);

            var reads = fastaReader.ParseFastaFile(fastaPath);
            errorCorrector.BuildHistogram(reads);

            var graphBuilder = new DeBruijnGraphBuilder(kmerLength, errorCorrector);
            var graph = graphBuilder.Build(reads);
            errorCorrector.PrintResult();

            graph.CleanUp();
            var dotFileDirectory = Path.Combine(Path.GetDirectoryName(fastaPath), "graphs");
            Directory.CreateDirectory(dotFileDirectory);
            graphBuilder.ToDot(fileService, Path.Combine(dotFileDirectory, Path.GetFileNameWithoutExtension(fastaPath) + ".dot"), graph);

            var contigs = graph.GetContigs();
            var contigsDirectory = Path.Combine(Path.GetDirectoryName(fastaPath), "contigs");
            Directory.CreateDirectory(contigsDirectory);
            fastaReader.WriteFastaFile(Path.Combine(contigsDirectory, Path.GetFileNameWithoutExtension(fastaPath) + ".contigs.fasta"), contigs);
        }
    }
}