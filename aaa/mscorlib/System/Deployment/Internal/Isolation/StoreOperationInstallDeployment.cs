using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x0200020A RID: 522
	internal struct StoreOperationInstallDeployment
	{
		// Token: 0x06001562 RID: 5474 RVA: 0x00037005 File Offset: 0x00036005
		public StoreOperationInstallDeployment(IDefinitionAppId App, StoreApplicationReference reference)
		{
			this = new StoreOperationInstallDeployment(App, true, reference);
		}

		// Token: 0x06001563 RID: 5475 RVA: 0x00037010 File Offset: 0x00036010
		public StoreOperationInstallDeployment(IDefinitionAppId App, bool UninstallOthers, StoreApplicationReference reference)
		{
			this.Size = (uint)Marshal.SizeOf(typeof(StoreOperationInstallDeployment));
			this.Flags = StoreOperationInstallDeployment.OpFlags.Nothing;
			this.Application = App;
			if (UninstallOthers)
			{
				this.Flags |= StoreOperationInstallDeployment.OpFlags.UninstallOthers;
			}
			this.Reference = reference.ToIntPtr();
		}

		// Token: 0x06001564 RID: 5476 RVA: 0x0003705E File Offset: 0x0003605E
		public void Destroy()
		{
			StoreApplicationReference.Destroy(this.Reference);
		}

		// Token: 0x0400086B RID: 2155
		[MarshalAs(UnmanagedType.U4)]
		public uint Size;

		// Token: 0x0400086C RID: 2156
		[MarshalAs(UnmanagedType.U4)]
		public StoreOperationInstallDeployment.OpFlags Flags;

		// Token: 0x0400086D RID: 2157
		[MarshalAs(UnmanagedType.Interface)]
		public IDefinitionAppId Application;

		// Token: 0x0400086E RID: 2158
		public IntPtr Reference;

		// Token: 0x0200020B RID: 523
		[Flags]
		public enum OpFlags
		{
			// Token: 0x04000870 RID: 2160
			Nothing = 0,
			// Token: 0x04000871 RID: 2161
			UninstallOthers = 1
		}

		// Token: 0x0200020C RID: 524
		public enum Disposition
		{
			// Token: 0x04000873 RID: 2163
			Failed,
			// Token: 0x04000874 RID: 2164
			AlreadyInstalled,
			// Token: 0x04000875 RID: 2165
			Installed
		}
	}
}
