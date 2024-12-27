using System;

namespace System.Data
{
	// Token: 0x0200007B RID: 123
	internal struct DataKey
	{
		// Token: 0x060006E9 RID: 1769 RVA: 0x001DCF14 File Offset: 0x001DC314
		internal DataKey(DataColumn[] columns, bool copyColumns)
		{
			if (columns == null)
			{
				throw ExceptionBuilder.ArgumentNull("columns");
			}
			if (columns.Length == 0)
			{
				throw ExceptionBuilder.KeyNoColumns();
			}
			if (columns.Length > 32)
			{
				throw ExceptionBuilder.KeyTooManyColumns(32);
			}
			for (int i = 0; i < columns.Length; i++)
			{
				if (columns[i] == null)
				{
					throw ExceptionBuilder.ArgumentNull("column");
				}
			}
			for (int j = 0; j < columns.Length; j++)
			{
				for (int k = 0; k < j; k++)
				{
					if (columns[j] == columns[k])
					{
						throw ExceptionBuilder.KeyDuplicateColumns(columns[j].ColumnName);
					}
				}
			}
			if (copyColumns)
			{
				this.columns = new DataColumn[columns.Length];
				for (int l = 0; l < columns.Length; l++)
				{
					this.columns[l] = columns[l];
				}
			}
			else
			{
				this.columns = columns;
			}
			this.CheckState();
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060006EA RID: 1770 RVA: 0x001DCFD0 File Offset: 0x001DC3D0
		internal DataColumn[] ColumnsReference
		{
			get
			{
				return this.columns;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060006EB RID: 1771 RVA: 0x001DCFE4 File Offset: 0x001DC3E4
		internal bool HasValue
		{
			get
			{
				return null != this.columns;
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060006EC RID: 1772 RVA: 0x001DD000 File Offset: 0x001DC400
		internal DataTable Table
		{
			get
			{
				return this.columns[0].Table;
			}
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x001DD01C File Offset: 0x001DC41C
		internal void CheckState()
		{
			DataTable table = this.columns[0].Table;
			if (table == null)
			{
				throw ExceptionBuilder.ColumnNotInAnyTable();
			}
			for (int i = 1; i < this.columns.Length; i++)
			{
				if (this.columns[i].Table == null)
				{
					throw ExceptionBuilder.ColumnNotInAnyTable();
				}
				if (this.columns[i].Table != table)
				{
					throw ExceptionBuilder.KeyTableMismatch();
				}
			}
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x001DD080 File Offset: 0x001DC480
		internal bool ColumnsEqual(DataKey key)
		{
			DataColumn[] array = this.columns;
			DataColumn[] array2 = key.columns;
			if (array == array2)
			{
				return true;
			}
			if (array == null || array2 == null)
			{
				return false;
			}
			if (array.Length != array2.Length)
			{
				return false;
			}
			for (int i = 0; i < array.Length; i++)
			{
				bool flag = false;
				for (int j = 0; j < array2.Length; j++)
				{
					if (array[i].Equals(array2[j]))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x001DD0EC File Offset: 0x001DC4EC
		internal bool ContainsColumn(DataColumn column)
		{
			for (int i = 0; i < this.columns.Length; i++)
			{
				if (column == this.columns[i])
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x001DD11C File Offset: 0x001DC51C
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x001DD13C File Offset: 0x001DC53C
		public static bool operator ==(DataKey x, DataKey y)
		{
			return x.Equals(y);
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x001DD15C File Offset: 0x001DC55C
		public static bool operator !=(DataKey x, DataKey y)
		{
			return !x.Equals(y);
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x001DD180 File Offset: 0x001DC580
		public override bool Equals(object value)
		{
			return this.Equals((DataKey)value);
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x001DD19C File Offset: 0x001DC59C
		internal bool Equals(DataKey value)
		{
			DataColumn[] array = this.columns;
			DataColumn[] array2 = value.columns;
			if (array == array2)
			{
				return true;
			}
			if (array == null || array2 == null)
			{
				return false;
			}
			if (array.Length != array2.Length)
			{
				return false;
			}
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i].Equals(array2[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x001DD1F0 File Offset: 0x001DC5F0
		internal string[] GetColumnNames()
		{
			string[] array = new string[this.columns.Length];
			for (int i = 0; i < this.columns.Length; i++)
			{
				array[i] = this.columns[i].ColumnName;
			}
			return array;
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x001DD230 File Offset: 0x001DC630
		internal IndexField[] GetIndexDesc()
		{
			IndexField[] array = new IndexField[this.columns.Length];
			for (int i = 0; i < this.columns.Length; i++)
			{
				array[i] = new IndexField(this.columns[i], false);
			}
			return array;
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x001DD27C File Offset: 0x001DC67C
		internal object[] GetKeyValues(int record)
		{
			object[] array = new object[this.columns.Length];
			for (int i = 0; i < this.columns.Length; i++)
			{
				array[i] = this.columns[i][record];
			}
			return array;
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x001DD2BC File Offset: 0x001DC6BC
		internal Index GetSortIndex()
		{
			return this.GetSortIndex(DataViewRowState.CurrentRows);
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x001DD2D4 File Offset: 0x001DC6D4
		internal Index GetSortIndex(DataViewRowState recordStates)
		{
			IndexField[] indexDesc = this.GetIndexDesc();
			return this.columns[0].Table.GetIndex(indexDesc, recordStates, null);
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x001DD300 File Offset: 0x001DC700
		internal bool RecordsEqual(int record1, int record2)
		{
			for (int i = 0; i < this.columns.Length; i++)
			{
				if (this.columns[i].Compare(record1, record2) != 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x001DD334 File Offset: 0x001DC734
		internal DataColumn[] ToArray()
		{
			DataColumn[] array = new DataColumn[this.columns.Length];
			for (int i = 0; i < this.columns.Length; i++)
			{
				array[i] = this.columns[i];
			}
			return array;
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x001DD370 File Offset: 0x001DC770
		internal static int ColumnOrder(int indexDesc)
		{
			return indexDesc & 65535;
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x001DD384 File Offset: 0x001DC784
		internal static bool SortDecending(int indexDesc)
		{
			return (indexDesc & int.MinValue) != 0;
		}

		// Token: 0x0400070F RID: 1807
		internal const int COLUMN = 65535;

		// Token: 0x04000710 RID: 1808
		internal const int DESCENDING = -2147483648;

		// Token: 0x04000711 RID: 1809
		private const int maxColumns = 32;

		// Token: 0x04000712 RID: 1810
		private readonly DataColumn[] columns;
	}
}
