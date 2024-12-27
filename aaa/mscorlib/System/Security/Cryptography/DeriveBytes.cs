using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000864 RID: 2148
	[ComVisible(true)]
	public abstract class DeriveBytes
	{
		// Token: 0x06004ED4 RID: 20180
		public abstract byte[] GetBytes(int cb);

		// Token: 0x06004ED5 RID: 20181
		public abstract void Reset();
	}
}
