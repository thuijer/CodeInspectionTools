using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Inspector.CodeMetrics.Scores;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inspector.CodeMetrics.CSharp
{
    public class MethodLength : CSharpAnalyzer
    {
        public override IEnumerable<CodeMetricScore> GetMetrics(SyntaxNode node)
        {
            return GetMethods(node).Select(item =>
            {
                string fullMethod = string.Empty;

                fullMethod = GetBodyText(item, fullMethod);

                var lines = fullMethod.Split('\n');

                var totalLength = lines.Length - 1;
                return CreateScore<MethodLengthScore>(item, totalLength);
            });
        }

        private static string GetBodyText(MethodDeclarationSyntax method, string fullMethod)
        {
            var body = method.Body;
            if (body != null)
            {
                using (var writer = new StringWriter())
                {
                    body.WriteTo(writer);
                    fullMethod = writer.ToString();
                }
            }

            return fullMethod;
        }
    }
}
