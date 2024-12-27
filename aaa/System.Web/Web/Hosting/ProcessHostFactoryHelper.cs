using System;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web.Hosting
{
	// Token: 0x020002BD RID: 701
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class ProcessHostFactoryHelper : MarshalByRefObject, IProcessHostFactoryHelper
	{
		// Token: 0x06002430 RID: 9264 RVA: 0x0009B220 File Offset: 0x0009A220
		[AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
		public ProcessHostFactoryHelper()
		{
		}

		// Token: 0x06002431 RID: 9265 RVA: 0x0009B228 File Offset: 0x0009A228
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public override object InitializeLifetimeService()
		{
			return null;
		}

		// Token: 0x06002432 RID: 9266 RVA: 0x0009B22C File Offset: 0x0009A22C
		public object GetProcessHost(IProcessHostSupportFunctions functions)
		{
			object processHost;
			try
			{
				processHost = ProcessHost.GetProcessHost(functions);
			}
			catch (Exception ex)
			{
				Misc.ReportUnhandledException(ex, new string[] { SR.GetString("Cant_Create_Process_Host") });
				throw;
			}
			return processHost;
		}
	}
}
