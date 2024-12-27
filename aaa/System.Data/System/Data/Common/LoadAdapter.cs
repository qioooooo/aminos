using System;

namespace System.Data.Common
{
	// Token: 0x02000118 RID: 280
	internal sealed class LoadAdapter : DataAdapter
	{
		// Token: 0x060011ED RID: 4589 RVA: 0x0021DB94 File Offset: 0x0021CF94
		internal LoadAdapter()
		{
		}

		// Token: 0x060011EE RID: 4590 RVA: 0x0021DBA8 File Offset: 0x0021CFA8
		internal int FillFromReader(DataTable[] dataTables, IDataReader dataReader, int startRecord, int maxRecords)
		{
			return this.Fill(dataTables, dataReader, startRecord, maxRecords);
		}
	}
}
