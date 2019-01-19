using System;
using System.Collections.Generic;
using System.IO;


namespace Assembly.Fasta

{
    public class FastaReader
    {
        public int Count { get; private set; }
        private readonly IFileService _fileReader;

        public FastaReader(IFileService fileReader)
        {
            _fileReader = fileReader;
        }

        public IEnumerable<string> ParseFastaFile(string filePath)
        {
            var sequence = string.Empty;
            var headerRead = false;

            foreach (var line in _fileReader.ReadLines(filePath))
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
    }
}