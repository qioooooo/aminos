using System;

namespace System.Data
{
	// Token: 0x0200006D RID: 109
	internal sealed class DataError
	{
		// Token: 0x0600058F RID: 1423 RVA: 0x001D9CFC File Offset: 0x001D90FC
		internal DataError()
		{
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x001D9D1C File Offset: 0x001D911C
		internal DataError(string rowError)
		{
			this.SetText(rowError);
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000591 RID: 1425 RVA: 0x001D9D44 File Offset: 0x001D9144
		// (set) Token: 0x06000592 RID: 1426 RVA: 0x001D9D58 File Offset: 0x001D9158
		internal string Text
		{
			get
			{
				return this.rowError;
			}
			set
			{
				this.SetText(value);
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000593 RID: 1427 RVA: 0x001D9D6C File Offset: 0x001D916C
		internal bool HasErrors
		{
			get
			{
				return this.rowError.Length != 0 || this.count != 0;
			}
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x001D9D94 File Offset: 0x001D9194
		internal void SetColumnError(DataColumn column, string error)
		{
			if (error == null || error.Length == 0)
			{
				this.Clear(column);
				return;
			}
			if (this.errorList == null)
			{
				this.errorList = new DataError.ColumnError[1];
			}
			int num = this.IndexOf(column);
			this.errorList[num].column = column;
			this.errorList[num].error = error;
			column.errors++;
			if (num == this.count)
			{
				this.count++;
			}
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x001D9E1C File Offset: 0x001D921C
		internal string GetColumnError(DataColumn column)
		{
			for (int i = 0; i < this.count; i++)
			{
				if (this.errorList[i].column == column)
				{
					return this.errorList[i].error;
				}
			}
			return string.Empty;
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x001D9E68 File Offset: 0x001D9268
		internal void Clear(DataColumn column)
		{
			if (this.count == 0)
			{
				return;
			}
			for (int i = 0; i < this.count; i++)
			{
				if (this.errorList[i].column == column)
				{
					Array.Copy(this.errorList, i + 1, this.errorList, i, this.count - i - 1);
					this.count--;
					column.errors--;
				}
			}
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x001D9EE0 File Offset: 0x001D92E0
		internal void Clear()
		{
			for (int i = 0; i < this.count; i++)
			{
				this.errorList[i].column.errors--;
			}
			this.count = 0;
			this.rowError = string.Empty;
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x001D9F30 File Offset: 0x001D9330
		internal DataColumn[] GetColumnsInError()
		{
			DataColumn[] array = new DataColumn[this.count];
			for (int i = 0; i < this.count; i++)
			{
				array[i] = this.errorList[i].column;
			}
			return array;
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x001D9F70 File Offset: 0x001D9370
		private void SetText(string errorText)
		{
			if (errorText == null)
			{
				errorText = string.Empty;
			}
			this.rowError = errorText;
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x001D9F90 File Offset: 0x001D9390
		internal int IndexOf(DataColumn column)
		{
			for (int i = 0; i < this.count; i++)
			{
				if (this.errorList[i].column == column)
				{
					return i;
				}
			}
			if (this.count >= this.errorList.Length)
			{
				int num = Math.Min(this.count * 2, column.Table.Columns.Count);
				DataError.ColumnError[] array = new DataError.ColumnError[num];
				Array.Copy(this.errorList, 0, array, 0, this.count);
				this.errorList = array;
			}
			return this.count;
		}

		// Token: 0x04000709 RID: 1801
		internal const int initialCapacity = 1;

		// Token: 0x0400070A RID: 1802
		private string rowError = string.Empty;

		// Token: 0x0400070B RID: 1803
		private int count;

		// Token: 0x0400070C RID: 1804
		private DataError.ColumnError[] errorList;

		// Token: 0x0200006E RID: 110
		internal struct ColumnError
		{
			// Token: 0x0400070D RID: 1805
			internal DataColumn column;

			// Token: 0x0400070E RID: 1806
			internal string error;
		}
	}
}
