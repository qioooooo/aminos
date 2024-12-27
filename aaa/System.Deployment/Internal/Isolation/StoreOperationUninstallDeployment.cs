using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000133 RID: 307
	internal struct StoreOperationUninstallDeployment
	{
		// Token: 0x060006CD RID: 1741 RVA: 0x0001F957 File Offset: 0x0001E957
		public StoreOperationUninstallDeployment(IDefinitionAppId appid, StoreApplicationReference AppRef)
		{
			this.Size = (uint)Marshal.SizeOf(typeof(StoreOperationUninstallDeployment));
			this.Flags = StoreOperationUninstallDeployment.OpFlags.Nothing;
			this.Application = appid;
			this.Reference = AppRef.ToIntPtr();
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x0001F989 File Offset: 0x0001E989
		public void Destroy()
		{
			StoreApplicationReference.Destroy(this.Reference);
		}

		// Token: 0x0400055E RID: 1374
		[MarshalAs(UnmanagedType.U4)]
		public uint Size;

		// Token: 0x0400055F RID: 1375
		[MarshalAs(UnmanagedType.U4)]
		public StoreOperationUninstallDeployment.OpFlags Flags;

		// Token: 0x04000560 RID: 1376
		[MarshalAs(UnmanagedType.Interface)]
		public IDefinitionAppId Application;

		// Token: 0x04000561 RID: 1377
		public IntPtr Reference;

		// Token: 0x02000134 RID: 308
		[Flags]
		public enum OpFlags
		{
			// Token: 0x04000563 RID: 1379
			Nothing = 0
		}

		// Token: 0x02000135 RID: 309
		public enum Disposition
		{
			// Token: 0x04000565 RID: 1381
			Failed,
			// Token: 0x04000566 RID: 1382
			DidNotExist,
			// Token: 0x04000567 RID: 1383
			Uninstalled
		}
	}
}
