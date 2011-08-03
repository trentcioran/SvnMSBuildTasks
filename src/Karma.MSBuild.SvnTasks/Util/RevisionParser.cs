using System;
using SharpSvn;

namespace Karma.MSBuild.SvnTasks.Util
{
    public class RevisionParser
    {
        public static SvnRevision SafeParse(string expression)
        {
            SvnRevision revision = new SvnRevision(SvnRevisionType.Head);
            // check revision number
            long revNumber;
            if (long.TryParse(expression, out revNumber))
            {
                revision = new SvnRevision(revNumber);
            }
            else
            {
                // check revision date
                DateTime revDate;
                if (DateTime.TryParse(expression, out revDate))
                {
                    revision = new SvnRevision(revDate);
                }
                else
                {
                    // check for revision type
                    SvnRevisionType revisionType;
                    if (SvnRevisionType.TryParse(expression, true, out revisionType))
                    {
                        revision = new SvnRevision(revisionType);
                    }
                }
            }

            return revision;
        }
    }
}
