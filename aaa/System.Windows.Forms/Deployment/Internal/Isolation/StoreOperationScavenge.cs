using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000111 RID: 273
	internal struct StoreOperationScavenge
	{
		// Token: 0x060003EE RID: 1006 RVA: 0x00008588 File Offset: 0x00007588
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

		// Token: 0x060003EF RID: 1007 RVA: 0x00008610 File Offset: 0x00007610
		public StoreOperationScavenge(bool Light)
		{
			this = new StoreOperationScavenge(Light, 0UL, 0UL, 0U);
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x0000861E File Offset: 0x0000761E
		public void Destroy()
		{
		}

		// Token: 0x04000E0B RID: 3595
		[MarshalAs(UnmanagedType.U4)]
		public uint Size;

		// Token: 0x04000E0C RID: 3596
		[MarshalAs(UnmanagedType.U4)]
		public StoreOperationScavenge.OpFlags Flags;

		// Token: 0x04000E0D RID: 3597
		[MarshalAs(UnmanagedType.U8)]
		public ulong SizeReclaimationLimit;

		// Token: 0x04000E0E RID: 3598
		[MarshalAs(UnmanagedType.U8)]
		public ulong RuntimeLimit;

		// Token: 0x04000E0F RID: 3599
		[MarshalAs(UnmanagedType.U4)]
		public uint ComponentCountLimit;

		// Token: 0x02000112 RID: 274
		[Flags]
		public enum OpFlags
		{
			// Token: 0x04000E11 RID: 3601
			Nothing = 0,
			// Token: 0x04000E12 RID: 3602
			Light = 1,
			// Token: 0x04000E13 RID: 3603
			LimitSize = 2,
			// Token: 0x04000E14 RID: 3604
			LimitTime = 4,
			// Token: 0x04000E15 RID: 3605
			LimitCount = 8
		}
	}
}
