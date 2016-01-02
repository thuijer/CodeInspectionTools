using Microsoft.CodeAnalysis.CSharp;

namespace InspectionTests.CodeMetricsTests.CSharp
{
    public class CsharpMetricTest
    {
        protected Microsoft.CodeAnalysis.SyntaxNode GetSourceAsSyntaxTree(string csharpCode)
        {
            var parsedNode = CSharpSyntaxTree.ParseText(csharpCode);
            return parsedNode.GetRoot();
        }

    }
}