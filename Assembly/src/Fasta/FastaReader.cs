using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Assembly.Fasta

{
    public class FastaReader
    {
        public int Count { get; private set; }
        private readonly IFileService _fileService;

        public FastaReader(IFileService fileService)
        {
            _fileService = fileService;
        }

        public IEnumerable<string> ParseFastaFile(string filePath)
        {
            var sequence = string.Empty;
            var headerRead = false;

            foreach (var line in _fileService.ReadLines(filePath))
            {
                if (line.StartsWith('>'))
                {
                    headerRead = true;
                    if (!string.IsNullOrEmpty(sequence))
                    {
                        Count += 1;

                        yield return sequence;
                        sequence = string.Empty;
                    }
                }
                else
                {
                    sequence += line.Trim();
                }
            }

            if (headerRead && !string.IsNullOrEmpty(sequence))
            {
                Count += 1;
                yield return sequence;
            }

            if (Count == 0)
            {
                throw new ArgumentException("No fasta sequences in file.");
            }
        }

        public void WriteFastaFile(string filePath, IEnumerable<string> sequences)
        {
            string header = "contig";
            int i = 0;
            var sequencesWithHeaders = sequences.Select(s => $"{header}{i++}\n{s}");
            _fileService.WriteAllLines(filePath, sequencesWithHeaders);
            Console.WriteLine($"Wrote fasta file containing { i } sequences to \"{ filePath }\" ");
        }
    }
}