using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Assembly.DeBruijn;
using Assembly.Fasta;
using McMaster.Extensions.CommandLineUtils;

namespace Assembly
{
    [VersionOption("0.1.0")]
    [Command(Name = "assembly", Description = "A contigs assembler."), HelpOption]
    public class Program
    {
        public static int Main(string[] args) =>
            CommandLineApplication.Execute<Program>(args);

        [FileExists]
        [Required]
        [Argument(0, Name = "reads", Description = "fasta file with reads")]
        public string ReadsPath { get; }

        [Required]
        [Argument(1, Description = "contigs output file")]
        public string ContigsPath { get; }

        [Option(ShortName = "d", LongName = "dot", Description = "output graph to dot file")]
        public string DotFilePath { get; }

        [Option(ShortName = "k", Description = "Mer length, default is 19")]
        public int? K { get; }

        private int OnExecute()
        {
            int result = 0;

            try
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Run();
            }
            catch (System.Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write($"An error occured ;_;\n{ex.Message}\n");
                result = 1;
            }
            Console.ResetColor();

            return result;
        }

        private void Run()
        {
            var fileService = new FileService();
            var kmerLength = K ?? 19;
            var errorCorrector = new ErrorCorrector(kmerLength);
            var fastaReader = new FastaReader(fileService);

            var reads = fastaReader.ParseFastaFile(ReadsPath);
            errorCorrector.BuildHistogram(reads);

            var graphBuilder = new DeBruijnGraphBuilder(kmerLength, errorCorrector);
            var graph = graphBuilder.Build(reads);
            errorCorrector.PrintResult();

            graph.CleanUp();

            if (!string.IsNullOrWhiteSpace(DotFilePath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(DotFilePath));
                graphBuilder.ToDot(fileService, DotFilePath, graph);
            }

            var contigs = graph.GetContigs();
            Directory.CreateDirectory(Path.GetDirectoryName(ContigsPath));
            fastaReader.WriteFastaFile(ContigsPath, contigs);
        }
    }
}