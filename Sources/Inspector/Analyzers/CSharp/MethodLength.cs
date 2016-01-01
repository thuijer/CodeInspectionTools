using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;

namespace Inspector.Analyzers.CSharp
{
    public class MethodLength : CSharpAnalyzer
    {
        public override IEnumerable<MethodScore> GetMethodScores(SyntaxNode node)
        {
            return GetMethods(node).Select(item =>
            {
                string fullMethod = string.Empty;
                using (var writer = new StringWriter())
                {
                    item.WriteTo(writer);
                    fullMethod = writer.ToString();
                }
                var lines = fullMethod.Split('\n');

                var totalLength = lines.Length - 1;
                var emptyLines = lines.Where(l => string.IsNullOrWhiteSpace(l)).Count();
                var linesStartingWithComment = lines.Where(l => l.Trim().StartsWith("//")).Count();

                var methodName = $"{ item.ReturnType } { item.Identifier}";
                var className = item.Parent.ToString();

                return CreateScore<MethodLengthScore>(item, totalLength - emptyLines - linesStartingWithComment);
            });
        }
    }
}
