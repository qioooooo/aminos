using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x0200013C RID: 316
	internal struct StoreOperationScavenge
	{
		// Token: 0x060006D8 RID: 1752 RVA: 0x0001FC40 File Offset: 0x0001EC40
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

		// Token: 0x060006D9 RID: 1753 RVA: 0x0001FCC8 File Offset: 0x0001ECC8
		public StoreOperationScavenge(bool Light)
		{
			this = new StoreOperationScavenge(Light, 0UL, 0UL, 0U);
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x0001FCD6 File Offset: 0x0001ECD6
		public void Destroy()
		{
		}

		// Token: 0x0400057F RID: 1407
		[MarshalAs(UnmanagedType.U4)]
		public uint Size;

		// Token: 0x04000580 RID: 1408
		[MarshalAs(UnmanagedType.U4)]
		public StoreOperationScavenge.OpFlags Flags;

		// Token: 0x04000581 RID: 1409
		[MarshalAs(UnmanagedType.U8)]
		public ulong SizeReclaimationLimit;

		// Token: 0x04000582 RID: 1410
		[MarshalAs(UnmanagedType.U8)]
		public ulong RuntimeLimit;

		// Token: 0x04000583 RID: 1411
		[MarshalAs(UnmanagedType.U4)]
		public uint ComponentCountLimit;

		// Token: 0x0200013D RID: 317
		[Flags]
		public enum OpFlags
		{
			// Token: 0x04000585 RID: 1413
			Nothing = 0,
			// Token: 0x04000586 RID: 1414
			Light = 1,
			// Token: 0x04000587 RID: 1415
			LimitSize = 2,
			// Token: 0x04000588 RID: 1416
			LimitTime = 4,
			// Token: 0x04000589 RID: 1417
			LimitCount = 8
		}
	}
}
