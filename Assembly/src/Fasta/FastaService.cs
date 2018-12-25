using System;
using System.Collections.Generic;
using System.IO;

namespace Assembly.Fasta
{
    public class FastaService
    {
        private readonly List<FastaSequence> _fastaSequences = new List<FastaSequence>();
        private readonly IFileReader _fileReader;

        public FastaService(IFileReader fileReader)
        {
            _fileReader = fileReader;
        }

        public List<FastaSequence> ParseFastaFile(string filePath)
        {
            var header = string.Empty;
            var sequence = string.Empty;

            foreach (var line in _fileReader.ReadLines(filePath))
            {
                if (line.StartsWith('>'))
                {
                    appendSequenceIfNotEmpty(header, ref sequence);

                    header = line.Substring(1).Trim();
                }
                else
                {
                    sequence += line.Trim();
                }
            }

            appendSequenceIfNotEmpty(header, ref sequence);

            if (_fastaSequences.Count == 0)
            {
                throw new ArgumentException("No fasta sequences in file.");
            }

            return _fastaSequences;
        }

        private void appendSequenceIfNotEmpty(string header, ref string sequence)
        {
            if (!string.IsNullOrEmpty(header) && !string.IsNullOrEmpty(sequence))
            {
                _fastaSequences.Add(new FastaSequence(header, sequence));
                sequence = string.Empty;
            }
        }
    }
}