using System;

namespace System.Data
{
	// Token: 0x02000069 RID: 105
	public class DataColumnChangeEventArgs : EventArgs
	{
		// Token: 0x06000542 RID: 1346 RVA: 0x001D881C File Offset: 0x001D7C1C
		internal DataColumnChangeEventArgs(DataRow row)
		{
			this._row = row;
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x001D8838 File Offset: 0x001D7C38
		public DataColumnChangeEventArgs(DataRow row, DataColumn column, object value)
		{
			this._row = row;
			this._column = column;
			this._proposedValue = value;
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000544 RID: 1348 RVA: 0x001D8860 File Offset: 0x001D7C60
		public DataColumn Column
		{
			get
			{
				return this._column;
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000545 RID: 1349 RVA: 0x001D8874 File Offset: 0x001D7C74
		public DataRow Row
		{
			get
			{
				return this._row;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000546 RID: 1350 RVA: 0x001D8888 File Offset: 0x001D7C88
		// (set) Token: 0x06000547 RID: 1351 RVA: 0x001D889C File Offset: 0x001D7C9C
		public object ProposedValue
		{
			get
			{
				return this._proposedValue;
			}
			set
			{
				this._proposedValue = value;
			}
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x001D88B0 File Offset: 0x001D7CB0
		internal void InitializeColumnChangeEvent(DataColumn column, object value)
		{
			this._column = column;
			this._proposedValue = value;
		}

		// Token: 0x040006F9 RID: 1785
		private readonly DataRow _row;

		// Token: 0x040006FA RID: 1786
		private DataColumn _column;

		// Token: 0x040006FB RID: 1787
		private object _proposedValue;
	}
}
