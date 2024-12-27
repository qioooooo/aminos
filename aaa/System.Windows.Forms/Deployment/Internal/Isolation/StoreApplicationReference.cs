using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020000FD RID: 253
	internal struct StoreApplicationReference
	{
		// Token: 0x060003D8 RID: 984 RVA: 0x0000810A File Offset: 0x0000710A
		public StoreApplicationReference(Guid RefScheme, string Id, string NcData)
		{
			this.Size = (uint)Marshal.SizeOf(typeof(StoreApplicationReference));
			this.Flags = StoreApplicationReference.RefFlags.Nothing;
			this.GuidScheme = RefScheme;
			this.Identifier = Id;
			this.NonCanonicalData = NcData;
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x00008140 File Offset: 0x00007140
		public IntPtr ToIntPtr()
		{
			IntPtr intPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(this));
			Marshal.StructureToPtr(this, intPtr, false);
			return intPtr;
		}

		// Token: 0x060003DA RID: 986 RVA: 0x00008176 File Offset: 0x00007176
		public static void Destroy(IntPtr ip)
		{
			if (ip != IntPtr.Zero)
			{
				Marshal.DestroyStructure(ip, typeof(StoreApplicationReference));
				Marshal.FreeCoTaskMem(ip);
			}
		}

		// Token: 0x04000DC4 RID: 3524
		[MarshalAs(UnmanagedType.U4)]
		public uint Size;

		// Token: 0x04000DC5 RID: 3525
		[MarshalAs(UnmanagedType.U4)]
		public StoreApplicationReference.RefFlags Flags;

		// Token: 0x04000DC6 RID: 3526
		public Guid GuidScheme;

		// Token: 0x04000DC7 RID: 3527
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Identifier;

		// Token: 0x04000DC8 RID: 3528
		[MarshalAs(UnmanagedType.LPWStr)]
		public string NonCanonicalData;

		// Token: 0x020000FE RID: 254
		[Flags]
		public enum RefFlags
		{
			// Token: 0x04000DCA RID: 3530
			Nothing = 0
		}
	}
}
