using System;
using System.Collections.Generic;
using System.Linq;

namespace Assembly.Models
{
    public class Node
    {

        public Node(string kMinus1Mer)
        {
            KMinus1Mer = kMinus1Mer;
        }

        public string KMinus1Mer { get; set; }
        public List<Node> Neighbors { get; private set; } = new List<Node>();
        public List<int> Costs { get; private set; } = new List<int>();
        public int TotalOutcomingCost { get; private set; } = 0;
        public int TotalIncomingCost { get; private set; } = 0;

        public void AddNeighbor(Node to)
        {
            if (Neighbors.Contains(to))
            {
                var index = Neighbors.FindIndex(n => n.Equals(to));
                Costs[index] += 1;
                Neighbors[index].IncrementIncomingEdgesCount();
            }
            else
            {
                to.IncrementIncomingEdgesCount();
                Neighbors.Add(to);
                Costs.Add(1);
            }

            IncrementOutcomingEdgesCount();
        }

        public void IncrementIncomingEdgesCount() =>
            TotalIncomingCost += 1;

        public bool IsSemiBalanced() =>
            Math.Abs(TotalIncomingCost - TotalOutcomingCost) == 1;

        public bool IsBalanced() =>
            TotalIncomingCost == TotalOutcomingCost;

        public override string ToString()
        {
            return KMinus1Mer;
        }

        public override int GetHashCode()
        {
            return KMinus1Mer.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var node = obj as Node;
            return node != null && node.KMinus1Mer == KMinus1Mer;
        }

        private void IncrementOutcomingEdgesCount() =>
            TotalOutcomingCost += 1;
    }
}