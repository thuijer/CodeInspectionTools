using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inspector.Infrastructure
{
    static class MethodDeclarationSyntaxExtensions
    {
        public static string GetBodyText(this MethodDeclarationSyntax method)
        {
            var body = method.Body;
            if (body != null)
            {
                using (var writer = new StringWriter())
                {
                    body.WriteTo(writer);
                    return writer.ToString();
                }
            }
            return string.Empty;
        }

        public static int GetLineCount(this MethodDeclarationSyntax method)
        {
            return method.GetBodyText().Split('\n').Count();
        }
    }
}
