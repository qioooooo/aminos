using System;

namespace GeFanuc.iFixToolkit.Adapter
{
	// Token: 0x02000006 RID: 6
	[Serializable]
	public struct VSP
	{
		// Token: 0x0400001E RID: 30
		public byte mach_data;

		// Token: 0x0400001F RID: 31
		public byte type;

		// Token: 0x04000020 RID: 32
		public short ipn;

		// Token: 0x04000021 RID: 33
		public short field;

		// Token: 0x04000022 RID: 34
		public short index;

		// Token: 0x04000023 RID: 35
		public short size;
	}
}
