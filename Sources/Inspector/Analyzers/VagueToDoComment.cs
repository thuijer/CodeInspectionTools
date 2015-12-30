using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Inspector.Analyzers
{
    public class VagueToDoComment
    {
        private const string csharp = "C#";
        private readonly Predicate<string> _toDoCommentMatcher;
        public VagueToDoComment(Predicate<string> toDoCommentMatcher)
        {
            if (toDoCommentMatcher == null)
                throw new ArgumentNullException("toDoCommentMatcher");

            _toDoCommentMatcher = toDoCommentMatcher;
        }
        public VagueToDoComment() : this(DefaultToDoCommentFilter) { }

        public static Predicate<string> DefaultToDoCommentFilter
        {
            get
            {               
                return commentContent =>
                    Regex.IsMatch(commentContent, @"(//\s)*TO\s*DO\s*[:\[\r\n]+") ||
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

        public IEnumerable<Comment> GetComments (SyntaxNode node)
        {
            Analyzers.CommentLocator cl;
            if (node.Language == csharp)
                cl = new Analyzers.CSharp.CommentLocator(node);
            else
                cl = new Analyzers.VisualBasic.CommentLocator(node);

            return cl.GetComments(_toDoCommentMatcher);
        }
    }
}
