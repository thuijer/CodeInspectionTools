using Microsoft.Build.Construction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inspector.Infrastructure
{
    public class SolutionParser : ISolutionParser
    {
        public IEnumerable<Project> GetProjects(Solution solution)
        {
            var x = SolutionFile.Parse(solution.FileName);
            return x.ProjectsInOrder.Select(p => new Project(p.ProjectGuid, p.ProjectName, System.IO.Path.GetFullPath(p.AbsolutePath)));
        }
    }
}
