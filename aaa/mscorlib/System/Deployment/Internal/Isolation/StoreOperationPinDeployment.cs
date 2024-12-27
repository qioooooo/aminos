using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000204 RID: 516
	internal struct StoreOperationPinDeployment
	{
		// Token: 0x0600155D RID: 5469 RVA: 0x00036F67 File Offset: 0x00035F67
		public StoreOperationPinDeployment(IDefinitionAppId AppId, StoreApplicationReference Ref)
		{
			this.Size = (uint)Marshal.SizeOf(typeof(StoreOperationPinDeployment));
			this.Flags = StoreOperationPinDeployment.OpFlags.NeverExpires;
			this.Application = AppId;
			this.Reference = Ref.ToIntPtr();
			this.ExpirationTime = 0L;
		}

		// Token: 0x0600155E RID: 5470 RVA: 0x00036FA1 File Offset: 0x00035FA1
		public StoreOperationPinDeployment(IDefinitionAppId AppId, DateTime Expiry, StoreApplicationReference Ref)
		{
			this = new StoreOperationPinDeployment(AppId, Ref);
			this.Flags |= StoreOperationPinDeployment.OpFlags.NeverExpires;
		}

		// Token: 0x0600155F RID: 5471 RVA: 0x00036FB9 File Offset: 0x00035FB9
		public void Destroy()
		{
			StoreApplicationReference.Destroy(this.Reference);
		}

		// Token: 0x04000857 RID: 2135
		[MarshalAs(UnmanagedType.U4)]
		public uint Size;

		// Token: 0x04000858 RID: 2136
		[MarshalAs(UnmanagedType.U4)]
		public StoreOperationPinDeployment.OpFlags Flags;

		// Token: 0x04000859 RID: 2137
		[MarshalAs(UnmanagedType.Interface)]
		public IDefinitionAppId Application;

		// Token: 0x0400085A RID: 2138
		[MarshalAs(UnmanagedType.I8)]
		public long ExpirationTime;

		// Token: 0x0400085B RID: 2139
		public IntPtr Reference;

		// Token: 0x02000205 RID: 517
		[Flags]
		public enum OpFlags
		{
			// Token: 0x0400085D RID: 2141
			Nothing = 0,
			// Token: 0x0400085E RID: 2142
			NeverExpires = 1
		}

		// Token: 0x02000206 RID: 518
		public enum Disposition
		{
			// Token: 0x04000860 RID: 2144
			Failed,
			// Token: 0x04000861 RID: 2145
			Pinned
		}
	}
}
