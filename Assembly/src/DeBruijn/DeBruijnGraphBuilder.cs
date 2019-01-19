using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assembly.Fasta;

namespace Assembly.DeBruijn
{
    public class DeBruijnGraphBuilder
    {
        private int _kmerLength;
        private IErrorCorrector _errorCorrector;

        public DeBruijnGraphBuilder(int kmerLength, IErrorCorrector errorCorrector)
        {
            _kmerLength = kmerLength;
            _errorCorrector = errorCorrector;
        }

        public Graph Build(IEnumerable<string> reads)
        {
            var graph = new Graph(_kmerLength);
            int kmerCount = 0;
            Node leftKMinus1Mer;
            Node rightKMinus1Mer;

            Console.WriteLine($"Building graph...");

            foreach (var kmer in _errorCorrector.CorrectReadsAndSplitToKmers(reads))
            {
                leftKMinus1Mer = graph.GetNewOrExistingNode(kmer.LeftKMinus1Mer);
                rightKMinus1Mer = graph.GetNewOrExistingNode(kmer.RigthKMinus1Mer);

                leftKMinus1Mer.AddNeighbor(rightKMinus1Mer);

                graph.AddOrUpdateNode(leftKMinus1Mer);
                graph.AddOrUpdateNode(rightKMinus1Mer);

                kmerCount += 1;

                if (kmerCount % 500 == 0)
                {
                    Console.WriteLine($"Processed { kmerCount } kmers, { graph.Count } nodes.");
                }
            }

            Console.WriteLine($"Built graph consisting of { graph.Count } distinct nodes.");
            return graph;
        }

        public void ToDot(IFileService fileService, string filePath, Graph graph)
        {
            var content = new StringBuilder();
            content.AppendLine("digraph \"Graph\" {\n  bgcolor=\"transparent\";");

            foreach (var node in graph.Nodes)
            {
                content.AppendLine($"{ node.id } [label=\"{ node.Value.First() + "..." + node.Value.Last() }\"];");
                for (int i = 0; i < node.Neighbors.Count; i++)
                {
                    content.AppendLine($"{node.id } -> { node.Neighbors[i].id } [label=\"{ node.Weights[i] }\"];");
                }
            }

            content.AppendLine("}");

            fileService.WriteAllText(filePath, content.ToString());
            Console.WriteLine($"Wrote dot file to \"{ filePath }\" ");
        }
    }
}