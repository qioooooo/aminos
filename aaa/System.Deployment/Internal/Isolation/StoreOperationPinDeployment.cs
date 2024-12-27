using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x0200012A RID: 298
	internal struct StoreOperationPinDeployment
	{
		// Token: 0x060006C5 RID: 1733 RVA: 0x0001F853 File Offset: 0x0001E853
		public StoreOperationPinDeployment(IDefinitionAppId AppId, StoreApplicationReference Ref)
		{
			this.Size = (uint)Marshal.SizeOf(typeof(StoreOperationPinDeployment));
			this.Flags = StoreOperationPinDeployment.OpFlags.NeverExpires;
			this.Application = AppId;
			this.Reference = Ref.ToIntPtr();
			this.ExpirationTime = 0L;
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x0001F88D File Offset: 0x0001E88D
		public StoreOperationPinDeployment(IDefinitionAppId AppId, DateTime Expiry, StoreApplicationReference Ref)
		{
			this = new StoreOperationPinDeployment(AppId, Ref);
			this.Flags |= StoreOperationPinDeployment.OpFlags.NeverExpires;
		}

		// Token: 0x060006C7 RID: 1735 RVA: 0x0001F8A5 File Offset: 0x0001E8A5
		public void Destroy()
		{
			StoreApplicationReference.Destroy(this.Reference);
		}

		// Token: 0x0400053F RID: 1343
		[MarshalAs(UnmanagedType.U4)]
		public uint Size;

		// Token: 0x04000540 RID: 1344
		[MarshalAs(UnmanagedType.U4)]
		public StoreOperationPinDeployment.OpFlags Flags;

		// Token: 0x04000541 RID: 1345
		[MarshalAs(UnmanagedType.Interface)]
		public IDefinitionAppId Application;

		// Token: 0x04000542 RID: 1346
		[MarshalAs(UnmanagedType.I8)]
		public long ExpirationTime;

		// Token: 0x04000543 RID: 1347
		public IntPtr Reference;

		// Token: 0x0200012B RID: 299
		[Flags]
		public enum OpFlags
		{
			// Token: 0x04000545 RID: 1349
			Nothing = 0,
			// Token: 0x04000546 RID: 1350
			NeverExpires = 1
		}

		// Token: 0x0200012C RID: 300
		public enum Disposition
		{
			// Token: 0x04000548 RID: 1352
			Failed,
			// Token: 0x04000549 RID: 1353
			Pinned
		}
	}
}
