using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001AE RID: 430
	[StructLayout(LayoutKind.Sequential)]
	internal class AssemblyReferenceDependentAssemblyEntry : IDisposable
	{
		// Token: 0x0600145A RID: 5210 RVA: 0x0003654C File Offset: 0x0003554C
		~AssemblyReferenceDependentAssemblyEntry()
		{
			this.Dispose(false);
		}

		// Token: 0x0600145B RID: 5211 RVA: 0x0003657C File Offset: 0x0003557C
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x0600145C RID: 5212 RVA: 0x00036585 File Offset: 0x00035585
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

		// Token: 0x04000768 RID: 1896
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Group;

		// Token: 0x04000769 RID: 1897
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Codebase;

		// Token: 0x0400076A RID: 1898
		public ulong Size;

		// Token: 0x0400076B RID: 1899
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr HashValue;

		// Token: 0x0400076C RID: 1900
		public uint HashValueSize;

		// Token: 0x0400076D RID: 1901
		public uint HashAlgorithm;

		// Token: 0x0400076E RID: 1902
		public uint Flags;

		// Token: 0x0400076F RID: 1903
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ResourceFallbackCulture;

		// Token: 0x04000770 RID: 1904
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Description;

		// Token: 0x04000771 RID: 1905
		[MarshalAs(UnmanagedType.LPWStr)]
		public string SupportUrl;

		// Token: 0x04000772 RID: 1906
		public ISection HashElements;
	}
}
