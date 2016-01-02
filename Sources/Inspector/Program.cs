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
            Console.WriteLine("--- Reading solutionfile contents ---");
            var info = new Solution(args.First());

            var sourceAnalyzer = new SourceFileAnalyzer();
            sourceAnalyzer.AddAnalyzer(new CodeMetrics.VisualBasic.ControlFlowComplexity());
            sourceAnalyzer.AddAnalyzer(new CodeMetrics.VisualBasic.MethodLength());
            sourceAnalyzer.AddAnalyzer(new CodeMetrics.VisualBasic.NestingLevel());
            sourceAnalyzer.AddAnalyzer(new CodeMetrics.CSharp.MethodLength());
            sourceAnalyzer.AddAnalyzer(new CodeMetrics.CSharp.ControlFlowComplexity());
            sourceAnalyzer.AddAnalyzer(new CodeMetrics.CSharp.NestingLevel());

            Console.WriteLine("--- Analyzing sourcefiles ---");
            var sourceFilesWithCodeMetrics = sourceAnalyzer.AppendCodeMetrics(info.SourceFiles);

            Console.WriteLine($"Solution based linecount: {sourceFilesWithCodeMetrics.Sum(sf => sf.LinesOfCode)}");

            sourceFilesWithCodeMetrics.Select(sf => new Level2Score(sf)).OrderByDescending(s=>s.Rating).ToList().ForEach(s => Console.WriteLine(s));

            Console.WriteLine("--- Analyzing done ---");
            Console.ReadLine();
        }


    }
}
