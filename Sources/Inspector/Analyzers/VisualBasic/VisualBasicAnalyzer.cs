using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;

namespace Inspector.Analyzers.VisualBasic
{
    public abstract class VisualBasicAnalyzer : ICodeAnalyzer
    {
        public abstract IEnumerable<MethodScore> GetMethodScores(SyntaxNode node);

        protected MethodScore CreateScore<T>(MethodBlockSyntax m, int score) where T: MethodScore, new()
        {
            return new T()
            {
                ClassName = GetClassName(m),
                Method = GetMethodName(m),
                Score = score
            };
        }
        private string GetClassName(MethodBlockSyntax m)
        {

            var moduleBlock = m.Parent as Microsoft.CodeAnalysis.VisualBasic.Syntax.ModuleBlockSyntax;
            if (moduleBlock != null)
                return moduleBlock.ModuleStatement.Identifier.ValueText;

            var classBlock = m.Parent as Microsoft.CodeAnalysis.VisualBasic.Syntax.ClassBlockSyntax;
            if (classBlock != null)
                return classBlock.ClassStatement.Identifier.ValueText;

            return "n/a";
        }

        private string GetMethodName(MethodBlockSyntax m)
        {
            var data = m.BlockStatement as MethodStatementSyntax;
            return $"{ data.DeclarationKeyword } { data.Identifier }{data.ParameterList.ToString()}";
        }

        protected IEnumerable<MethodBlockSyntax> GetMethods(SyntaxNode node)
        {
            return node.DescendantNodes().OfType<MethodBlockSyntax>();
        }
    }
}
