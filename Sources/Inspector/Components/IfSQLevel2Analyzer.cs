using System.Collections.Generic;
using System.Linq;
using Inspector.CodeMetrics.Scores;
using Inspector.IfSQ;

namespace Inspector.Components
{
    class IfSQLevel2Analyzer : SourceFileAnalyzer
    {
        IEnumerable<Level2Score> scores;

        public IfSQLevel2Analyzer()
        {
            AddIfSQLevel2Analyzers();
        }

        private void AddIfSQLevel2Analyzers()
        {
            //SP-1
            AddAnalyzer(new CodeMetrics.CSharp.MethodLength());
            AddAnalyzer(new CodeMetrics.VisualBasic.MethodLength());

            //SP-2
            AddAnalyzer(new CodeMetrics.CSharp.NestingLevel());
            AddAnalyzer(new CodeMetrics.VisualBasic.NestingLevel());

            //SP-3
            AddAnalyzer(new CodeMetrics.CSharp.ControlFlowComplexity());
            AddAnalyzer(new CodeMetrics.VisualBasic.ControlFlowComplexity());

            //WIP-1
            AddAnalyzer(new CodeMetrics.CSharp.VagueToDo());
            AddAnalyzer(new CodeMetrics.VisualBasic.VagueToDo());

            //WIP-2
            AddAnalyzer(new CodeMetrics.CSharp.DisabledCode());
            AddAnalyzer(new CodeMetrics.VisualBasic.DisabledCode());

            //WIP-3
            AddAnalyzer(new CodeMetrics.CSharp.EmptyStatementBlock());
            AddAnalyzer(new CodeMetrics.VisualBasic.EmptyStatementBlock());

            //SPM-1
            AddAnalyzer(new CodeMetrics.CSharp.MagicNumber());
            AddAnalyzer(new CodeMetrics.VisualBasic.MagicNumber());

            //SPM-2
            AddAnalyzer(new CodeMetrics.CSharp.MagicString());
            AddAnalyzer(new CodeMetrics.VisualBasic.MagicString());
        }

        public void Analyze(IEnumerable<SourceFile> sourceFiles)
        {
            Dictionary<SourceFile, IEnumerable<CodeMetricScore>> codeMetrics = CalculateCodeMetrics(sourceFiles);
            scores = codeMetrics.Select(kvp => new Level2Score(kvp.Key, kvp.Value));
        }

        public IEnumerable<Level2Score> Scores
        {
            get { return scores; }
        }
    }
}
