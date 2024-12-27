using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020002F7 RID: 759
	[ComVisible(true)]
	[Flags]
	[Serializable]
	public enum EventAttributes
	{
		// Token: 0x04000AF9 RID: 2809
		None = 0,
		// Token: 0x04000AFA RID: 2810
		SpecialName = 512,
		// Token: 0x04000AFB RID: 2811
		ReservedMask = 1024,
		// Token: 0x04000AFC RID: 2812
		RTSpecialName = 1024
	}
}
