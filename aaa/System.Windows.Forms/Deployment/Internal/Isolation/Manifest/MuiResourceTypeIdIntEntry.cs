using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000068 RID: 104
	[StructLayout(LayoutKind.Sequential)]
	internal class MuiResourceTypeIdIntEntry : IDisposable
	{
		// Token: 0x06000222 RID: 546 RVA: 0x000073B8 File Offset: 0x000063B8
		~MuiResourceTypeIdIntEntry()
		{
			this.Dispose(false);
		}

		// Token: 0x06000223 RID: 547 RVA: 0x000073E8 File Offset: 0x000063E8
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06000224 RID: 548 RVA: 0x000073F4 File Offset: 0x000063F4
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

		// Token: 0x04000BFC RID: 3068
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr StringIds;

		// Token: 0x04000BFD RID: 3069
		public uint StringIdsSize;

		// Token: 0x04000BFE RID: 3070
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr IntegerIds;

		// Token: 0x04000BFF RID: 3071
		public uint IntegerIdsSize;
	}
}
