using System.Collections.Generic;
using System.Linq;
using Assembly.Models;

namespace Assembly.Services
{
    public class DeBruijnGraph
    {
        private IEnumerable<string> _reads;
        private int _kmerLength;

        public DeBruijnGraph(IEnumerable<string> reads, int kmerLength)
        {
            _reads = reads;
            _kmerLength = kmerLength;
        }

        public HashSet<Node> Build()
        {
            var nodes = new HashSet<Node>();
            Node leftKMinus1Mer;
            Node rightKMinus1Mer;

            foreach (var read in _reads)
            {
                foreach (var kmer in SplitToKmers(read, _kmerLength))
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
            }

            return nodes;
        }

        public IEnumerable<string> GetContigs(HashSet<Node> nodes)
        {

            return null;
        }

        private IEnumerable<KMer> SplitToKmers(string s, int kmerLength)
        {
            for (int i = 0; i <= s.Length - kmerLength; i++)
            {
                yield return new KMer(s.Substring(i, kmerLength));
            }
        }
    }
}