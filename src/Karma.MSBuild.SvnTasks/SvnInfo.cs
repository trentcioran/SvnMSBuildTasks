using System;
using System.Collections.ObjectModel;
using Microsoft.Build.Framework;
using SharpSvn;

namespace Karma.MSBuild.SvnTasks
{
    public class SvnInfo : SvnTaskBase
    {
        #region Outputs

        [Output]
        public long Revision { get; private set; }

        [Output]
        public string NodeKind { get; private set; }

        [Output]
        public string Schedule { get; private set; }

        [Output]
        public Guid RepositoryId { get; private set; }

        [Output]
        public string LastChangedAuthor { get; private set; }

        [Output]
        public long LastChangedRevision { get; private set; }

        [Output]
        public DateTime LastChangeDate { get; private set; } 

        #endregion

        /// <summary>
        /// Actual method to be executed for the implementing task
        /// </summary>
        /// <param name="client">The instance of the SVN client</param>
        /// <returns></returns>
        public override bool ExecuteCommand(SvnClient client)
        {
            SvnTarget target = new SvnPathTarget(RepositoryPath);
            SvnInfoArgs args = new SvnInfoArgs();
            Collection<SvnInfoEventArgs> infoResult = new Collection<SvnInfoEventArgs>();

            bool result = client.GetInfo(target, args, out infoResult);

            SvnInfoEventArgs info = infoResult[0];

            Revision = info.Revision;
            RepositoryId = info.RepositoryId;
            NodeKind = info.NodeKind.ToString();
            Schedule = info.Schedule.ToString();
            LastChangedRevision = info.LastChangeRevision;
            LastChangedAuthor = info.LastChangeAuthor;
            LastChangeDate = info.LastChangeTime;

            return result;
        }

    }
}
