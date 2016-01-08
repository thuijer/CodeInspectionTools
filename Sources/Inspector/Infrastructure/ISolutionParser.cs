using System.Collections.Generic;
using Inspector.Components;

namespace Inspector.Infrastructure
{
    public interface ISolutionParser
    {
        IEnumerable<Project> GetProjects(Solution solution);
    }
}