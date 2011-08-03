using System;
using Karma.MSBuild.SvnTasks.Util;
using Microsoft.Build.Framework;
using SharpSvn;

namespace Karma.MSBuild.SvnTasks
{
    public class SvnExport: SvnTaskBase
    {
        /// <summary>
        /// The path to export the repository
        /// </summary>
        [Required]
        public string DestinationPath { get; set; }

        /// <summary>
        /// The revision to take for export
        /// </summary>
        public string Revision { get; set; }

        /// <summary>
        /// Actual method to be executed for the implementing task
        /// </summary>
        /// <param name="client">The instance of the SVN client</param>
        /// <returns></returns>
        public override bool ExecuteCommand(SvnClient client)
        {
            SvnTarget target = GetTarget();
            return client.Export(target, DestinationPath);
        }

        private SvnTarget GetTarget()
        {
            SvnTarget target;
            SvnRevision revision = RevisionParser.SafeParse(Revision);
            if (Uri.IsWellFormedUriString(RepositoryPath, UriKind.Absolute))
            {
                target = new SvnUriTarget(RepositoryPath, revision);
            }
            else
            {
                target = new SvnPathTarget(RepositoryPath, revision);
            }
            
            return target;
        }
    }
}