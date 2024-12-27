using System;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006CC RID: 1740
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface ITransformerConfigurationControl
	{
		// Token: 0x1400010A RID: 266
		// (add) Token: 0x0600559E RID: 21918
		// (remove) Token: 0x0600559F RID: 21919
		event EventHandler Cancelled;

		// Token: 0x1400010B RID: 267
		// (add) Token: 0x060055A0 RID: 21920
		// (remove) Token: 0x060055A1 RID: 21921
		event EventHandler Succeeded;
	}
}
