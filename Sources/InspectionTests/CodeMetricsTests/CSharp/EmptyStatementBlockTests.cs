using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Inspector.CodeMetrics.CSharp;
using FluentAssertions;
using System.Linq;

namespace InspectionTests.CodeMetricsTests.CSharp
{
    [TestClass]
    public class EmptyStatementBlockTests : CsharpMetricTest
    {
        [TestMethod]
        public void SimpleReturn_ShouldHave_Score0()
        {
            var parsedNode = GetSourceAsSyntaxTree(@"
                using System;
                using System.Text;

                [Serializable]
                public class TestClass {
                    public TestClass() { }
                    
                    public bool TestMe(int i) {
                        return false;
                    }
                }
                ");

            var sut = new EmptyStatementBlock();
            var results = sut.GetMetrics(parsedNode);

            results.Count().Should().Be(1);
            results.First().Method.Should().Be("bool TestMe (int i)");
            results.First().Score.Should().Be(0);
        }

        [TestMethod]
        public void EmptyCatchBlock_ShouldHave_Score1()
        {
            var parsedNode = GetSourceAsSyntaxTree(@"
                using System;
                using System.Text;

                [Serializable]
                public class TestClass {
                    public TestClass() { }
                    
                    public bool TestMe(int i) {
                        try {
                            i=i*2;
                        }
                        catch {}
                        return false;
                    }
                }
                ");

            var sut = new EmptyStatementBlock();
            var results = sut.GetMetrics(parsedNode);

            results.Count().Should().Be(1);
            results.First().Method.Should().Be("bool TestMe (int i)");
            results.First().Score.Should().Be(1);
        }

        [TestMethod]
        public void EmptyCatchBlockWithComment_ShouldHave_Score0()
        {
            var parsedNode = GetSourceAsSyntaxTree(@"
                using System;
                using System.Text;

                [Serializable]
                public class TestClass {
                    public TestClass() { }
                    
                    public bool TestMe(int i) {
                        try {
                            i=i*2;
                        }
                        catch {
                            //ignore error because...
                        }
                        return false;
                    }
                }
                ");

            var sut = new EmptyStatementBlock();
            var results = sut.GetMetrics(parsedNode);

            results.Count().Should().Be(1);
            results.First().Method.Should().Be("bool TestMe (int i)");
            results.First().Score.Should().Be(0);
        }
    }
}
