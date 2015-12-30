using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolutionCrawler
{
    public class Sp3_TooComplex
    {
        
        
        public IEnumerable<string> GetLinesWithMagicNumbers(SyntaxNode node)
        {
            List<string> results = new List<string>();
            if (node.Language == "C#")
            {
                var methods = node.DescendantNodes().OfType<MethodDeclarationSyntax>();
                foreach (var m in methods)
                {
                    var binaryCount = 0;
                    var identifier = new ComparisionIdentifier(s =>
                    {

                        binaryCount++;
                    });

                    identifier.Visit(m);


                    results.Add($"Comparisons: {m.Identifier} {binaryCount}");
                }
            }
            else
            {
                var methods = node.DescendantNodes().OfType<Microsoft.CodeAnalysis.VisualBasic.Syntax.MethodBlockSyntax>();
                foreach (var m in methods)
                {
                    //var ifs = m.DescendantNodes().OfType<Microsoft.CodeAnalysis.VisualBasic.Syntax.SingleLineIfStatementSyntax>();
                    //var ifs = m.DescendantNodes().OfType<Microsoft.CodeAnalysis.VisualBasic.Syntax.CaseBlockSyntax>();
                    //var ifs = m.DescendantNodes().OfType<Microsoft.CodeAnalysis.VisualBasic.Syntax.MultiLineIfBlockSyntax>();
                    var ifs = m.DescendantNodes().OfType<Microsoft.CodeAnalysis.VisualBasic.Syntax.BinaryExpressionSyntax>();
                    

                    foreach (var item in ifs)
                    {
                            Console.WriteLine($"{Microsoft.CodeAnalysis.VisualBasic.VisualBasicExtensions.Kind(item.Parent)} - {item}");
                    }
                    // var binaryCount = 0;
                    //var identifier = new ComparisionIdentifier(s =>
                    //{

                    //    binaryCount++;
                    //});

                    //identifier.Visit(m);


                    //results.Add($"Comparisons: {m.BlockStatement.DeclarationKeyword} {(m.BlockStatement as Microsoft.CodeAnalysis.VisualBasic.Syntax.MethodStatementSyntax).Identifier} {binaryCount}");
                }
            }

            return results; 
        }

        private class ComparisionIdentifier : SyntaxWalker
        {
            private static HashSet<SyntaxKind> _comparisonTypesCS = new HashSet<SyntaxKind>(new[] {
                SyntaxKind.EqualsEqualsToken,
                SyntaxKind.ExclamationEqualsToken,
                SyntaxKind.ExclamationToken,
                SyntaxKind.GreaterThanEqualsToken,
                SyntaxKind.GreaterThanToken,
                SyntaxKind.LessThanEqualsToken,
                SyntaxKind.LessThanToken,
                SyntaxKind.AmpersandAmpersandToken,
                SyntaxKind.BarBarToken,
            });

            private static HashSet<Microsoft.CodeAnalysis.VisualBasic.SyntaxKind> _comparisionTypesVB = new HashSet<Microsoft.CodeAnalysis.VisualBasic.SyntaxKind>(new[] {
                Microsoft.CodeAnalysis.VisualBasic.SyntaxKind.GreaterThanEqualsToken,
                Microsoft.CodeAnalysis.VisualBasic.SyntaxKind.GreaterThanToken,
                Microsoft.CodeAnalysis.VisualBasic.SyntaxKind.NotKeyword,
                Microsoft.CodeAnalysis.VisualBasic.SyntaxKind.LessThanEqualsToken,
                Microsoft.CodeAnalysis.VisualBasic.SyntaxKind.LessThanExpression,
                Microsoft.CodeAnalysis.VisualBasic.SyntaxKind.EqualsToken

                });

            private Action<string> _comparisonLocated;

            public ComparisionIdentifier(Action<string> comparisonLocated) : base(SyntaxWalkerDepth.Token)
            {
                _comparisonLocated = comparisonLocated;
            }

            protected override void VisitToken(SyntaxToken token)
            {
                if (_comparisonTypesCS.Contains(token.Kind()))
                    _comparisonLocated(token.ToString());

                var k = Microsoft.CodeAnalysis.VisualBasic.VisualBasicExtensions.Kind(token);
                if (_comparisionTypesVB.Contains(k))
                {
                    var x = token.Parent is Microsoft.CodeAnalysis.VisualBasic.Syntax.IfStatementSyntax;
                    _comparisonLocated(token.ToString());
                }

                base.VisitToken(token);
            }

            public override void Visit(SyntaxNode node)
            {
                base.Visit(node);
            }

            protected override void VisitTrivia(SyntaxTrivia trivia)
            {
                if (_comparisonTypesCS.Contains(trivia.Kind()))
                {
                    _comparisonLocated(trivia.ToString());
                }
                if (_comparisionTypesVB.Contains(Microsoft.CodeAnalysis.VisualBasic.VisualBasicExtensions.Kind(trivia)))
                {
                    _comparisonLocated(trivia.ToString());
                }
                base.VisitTrivia(trivia);
            }
        }
    }
}
