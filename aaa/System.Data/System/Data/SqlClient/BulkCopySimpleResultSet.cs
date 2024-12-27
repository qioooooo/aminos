using System;
using System.Collections;

namespace System.Data.SqlClient
{
	// Token: 0x020002B3 RID: 691
	internal sealed class BulkCopySimpleResultSet
	{
		// Token: 0x06002312 RID: 8978 RVA: 0x0026F9A4 File Offset: 0x0026EDA4
		internal BulkCopySimpleResultSet()
		{
			this._results = new ArrayList();
		}

		// Token: 0x17000532 RID: 1330
		internal Result this[int idx]
		{
			get
			{
				return (Result)this._results[idx];
			}
		}

		// Token: 0x06002314 RID: 8980 RVA: 0x0026F9E4 File Offset: 0x0026EDE4
		internal void SetMetaData(_SqlMetaDataSet metadata)
		{
			this.resultSet = new Result(metadata);
			this._results.Add(this.resultSet);
			this.indexmap = new int[this.resultSet.MetaData.Length];
			for (int i = 0; i < this.indexmap.Length; i++)
			{
				this.indexmap[i] = i;
			}
		}

		// Token: 0x06002315 RID: 8981 RVA: 0x0026FA48 File Offset: 0x0026EE48
		internal int[] CreateIndexMap()
		{
			return this.indexmap;
		}

		// Token: 0x06002316 RID: 8982 RVA: 0x0026FA5C File Offset: 0x0026EE5C
		internal object[] CreateRowBuffer()
		{
			Row row = new Row(this.resultSet.MetaData.Length);
			this.resultSet.AddRow(row);
			return row.DataFields;
		}

		// Token: 0x040016B7 RID: 5815
		private ArrayList _results;

		// Token: 0x040016B8 RID: 5816
		private Result resultSet;

		// Token: 0x040016B9 RID: 5817
		private int[] indexmap;
	}
}
