using System;
using System.Collections.Generic;

namespace Inspector
{
    public class Project
    {
        private readonly string absolutePath;
        private readonly string projectGuid;
        private readonly string projectName;
        private readonly ICollection<SourceFile> sources= new List<SourceFile>();

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

        public void AddSourceFile(SourceFile sourceFile)
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
    }
}