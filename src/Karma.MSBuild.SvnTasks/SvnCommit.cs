using SharpSvn;

namespace Karma.MSBuild.SvnTasks
{
    public class SvnCommit: SvnTaskBase
    {
        /// <summary>
        /// Indicates if current locks should be kept
        /// </summary>
        public bool KeepLocks { get; set; }

        /// <summary>
        /// Indicates if current locks should be kept
        /// </summary>
        public bool KeepChangeLists { get; set; }

        /// <summary>
        /// Sets or gets the commit message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Actual method to be executed for the implementing task
        /// </summary>
        /// <param name="client">The instance of the SVN client</param>
        /// <returns></returns>
        public override bool ExecuteCommand(SvnClient client)
        {
            SvnCommitArgs args = new SvnCommitArgs();
            args.LogMessage = Message;
            args.KeepLocks = KeepLocks;
            args.KeepChangeLists = KeepChangeLists;

            SvnCommitResult result;

            bool success = client.Commit(RepositoryPath, args, out result);

            return success;
        }
    }
}