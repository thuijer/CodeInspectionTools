using Inspector.CodeMetrics;
using Inspector.Components;
using Microsoft.CodeAnalysis;
using System;
using System.Linq;

namespace Inspector
{
    class Program
    {
        static void Main(string[] args)
        {
            var info = new Solution(args.First());
            var analysis = new SourceFileAnalyzer();
            analysis.AddCodeMetric(new CodeMetrics.VisualBasic.ControlFlowComplexity());
            analysis.AddCodeMetric(new CodeMetrics.VisualBasic.MethodLength());
            analysis.AddCodeMetric(new CodeMetrics.CSharp.MethodLength());
            analysis.AddCodeMetric(new CodeMetrics.CSharp.ControlFlowComplexity());

            var sourceFiles = analysis.Analyze(info);

            Console.WriteLine($"Solution based linecount: {sourceFiles.Sum(sf => sf.LinesOfCode)}");

            sourceFiles.Where(sf=>sf.MethodScores.OfType<ControlFlowComplexityScore>().Any(s=>s.Score > 10)).ToList().ForEach(f =>
            {
                Console.WriteLine($"{f.Project.Name} {f.FileName} {f.LinesOfCode}");
                f.MethodScores.OfType<ControlFlowComplexityScore>().Where(ms=>ms.Score > 10).ToList().ForEach(ms => Console.WriteLine(ms));
            });

            Console.WriteLine("--- Analyzing done ---");
            Console.ReadLine();
        }

        
    }
}
