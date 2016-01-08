using System.Collections.Generic;
using Inspector.Components;

namespace Inspector.Infrastructure
{
    public interface IProjectParser
    {
        IEnumerable<SourceFile> GetSourceFiles(Project project);
    }
}