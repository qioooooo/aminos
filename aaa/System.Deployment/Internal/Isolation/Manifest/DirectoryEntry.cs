using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001CB RID: 459
	[StructLayout(LayoutKind.Sequential)]
	internal class DirectoryEntry : IDisposable
	{
		// Token: 0x06000842 RID: 2114 RVA: 0x000210E8 File Offset: 0x000200E8
		~DirectoryEntry()
		{
			this.Dispose(false);
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x00021118 File Offset: 0x00020118
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x00021121 File Offset: 0x00020121
		public void Dispose(bool fDisposing)
		{
			if (this.SecurityDescriptor != IntPtr.Zero)
			{
				Marshal.FreeCoTaskMem(this.SecurityDescriptor);
				this.SecurityDescriptor = IntPtr.Zero;
			}
			if (fDisposing)
			{
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x040007BD RID: 1981
		public uint Flags;

		// Token: 0x040007BE RID: 1982
		public uint Protection;

		// Token: 0x040007BF RID: 1983
		[MarshalAs(UnmanagedType.LPWStr)]
		public string BuildFilter;

		// Token: 0x040007C0 RID: 1984
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr SecurityDescriptor;

		// Token: 0x040007C1 RID: 1985
		public uint SecurityDescriptorSize;
	}
}
