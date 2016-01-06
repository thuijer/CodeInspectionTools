using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.VisualBasic;

namespace InspectionTests.Builders
{
    class VBSyntaxTreeBuilder
    {
        internal SyntaxNode FromSource(string vbCode)
        {
            var parsedNode = VisualBasicSyntaxTree.ParseText(vbCode);
            return parsedNode.GetRoot();
        }
    }
}
