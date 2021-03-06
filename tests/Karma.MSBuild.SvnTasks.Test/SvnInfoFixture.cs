﻿using System;
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

            string path = string.Format(RepositoryPathTemplate, DateTime.Now.Ticks);
            CheckoutProject(engine, path);

            SvnInfo task = new SvnInfo();
            task.Username = "guest";
            task.RepositoryPath = path;
            task.BuildEngine = engine;

            bool success = task.Execute();

            Assert.That(success, Is.True);
            Assert.That(task.Revision, Is.GreaterThan(1));
            Assert.That(task.LastChangedAuthor, Is.EqualTo("karmasvntest@gmail.com"));
            Assert.That(task.LastChangedRevision, Is.GreaterThan(1));
        }

        [Test]
        public void TestSvnInfoWithSSL()
        {
            MockRepository repository = new MockRepository();
            IBuildEngine engine = repository.StrictMock<IBuildEngine>();

            string path = string.Format(RepositoryPathTemplate, DateTime.Now.Ticks);
            CheckoutProjectWithSSL(engine, path);

            SvnInfo task = new SvnInfo();
            task.Username = "guest";
            task.RepositoryPath = path;
            task.BuildEngine = engine;

            bool success = task.Execute();

            Assert.That(success, Is.True);
            Assert.That(task.Revision, Is.GreaterThan(1));
            Assert.That(task.LastChangedAuthor, Is.EqualTo("karmasvntest@gmail.com"));
            Assert.That(task.LastChangedRevision, Is.GreaterThan(1));
        }
    }
}

