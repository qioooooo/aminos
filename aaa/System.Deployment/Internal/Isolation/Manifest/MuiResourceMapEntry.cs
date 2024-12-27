using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x0200017D RID: 381
	[StructLayout(LayoutKind.Sequential)]
	internal class MuiResourceMapEntry : IDisposable
	{
		// Token: 0x06000774 RID: 1908 RVA: 0x00020C5C File Offset: 0x0001FC5C
		~MuiResourceMapEntry()
		{
			this.Dispose(false);
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x00020C8C File Offset: 0x0001FC8C
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x00020C98 File Offset: 0x0001FC98
		public void Dispose(bool fDisposing)
		{
			if (this.ResourceTypeIdInt != IntPtr.Zero)
			{
				Marshal.FreeCoTaskMem(this.ResourceTypeIdInt);
				this.ResourceTypeIdInt = IntPtr.Zero;
			}
			if (this.ResourceTypeIdString != IntPtr.Zero)
			{
				Marshal.FreeCoTaskMem(this.ResourceTypeIdString);
				this.ResourceTypeIdString = IntPtr.Zero;
			}
			if (fDisposing)
			{
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x0400068F RID: 1679
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr ResourceTypeIdInt;

		// Token: 0x04000690 RID: 1680
		public uint ResourceTypeIdIntSize;

		// Token: 0x04000691 RID: 1681
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr ResourceTypeIdString;

		// Token: 0x04000692 RID: 1682
		public uint ResourceTypeIdStringSize;
	}
}
