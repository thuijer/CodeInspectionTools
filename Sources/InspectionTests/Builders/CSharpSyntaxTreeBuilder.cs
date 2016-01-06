using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace InspectionTests.Builders
{
    class CSharpSyntaxTreeBuilder
    {
        internal SyntaxNode FromSource(string csharpCode)
        {
            return CSharpSyntaxTree.ParseText(csharpCode).GetRoot();
        }
    }
}
