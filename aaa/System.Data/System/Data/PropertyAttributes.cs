using System;
using System.ComponentModel;

namespace System.Data
{
	// Token: 0x0200025A RID: 602
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Flags]
	[Obsolete("PropertyAttributes has been deprecated.  http://go.microsoft.com/fwlink/?linkid=14202")]
	public enum PropertyAttributes
	{
		// Token: 0x04001524 RID: 5412
		NotSupported = 0,
		// Token: 0x04001525 RID: 5413
		Required = 1,
		// Token: 0x04001526 RID: 5414
		Optional = 2,
		// Token: 0x04001527 RID: 5415
		Read = 512,
		// Token: 0x04001528 RID: 5416
		Write = 1024
	}
}
