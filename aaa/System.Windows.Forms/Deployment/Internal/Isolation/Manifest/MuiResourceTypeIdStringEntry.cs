using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000065 RID: 101
	[StructLayout(LayoutKind.Sequential)]
	internal class MuiResourceTypeIdStringEntry : IDisposable
	{
		// Token: 0x0600021B RID: 539 RVA: 0x0000730C File Offset: 0x0000630C
		~MuiResourceTypeIdStringEntry()
		{
			this.Dispose(false);
		}

		// Token: 0x0600021C RID: 540 RVA: 0x0000733C File Offset: 0x0000633C
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x0600021D RID: 541 RVA: 0x00007348 File Offset: 0x00006348
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

		// Token: 0x04000BF3 RID: 3059
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr StringIds;

		// Token: 0x04000BF4 RID: 3060
		public uint StringIdsSize;

		// Token: 0x04000BF5 RID: 3061
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr IntegerIds;

		// Token: 0x04000BF6 RID: 3062
		public uint IntegerIdsSize;
	}
}
