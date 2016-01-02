using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Inspector;
using FluentAssertions;

namespace InspectionTests.ComponentTests
{
    [TestClass]
    public class ProjectTests
    {
        [TestMethod]
        public void CreatingSourceFile_Should_AddFileToProject()
        {
            Project project = CreateProject();
            SourceFile file = CreateSourceFile(project);

            project.SourceFiles.Should().Contain(file);
        }


        [TestMethod]
        public void CreatingSourceFile_Should_HaveConnectionWithProject()
        {
            Project project = CreateProject();
            SourceFile file = CreateSourceFile(project);

            file.Project.Should().Be(project);
        }

        private static Project CreateProject()
        {
            return new Project(Guid.Empty.ToString(), "TestApp.Domain", "somefile.csproj");
        }

        private static SourceFile CreateSourceFile(Project project)
        {
            return new SourceFile(project, "app.cs", "");
        }
    }
}
