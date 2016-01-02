using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace Inspector.CodeMetrics.VisualBasic
{

    public class CommentLocator : CodeMetrics.CommentLocator
    {
        public CommentLocator(SyntaxNode node) : base(node)
        {

        }

        private static HashSet<Microsoft.CodeAnalysis.VisualBasic.SyntaxKind> _commentTypesVB = new HashSet<Microsoft.CodeAnalysis.VisualBasic.SyntaxKind>(new[] {
                Microsoft.CodeAnalysis.VisualBasic.SyntaxKind.CommentTrivia
            });

        protected override bool IsComment(SyntaxTrivia trivia)
        {
            return _commentTypesVB.Contains(Microsoft.CodeAnalysis.VisualBasic.VisualBasicExtensions.Kind(trivia));
        }
    }
}
