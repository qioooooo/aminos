using System;

namespace System.Net.Mail
{
	// Token: 0x0200068C RID: 1676
	internal enum ServerState
	{
		// Token: 0x04002FC6 RID: 12230
		Starting = 1,
		// Token: 0x04002FC7 RID: 12231
		Started,
		// Token: 0x04002FC8 RID: 12232
		Stopping,
		// Token: 0x04002FC9 RID: 12233
		Stopped,
		// Token: 0x04002FCA RID: 12234
		Pausing,
		// Token: 0x04002FCB RID: 12235
		Paused,
		// Token: 0x04002FCC RID: 12236
		Continuing
	}
}
