using System;
using System.Collections.Generic;
using System.IO;

namespace Assembly.Fasta
{
    public class FileReader : IFileReader
    {
        public IEnumerable<string> ReadLines(string filePath)
        {
            return File.ReadLines(filePath);
        }
    }
}