using System;
using System.IO;
using Microsoft.Build.Framework;

namespace Karma.MSBuild.SvnTasks.Test
{
    public class SvnFixtureBase
    {
        protected readonly string StringBaseTemplate = "the new contents of the file.";
        protected readonly string ModifyString;
        protected string RepositoryURL = "http://karma-test-repository.googlecode.com/svn/";
        protected string DocumentToModify = "trunk\\DocumentA.txt";

        public SvnFixtureBase()
        {
            ModifyString = string.Format(
                "{0} Ticks {1}", StringBaseTemplate, DateTime.Now.Ticks);
        }

        protected void CheckoutProject(IBuildEngine engine, string path)
        {
            SvnCheckout task = new SvnCheckout();
            task.RepositoryPath = path;
            task.RepositoryUrl = RepositoryURL;
            task.BuildEngine = engine;

            task.Execute();
        }

        protected void CheckoutProjectWithSSL(IBuildEngine engine, string path)
        {
            RepositoryURL = "https://karma-test-repository.googlecode.com/svn/";
            CheckoutProject(engine, path);
        }

        protected void ModifyFiles(string basePath)
        {
            string path = Path.Combine(basePath, DocumentToModify);

            using (Stream stream = File.OpenWrite(path))
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.Write(ModifyString);
            }
        }
    }
}