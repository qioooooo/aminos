using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x0200010F RID: 271
	internal struct StoreOperationSetCanonicalizationContext
	{
		// Token: 0x060003EC RID: 1004 RVA: 0x00008559 File Offset: 0x00007559
		public StoreOperationSetCanonicalizationContext(string Bases, string Exports)
		{
			this.Size = (uint)Marshal.SizeOf(typeof(StoreOperationSetCanonicalizationContext));
			this.Flags = StoreOperationSetCanonicalizationContext.OpFlags.Nothing;
			this.BaseAddressFilePath = Bases;
			this.ExportsFilePath = Exports;
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x00008585 File Offset: 0x00007585
		public void Destroy()
		{
		}

		// Token: 0x04000E05 RID: 3589
		[MarshalAs(UnmanagedType.U4)]
		public uint Size;

		// Token: 0x04000E06 RID: 3590
		[MarshalAs(UnmanagedType.U4)]
		public StoreOperationSetCanonicalizationContext.OpFlags Flags;

		// Token: 0x04000E07 RID: 3591
		[MarshalAs(UnmanagedType.LPWStr)]
		public string BaseAddressFilePath;

		// Token: 0x04000E08 RID: 3592
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ExportsFilePath;

		// Token: 0x02000110 RID: 272
		[Flags]
		public enum OpFlags
		{
			// Token: 0x04000E0A RID: 3594
			Nothing = 0
		}
	}
}
