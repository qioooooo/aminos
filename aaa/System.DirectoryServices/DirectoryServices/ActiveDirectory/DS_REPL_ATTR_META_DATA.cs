using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x02000111 RID: 273
	[StructLayout(LayoutKind.Sequential)]
	internal sealed class DS_REPL_ATTR_META_DATA
	{
		// Token: 0x04000680 RID: 1664
		public IntPtr pszAttributeName;

		// Token: 0x04000681 RID: 1665
		public int dwVersion;

		// Token: 0x04000682 RID: 1666
		public int ftimeLastOriginatingChange1;

		// Token: 0x04000683 RID: 1667
		public int ftimeLastOriginatingChange2;

		// Token: 0x04000684 RID: 1668
		public Guid uuidLastOriginatingDsaInvocationID;

		// Token: 0x04000685 RID: 1669
		public long usnOriginatingChange;

		// Token: 0x04000686 RID: 1670
		public long usnLocalChange;
	}
}
