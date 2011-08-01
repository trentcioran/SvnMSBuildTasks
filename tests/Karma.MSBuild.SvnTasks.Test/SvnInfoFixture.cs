using System;
using Microsoft.Build.Framework;
using NUnit.Framework;
using Rhino.Mocks;

namespace Karma.MSBuild.SvnTasks.Test
{
    [TestFixture]
    public class SvnInfoFixture : SvnFixtureBase
    {
        [Test]
        public void TestSvnInfo()
        {
            MockRepository repository = new MockRepository();
            IBuildEngine engine = repository.StrictMock<IBuildEngine>();

            string path = string.Format("C:\\tmp\\{0}", DateTime.Now.Ticks);
            CheckoutProject(engine, path);

            SvnInfo task = new SvnInfo();
            task.Username = "guest";
            task.RepositoryPath = path;
            task.BuildEngine = engine;

            bool success = task.Execute();

            Assert.That(success, Is.True);
            Assert.That(task.Revision, Is.EqualTo(2));
            Assert.That(task.LastChangedAuthor, Is.EqualTo("trentcioran@gmail.com"));
            Assert.That(task.LastChangedRevision, Is.EqualTo(2));
        }

        [Test]
        public void TestSvnInfoWithSSL()
        {
            MockRepository repository = new MockRepository();
            IBuildEngine engine = repository.StrictMock<IBuildEngine>();

            string path = string.Format("C:\\tmp\\{0}", DateTime.Now.Ticks);
            CheckoutProjectWithSSL(engine, path);

            SvnInfo task = new SvnInfo();
            task.Username = "guest";
            task.RepositoryPath = path;
            task.BuildEngine = engine;

            bool success = task.Execute();

            Assert.That(success, Is.True);
            Assert.That(task.Revision, Is.EqualTo(2));
            Assert.That(task.LastChangedAuthor, Is.EqualTo("trentcioran@gmail.com"));
            Assert.That(task.LastChangedRevision, Is.EqualTo(2));
        }
    }
}

