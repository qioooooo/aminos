using System;

namespace System.Data
{
	// Token: 0x020000A8 RID: 168
	internal interface IFilter
	{
		// Token: 0x06000BA7 RID: 2983
		bool Invoke(DataRow row, DataRowVersion version);
	}
}
