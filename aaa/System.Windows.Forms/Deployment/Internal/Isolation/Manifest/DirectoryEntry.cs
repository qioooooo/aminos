using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000B9 RID: 185
	[StructLayout(LayoutKind.Sequential)]
	internal class DirectoryEntry : IDisposable
	{
		// Token: 0x060002F7 RID: 759 RVA: 0x000078F0 File Offset: 0x000068F0
		~DirectoryEntry()
		{
			this.Dispose(false);
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x00007920 File Offset: 0x00006920
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x00007929 File Offset: 0x00006929
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

		// Token: 0x04000D33 RID: 3379
		public uint Flags;

		// Token: 0x04000D34 RID: 3380
		public uint Protection;

		// Token: 0x04000D35 RID: 3381
		[MarshalAs(UnmanagedType.LPWStr)]
		public string BuildFilter;

		// Token: 0x04000D36 RID: 3382
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr SecurityDescriptor;

		// Token: 0x04000D37 RID: 3383
		public uint SecurityDescriptorSize;
	}
}
