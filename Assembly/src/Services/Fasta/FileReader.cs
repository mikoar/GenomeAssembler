using System;
using System.Collections.Generic;
using System.IO;

namespace Assembly.Services
{
    public class FileReader : IFileReader
    {
        public IEnumerable<string> ReadLines(string filePath)
        {
            return File.ReadLines(filePath);
        }
    }
}