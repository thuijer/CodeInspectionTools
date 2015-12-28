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
            var results = new List<IfsqScore>();
            var methods = GetMethods(node);

            foreach (var m in methods)
            {
                results.Add(new IfsqScore
                {
                    Method = GetMethodNameTemplate(m),
                    Score = GetScore(m)
                });
            }
            return results;
        }

        private string GetMethodNameTemplate(MethodBlockSyntax m)
        {
            return $"{ m.BlockStatement.DeclarationKeyword } { (m.BlockStatement as MethodStatementSyntax).Identifier }";
        }

        private int GetScore(MethodBlockSyntax method)
        {
            var ifs = method.DescendantNodes().OfType<BinaryExpressionSyntax>();
            var cases = method.DescendantNodes().OfType<CaseClauseSyntax>();
            var boolExpr = method.DescendantNodes().OfType<SingleLineIfStatementSyntax>().Where(c=>c.Condition is LiteralExpressionSyntax);
            var boolExprMulti = method.DescendantNodes().OfType<IfStatementSyntax>().Where(c => c.Condition is LiteralExpressionSyntax);
            var boolExprElse = method.DescendantNodes().OfType<ElseIfStatementSyntax>().Where(c => c.Condition is LiteralExpressionSyntax);

            return ifs.Count() + cases.Count() + boolExpr.Count() + boolExprMulti.Count() + boolExprElse.Count();
        }

        private IEnumerable<MethodBlockSyntax> GetMethods(SyntaxNode node)
        {
            return node.DescendantNodes().OfType<MethodBlockSyntax>();
        }
    }
}
