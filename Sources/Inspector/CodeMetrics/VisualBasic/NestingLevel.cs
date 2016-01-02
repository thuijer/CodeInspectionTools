using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.VisualBasic;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Inspector.CodeMetrics.VisualBasic
{
    public class NestingLevel : VisualBasicAnalyzer
    {
        public override IEnumerable<MethodScore> GetMetrics(SyntaxNode node)
        {
            return GetMethods(node).ToList().Select(m =>
            {
                int baseLine = m.Ancestors().Count();
                var visitor = new Walker(baseLine);
                visitor.Visit(m);

                int score = visitor.MaxLevel;
                return CreateScore<NestingLevelScore>(m, score);
            }
            );

        }
        private class Walker : VisualBasicSyntaxWalker
        {
            public override void VisitWhileBlock(WhileBlockSyntax node)
            {
                IncreaseLevel();
                base.VisitWhileBlock(node);
                DecreaseLevel();
            }

            public override void VisitDoLoopBlock(DoLoopBlockSyntax node)
            {
                IncreaseLevel();
                base.VisitDoLoopBlock(node);
                DecreaseLevel();
            }

            public override void VisitSingleLineIfStatement(SingleLineIfStatementSyntax node)
            {
                IncreaseLevel();
                base.VisitSingleLineIfStatement(node);
                DecreaseLevel();
            }

            public override void VisitMultiLineIfBlock(MultiLineIfBlockSyntax node)
            {
                IncreaseLevel();
                base.VisitMultiLineIfBlock(node);
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

            public override void VisitCaseBlock(CaseBlockSyntax node)
            {
                IncreaseLevel();
                base.VisitCaseBlock(node);
                DecreaseLevel();
            }

            private int currentLevel = 0;
            private int baseLine;

            public Walker(int baseLine)
            {
                this.baseLine = baseLine;
            }

            public int MaxLevel { get; private set; } = 0;
        }
    }
}
