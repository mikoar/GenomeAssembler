using System;
using System.Collections.Generic;
using System.Linq;

namespace Assembly.DeBruijn
{
    public class Node
    {
        private static int nextId = 0;
        public readonly int id;

        public Node(string kMinus1Mer)
        {
            Value = kMinus1Mer;
            id = nextId++;
        }

        public string Value { get; set; }
        public List<Node> Neighbors { get; private set; } = new List<Node>();
        public List<int> Weights { get; private set; } = new List<int>();

        public int TotalOutcomingWeight { get; set; } = 0;
        public int TotalIncomingWeight { get; set; } = 0;

        public void AddNeighbor(Node to)
        {
            if (Neighbors.Contains(to))
            {
                var index = Neighbors.FindIndex(n => n.Equals(to));
                Weights[index] += 1;
                Neighbors[index].IncrementIncomingEdgesCount();
            }
            else
            {
                to.IncrementIncomingEdgesCount();
                Neighbors.Add(to);
                Weights.Add(1);
            }

            IncrementOutcomingEdgesCount();
        }

        public void Merge(Node neighbor, int k)
        {
            Value += neighbor.Value.Substring(k - 2);
            Neighbors.Clear();
            Weights.Clear();
            Neighbors.AddRange(neighbor.Neighbors);
            Weights.AddRange(neighbor.Weights);
            TotalOutcomingWeight = neighbor.TotalOutcomingWeight;
        }

        public override string ToString()
        {
            return Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var node = obj as Node;
            return node != null && node.Value == Value;
        }

        public bool TryGetPrimaryNeighbor(out Node neighbor)
        {
            if (Neighbors.Any())
            {
                var maxWeightNeighbors = new List<Node>();
                for (int i = 0; i < Weights.Count; i++)
                {
                    if (Weights[i] == Weights.Max())
                    {
                        maxWeightNeighbors.Add(Neighbors[i]);
                    }
                }

                neighbor = maxWeightNeighbors
                    .OrderByDescending(n => n.TotalOutcomingWeight)
                    .ThenByDescending(n => n.Value.Length)
                    .First();

                return true;
            }
            else
            {
                neighbor = null;
                return false;
            }
        }

        internal void CutPrecedingNode(Node tip)
        {
            TotalIncomingWeight -= tip.Weights[tip.Neighbors.IndexOf(this)];
        }

        private void IncrementOutcomingEdgesCount() =>
            TotalOutcomingWeight += 1;

        private void IncrementIncomingEdgesCount() =>
            TotalIncomingWeight += 1;

        private bool IsStartingNode() =>
            TotalIncomingWeight == 0;

    }
}