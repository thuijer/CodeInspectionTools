namespace Inspector.CustomAnalyzers
{
    internal class ClassScore
    {
        private readonly string classname;
        private readonly int memberCount;
        private readonly int totalLineCount;

        public ClassScore(string classname, int memberCount, int totalLineCount)
        {
            this.memberCount = memberCount;
            this.totalLineCount = totalLineCount;
            this.classname = classname;
        }

        public string Classname
        {
            get { return classname; }
        }

        public int MemberCount
        {
            get { return memberCount; }
        }

        public int TotalLineCount
        {
            get { return totalLineCount; }
        }
    }
}