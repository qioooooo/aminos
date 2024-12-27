using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020001FC RID: 508
	internal struct StoreOperationStageComponent
	{
		// Token: 0x06001554 RID: 5460 RVA: 0x00036E4D File Offset: 0x00035E4D
		public void Destroy()
		{
		}

		// Token: 0x06001555 RID: 5461 RVA: 0x00036E4F File Offset: 0x00035E4F
		public StoreOperationStageComponent(IDefinitionAppId app, string Manifest)
		{
			this = new StoreOperationStageComponent(app, null, Manifest);
		}

		// Token: 0x06001556 RID: 5462 RVA: 0x00036E5A File Offset: 0x00035E5A
		public StoreOperationStageComponent(IDefinitionAppId app, IDefinitionIdentity comp, string Manifest)
		{
			this.Size = (uint)Marshal.SizeOf(typeof(StoreOperationStageComponent));
			this.Flags = StoreOperationStageComponent.OpFlags.Nothing;
			this.Application = app;
			this.Component = comp;
			this.ManifestPath = Manifest;
		}

		// Token: 0x04000837 RID: 2103
		[MarshalAs(UnmanagedType.U4)]
		public uint Size;

		// Token: 0x04000838 RID: 2104
		[MarshalAs(UnmanagedType.U4)]
		public StoreOperationStageComponent.OpFlags Flags;

		// Token: 0x04000839 RID: 2105
		[MarshalAs(UnmanagedType.Interface)]
		public IDefinitionAppId Application;

		// Token: 0x0400083A RID: 2106
		[MarshalAs(UnmanagedType.Interface)]
		public IDefinitionIdentity Component;

		// Token: 0x0400083B RID: 2107
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ManifestPath;

		// Token: 0x020001FD RID: 509
		[Flags]
		public enum OpFlags
		{
			// Token: 0x0400083D RID: 2109
			Nothing = 0
		}

		// Token: 0x020001FE RID: 510
		public enum Disposition
		{
			// Token: 0x0400083F RID: 2111
			Failed,
			// Token: 0x04000840 RID: 2112
			Installed,
			// Token: 0x04000841 RID: 2113
			Refreshed,
			// Token: 0x04000842 RID: 2114
			AlreadyInstalled
		}
	}
}
