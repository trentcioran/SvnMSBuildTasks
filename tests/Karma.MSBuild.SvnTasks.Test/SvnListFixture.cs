using System.Collections.Generic;
using System.Linq;
using Microsoft.Build.Framework;
using NUnit.Framework;
using Rhino.Mocks;

namespace Karma.MSBuild.SvnTasks.Test
{
    [TestFixture]
    public class SvnListFixture
    {
        [Test]
        public void ListNonRecursive()
        {
            MockRepository repository = new MockRepository();
            IBuildEngine engine = repository.StrictMock<IBuildEngine>();

            SvnList task = new SvnList();
            task.Username = "guest";
            task.RepositoryPath = "http://karma-test-repository.googlecode.com/svn";
            task.BuildEngine = engine;
            bool success = task.Execute();

            Assert.That(success, Is.True);
            Assert.That(task.ItemsList, Is.Not.Null);
            Assert.That(task.ItemsList.Count, Is.EqualTo(5));
            List<string> list = task.ItemsList.ToList();
            Assert.That(list[0], Is.EqualTo("/"));
            Assert.That(list[3], Is.EqualTo("/trunk"));
            Assert.That(list[4], Is.EqualTo("/wiki"));
        }

        [Test]
        public void ListRecursive()
        {
            MockRepository repository = new MockRepository();
            IBuildEngine engine = repository.StrictMock<IBuildEngine>();

            SvnList task = new SvnList();
            task.Username = "guest";
            task.RepositoryPath = "http://karma-test-repository.googlecode.com/svn";
            task.Recursive = true;
            task.BuildEngine = engine;
            bool success = task.Execute();

            Assert.That(success, Is.True);
            Assert.That(task.ItemsList, Is.Not.Null);
            Assert.That(task.ItemsList.Count, Is.EqualTo(7));
            List<string> list = task.ItemsList.ToList();
            Assert.That(list[0], Is.EqualTo("/"));
            Assert.That(list[3], Is.EqualTo("/trunk"));
            Assert.That(list[4], Is.EqualTo("/trunk/DocumentA.txt"));
            Assert.That(list[6], Is.EqualTo("/wiki"));
        }
    }
}