using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x0200017A RID: 378
	[StructLayout(LayoutKind.Sequential)]
	internal class MuiResourceTypeIdIntEntry : IDisposable
	{
		// Token: 0x0600076D RID: 1901 RVA: 0x00020BB0 File Offset: 0x0001FBB0
		~MuiResourceTypeIdIntEntry()
		{
			this.Dispose(false);
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x00020BE0 File Offset: 0x0001FBE0
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x00020BEC File Offset: 0x0001FBEC
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

		// Token: 0x04000686 RID: 1670
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr StringIds;

		// Token: 0x04000687 RID: 1671
		public uint StringIdsSize;

		// Token: 0x04000688 RID: 1672
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr IntegerIds;

		// Token: 0x04000689 RID: 1673
		public uint IntegerIdsSize;
	}
}
