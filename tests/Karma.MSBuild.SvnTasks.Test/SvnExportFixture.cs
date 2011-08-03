using System;
using Microsoft.Build.Framework;
using NUnit.Framework;
using Rhino.Mocks;

namespace Karma.MSBuild.SvnTasks.Test
{
    [TestFixture]
    public class SvnExportFixture : SvnFixtureBase
    {
        [Test]
        public void TestExportFolder()
        {
            MockRepository repository = new MockRepository();
            IBuildEngine engine = repository.StrictMock<IBuildEngine>();

            string sourcepath = string.Format(RepositoryPathTemplate, DateTime.Now.Ticks);
            string path = string.Format(RepositoryPathTemplate, DateTime.Now.Ticks);

            CheckoutProject(engine, sourcepath);

            SvnExport task = new SvnExport();
            task.BuildEngine = engine;
            task.RepositoryPath = sourcepath;
            task.Username = "guest";
            task.DestinationPath = path;

            bool success = task.Execute();

            Assert.That(success, Is.True);
        }

        [Test]
        public void TestExportURL()
        {
            MockRepository repository = new MockRepository();
            IBuildEngine engine = repository.StrictMock<IBuildEngine>();

            string path = string.Format(RepositoryPathTemplate, DateTime.Now.Ticks);

            SvnExport task = new SvnExport();
            task.BuildEngine = engine;
            task.RepositoryPath = RepositoryURL;
            task.Username = "guest";
            task.DestinationPath = path;

            bool success = task.Execute();

            Assert.That(success, Is.True);
        }

        [Test]
        public void TestExportURLSpecificRevisionNumber()
        {
            MockRepository repository = new MockRepository();
            IBuildEngine engine = repository.StrictMock<IBuildEngine>();

            string path = string.Format(RepositoryPathTemplate, DateTime.Now.Ticks);

            SvnExport task = new SvnExport();
            task.BuildEngine = engine;
            task.RepositoryPath = RepositoryURL;
            task.Username = "guest";
            task.Revision = "1";
            task.DestinationPath = path;

            bool success = task.Execute();

            Assert.That(success, Is.True);
        }
    }
}