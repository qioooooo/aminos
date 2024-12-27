using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000207 RID: 519
	internal struct StoreOperationUnpinDeployment
	{
		// Token: 0x06001560 RID: 5472 RVA: 0x00036FC6 File Offset: 0x00035FC6
		public StoreOperationUnpinDeployment(IDefinitionAppId app, StoreApplicationReference reference)
		{
			this.Size = (uint)Marshal.SizeOf(typeof(StoreOperationUnpinDeployment));
			this.Flags = StoreOperationUnpinDeployment.OpFlags.Nothing;
			this.Application = app;
			this.Reference = reference.ToIntPtr();
		}

		// Token: 0x06001561 RID: 5473 RVA: 0x00036FF8 File Offset: 0x00035FF8
		public void Destroy()
		{
			StoreApplicationReference.Destroy(this.Reference);
		}

		// Token: 0x04000862 RID: 2146
		[MarshalAs(UnmanagedType.U4)]
		public uint Size;

		// Token: 0x04000863 RID: 2147
		[MarshalAs(UnmanagedType.U4)]
		public StoreOperationUnpinDeployment.OpFlags Flags;

		// Token: 0x04000864 RID: 2148
		[MarshalAs(UnmanagedType.Interface)]
		public IDefinitionAppId Application;

		// Token: 0x04000865 RID: 2149
		public IntPtr Reference;

		// Token: 0x02000208 RID: 520
		[Flags]
		public enum OpFlags
		{
			// Token: 0x04000867 RID: 2151
			Nothing = 0
		}

		// Token: 0x02000209 RID: 521
		public enum Disposition
		{
			// Token: 0x04000869 RID: 2153
			Failed,
			// Token: 0x0400086A RID: 2154
			Unpinned
		}
	}
}
