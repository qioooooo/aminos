using System;
using System.Runtime.InteropServices;

namespace System.ComponentModel.Design
{
	// Token: 0x020001A1 RID: 417
	[ComVisible(true)]
	public enum ViewTechnology
	{
		// Token: 0x04000EA5 RID: 3749
		[Obsolete("This value has been deprecated. Use ViewTechnology.Default instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		Passthrough,
		// Token: 0x04000EA6 RID: 3750
		[Obsolete("This value has been deprecated. Use ViewTechnology.Default instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		WindowsForms,
		// Token: 0x04000EA7 RID: 3751
		Default
	}
}
