namespace Inspector
{
    public class Project
    {
        private readonly string absolutePath;
        private readonly string projectGuid;
        private readonly string projectName;

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

        public string Name
        {
            get { return projectName;  }
        }
    }
}