using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000071 RID: 113
	[StructLayout(LayoutKind.Sequential)]
	internal class FileEntry : IDisposable
	{
		// Token: 0x0600023B RID: 571 RVA: 0x000075BC File Offset: 0x000065BC
		~FileEntry()
		{
			this.Dispose(false);
		}

		// Token: 0x0600023C RID: 572 RVA: 0x000075EC File Offset: 0x000065EC
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x0600023D RID: 573 RVA: 0x000075F8 File Offset: 0x000065F8
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

		// Token: 0x04000C1E RID: 3102
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Name;

		// Token: 0x04000C1F RID: 3103
		public uint HashAlgorithm;

		// Token: 0x04000C20 RID: 3104
		[MarshalAs(UnmanagedType.LPWStr)]
		public string LoadFrom;

		// Token: 0x04000C21 RID: 3105
		[MarshalAs(UnmanagedType.LPWStr)]
		public string SourcePath;

		// Token: 0x04000C22 RID: 3106
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ImportPath;

		// Token: 0x04000C23 RID: 3107
		[MarshalAs(UnmanagedType.LPWStr)]
		public string SourceName;

		// Token: 0x04000C24 RID: 3108
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Location;

		// Token: 0x04000C25 RID: 3109
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr HashValue;

		// Token: 0x04000C26 RID: 3110
		public uint HashValueSize;

		// Token: 0x04000C27 RID: 3111
		public ulong Size;

		// Token: 0x04000C28 RID: 3112
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Group;

		// Token: 0x04000C29 RID: 3113
		public uint Flags;

		// Token: 0x04000C2A RID: 3114
		public MuiResourceMapEntry MuiMapping;

		// Token: 0x04000C2B RID: 3115
		public uint WritableType;

		// Token: 0x04000C2C RID: 3116
		public ISection HashElements;
	}
}
