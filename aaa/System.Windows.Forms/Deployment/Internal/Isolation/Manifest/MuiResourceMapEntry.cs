using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x0200006B RID: 107
	[StructLayout(LayoutKind.Sequential)]
	internal class MuiResourceMapEntry : IDisposable
	{
		// Token: 0x06000229 RID: 553 RVA: 0x00007464 File Offset: 0x00006464
		~MuiResourceMapEntry()
		{
			this.Dispose(false);
		}

		// Token: 0x0600022A RID: 554 RVA: 0x00007494 File Offset: 0x00006494
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x0600022B RID: 555 RVA: 0x000074A0 File Offset: 0x000064A0
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

		// Token: 0x04000C05 RID: 3077
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr ResourceTypeIdInt;

		// Token: 0x04000C06 RID: 3078
		public uint ResourceTypeIdIntSize;

		// Token: 0x04000C07 RID: 3079
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr ResourceTypeIdString;

		// Token: 0x04000C08 RID: 3080
		public uint ResourceTypeIdStringSize;
	}
}
