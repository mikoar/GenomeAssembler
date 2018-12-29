using System;
using System.Collections.Generic;
using Assembly.Models;
using Xunit;

namespace Assembly.Tests
{
    public class NodeTests
    {
        [Fact]
        public void AddNeighbor_IncrementsWeightForRepeatedKmers()
        {
            var node1 = new Node("ABB");
            var node2 = new Node("BBA");
            var node3 = new Node("BBA");
            var node4 = new Node("BAA");

            node1.AddNeighbor(node2);
            node1.AddNeighbor(node3);
            node1.AddNeighbor(node4);

            Assert.Contains(node2, node1.Neighbors);
            Assert.Contains(node3, node1.Neighbors);
            Assert.Contains(node4, node1.Neighbors);

            Assert.Equal(2, node1.Neighbors.Count);
            Assert.Equal(2, node1.Weights[0]);
            Assert.Equal(2, node1.Neighbors[0].TotalIncomingWeight);
            Assert.Equal(1, node1.Neighbors[1].TotalIncomingWeight);
            Assert.Equal(3, node1.TotalOutcomingWeight);
        }

    }
}