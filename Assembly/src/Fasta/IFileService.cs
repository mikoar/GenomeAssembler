using System.Collections.Generic;

namespace Assembly.Fasta
{
    public interface IFileService
    {
        IEnumerable<string> ReadLines(string filePath);
        void WriteAllText(string filePath, string contents);
    }
}