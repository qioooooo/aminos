using System;
using System.Configuration.Provider;
using System.Security.Permissions;
using System.Threading;

namespace System.Web.Management
{
	// Token: 0x020002C2 RID: 706
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class WebEventProvider : ProviderBase
	{
		// Token: 0x06002466 RID: 9318
		public abstract void ProcessEvent(WebBaseEvent raisedEvent);

		// Token: 0x06002467 RID: 9319
		public abstract void Shutdown();

		// Token: 0x06002468 RID: 9320
		public abstract void Flush();

		// Token: 0x06002469 RID: 9321 RVA: 0x0009B773 File Offset: 0x0009A773
		internal void LogException(Exception e)
		{
			if (Interlocked.CompareExchange(ref this._exceptionLogged, 1, 0) == 0)
			{
				UnsafeNativeMethods.LogWebeventProviderFailure(HttpRuntime.AppDomainAppVirtualPath, this.Name, e.ToString());
			}
		}

		// Token: 0x04001C44 RID: 7236
		private int _exceptionLogged;
	}
}
