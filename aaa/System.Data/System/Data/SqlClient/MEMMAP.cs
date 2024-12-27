using System;
using System.Runtime.InteropServices;

namespace System.Data.SqlClient
{
	// Token: 0x020002CF RID: 719
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct MEMMAP
	{
		// Token: 0x0400177A RID: 6010
		[MarshalAs(UnmanagedType.U4)]
		internal uint dbgpid;

		// Token: 0x0400177B RID: 6011
		[MarshalAs(UnmanagedType.U4)]
		internal uint fOption;

		// Token: 0x0400177C RID: 6012
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		internal byte[] rgbMachineName;

		// Token: 0x0400177D RID: 6013
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		internal byte[] rgbDllName;

		// Token: 0x0400177E RID: 6014
		[MarshalAs(UnmanagedType.U4)]
		internal uint cbData;

		// Token: 0x0400177F RID: 6015
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 255)]
		internal byte[] rgbData;
	}
}
