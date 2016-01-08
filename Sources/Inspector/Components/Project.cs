using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Inspector.Components
{
    public class Project
    {
        private readonly string absolutePath;
        private readonly string projectGuid;
        private readonly string projectName;
        private readonly IList<SourceFile> sources= new List<SourceFile>();

        public Project(string projectGuid, string projectName, string absolutePath)
        {
            this.projectGuid = projectGuid;
            this.projectName = projectName;
            this.absolutePath = absolutePath;
        }

        public string Path
        {
            get { return absolutePath; }
        }

        public string Id
        {
            get { return projectGuid;  }
        }

        internal void AddSourceFile(SourceFile sourceFile)
        {
            sources.Add(sourceFile);
        }

        public string Name
        {
            get { return projectName;  }
        }

        public override string ToString()
        {
            return $"{Name} ({Path})";
        }

        public ICollection<SourceFile> SourceFiles
        {
            get
            {
                return new ReadOnlyCollection<SourceFile>(sources);
            }
        }
    }

    public class WebProject : Project
    {
        public WebProject(string projectGuid, string projectName, string absolutePath) : base(projectGuid, projectName, absolutePath)
        {

        }
    }

    public class UnsupportedProject : Project
    {
        public UnsupportedProject(string projectGuid, string projectName, string absolutePath) : base(projectGuid, projectName, absolutePath)
        {

        }
    }
}