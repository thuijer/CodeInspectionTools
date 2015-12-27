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
    class Spm1_MagicNumbers
    {
        public IEnumerable<string> GetLinesWithMagicNumbers(SyntaxNode node)
        {
            var methods = node.DescendantNodes().OfType<MethodDeclarationSyntax>();
            foreach (var m in methods)
            {
                foreach(var ifs in m.DescendantNodes().OfType<IfStatementSyntax>())
                {
                    
                }
            }

            return new List<string>();
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
