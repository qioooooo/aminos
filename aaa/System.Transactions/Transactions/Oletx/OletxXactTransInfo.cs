using System;
using System.Runtime.InteropServices;

namespace System.Transactions.Oletx
{
	// Token: 0x0200007F RID: 127
	[ComVisible(false)]
	internal struct OletxXactTransInfo
	{
		// Token: 0x0600034F RID: 847 RVA: 0x00033E90 File Offset: 0x00033290
		internal OletxXactTransInfo(Guid guid, OletxTransactionIsolationLevel isoLevel)
		{
			this.uow = guid;
			this.isoLevel = isoLevel;
			this.isoFlags = OletxTransactionIsoFlags.ISOFLAG_NONE;
			this.grfTCSupported = 0;
			this.grfRMSupported = 0;
			this.grfTCSupportedRetaining = 0;
			this.grfRMSupportedRetaining = 0;
		}

		// Token: 0x040001BF RID: 447
		internal Guid uow;

		// Token: 0x040001C0 RID: 448
		internal OletxTransactionIsolationLevel isoLevel;

		// Token: 0x040001C1 RID: 449
		internal OletxTransactionIsoFlags isoFlags;

		// Token: 0x040001C2 RID: 450
		internal int grfTCSupported;

		// Token: 0x040001C3 RID: 451
		internal int grfRMSupported;

		// Token: 0x040001C4 RID: 452
		internal int grfTCSupportedRetaining;

		// Token: 0x040001C5 RID: 453
		internal int grfRMSupportedRetaining;
	}
}
