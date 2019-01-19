using System;
using System.Collections.Generic;
using System.IO;

namespace Assembly.Fasta
{
    public class FileService : IFileService
    {
        public IEnumerable<string> ReadLines(string filePath)
        {
            return File.ReadLines(filePath);
        }

        public void WriteAllLines(string filePath, IEnumerable<string> contents)
        {
            File.WriteAllLines(filePath, contents);
        }

        public void WriteAllText(string filePath, string contents)
        {
            File.WriteAllText(filePath, contents);
        }
    }
}