using System;
using System.Collections.Generic;
using Karma.MSBuild.SvnTasks.Util;
using Microsoft.Build.Framework;
using SharpSvn;

namespace Karma.MSBuild.SvnTasks
{
    public class SvnCheckout : SvnTaskBase
    {

        #region Inputs
        
        /// <summary>
        /// URL of repository to checkout, should be a valiud URI.
        /// </summary>
        [Required]
        public string RepositoryUrl { get; set; }

        /// <summary>
        /// Indicates Depth of checkout, valid values are:
        /// - Children
        /// - Empty
        /// - Exclude
        /// - Files
        /// - Infinity (Default)
        /// - Unknown
        /// </summary>
        public string Depth { get; set; }

        /// <summary>
        /// Indicated if include or ignore configured externals
        /// </summary>
        public bool IgnoreExternals { get; set; }

        /// <summary>
        /// Revision to checkout, valid values are:
        /// - Revision number
        /// - Revision Date
        /// - Type: HEAD, Base, None
        /// - No value, defaults to HEAD.
        /// </summary>
        public string Revision { get; set; }

        #endregion
        
        #region Outputs

        [Output]
        public long CheckedRevision { get; private set; }

        #endregion

        /// <summary>
        /// Actual method to be executed for the implementing task
        /// </summary>
        /// <param name="client">The instance of the SVN client</param>
        /// <returns></returns>
        public override bool ExecuteCommand(SvnClient client)
        {
            SvnUriTarget uriTarget = new SvnUriTarget(RepositoryUrl);
            SvnCheckOutArgs checkOutArgs = new SvnCheckOutArgs();
            checkOutArgs.Depth = GetDepth();
            checkOutArgs.IgnoreExternals = IgnoreExternals;
            checkOutArgs.Revision = RevisionParser.SafeParse(Revision);

            SvnUpdateResult result;

            bool success = client.CheckOut(uriTarget, RepositoryPath, checkOutArgs, out result);

            if (result.HasRevision)
            {
                CheckedRevision = result.Revision; 
                Log.LogMessage(MessageImportance.Normal, "Checked revision: {0}", CheckedRevision);
            }
            if (result.HasResultMap)
            {
                ReadResults(result);
            }

            return success;
        }

        private void ReadResults(SvnUpdateResult result)
        {
            IDictionary<string, SvnUpdateResult> results = result.ResultMap;
            foreach (string key in results.Keys)
            {
                SvnUpdateResult updateResult = results[key];
                Log.LogMessage(MessageImportance.Normal, "[{0} - {1}]", key, updateResult.Revision);
            }
        }

        private SvnDepth GetDepth()
        {
            SvnDepth depthValue = SvnDepth.Infinity;
            if (!string.IsNullOrEmpty(Depth))
            {
                SvnDepth.TryParse(Depth, true, out depthValue);
            }

            Log.LogMessage(MessageImportance.Low, "Depth of checkout: {0}", depthValue);
            return depthValue;
        }
    }
}