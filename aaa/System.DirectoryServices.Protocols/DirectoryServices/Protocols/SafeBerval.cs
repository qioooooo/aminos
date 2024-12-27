using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000096 RID: 150
	[StructLayout(LayoutKind.Sequential)]
	internal sealed class SafeBerval
	{
		// Token: 0x06000322 RID: 802 RVA: 0x0000F538 File Offset: 0x0000E538
		~SafeBerval()
		{
			if (this.bv_val != (IntPtr)0)
			{
				Marshal.FreeHGlobal(this.bv_val);
			}
		}

		// Token: 0x040002ED RID: 749
		public int bv_len;

		// Token: 0x040002EE RID: 750
		public IntPtr bv_val = (IntPtr)0;
	}
}
