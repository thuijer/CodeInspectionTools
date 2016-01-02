using System.Collections.Generic;

namespace Inspector.Infrastructure
{
    public interface ISolutionParser
    {
        IEnumerable<Project> GetProjects(Solution solution);
    }
}