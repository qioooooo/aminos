using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020000FA RID: 250
	internal struct StoreOperationStageComponentFile
	{
		// Token: 0x060003D5 RID: 981 RVA: 0x000080C1 File Offset: 0x000070C1
		public StoreOperationStageComponentFile(IDefinitionAppId App, string CompRelPath, string SrcFile)
		{
			this = new StoreOperationStageComponentFile(App, null, CompRelPath, SrcFile);
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x000080CD File Offset: 0x000070CD
		public StoreOperationStageComponentFile(IDefinitionAppId App, IDefinitionIdentity Component, string CompRelPath, string SrcFile)
		{
			this.Size = (uint)Marshal.SizeOf(typeof(StoreOperationStageComponentFile));
			this.Flags = StoreOperationStageComponentFile.OpFlags.Nothing;
			this.Application = App;
			this.Component = Component;
			this.ComponentRelativePath = CompRelPath;
			this.SourceFilePath = SrcFile;
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x00008108 File Offset: 0x00007108
		public void Destroy()
		{
		}

		// Token: 0x04000DB7 RID: 3511
		[MarshalAs(UnmanagedType.U4)]
		public uint Size;

		// Token: 0x04000DB8 RID: 3512
		[MarshalAs(UnmanagedType.U4)]
		public StoreOperationStageComponentFile.OpFlags Flags;

		// Token: 0x04000DB9 RID: 3513
		[MarshalAs(UnmanagedType.Interface)]
		public IDefinitionAppId Application;

		// Token: 0x04000DBA RID: 3514
		[MarshalAs(UnmanagedType.Interface)]
		public IDefinitionIdentity Component;

		// Token: 0x04000DBB RID: 3515
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ComponentRelativePath;

		// Token: 0x04000DBC RID: 3516
		[MarshalAs(UnmanagedType.LPWStr)]
		public string SourceFilePath;

		// Token: 0x020000FB RID: 251
		[Flags]
		public enum OpFlags
		{
			// Token: 0x04000DBE RID: 3518
			Nothing = 0
		}

		// Token: 0x020000FC RID: 252
		public enum Disposition
		{
			// Token: 0x04000DC0 RID: 3520
			Failed,
			// Token: 0x04000DC1 RID: 3521
			Installed,
			// Token: 0x04000DC2 RID: 3522
			Refreshed,
			// Token: 0x04000DC3 RID: 3523
			AlreadyInstalled
		}
	}
}
