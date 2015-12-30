using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolutionCrawler
{
    class Sp1_RoutineTooLong
    {
        public IEnumerable<string> GetMethodsTooLong(SyntaxNode node)
        {
            if (node.Language == "C#")
            {
                var methods = node.DescendantNodes().OfType<MethodDeclarationSyntax>();
                foreach (var item in methods)
                {
                    string fullMethod = string.Empty;
                    using (var writer = new StringWriter())
                    {
                        item.WriteTo(writer);
                        fullMethod = writer.ToString();
                    }
                    var lines = fullMethod.Split('\n');

                    var totalLength = lines.Length - 1;

                    if (totalLength > 150)
                        yield return $"@{item.SyntaxTree.GetLineSpan(item.Span).StartLinePosition.Line}: {item.ReturnType} {item.Identifier} (+{totalLength - 150})";
                }
            }
            else
            {
                var methods = node.DescendantNodes().OfType<Microsoft.CodeAnalysis.VisualBasic.Syntax.MethodBlockSyntax>();
                foreach (var item in methods)
                {
                    string fullMethod = string.Empty;
                    fullMethod = item.ToFullString();

                    var lines = fullMethod.Split('\n');

                    var totalLength = lines.Length - 1;

                    if (totalLength > 150)
                        yield return $"{item.SyntaxTree.GetLineSpan(item.Span).StartLinePosition.Line}: {item.BlockStatement.DeclarationKeyword} {(item.BlockStatement as Microsoft.CodeAnalysis.VisualBasic.Syntax.MethodStatementSyntax).Identifier} (+{totalLength -150})";
                }
            }
        }
        private class LongMethodIdentifier : SyntaxWalker
        {
            protected override void VisitTrivia(SyntaxTrivia trivia)
            {
                base.VisitTrivia(trivia);
            }
        }
    }
}
