using System;
using System.Collections.Generic;
using System.IO;

namespace Assembly.Services
{
    public class FileService : IFileService
    {
        public IEnumerable<string> ReadLines(string filePath)
        {
            return File.ReadLines(filePath);
        }

        public void WriteAllText(string filePath, string contents)
        {
            File.WriteAllText(filePath, contents);
        }
    }
}