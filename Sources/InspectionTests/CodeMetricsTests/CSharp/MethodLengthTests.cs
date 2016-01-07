using System.Linq;
using FluentAssertions;
using InspectionTests.Builders;
using Inspector.CodeMetrics.CSharp;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InspectionTests.CodeMetricsTests.CSharp
{
    [TestClass]
    public class MethodLengthTests
    {
        [TestMethod]
        public void EmptyMethod_ShouldReturn_MethodWithScoreOf_2()
        {
            var parsedNode = new CSharpSyntaxTreeBuilder().FromSource(@"
                using System;
                using System.Text;

                [Serializable]
                public class TestClass {
                    public TestClass() { }
                    
                    public bool TestMe(int i) 
                    {                        
                    }
                }
                ");

            var sut = new MethodLength();
            var results = sut.GetMetrics(parsedNode);

            results.Should().HaveCount(1);
            results.First().Method.Should().Be("bool TestMe (int i)");
            results.First().Score.Should().Be(2);
        }

        [TestMethod]
        public void MethodWith25Lines_ShouldReturn_MethodWithScoreOf_25()
        {
            var parsedNode = new CSharpSyntaxTreeBuilder().FromSource(@"
                using System;
                using System.Text;

                [Serializable]
                public class TestClass {
                    public TestClass() { }
                    
                    public bool TestMe(int i) 
                    {                        
                        if (i == 10) 
                            return true;
                        if (i == 10) 
                            return true;
                        
                        if (i == 10) 
                            return true;


                        if (i == 10) 
                            return true;
                        if (i == 10) 
                            return true;

                        if (i == 10) 
                            return true;
                        if (i == 10) 
                            return true;
                        if (i == 10) 
                            return true;
                        if (i == 10) 
                            return true;                       
                        return true;
                     }
                }
                ");

            var sut = new MethodLength();
            var results = sut.GetMetrics(parsedNode);

            results.Should().HaveCount(1);
            results.First().Method.Should().Be("bool TestMe (int i)");
            results.First().Score.Should().Be(25);
        }     
    }
}
