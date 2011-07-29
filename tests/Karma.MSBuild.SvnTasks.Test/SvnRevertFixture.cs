using System;
using System.IO;
using System.Text;
using Microsoft.Build.Framework;
using NUnit.Framework;
using Rhino.Mocks;

namespace Karma.MSBuild.SvnTasks.Test
{
    [TestFixture]
    public class SvnRevertFixture
    {
        private void CheckoutProject(IBuildEngine engine, string path)
        {
            SvnCheckout task = new SvnCheckout();
            task.RepositoryPath = path;
            task.RepositoryUrl = "http://karma-test-repository.googlecode.com/svn";
            task.BuildEngine = engine;

            task.Execute();
        }

        private void ModifyFile(string basePath)
        {
            string path = Path.Combine(basePath, "trunk\\DocumentA.txt");

            using (Stream stream = File.OpenWrite(path))
            using(StreamWriter writer = new StreamWriter(stream))
            {
                writer.Write("the new contents of the file");
            }
        }

        [Test]
        public void TestRevert()
        {
            MockRepository repository = new MockRepository();
            IBuildEngine engine = repository.StrictMock<IBuildEngine>();

            string path = string.Format("C:\\tmp\\{0}", DateTime.Now.Ticks);

            CheckoutProject(engine, path);

            ModifyFile(path);

            SvnRevert task = new SvnRevert();
            task.Username = "guest";
            task.RepositoryPath = path;
            task.BuildEngine = engine;

            bool success = task.Execute();

            Assert.That(success, Is.True);
            AssertFileChangesReverted(path);
        }

        private void AssertFileChangesReverted(string basePath)
        {
            string path = Path.Combine(basePath, "trunk\\DocumentA.txt");
            
            string contents = File.ReadAllText(path);
            Assert.That(contents, Is.EqualTo("document A"));
        }
    }
}