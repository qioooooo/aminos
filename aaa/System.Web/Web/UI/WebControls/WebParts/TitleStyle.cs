using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006FD RID: 1789
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class TitleStyle : TableItemStyle
	{
		// Token: 0x06005762 RID: 22370 RVA: 0x00160CCA File Offset: 0x0015FCCA
		public TitleStyle()
		{
			this.Wrap = false;
		}

		// Token: 0x1700168A RID: 5770
		// (get) Token: 0x06005763 RID: 22371 RVA: 0x00160CD9 File Offset: 0x0015FCD9
		// (set) Token: 0x06005764 RID: 22372 RVA: 0x00160CE1 File Offset: 0x0015FCE1
		[DefaultValue(false)]
		public override bool Wrap
		{
			get
			{
				return base.Wrap;
			}
			set
			{
				base.Wrap = value;
			}
		}
	}
}
