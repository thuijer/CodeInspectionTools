using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Inspector.Analyzers.CSharp
{
    class MethodLength
    {
        public IEnumerable<MethodScore> GetMethodsTooLong(SyntaxNode node)
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
                var methodName = $"{ item.ReturnType } { item.Identifier}";

                yield return new MethodScore
                {
                    Method = methodName,
                    Score = totalLength
                };
            }
        }
    }
}
