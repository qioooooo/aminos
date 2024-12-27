using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001C8 RID: 456
	[StructLayout(LayoutKind.Sequential)]
	internal class RegistryKeyEntry : IDisposable
	{
		// Token: 0x06000837 RID: 2103 RVA: 0x00021014 File Offset: 0x00020014
		~RegistryKeyEntry()
		{
			this.Dispose(false);
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x00021044 File Offset: 0x00020044
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x00021050 File Offset: 0x00020050
		public void Dispose(bool fDisposing)
		{
			if (this.SecurityDescriptor != IntPtr.Zero)
			{
				Marshal.FreeCoTaskMem(this.SecurityDescriptor);
				this.SecurityDescriptor = IntPtr.Zero;
			}
			if (this.Values != IntPtr.Zero)
			{
				Marshal.FreeCoTaskMem(this.Values);
				this.Values = IntPtr.Zero;
			}
			if (this.Keys != IntPtr.Zero)
			{
				Marshal.FreeCoTaskMem(this.Keys);
				this.Keys = IntPtr.Zero;
			}
			if (fDisposing)
			{
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x040007AA RID: 1962
		public uint Flags;

		// Token: 0x040007AB RID: 1963
		public uint Protection;

		// Token: 0x040007AC RID: 1964
		[MarshalAs(UnmanagedType.LPWStr)]
		public string BuildFilter;

		// Token: 0x040007AD RID: 1965
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr SecurityDescriptor;

		// Token: 0x040007AE RID: 1966
		public uint SecurityDescriptorSize;

		// Token: 0x040007AF RID: 1967
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr Values;

		// Token: 0x040007B0 RID: 1968
		public uint ValuesSize;

		// Token: 0x040007B1 RID: 1969
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr Keys;

		// Token: 0x040007B2 RID: 1970
		public uint KeysSize;
	}
}
