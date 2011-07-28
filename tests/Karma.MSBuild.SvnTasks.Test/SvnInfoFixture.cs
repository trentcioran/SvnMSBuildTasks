using System;
using Microsoft.Build.Framework;
using NUnit.Framework;
using Rhino.Mocks;

namespace Karma.MSBuild.SvnTasks.Test
{
    [TestFixture]
    public class SvnInfoFixture
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestMissingParameters()
        {
            MockRepository repository = new MockRepository();
            IBuildEngine engine = repository.StrictMock<IBuildEngine>();

            SvnInfo task = new SvnInfo();
            task.BuildEngine = engine;

            task.Execute();
        }

        // TODO: Fix tis test, can't be dependent on local repository
        [Test]
        public void TestSvnInfo()
        {
            MockRepository repository = new MockRepository();
            IBuildEngine engine = repository.StrictMock<IBuildEngine>();

            SvnInfo task = new SvnInfo();
            task.Username = "guest";
            task.RepositoryPath = "C:\\Proyectos\\log4net";
            task.BuildEngine = engine;

            bool success = task.Execute();

            Assert.That(success, Is.True);
            Assert.That(task.Revision, Is.EqualTo(1146579));
            Assert.That(task.LastChangedAuthor, Is.EqualTo("rgrabowski"));
            Assert.That(task.LastChangedRevision, Is.EqualTo(1021982));
        }
    }
}
