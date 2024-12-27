using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x0200020D RID: 525
	internal struct StoreOperationUninstallDeployment
	{
		// Token: 0x06001565 RID: 5477 RVA: 0x0003706B File Offset: 0x0003606B
		public StoreOperationUninstallDeployment(IDefinitionAppId appid, StoreApplicationReference AppRef)
		{
			this.Size = (uint)Marshal.SizeOf(typeof(StoreOperationUninstallDeployment));
			this.Flags = StoreOperationUninstallDeployment.OpFlags.Nothing;
			this.Application = appid;
			this.Reference = AppRef.ToIntPtr();
		}

		// Token: 0x06001566 RID: 5478 RVA: 0x0003709D File Offset: 0x0003609D
		public void Destroy()
		{
			StoreApplicationReference.Destroy(this.Reference);
		}

		// Token: 0x04000876 RID: 2166
		[MarshalAs(UnmanagedType.U4)]
		public uint Size;

		// Token: 0x04000877 RID: 2167
		[MarshalAs(UnmanagedType.U4)]
		public StoreOperationUninstallDeployment.OpFlags Flags;

		// Token: 0x04000878 RID: 2168
		[MarshalAs(UnmanagedType.Interface)]
		public IDefinitionAppId Application;

		// Token: 0x04000879 RID: 2169
		public IntPtr Reference;

		// Token: 0x0200020E RID: 526
		[Flags]
		public enum OpFlags
		{
			// Token: 0x0400087B RID: 2171
			Nothing = 0
		}

		// Token: 0x0200020F RID: 527
		public enum Disposition
		{
			// Token: 0x0400087D RID: 2173
			Failed,
			// Token: 0x0400087E RID: 2174
			DidNotExist,
			// Token: 0x0400087F RID: 2175
			Uninstalled
		}
	}
}
