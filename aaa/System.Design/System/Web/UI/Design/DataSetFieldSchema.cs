using System;
using System.Data;

namespace System.Web.UI.Design
{
	// Token: 0x0200034E RID: 846
	public sealed class DataSetFieldSchema : IDataSourceFieldSchema
	{
		// Token: 0x06001FCF RID: 8143 RVA: 0x000B5A7D File Offset: 0x000B4A7D
		public DataSetFieldSchema(DataColumn column)
		{
			if (column == null)
			{
				throw new ArgumentNullException("column");
			}
			this._column = column;
		}

		// Token: 0x17000591 RID: 1425
		// (get) Token: 0x06001FD0 RID: 8144 RVA: 0x000B5A9A File Offset: 0x000B4A9A
		public Type DataType
		{
			get
			{
				return this._column.DataType;
			}
		}

		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x06001FD1 RID: 8145 RVA: 0x000B5AA7 File Offset: 0x000B4AA7
		public bool Identity
		{
			get
			{
				return this._column.AutoIncrement;
			}
		}

		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x06001FD2 RID: 8146 RVA: 0x000B5AB4 File Offset: 0x000B4AB4
		public bool IsReadOnly
		{
			get
			{
				return this._column.ReadOnly;
			}
		}

		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x06001FD3 RID: 8147 RVA: 0x000B5AC1 File Offset: 0x000B4AC1
		public bool IsUnique
		{
			get
			{
				return this._column.Unique;
			}
		}

		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x06001FD4 RID: 8148 RVA: 0x000B5ACE File Offset: 0x000B4ACE
		public int Length
		{
			get
			{
				return this._column.MaxLength;
			}
		}

		// Token: 0x17000596 RID: 1430
		// (get) Token: 0x06001FD5 RID: 8149 RVA: 0x000B5ADB File Offset: 0x000B4ADB
		public string Name
		{
			get
			{
				return this._column.ColumnName;
			}
		}

		// Token: 0x17000597 RID: 1431
		// (get) Token: 0x06001FD6 RID: 8150 RVA: 0x000B5AE8 File Offset: 0x000B4AE8
		public bool Nullable
		{
			get
			{
				return this._column.AllowDBNull;
			}
		}

		// Token: 0x17000598 RID: 1432
		// (get) Token: 0x06001FD7 RID: 8151 RVA: 0x000B5AF5 File Offset: 0x000B4AF5
		public int Precision
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x17000599 RID: 1433
		// (get) Token: 0x06001FD8 RID: 8152 RVA: 0x000B5AF8 File Offset: 0x000B4AF8
		public bool PrimaryKey
		{
			get
			{
				if (this._column.Table == null || this._column.Table.PrimaryKey == null)
				{
					return false;
				}
				foreach (DataColumn dataColumn in this._column.Table.PrimaryKey)
				{
					if (dataColumn == this._column)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x1700059A RID: 1434
		// (get) Token: 0x06001FD9 RID: 8153 RVA: 0x000B5B59 File Offset: 0x000B4B59
		public int Scale
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x040017C0 RID: 6080
		private DataColumn _column;
	}
}
