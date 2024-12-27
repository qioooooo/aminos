using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x0200009C RID: 156
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal sealed class LdapVlvInfo
	{
		// Token: 0x06000326 RID: 806 RVA: 0x0000F614 File Offset: 0x0000E614
		public LdapVlvInfo(int version, int before, int after, int offset, int count, IntPtr attribute, IntPtr context)
		{
			this.version = version;
			this.beforeCount = before;
			this.afterCount = after;
			this.offset = offset;
			this.count = count;
			this.attrvalue = attribute;
			this.context = context;
		}

		// Token: 0x040002FD RID: 765
		private int version = 1;

		// Token: 0x040002FE RID: 766
		private int beforeCount;

		// Token: 0x040002FF RID: 767
		private int afterCount;

		// Token: 0x04000300 RID: 768
		private int offset;

		// Token: 0x04000301 RID: 769
		private int count;

		// Token: 0x04000302 RID: 770
		private IntPtr attrvalue = (IntPtr)0;

		// Token: 0x04000303 RID: 771
		private IntPtr context = (IntPtr)0;

		// Token: 0x04000304 RID: 772
		private IntPtr extraData = (IntPtr)0;
	}
}
