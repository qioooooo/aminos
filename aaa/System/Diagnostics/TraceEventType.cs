using System;
using System.ComponentModel;

namespace System.Diagnostics
{
	// Token: 0x020001D7 RID: 471
	public enum TraceEventType
	{
		// Token: 0x04000F1A RID: 3866
		Critical = 1,
		// Token: 0x04000F1B RID: 3867
		Error,
		// Token: 0x04000F1C RID: 3868
		Warning = 4,
		// Token: 0x04000F1D RID: 3869
		Information = 8,
		// Token: 0x04000F1E RID: 3870
		Verbose = 16,
		// Token: 0x04000F1F RID: 3871
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		Start = 256,
		// Token: 0x04000F20 RID: 3872
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		Stop = 512,
		// Token: 0x04000F21 RID: 3873
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		Suspend = 1024,
		// Token: 0x04000F22 RID: 3874
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		Resume = 2048,
		// Token: 0x04000F23 RID: 3875
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		Transfer = 4096
	}
}
