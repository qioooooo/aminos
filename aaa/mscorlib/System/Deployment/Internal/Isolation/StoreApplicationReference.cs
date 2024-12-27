using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000202 RID: 514
	internal struct StoreApplicationReference
	{
		// Token: 0x0600155A RID: 5466 RVA: 0x00036ED6 File Offset: 0x00035ED6
		public StoreApplicationReference(Guid RefScheme, string Id, string NcData)
		{
			this.Size = (uint)Marshal.SizeOf(typeof(StoreApplicationReference));
			this.Flags = StoreApplicationReference.RefFlags.Nothing;
			this.GuidScheme = RefScheme;
			this.Identifier = Id;
			this.NonCanonicalData = NcData;
		}

		// Token: 0x0600155B RID: 5467 RVA: 0x00036F0C File Offset: 0x00035F0C
		public IntPtr ToIntPtr()
		{
			IntPtr intPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(this));
			Marshal.StructureToPtr(this, intPtr, false);
			return intPtr;
		}

		// Token: 0x0600155C RID: 5468 RVA: 0x00036F42 File Offset: 0x00035F42
		public static void Destroy(IntPtr ip)
		{
			if (ip != IntPtr.Zero)
			{
				Marshal.DestroyStructure(ip, typeof(StoreApplicationReference));
				Marshal.FreeCoTaskMem(ip);
			}
		}

		// Token: 0x04000850 RID: 2128
		[MarshalAs(UnmanagedType.U4)]
		public uint Size;

		// Token: 0x04000851 RID: 2129
		[MarshalAs(UnmanagedType.U4)]
		public StoreApplicationReference.RefFlags Flags;

		// Token: 0x04000852 RID: 2130
		public Guid GuidScheme;

		// Token: 0x04000853 RID: 2131
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Identifier;

		// Token: 0x04000854 RID: 2132
		[MarshalAs(UnmanagedType.LPWStr)]
		public string NonCanonicalData;

		// Token: 0x02000203 RID: 515
		[Flags]
		public enum RefFlags
		{
			// Token: 0x04000856 RID: 2134
			Nothing = 0
		}
	}
}
