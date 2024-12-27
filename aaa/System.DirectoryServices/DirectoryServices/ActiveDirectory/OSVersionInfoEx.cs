using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000BD RID: 189
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal sealed class OSVersionInfoEx
	{
		// Token: 0x060005F0 RID: 1520 RVA: 0x00022618 File Offset: 0x00021618
		public OSVersionInfoEx()
		{
			this.osVersionInfoSize = Marshal.SizeOf(this);
		}

		// Token: 0x040004D4 RID: 1236
		public int osVersionInfoSize;

		// Token: 0x040004D5 RID: 1237
		public int majorVersion;

		// Token: 0x040004D6 RID: 1238
		public int minorVersion;

		// Token: 0x040004D7 RID: 1239
		public int buildNumber;

		// Token: 0x040004D8 RID: 1240
		public int platformId;

		// Token: 0x040004D9 RID: 1241
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string csdVersion;

		// Token: 0x040004DA RID: 1242
		public short servicePackMajor;

		// Token: 0x040004DB RID: 1243
		public short servicePackMinor;

		// Token: 0x040004DC RID: 1244
		public short suiteMask;

		// Token: 0x040004DD RID: 1245
		public byte productType;

		// Token: 0x040004DE RID: 1246
		public byte reserved;
	}
}
