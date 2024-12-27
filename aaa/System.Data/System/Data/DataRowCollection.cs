using System;
using System.Collections;

namespace System.Data
{
	// Token: 0x02000086 RID: 134
	public sealed class DataRowCollection : InternalDataCollectionBase
	{
		// Token: 0x060007DC RID: 2012 RVA: 0x001E142C File Offset: 0x001E082C
		internal DataRowCollection(DataTable table)
		{
			this.table = table;
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060007DD RID: 2013 RVA: 0x001E1454 File Offset: 0x001E0854
		public override int Count
		{
			get
			{
				return this.list.Count;
			}
		}

		// Token: 0x170000F7 RID: 247
		public DataRow this[int index]
		{
			get
			{
				return this.list[index];
			}
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x001E1488 File Offset: 0x001E0888
		public void Add(DataRow row)
		{
			this.table.AddRow(row, -1);
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x001E14A4 File Offset: 0x001E08A4
		public void InsertAt(DataRow row, int pos)
		{
			if (pos < 0)
			{
				throw ExceptionBuilder.RowInsertOutOfRange(pos);
			}
			if (pos >= this.list.Count)
			{
				this.table.AddRow(row, -1);
				return;
			}
			this.table.InsertRow(row, -1, pos);
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x001E14E8 File Offset: 0x001E08E8
		internal void DiffInsertAt(DataRow row, int pos)
		{
			if (pos < 0 || pos == this.list.Count)
			{
				this.table.AddRow(row, (pos > -1) ? (pos + 1) : (-1));
				return;
			}
			if (this.table.NestedParentRelations.Length <= 0)
			{
				this.table.InsertRow(row, pos + 1, (pos > this.list.Count) ? (-1) : pos);
				return;
			}
			if (pos >= this.list.Count)
			{
				while (pos > this.list.Count)
				{
					this.list.Add(null);
					this.nullInList++;
				}
				this.table.AddRow(row, pos + 1);
				return;
			}
			if (this.list[pos] != null)
			{
				throw ExceptionBuilder.RowInsertTwice(pos, this.table.TableName);
			}
			this.list.RemoveAt(pos);
			this.nullInList--;
			this.table.InsertRow(row, pos + 1, pos);
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x001E15E4 File Offset: 0x001E09E4
		public int IndexOf(DataRow row)
		{
			if (row == null || row.Table != this.table || (row.RBTreeNodeId == 0 && row.RowState == DataRowState.Detached))
			{
				return -1;
			}
			return this.list.IndexOf(row.RBTreeNodeId, row);
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x001E1628 File Offset: 0x001E0A28
		internal DataRow AddWithColumnEvents(params object[] values)
		{
			DataRow dataRow = this.table.NewRow(-1);
			dataRow.ItemArray = values;
			this.table.AddRow(dataRow, -1);
			return dataRow;
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x001E1658 File Offset: 0x001E0A58
		public DataRow Add(params object[] values)
		{
			int num = this.table.NewRecordFromArray(values);
			DataRow dataRow = this.table.NewRow(num);
			this.table.AddRow(dataRow, -1);
			return dataRow;
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x001E1690 File Offset: 0x001E0A90
		internal void ArrayAdd(DataRow row)
		{
			row.RBTreeNodeId = this.list.Add(row);
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x001E16B0 File Offset: 0x001E0AB0
		internal void ArrayInsert(DataRow row, int pos)
		{
			row.RBTreeNodeId = this.list.Insert(pos, row);
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x001E16D0 File Offset: 0x001E0AD0
		internal void ArrayClear()
		{
			this.list.Clear();
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x001E16E8 File Offset: 0x001E0AE8
		internal void ArrayRemove(DataRow row)
		{
			if (row.RBTreeNodeId == 0)
			{
				throw ExceptionBuilder.InternalRBTreeError(RBTreeError.AttachedNodeWithZerorbTreeNodeId);
			}
			this.list.RBDelete(row.RBTreeNodeId);
			row.RBTreeNodeId = 0;
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x001E1720 File Offset: 0x001E0B20
		public DataRow Find(object key)
		{
			return this.table.FindByPrimaryKey(key);
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x001E173C File Offset: 0x001E0B3C
		public DataRow Find(object[] keys)
		{
			return this.table.FindByPrimaryKey(keys);
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x001E1758 File Offset: 0x001E0B58
		public void Clear()
		{
			this.table.Clear(false);
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x001E1774 File Offset: 0x001E0B74
		public bool Contains(object key)
		{
			return this.table.FindByPrimaryKey(key) != null;
		}

		// Token: 0x060007ED RID: 2029 RVA: 0x001E1794 File Offset: 0x001E0B94
		public bool Contains(object[] keys)
		{
			return this.table.FindByPrimaryKey(keys) != null;
		}

		// Token: 0x060007EE RID: 2030 RVA: 0x001E17B4 File Offset: 0x001E0BB4
		public override void CopyTo(Array ar, int index)
		{
			this.list.CopyTo(ar, index);
		}

		// Token: 0x060007EF RID: 2031 RVA: 0x001E17D0 File Offset: 0x001E0BD0
		public void CopyTo(DataRow[] array, int index)
		{
			this.list.CopyTo(array, index);
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x001E17EC File Offset: 0x001E0BEC
		public override IEnumerator GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x001E1804 File Offset: 0x001E0C04
		public void Remove(DataRow row)
		{
			if (row == null || row.Table != this.table || -1L == row.rowID)
			{
				throw ExceptionBuilder.RowOutOfRange();
			}
			if (row.RowState != DataRowState.Deleted && row.RowState != DataRowState.Detached)
			{
				row.Delete();
			}
			if (row.RowState != DataRowState.Detached)
			{
				row.AcceptChanges();
			}
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x001E185C File Offset: 0x001E0C5C
		public void RemoveAt(int index)
		{
			this.Remove(this[index]);
		}

		// Token: 0x04000752 RID: 1874
		private readonly DataTable table;

		// Token: 0x04000753 RID: 1875
		private readonly DataRowCollection.DataRowTree list = new DataRowCollection.DataRowTree();

		// Token: 0x04000754 RID: 1876
		internal int nullInList;

		// Token: 0x0200008D RID: 141
		private sealed class DataRowTree : RBTree<DataRow>
		{
			// Token: 0x06000842 RID: 2114 RVA: 0x001E3AB0 File Offset: 0x001E2EB0
			internal DataRowTree()
				: base(TreeAccessMethod.INDEX_ONLY)
			{
			}

			// Token: 0x06000843 RID: 2115 RVA: 0x001E3AC4 File Offset: 0x001E2EC4
			protected override int CompareNode(DataRow record1, DataRow record2)
			{
				throw ExceptionBuilder.InternalRBTreeError(RBTreeError.CompareNodeInDataRowTree);
			}

			// Token: 0x06000844 RID: 2116 RVA: 0x001E3AD8 File Offset: 0x001E2ED8
			protected override int CompareSateliteTreeNode(DataRow record1, DataRow record2)
			{
				throw ExceptionBuilder.InternalRBTreeError(RBTreeError.CompareSateliteTreeNodeInDataRowTree);
			}
		}
	}
}
