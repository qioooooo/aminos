using System;
using System.Diagnostics;
using System.Security.Permissions;
using System.Web;

namespace Microsoft.VisualBasic.Logging
{
	[HostProtection(SecurityAction.LinkDemand, Resources = HostProtectionResource.ExternalProcessMgmt)]
	public class AspLog : Log
	{
		public AspLog()
		{
		}

		public AspLog(string name)
			: base(name)
		{
		}

		protected internal override void InitializeWithDefaultsSinceNoConfigExists()
		{
			this.TraceSource.Listeners.Add(new WebPageTraceListener());
			this.TraceSource.Switch.Level = SourceLevels.Information;
		}
	}
}
