using System.Collections.ObjectModel;
using Microsoft.Build.Framework;
using SharpSvn;

namespace Karma.MSBuild.SvnTasks
{
    public class SvnList: SvnTaskBase
    {

        /// <summary>
        /// Indicates if only current directory or recursive across all directories
        /// </summary>
        public bool Recursive { get; set; }

        /// <summary>
        /// Items listed, Uri relative to the repository provided
        /// </summary>
        [Output]
        public Collection<string> ItemsList { get; private set; }

        public SvnList()
        {
            ItemsList = new Collection<string>();
        }

        /// <summary>
        /// Actual method to be executed for the implementing task
        /// </summary>
        /// <param name="client">The instance of the SVN client</param>
        /// <returns></returns>
        public override bool ExecuteCommand(SvnClient client)
        {
            SvnTarget target = new SvnUriTarget(RepositoryPath);
            SvnListArgs args = new SvnListArgs();
            args.Depth = Recursive ? SvnDepth.Infinity : SvnDepth.Children;

            return client.List(target, args, listHandler);
        }

        private void listHandler(object sender, SvnListEventArgs e)
        {
            Log.LogMessage(MessageImportance.Normal, "{0} - {1}", 
                e.BasePath, e.Path);
            ItemsList.Add(string.Format("{0}{1}", e.BasePath, e.Path));
        }
    }
}