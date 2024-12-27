using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000108 RID: 264
	internal struct StoreOperationUninstallDeployment
	{
		// Token: 0x060003E3 RID: 995 RVA: 0x0000829F File Offset: 0x0000729F
		public StoreOperationUninstallDeployment(IDefinitionAppId appid, StoreApplicationReference AppRef)
		{
			this.Size = (uint)Marshal.SizeOf(typeof(StoreOperationUninstallDeployment));
			this.Flags = StoreOperationUninstallDeployment.OpFlags.Nothing;
			this.Application = appid;
			this.Reference = AppRef.ToIntPtr();
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x000082D1 File Offset: 0x000072D1
		public void Destroy()
		{
			StoreApplicationReference.Destroy(this.Reference);
		}

		// Token: 0x04000DEA RID: 3562
		[MarshalAs(UnmanagedType.U4)]
		public uint Size;

		// Token: 0x04000DEB RID: 3563
		[MarshalAs(UnmanagedType.U4)]
		public StoreOperationUninstallDeployment.OpFlags Flags;

		// Token: 0x04000DEC RID: 3564
		[MarshalAs(UnmanagedType.Interface)]
		public IDefinitionAppId Application;

		// Token: 0x04000DED RID: 3565
		public IntPtr Reference;

		// Token: 0x02000109 RID: 265
		[Flags]
		public enum OpFlags
		{
			// Token: 0x04000DEF RID: 3567
			Nothing = 0
		}

		// Token: 0x0200010A RID: 266
		public enum Disposition
		{
			// Token: 0x04000DF1 RID: 3569
			Failed,
			// Token: 0x04000DF2 RID: 3570
			DidNotExist,
			// Token: 0x04000DF3 RID: 3571
			Uninstalled
		}
	}
}
