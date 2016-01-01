
using Inspector.Analyzers;
using Microsoft.CodeAnalysis;
using System;
using System.Linq;

namespace Inspector
{
    class Program
    {

        static void Main(string[] args)
        {
            var info = new SolutionInfo();
            info.AddCodeAnalyzer(new Analyzers.VisualBasic.ControlFlowComplexity());
            info.AddCodeAnalyzer(new Analyzers.VisualBasic.MethodLength());
            info.AddCodeAnalyzer(new Analyzers.CSharp.MethodLength());
            info.AddCodeAnalyzer(new Analyzers.CSharp.ControlFlowComplexity());

            var sourceFiles = info.GetSourceFiles(args.First());

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
