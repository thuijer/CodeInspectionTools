using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;

namespace SolutionCrawler
{
    public class Wip1_VagueToDoIdentifier
    {
  
        private readonly Predicate<string> _toDoCommentMatcher;
        public Wip1_VagueToDoIdentifier(Predicate<string> toDoCommentMatcher)
        {
            if (toDoCommentMatcher == null)
                throw new ArgumentNullException("toDoCommentMatcher");

            _toDoCommentMatcher = toDoCommentMatcher;
        }
        public Wip1_VagueToDoIdentifier() : this(DefaultToDoCommentFilter) { }

        public static Predicate<string> DefaultToDoCommentFilter
        {
            get
            {
                // TODO: Tidy this up with a reg ex?
                return commentContent =>
                    commentContent.Contains("//TODO") ||
                    commentContent.Contains("// TODO") ||
                    commentContent.Contains("TODO:") ||
                    commentContent.Contains("TODO[") ||
                    commentContent.Contains("TODO [") ||
                    commentContent.Contains("TODO\r") ||
                    commentContent.Contains("TODO\n") ||
                    commentContent.EndsWith("TODO");
            }
        }

        public IEnumerable<Comment> GetToDoComments(SyntaxNode syntaxNode)
        {
            if (syntaxNode == null)
                throw new ArgumentException("syntaxNode");

            var todoComments = new List<Comment>();
            
            var commentLocatingVisitor = new CommentLocatingVisitor(
                comment =>
                {
                    if (_toDoCommentMatcher(comment.Content))
                        todoComments.Add(comment);
                }
            );
            commentLocatingVisitor.Visit(
                syntaxNode
            );

            return todoComments;
        }

        private class CommentLocatingVisitor : SyntaxWalker
        {
            private const string csharp = "C#";
            private const string vb = "Visual Basic";
            private readonly Action<Comment> _commentLocated;
            public CommentLocatingVisitor(Action<Comment> commentLocated) : base(SyntaxWalkerDepth.StructuredTrivia)
            {
                if (commentLocated == null)
                    throw new ArgumentNullException("commentLocated");

                _commentLocated = commentLocated;
            }

            private static HashSet<SyntaxKind> _commentTypesCS = new HashSet<SyntaxKind>(new[] {
                Microsoft.CodeAnalysis.CSharp.SyntaxKind.SingleLineCommentTrivia,
                Microsoft.CodeAnalysis.CSharp.SyntaxKind.MultiLineCommentTrivia,
                Microsoft.CodeAnalysis.CSharp.SyntaxKind.DocumentationCommentExteriorTrivia,
                Microsoft.CodeAnalysis.CSharp.SyntaxKind.SingleLineDocumentationCommentTrivia,
                Microsoft.CodeAnalysis.CSharp.SyntaxKind.MultiLineDocumentationCommentTrivia,
            });

            private static HashSet<Microsoft.CodeAnalysis.VisualBasic.SyntaxKind> _commentTypesVB = new HashSet<Microsoft.CodeAnalysis.VisualBasic.SyntaxKind>(new[] {
                Microsoft.CodeAnalysis.VisualBasic.SyntaxKind.CommentTrivia
            });

            protected override void VisitTrivia(SyntaxTrivia trivia)
            {                
                if (IsComment(trivia))
                {
                    string triviaContent;
                    using (var writer = new StringWriter())
                    {
                        trivia.WriteTo(writer);
                        triviaContent = writer.ToString();
                    }

                    // Note: When looking for the containingMethodOrPropertyIfAny, we want MemberDeclarationSyntax types such as ConstructorDeclarationSyntax, MethodDeclarationSyntax,
                    // IndexerDeclarationSyntax, PropertyDeclarationSyntax but NamespaceDeclarationSyntax and TypeDeclarationSyntax also inherit from MemberDeclarationSyntax and we
                    // don't want those
                    var containingNode = trivia.Token.Parent;
                    var containingMethodOrPropertyIfAny = TryToGetContainingNode<MemberDeclarationSyntax>(
                        containingNode,
                        n => !(n is NamespaceDeclarationSyntax) && !(n is TypeDeclarationSyntax)
                    );
                    var containingTypeIfAny = TryToGetContainingNode<TypeDeclarationSyntax>(containingNode);
                    var containingNameSpaceIfAny = TryToGetContainingNode<NamespaceDeclarationSyntax>(containingNode);
                    _commentLocated(new Comment(
                        triviaContent,
                        trivia.SyntaxTree.GetLineSpan(trivia.Span).StartLinePosition.Line,
                        containingMethodOrPropertyIfAny,
                        containingTypeIfAny,
                        containingNameSpaceIfAny
                    ));
                }
                base.VisitTrivia(trivia);
            }

            private bool IsComment(SyntaxTrivia trivia)
            {
                switch (trivia.Language)
                {
                    case csharp:
                        return _commentTypesCS.Contains(trivia.Kind());
                    case vb:
                        return _commentTypesVB.Contains(Microsoft.CodeAnalysis.VisualBasic.VisualBasicExtensions.Kind(trivia));
                    default:
                        return false;
                }                    
            }

            private T TryToGetContainingNode<T>(SyntaxNode node, Predicate<T> optionalFilter = null) where T : SyntaxNode
            {
                if (node == null)
                    throw new ArgumentNullException("node");

                var currentNode = node;
                while (true)
                {
                    var nodeOfType = currentNode as T;
                    if (nodeOfType != null)
                    {
                        if ((optionalFilter == null) || optionalFilter(nodeOfType))
                            return nodeOfType;
                    }
                    if (currentNode.Parent == null)
                        break;
                    currentNode = currentNode.Parent;
                }
                return null;
            }
        }
    }
}
