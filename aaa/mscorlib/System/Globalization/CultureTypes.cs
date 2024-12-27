using System;
using System.Runtime.InteropServices;

namespace System.Globalization
{
	// Token: 0x0200037F RID: 895
	[ComVisible(true)]
	[Flags]
	[Serializable]
	public enum CultureTypes
	{
		// Token: 0x04000F47 RID: 3911
		NeutralCultures = 1,
		// Token: 0x04000F48 RID: 3912
		SpecificCultures = 2,
		// Token: 0x04000F49 RID: 3913
		InstalledWin32Cultures = 4,
		// Token: 0x04000F4A RID: 3914
		AllCultures = 7,
		// Token: 0x04000F4B RID: 3915
		UserCustomCulture = 8,
		// Token: 0x04000F4C RID: 3916
		ReplacementCultures = 16,
		// Token: 0x04000F4D RID: 3917
		WindowsOnlyCultures = 32,
		// Token: 0x04000F4E RID: 3918
		FrameworkCultures = 64
	}
}
