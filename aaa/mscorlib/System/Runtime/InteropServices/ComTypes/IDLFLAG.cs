using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x0200056F RID: 1391
	[Flags]
	[Serializable]
	public enum IDLFLAG : short
	{
		// Token: 0x04001B1B RID: 6939
		IDLFLAG_NONE = 0,
		// Token: 0x04001B1C RID: 6940
		IDLFLAG_FIN = 1,
		// Token: 0x04001B1D RID: 6941
		IDLFLAG_FOUT = 2,
		// Token: 0x04001B1E RID: 6942
		IDLFLAG_FLCID = 4,
		// Token: 0x04001B1F RID: 6943
		IDLFLAG_FRETVAL = 8
	}
}
