using System;
using System.Collections;

namespace System.Data
{
	// Token: 0x020000C3 RID: 195
	public interface ITableMappingCollection : IList, ICollection, IEnumerable
	{
		// Token: 0x170001E9 RID: 489
		object this[string index] { get; set; }

		// Token: 0x06000CBC RID: 3260
		ITableMapping Add(string sourceTableName, string dataSetTableName);

		// Token: 0x06000CBD RID: 3261
		bool Contains(string sourceTableName);

		// Token: 0x06000CBE RID: 3262
		ITableMapping GetByDataSetTable(string dataSetTableName);

		// Token: 0x06000CBF RID: 3263
		int IndexOf(string sourceTableName);

		// Token: 0x06000CC0 RID: 3264
		void RemoveAt(string sourceTableName);
	}
}
