using Inspector.CodeMetrics.Scores;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Inspector.CodeMetrics.VisualBasic
{
    public class EmptyStatementBlock : VisualBasicAnalyzer
    {
        public override IEnumerable<MethodScore> GetMetrics(SyntaxNode node)
        {
            return GetMethods(node).Select(m =>
            {
                int score = CalculateScore(m);
                return CreateScore<EmptyStatementBlockScore>(m, score);
            });
        }

        private int CalculateScore(MethodBlockSyntax m)
        {
            var nodes = m.DescendantNodes();

            var emptyCatchClauses = nodes.OfType<CatchBlockSyntax>().Where(s =>
            {
                if (s.Statements.Count == 0)
                    return !HasCommentWhyEmpty(s);
                else
                    return false;
            });
            var emptyFinallyClauses = nodes.OfType<FinallyBlockSyntax>().Where(s =>
            {
                if (s.Statements.Count == 0)
                    return !HasCommentWhyEmpty(s);
                else
                    return false;
            });
            var emptyIf = nodes.OfType<MultiLineIfBlockSyntax>().Where(s =>
            {
                if (s.Statements.Count == 0)
                    return !HasCommentWhyEmpty(s);
                else
                    return false;
            });

            var emptyElses = nodes.OfType<ElseBlockSyntax>().Where(s =>
            {
                if (s.Statements.Count == 0)
                    return !HasCommentWhyEmpty(s);
                else
                    return false;
            });

            var emptyElseIfs = nodes.OfType<ElseIfBlockSyntax>().Where(s =>
            {
                if (s.Statements.Count == 0)
                    return !HasCommentWhyEmpty(s);
                else
                    return false;
            });

            return emptyCatchClauses.Count() + emptyFinallyClauses.Count() + emptyIf.Count() + emptyElses.Count() + emptyElseIfs.Count();
        }

        private static bool HasCommentWhyEmpty(SyntaxNode s)
        {
            var locator = new CommentLocator(s.Parent);
            int commentCount = locator.GetComments().Count();
            return commentCount > 0; //empty catch without comments why.
        }
    }
}
