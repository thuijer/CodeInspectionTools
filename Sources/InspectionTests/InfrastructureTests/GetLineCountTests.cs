using System.Linq;
using FluentAssertions;
using InspectionTests.Builders;
using Inspector.Infrastructure;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InspectionTests.InfrastructureTests
{
    [TestClass]
    public class GetLineCountTests
    {
        [TestMethod]
        public void EmptyBlock_ShouldReturn_0()
        {
            var parsedNode = new CSharpSyntaxTreeBuilder().FromSource(@"
                using System;
                using System.Text;

                [Serializable]
                public class TestClass {
                    public bool TestMe(int i) 
                    {                        
                    }
                }
                ");

            var body = parsedNode.DescendantNodes().OfType<MethodDeclarationSyntax>().First();

            var lines = body.GetLineCount();

            lines.Should().Be(0);
        }

        [TestMethod]
        public void EmptyBlockWithLeadingComment_ShouldReturn_0()
        {
            var parsedNode = new CSharpSyntaxTreeBuilder().FromSource(@"
                using System;
                using System.Text;

                [Serializable]
                public class TestClass {
                    public bool TestMe(int i) 
                    /* leading comment */ {                        
                    }
                }
                ");

            var body = parsedNode.DescendantNodes().OfType<MethodDeclarationSyntax>().First();

            var lines = body.GetLineCount();

            lines.Should().Be(0);
        }

        [TestMethod]
        public void EmptyBlockWithTrailingCommentBeforeOpenBrace_ShouldReturn_1()
        {
            var parsedNode = new CSharpSyntaxTreeBuilder().FromSource(@"
                using System;
                using System.Text;

                [Serializable]
                public class TestClass {
                    public bool TestMe(int i) 
                    { /* trailing comment */                       
                    }
                }
                ");

            var body = parsedNode.DescendantNodes().OfType<MethodDeclarationSyntax>().First();

            var lines = body.GetLineCount();

            lines.Should().Be(1);
        }

        [TestMethod]
        public void EmptyBlockWithTrailingCommentAfterCloseBrace_ShouldReturn_0()
        {
            var parsedNode = new CSharpSyntaxTreeBuilder().FromSource(@"
                using System;
                using System.Text;

                [Serializable]
                public class TestClass {
                    public bool TestMe(int i) 
                    {                        
                    } /* trailing comment */
                }
                ");

            var body = parsedNode.DescendantNodes().OfType<MethodDeclarationSyntax>().First();

            var lines = body.GetLineCount();

            lines.Should().Be(0);
        }

        [TestMethod]
        public void EmptyBlockWithLeadingCommentBeforeCloseBrace_ShouldReturn_1()
        {
            var parsedNode = new CSharpSyntaxTreeBuilder().FromSource(@"
                using System;
                using System.Text;

                [Serializable]
                public class TestClass {
                    public bool TestMe(int i) 
                    {                        
                    /* leading comment */ } 
                }
                ");

            var body = parsedNode.DescendantNodes().OfType<MethodDeclarationSyntax>().First();

            var lines = body.GetLineCount();

            lines.Should().Be(1);
        }

        [TestMethod]
        public void BlockWithOneLineStatement_ShouldReturn_1()
        {
            var parsedNode = new CSharpSyntaxTreeBuilder().FromSource(@"
                using System;
                using System.Text;

                [Serializable]
                public class TestClass {
                    public bool TestMe(int i) 
                    {                        
                        OneStatement();
                    }
                }
                ");

            var body = parsedNode.DescendantNodes().OfType<MethodDeclarationSyntax>().First();

            var lines = body.GetLineCount();

            lines.Should().Be(1);
        }

        [TestMethod]
        public void BlockWithOneLineStatementAnd2EmptyLines_ShouldReturn_3()
        {
            var parsedNode = new CSharpSyntaxTreeBuilder().FromSource(@"
                using System;
                using System.Text;

                [Serializable]
                public class TestClass {
                    public bool TestMe(int i) 
                    {     
                                           
                        OneStatement();

                    }
                }
                ");

            var body = parsedNode.DescendantNodes().OfType<MethodDeclarationSyntax>().First();

            var lines = body.GetLineCount();

            lines.Should().Be(1);
        }

        [TestMethod]
        public void BlockWithOneLineStatementAnd2CommentLines_ShouldReturn_3()
        {
            var parsedNode = new CSharpSyntaxTreeBuilder().FromSource(@"
                using System;
                using System.Text;

                [Serializable]
                public class TestClass {
                    public bool TestMe(int i) 
                    {     
                        /* comments */                   
                        OneStatement();
                        //Comments
                    }
                }
                ");

            var body = parsedNode.DescendantNodes().OfType<MethodDeclarationSyntax>().First();

            var lines = body.GetLineCount();

            lines.Should().Be(3);
        }
    }
}
