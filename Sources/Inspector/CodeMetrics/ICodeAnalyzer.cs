using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Inspector.CodeMetrics
{
    public interface ICodeMetricAnalyzer
    {
        IEnumerable<MethodScore> GetMetrics(SyntaxNode node);
    }
}