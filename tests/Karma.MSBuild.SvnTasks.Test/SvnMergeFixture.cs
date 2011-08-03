using System;
using System.IO;
using Microsoft.Build.Framework;
using NUnit.Framework;
using Rhino.Mocks;

namespace Karma.MSBuild.SvnTasks.Test
{
    [TestFixture]
    public class SvnMergeFixture : SvnFixtureBase
    {
        private void AssertFilesUpdated(string path)
        {
            string contentsAOnBranch = File.ReadAllText(Path.Combine(path, "branches\\testbranch\\DocumentA.txt"));
            string contentsAOntrunk = File.ReadAllText(Path.Combine(path, "trunk\\DocumentA.txt"));

            Assert.That(contentsAOnBranch, Is.EqualTo(contentsAOntrunk));

            string contentsBOnBranch = File.ReadAllText(Path.Combine(path, "branches\\testbranch\\DocumentB.txt"));
            string contentsBOntrunk = File.ReadAllText(Path.Combine(path, "trunk\\DocumentB.txt"));

            Assert.That(contentsBOnBranch, Is.EqualTo(contentsBOntrunk));
        }

        [Test]
        public void TestMergeRevisionNumbers()
        {
            MockRepository repository = new MockRepository();
            IBuildEngine engine = repository.StrictMock<IBuildEngine>();

            string path = string.Format(RepositoryPathTemplate, DateTime.Now.Ticks);
            CheckoutProjectWithSSL(engine, path);

            SvnMerge task = new SvnMerge();
            task.BuildEngine = engine;
            task.RepositoryPath = path + "\\trunk";
            task.FromRepositoryUrl = "https://karma-test-repository.googlecode.com/svn/branches/testbranch";
            task.RevisionRange = "1:9";

            bool success = task.Execute();
            Assert.That(success, Is.True);

            AssertFilesUpdated(path);
        }

        [Test]
        public void TestMergeRevisionToHead()
        {
            MockRepository repository = new MockRepository();
            IBuildEngine engine = repository.StrictMock<IBuildEngine>();

            string path = string.Format(RepositoryPathTemplate, DateTime.Now.Ticks);
            CheckoutProjectWithSSL(engine, path);

            SvnMerge task = new SvnMerge();
            task.BuildEngine = engine;
            task.RepositoryPath = path + "\\trunk";
            task.FromRepositoryUrl = "https://karma-test-repository.googlecode.com/svn/branches/testbranch";
            task.RevisionRange = "1:HEAD";

            bool success = task.Execute();
            Assert.That(success, Is.True);

            AssertFilesUpdated(path);
        }

    }
}