using Inspector.Components;
using Inspector.IfSQ;
using System;
using System.Linq;
using Inspector.CodeMetrics.Scores;
using System.Collections.Generic;
using Inspector.Reports;

namespace Inspector
{
    class Program
    {
        static void Main(string[] args)
        {
            Solution solution = new Solution(args.First());

            var sourceAnalyzer = new IfSQLevel2Analyzer();

            Dictionary<SourceFile, IEnumerable<CodeMetricScore>> codeMetrics = sourceAnalyzer.CalculateCodeMetrics(solution.SourceFiles);
            IEnumerable<Level2Score> ifsqScores = codeMetrics.Select(kvp => new Level2Score(kvp.Key, kvp.Value));

            var report = new IfSQLevel2Report();
            report.PrintMetrics( ifsqScores );
        }
    }
}
