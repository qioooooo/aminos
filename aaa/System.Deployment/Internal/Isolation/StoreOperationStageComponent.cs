using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000122 RID: 290
	internal struct StoreOperationStageComponent
	{
		// Token: 0x060006BC RID: 1724 RVA: 0x0001F739 File Offset: 0x0001E739
		public void Destroy()
		{
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x0001F73B File Offset: 0x0001E73B
		public StoreOperationStageComponent(IDefinitionAppId app, string Manifest)
		{
			this = new StoreOperationStageComponent(app, null, Manifest);
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x0001F746 File Offset: 0x0001E746
		public StoreOperationStageComponent(IDefinitionAppId app, IDefinitionIdentity comp, string Manifest)
		{
			this.Size = (uint)Marshal.SizeOf(typeof(StoreOperationStageComponent));
			this.Flags = StoreOperationStageComponent.OpFlags.Nothing;
			this.Application = app;
			this.Component = comp;
			this.ManifestPath = Manifest;
		}

		// Token: 0x0400051F RID: 1311
		[MarshalAs(UnmanagedType.U4)]
		public uint Size;

		// Token: 0x04000520 RID: 1312
		[MarshalAs(UnmanagedType.U4)]
		public StoreOperationStageComponent.OpFlags Flags;

		// Token: 0x04000521 RID: 1313
		[MarshalAs(UnmanagedType.Interface)]
		public IDefinitionAppId Application;

		// Token: 0x04000522 RID: 1314
		[MarshalAs(UnmanagedType.Interface)]
		public IDefinitionIdentity Component;

		// Token: 0x04000523 RID: 1315
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ManifestPath;

		// Token: 0x02000123 RID: 291
		[Flags]
		public enum OpFlags
		{
			// Token: 0x04000525 RID: 1317
			Nothing = 0
		}

		// Token: 0x02000124 RID: 292
		public enum Disposition
		{
			// Token: 0x04000527 RID: 1319
			Failed,
			// Token: 0x04000528 RID: 1320
			Installed,
			// Token: 0x04000529 RID: 1321
			Refreshed,
			// Token: 0x0400052A RID: 1322
			AlreadyInstalled
		}
	}
}
