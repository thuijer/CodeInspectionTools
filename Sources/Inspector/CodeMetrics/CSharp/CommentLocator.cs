using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Inspector.CodeMetrics.CSharp
{
    public class CommentLocator : Generic.CommentLocator
    {
        public CommentLocator(SyntaxNode node) : base(node)
        {

        }

        private static HashSet<SyntaxKind> _commentTypesCS = new HashSet<SyntaxKind>(new[] {
                SyntaxKind.SingleLineCommentTrivia,
                SyntaxKind.MultiLineCommentTrivia,
                SyntaxKind.DocumentationCommentExteriorTrivia,
                SyntaxKind.SingleLineDocumentationCommentTrivia,
                SyntaxKind.MultiLineDocumentationCommentTrivia,
            });

        protected override bool IsComment(SyntaxTrivia trivia)
        {
            return _commentTypesCS.Contains(trivia.Kind());
        }
    }
}
