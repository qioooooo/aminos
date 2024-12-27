using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x0200018A RID: 394
	[StructLayout(LayoutKind.Sequential)]
	internal class MuiResourceTypeIdStringEntry : IDisposable
	{
		// Token: 0x06001400 RID: 5120 RVA: 0x000361C4 File Offset: 0x000351C4
		~MuiResourceTypeIdStringEntry()
		{
			this.Dispose(false);
		}

		// Token: 0x06001401 RID: 5121 RVA: 0x000361F4 File Offset: 0x000351F4
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06001402 RID: 5122 RVA: 0x00036200 File Offset: 0x00035200
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

		// Token: 0x040006ED RID: 1773
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr StringIds;

		// Token: 0x040006EE RID: 1774
		public uint StringIdsSize;

		// Token: 0x040006EF RID: 1775
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr IntegerIds;

		// Token: 0x040006F0 RID: 1776
		public uint IntegerIdsSize;
	}
}
