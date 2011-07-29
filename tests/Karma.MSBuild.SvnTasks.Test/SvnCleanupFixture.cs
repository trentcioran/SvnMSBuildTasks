using Microsoft.Build.Framework;
using NUnit.Framework;
using Rhino.Mocks;

namespace Karma.MSBuild.SvnTasks.Test
{
    [TestFixture]
    public class SvnCleanupFixture
    {
        [Test]
        public void TestCleanup()
        {
            MockRepository repository = new MockRepository();
            IBuildEngine engine = repository.StrictMock<IBuildEngine>();

            SvnCleanup task = new SvnCleanup();
            task.Username = "guest";
            task.RepositoryPath = "D:\\Proyectos\\log4net";
            task.BuildEngine = engine;

            bool success = task.Execute();

            Assert.That(success, Is.True);
        }
    }
}