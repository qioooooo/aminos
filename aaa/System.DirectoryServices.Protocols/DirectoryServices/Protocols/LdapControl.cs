using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000097 RID: 151
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal sealed class LdapControl
	{
		// Token: 0x040002EF RID: 751
		public IntPtr ldctl_oid = (IntPtr)0;

		// Token: 0x040002F0 RID: 752
		public berval ldctl_value;

		// Token: 0x040002F1 RID: 753
		public bool ldctl_iscritical;
	}
}
