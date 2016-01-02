using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;

namespace Inspector.CodeMetrics.CSharp
{
    public class NestingLevel : CSharpAnalyzer
    {
        public override IEnumerable<MethodScore> GetMetrics(SyntaxNode node)
        {
            return GetMethods(node).ToList().Select(m =>
            {
                var visitor = new Walker();
                visitor.Visit(m);

                int score = visitor.MaxLevel;
                return CreateScore<NestingLevelScore>(m, score);
            }
            );

        }
        private class Walker : CSharpSyntaxWalker
        {

            public override void VisitIfStatement(IfStatementSyntax node)
            {
                IncreaseLevel();

                base.VisitIfStatement(node);

                DecreaseLevel();
            }

            private void DecreaseLevel()
            {
                currentLevel--;
            }

            private void IncreaseLevel()
            {
                currentLevel++;

                if (currentLevel > MaxLevel)
                    MaxLevel = currentLevel;
            }

            public override void VisitCaseSwitchLabel(CaseSwitchLabelSyntax node)
            {
                IncreaseLevel();
                base.VisitCaseSwitchLabel(node);

                //Do not decrease, because CaseSwitchLabel works differently then if
            }

            public override void VisitForStatement(ForStatementSyntax node)
            {
                IncreaseLevel();
                base.VisitForStatement(node);
                DecreaseLevel();
            }
            private int currentLevel = 0;
            public int MaxLevel { get; private set; } = 0;
        }
    }
}
