using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Inspector.CodeMetrics.CSharp;
using FluentAssertions;
using System.Linq;

namespace InspectionTests.CodeMetricsTests.CSharp
{
    [TestClass]
    public class MagicNumberTests : CsharpMetricTest
    {
        [TestMethod]
        public void EmptyMethod_ShouldHave_Score0()
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

            var sut = new MagicNumber();
            var results = sut.GetMetrics(parsedNode);

            results.Count().Should().Be(1);
            results.First().Method.Should().Be("bool TestMe (int i)");
            results.First().Score.Should().Be(0);
        }

        [TestMethod]
        public void Number0or1_ShouldHave_Score0()
        {
            var parsedNode = GetSourceAsSyntaxTree(@"
                using System;
                using System.Text;

                [Serializable]
                public class TestClass {
                    public TestClass() { }
                    
                    public bool TestMe(int i) {
                        if (i > 0)
                            return true;
                        return false;
                    }
                }
                ");

            var sut = new MagicNumber();
            var results = sut.GetMetrics(parsedNode);

            results.Count().Should().Be(1);
            results.First().Method.Should().Be("bool TestMe (int i)");
            results.First().Score.Should().Be(0);
        }

        [TestMethod]
        public void NumberHigherThan1_ShouldHave_Score1()
        {
            var parsedNode = GetSourceAsSyntaxTree(@"
                using System;
                using System.Text;

                [Serializable]
                public class TestClass {
                    public TestClass() { }
                    
                    public bool TestMe(int i) {
                        if (i > 10)
                            return true;
                        return false;
                    }
                }
                ");

            var sut = new MagicNumber();
            var results = sut.GetMetrics(parsedNode);

            results.Count().Should().Be(1);
            results.First().Method.Should().Be("bool TestMe (int i)");
            results.First().Score.Should().Be(1);
        }

        [TestMethod]
        public void NumberHigherThan1AndVariable_ShouldHave_Score1()
        {
            var parsedNode = GetSourceAsSyntaxTree(@"
                using System;
                using System.Text;

                [Serializable]
                public class TestClass {
                    public TestClass() { }
                    
                    public bool TestMe(int i) {
                        int j=10;
                        if (i > 10)
                            return true;
                        return false;
                    }
                }
                ");

            var sut = new MagicNumber();
            var results = sut.GetMetrics(parsedNode);

            results.Count().Should().Be(1);
            results.First().Method.Should().Be("bool TestMe (int i)");
            results.First().Score.Should().Be(1);
        }

        [TestMethod]
        public void ParameterDefaultValue_Should_NotRaiseScore()
        {
            var parsedNode = GetSourceAsSyntaxTree(@"
                using System;
                using System.Text;

                [Serializable]
                public class TestClass {
                    public TestClass() { }
                    
                    public bool TestMe(int i=1) {
                        if (i > 10)
                            return true;
                        return false;
                    }
                }
                ");

            var sut = new MagicNumber();
            var results = sut.GetMetrics(parsedNode);

            results.Count().Should().Be(1);
            results.First().Method.Should().Be("bool TestMe (int i=1)");
            results.First().Score.Should().Be(1);
        }


        [TestMethod]
        public void Case_ShouldHave_Score1()
        {
            var parsedNode = GetSourceAsSyntaxTree(@"
                using System;
                using System.Text;

                [Serializable]
                public class TestClass {
                    public TestClass() { }
                    
                    public bool TestMe(int i) {
                        switch (i) {
                            case 0:
                                break;
                            case 1:
                                break;
                            case 10:
                                return true;
                        }
                        return false;
                    }
                }
                ");

            var sut = new MagicNumber();
            var results = sut.GetMetrics(parsedNode);

            results.Count().Should().Be(1);
            results.First().Method.Should().Be("bool TestMe (int i)");
            results.First().Score.Should().Be(1);
        }

        [TestMethod]
        public void NumberNegative1_ShouldHave_Score1()
        {
            var parsedNode = GetSourceAsSyntaxTree(@"
                using System;
                using System.Text;

                [Serializable]
                public class TestClass {
                    public TestClass() { }
                    
                    public bool TestMe(int i) {
                        if (i == -1)
                            return true;
                        return false;
                    }
                }
                ");

            var sut = new MagicNumber();
            var results = sut.GetMetrics(parsedNode);

            results.Count().Should().Be(1);
            results.First().Method.Should().Be("bool TestMe (int i)");
            results.First().Score.Should().Be(1);
        }

        [TestMethod]
        public void ReturnWithInlineComparisionAndNumber_ShouldHave_Score1()
        {
            var parsedNode = GetSourceAsSyntaxTree(@"
                using System;
                using System.Text;

                [Serializable]
                public class TestClass {
                    public TestClass() { }
                    
                    public bool TestMe(int i) {
                        return i>10;
                    }
                }
                ");

            var sut = new MagicNumber();
            var results = sut.GetMetrics(parsedNode);

            results.Count().Should().Be(1);
            results.First().Method.Should().Be("bool TestMe (int i)");
            results.First().Score.Should().Be(1);
        }

        [TestMethod]
        public void Constant_ShouldHave_Score0()
        {
            var parsedNode = GetSourceAsSyntaxTree(@"
                using System;
                using System.Text;

                [Serializable]
                public class TestClass {
                    public TestClass() { }
                    
                    public bool TestMe(int i) {
                        const j=5;
                        
                        return i>0;
                    }
                }
                ");

            var sut = new MagicNumber();
            var results = sut.GetMetrics(parsedNode);

            results.Count().Should().Be(1);
            results.First().Method.Should().Be("bool TestMe (int i)");
            results.First().Score.Should().Be(0);
        }
    }
}
