using Inspector.CodeMetrics;
using Inspector.Components;
using Inspector.IfSQ;
using Microsoft.CodeAnalysis;
using System;
using System.IO;
using System.Linq;

namespace Inspector
{
    class Program
    {
        static void Main(string[] args)
        {
            var info = new Solution(args.First());

            var sourceAnalyzer = new SourceFileAnalyzer();
            AddVisualBasicAnalyzers(sourceAnalyzer);
            AddCSharpAnalyzers(sourceAnalyzer);

            var sourceFilesWithCodeMetrics = sourceAnalyzer.AppendCodeMetrics(info.SourceFiles);
            Console.WriteLine(Level2Score.HeaderString());
            var ifsqScores = sourceFilesWithCodeMetrics.Select(sf => new Level2Score(sf));
            ifsqScores.OrderByDescending(s => s.Rating).ToList().ForEach(s => Console.WriteLine(s));

            int totalLines = sourceFilesWithCodeMetrics.Sum(sf => sf.LinesOfCode);
            var total = new Level2Score(totalLines, ifsqScores);
            Console.WriteLine(total);
        }

        private static void AddCSharpAnalyzers(SourceFileAnalyzer sourceAnalyzer)
        {
            sourceAnalyzer.AddAnalyzer(new CodeMetrics.CSharp.MethodLength());
            sourceAnalyzer.AddAnalyzer(new CodeMetrics.CSharp.ControlFlowComplexity());
            sourceAnalyzer.AddAnalyzer(new CodeMetrics.CSharp.NestingLevel());
            sourceAnalyzer.AddAnalyzer(new CodeMetrics.CSharp.VagueToDo());
        }

        private static void AddVisualBasicAnalyzers(SourceFileAnalyzer sourceAnalyzer)
        {
            sourceAnalyzer.AddAnalyzer(new CodeMetrics.VisualBasic.ControlFlowComplexity());
            sourceAnalyzer.AddAnalyzer(new CodeMetrics.VisualBasic.MethodLength());
            sourceAnalyzer.AddAnalyzer(new CodeMetrics.VisualBasic.NestingLevel());
            sourceAnalyzer.AddAnalyzer(new CodeMetrics.VisualBasic.VagueToDo());
        }
    }
}
