using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.CodeAnalysis;

namespace SolutionCrawler.VB
{
    public class Sp3_TooComplex
    {
        public IEnumerable<IfsqScore> GetMethodScores(SyntaxNode node)
        {
            return GetMethods(node).Select(m => new IfsqScore
            {
                Method = GetMethodNameTemplate(m),
                Score = GetScore(m)
            });
        }

        private string GetMethodNameTemplate(MethodBlockSyntax m)
        {
            return $"{ m.BlockStatement.DeclarationKeyword } { (m.BlockStatement as MethodStatementSyntax).Identifier }";
        }

        private int GetScore(MethodBlockSyntax method)
        {
            var nodes = method.DescendantNodes();
            var ifs = nodes.OfType<BinaryExpressionSyntax>();
            var cases = nodes.OfType<CaseClauseSyntax>();
            var boolExpr = nodes.OfType<SingleLineIfStatementSyntax>().Where(c=>c.Condition is LiteralExpressionSyntax);
            var boolExprMulti = nodes.OfType<IfStatementSyntax>().Where(c => c.Condition is LiteralExpressionSyntax);
            var boolExprElse = nodes.OfType<ElseIfStatementSyntax>().Where(c => c.Condition is LiteralExpressionSyntax);

            return ifs.Count() + cases.Count() + boolExpr.Count() + boolExprMulti.Count() + boolExprElse.Count();
        }

        private IEnumerable<MethodBlockSyntax> GetMethods(SyntaxNode node)
        {
            return node.DescendantNodes().OfType<MethodBlockSyntax>();
        }
    }
}
