using System;

namespace System.Data
{
	// Token: 0x020000BF RID: 191
	public interface IDbDataParameter : IDataParameter
	{
		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06000CAB RID: 3243
		// (set) Token: 0x06000CAC RID: 3244
		byte Precision { get; set; }

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000CAD RID: 3245
		// (set) Token: 0x06000CAE RID: 3246
		byte Scale { get; set; }

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000CAF RID: 3247
		// (set) Token: 0x06000CB0 RID: 3248
		int Size { get; set; }
	}
}
