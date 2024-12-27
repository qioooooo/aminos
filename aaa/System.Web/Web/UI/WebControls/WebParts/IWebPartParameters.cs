using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006D0 RID: 1744
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface IWebPartParameters
	{
		// Token: 0x17001617 RID: 5655
		// (get) Token: 0x060055B5 RID: 21941
		PropertyDescriptorCollection Schema { get; }

		// Token: 0x060055B6 RID: 21942
		void GetParametersData(ParametersCallback callback);

		// Token: 0x060055B7 RID: 21943
		void SetConsumerSchema(PropertyDescriptorCollection schema);
	}
}
