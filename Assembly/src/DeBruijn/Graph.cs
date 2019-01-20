using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assembly.DeBruijn.Exceptions;

namespace Assembly.DeBruijn
{
    public class Graph
    {
        private HashSet<Node> _graph;
        public int K { get; private set; }
        private int _cutoffLength => 2 * K;
        private readonly int _minContigLength;

        public int Count => _graph.Count;
        public IEnumerable<Node> Nodes => _graph.AsEnumerable();

        public Graph(int k, int minContigLength = 300)
        {
            _graph = new HashSet<Node>();
            K = k;
            _minContigLength = minContigLength;
        }

        public Node GetNewOrExistingNode(Node node)
        {
            return _graph.FirstOrDefault(n =>
                    n.Equals(node)) ??
                node;
        }

        public void AddOrUpdateNode(Node node)
        {
            _graph.Add(node);
        }

        public void CleanUp()
        {
            var mergeCount = Simplify();
            do
            {
                RemoveLowWeightEdges();
                // RemoveShortChains();
                RemoveTips();
                mergeCount = Simplify();

            } while (mergeCount != 0);
        }

        ///temporary solution
        private void RemoveLowWeightEdges()
        {
            foreach (var node in _graph)
            {
                if (!node.Neighbors.Any())
                {
                    continue;
                }

                var max = node.Weights.Max();
                var maxWeightEdgesCount = node.Weights.Count(w => w == max);

                if (maxWeightEdgesCount > 1 || max == 1)
                {
                    for (int j = 0; j < node.Weights.Count; j++)
                    {
                        node.Neighbors[j].TotalIncomingWeight -= node.Weights[j];
                    }

                    node.Neighbors.Clear();
                    node.Weights.Clear();
                    node.TotalOutcomingWeight = 0;
                }
                else if (maxWeightEdgesCount == 1)
                {
                    var i = node.Weights.IndexOf(max);
                    for (int j = 0; j < node.Weights.Count; j++)
                    {
                        if (i == j) { continue; }
                        else
                        {
                            node.Neighbors[j].TotalIncomingWeight -= node.Weights[j];
                            node.Neighbors.RemoveAt(j);
                            node.Weights.RemoveAt(j);
                        }
                    }

                    node.TotalOutcomingWeight = max;
                }
            }
        }

        public int Simplify()
        {
            Console.WriteLine($"Merging nodes...");
            var mergeCount = 0;
            bool merged = true;

            while (merged)
            {
                foreach (var node in _graph)
                {
                    merged = TryMerge(node);

                    if (merged)
                    {
                        mergeCount++;
                        break;
                    }
                }
            }

            Console.WriteLine($"Merged {mergeCount} times...");

            return mergeCount;
        }

        public void RemoveShortChains()
        {
            Console.WriteLine($"Removing chains shrorter than {_minContigLength}...");
            var shortChains = _graph
                .Where(n => n.Value.Length < _minContigLength)
                .Where(n => n.TotalIncomingWeight == 0 && n.TotalOutcomingWeight == 0)
                .ToArray();

            foreach (var node in shortChains)
            {
                Remove(node);
            }

            Console.WriteLine($"Removed {shortChains.Count()} chains...");
        }

        public void RemoveTips()
        {
            Console.WriteLine("Removing tips...");

            var frontTips = _graph
                .Except(_graph
                    .SelectMany(n => n.Neighbors))
                .Where(n => n.Neighbors.Count == 1);

            var frontTipsToRemove = new HashSet<Node>();

            foreach (var frontTip in frontTips)
            {
                if (frontTipsToRemove.Contains(frontTip))
                {
                    continue;
                }

                var neighbor = frontTip.Neighbors.Single();
                var nodesWithSameNeighbor = _graph
                    .Where(n => n.Neighbors.Contains(neighbor))
                    .Where(n => n.Neighbors.Count == 1)
                    .OrderByDescending(n => n.Weights.Single());

                foreach (var n in nodesWithSameNeighbor)
                {
                    frontTipsToRemove.Add(n);
                }
            }

            foreach (var tip in frontTipsToRemove.Distinct())
            {
                RemoveFrontTip(tip);
            }

            Console.WriteLine($"Removed {frontTipsToRemove.Count()} tips...");
        }

        private void RemoveFrontTip(Node tip)
        {
            tip.Neighbors.Single().CutPrecedingNode(tip);
            Remove(tip);
        }

        public IEnumerable<string> GetContigs()
        {
            var startNodes = _graph
                .Except(_graph
                    .SelectMany(n => n.Neighbors));
            var returned = 0;
            var rejected = 0;
            var totalLength = 0;
            foreach (var startNode in startNodes)
            {
                var contig = new StringBuilder();
                var node = startNode;
                contig.Append(node.Value);
                while (node.TryGetPrimaryNeighbor(out node))
                {
                    contig.Append(node.Value.Substring(0, K - 2));
                }

                if (contig.Length >= _minContigLength)
                {
                    returned += 1;
                    totalLength += contig.Length;
                    yield return contig.ToString();
                }
                else
                {
                    rejected += 1;
                }
            }

            Console.WriteLine($"Assembled {returned} contigs with minimum length of { _minContigLength }. { rejected } contigs were rejected. Total contigs length is { totalLength }.");
        }

        private bool TryMerge(Node node)
        {
            if (CanBeMergedWithNeighbor(node))
            {
                Remove(node);
                var neighbor = node.Neighbors.Single();
                node.Merge(neighbor, K);
                Remove(neighbor);
                _graph.Add(node);

                return true;
            }

            return false;
        }

        private void Remove(Node node)
        {
            var removed = _graph.Remove(node);
            if (!removed)
            {
                //;_;
                //throw new GraphException("Could not remove node");
            }
        }

        private bool CanBeMergedWithNeighbor(Node node)
        {
            return node.Neighbors.Count == 1 && node.Neighbors.Single().TotalIncomingWeight == node.TotalOutcomingWeight;
        }
    }
}