using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Inspector.Analyzers
{
    public interface ICodeAnalyzer
    {
        IEnumerable<MethodScore> GetMethodScores(SyntaxNode node);
    }
}