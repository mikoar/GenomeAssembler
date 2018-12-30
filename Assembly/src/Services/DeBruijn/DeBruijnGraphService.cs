using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assembly.Models;

namespace Assembly.Services
{
    public class DeBruijnGraphService
    {
        private int _kmerLength;
        private IErrorCorrector _errorCorrector;
        private Dictionary<string, int> _histogram;

        public DeBruijnGraphService(int kmerLength, IErrorCorrector errorCorrector, Dictionary<string, int> histogram)
        {
            _kmerLength = kmerLength;
            _errorCorrector = errorCorrector;
            _histogram = histogram;
        }

        public HashSet<Node> Build(IEnumerable<string> reads)
        {
            var nodes = new HashSet<Node>();
            int kmerCount = 0;
            Node leftKMinus1Mer;
            Node rightKMinus1Mer;

            Console.WriteLine($"Building graph...");

            foreach (var kmer in _errorCorrector.CorrectReadsAndSplitToKmers(reads, _histogram))
            {
                leftKMinus1Mer = nodes.FirstOrDefault(n =>
                        n.Equals(kmer.LeftKMinus1Mer)) ??
                    kmer.LeftKMinus1Mer;

                rightKMinus1Mer = nodes.FirstOrDefault(n =>
                        n.Equals(kmer.RigthKMinus1Mer)) ??
                    kmer.RigthKMinus1Mer;

                leftKMinus1Mer.AddNeighbor(rightKMinus1Mer);
                nodes.Add(leftKMinus1Mer);
                nodes.Add(rightKMinus1Mer);

                kmerCount += 1;

                if (kmerCount % 500 == 0)
                {
                    Console.WriteLine($"Processed { kmerCount } kmers, { nodes.Count } nodes.");
                }
            }

            Console.WriteLine($"Built graph consisting of { nodes.Count } distinct nodes.");
            return nodes;
        }

        public IEnumerable<string> GetContigs(HashSet<Node> nodes)
        {

            return null;
        }

        public void ToDot(IFileService fileService, string filePath, HashSet<Node> graph, bool hashed = true)
        {
            var content = new StringBuilder();
            content.AppendLine("digraph \"Graph\" {\n  bgcolor=\"transparent\";");

            foreach (var node in graph)
            {
                content.AppendLine($"{ GetNodeText(node, hashed) } [label=\"{ node.KMinus1Mer.First() + "..." + node.KMinus1Mer.Last() }\"];");
                for (int i = 0; i < node.Neighbors.Count; i++)
                {
                    content.AppendLine($"{GetNodeText(node, hashed) } -> { GetNodeText(node.Neighbors[i], hashed) } [label=\"{ node.Weights[i] }\"];");
                }
            }

            content.AppendLine("}");

            fileService.WriteAllText(filePath, content.ToString());
            Console.WriteLine($"Wrote dot file to \"{ filePath }\" ");
        }

        private string GetNodeText(Node node, bool hashed)
        {
            return hashed ? node.GetHashCode().ToString() : node.ToString();
        }
    }
}