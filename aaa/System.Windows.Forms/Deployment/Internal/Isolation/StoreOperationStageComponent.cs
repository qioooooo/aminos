using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020000F7 RID: 247
	internal struct StoreOperationStageComponent
	{
		// Token: 0x060003D2 RID: 978 RVA: 0x00008081 File Offset: 0x00007081
		public void Destroy()
		{
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x00008083 File Offset: 0x00007083
		public StoreOperationStageComponent(IDefinitionAppId app, string Manifest)
		{
			this = new StoreOperationStageComponent(app, null, Manifest);
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x0000808E File Offset: 0x0000708E
		public StoreOperationStageComponent(IDefinitionAppId app, IDefinitionIdentity comp, string Manifest)
		{
			this.Size = (uint)Marshal.SizeOf(typeof(StoreOperationStageComponent));
			this.Flags = StoreOperationStageComponent.OpFlags.Nothing;
			this.Application = app;
			this.Component = comp;
			this.ManifestPath = Manifest;
		}

		// Token: 0x04000DAB RID: 3499
		[MarshalAs(UnmanagedType.U4)]
		public uint Size;

		// Token: 0x04000DAC RID: 3500
		[MarshalAs(UnmanagedType.U4)]
		public StoreOperationStageComponent.OpFlags Flags;

		// Token: 0x04000DAD RID: 3501
		[MarshalAs(UnmanagedType.Interface)]
		public IDefinitionAppId Application;

		// Token: 0x04000DAE RID: 3502
		[MarshalAs(UnmanagedType.Interface)]
		public IDefinitionIdentity Component;

		// Token: 0x04000DAF RID: 3503
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ManifestPath;

		// Token: 0x020000F8 RID: 248
		[Flags]
		public enum OpFlags
		{
			// Token: 0x04000DB1 RID: 3505
			Nothing = 0
		}

		// Token: 0x020000F9 RID: 249
		public enum Disposition
		{
			// Token: 0x04000DB3 RID: 3507
			Failed,
			// Token: 0x04000DB4 RID: 3508
			Installed,
			// Token: 0x04000DB5 RID: 3509
			Refreshed,
			// Token: 0x04000DB6 RID: 3510
			AlreadyInstalled
		}
	}
}
