using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000216 RID: 534
	internal struct StoreOperationScavenge
	{
		// Token: 0x06001570 RID: 5488 RVA: 0x00037354 File Offset: 0x00036354
		public StoreOperationScavenge(bool Light, ulong SizeLimit, ulong RunLimit, uint ComponentLimit)
		{
			this.Size = (uint)Marshal.SizeOf(typeof(StoreOperationScavenge));
			this.Flags = StoreOperationScavenge.OpFlags.Nothing;
			if (Light)
			{
				this.Flags |= StoreOperationScavenge.OpFlags.Light;
			}
			this.SizeReclaimationLimit = SizeLimit;
			if (SizeLimit != 0UL)
			{
				this.Flags |= StoreOperationScavenge.OpFlags.LimitSize;
			}
			this.RuntimeLimit = RunLimit;
			if (RunLimit != 0UL)
			{
				this.Flags |= StoreOperationScavenge.OpFlags.LimitTime;
			}
			this.ComponentCountLimit = ComponentLimit;
			if (ComponentLimit != 0U)
			{
				this.Flags |= StoreOperationScavenge.OpFlags.LimitCount;
			}
		}

		// Token: 0x06001571 RID: 5489 RVA: 0x000373DC File Offset: 0x000363DC
		public StoreOperationScavenge(bool Light)
		{
			this = new StoreOperationScavenge(Light, 0UL, 0UL, 0U);
		}

		// Token: 0x06001572 RID: 5490 RVA: 0x000373EA File Offset: 0x000363EA
		public void Destroy()
		{
		}

		// Token: 0x04000897 RID: 2199
		[MarshalAs(UnmanagedType.U4)]
		public uint Size;

		// Token: 0x04000898 RID: 2200
		[MarshalAs(UnmanagedType.U4)]
		public StoreOperationScavenge.OpFlags Flags;

		// Token: 0x04000899 RID: 2201
		[MarshalAs(UnmanagedType.U8)]
		public ulong SizeReclaimationLimit;

		// Token: 0x0400089A RID: 2202
		[MarshalAs(UnmanagedType.U8)]
		public ulong RuntimeLimit;

		// Token: 0x0400089B RID: 2203
		[MarshalAs(UnmanagedType.U4)]
		public uint ComponentCountLimit;

		// Token: 0x02000217 RID: 535
		[Flags]
		public enum OpFlags
		{
			// Token: 0x0400089D RID: 2205
			Nothing = 0,
			// Token: 0x0400089E RID: 2206
			Light = 1,
			// Token: 0x0400089F RID: 2207
			LimitSize = 2,
			// Token: 0x040008A0 RID: 2208
			LimitTime = 4,
			// Token: 0x040008A1 RID: 2209
			LimitCount = 8
		}
	}
}
