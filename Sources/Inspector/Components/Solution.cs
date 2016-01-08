using System.Collections.Generic;
using System.Linq;
using Inspector.Infrastructure;

namespace Inspector.Components
{
    public class Solution
    {
        private readonly ISolutionParser solutionParser;
        private readonly IProjectParser projectParser;

        public string FileName { get; private set; }

        public Solution(string solutionFile) : this(solutionFile, new SolutionParser(), new ProjectParser())
        {

        }

        public Solution(string solutionFile, ISolutionParser solutionParser, IProjectParser projectParser)
        {
            FileName = solutionFile;
            this.solutionParser = solutionParser;
            this.projectParser = projectParser;
        }

        public IEnumerable<Project> Projects
        {
            get
            {
                return solutionParser.GetProjects(this);
            }
        }

        public IEnumerable<SourceFile> SourceFiles
        {
            get
            {
                return Projects.SelectMany(project => projectParser.GetSourceFiles(project));
            }
        }


    }
}
