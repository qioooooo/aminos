using System;

namespace System.Data
{
	// Token: 0x020000A1 RID: 161
	public interface IDataReader : IDisposable, IDataRecord
	{
		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000AAE RID: 2734
		int Depth { get; }

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x06000AAF RID: 2735
		bool IsClosed { get; }

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000AB0 RID: 2736
		int RecordsAffected { get; }

		// Token: 0x06000AB1 RID: 2737
		void Close();

		// Token: 0x06000AB2 RID: 2738
		DataTable GetSchemaTable();

		// Token: 0x06000AB3 RID: 2739
		bool NextResult();

		// Token: 0x06000AB4 RID: 2740
		bool Read();
	}
}
