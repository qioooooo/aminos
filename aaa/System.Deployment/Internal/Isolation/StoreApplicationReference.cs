using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000128 RID: 296
	internal struct StoreApplicationReference
	{
		// Token: 0x060006C2 RID: 1730 RVA: 0x0001F7C2 File Offset: 0x0001E7C2
		public StoreApplicationReference(Guid RefScheme, string Id, string NcData)
		{
			this.Size = (uint)Marshal.SizeOf(typeof(StoreApplicationReference));
			this.Flags = StoreApplicationReference.RefFlags.Nothing;
			this.GuidScheme = RefScheme;
			this.Identifier = Id;
			this.NonCanonicalData = NcData;
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x0001F7F8 File Offset: 0x0001E7F8
		public IntPtr ToIntPtr()
		{
			IntPtr intPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(this));
			Marshal.StructureToPtr(this, intPtr, false);
			return intPtr;
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x0001F82E File Offset: 0x0001E82E
		public static void Destroy(IntPtr ip)
		{
			if (ip != IntPtr.Zero)
			{
				Marshal.DestroyStructure(ip, typeof(StoreApplicationReference));
				Marshal.FreeCoTaskMem(ip);
			}
		}

		// Token: 0x04000538 RID: 1336
		[MarshalAs(UnmanagedType.U4)]
		public uint Size;

		// Token: 0x04000539 RID: 1337
		[MarshalAs(UnmanagedType.U4)]
		public StoreApplicationReference.RefFlags Flags;

		// Token: 0x0400053A RID: 1338
		public Guid GuidScheme;

		// Token: 0x0400053B RID: 1339
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Identifier;

		// Token: 0x0400053C RID: 1340
		[MarshalAs(UnmanagedType.LPWStr)]
		public string NonCanonicalData;

		// Token: 0x02000129 RID: 297
		[Flags]
		public enum RefFlags
		{
			// Token: 0x0400053E RID: 1342
			Nothing = 0
		}
	}
}
