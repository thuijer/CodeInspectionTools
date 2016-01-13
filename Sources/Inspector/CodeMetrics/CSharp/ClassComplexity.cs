using System.Collections.Generic;
using System.Linq;
using Inspector.CodeMetrics.Scores;
using Inspector.Infrastructure;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inspector.CodeMetrics.CSharp
{
    public class ClassComplexity : CSharpAnalyzer
    {
        public override IEnumerable<CodeMetricScore> GetMetrics(SyntaxNode node)
        {
            return GetClasses(node).Select(
                cls => new ClassComplexityScore
                {
                    ClassName = cls.Identifier.ValueText,
                    Score = cls.Members.Count,
                    LineCount = cls.Members.OfType<MethodDeclarationSyntax>().Select(mthd => mthd.GetLineCount()).Sum()
                });
        }
    }
}
