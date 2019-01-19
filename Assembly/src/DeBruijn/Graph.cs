using System;
using System.Collections.Generic;
using System.Linq;
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
            var allFrontTips = _graph
                .Where(n => n.TotalIncomingWeight == 0)
                .Where(n => n.Neighbors.Count == 1);

            var frontTipsToRemove = allFrontTips
                .GroupBy(n => n.Neighbors.Single(),
                    n => n,
                    (sharedNeighbor, nodes) =>
                    nodes.OrderByDescending(n => n.TotalOutcomingWeight).Skip(1))
                .SelectMany(secondaryTips => secondaryTips.AsEnumerable())
                .Where(secondaryTip => secondaryTip.Value.Length < _cutoffLength)
                .ToList();

            foreach (var tip in frontTipsToRemove)
            {
                tip.Neighbors.Single().CutPrecedingNode(tip);
                Remove(tip);
            }

            Console.WriteLine($"Removed {frontTipsToRemove.Count()} tips...");
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
            return node.Neighbors.Count == 1 && _graph.SelectMany(n => n.Neighbors).Count(n => n == node.Neighbors.Single()) == 1;
        }
    }
}