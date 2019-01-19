using System;
using System.Collections.Generic;
using Assembly.DeBruijn;
using Xunit;

namespace Assembly.UnitTests
{
    public class DeBruijnGraphBuilderTests
    {
        [Fact]
        public void Build_SimpleString()
        {
            var sequences = new List<string>()
            {
                "a_long_long_long_time",
            };

            var graph = new DeBruijnGraphBuilder(4, new TestErrorCorrector(4))
                .Build(sequences);

            Assert.Equal(10, graph.Count);
        }
    }
}