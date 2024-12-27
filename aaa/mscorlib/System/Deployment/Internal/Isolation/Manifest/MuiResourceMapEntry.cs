using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000190 RID: 400
	[StructLayout(LayoutKind.Sequential)]
	internal class MuiResourceMapEntry : IDisposable
	{
		// Token: 0x0600140E RID: 5134 RVA: 0x0003631C File Offset: 0x0003531C
		~MuiResourceMapEntry()
		{
			this.Dispose(false);
		}

		// Token: 0x0600140F RID: 5135 RVA: 0x0003634C File Offset: 0x0003534C
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06001410 RID: 5136 RVA: 0x00036358 File Offset: 0x00035358
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

		// Token: 0x040006FF RID: 1791
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr ResourceTypeIdInt;

		// Token: 0x04000700 RID: 1792
		public uint ResourceTypeIdIntSize;

		// Token: 0x04000701 RID: 1793
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr ResourceTypeIdString;

		// Token: 0x04000702 RID: 1794
		public uint ResourceTypeIdStringSize;
	}
}
