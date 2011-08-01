using System;
using System.IO;
using Microsoft.Build.Framework;
using NUnit.Framework;
using Rhino.Mocks;

namespace Karma.MSBuild.SvnTasks.Test
{
    [TestFixture]
    public class SvnCommitFixture : SvnFixtureBase
    {
        [Test]
        public void SvnCommit()
        {
            MockRepository repository = new MockRepository();
            IBuildEngine engine = repository.StrictMock<IBuildEngine>();

            string path = string.Format(RepositoryPathTemplate, DateTime.Now.Ticks);
            CheckoutProject(engine, path);
            ModifyFiles(path);

            SvnCommit task = new SvnCommit();
            task.RepositoryPath = path;
            task.Message = "test modify documentA.txt";
            task.Username = "karmasvntest@gmail.com";
            task.Password = "AW7Tw3nV3zJ2";
            task.BuildEngine = engine;

            bool success = task.Execute();

            Assert.That(success, Is.True);
            AssertFilesSubmitted(engine);
        }

        [Test]
        public void SvnCommitWithConflicts()
        {
            Assert.Fail();
        }

        private void AssertFilesSubmitted(IBuildEngine engine)
        {
            string path = string.Format(RepositoryPathTemplate, DateTime.Now.Ticks);
            CheckoutProject(engine, path);

            string filePath = Path.Combine(path, "trunk\\DocumentA.txt");

            string contents = File.ReadAllText(filePath);
            Assert.That(contents, Is.EqualTo(ModifyString));
        }
    }
}