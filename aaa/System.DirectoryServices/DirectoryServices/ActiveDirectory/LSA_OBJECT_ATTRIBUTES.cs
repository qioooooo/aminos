using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x0200011F RID: 287
	[StructLayout(LayoutKind.Sequential)]
	internal sealed class LSA_OBJECT_ATTRIBUTES
	{
		// Token: 0x06000779 RID: 1913 RVA: 0x00027738 File Offset: 0x00026738
		public LSA_OBJECT_ATTRIBUTES()
		{
			this.Length = 0;
			this.RootDirectory = (IntPtr)0;
			this.ObjectName = (IntPtr)0;
			this.Attributes = 0;
			this.SecurityDescriptor = (IntPtr)0;
			this.SecurityQualityOfService = (IntPtr)0;
		}

		// Token: 0x040006BA RID: 1722
		internal int Length;

		// Token: 0x040006BB RID: 1723
		private IntPtr RootDirectory;

		// Token: 0x040006BC RID: 1724
		private IntPtr ObjectName;

		// Token: 0x040006BD RID: 1725
		internal int Attributes;

		// Token: 0x040006BE RID: 1726
		private IntPtr SecurityDescriptor;

		// Token: 0x040006BF RID: 1727
		private IntPtr SecurityQualityOfService;
	}
}
