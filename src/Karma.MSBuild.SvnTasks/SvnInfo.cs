using System;
using System.Collections.ObjectModel;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using SharpSvn;
using SharpSvn.Security;

namespace Karma.MSBuild.SvnTasks
{
    public class SvnInfo : Task
    {
        #region Inputs
        
        [Required]
        public string Username { get; set; }

        public string Password { get; set; }

        [Required]
        public string RepositoryPath { get; set; }

        #endregion

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

        public override bool Execute()
        {
            SvnClient client = new SvnClient();
            client.Authentication.Clear();
            if (string.IsNullOrEmpty(Password))
            {
                client.Authentication.UserNameHandlers +=
                    delegate(object o, SvnUserNameEventArgs eventArgs)
                        {
                            eventArgs.UserName = Username;
                        };
            }
            else
            {
                client.Authentication.UserNamePasswordHandlers +=
                    delegate(object o, SvnUserNamePasswordEventArgs eventArgs)
                        {
                            eventArgs.UserName = Username;
                            eventArgs.Password = Password;
                        };
            }

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
