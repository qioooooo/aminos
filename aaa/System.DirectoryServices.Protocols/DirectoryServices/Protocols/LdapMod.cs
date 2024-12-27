using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x0200009B RID: 155
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal sealed class LdapMod
	{
		// Token: 0x06000325 RID: 805 RVA: 0x0000F5B0 File Offset: 0x0000E5B0
		~LdapMod()
		{
			if (this.attribute != (IntPtr)0)
			{
				Marshal.FreeHGlobal(this.attribute);
			}
			if (this.values != (IntPtr)0)
			{
				Marshal.FreeHGlobal(this.values);
			}
		}

		// Token: 0x040002FA RID: 762
		public int type;

		// Token: 0x040002FB RID: 763
		public IntPtr attribute = (IntPtr)0;

		// Token: 0x040002FC RID: 764
		public IntPtr values = (IntPtr)0;
	}
}
