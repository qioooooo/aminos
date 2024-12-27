using System;

namespace System.Data.SqlClient
{
	// Token: 0x020002B1 RID: 689
	internal sealed class Row
	{
		// Token: 0x0600230A RID: 8970 RVA: 0x0026F8C8 File Offset: 0x0026ECC8
		internal Row(int rowCount)
		{
			this._dataFields = new object[rowCount];
		}

		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x0600230B RID: 8971 RVA: 0x0026F8E8 File Offset: 0x0026ECE8
		internal object[] DataFields
		{
			get
			{
				return this._dataFields;
			}
		}

		// Token: 0x1700052E RID: 1326
		internal object this[int index]
		{
			get
			{
				return this._dataFields[index];
			}
		}

		// Token: 0x040016B4 RID: 5812
		private object[] _dataFields;
	}
}
