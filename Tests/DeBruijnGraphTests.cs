using System;
using System.Collections.Generic;
using Assembly.Models;
using Assembly.Services;
using Xunit;

namespace Assembly.Tests
{
    public class DeBruijnGraphTests
    {
        [Fact]
        public void Build_SimpleString()
        {
            var sequences = new List<string>()
            {
                "a_long_long_long_time",
            };

            var nodes = new DeBruijnGraph(sequences, 4).Build();

            Assert.Equal(10, nodes.Count);
        }

        [Fact]
        public void Build()
        {
            var sequences = new List<string>()
            {
                "CAGCAGCAGCGGTTATACGGATAGAATAAGCGTTTAACATAATAAAAAGGCCTAAAAAGTATGTAGATGGAAAAAAACAA",
                "TGTAGCTCAATGGTAGAGCAAGTCACTGTTAATGACCTGATAGTGGTTCGATTCCACTCAATAACGATAAAAATATATAT",
                "AAGTCACTGTTAATGACCTGATAGTGGTTCGATTCCACTCAATAACGATAAAAATATATATAATGAGTTTGATGTTAGCT",
                "CTAGATCTATATAATTTATCTAGAAATCAAAGTATCCAGAAACATTTTTTAAGTGTTTCTTAATTAAAGATACATGACGC",
                "TTAAACAAAGTTTAAAAAAGTTATTAAATAATCTTAATGATCTTTTCATCTACCCTAAAAATATTGAATCCTTTTAAAAT",
                "ATAATATATTTGCTCATCCTAGATTTAAAGAATTTAGTAAAACTATTACTTTAGGTTTAAATTCAAATAATAATTTAAAT",
                "GTAAAAAAAGGTCAAATTCCTTTTCACCGAAAACTGAAGGGTTTTGACTTAAAAAGCTTAGCTTAAGTCAAATAAAGTAA",
                "AAAGTAACAATCTCTAAGCTAATAAATAGTGATGAGTAAATCTTAACGGATCTTAAAATAAAAGTTTTATTGAATAGAAA",
                "AAAAAATAATATTTTCTGGAGAAAGGCATCAAAAATTTAACGGATCTTAAACTTAATACTTAAACTTTGGCAGTATTTTA",
                "AAAATTCTCTTTTACTCAATGGTTAGTAGGATTTACTGATGGTGATGGTTGTTTTAGTATTTCAAAACAAAAAATAAAAA",
            };

            var nodes = new DeBruijnGraph(sequences, 4).Build();
        }

    }
}