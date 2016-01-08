using System.Collections.Generic;
using Inspector.CodeMetrics.Scores;
using Microsoft.CodeAnalysis;

namespace Inspector.CodeMetrics.CSharp
{
    public class ClassMembersCount: CSharpAnalyzer
    {
        public override IEnumerable<ClassMembersScore> GetMetrics<ClassMembersScore>(SyntaxNode node)
        {
            return null;
        }
    }
}
