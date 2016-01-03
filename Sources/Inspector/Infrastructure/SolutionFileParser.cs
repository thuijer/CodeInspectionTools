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
            return x.ProjectsInOrder
                .Where(p => p.ProjectType == SolutionProjectType.KnownToBeMSBuildFormat
                            || p.ProjectType == SolutionProjectType.WebProject)
                .Select(p =>
                {
                    switch (p.ProjectType)
                    {
                        case SolutionProjectType.WebProject:
                            return new WebProject(p.ProjectGuid, p.ProjectName, System.IO.Path.GetFullPath(p.AbsolutePath));
                        case SolutionProjectType.KnownToBeMSBuildFormat:
                            return new Project(p.ProjectGuid, p.ProjectName, System.IO.Path.GetFullPath(p.AbsolutePath));
                        default:
                            return new UnsupportedProject(p.ProjectGuid, p.ProjectName, System.IO.Path.GetFullPath(p.AbsolutePath));
                    }
                });
        }
    }
}
