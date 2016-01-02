namespace Inspector.Infrastructure
{
    public interface IFileSystemAdapter
    {
        string ReadAllTextFromFile(string absPath);
    }
}