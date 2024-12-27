using System;

namespace System.EnterpriseServices.CompensatingResourceManager
{
	// Token: 0x020000AA RID: 170
	internal interface IFormatLogRecords
	{
		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000401 RID: 1025
		int ColumnCount { get; }

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000402 RID: 1026
		string[] ColumnHeaders { get; }

		// Token: 0x06000403 RID: 1027
		string[] Format(LogRecord r);
	}
}
