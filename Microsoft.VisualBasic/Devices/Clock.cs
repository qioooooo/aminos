using System;
using System.Security.Permissions;

namespace Microsoft.VisualBasic.Devices
{
	[HostProtection(SecurityAction.LinkDemand, Resources = HostProtectionResource.ExternalProcessMgmt)]
	public class Clock
	{
		public DateTime LocalTime
		{
			get
			{
				return DateTime.Now;
			}
		}

		public DateTime GmtTime
		{
			get
			{
				return DateTime.UtcNow;
			}
		}

		public int TickCount
		{
			get
			{
				return Environment.TickCount;
			}
		}
	}
}
