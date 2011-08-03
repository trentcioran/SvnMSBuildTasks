using System;
using Microsoft.Build.Framework;
using NUnit.Framework;
using Rhino.Mocks;

namespace Karma.MSBuild.SvnTasks.Test
{
    [TestFixture]
    public class SvnCheckoutFixture: SvnFixtureBase
    {
        [Test]
        [ExpectedException(typeof(UriFormatException))]
        public void TestInvalidUrl()
        {
            MockRepository repository = new MockRepository();
            IBuildEngine engine = repository.StrictMock<IBuildEngine>();

            SvnCheckout task = new SvnCheckout();
            task.Username = "guest";
            task.RepositoryPath = string.Format(RepositoryPathTemplate, DateTime.Now.Ticks);
            task.RepositoryUrl = "someurl";
            task.BuildEngine = engine;

            task.Execute();
        }

        [Test]
        public void TestCheckoutHead()
        {
            MockRepository repository = new MockRepository();
            IBuildEngine engine = repository.StrictMock<IBuildEngine>();

            SvnCheckout task = new SvnCheckout();
            task.Username = "guest";
            task.RepositoryPath = string.Format(RepositoryPathTemplate, DateTime.Now.Ticks);
            task.RepositoryUrl = "http://karma-test-repository.googlecode.com/svn/";
            task.BuildEngine = engine;

            bool success = task.Execute();

            Assert.That(success, Is.True);
            Assert.That(task.CheckedRevision, Is.Not.EqualTo(0));
        }

        [Test]
        public void TestCheckoutHeadWithSSL()
        {
            MockRepository repository = new MockRepository();
            IBuildEngine engine = repository.StrictMock<IBuildEngine>();

            SvnCheckout task = new SvnCheckout();
            task.Username = "guest";
            task.RepositoryPath = string.Format(RepositoryPathTemplate, DateTime.Now.Ticks);
            task.RepositoryUrl = "https://karma-test-repository.googlecode.com/svn/";
            task.BuildEngine = engine;

            bool success = task.Execute();

            Assert.That(success, Is.True);
            Assert.That(task.CheckedRevision, Is.Not.EqualTo(0));
        }

        [Ignore]
        [Test]
        public void TestCheckoutSpecificRevision()
        {
            Assert.Fail();
        }

        [Ignore]
        [Test]
        public void TestCheckoutSpecificDate()
        {
            Assert.Fail();
        }

        [Ignore]
        [Test]
        public void TestCheckoutDepthInfinity()
        {
            Assert.Fail();
        }

        [Ignore]
        [Test]
        public void TestCheckoutDepthChildren()
        {
            Assert.Fail();
        }

        [Ignore]
        [Test]
        public void TestCheckoutDepthEmpty()
        {
            Assert.Fail();
        }

        [Ignore]
        [Test]
        public void TestCheckoutDepthExclude()
        {
            Assert.Fail();
        }

        [Ignore]
        [Test]
        public void TestCheckoutDepthFiles()
        {
            Assert.Fail();
        }
    }
}