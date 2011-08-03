using System;
using System.IO;
using System.Text;
using Microsoft.Build.Framework;
using NUnit.Framework;
using Rhino.Mocks;

namespace Karma.MSBuild.SvnTasks.Test
{
    [TestFixture]
    public class SvnRevertFixture : SvnFixtureBase
    {

        private void AssertFileChangesReverted(string basePath)
        {
            string path = Path.Combine(basePath, "trunk\\DocumentA.txt");

            string contents = File.ReadAllText(path);
            Assert.That(contents, Is.StringContaining(StringBaseTemplate));
        }

        private void AssertFileChangesNotReverted(string basePath)
        {
            string path = Path.Combine(basePath, "trunk\\DocumentA.txt");

            string contents = File.ReadAllText(path);
            Assert.That(contents, Is.StringContaining("the new contents of the file"));
        }

        [Test]
        public void TestRevert()
        {
            MockRepository repository = new MockRepository();
            IBuildEngine engine = repository.StrictMock<IBuildEngine>();

            string path = string.Format(RepositoryPathTemplate, DateTime.Now.Ticks);

            CheckoutProject(engine, path);

            ModifyFiles(path);

            SvnRevert task = new SvnRevert();
            task.Username = "guest";
            task.RepositoryPath = path;
            task.Recursive = true;
            task.BuildEngine = engine;

            bool success = task.Execute();

            Assert.That(success, Is.True);
            AssertFileChangesReverted(path);
        }

        [Test]
        public void TestRevertWithSSL()
        {
            MockRepository repository = new MockRepository();
            IBuildEngine engine = repository.StrictMock<IBuildEngine>();

            string path = string.Format(RepositoryPathTemplate, DateTime.Now.Ticks);

            CheckoutProjectWithSSL(engine, path);

            ModifyFiles(path);

            SvnRevert task = new SvnRevert();
            task.Username = "guest";
            task.RepositoryPath = path;
            task.Recursive = true;
            task.BuildEngine = engine;

            bool success = task.Execute();

            Assert.That(success, Is.True);
            AssertFileChangesReverted(path);
        }

        [Test]
        public void TestRevertNotRecursive()
        {
            MockRepository repository = new MockRepository();
            IBuildEngine engine = repository.StrictMock<IBuildEngine>();

            string path = string.Format(RepositoryPathTemplate, DateTime.Now.Ticks);

            CheckoutProject(engine, path);

            ModifyFiles(path);

            SvnRevert task = new SvnRevert();
            task.Username = "guest";
            task.RepositoryPath = path;
            task.BuildEngine = engine;

            bool success = task.Execute();

            Assert.That(success, Is.True);
            AssertFileChangesNotReverted(path);
        }

        [Test]
        public void TestRevertNotRecursiveWithSSL()
        {
            MockRepository repository = new MockRepository();
            IBuildEngine engine = repository.StrictMock<IBuildEngine>();

            string path = string.Format(RepositoryPathTemplate, DateTime.Now.Ticks);

            CheckoutProjectWithSSL(engine, path);

            ModifyFiles(path);

            SvnRevert task = new SvnRevert();
            task.Username = "guest";
            task.RepositoryPath = path;
            task.BuildEngine = engine;

            bool success = task.Execute();

            Assert.That(success, Is.True);
            AssertFileChangesNotReverted(path);
        }
    }
}