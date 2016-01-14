using System;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inspector.Infrastructure
{
    public static class BaseMethodDeclarationSyntaxExtensions
    {
        public static string GetBodyText(this BaseMethodDeclarationSyntax method)
        {
            var body = method.Body.WithoutTrivia();

            if (body != null)
            {
                using (var writer = new StringWriter())
                {
                    body.WriteTo(writer);
                    var bodyWithBraces = writer.ToString();
                    return StripBracesFrom(bodyWithBraces);
                }
            }
            return string.Empty;
        }

        private static string StripBracesFrom(string bodyWithBraces)
        {
            int start = 0;
            int end = bodyWithBraces.Length - 1;

            if (bodyWithBraces[0] == '{')
            {
                start++;
                end--;
            }

            if (bodyWithBraces[end] == '}')
                end--;

            return bodyWithBraces.Substring(start, end).Trim();
        }

        public static int GetLineCount(this BaseMethodDeclarationSyntax method)
        {
            var lines = method.GetBodyText();
            if (String.IsNullOrEmpty(lines))
                return 0;
            return lines.Split('\n').Count();
        }
    }
}
