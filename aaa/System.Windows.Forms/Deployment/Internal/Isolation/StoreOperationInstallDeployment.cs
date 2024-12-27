using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000105 RID: 261
	internal struct StoreOperationInstallDeployment
	{
		// Token: 0x060003E0 RID: 992 RVA: 0x00008239 File Offset: 0x00007239
		public StoreOperationInstallDeployment(IDefinitionAppId App, StoreApplicationReference reference)
		{
			this = new StoreOperationInstallDeployment(App, true, reference);
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x00008244 File Offset: 0x00007244
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

		// Token: 0x060003E2 RID: 994 RVA: 0x00008292 File Offset: 0x00007292
		public void Destroy()
		{
			StoreApplicationReference.Destroy(this.Reference);
		}

		// Token: 0x04000DDF RID: 3551
		[MarshalAs(UnmanagedType.U4)]
		public uint Size;

		// Token: 0x04000DE0 RID: 3552
		[MarshalAs(UnmanagedType.U4)]
		public StoreOperationInstallDeployment.OpFlags Flags;

		// Token: 0x04000DE1 RID: 3553
		[MarshalAs(UnmanagedType.Interface)]
		public IDefinitionAppId Application;

		// Token: 0x04000DE2 RID: 3554
		public IntPtr Reference;

		// Token: 0x02000106 RID: 262
		[Flags]
		public enum OpFlags
		{
			// Token: 0x04000DE4 RID: 3556
			Nothing = 0,
			// Token: 0x04000DE5 RID: 3557
			UninstallOthers = 1
		}

		// Token: 0x02000107 RID: 263
		public enum Disposition
		{
			// Token: 0x04000DE7 RID: 3559
			Failed,
			// Token: 0x04000DE8 RID: 3560
			AlreadyInstalled,
			// Token: 0x04000DE9 RID: 3561
			Installed
		}
	}
}
