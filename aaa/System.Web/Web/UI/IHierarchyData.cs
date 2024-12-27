using System;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x020000CC RID: 204
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface IHierarchyData
	{
		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06000911 RID: 2321
		bool HasChildren { get; }

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06000912 RID: 2322
		string Path { get; }

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06000913 RID: 2323
		object Item { get; }

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06000914 RID: 2324
		string Type { get; }

		// Token: 0x06000915 RID: 2325
		IHierarchicalEnumerable GetChildren();

		// Token: 0x06000916 RID: 2326
		IHierarchyData GetParent();
	}
}
