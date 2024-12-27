using System;
using System.Runtime.InteropServices;

namespace System.ComponentModel.Design
{
	// Token: 0x02000199 RID: 409
	[Flags]
	[ComVisible(true)]
	public enum SelectionTypes
	{
		// Token: 0x04000AF8 RID: 2808
		Auto = 1,
		// Token: 0x04000AF9 RID: 2809
		[Obsolete("This value has been deprecated. Use SelectionTypes.Auto instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		Normal = 1,
		// Token: 0x04000AFA RID: 2810
		Replace = 2,
		// Token: 0x04000AFB RID: 2811
		[Obsolete("This value has been deprecated.  It is no longer supported. http://go.microsoft.com/fwlink/?linkid=14202")]
		MouseDown = 4,
		// Token: 0x04000AFC RID: 2812
		[Obsolete("This value has been deprecated.  It is no longer supported. http://go.microsoft.com/fwlink/?linkid=14202")]
		MouseUp = 8,
		// Token: 0x04000AFD RID: 2813
		[Obsolete("This value has been deprecated. Use SelectionTypes.Primary instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		Click = 16,
		// Token: 0x04000AFE RID: 2814
		Primary = 16,
		// Token: 0x04000AFF RID: 2815
		Toggle = 32,
		// Token: 0x04000B00 RID: 2816
		Add = 64,
		// Token: 0x04000B01 RID: 2817
		Remove = 128,
		// Token: 0x04000B02 RID: 2818
		[Obsolete("This value has been deprecated. Use Enum class methods to determine valid values, or use a type converter. http://go.microsoft.com/fwlink/?linkid=14202")]
		Valid = 31
	}
}
