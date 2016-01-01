using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Inspector.Analyzers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.VisualBasic;

namespace Inspector
{
    public class SourceFile
    {
        private const string csharpSourceFileExtension = ".cs";
        private const string visualBasicSourceFileExtension = ".vb";

        private string absPath;

        public SourceFile(string absPath)
        {
            this.absPath = absPath;
        }

        public int LinesOfCode
        {
            get; internal set;
        }

        public Project Project { get; internal set; }
        public string FileName { get; internal set; }
        public string Language { get; private set; }

        public IEnumerable<MethodScore> MethodScores { get; private set; }

        private SyntaxNode GetSyntaxNodeFromFile(string absPath)
        {
            var source = File.OpenText(absPath).ReadToEnd();
            SyntaxNode sourceFileRootNode = null;
            if (absPath.EndsWith(csharpSourceFileExtension))
            {
                sourceFileRootNode = CSharpSyntaxTree.ParseText(source).GetRoot();
            }
            else if (absPath.EndsWith(visualBasicSourceFileExtension))
            {
                sourceFileRootNode = VisualBasicSyntaxTree.ParseText(source).GetRoot();
            };
            return sourceFileRootNode;
        }

        internal void CalculateMetricsWith(ICollection<ICodeAnalyzer> analyzers)
        {
            LinesOfCode = File.ReadAllLines(absPath).Count();
            var syntax = GetSyntaxNodeFromFile(absPath);
            FileName = absPath;
            Language = syntax.Language;
            MethodScores = analyzers.SelectMany(a => a.GetMethodScores(syntax)).ToList();
        }
    }
}