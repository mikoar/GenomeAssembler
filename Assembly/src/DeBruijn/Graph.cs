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

        public int Count => _graph.Count;
        public IEnumerable<Node> Nodes => _graph.AsEnumerable();

        public Graph(int k)
        {
            _graph = new HashSet<Node>();
            K = k;
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
            Simplify();
            RemoveShortChains();
            RemoveTips();
            Simplify();
        }

        public void Simplify()
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
        }

        public void RemoveShortChains()
        {
            Console.WriteLine($"Removing chains shrorter than {_cutoffLength}...");
            var shortChains = _graph
                .Where(n => n.Value.Length < _cutoffLength)
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
            RemoveFrontTips();

            // var splittingNodes = _graph
            //     .Where(n => n.Neighbors.Count > 1);

            // foreach (var splittingNode in splittingNodes)
            // {
            //     var maxWeight = splittingNode.Weights.Max();

            //     var neighbors = splittingNode.Neighbors
            //         .Where(n => n.Value.Length < _cutoffLength);

            //     // var joiningNodes = _graph
            //     //     .SelectMany(n => n.Neighbors)
            //     //     .GroupBy(n => n,
            //     //         n => n,
            //     //         (sharedNeighbor, nodes) =>
            //     //         nodes.OrderByDescending(n => n.TotalOutcomingWeight).Skip(1))
            //     //     .SelectMany(secondaryTips => secondaryTips.AsEnumerable())
            //     //     .ToList();

            // }
        }

        private void RemoveFrontTips()
        {
            var frontTips = _graph
                .Where(n => n.TotalIncomingWeight == 0)
                .Where(n => n.Neighbors.Count == 1)
                .Where(n => n.Value.Length < _cutoffLength);

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
                    .Where(n => n != frontTip);

                if (frontTip.TotalOutcomingWeight < nodesWithSameNeighbor.Max(n => n.TotalOutcomingWeight) ||
                    (frontTip.TotalOutcomingWeight == nodesWithSameNeighbor.Max(n => n.TotalOutcomingWeight) &&
                        nodesWithSameNeighbor.All(n => n.TotalIncomingWeight > 0)))
                {
                    frontTipsToRemove.Add(frontTip);
                }
            }

            foreach (var tip in frontTipsToRemove)
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

        public IEnumerable<string> GetContigs(int minLength = 300)
        {
            var startNodes = _graph.Where(n => n.TotalIncomingWeight == 0);
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

                if (contig.Length >= minLength)
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

            Console.WriteLine($"Assembled {returned} contigs with minimum length of { minLength }. { rejected } contigs were rejected. Total contigs length is { totalLength }.");
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
                throw new GraphException("Could not remove node");
            }
        }

        private bool CanBeMergedWithNeighbor(Node node)
        {
            return node.Neighbors.Count == 1 && node.Neighbors.Single().TotalIncomingWeight == node.TotalOutcomingWeight;
        }
    }
}