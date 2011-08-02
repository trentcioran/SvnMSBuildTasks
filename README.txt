SVN MSBuild Tasks

SVN MSBUild Tasks takes a different approach than tigris version using SharpSVN which is more flexible and robust than the current tigris implementation.

Commands currently supported:
- info
- cleanup
- list
- checkout (limmited support)
- revert
- update
- commit

In progress
- export

Roadmap:
- merge

Working with HTTPS:

Karma.MSBuild.SvnTasks works using SvnAuthentication.AddSubversionFileHandlers mechanism, in case of trust problems check the following link:

http://googlecode.blogspot.com/2008/06/ssl-certificate-renewal-for-project.html
