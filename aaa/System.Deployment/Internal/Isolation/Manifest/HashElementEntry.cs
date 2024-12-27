using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000180 RID: 384
	[StructLayout(LayoutKind.Sequential)]
	internal class HashElementEntry : IDisposable
	{
		// Token: 0x0600077B RID: 1915 RVA: 0x00020D08 File Offset: 0x0001FD08
		~HashElementEntry()
		{
			this.Dispose(false);
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x00020D38 File Offset: 0x0001FD38
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x00020D44 File Offset: 0x0001FD44
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

		// Token: 0x04000698 RID: 1688
		public uint index;

		// Token: 0x04000699 RID: 1689
		public byte Transform;

		// Token: 0x0400069A RID: 1690
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr TransformMetadata;

		// Token: 0x0400069B RID: 1691
		public uint TransformMetadataSize;

		// Token: 0x0400069C RID: 1692
		public byte DigestMethod;

		// Token: 0x0400069D RID: 1693
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr DigestValue;

		// Token: 0x0400069E RID: 1694
		public uint DigestValueSize;

		// Token: 0x0400069F RID: 1695
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Xml;
	}
}
