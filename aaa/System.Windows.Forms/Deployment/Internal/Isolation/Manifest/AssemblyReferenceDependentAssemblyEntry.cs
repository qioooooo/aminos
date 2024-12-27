using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000089 RID: 137
	[StructLayout(LayoutKind.Sequential)]
	internal class AssemblyReferenceDependentAssemblyEntry : IDisposable
	{
		// Token: 0x06000275 RID: 629 RVA: 0x00007694 File Offset: 0x00006694
		~AssemblyReferenceDependentAssemblyEntry()
		{
			this.Dispose(false);
		}

		// Token: 0x06000276 RID: 630 RVA: 0x000076C4 File Offset: 0x000066C4
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06000277 RID: 631 RVA: 0x000076CD File Offset: 0x000066CD
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

		// Token: 0x04000C6E RID: 3182
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Group;

		// Token: 0x04000C6F RID: 3183
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Codebase;

		// Token: 0x04000C70 RID: 3184
		public ulong Size;

		// Token: 0x04000C71 RID: 3185
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr HashValue;

		// Token: 0x04000C72 RID: 3186
		public uint HashValueSize;

		// Token: 0x04000C73 RID: 3187
		public uint HashAlgorithm;

		// Token: 0x04000C74 RID: 3188
		public uint Flags;

		// Token: 0x04000C75 RID: 3189
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ResourceFallbackCulture;

		// Token: 0x04000C76 RID: 3190
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Description;

		// Token: 0x04000C77 RID: 3191
		[MarshalAs(UnmanagedType.LPWStr)]
		public string SupportUrl;

		// Token: 0x04000C78 RID: 3192
		public ISection HashElements;
	}
}
