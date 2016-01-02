using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Inspector.CodeMetrics.CSharp
{
    public abstract class CSharpAnalyzer : ICodeMetricAnalyzer
    {
        public abstract IEnumerable<MethodScore> GetMetrics(SyntaxNode node);

        protected T CreateScore<T>(MethodDeclarationSyntax m, int score) where T : MethodScore, new()
        {
            T result = new T();
            result.ClassName = GetClassName(m);
            result.Method = GetMethodName(m);
            result.Score = score;

            return result;
        }
                
        private string GetClassName(MethodDeclarationSyntax m)
        {
            var classBlock = m.Parent as Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax;
            if (classBlock != null)
                return classBlock.Identifier.ValueText;

            return "n/a";
        }

        private string GetMethodName(MethodDeclarationSyntax m)
        {
            return $"{ m.ReturnType } { m.Identifier } {m.ParameterList.ToString()}";
        }

        protected IEnumerable<MethodDeclarationSyntax> GetMethods(SyntaxNode node)
        {
            return node.DescendantNodes().OfType<MethodDeclarationSyntax>();
        }
    }
}