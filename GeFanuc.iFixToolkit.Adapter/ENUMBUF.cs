using System;
using System.Runtime.InteropServices;

namespace GeFanuc.iFixToolkit.Adapter
{
	// Token: 0x02000007 RID: 7
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	public struct ENUMBUF
	{
		// Token: 0x04000024 RID: 36
		public short api_code;

		// Token: 0x04000025 RID: 37
		public short lookup_done;

		// Token: 0x04000026 RID: 38
		public VSP vsp;

		// Token: 0x04000027 RID: 39
		public int pdbsn;
	}
}
