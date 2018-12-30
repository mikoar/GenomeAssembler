using System;
using System.Collections.Generic;
using Assembly.Models;
using Assembly.Services;
using Xunit;

namespace Assembly.UnitTests
{
    public class DeBruijnGraphServiceTests
    {
        [Fact]
        public void Build_SimpleString()
        {
            var sequences = new List<string>()
            {
                "a_long_long_long_time",
            };

            var graph = new DeBruijnGraphService(4, new TestErrorCorrector(4), new Dictionary<string, int>())
                .Build(sequences);

            Assert.Equal(10, graph.Count);
        }
    }
}