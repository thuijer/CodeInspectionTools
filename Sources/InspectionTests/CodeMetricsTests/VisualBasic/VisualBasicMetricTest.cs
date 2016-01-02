using Microsoft.CodeAnalysis.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectionTests.CodeMetricsTests.VisualBasic
{
    public class VisualBasicMetricTest
    {
        protected static Microsoft.CodeAnalysis.SyntaxNode GetSourceAsSyntaxTree(string vbCode)
        {
            var parsedNode = VisualBasicSyntaxTree.ParseText(vbCode);
            return parsedNode.GetRoot();
        }
    }
}
