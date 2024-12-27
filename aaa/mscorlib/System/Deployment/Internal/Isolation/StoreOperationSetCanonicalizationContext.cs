using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000214 RID: 532
	internal struct StoreOperationSetCanonicalizationContext
	{
		// Token: 0x0600156E RID: 5486 RVA: 0x00037325 File Offset: 0x00036325
		public StoreOperationSetCanonicalizationContext(string Bases, string Exports)
		{
			this.Size = (uint)Marshal.SizeOf(typeof(StoreOperationSetCanonicalizationContext));
			this.Flags = StoreOperationSetCanonicalizationContext.OpFlags.Nothing;
			this.BaseAddressFilePath = Bases;
			this.ExportsFilePath = Exports;
		}

		// Token: 0x0600156F RID: 5487 RVA: 0x00037351 File Offset: 0x00036351
		public void Destroy()
		{
		}

		// Token: 0x04000891 RID: 2193
		[MarshalAs(UnmanagedType.U4)]
		public uint Size;

		// Token: 0x04000892 RID: 2194
		[MarshalAs(UnmanagedType.U4)]
		public StoreOperationSetCanonicalizationContext.OpFlags Flags;

		// Token: 0x04000893 RID: 2195
		[MarshalAs(UnmanagedType.LPWStr)]
		public string BaseAddressFilePath;

		// Token: 0x04000894 RID: 2196
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ExportsFilePath;

		// Token: 0x02000215 RID: 533
		[Flags]
		public enum OpFlags
		{
			// Token: 0x04000896 RID: 2198
			Nothing = 0
		}
	}
}
