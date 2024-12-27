using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000183 RID: 387
	[StructLayout(LayoutKind.Sequential)]
	internal class FileEntry : IDisposable
	{
		// Token: 0x06000786 RID: 1926 RVA: 0x00020DB4 File Offset: 0x0001FDB4
		~FileEntry()
		{
			this.Dispose(false);
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x00020DE4 File Offset: 0x0001FDE4
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x00020DF0 File Offset: 0x0001FDF0
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

		// Token: 0x040006A8 RID: 1704
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Name;

		// Token: 0x040006A9 RID: 1705
		public uint HashAlgorithm;

		// Token: 0x040006AA RID: 1706
		[MarshalAs(UnmanagedType.LPWStr)]
		public string LoadFrom;

		// Token: 0x040006AB RID: 1707
		[MarshalAs(UnmanagedType.LPWStr)]
		public string SourcePath;

		// Token: 0x040006AC RID: 1708
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ImportPath;

		// Token: 0x040006AD RID: 1709
		[MarshalAs(UnmanagedType.LPWStr)]
		public string SourceName;

		// Token: 0x040006AE RID: 1710
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Location;

		// Token: 0x040006AF RID: 1711
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr HashValue;

		// Token: 0x040006B0 RID: 1712
		public uint HashValueSize;

		// Token: 0x040006B1 RID: 1713
		public ulong Size;

		// Token: 0x040006B2 RID: 1714
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Group;

		// Token: 0x040006B3 RID: 1715
		public uint Flags;

		// Token: 0x040006B4 RID: 1716
		public MuiResourceMapEntry MuiMapping;

		// Token: 0x040006B5 RID: 1717
		public uint WritableType;

		// Token: 0x040006B6 RID: 1718
		public ISection HashElements;
	}
}
