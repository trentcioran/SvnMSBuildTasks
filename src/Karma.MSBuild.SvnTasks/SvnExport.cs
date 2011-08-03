using System;
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
            SvnRevision revision = GetRevision();
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

        private SvnRevision GetRevision()
        {
            Log.LogMessage(MessageImportance.Low, "Revision to checkout: {0}", Revision);
            // defaults to HEAD
            SvnRevision revision = new SvnRevision(SvnRevisionType.Head);
            // check revision number
            long revNumber;
            if (long.TryParse(Revision, out revNumber))
            {
                revision = new SvnRevision(revNumber);
            }
            // check revision date
            DateTime revDate;
            if (DateTime.TryParse(Revision, out revDate))
            {
                revision = new SvnRevision(revDate);
            }
            // check for revision type
            SvnRevisionType revisionType;
            if (SvnRevisionType.TryParse(Revision, true, out revisionType))
            {
                revision = new SvnRevision(revisionType);
            }

            return revision;
        }

    }
}