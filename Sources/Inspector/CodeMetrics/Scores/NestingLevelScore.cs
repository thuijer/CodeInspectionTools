using System.Collections.Generic;

namespace Inspector.CodeMetrics.Scores
{
    public class NestingLevelScore : MethodScore
    {
        public Dictionary<int, int> LineCountPerLevel { get; internal set; }
    }
}
