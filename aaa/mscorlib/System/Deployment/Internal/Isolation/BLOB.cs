using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020001D0 RID: 464
	internal struct BLOB : IDisposable
	{
		// Token: 0x060014C0 RID: 5312 RVA: 0x00036A08 File Offset: 0x00035A08
		public void Dispose()
		{
			if (this.BlobData != IntPtr.Zero)
			{
				Marshal.FreeCoTaskMem(this.BlobData);
				this.BlobData = IntPtr.Zero;
			}
		}

		// Token: 0x040007F3 RID: 2035
		[MarshalAs(UnmanagedType.U4)]
		public uint Size;

		// Token: 0x040007F4 RID: 2036
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr BlobData;
	}
}
