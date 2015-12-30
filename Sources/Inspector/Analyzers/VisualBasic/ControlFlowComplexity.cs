using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.CodeAnalysis;

namespace Inspector.Analyzers.VisualBasic
{
    /// <summary>
    /// Calculates the control-flow complexity for all methods in a given class.
    /// Score can be used in the IfSQ Level 2 Sp-3 indicator (Count > 10)
    /// </summary>
    public class ControlFlowComplexity
    {
        public IEnumerable<MethodScore> GetMethodScores(SyntaxNode node)
        {
            return GetMethods(node).Select(m => new MethodScore
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
