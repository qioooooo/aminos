using System;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020004D5 RID: 1237
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface IButtonControl
	{
		// Token: 0x17000D9D RID: 3485
		// (get) Token: 0x06003B62 RID: 15202
		// (set) Token: 0x06003B63 RID: 15203
		bool CausesValidation { get; set; }

		// Token: 0x17000D9E RID: 3486
		// (get) Token: 0x06003B64 RID: 15204
		// (set) Token: 0x06003B65 RID: 15205
		string CommandArgument { get; set; }

		// Token: 0x17000D9F RID: 3487
		// (get) Token: 0x06003B66 RID: 15206
		// (set) Token: 0x06003B67 RID: 15207
		string CommandName { get; set; }

		// Token: 0x1400006A RID: 106
		// (add) Token: 0x06003B68 RID: 15208
		// (remove) Token: 0x06003B69 RID: 15209
		event EventHandler Click;

		// Token: 0x1400006B RID: 107
		// (add) Token: 0x06003B6A RID: 15210
		// (remove) Token: 0x06003B6B RID: 15211
		event CommandEventHandler Command;

		// Token: 0x17000DA0 RID: 3488
		// (get) Token: 0x06003B6C RID: 15212
		// (set) Token: 0x06003B6D RID: 15213
		string PostBackUrl { get; set; }

		// Token: 0x17000DA1 RID: 3489
		// (get) Token: 0x06003B6E RID: 15214
		// (set) Token: 0x06003B6F RID: 15215
		string Text { get; set; }

		// Token: 0x17000DA2 RID: 3490
		// (get) Token: 0x06003B70 RID: 15216
		// (set) Token: 0x06003B71 RID: 15217
		string ValidationGroup { get; set; }
	}
}
