using System.Collections.Generic;

namespace Assembly.Services
{
    public interface IFileService
    {
        IEnumerable<string> ReadLines(string filePath);
        void WriteAllText(string filePath, string contents);
    }
}