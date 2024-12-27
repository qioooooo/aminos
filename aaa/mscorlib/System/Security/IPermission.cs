using System;
using System.Runtime.InteropServices;

namespace System.Security
{
	// Token: 0x0200060E RID: 1550
	[ComVisible(true)]
	public interface IPermission : ISecurityEncodable
	{
		// Token: 0x06003850 RID: 14416
		IPermission Copy();

		// Token: 0x06003851 RID: 14417
		IPermission Intersect(IPermission target);

		// Token: 0x06003852 RID: 14418
		IPermission Union(IPermission target);

		// Token: 0x06003853 RID: 14419
		bool IsSubsetOf(IPermission target);

		// Token: 0x06003854 RID: 14420
		void Demand();
	}
}
