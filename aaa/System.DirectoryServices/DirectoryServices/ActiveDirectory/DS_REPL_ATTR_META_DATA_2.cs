using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x0200010F RID: 271
	[StructLayout(LayoutKind.Sequential)]
	internal sealed class DS_REPL_ATTR_META_DATA_2
	{
		// Token: 0x04000676 RID: 1654
		public IntPtr pszAttributeName;

		// Token: 0x04000677 RID: 1655
		public int dwVersion;

		// Token: 0x04000678 RID: 1656
		public int ftimeLastOriginatingChange1;

		// Token: 0x04000679 RID: 1657
		public int ftimeLastOriginatingChange2;

		// Token: 0x0400067A RID: 1658
		public Guid uuidLastOriginatingDsaInvocationID;

		// Token: 0x0400067B RID: 1659
		public long usnOriginatingChange;

		// Token: 0x0400067C RID: 1660
		public long usnLocalChange;

		// Token: 0x0400067D RID: 1661
		public IntPtr pszLastOriginatingDsaDN;
	}
}
