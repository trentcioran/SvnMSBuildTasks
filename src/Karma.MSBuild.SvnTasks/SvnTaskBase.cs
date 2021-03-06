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
        #region Inputs

        [Required]
        public string Username { get; set; }

        [Required]
        public string RepositoryPath { get; set; }

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

            client.Authentication.AddSubversionFileHandlers();

            client.Authentication.SslServerTrustHandlers += SslServerTrustHandlers;
            client.Authentication.SslClientCertificateHandlers += SslClientCertificateHandlers;

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
            e.CertificateFile = "D:\\Proyectos\\personal\\SvnMSBuildTasks\\certificate.cer";
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