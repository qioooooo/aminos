using System;
using System.Collections;

namespace System.Data
{
	// Token: 0x020000B8 RID: 184
	public interface IColumnMappingCollection : IList, ICollection, IEnumerable
	{
		// Token: 0x170001C6 RID: 454
		object this[string index] { get; set; }

		// Token: 0x06000C64 RID: 3172
		IColumnMapping Add(string sourceColumnName, string dataSetColumnName);

		// Token: 0x06000C65 RID: 3173
		bool Contains(string sourceColumnName);

		// Token: 0x06000C66 RID: 3174
		IColumnMapping GetByDataSetColumn(string dataSetColumnName);

		// Token: 0x06000C67 RID: 3175
		int IndexOf(string sourceColumnName);

		// Token: 0x06000C68 RID: 3176
		void RemoveAt(string sourceColumnName);
	}
}
