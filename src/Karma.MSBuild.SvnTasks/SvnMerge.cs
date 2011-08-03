using System;
using Karma.MSBuild.SvnTasks.Util;
using Microsoft.Build.Framework;
using SharpSvn;

namespace Karma.MSBuild.SvnTasks
{
    public class SvnMerge: SvnTaskBase
    {
        /// <summary>
        /// URL from where to perform the merge
        /// </summary>
        [Required]
        public string FromRepositoryUrl { get; set; }

        /// <summary>
        /// Indicates Depth of merge, valid values are:
        /// - Children
        /// - Empty
        /// - Exclude
        /// - Files
        /// - Infinity (Default)
        /// - Unknown
        /// </summary>
        public string Depth { get; set; }

        public bool DryRun { get; set; }

        public bool IgnoreAncestry { get; set; }

        public bool Force { get; set; }

        public bool RecordOnly { get; set; }

        /// <summary>
        /// Revisions to merge in the format r1:r2, valid revision values are:
        /// - Revision number
        /// - Type: HEAD, Base, None
        /// </summary>
        [Required]
        public string RevisionRange { get; set; }

        /// <summary>
        /// Actual method to be executed for the implementing task
        /// </summary>
        /// <param name="client">The instance of the SVN client</param>
        /// <returns></returns>
        public override bool ExecuteCommand(SvnClient client)
        {
            SvnUriTarget uriTarget = new SvnUriTarget(FromRepositoryUrl);

            SvnMergeArgs args = new SvnMergeArgs();
            args.Depth = GetDepth();
            args.DryRun = DryRun;
            args.Force = Force;
            args.IgnoreAncestry = IgnoreAncestry;

            SvnRevisionRange revisionRange = GetRevisionRange();

            return client.Merge(RepositoryPath, uriTarget, revisionRange, args);
        }

        private SvnRevisionRange GetRevisionRange()
        {
            if (!RevisionRange.Contains(":"))
            {
                throw new ArgumentException("Revision range must have the format r1:r2");
            }

            string[] revisions = RevisionRange.Split(':');

            if (revisions.Length != 2)
            {
                throw new ArgumentException("Revision range must have the format r1:r2");
            }

            SvnRevisionRange range = new SvnRevisionRange(
                RevisionParser.SafeParse(revisions[0]), 
                RevisionParser.SafeParse(revisions[1]));

            return range;
        }

        private SvnDepth GetDepth()
        {
            SvnDepth depthValue = SvnDepth.Infinity;
            if (!string.IsNullOrEmpty(Depth))
            {
                SvnDepth.TryParse(Depth, true, out depthValue);
            }

            return depthValue;
        }

    }
}