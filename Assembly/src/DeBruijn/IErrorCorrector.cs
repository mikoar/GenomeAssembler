using System.Collections.Generic;


namespace Assembly.DeBruijn
{
    public interface IErrorCorrector
    {
        IEnumerable<KMer> CorrectReadsAndSplitToKmers(IEnumerable<string> reads, int threshold = 1);
    }
}