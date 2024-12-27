using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x0200019B RID: 411
	[StructLayout(LayoutKind.Sequential)]
	internal class AssemblyReferenceDependentAssemblyEntry : IDisposable
	{
		// Token: 0x060007C0 RID: 1984 RVA: 0x00020E8C File Offset: 0x0001FE8C
		~AssemblyReferenceDependentAssemblyEntry()
		{
			this.Dispose(false);
		}

		// Token: 0x060007C1 RID: 1985 RVA: 0x00020EBC File Offset: 0x0001FEBC
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x060007C2 RID: 1986 RVA: 0x00020EC5 File Offset: 0x0001FEC5
		public void Dispose(bool fDisposing)
		{
			if (this.HashValue != IntPtr.Zero)
			{
				Marshal.FreeCoTaskMem(this.HashValue);
				this.HashValue = IntPtr.Zero;
			}
			if (fDisposing)
			{
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x040006F8 RID: 1784
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Group;

		// Token: 0x040006F9 RID: 1785
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Codebase;

		// Token: 0x040006FA RID: 1786
		public ulong Size;

		// Token: 0x040006FB RID: 1787
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr HashValue;

		// Token: 0x040006FC RID: 1788
		public uint HashValueSize;

		// Token: 0x040006FD RID: 1789
		public uint HashAlgorithm;

		// Token: 0x040006FE RID: 1790
		public uint Flags;

		// Token: 0x040006FF RID: 1791
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ResourceFallbackCulture;

		// Token: 0x04000700 RID: 1792
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Description;

		// Token: 0x04000701 RID: 1793
		[MarshalAs(UnmanagedType.LPWStr)]
		public string SupportUrl;

		// Token: 0x04000702 RID: 1794
		public ISection HashElements;
	}
}
