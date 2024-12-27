using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000125 RID: 293
	internal struct StoreOperationStageComponentFile
	{
		// Token: 0x060006BF RID: 1727 RVA: 0x0001F779 File Offset: 0x0001E779
		public StoreOperationStageComponentFile(IDefinitionAppId App, string CompRelPath, string SrcFile)
		{
			this = new StoreOperationStageComponentFile(App, null, CompRelPath, SrcFile);
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x0001F785 File Offset: 0x0001E785
		public StoreOperationStageComponentFile(IDefinitionAppId App, IDefinitionIdentity Component, string CompRelPath, string SrcFile)
		{
			this.Size = (uint)Marshal.SizeOf(typeof(StoreOperationStageComponentFile));
			this.Flags = StoreOperationStageComponentFile.OpFlags.Nothing;
			this.Application = App;
			this.Component = Component;
			this.ComponentRelativePath = CompRelPath;
			this.SourceFilePath = SrcFile;
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x0001F7C0 File Offset: 0x0001E7C0
		public void Destroy()
		{
		}

		// Token: 0x0400052B RID: 1323
		[MarshalAs(UnmanagedType.U4)]
		public uint Size;

		// Token: 0x0400052C RID: 1324
		[MarshalAs(UnmanagedType.U4)]
		public StoreOperationStageComponentFile.OpFlags Flags;

		// Token: 0x0400052D RID: 1325
		[MarshalAs(UnmanagedType.Interface)]
		public IDefinitionAppId Application;

		// Token: 0x0400052E RID: 1326
		[MarshalAs(UnmanagedType.Interface)]
		public IDefinitionIdentity Component;

		// Token: 0x0400052F RID: 1327
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ComponentRelativePath;

		// Token: 0x04000530 RID: 1328
		[MarshalAs(UnmanagedType.LPWStr)]
		public string SourceFilePath;

		// Token: 0x02000126 RID: 294
		[Flags]
		public enum OpFlags
		{
			// Token: 0x04000532 RID: 1330
			Nothing = 0
		}

		// Token: 0x02000127 RID: 295
		public enum Disposition
		{
			// Token: 0x04000534 RID: 1332
			Failed,
			// Token: 0x04000535 RID: 1333
			Installed,
			// Token: 0x04000536 RID: 1334
			Refreshed,
			// Token: 0x04000537 RID: 1335
			AlreadyInstalled
		}
	}
}
