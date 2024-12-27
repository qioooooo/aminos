using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000196 RID: 406
	[StructLayout(LayoutKind.Sequential)]
	internal class FileEntry : IDisposable
	{
		// Token: 0x06001420 RID: 5152 RVA: 0x00036474 File Offset: 0x00035474
		~FileEntry()
		{
			this.Dispose(false);
		}

		// Token: 0x06001421 RID: 5153 RVA: 0x000364A4 File Offset: 0x000354A4
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06001422 RID: 5154 RVA: 0x000364B0 File Offset: 0x000354B0
		public void Dispose(bool fDisposing)
		{
			if (this.HashValue != IntPtr.Zero)
			{
				Marshal.FreeCoTaskMem(this.HashValue);
				this.HashValue = IntPtr.Zero;
			}
			if (fDisposing)
			{
				if (this.MuiMapping != null)
				{
					this.MuiMapping.Dispose(true);
					this.MuiMapping = null;
				}
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x04000718 RID: 1816
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Name;

		// Token: 0x04000719 RID: 1817
		public uint HashAlgorithm;

		// Token: 0x0400071A RID: 1818
		[MarshalAs(UnmanagedType.LPWStr)]
		public string LoadFrom;

		// Token: 0x0400071B RID: 1819
		[MarshalAs(UnmanagedType.LPWStr)]
		public string SourcePath;

		// Token: 0x0400071C RID: 1820
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ImportPath;

		// Token: 0x0400071D RID: 1821
		[MarshalAs(UnmanagedType.LPWStr)]
		public string SourceName;

		// Token: 0x0400071E RID: 1822
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Location;

		// Token: 0x0400071F RID: 1823
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr HashValue;

		// Token: 0x04000720 RID: 1824
		public uint HashValueSize;

		// Token: 0x04000721 RID: 1825
		public ulong Size;

		// Token: 0x04000722 RID: 1826
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Group;

		// Token: 0x04000723 RID: 1827
		public uint Flags;

		// Token: 0x04000724 RID: 1828
		public MuiResourceMapEntry MuiMapping;

		// Token: 0x04000725 RID: 1829
		public uint WritableType;

		// Token: 0x04000726 RID: 1830
		public ISection HashElements;
	}
}
