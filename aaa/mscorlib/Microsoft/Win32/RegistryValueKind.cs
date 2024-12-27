using System;
using System.Runtime.InteropServices;

namespace Microsoft.Win32
{
	// Token: 0x02000461 RID: 1121
	[ComVisible(true)]
	public enum RegistryValueKind
	{
		// Token: 0x04001741 RID: 5953
		String = 1,
		// Token: 0x04001742 RID: 5954
		ExpandString,
		// Token: 0x04001743 RID: 5955
		Binary,
		// Token: 0x04001744 RID: 5956
		DWord,
		// Token: 0x04001745 RID: 5957
		MultiString = 7,
		// Token: 0x04001746 RID: 5958
		QWord = 11,
		// Token: 0x04001747 RID: 5959
		Unknown = 0
	}
}
