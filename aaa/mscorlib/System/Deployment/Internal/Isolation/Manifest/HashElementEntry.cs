using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000193 RID: 403
	[StructLayout(LayoutKind.Sequential)]
	internal class HashElementEntry : IDisposable
	{
		// Token: 0x06001415 RID: 5141 RVA: 0x000363C8 File Offset: 0x000353C8
		~HashElementEntry()
		{
			this.Dispose(false);
		}

		// Token: 0x06001416 RID: 5142 RVA: 0x000363F8 File Offset: 0x000353F8
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06001417 RID: 5143 RVA: 0x00036404 File Offset: 0x00035404
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

		// Token: 0x04000708 RID: 1800
		public uint index;

		// Token: 0x04000709 RID: 1801
		public byte Transform;

		// Token: 0x0400070A RID: 1802
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr TransformMetadata;

		// Token: 0x0400070B RID: 1803
		public uint TransformMetadataSize;

		// Token: 0x0400070C RID: 1804
		public byte DigestMethod;

		// Token: 0x0400070D RID: 1805
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr DigestValue;

		// Token: 0x0400070E RID: 1806
		public uint DigestValueSize;

		// Token: 0x0400070F RID: 1807
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Xml;
	}
}
