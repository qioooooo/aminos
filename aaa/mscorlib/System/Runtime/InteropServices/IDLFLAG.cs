using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200053C RID: 1340
	[Flags]
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.IDLFLAG instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[Serializable]
	public enum IDLFLAG : short
	{
		// Token: 0x04001A51 RID: 6737
		IDLFLAG_NONE = 0,
		// Token: 0x04001A52 RID: 6738
		IDLFLAG_FIN = 1,
		// Token: 0x04001A53 RID: 6739
		IDLFLAG_FOUT = 2,
		// Token: 0x04001A54 RID: 6740
		IDLFLAG_FLCID = 4,
		// Token: 0x04001A55 RID: 6741
		IDLFLAG_FRETVAL = 8
	}
}
