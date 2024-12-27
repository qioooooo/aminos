using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000102 RID: 258
	internal struct StoreOperationUnpinDeployment
	{
		// Token: 0x060003DE RID: 990 RVA: 0x000081FA File Offset: 0x000071FA
		public StoreOperationUnpinDeployment(IDefinitionAppId app, StoreApplicationReference reference)
		{
			this.Size = (uint)Marshal.SizeOf(typeof(StoreOperationUnpinDeployment));
			this.Flags = StoreOperationUnpinDeployment.OpFlags.Nothing;
			this.Application = app;
			this.Reference = reference.ToIntPtr();
		}

		// Token: 0x060003DF RID: 991 RVA: 0x0000822C File Offset: 0x0000722C
		public void Destroy()
		{
			StoreApplicationReference.Destroy(this.Reference);
		}

		// Token: 0x04000DD6 RID: 3542
		[MarshalAs(UnmanagedType.U4)]
		public uint Size;

		// Token: 0x04000DD7 RID: 3543
		[MarshalAs(UnmanagedType.U4)]
		public StoreOperationUnpinDeployment.OpFlags Flags;

		// Token: 0x04000DD8 RID: 3544
		[MarshalAs(UnmanagedType.Interface)]
		public IDefinitionAppId Application;

		// Token: 0x04000DD9 RID: 3545
		public IntPtr Reference;

		// Token: 0x02000103 RID: 259
		[Flags]
		public enum OpFlags
		{
			// Token: 0x04000DDB RID: 3547
			Nothing = 0
		}

		// Token: 0x02000104 RID: 260
		public enum Disposition
		{
			// Token: 0x04000DDD RID: 3549
			Failed,
			// Token: 0x04000DDE RID: 3550
			Unpinned
		}
	}
}
