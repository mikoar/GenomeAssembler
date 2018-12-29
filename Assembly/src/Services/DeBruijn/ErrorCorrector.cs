using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assembly.Services
{
    public class ErrorCorrector
    {

        public int KmerLength { get; private set; }
        public char[] Alphabet { get; private set; }
        public int CorrectedKmersCount { get; private set; }
        public int FailedToCorrectKmersCount { get; private set; }

        public ErrorCorrector(int kmerLength = 6)
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

        public ErrorCorrector(char[] alphabet, int kmerLength = 6)
        {
            KmerLength = kmerLength;
            Alphabet = alphabet;
        }

        public IEnumerable<string> GenerateNeighbors(string kmer)
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

        public Dictionary<string, int> BuildHistogram(IEnumerable<string> reads)
        {
            var histogram = new Dictionary<string, int>();
            string kmer;

            Console.WriteLine($"Building histogram of { KmerLength }-mers...");

            foreach (var read in reads)
            {
                for (int i = 0; i <= read.Length - KmerLength; i++)
                {
                    kmer = read.Substring(i, KmerLength);

                    if (!histogram.TryAdd(kmer, 1))
                    {
                        histogram[kmer] += 1;
                    }
                }
            }

            Console.WriteLine($"Generated histogram with { histogram.Count } distinct { KmerLength }-mers.");

            return histogram;
        }

        public IEnumerable<string> Correct(IEnumerable<string> reads, Dictionary<string, int> histogram, int threshold = 4)
        {
            string kmer;
            string correctedRead;
            int frequency;
            var sb = new StringBuilder();
            bool newKmerFound;
            CorrectedKmersCount = 0;

            foreach (var read in reads)
            {
                correctedRead = read;

                for (int i = 0; i <= correctedRead.Length - KmerLength; i++)
                {
                    kmer = correctedRead.Substring(i, KmerLength);

                    if (!histogram.TryGetValue(kmer, out frequency) || frequency <= threshold)
                    {
                        newKmerFound = false;

                        foreach (var newKmer in GenerateNeighbors(kmer))
                        {
                            if (histogram.TryGetValue(newKmer, out frequency) &&
                                frequency > threshold)
                            {
                                correctedRead = sb.Append(correctedRead.Substring(0, i)).Append(newKmer).Append(correctedRead.Substring(i + KmerLength)).ToString();
                                sb.Clear();
                                newKmerFound = true;
                                break;
                            }
                        }

                        IncrementCounter(newKmerFound);
                    }
                }

                yield return correctedRead;
            }
        }

        public void PrintResult()
        {
            Console.WriteLine($"{KmerLength}-mers corrected: {CorrectedKmersCount}, failed to correct: {FailedToCorrectKmersCount}.");
        }

        private void IncrementCounter(bool newKmerFound)
        {
            if (newKmerFound)
            {
                CorrectedKmersCount += 1;
            }
            else
            {
                FailedToCorrectKmersCount += 1;
            }
        }
    }
}