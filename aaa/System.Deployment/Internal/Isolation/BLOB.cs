using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020000F0 RID: 240
	internal struct BLOB : IDisposable
	{
		// Token: 0x060005FF RID: 1535 RVA: 0x0001F034 File Offset: 0x0001E034
		public void Dispose()
		{
			if (this.BlobData != IntPtr.Zero)
			{
				Marshal.FreeCoTaskMem(this.BlobData);
				this.BlobData = IntPtr.Zero;
			}
		}

		// Token: 0x040004D1 RID: 1233
		[MarshalAs(UnmanagedType.U4)]
		public uint Size;

		// Token: 0x040004D2 RID: 1234
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr BlobData;
	}
}
