namespace Inspector.CodeMetrics
{
    public class MethodScore
    {
        public string ClassName { get; internal set; }
        public string Method { get; internal set; }
        public int Score { get; internal set; }

        public override string ToString()
        {
            return $"\t{ClassName}-{Method}: {Score}";
        }
    }
}