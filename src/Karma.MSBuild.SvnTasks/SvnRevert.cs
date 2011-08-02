using SharpSvn;

namespace Karma.MSBuild.SvnTasks
{
    public class SvnRevert : SvnTaskBase
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
            SvnRevertArgs args = new SvnRevertArgs();
            args.Depth = Recursive ? SvnDepth.Infinity : SvnDepth.Children;

            return client.Revert(RepositoryPath, args);
        }
    }
}