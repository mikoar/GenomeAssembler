using System.Collections.Generic;

public interface IFileService
{
    IEnumerable<string> ReadLines(string filePath);
    void WriteAllText(string filePath, string contents);
}