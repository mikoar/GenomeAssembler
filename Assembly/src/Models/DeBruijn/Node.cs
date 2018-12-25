using System;

namespace Assembly.Models
{
    internal class Node
    {
        public Node(string kMinus1Mer)
        {
            KMinus1Mer = kMinus1Mer;
        }

        private int numberOfIncomingEdges = 0;
        private int numberOfOutcomingEdges = 0;
        public string KMinus1Mer { get; set; }
        public bool IsSemiBalanced() =>
            Math.Abs(numberOfIncomingEdges - numberOfOutcomingEdges) == 1;

        public bool IsBalanced() =>
            numberOfIncomingEdges == numberOfOutcomingEdges;

        public override string ToString()
        {
            return KMinus1Mer;
        }
    }
}