using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x0200018D RID: 397
	[StructLayout(LayoutKind.Sequential)]
	internal class MuiResourceTypeIdIntEntry : IDisposable
	{
		// Token: 0x06001407 RID: 5127 RVA: 0x00036270 File Offset: 0x00035270
		~MuiResourceTypeIdIntEntry()
		{
			this.Dispose(false);
		}

		// Token: 0x06001408 RID: 5128 RVA: 0x000362A0 File Offset: 0x000352A0
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06001409 RID: 5129 RVA: 0x000362AC File Offset: 0x000352AC
		public void Dispose(bool fDisposing)
		{
			if (this.StringIds != IntPtr.Zero)
			{
				Marshal.FreeCoTaskMem(this.StringIds);
				this.StringIds = IntPtr.Zero;
			}
			if (this.IntegerIds != IntPtr.Zero)
			{
				Marshal.FreeCoTaskMem(this.IntegerIds);
				this.IntegerIds = IntPtr.Zero;
			}
			if (fDisposing)
			{
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x040006F6 RID: 1782
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr StringIds;

		// Token: 0x040006F7 RID: 1783
		public uint StringIdsSize;

		// Token: 0x040006F8 RID: 1784
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr IntegerIds;

		// Token: 0x040006F9 RID: 1785
		public uint IntegerIdsSize;
	}
}
