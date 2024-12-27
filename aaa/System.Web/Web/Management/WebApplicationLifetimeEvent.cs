using System;
using System.Security.Permissions;

namespace System.Web.Management
{
	// Token: 0x020002E9 RID: 745
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class WebApplicationLifetimeEvent : WebManagementEvent
	{
		// Token: 0x0600257C RID: 9596 RVA: 0x000A120B File Offset: 0x000A020B
		protected internal WebApplicationLifetimeEvent(string message, object eventSource, int eventCode)
			: base(message, eventSource, eventCode)
		{
		}

		// Token: 0x0600257D RID: 9597 RVA: 0x000A1216 File Offset: 0x000A0216
		protected internal WebApplicationLifetimeEvent(string message, object eventSource, int eventCode, int eventDetailCode)
			: base(message, eventSource, eventCode, eventDetailCode)
		{
		}

		// Token: 0x0600257E RID: 9598 RVA: 0x000A1223 File Offset: 0x000A0223
		internal WebApplicationLifetimeEvent()
		{
		}

		// Token: 0x0600257F RID: 9599 RVA: 0x000A122C File Offset: 0x000A022C
		internal static int DetailCodeFromShutdownReason(ApplicationShutdownReason reason)
		{
			switch (reason)
			{
			case ApplicationShutdownReason.HostingEnvironment:
				return 50002;
			case ApplicationShutdownReason.ChangeInGlobalAsax:
				return 50003;
			case ApplicationShutdownReason.ConfigurationChange:
				return 50004;
			case ApplicationShutdownReason.UnloadAppDomainCalled:
				return 50005;
			case ApplicationShutdownReason.ChangeInSecurityPolicyFile:
				return 50006;
			case ApplicationShutdownReason.BinDirChangeOrDirectoryRename:
				return 50007;
			case ApplicationShutdownReason.BrowsersDirChangeOrDirectoryRename:
				return 50008;
			case ApplicationShutdownReason.CodeDirChangeOrDirectoryRename:
				return 50009;
			case ApplicationShutdownReason.ResourcesDirChangeOrDirectoryRename:
				return 50010;
			case ApplicationShutdownReason.IdleTimeout:
				return 50011;
			case ApplicationShutdownReason.PhysicalApplicationPathChanged:
				return 50012;
			case ApplicationShutdownReason.HttpRuntimeClose:
				return 50013;
			case ApplicationShutdownReason.InitializationError:
				return 50014;
			case ApplicationShutdownReason.MaxRecompilationsReached:
				return 50015;
			case ApplicationShutdownReason.BuildManagerChange:
				return 50017;
			default:
				return 50001;
			}
		}

		// Token: 0x06002580 RID: 9600 RVA: 0x000A12E0 File Offset: 0x000A02E0
		protected internal override void IncrementPerfCounters()
		{
			base.IncrementPerfCounters();
			PerfCounters.IncrementCounter(AppPerfCounter.EVENTS_APP);
		}
	}
}
