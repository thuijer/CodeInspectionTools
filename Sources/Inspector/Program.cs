using Inspector.Components;
using Inspector.IfSQ;
using System;
using System.Linq;

namespace Inspector
{
    class Program
    {
        static void Main(string[] args)
        {
            var solution = new Solution(args.First());

            var sourceAnalyzer = new SourceFileAnalyzer();
            AddCodeAnalyzers(sourceAnalyzer);

            var sourceFilesWithCodeMetrics = sourceAnalyzer.AppendCodeMetrics(solution.SourceFiles);
            var ifsqScores = sourceFilesWithCodeMetrics.Select(sf => new Level2Score(sf));

            PrintDetails(ifsqScores);
            PrintTotal(sourceFilesWithCodeMetrics, ifsqScores);
        }

        private static void PrintDetails(System.Collections.Generic.IEnumerable<Level2Score> ifsqScores)
        {
            Console.WriteLine(Level2Score.HeaderString());
            ifsqScores.OrderByDescending(s => s.Rating).ToList().ForEach(s => Console.WriteLine(s));
        }

        private static void PrintTotal(System.Collections.Generic.IEnumerable<SourceFile> sourceFilesWithCodeMetrics, System.Collections.Generic.IEnumerable<Level2Score> ifsqScores)
        {
            int totalLines = sourceFilesWithCodeMetrics.Sum(sf => sf.LinesOfCode);
            var total = new Level2Score(totalLines, ifsqScores);
            Console.WriteLine(total);
        }

        private static void AddCodeAnalyzers(SourceFileAnalyzer sourceAnalyzer)
        {
            //SP-1
            sourceAnalyzer.AddAnalyzer(new CodeMetrics.CSharp.MethodLength());
            sourceAnalyzer.AddAnalyzer(new CodeMetrics.VisualBasic.MethodLength());

            //SP-2
            sourceAnalyzer.AddAnalyzer(new CodeMetrics.CSharp.NestingLevel());
            sourceAnalyzer.AddAnalyzer(new CodeMetrics.VisualBasic.NestingLevel());

            //SP-3
            sourceAnalyzer.AddAnalyzer(new CodeMetrics.CSharp.ControlFlowComplexity());
            sourceAnalyzer.AddAnalyzer(new CodeMetrics.VisualBasic.ControlFlowComplexity());

            //WIP-1
            sourceAnalyzer.AddAnalyzer(new CodeMetrics.CSharp.VagueToDo());
            sourceAnalyzer.AddAnalyzer(new CodeMetrics.VisualBasic.VagueToDo());

            //SPM-1
            sourceAnalyzer.AddAnalyzer(new CodeMetrics.CSharp.MagicNumber());
            sourceAnalyzer.AddAnalyzer(new CodeMetrics.VisualBasic.MagicNumber());
        }
    }
}
