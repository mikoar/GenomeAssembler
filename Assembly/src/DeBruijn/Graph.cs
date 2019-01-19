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

        public void Simplify()
        {
            bool merged = true;

            while (merged)
            {
                foreach (var node in _graph)
                {
                    merged = TryMerge(node);

                    if (merged)
                    {
                        break;
                    }
                }
            }
        }

        private bool TryMerge(Node node)
        {
            if (CanBeMergedWithNeighbor(node))
            {
                var neighbor = node.Neighbors.Single();
                node.Merge(neighbor, K);

                var removed = _graph.Remove(neighbor);
                if (!removed)
                {
                    throw new GraphException();
                }

                return true;
            }

            return false;
        }

        private bool CanBeMergedWithNeighbor(Node node)
        {
            return node.Neighbors.Count == 1 && _graph.SelectMany(n => n.Neighbors).Count(n => n == node.Neighbors.Single()) == 1;
        }
    }
}