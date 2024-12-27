using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006D2 RID: 1746
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface IWebPartTable
	{
		// Token: 0x17001619 RID: 5657
		// (get) Token: 0x060055BA RID: 21946
		PropertyDescriptorCollection Schema { get; }

		// Token: 0x060055BB RID: 21947
		void GetTableData(TableCallback callback);
	}
}
