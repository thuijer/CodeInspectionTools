using System.IO;
using Inspector.Components;
using System.Linq;
using Inspector.CustomAnalyzers;
using Inspector.Reports;

namespace Inspector
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = Path.GetFullPath(args.First());
            Solution solution = new Solution(path);

            PrintIfSQLevel2Scores(solution);
            PrintClassMemberScores(solution);
        }

        private static void PrintClassMemberScores(Solution solution)
        {
            var classStatisticsAnalyzer = new ClassStatisticsAnalyzer();

            classStatisticsAnalyzer.Analyze(solution.SourceFiles);

            var report = new ClassMembersReport();
            report.PrintMetrics(classStatisticsAnalyzer.Scores);
        }

        private static void PrintIfSQLevel2Scores(Solution solution)
        {
            var ifsqAnalyzer = new IfSQLevel2CSharpAnalyzer();

            ifsqAnalyzer.Analyze(solution.SourceFiles);

            var report = new IfSQLevel2Report();
            report.PrintMetrics(ifsqAnalyzer.Scores);
        }
    }
}
