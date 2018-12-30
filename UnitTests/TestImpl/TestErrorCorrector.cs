using System;
using System.Collections.Generic;
using System.IO;
using Assembly.Models;
using Assembly.Services;

namespace Assembly.UnitTests
{
    public class TestErrorCorrector : IErrorCorrector
    {
        private int _kmerLength;

        public TestErrorCorrector(int kmerLength)
        {
            _kmerLength = kmerLength;
        }

        public IEnumerable<KMer> CorrectReadsAndSplitToKmers(IEnumerable<string> reads, Dictionary<string, int> histogram, int threshold = 1)
        {
            foreach (var read in reads)
            {
                for (int i = 0; i <= read.Length - _kmerLength; i++)
                {
                    yield return new KMer(read.Substring(i, _kmerLength));
                }
            }
        }
    }
}