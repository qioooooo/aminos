using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020000FF RID: 255
	internal struct StoreOperationPinDeployment
	{
		// Token: 0x060003DB RID: 987 RVA: 0x0000819B File Offset: 0x0000719B
		public StoreOperationPinDeployment(IDefinitionAppId AppId, StoreApplicationReference Ref)
		{
			this.Size = (uint)Marshal.SizeOf(typeof(StoreOperationPinDeployment));
			this.Flags = StoreOperationPinDeployment.OpFlags.NeverExpires;
			this.Application = AppId;
			this.Reference = Ref.ToIntPtr();
			this.ExpirationTime = 0L;
		}

		// Token: 0x060003DC RID: 988 RVA: 0x000081D5 File Offset: 0x000071D5
		public StoreOperationPinDeployment(IDefinitionAppId AppId, DateTime Expiry, StoreApplicationReference Ref)
		{
			this = new StoreOperationPinDeployment(AppId, Ref);
			this.Flags |= StoreOperationPinDeployment.OpFlags.NeverExpires;
		}

		// Token: 0x060003DD RID: 989 RVA: 0x000081ED File Offset: 0x000071ED
		public void Destroy()
		{
			StoreApplicationReference.Destroy(this.Reference);
		}

		// Token: 0x04000DCB RID: 3531
		[MarshalAs(UnmanagedType.U4)]
		public uint Size;

		// Token: 0x04000DCC RID: 3532
		[MarshalAs(UnmanagedType.U4)]
		public StoreOperationPinDeployment.OpFlags Flags;

		// Token: 0x04000DCD RID: 3533
		[MarshalAs(UnmanagedType.Interface)]
		public IDefinitionAppId Application;

		// Token: 0x04000DCE RID: 3534
		[MarshalAs(UnmanagedType.I8)]
		public long ExpirationTime;

		// Token: 0x04000DCF RID: 3535
		public IntPtr Reference;

		// Token: 0x02000100 RID: 256
		[Flags]
		public enum OpFlags
		{
			// Token: 0x04000DD1 RID: 3537
			Nothing = 0,
			// Token: 0x04000DD2 RID: 3538
			NeverExpires = 1
		}

		// Token: 0x02000101 RID: 257
		public enum Disposition
		{
			// Token: 0x04000DD4 RID: 3540
			Failed,
			// Token: 0x04000DD5 RID: 3541
			Pinned
		}
	}
}
