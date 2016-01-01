using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.CodeAnalysis;
using Inspector.Analyzers.CSharp;
using Microsoft.CodeAnalysis.CSharp;

namespace InspectionTests.Analyzers.CSharp
{

    [TestClass]
    public class ControlFlowComplexityTests
    {
        [TestMethod]
        public void EmptyMethod_ShouldReturn_MethodWithScoreOf_0()
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

            var sut = new ControlFlowComplexity();
            var results = sut.GetMethodScores(parsedNode);

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual("bool TestMe (int i)", results.First().Method);
            Assert.AreEqual(0, results.First().Score);
        }

        [TestMethod]
        public void SimpleIf_ShouldReturn_MethodWithScoreOf_1()
        {
            var parsedNode = GetSourceAsSyntaxTree(@"
                using System;
                using System.Text;

                [Serializable]
                public class TestClass {
                    public TestClass() { }
                    
                    public bool TestMe(int i) {
                        if (i == 10) 
                            return true;

                        return false;
                    }
                }
                ");

            var sut = new ControlFlowComplexity();
            var results = sut.GetMethodScores(parsedNode);

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual("bool TestMe (int i)", results.First().Method);
            Assert.AreEqual(1, results.First().Score);
        }


        [TestMethod]
        public void IfElseIf_ShouldReturn_MethodWithScoreOf_2()
        {
            var parsedNode = GetSourceAsSyntaxTree(@"
                using System;
                using System.Text;

                [Serializable]
                public class TestClass {
                    public TestClass() { }
                    
                    public bool TestMe(int i) {
                        if (i == 10) 
                            return true;
                        else if (i>10) 
                            return true;
                        else
                            return false;
                        
                        return false;
                    }
                }
                ");

            var sut = new ControlFlowComplexity();
            var results = sut.GetMethodScores(parsedNode);

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual("bool TestMe (int i)", results.First().Method);
            Assert.AreEqual(2, results.First().Score);
        }


        [TestMethod]
        public void IfElseIfAndReturnWithExpression_ShouldReturn_MethodWithScoreOf_3()
        {
            var parsedNode = GetSourceAsSyntaxTree(@"
                using System;
                using System.Text;

                [Serializable]
                public class TestClass {
                    public TestClass() { }
                    
                    public bool TestMe(int i) {
                        if (i == 10) 
                            return true;
                        else if (i>10) 
                            return true;                        

                        return i==0;
                    }
                }
                ");

            var sut = new ControlFlowComplexity();
            var results = sut.GetMethodScores(parsedNode);

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual("bool TestMe (int i)", results.First().Method);
            Assert.AreEqual(3, results.First().Score);
        }

        [TestMethod]
        public void IfWithAnd_ShouldReturn_MethodWithScoreOf_3()
        {
            var parsedNode = GetSourceAsSyntaxTree(@"
                using System;
                using System.Text;

                [Serializable]
                public class TestClass {
                    public TestClass() { }
                    
                    public bool TestMe(int i) {
                        var check = false;
                        if ((i == 10) && (check==true))
                            return true;
                    }
                }
                ");

            var sut = new ControlFlowComplexity();
            var results = sut.GetMethodScores(parsedNode);

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual("bool TestMe (int i)", results.First().Method);
            Assert.AreEqual(3, results.First().Score);
        }

        [TestMethod]
        public void SingleLineIf_ShouldReturn_MethodWithScoreOf_1()
        {
            var parsedNode = GetSourceAsSyntaxTree(@"
                using System;
                using System.Text;

                [Serializable]
                public class TestClass {
                    public TestClass() { }
                    
                    public bool TestMe(int i) {
                        if (i == 10) return true;
                    }
                }
                ");

            var sut = new ControlFlowComplexity();
            var results = sut.GetMethodScores(parsedNode);

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual("bool TestMe (int i)", results.First().Method);
            Assert.AreEqual(1, results.First().Score);
        }

        [TestMethod]
        public void SelectCaseWith1Option_ShouldReturn_MethodWithScoreOf_1()
        {
            var parsedNode = GetSourceAsSyntaxTree(@"
                using System;
                using System.Text;

                [Serializable]
                public class TestClass {
                    public TestClass() { }
                    
                    public bool TestMe(int i) {
                        switch (i)
                        {
                            case 10:
                                break;
                        }
                        return false;
                    }
                }
                ");

            var sut = new ControlFlowComplexity();
            var results = sut.GetMethodScores(parsedNode);

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual("bool TestMe (int i)", results.First().Method);
            Assert.AreEqual(1, results.First().Score);
        }

        [TestMethod]
        public void SelectCaseWith2Options_ShouldReturn_MethodWithScoreOf_2()
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
                                return true;
                                break;
                            case 1:
                                return true;
                                break;
                        }

                        return false;
                    }
                }
                ");

            var sut = new ControlFlowComplexity();
            var results = sut.GetMethodScores(parsedNode);

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual("bool TestMe (int i)", results.First().Method);
            Assert.AreEqual(2, results.First().Score);
        }

        [TestMethod]
        public void SelectCaseWith2OptionsAndEmbeddedIf_ShouldReturn_MethodWithScoreOf_3()
        {
            var parsedNode = GetSourceAsSyntaxTree(@"
                using System;
                using System.Text;

                [Serializable]
                public class TestClass {
                    public TestClass() { }
                    
                    public bool TestMe(int i) {
                        var z = true;
                        switch (i) {
                            case 0:
                                if (z == true)  
                                    return true;       
                                break;
                            case 1:
                                return true;
                                break;
                        }

                        return false;
                    }
                }
                ");

            var sut = new ControlFlowComplexity();
            var results = sut.GetMethodScores(parsedNode);

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual("bool TestMe (int i)", results.First().Method);
            Assert.AreEqual(3, results.First().Score);
        }

        [TestMethod]
        public void ReturnWithExpression_ShouldReturn_MethodWithScoreOf_1()
        {
            var parsedNode = GetSourceAsSyntaxTree(@"
                using System;
                using System.Text;

                [Serializable]
                public class TestClass {
                    public TestClass() { }
                    
                    public bool TestMe(int i) {
                        return i > 0;
                    }
                }
                ");

            var sut = new ControlFlowComplexity();
            var results = sut.GetMethodScores(parsedNode);

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual("bool TestMe (int i)", results.First().Method);
            Assert.AreEqual(1, results.First().Score);
        }

        [TestMethod]
        public void IfWithBoolean_ShouldReturn_MethodWithScoreOf_1()
        {
            var parsedNode = GetSourceAsSyntaxTree(@"
                using System;
                using System.Text;

                [Serializable]
                public class TestClass {
                    public TestClass() { }
                    
                    public bool TestMe(int i) {
                        if (true) return false; else return true;
                    }
                }
                ");

            var sut = new ControlFlowComplexity();
            var results = sut.GetMethodScores(parsedNode);

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual("bool TestMe (int i)", results.First().Method);
            Assert.AreEqual(1, results.First().Score);
        }

        [TestMethod]
        public void IfWithBooleanMultiLine_ShouldReturn_MethodWithScoreOf_1()
        {
            var parsedNode = GetSourceAsSyntaxTree(@"
                using System;
                using System.Text;

                [Serializable]
                public class TestClass {
                    public TestClass() { }
                    
                    public bool TestMe(int i) {
                        if (true)
                            return false;
                        
                        return true;
                    }
                }
                ");

            var sut = new ControlFlowComplexity();
            var results = sut.GetMethodScores(parsedNode);

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual("bool TestMe (int i)", results.First().Method);
            Assert.AreEqual(1, results.First().Score);
        }

        [TestMethod]
        public void ElseIfWithBoolean_ShouldReturn_MethodWithScoreOf_2()
        {
            var parsedNode = GetSourceAsSyntaxTree(@"
                using System;
                using System.Text;

                [Serializable]
                public class TestClass {
                    public TestClass() { }
                    
                    public bool TestMe(int i) {
                        if (true)
                            return false;
                        else if (false)
                            return true;
                    }
                }
                ");

            var sut = new ControlFlowComplexity();
            var results = sut.GetMethodScores(parsedNode);

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual("bool TestMe (int i)", results.First().Method);
            Assert.AreEqual(2, results.First().Score);
        }
        private static Microsoft.CodeAnalysis.SyntaxNode GetSourceAsSyntaxTree(string vbCode)
        {
            var parsedNode = CSharpSyntaxTree.ParseText(vbCode);
            return parsedNode.GetRoot();
        }
    }
}
