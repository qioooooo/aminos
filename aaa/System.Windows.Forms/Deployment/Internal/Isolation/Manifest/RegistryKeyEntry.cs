using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000B6 RID: 182
	[StructLayout(LayoutKind.Sequential)]
	internal class RegistryKeyEntry : IDisposable
	{
		// Token: 0x060002EC RID: 748 RVA: 0x0000781C File Offset: 0x0000681C
		~RegistryKeyEntry()
		{
			this.Dispose(false);
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0000784C File Offset: 0x0000684C
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x060002EE RID: 750 RVA: 0x00007858 File Offset: 0x00006858
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

		// Token: 0x04000D20 RID: 3360
		public uint Flags;

		// Token: 0x04000D21 RID: 3361
		public uint Protection;

		// Token: 0x04000D22 RID: 3362
		[MarshalAs(UnmanagedType.LPWStr)]
		public string BuildFilter;

		// Token: 0x04000D23 RID: 3363
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr SecurityDescriptor;

		// Token: 0x04000D24 RID: 3364
		public uint SecurityDescriptorSize;

		// Token: 0x04000D25 RID: 3365
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr Values;

		// Token: 0x04000D26 RID: 3366
		public uint ValuesSize;

		// Token: 0x04000D27 RID: 3367
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr Keys;

		// Token: 0x04000D28 RID: 3368
		public uint KeysSize;
	}
}
