using System;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006F3 RID: 1779
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class WebPartTransformer
	{
		// Token: 0x060056FF RID: 22271 RVA: 0x0015E9C0 File Offset: 0x0015D9C0
		public virtual Control CreateConfigurationControl()
		{
			return null;
		}

		// Token: 0x06005700 RID: 22272 RVA: 0x0015E9C3 File Offset: 0x0015D9C3
		protected internal virtual void LoadConfigurationState(object savedState)
		{
		}

		// Token: 0x06005701 RID: 22273 RVA: 0x0015E9C5 File Offset: 0x0015D9C5
		protected internal virtual object SaveConfigurationState()
		{
			return null;
		}

		// Token: 0x06005702 RID: 22274
		public abstract object Transform(object providerData);
	}
}
