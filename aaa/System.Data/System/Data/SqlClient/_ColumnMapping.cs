using System;

namespace System.Data.SqlClient
{
	// Token: 0x020002B0 RID: 688
	internal sealed class _ColumnMapping
	{
		// Token: 0x06002309 RID: 8969 RVA: 0x0026F8A4 File Offset: 0x0026ECA4
		internal _ColumnMapping(int columnId, _SqlMetaData metadata)
		{
			this._sourceColumnOrdinal = columnId;
			this._metadata = metadata;
		}

		// Token: 0x040016B2 RID: 5810
		internal int _sourceColumnOrdinal;

		// Token: 0x040016B3 RID: 5811
		internal _SqlMetaData _metadata;
	}
}
