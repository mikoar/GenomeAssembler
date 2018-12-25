using System.Collections.Generic;

public interface IFileReader
{
    IEnumerable<string> ReadLines(string filePath);
}