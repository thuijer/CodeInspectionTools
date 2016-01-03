using Inspector.CodeMetrics.Scores;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.VisualBasic;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inspector.CodeMetrics.VisualBasic
{
    public class MagicString : VisualBasicAnalyzer
    {
        public override IEnumerable<MethodScore> GetMetrics(SyntaxNode node)
        {
            return GetMethods(node).ToList().Select(ms =>
            {
                int score = CalculateScore(ms);
                return CreateScore<MagicStringScore>(ms, score);
            });
        }

        private int CalculateScore(MethodBlockSyntax method)
        {
            var nodes = method.DescendantNodes();
            var ifs = nodes.OfType<BinaryExpressionSyntax>().Where(bes => 
                bes.Left.IsKind(SyntaxKind.StringLiteralExpression) || 
                bes.Right.IsKind(SyntaxKind.StringLiteralExpression));
            var cases = nodes.OfType<SimpleCaseClauseSyntax>().Where(ccs => ccs.Value.IsKind(SyntaxKind.StringLiteralExpression)) ;

            return ifs.Count() + cases.Count();
        }
    }
}
