using System;

namespace System.Data.SqlClient
{
	// Token: 0x0200032A RID: 810
	internal sealed class _SqlMetaDataSet
	{
		// Token: 0x06002A77 RID: 10871 RVA: 0x0029CDF0 File Offset: 0x0029C1F0
		internal _SqlMetaDataSet(int count)
		{
			this.metaDataArray = new _SqlMetaData[count];
			for (int i = 0; i < this.metaDataArray.Length; i++)
			{
				this.metaDataArray[i] = new _SqlMetaData(i);
			}
		}

		// Token: 0x170006EC RID: 1772
		// (get) Token: 0x06002A78 RID: 10872 RVA: 0x0029CE30 File Offset: 0x0029C230
		internal int Length
		{
			get
			{
				return this.metaDataArray.Length;
			}
		}

		// Token: 0x170006ED RID: 1773
		internal _SqlMetaData this[int index]
		{
			get
			{
				return this.metaDataArray[index];
			}
			set
			{
				this.metaDataArray[index] = value;
			}
		}

		// Token: 0x04001BD3 RID: 7123
		internal ushort id;

		// Token: 0x04001BD4 RID: 7124
		internal int[] indexMap;

		// Token: 0x04001BD5 RID: 7125
		internal int visibleColumns;

		// Token: 0x04001BD6 RID: 7126
		internal DataTable schemaTable;

		// Token: 0x04001BD7 RID: 7127
		private readonly _SqlMetaData[] metaDataArray;
	}
}
