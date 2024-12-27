using System;

namespace System.Web.UI.Design
{
	// Token: 0x0200037F RID: 895
	public interface IProjectItem
	{
		// Token: 0x170005FD RID: 1533
		// (get) Token: 0x06002138 RID: 8504
		string AppRelativeUrl { get; }

		// Token: 0x170005FE RID: 1534
		// (get) Token: 0x06002139 RID: 8505
		string Name { get; }

		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x0600213A RID: 8506
		IProjectItem Parent { get; }

		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x0600213B RID: 8507
		string PhysicalPath { get; }
	}
}
