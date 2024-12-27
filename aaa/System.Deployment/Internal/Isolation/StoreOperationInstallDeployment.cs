using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000130 RID: 304
	internal struct StoreOperationInstallDeployment
	{
		// Token: 0x060006CA RID: 1738 RVA: 0x0001F8F1 File Offset: 0x0001E8F1
		public StoreOperationInstallDeployment(IDefinitionAppId App, StoreApplicationReference reference)
		{
			this = new StoreOperationInstallDeployment(App, true, reference);
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x0001F8FC File Offset: 0x0001E8FC
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

		// Token: 0x060006CC RID: 1740 RVA: 0x0001F94A File Offset: 0x0001E94A
		public void Destroy()
		{
			StoreApplicationReference.Destroy(this.Reference);
		}

		// Token: 0x04000553 RID: 1363
		[MarshalAs(UnmanagedType.U4)]
		public uint Size;

		// Token: 0x04000554 RID: 1364
		[MarshalAs(UnmanagedType.U4)]
		public StoreOperationInstallDeployment.OpFlags Flags;

		// Token: 0x04000555 RID: 1365
		[MarshalAs(UnmanagedType.Interface)]
		public IDefinitionAppId Application;

		// Token: 0x04000556 RID: 1366
		public IntPtr Reference;

		// Token: 0x02000131 RID: 305
		[Flags]
		public enum OpFlags
		{
			// Token: 0x04000558 RID: 1368
			Nothing = 0,
			// Token: 0x04000559 RID: 1369
			UninstallOthers = 1
		}

		// Token: 0x02000132 RID: 306
		public enum Disposition
		{
			// Token: 0x0400055B RID: 1371
			Failed,
			// Token: 0x0400055C RID: 1372
			AlreadyInstalled,
			// Token: 0x0400055D RID: 1373
			Installed
		}
	}
}
