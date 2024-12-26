using System;
using System.Runtime.InteropServices;

namespace GeFanuc.iFixToolkit.Adapter
{
	// Token: 0x02000005 RID: 5
	[StructLayout(LayoutKind.Explicit)]
	public struct almUnion
	{
		// Token: 0x04000015 RID: 21
		[FieldOffset(0)]
		public short bValue;

		// Token: 0x04000016 RID: 22
		[FieldOffset(0)]
		public byte ucValue;

		// Token: 0x04000017 RID: 23
		[FieldOffset(0)]
		public char cValue;

		// Token: 0x04000018 RID: 24
		[FieldOffset(0)]
		public short iValue;

		// Token: 0x04000019 RID: 25
		[FieldOffset(0)]
		public short uiValue;

		// Token: 0x0400001A RID: 26
		[FieldOffset(0)]
		public int lValue;

		// Token: 0x0400001B RID: 27
		[FieldOffset(0)]
		public int ulValue;

		// Token: 0x0400001C RID: 28
		[FieldOffset(0)]
		public float fValue;

		// Token: 0x0400001D RID: 29
		[FieldOffset(0)]
		public double dValue;
	}
}
