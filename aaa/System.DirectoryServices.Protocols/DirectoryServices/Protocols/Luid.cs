using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x0200008F RID: 143
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal class Luid
	{
		// Token: 0x0600031B RID: 795 RVA: 0x0000F4E7 File Offset: 0x0000E4E7
		internal Luid()
		{
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x0600031C RID: 796 RVA: 0x0000F4EF File Offset: 0x0000E4EF
		public int LowPart
		{
			get
			{
				return this.lowPart;
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x0600031D RID: 797 RVA: 0x0000F4F7 File Offset: 0x0000E4F7
		public int HighPart
		{
			get
			{
				return this.highPart;
			}
		}

		// Token: 0x040002A5 RID: 677
		internal int lowPart;

		// Token: 0x040002A6 RID: 678
		internal int highPart;
	}
}
