using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Inspector.CodeMetrics.VisualBasic;
using FluentAssertions;
using System.Linq;

namespace InspectionTests.CodeMetricsTests.VisualBasic
{
    [TestClass]
    public class MagicNumberTests : VisualBasicMetricTest
    {
        [TestMethod]
        public void EmptyMethod_ShouldHave_Score0()
        {
            var parsedNode = GetSourceAsSyntaxTree(@"
                Imports System
                Imports System.Text

                <Serializable()> _
                Public Class TestClass
                    Sub New()
                    End Sub

                    Public Function TestMe(i as Integer) As Boolean                       
                        return false
                    End Function
                End Class
                ");

            var sut = new MagicNumber();
            var results = sut.GetMetrics(parsedNode);

            results.Count().Should().Be(1);
            results.First().Method.Should().Be("Function TestMe(i as Integer)");
            results.First().Score.Should().Be(0);
        }

        [TestMethod]
        public void TestWith0Or1_ShouldHave_Score0()
        {
            var parsedNode = GetSourceAsSyntaxTree(@"
                Imports System
                Imports System.Text

                <Serializable()> _
                Public Class TestClass
                    Sub New()
                    End Sub

                    Public Function TestMe(i as Integer) As Boolean                       
                        return i > 0
                    End Function
                End Class
                ");

            var sut = new MagicNumber();
            var results = sut.GetMetrics(parsedNode);

            results.Count().Should().Be(1);
            results.First().Method.Should().Be("Function TestMe(i as Integer)");
            results.First().Score.Should().Be(0);
        }

        [TestMethod]
        public void TestNumber_ShouldHave_Score1()
        {
            var parsedNode = GetSourceAsSyntaxTree(@"
                Imports System
                Imports System.Text

                <Serializable()> _
                Public Class TestClass
                    Sub New()
                    End Sub

                    Public Function TestMe(i as Integer) As Boolean                       
                        return i > 10
                    End Function
                End Class
                ");

            var sut = new MagicNumber();
            var results = sut.GetMetrics(parsedNode);

            results.Count().Should().Be(1);
            results.First().Method.Should().Be("Function TestMe(i as Integer)");
            results.First().Score.Should().Be(1);
        }

        [TestMethod]
        public void VariableDeclarationWithNumber_ShouldNot_RaiseScore()
        {
            var parsedNode = GetSourceAsSyntaxTree(@"
                Imports System
                Imports System.Text

                <Serializable()> _
                Public Class TestClass
                    Sub New()
                    End Sub

                    Public Function TestMe(i as Integer) As Boolean   
                        Dim j as Integer  = 200                    
                        return i > 10
                    End Function
                End Class
                ");

            var sut = new MagicNumber();
            var results = sut.GetMetrics(parsedNode);

            results.Count().Should().Be(1);
            results.First().Method.Should().Be("Function TestMe(i as Integer)");
            results.First().Score.Should().Be(1);
        }

        [TestMethod]
        public void SelectCase_ShouldHave_Score1()
        {
            var parsedNode = GetSourceAsSyntaxTree(@"
                Imports System
                Imports System.Text

                <Serializable()> _
                Public Class TestClass
                    Sub New()
                    End Sub

                    Public Function TestMe(i as Integer) As Boolean   
                        Dim j as Integer  = 200                    
                        Select Case j
                            Case 10
                                Return False
                        End Select
                        Return i > 0
                    End Function
                End Class
                ");

            var sut = new MagicNumber();
            var results = sut.GetMetrics(parsedNode);

            results.Count().Should().Be(1);
            results.First().Method.Should().Be("Function TestMe(i as Integer)");
            results.First().Score.Should().Be(1);
        }
    }
}
