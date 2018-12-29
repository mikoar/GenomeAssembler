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

        public DeBruijnGraphService(int kmerLength)
        {
            _kmerLength = kmerLength;
        }

        public HashSet<Node> Build(IEnumerable<string> reads)
        {
            var nodes = new HashSet<Node>();
            int readCount = 0;
            Node leftKMinus1Mer;
            Node rightKMinus1Mer;

            Console.WriteLine($"Building graph...");

            foreach (var read in reads)
            {
                foreach (var kmer in SplitToKmers(read))
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
                }

                readCount += 1;
                if (readCount % 100 == 0)
                {
                    Console.WriteLine($"Processed { readCount } reads, { nodes.Count } nodes.");
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

        private IEnumerable<KMer> SplitToKmers(string s)
        {
            for (int i = 0; i <= s.Length - _kmerLength; i++)
            {
                yield return new KMer(s.Substring(i, _kmerLength));
            }
        }

        private string GetNodeText(Node node, bool hashed)
        {
            return hashed ? node.GetHashCode().ToString() : node.ToString();
        }
    }
}