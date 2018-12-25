using System;
using System.Collections.Generic;
using System.IO;

namespace Assembly.Tests
{
    public class TestFileReader : IFileReader
    {
        public IEnumerable<string> fileLines = new List<string>()
        {
            ">read_0",
            "CAGCAGCAGCGGTTATACGGATAGAATAAGCGTTTAACATAATAAAAAGGCCTAAAAAGTATGTAGATGGAAAAAAACAA",
            ">read_1",
            "TGTAGCTCAATGGTAGAGCAAGTCACTGTTAATGACCTGATAGTGGTTCGATTCCACTCAATAACGATAAAAATATATAT",
            ">read_2",
            "AAGTCACTGTTAATGACCTGATAGTGGTTCGATTCCACTCAATAACGATAAAAATATATATAATGAGTTTGATGTTAGCT      ",
            ">read_3",
            "CTAGATCTATATAATTTATCTAGAAATCAAAGTATCCAGAAACATTTTTTAAGTGTTTCTTAATTAAAGATACATGACGC",
            ">read_4",
            "TTAAACAAAGTTTAAAAAAGTTATTAAATAATCTTAATGATCTTTTCATCTACCCTAAAAATATTGAATCCTTTTAAAAT",
            ">read_5",
            "ATAATATATTTGCTCATCCTAGATTTAAAGAATTTAGTAAAACTATTACTTTAGGTTTAAATTCAAATAATAATTTAAAT",
            ">broken_read",
            "    ",
            ">read_6",
            "GTAAAAAAAGGTCAAATTCCTTTTCACCGAAAACTGAAG",
            "GGTTTTGACTTAAAAAGCTTAGCTTAAGTCAAATAAAGTAA",
            ">read_7",
            "AAAGTAACAATCTCTAAGCTAATAAATAGTGATGAGTAAATCTTAACGGATCTTAAAATAAAAGTTTTATTGAATAGAAA",
            ">read_8",
            "AAAAAATAATATTTTCTGGAGAAAGGCATCAAAAATTTAACGGATCTTAAACTTAATACTTAAACTTTGGCAGTATTTTA",
            ">read_9",
            "AAAATTCTCTTTTACTCAATGGTTAGTAGGATTTACTGATGGTGATGGTTGTTTTAGTATTTCAAAACAAAAAATAAAAA",
            ">broken_read",
            "    "
        };

        public IEnumerable<string> ReadLines(string filePath)
        {
            foreach (var line in fileLines)
            {
                yield return line;
            }
        }
    }
}