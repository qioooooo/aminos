using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000177 RID: 375
	[StructLayout(LayoutKind.Sequential)]
	internal class MuiResourceTypeIdStringEntry : IDisposable
	{
		// Token: 0x06000766 RID: 1894 RVA: 0x00020B04 File Offset: 0x0001FB04
		~MuiResourceTypeIdStringEntry()
		{
			this.Dispose(false);
		}

		// Token: 0x06000767 RID: 1895 RVA: 0x00020B34 File Offset: 0x0001FB34
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x00020B40 File Offset: 0x0001FB40
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

		// Token: 0x0400067D RID: 1661
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr StringIds;

		// Token: 0x0400067E RID: 1662
		public uint StringIdsSize;

		// Token: 0x0400067F RID: 1663
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr IntegerIds;

		// Token: 0x04000680 RID: 1664
		public uint IntegerIdsSize;
	}
}
