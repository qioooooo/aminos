using System;
using System.ComponentModel;

namespace System.Diagnostics
{
	// Token: 0x020001CE RID: 462
	[Flags]
	public enum SourceLevels
	{
		// Token: 0x04000EFC RID: 3836
		Off = 0,
		// Token: 0x04000EFD RID: 3837
		Critical = 1,
		// Token: 0x04000EFE RID: 3838
		Error = 3,
		// Token: 0x04000EFF RID: 3839
		Warning = 7,
		// Token: 0x04000F00 RID: 3840
		Information = 15,
		// Token: 0x04000F01 RID: 3841
		Verbose = 31,
		// Token: 0x04000F02 RID: 3842
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		ActivityTracing = 65280,
		// Token: 0x04000F03 RID: 3843
		All = -1
	}
}
