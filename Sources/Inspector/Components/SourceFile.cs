using Inspector.CodeMetrics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.VisualBasic;
using System.Collections.Generic;
using System.Linq;

namespace Inspector
{
    public class SourceFile
    {
        private const string csharpSourceFileExtension = ".cs";
        private const string visualBasicSourceFileExtension = ".vb";

        public SourceFile(Project project, string srcFile, string code)
        {
            Project = project;
            project.AddSourceFile(this);

            FileName = srcFile;
            Code = code;            
            LinesOfCode = Code.Split('\n').Count();
            MethodScores = new List<MethodScore>();
        }

        public int LinesOfCode
        {
            get; internal set;
        }

        public Project Project { get; private set; }
        public string FileName { get; private set; }
        public string Language { get; private set; }

        public IEnumerable<MethodScore> MethodScores { get; private set; }

        public string Code { get; private set; }

        private SyntaxNode GetSyntaxNodeFromCode()
        {
            SyntaxNode sourceFileRootNode = null;
            if (FileName.EndsWith(csharpSourceFileExtension))
            {
                sourceFileRootNode = CSharpSyntaxTree.ParseText(Code).GetRoot();
            }
            else if (FileName.EndsWith(visualBasicSourceFileExtension))
            {
                sourceFileRootNode = VisualBasicSyntaxTree.ParseText(Code).GetRoot();
            };
            return sourceFileRootNode;
        }

        public void CalculateMetricsWith(ICollection<ICodeMetricAnalyzer> analyzers)
        {
            var syntax = GetSyntaxNodeFromCode();
            Language = syntax.Language;
            MethodScores = analyzers.SelectMany(a => a.GetMetrics(syntax)).ToList();
        }
    }
}