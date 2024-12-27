using System;
using System.Runtime.InteropServices;

namespace System.Xml.Schema
{
	// Token: 0x0200021B RID: 539
	[StructLayout(LayoutKind.Explicit)]
	internal struct StateUnion
	{
		// Token: 0x04001010 RID: 4112
		[FieldOffset(0)]
		public int State;

		// Token: 0x04001011 RID: 4113
		[FieldOffset(0)]
		public int AllElementsRequired;

		// Token: 0x04001012 RID: 4114
		[FieldOffset(0)]
		public int CurPosIndex;

		// Token: 0x04001013 RID: 4115
		[FieldOffset(0)]
		public int NumberOfRunningPos;
	}
}
