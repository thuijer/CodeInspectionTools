using System.Collections.Generic;
using System.Linq;
using Inspector.CodeMetrics.Scores;
using Microsoft.CodeAnalysis;

namespace Inspector.CodeMetrics.CSharp
{
    public class ClassMembers : CSharpAnalyzer
    {
        public override IEnumerable<CodeMetricScore> GetMetrics(SyntaxNode node)
        {
            return GetClasses(node).Select(
                cls => new CodeMetricScore
                {
                    ClassName = cls.Identifier.ValueText,
                    Score = cls.Members.Count
                });
        }       
    }
}
