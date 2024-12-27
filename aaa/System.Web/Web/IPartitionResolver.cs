using System;
using System.Security.Permissions;

namespace System.Web
{
	// Token: 0x020000B5 RID: 181
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface IPartitionResolver
	{
		// Token: 0x060008A2 RID: 2210
		void Initialize();

		// Token: 0x060008A3 RID: 2211
		string ResolvePartition(object key);
	}
}
