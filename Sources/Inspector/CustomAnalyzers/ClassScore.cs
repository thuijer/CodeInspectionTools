namespace Inspector.CustomAnalyzers
{
    internal class ClassScore
    {
        private readonly string classname;
        private readonly int memberCount;

        public ClassScore(string classname, int memberCount)
        {
            this.memberCount = memberCount;
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
    }
}