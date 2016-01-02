using System.Collections.Generic;

namespace Inspector.Infrastructure
{
    public interface IProjectParser
    {
        IEnumerable<SourceFile> GetSourceFiles(Project project);
    }
}