using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000512 RID: 1298
	[ComVisible(true)]
	[Flags]
	[Serializable]
	public enum TypeLibExporterFlags
	{
		// Token: 0x040019BD RID: 6589
		None = 0,
		// Token: 0x040019BE RID: 6590
		OnlyReferenceRegistered = 1,
		// Token: 0x040019BF RID: 6591
		CallerResolvedReferences = 2,
		// Token: 0x040019C0 RID: 6592
		OldNames = 4,
		// Token: 0x040019C1 RID: 6593
		ExportAs32Bit = 16,
		// Token: 0x040019C2 RID: 6594
		ExportAs64Bit = 32
	}
}
