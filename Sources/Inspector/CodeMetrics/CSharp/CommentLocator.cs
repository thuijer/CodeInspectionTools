using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace Inspector.CodeMetrics.CSharp
{
    public class CommentLocator : CodeMetrics.CommentLocator
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
