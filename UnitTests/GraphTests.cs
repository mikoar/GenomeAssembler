using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assembly.DeBruijn;
using Assembly.Fasta;
using Xunit;

namespace Assembly.UnitTests
{
    public class GraphTests
    {
        [Theory]
        [InlineData(1, 26, "qwertyuiopasdfghjklzxcvbnm")]
        [InlineData(3, 24, "qwertyuiopasdfg", "qwertyuiopzxcvb")]
        [InlineData(4, 31, "qwertyuiopasdfg", "qwertyuiopzxcvb", "qwertyuioplkjhg")]
        [InlineData(4, 23, "ATGCCGTAATA", "ATGCCGTAGGG", "ATGCCGTACTA")]
        public void Simplify(int mergedNodeCount, int totalMergedNodeValuesLength, params string[] str)
        {
            var k = 4;
            var graphBuilder = new DeBruijnGraphBuilder(k, new TestErrorCorrector(k));
            var graph = graphBuilder.Build(str);

            graph.Simplify();
            Assert.Equal(mergedNodeCount, graph.Nodes.Count());
            Assert.Equal(totalMergedNodeValuesLength, graph.Nodes.Select(n => n.Value).Aggregate((a, b) => a + b).Length);
        }

        [Theory]
        [InlineData(1, "qwertyuiop", "zxc", "vbn")]
        public void RemoveShortChains(int chainsCount, params string[] str)
        {
            var k = 4;
            var graphBuilder = new DeBruijnGraphBuilder(k, new TestErrorCorrector(k));
            var graph = graphBuilder.Build(str);
            graph.Simplify();

            graph.RemoveShortChains();
            Assert.Equal(chainsCount, graph.Nodes.Count());
        }

        [Theory]
        [InlineData(4, 1, "qwertyuiopasdfghjkl", "qwertyuiopasdfghjkl", "qwertyuiopasdfghjkl", "zxcvbasdfghjkl")]
        [InlineData(4, 1, "qwertyuiopasdfghjkl", "qwertyuiopasdfghjkl", "qwertyuiopasdfghjkl", "zxcvbasdfghjkl", "awertyuiop")]
        [InlineData(4, 1, "qwertyuiopasdfghjkl", "qwertyuiopasdfghjkl", "qwertyuiopasdfghjkl", "zxcvbasdfghjkl", "awertyuiop", "qwertyubnm")]
        public void RemoveTips_WithSimplify(int k, int chainsCount, params string[] str)
        {           
            var graphBuilder = new DeBruijnGraphBuilder(k, new TestErrorCorrector(k));
            var graph = graphBuilder.Build(str);
            graph.Simplify();

            graph.RemoveTips();
            Assert.Equal(chainsCount, graph.Nodes.Count());

            graph.Simplify();
            Assert.Single(graph.Nodes);
        }
    }
}

