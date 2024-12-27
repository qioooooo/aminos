using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006D1 RID: 1745
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface IWebPartRow
	{
		// Token: 0x17001618 RID: 5656
		// (get) Token: 0x060055B8 RID: 21944
		PropertyDescriptorCollection Schema { get; }

		// Token: 0x060055B9 RID: 21945
		void GetRowData(RowCallback callback);
	}
}
