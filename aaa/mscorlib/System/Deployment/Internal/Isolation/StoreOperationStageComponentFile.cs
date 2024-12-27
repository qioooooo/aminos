using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020001FF RID: 511
	internal struct StoreOperationStageComponentFile
	{
		// Token: 0x06001557 RID: 5463 RVA: 0x00036E8D File Offset: 0x00035E8D
		public StoreOperationStageComponentFile(IDefinitionAppId App, string CompRelPath, string SrcFile)
		{
			this = new StoreOperationStageComponentFile(App, null, CompRelPath, SrcFile);
		}

		// Token: 0x06001558 RID: 5464 RVA: 0x00036E99 File Offset: 0x00035E99
		public StoreOperationStageComponentFile(IDefinitionAppId App, IDefinitionIdentity Component, string CompRelPath, string SrcFile)
		{
			this.Size = (uint)Marshal.SizeOf(typeof(StoreOperationStageComponentFile));
			this.Flags = StoreOperationStageComponentFile.OpFlags.Nothing;
			this.Application = App;
			this.Component = Component;
			this.ComponentRelativePath = CompRelPath;
			this.SourceFilePath = SrcFile;
		}

		// Token: 0x06001559 RID: 5465 RVA: 0x00036ED4 File Offset: 0x00035ED4
		public void Destroy()
		{
		}

		// Token: 0x04000843 RID: 2115
		[MarshalAs(UnmanagedType.U4)]
		public uint Size;

		// Token: 0x04000844 RID: 2116
		[MarshalAs(UnmanagedType.U4)]
		public StoreOperationStageComponentFile.OpFlags Flags;

		// Token: 0x04000845 RID: 2117
		[MarshalAs(UnmanagedType.Interface)]
		public IDefinitionAppId Application;

		// Token: 0x04000846 RID: 2118
		[MarshalAs(UnmanagedType.Interface)]
		public IDefinitionIdentity Component;

		// Token: 0x04000847 RID: 2119
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ComponentRelativePath;

		// Token: 0x04000848 RID: 2120
		[MarshalAs(UnmanagedType.LPWStr)]
		public string SourceFilePath;

		// Token: 0x02000200 RID: 512
		[Flags]
		public enum OpFlags
		{
			// Token: 0x0400084A RID: 2122
			Nothing = 0
		}

		// Token: 0x02000201 RID: 513
		public enum Disposition
		{
			// Token: 0x0400084C RID: 2124
			Failed,
			// Token: 0x0400084D RID: 2125
			Installed,
			// Token: 0x0400084E RID: 2126
			Refreshed,
			// Token: 0x0400084F RID: 2127
			AlreadyInstalled
		}
	}
}
