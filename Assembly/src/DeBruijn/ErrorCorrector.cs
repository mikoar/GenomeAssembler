using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assembly.DeBruijn.Exceptions;

namespace Assembly.DeBruijn
{
    public class ErrorCorrector : IErrorCorrector
    {

        public int KmerLength { get; private set; }
        public char[] Alphabet { get; private set; }
        public int CorrectedKmersCount { get; private set; }
        public int FailedToCorrectKmersCount { get; private set; }
        public Dictionary<string, int> Histogram { get; private set; }

        public ErrorCorrector(int kmerLength = 19)
        {
            KmerLength = kmerLength;
            Alphabet = new char[]
            {
                'A',
                'T',
                'G',
                'C'
            };
        }

        public ErrorCorrector(char[] alphabet, int kmerLength = 19)
        {
            KmerLength = kmerLength;
            Alphabet = alphabet;
        }

        public IEnumerable<string> GenerateNeighbors(string kmer)
        {
            foreach (var neighbor in Distance1Neighbors(kmer))
            {
                yield return neighbor;
            }
            foreach (var neighbor in Distance2Neighbors(kmer))
            {
                yield return neighbor;
            }
        }

        private IEnumerable<string> Distance1Neighbors(string kmer)
        {
            char oldChar;
            char[] newKmer = kmer.ToCharArray();

            for (int i = kmer.Length - 1; i >= 0; i--)
            {
                oldChar = kmer[i];
                foreach (var newChar in Alphabet.Where(c => c != oldChar))
                {
                    newKmer[i] = newChar;
                    yield return new string(newKmer);
                }

                newKmer[i] = oldChar;
            }
        }

        private IEnumerable<string> Distance2Neighbors(string kmer)
        {
            char oldChar1, oldChar2;
            char[] newKmer = kmer.ToCharArray();

            for (int j = kmer.Length - 1; j >= 0; j--)
            {
                oldChar1 = kmer[j];
                foreach (var newChar1 in Alphabet.Where(c => c != oldChar1))
                {
                    newKmer[j] = newChar1;

                    for (int i = kmer.Length - 1; i >= 0; i--)
                    {
                        if (i == j)
                        {
                            continue;
                        }

                        oldChar2 = kmer[i];
                        foreach (var newChar2 in Alphabet.Where(c => c != oldChar2))
                        {
                            newKmer[i] = newChar2;
                            yield return new string(newKmer);
                        }

                        newKmer[i] = oldChar2;
                    }
                }

                newKmer[j] = oldChar1;
            }
        }

        public Dictionary<string, int> BuildHistogram(IEnumerable<string> reads)
        {
            Histogram = new Dictionary<string, int>();
            string kmer;

            Console.WriteLine($"Building histogram of { KmerLength }-mers...");

            foreach (var read in reads)
            {
                for (int i = 0; i <= read.Length - KmerLength; i++)
                {
                    kmer = read.Substring(i, KmerLength);

                    if (!Histogram.TryAdd(kmer, 1))
                    {
                        Histogram[kmer] += 1;
                    }
                }
            }

            Console.WriteLine($"Generated histogram with { Histogram.Count } distinct { KmerLength }-mers.");

            return Histogram;
        }

        public IEnumerable<KMer> CorrectReadsAndSplitToKmers(IEnumerable<string> reads, int threshold = 1)
        {
            if (Histogram == null)
            {
                throw new GraphException("Histogram not built");
            }

            string kmer;
            var correctedRead = new StringBuilder();
            int frequency;
            bool newKmerFound;
            CorrectedKmersCount = 0;
            FailedToCorrectKmersCount = 0;

            foreach (var read in reads)
            {
                correctedRead.Clear().Append(read);

                for (int i = 0; i <= correctedRead.Length - KmerLength; i++)
                {
                    kmer = correctedRead.ToString(i, KmerLength);

                    if (!Histogram.TryGetValue(kmer, out frequency) || frequency <= threshold)
                    {
                        newKmerFound = false;

                        foreach (var newKmer in GenerateNeighbors(kmer))
                        {
                            if (Histogram.TryGetValue(newKmer, out frequency) &&
                                frequency > threshold)
                            {
                                newKmerFound = true;
                                CorrectedKmersCount += 1;
                                correctedRead.Remove(i, KmerLength).Insert(i, newKmer);
                                yield return new KMer(newKmer);
                                break;
                            }
                        }

                        if (!newKmerFound)
                        {
                            FailedToCorrectKmersCount += 1;
                        }
                    }
                    else
                    {
                        yield return new KMer(kmer);
                    }
                }
            }
        }

        public void PrintResult()
        {
            Console.WriteLine($"{KmerLength}-mers corrected: {CorrectedKmersCount}, failed to correct: {FailedToCorrectKmersCount}.");
        }
    }
}