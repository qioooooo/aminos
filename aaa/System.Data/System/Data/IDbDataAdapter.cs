using System;

namespace System.Data
{
	// Token: 0x020000BE RID: 190
	public interface IDbDataAdapter : IDataAdapter
	{
		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06000CA3 RID: 3235
		// (set) Token: 0x06000CA4 RID: 3236
		IDbCommand SelectCommand { get; set; }

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06000CA5 RID: 3237
		// (set) Token: 0x06000CA6 RID: 3238
		IDbCommand InsertCommand { get; set; }

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000CA7 RID: 3239
		// (set) Token: 0x06000CA8 RID: 3240
		IDbCommand UpdateCommand { get; set; }

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06000CA9 RID: 3241
		// (set) Token: 0x06000CAA RID: 3242
		IDbCommand DeleteCommand { get; set; }
	}
}
