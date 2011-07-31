using System;
using System.Collections.ObjectModel;
using Microsoft.Build.Framework;
using SharpSvn;

namespace Karma.MSBuild.SvnTasks
{
    public class SvnUpdate : SvnTaskBase
    {
        /// <summary>
        /// Indicates if only current directory or recursive across all directories
        /// </summary>
        public bool Recursive { get; set; }

        /// <summary>
        /// Actual method to be executed for the implementing task
        /// </summary>
        /// <param name="client">The instance of the SVN client</param>
        /// <returns></returns>
        public override bool ExecuteCommand(SvnClient client)
        {
            SvnUpdateArgs args = new SvnUpdateArgs();
            args.Depth = Recursive ? SvnDepth.Infinity : SvnDepth.Children;

            SvnUpdateResult updateResult;

            bool success = client.Update(RepositoryPath, args, out updateResult);

            if (updateResult.HasResultMap)
            {
                foreach (string key in updateResult.ResultMap.Keys)
                {
                    SvnUpdateResult result = updateResult.ResultMap[key];
                    Log.LogMessage(MessageImportance.Normal, "[{0} - {1}]", key, result.Revision);
                }
            }

            return success;
        }
    }
}