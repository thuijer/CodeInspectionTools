using System.Collections.Generic;
using System.Linq;
using Inspector.CodeMetrics.Scores;
using Inspector.Components;
using Inspector.IfSQ;

namespace Inspector.CustomAnalyzers
{
    class ClassStatisticsAnalyzer : SourceFileAnalyzer
    {
        public ClassStatisticsAnalyzer()
        {
            AddStatisticsAnalyzers();
        }

        private void AddStatisticsAnalyzers()
        {
            //SP-1
            AddAnalyzer(new CodeMetrics.CSharp.ClassMembers());
        }

        public void Analyze(IEnumerable<SourceFile> sourceFiles)
        {
            Dictionary<SourceFile, IEnumerable<CodeMetricScore>> codeMetrics = CalculateCodeMetrics(sourceFiles);
            Scores = codeMetrics.Values.SelectMany(metrics => metrics).Select( metric => new ClassScore( metric.ClassName, metric.Score) );
        }

        public IEnumerable<ClassScore> Scores { get; set; }
    }
}
