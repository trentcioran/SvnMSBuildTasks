using System;
using Microsoft.Build.Framework;
using NUnit.Framework;
using Rhino.Mocks;

namespace Karma.MSBuild.SvnTasks.Test
{
    [TestFixture]
    public class SvnCheckoutFixture
    {
        [Test]
        [ExpectedException(typeof(UriFormatException))]
        public void TestInvalidUrl()
        {
            MockRepository repository = new MockRepository();
            IBuildEngine engine = repository.StrictMock<IBuildEngine>();

            SvnCheckout task = new SvnCheckout();
            task.Username = "guest";
            task.RepositoryPath = string.Format("C:\\tmp\\testrepo\\{0}", DateTime.Now.Ticks);
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
            task.RepositoryPath = string.Format("C:\\tmp\\testrepo\\{0}", DateTime.Now.Ticks);
            task.RepositoryUrl = "http://karma-test-repository.googlecode.com/svn/";
            task.BuildEngine = engine;

            bool success = task.Execute();

            Assert.That(success, Is.True);
            Assert.That(task.CheckedRevision, Is.Not.EqualTo(0));
        }

        [Test]
        public void TestCheckoutSpecificRevision()
        {
            Assert.Fail();
        }

        [Test]
        public void TestCheckoutSpecificDate()
        {
            Assert.Fail();
        }

        [Test]
        public void TestCheckoutDepthInfinity()
        {
            Assert.Fail();
        }

        [Test]
        public void TestCheckoutDepthChildren()
        {
            Assert.Fail();
        }

        [Test]
        public void TestCheckoutDepthEmpty()
        {
            Assert.Fail();
        }

        [Test]
        public void TestCheckoutDepthExclude()
        {
            Assert.Fail();
        }

        [Test]
        public void TestCheckoutDepthFiles()
        {
            Assert.Fail();
        }
    }
}