using System.IO;

namespace Inspector.Infrastructure
{
    public class FileSystemAdapter : IFileSystemAdapter
    {
        public string ReadAllTextFromFile(string absPath)
        {
            return File.ReadAllText(absPath);
        }
    }
}
