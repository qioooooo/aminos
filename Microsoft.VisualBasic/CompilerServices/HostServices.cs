using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace Microsoft.VisualBasic.CompilerServices
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	[HostProtection(SecurityAction.LinkDemand, Resources = HostProtectionResource.SharedState)]
	public sealed class HostServices
	{
		public static IVbHost VBHost
		{
			get
			{
				return HostServices.m_host;
			}
			set
			{
				HostServices.m_host = value;
			}
		}

		private static IVbHost m_host;
	}
}
