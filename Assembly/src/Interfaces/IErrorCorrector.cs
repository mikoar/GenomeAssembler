using System.Collections.Generic;
using Assembly.Models;

namespace Assembly.Services
{
    public interface IErrorCorrector
    {
        IEnumerable<KMer> CorrectReadsAndSplitToKmers(IEnumerable<string> reads, Dictionary<string, int> histogram, int threshold = 1);
    }
}