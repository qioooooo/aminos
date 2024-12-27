using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000098 RID: 152
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal struct LdapReferralCallback
	{
		// Token: 0x040002F2 RID: 754
		public int sizeofcallback;

		// Token: 0x040002F3 RID: 755
		public QUERYFORCONNECTIONInternal query;

		// Token: 0x040002F4 RID: 756
		public NOTIFYOFNEWCONNECTIONInternal notify;

		// Token: 0x040002F5 RID: 757
		public DEREFERENCECONNECTIONInternal dereference;
	}
}
