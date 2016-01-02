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
            Console.WriteLine("--- Reading solutionfile contents ---");
            var info = new Solution(args.First());
            
            var sourceAnalyzer = new SourceFileAnalyzer();
            sourceAnalyzer.AddAnalyzer(new CodeMetrics.VisualBasic.ControlFlowComplexity());
            sourceAnalyzer.AddAnalyzer(new CodeMetrics.VisualBasic.MethodLength());
            sourceAnalyzer.AddAnalyzer(new CodeMetrics.CSharp.MethodLength());
            sourceAnalyzer.AddAnalyzer(new CodeMetrics.CSharp.ControlFlowComplexity());

            Console.WriteLine("--- Analyzing sourcefiles ---");
            var sourceFilesWithCodeMetrics = sourceAnalyzer.AppendCodeMetrics(info.SourceFiles);

            Console.WriteLine($"Solution based linecount: {sourceFilesWithCodeMetrics.Sum(sf => sf.LinesOfCode)}");

            sourceFilesWithCodeMetrics.Where(sf=>sf.MethodScores.OfType<ControlFlowComplexityScore>().Any(s=>s.Score > 10)).ToList().ForEach(f =>
            {
                Console.WriteLine($"{f.Project.Name} {f.FileName} {f.LinesOfCode}");
                f.MethodScores.OfType<ControlFlowComplexityScore>().Where(ms=>ms.Score > 10).ToList().ForEach(ms => Console.WriteLine(ms));
            });

            Console.WriteLine("--- Analyzing done ---");
            Console.ReadLine();
        }

        
    }
}
