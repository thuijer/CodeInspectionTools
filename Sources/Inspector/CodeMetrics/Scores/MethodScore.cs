namespace Inspector.CodeMetrics.Scores
{
    public class MethodScore: CodeScore
    {
        public string Method { get; internal set; }

        public override string ToString()
        {
            return $"\t{ClassName}-{Method}: {Score}";
        }
    }
}