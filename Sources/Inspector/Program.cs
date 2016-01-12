using Inspector.Components;
using System.Linq;
using Inspector.Reports;

namespace Inspector
{
    class Program
    {
        static void Main(string[] args)
        {
            Solution solution = new Solution(args.First());

            var ifsqAnalyzer = new IfSQLevel2Analyzer();

            ifsqAnalyzer.Analyze( solution.SourceFiles );

            var report = new IfSQLevel2Report();
            report.PrintMetrics( ifsqAnalyzer.Scores );
        }
    }
}
