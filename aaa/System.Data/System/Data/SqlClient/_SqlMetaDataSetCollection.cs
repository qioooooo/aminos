using System;
using System.Collections.Generic;

namespace System.Data.SqlClient
{
	// Token: 0x0200032B RID: 811
	internal sealed class _SqlMetaDataSetCollection
	{
		// Token: 0x06002A7B RID: 10875 RVA: 0x0029CE78 File Offset: 0x0029C278
		internal _SqlMetaDataSetCollection()
		{
			this.altMetaDataSetArray = new List<_SqlMetaDataSet>();
		}

		// Token: 0x06002A7C RID: 10876 RVA: 0x0029CE98 File Offset: 0x0029C298
		internal void Add(_SqlMetaDataSet altMetaDataSet)
		{
			this.altMetaDataSetArray.Add(altMetaDataSet);
		}

		// Token: 0x170006EE RID: 1774
		internal _SqlMetaDataSet this[int id]
		{
			get
			{
				foreach (_SqlMetaDataSet sqlMetaDataSet in this.altMetaDataSetArray)
				{
					if ((int)sqlMetaDataSet.id == id)
					{
						return sqlMetaDataSet;
					}
				}
				return null;
			}
		}

		// Token: 0x04001BD8 RID: 7128
		private readonly List<_SqlMetaDataSet> altMetaDataSetArray;

		// Token: 0x04001BD9 RID: 7129
		internal _SqlMetaDataSet metaDataSet;
	}
}
