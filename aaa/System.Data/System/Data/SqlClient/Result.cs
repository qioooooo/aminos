using System;
using System.Collections;

namespace System.Data.SqlClient
{
	// Token: 0x020002B2 RID: 690
	internal sealed class Result
	{
		// Token: 0x0600230D RID: 8973 RVA: 0x0026F914 File Offset: 0x0026ED14
		internal Result(_SqlMetaDataSet metadata)
		{
			this._metadata = metadata;
			this._rowset = new ArrayList();
		}

		// Token: 0x1700052F RID: 1327
		// (get) Token: 0x0600230E RID: 8974 RVA: 0x0026F93C File Offset: 0x0026ED3C
		internal int Count
		{
			get
			{
				return this._rowset.Count;
			}
		}

		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x0600230F RID: 8975 RVA: 0x0026F954 File Offset: 0x0026ED54
		internal _SqlMetaDataSet MetaData
		{
			get
			{
				return this._metadata;
			}
		}

		// Token: 0x17000531 RID: 1329
		internal Row this[int index]
		{
			get
			{
				return (Row)this._rowset[index];
			}
		}

		// Token: 0x06002311 RID: 8977 RVA: 0x0026F988 File Offset: 0x0026ED88
		internal void AddRow(Row row)
		{
			this._rowset.Add(row);
		}

		// Token: 0x040016B5 RID: 5813
		private _SqlMetaDataSet _metadata;

		// Token: 0x040016B6 RID: 5814
		private ArrayList _rowset;
	}
}
