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
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidUrl()
        {
            MockRepository repository = new MockRepository();
            IBuildEngine engine = repository.StrictMock<IBuildEngine>();

            SvnCheckout task = new SvnCheckout();
            task.Username = "guest";
            task.RepositoryPath = string.Format("D:\\tmp\\log4net\\{0}", DateTime.Now.Ticks);
            task.RepositoryUrl = "http://svn.apache.org/repos/asf/logging/log4net/trunk";
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
            task.RepositoryPath = string.Format("D:\\tmp\\log4net\\{0}", DateTime.Now.Ticks);
            task.RepositoryUrl = "http://svn.apache.org/repos/asf/logging/log4net/trunk";
            task.BuildEngine = engine;

            bool success = task.Execute();

            Assert.That(success, Is.True);
        }

        [Test]
        public void TestCheckoutSpecificRevision()
        {
            
        }

        [Test]
        public void TestCheckoutSpecificDate()
        {
            
        }
    }
}