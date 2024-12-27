using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x0200012D RID: 301
	internal struct StoreOperationUnpinDeployment
	{
		// Token: 0x060006C8 RID: 1736 RVA: 0x0001F8B2 File Offset: 0x0001E8B2
		public StoreOperationUnpinDeployment(IDefinitionAppId app, StoreApplicationReference reference)
		{
			this.Size = (uint)Marshal.SizeOf(typeof(StoreOperationUnpinDeployment));
			this.Flags = StoreOperationUnpinDeployment.OpFlags.Nothing;
			this.Application = app;
			this.Reference = reference.ToIntPtr();
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x0001F8E4 File Offset: 0x0001E8E4
		public void Destroy()
		{
			StoreApplicationReference.Destroy(this.Reference);
		}

		// Token: 0x0400054A RID: 1354
		[MarshalAs(UnmanagedType.U4)]
		public uint Size;

		// Token: 0x0400054B RID: 1355
		[MarshalAs(UnmanagedType.U4)]
		public StoreOperationUnpinDeployment.OpFlags Flags;

		// Token: 0x0400054C RID: 1356
		[MarshalAs(UnmanagedType.Interface)]
		public IDefinitionAppId Application;

		// Token: 0x0400054D RID: 1357
		public IntPtr Reference;

		// Token: 0x0200012E RID: 302
		[Flags]
		public enum OpFlags
		{
			// Token: 0x0400054F RID: 1359
			Nothing = 0
		}

		// Token: 0x0200012F RID: 303
		public enum Disposition
		{
			// Token: 0x04000551 RID: 1361
			Failed,
			// Token: 0x04000552 RID: 1362
			Unpinned
		}
	}
}
