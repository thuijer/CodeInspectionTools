using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Inspector.Analyzers.VisualBasic
{
    /// <summary>
    /// Calculates the method length for all methods in a given class.
    /// Method length includes all lines, including blanc and comment lines
    /// </summary>
    public class MethodLength
    {
        public IEnumerable<MethodScore> GetMethodScores(SyntaxNode node)
        {
            var methods = node.DescendantNodes().OfType<MethodBlockSyntax>();
            foreach (var item in methods)
            {
                string fullMethod = string.Empty;
                fullMethod = item.ToFullString();

                var lines = fullMethod.Split('\n');

                var totalLength = lines.Length - 1;

                yield return new MethodScore
                {
                    Method = $"{item.BlockStatement.DeclarationKeyword} {(item.BlockStatement as Microsoft.CodeAnalysis.VisualBasic.Syntax.MethodStatementSyntax).Identifier}",
                    Score = totalLength
                };
            };
        }
    }
}
