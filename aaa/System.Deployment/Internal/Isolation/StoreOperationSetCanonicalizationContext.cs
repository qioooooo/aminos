using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x0200013A RID: 314
	internal struct StoreOperationSetCanonicalizationContext
	{
		// Token: 0x060006D6 RID: 1750 RVA: 0x0001FC11 File Offset: 0x0001EC11
		public StoreOperationSetCanonicalizationContext(string Bases, string Exports)
		{
			this.Size = (uint)Marshal.SizeOf(typeof(StoreOperationSetCanonicalizationContext));
			this.Flags = StoreOperationSetCanonicalizationContext.OpFlags.Nothing;
			this.BaseAddressFilePath = Bases;
			this.ExportsFilePath = Exports;
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x0001FC3D File Offset: 0x0001EC3D
		public void Destroy()
		{
		}

		// Token: 0x04000579 RID: 1401
		[MarshalAs(UnmanagedType.U4)]
		public uint Size;

		// Token: 0x0400057A RID: 1402
		[MarshalAs(UnmanagedType.U4)]
		public StoreOperationSetCanonicalizationContext.OpFlags Flags;

		// Token: 0x0400057B RID: 1403
		[MarshalAs(UnmanagedType.LPWStr)]
		public string BaseAddressFilePath;

		// Token: 0x0400057C RID: 1404
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ExportsFilePath;

		// Token: 0x0200013B RID: 315
		[Flags]
		public enum OpFlags
		{
			// Token: 0x0400057E RID: 1406
			Nothing = 0
		}
	}
}
