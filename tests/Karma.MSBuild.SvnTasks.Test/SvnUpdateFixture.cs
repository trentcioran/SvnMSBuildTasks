using System;
using System.IO;
using Microsoft.Build.Framework;
using NUnit.Framework;
using Rhino.Mocks;

namespace Karma.MSBuild.SvnTasks.Test
{
    [TestFixture]
    public class SvnUpdateFixture
    {
        private void CheckoutProject(IBuildEngine engine, string path)
        {
            SvnCheckout task = new SvnCheckout();
            task.RepositoryPath = path;
            task.RepositoryUrl = "http://karma-test-repository.googlecode.com/svn";
            task.BuildEngine = engine;

            task.Execute();
        }

        private void ModifyLocalRepository(string basePath)
        {
            File.Delete(Path.Combine(basePath, "trunk\\DocumentA.txt"));
            File.Delete(Path.Combine(basePath, "trunk\\DocumentB.txt"));
        }

        private void AssertRepositoryNotUpdated(string basePath)
        {
            Assert.That(File.Exists(Path.Combine(basePath, "trunk\\DocumentA.txt")), Is.False);
            Assert.That(File.Exists(Path.Combine(basePath, "trunk\\DocumentB.txt")), Is.False);
        }

        private void AssertRepositoryUpdated(string basePath)
        {
            Assert.That(File.Exists(Path.Combine(basePath, "trunk\\DocumentA.txt")), Is.True);
            Assert.That(File.Exists(Path.Combine(basePath, "trunk\\DocumentB.txt")), Is.True);
        }

        [Test]
        public void TestUpdateNormal()
        {
            MockRepository repository = new MockRepository();
            IBuildEngine engine = repository.StrictMock<IBuildEngine>();

            string path = string.Format("C:\\tmp\\{0}", DateTime.Now.Ticks);

            CheckoutProject(engine, path);

            ModifyLocalRepository(path);

            SvnUpdate task = new SvnUpdate();
            task.Username = "guest";
            task.RepositoryPath = path;
            task.BuildEngine = engine;

            bool success = task.Execute();

            Assert.That(success, Is.True);
            AssertRepositoryNotUpdated(path);
        }

        [Test]
        public void TestUpdateRecursive()
        {
            MockRepository repository = new MockRepository();
            IBuildEngine engine = repository.StrictMock<IBuildEngine>();

            string path = string.Format("C:\\tmp\\{0}", DateTime.Now.Ticks);

            CheckoutProject(engine, path);

            ModifyLocalRepository(path);

            SvnUpdate task = new SvnUpdate();
            task.Username = "guest";
            task.RepositoryPath = path;
            task.Recursive = true;
            task.BuildEngine = engine;

            bool success = task.Execute();

            Assert.That(success, Is.True);
            AssertRepositoryUpdated(path);
        }
    }
}