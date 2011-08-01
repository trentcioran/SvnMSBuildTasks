using System;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using SharpSvn;
using SharpSvn.Security;

namespace Karma.MSBuild.SvnTasks
{
    /// <summary>
    /// Base class to use when working with SVN tasks, it handles inputs 
    /// for user, password, repository path.
    /// </summary>
    public abstract class SvnTaskBase : Task
    {
        private string _repositoryPath;
        private bool _isSsl;

        #region Inputs

        [Required]
        public string Username { get; set; }

        [Required]
        public string RepositoryPath
        {
            get { return _repositoryPath; }
            set
            {
                _repositoryPath = value;
                if (_repositoryPath != null && _repositoryPath.ToLower().StartsWith("https"))
                {
                    _isSsl = true;
                }
            }
        }

        public string Password { get; set; }

        #endregion
        
        /// <summary>
        /// Sets credentials for credentials, path and passes the control to the 
        /// implementing task.
        /// </summary>
        /// <returns></returns>
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
            
            if (_isSsl)
            {
                client.Authentication.AddConsoleHandlers();

                client.Authentication.SslServerTrustHandlers += SslServerTrustHandlers;
                client.Authentication.SslClientCertificateHandlers += SslClientCertificateHandlers;
            }

            try
            {
                return ExecuteCommand(client);
            }
            finally
            {
                client.Dispose();
            }
        }

        void SslClientCertificateHandlers(object sender, SvnSslClientCertificateEventArgs e)
        {
            e.Save = true;
            throw new NotImplementedException();
        }

        void SslServerTrustHandlers(object sender, SvnSslServerTrustEventArgs e)
        {
            e.AcceptedFailures = e.Failures;
            e.Save = true;
        }

        /// <summary>
        /// Actual method to be executed for the implementing task
        /// </summary>
        /// <param name="client">The instance of the SVN client</param>
        /// <returns></returns>
        public abstract bool ExecuteCommand(SvnClient client);
    }
}