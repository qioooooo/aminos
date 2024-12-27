using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x0200006E RID: 110
	[StructLayout(LayoutKind.Sequential)]
	internal class HashElementEntry : IDisposable
	{
		// Token: 0x06000230 RID: 560 RVA: 0x00007510 File Offset: 0x00006510
		~HashElementEntry()
		{
			this.Dispose(false);
		}

		// Token: 0x06000231 RID: 561 RVA: 0x00007540 File Offset: 0x00006540
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0000754C File Offset: 0x0000654C
		public void Dispose(bool fDisposing)
		{
			if (this.TransformMetadata != IntPtr.Zero)
			{
				Marshal.FreeCoTaskMem(this.TransformMetadata);
				this.TransformMetadata = IntPtr.Zero;
			}
			if (this.DigestValue != IntPtr.Zero)
			{
				Marshal.FreeCoTaskMem(this.DigestValue);
				this.DigestValue = IntPtr.Zero;
			}
			if (fDisposing)
			{
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x04000C0E RID: 3086
		public uint index;

		// Token: 0x04000C0F RID: 3087
		public byte Transform;

		// Token: 0x04000C10 RID: 3088
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr TransformMetadata;

		// Token: 0x04000C11 RID: 3089
		public uint TransformMetadataSize;

		// Token: 0x04000C12 RID: 3090
		public byte DigestMethod;

		// Token: 0x04000C13 RID: 3091
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr DigestValue;

		// Token: 0x04000C14 RID: 3092
		public uint DigestValueSize;

		// Token: 0x04000C15 RID: 3093
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Xml;
	}
}
