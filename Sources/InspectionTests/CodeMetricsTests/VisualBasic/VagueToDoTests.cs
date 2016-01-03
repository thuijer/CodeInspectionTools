using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Inspector.CodeMetrics.VisualBasic;

namespace InspectionTests.CodeMetricsTests.VisualBasic
{
    [TestClass]
    public class VagueToDoTests : VisualBasicMetricTest
    {
        [TestMethod]
        public void EmptyMethod_ShouldHave_Score0()
        {
            var parsedNode = GetSourceAsSyntaxTree(@"
                Imports System
                Imports System.Text

                <Serializable>_
                Public Class TestClass
                    Sub New()
                    End Sub

                    Public Function TestMe(i as Integer) As Boolean
                        return false
                    End Function
                End Class
                ");

            var sut = new VagueToDo();
            var results = sut.GetMetrics(parsedNode);

            results.Count().Should().Be(1);
            results.First().Method.Should().Be("Function TestMe(i as Integer)");
            results.First().Score.Should().Be(0);
        }

        [TestMethod]
        public void MethodWithOneTodo_ShouldHave_Score1()
        {
            var parsedNode = GetSourceAsSyntaxTree(@"
                Imports System
                Imports System.Text

                <Serializable>_
                Public Class TestClass
                    Sub New()
                    End Sub

                    Public Function TestMe(i as Integer) As Boolean
                        'TODO: Visual Studio style
                        return false
                    End Function
                End Class
                ");

            var sut = new VagueToDo();
            var results = sut.GetMetrics(parsedNode);

            results.Count().Should().Be(1);
            results.First().Method.Should().Be("Function TestMe(i as Integer)");
            results.First().Score.Should().Be(1);
        }

        [TestMethod]
        public void MethodWith2TodoComments_ShouldHave_Score2()
        {
            var parsedNode = GetSourceAsSyntaxTree(@"
                Imports System
                Imports System.Text

                <Serializable>_
                Public Class TestClass
                    Sub New()
                    End Sub

                    Public Function TestMe(i as Integer) As Boolean
                        'TODO: Visual Studio style
                        return false
                        'more todo later
                    End Function
                End Class
                ");

            var sut = new VagueToDo();
            var results = sut.GetMetrics(parsedNode);

            results.Count().Should().Be(1);
            results.First().Method.Should().Be("Function TestMe(i as Integer)");
            results.First().Score.Should().Be(2);
        }

        [TestMethod]
        public void With5DifferentTodoComments_ShouldReturn_Score5()
        {
            var parsedNode = GetSourceAsSyntaxTree(@"
                Imports System
                Imports System.Text

                <Serializable>_
                Public Class TestClass
                    Sub New()
                    End Sub

                    Public Function TestMe(i as Integer) As Boolean
                        'TODO: test this            
                        ' todo something else
                        'to do
                        'Find something else todo   
                        ' test TODO
                        '

                        ' some other comment                        
                        return false
                    End Function
                End Class
                ");

            var sut = new VagueToDo();
            var results = sut.GetMetrics(parsedNode);

            results.Count().Should().Be(1);
            results.First().Method.Should().Be("Function TestMe(i as Integer)");
            results.First().Score.Should().Be(5);
        }
    }
}
