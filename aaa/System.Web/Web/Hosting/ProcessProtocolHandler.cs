using System;
using System.Security.Permissions;

namespace System.Web.Hosting
{
	// Token: 0x020002BE RID: 702
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class ProcessProtocolHandler : MarshalByRefObject
	{
		// Token: 0x06002434 RID: 9268 RVA: 0x0009B27C File Offset: 0x0009A27C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public override object InitializeLifetimeService()
		{
			return null;
		}

		// Token: 0x06002435 RID: 9269
		public abstract void StartListenerChannel(IListenerChannelCallback listenerChannelCallback, IAdphManager AdphManager);

		// Token: 0x06002436 RID: 9270
		public abstract void StopListenerChannel(int listenerChannelId, bool immediate);

		// Token: 0x06002437 RID: 9271
		public abstract void StopProtocol(bool immediate);
	}
}
