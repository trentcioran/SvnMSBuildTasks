using SharpSvn;

namespace Karma.MSBuild.SvnTasks
{
    /// <summary>
    /// 
    /// </summary>
    public class SvnCleanup : SvnTaskBase
    {
        /// <summary>
        /// Actual method to be executed for the implementing task
        /// </summary>
        /// <param name="client">The instance of the SVN client</param>
        /// <returns></returns>
        public override bool ExecuteCommand(SvnClient client)
        {
            return client.CleanUp(RepositoryPath);
        }
    }
}