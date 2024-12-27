using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006FF RID: 1791
	[ToolboxItem(false)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class UnauthorizedWebPart : ProxyWebPart
	{
		// Token: 0x0600576F RID: 22383 RVA: 0x00160EEE File Offset: 0x0015FEEE
		public UnauthorizedWebPart(WebPart webPart)
			: base(webPart)
		{
		}

		// Token: 0x06005770 RID: 22384 RVA: 0x00160EF7 File Offset: 0x0015FEF7
		public UnauthorizedWebPart(string originalID, string originalTypeName, string originalPath, string genericWebPartID)
			: base(originalID, originalTypeName, originalPath, genericWebPartID)
		{
		}
	}
}
