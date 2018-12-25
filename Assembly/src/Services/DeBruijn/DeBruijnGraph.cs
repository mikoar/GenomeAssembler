using System.Collections.Generic;
using System.Linq;
using Assembly.Models;

namespace Assembly.Services
{
    public class DeBruijnGraph
    {
        private IEnumerable<KMer> SplitToKmers(string s, int kmerLength)
        {
            for (int i = 0; i < s.Length - kmerLength - 1; i++)
            {
                yield return new KMer(s.Substring(i, kmerLength));
            }
        }
    }
}