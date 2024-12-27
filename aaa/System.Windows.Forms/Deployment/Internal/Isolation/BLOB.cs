using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020000C5 RID: 197
	internal struct BLOB : IDisposable
	{
		// Token: 0x06000315 RID: 789 RVA: 0x0000797C File Offset: 0x0000697C
		public void Dispose()
		{
			if (this.BlobData != IntPtr.Zero)
			{
				Marshal.FreeCoTaskMem(this.BlobData);
				this.BlobData = IntPtr.Zero;
			}
		}

		// Token: 0x04000D5D RID: 3421
		[MarshalAs(UnmanagedType.U4)]
		public uint Size;

		// Token: 0x04000D5E RID: 3422
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr BlobData;
	}
}
