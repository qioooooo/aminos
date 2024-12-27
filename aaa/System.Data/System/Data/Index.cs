using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace System.Data
{
	// Token: 0x020000DC RID: 220
	internal sealed class Index
	{
		// Token: 0x06000D22 RID: 3362 RVA: 0x001FE504 File Offset: 0x001FD904
		public Index(DataTable table, IndexField[] indexFields, DataViewRowState recordStates, IFilter rowFilter)
			: this(table, null, indexFields, recordStates, rowFilter)
		{
		}

		// Token: 0x06000D23 RID: 3363 RVA: 0x001FE520 File Offset: 0x001FD920
		public Index(DataTable table, int[] ndexDesc, IndexField[] indexFields, DataViewRowState recordStates, IFilter rowFilter)
			: this(table, ndexDesc, indexFields, null, recordStates, rowFilter)
		{
		}

		// Token: 0x06000D24 RID: 3364 RVA: 0x001FE53C File Offset: 0x001FD93C
		public Index(DataTable table, Comparison<DataRow> comparison, DataViewRowState recordStates, IFilter rowFilter)
			: this(table, null, Index.GetAllFields(table.Columns), comparison, recordStates, rowFilter)
		{
		}

		// Token: 0x06000D25 RID: 3365 RVA: 0x001FE560 File Offset: 0x001FD960
		private static IndexField[] GetAllFields(DataColumnCollection columns)
		{
			IndexField[] array = new IndexField[columns.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new IndexField(columns[i], false);
			}
			return array;
		}

		// Token: 0x06000D26 RID: 3366 RVA: 0x001FE5A4 File Offset: 0x001FD9A4
		private Index(DataTable table, int[] ndexDesc, IndexField[] indexFields, Comparison<DataRow> comparison, DataViewRowState recordStates, IFilter rowFilter)
		{
			Bid.Trace("<ds.Index.Index|API> %d#, table=%d, recordStates=%d{ds.DataViewRowState}\n", this.ObjectID, (table != null) ? table.ObjectID : 0, (int)recordStates);
			if ((recordStates & ~(DataViewRowState.Unchanged | DataViewRowState.Added | DataViewRowState.Deleted | DataViewRowState.ModifiedCurrent | DataViewRowState.ModifiedOriginal)) != DataViewRowState.None)
			{
				throw ExceptionBuilder.RecordStateRange();
			}
			this.table = table;
			this._listeners = new Listeners<DataViewListener>(this.ObjectID, (DataViewListener listener) => null != listener);
			this.IndexDesc = ndexDesc;
			this.IndexFields = indexFields;
			if (ndexDesc == null)
			{
				this.IndexDesc = Select.ConvertIndexFieldtoIndexDesc(indexFields);
			}
			this.recordStates = recordStates;
			this._comparison = comparison;
			DataColumnCollection columns = table.Columns;
			this.isSharable = rowFilter == null && comparison == null;
			if (rowFilter != null)
			{
				this.rowFilter = new WeakReference(rowFilter);
				DataExpression dataExpression = rowFilter as DataExpression;
				if (dataExpression != null)
				{
					this._hasRemoteAggregate = dataExpression.HasRemoteAggregate();
				}
			}
			this.InitRecords(rowFilter);
		}

		// Token: 0x06000D27 RID: 3367 RVA: 0x001FE6A0 File Offset: 0x001FDAA0
		public bool Equal(IndexField[] indexDesc, DataViewRowState recordStates, IFilter rowFilter)
		{
			if (!this.isSharable || this.IndexFields.Length != indexDesc.Length || this.recordStates != recordStates || rowFilter != null)
			{
				return false;
			}
			for (int i = 0; i < this.IndexFields.Length; i++)
			{
				if (this.IndexFields[i].Column != indexDesc[i].Column || this.IndexFields[i].IsDescending != indexDesc[i].IsDescending)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x06000D28 RID: 3368 RVA: 0x001FE738 File Offset: 0x001FDB38
		internal bool HasRemoteAggregate
		{
			get
			{
				return this._hasRemoteAggregate;
			}
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x06000D29 RID: 3369 RVA: 0x001FE74C File Offset: 0x001FDB4C
		internal int ObjectID
		{
			get
			{
				return this._objectID;
			}
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06000D2A RID: 3370 RVA: 0x001FE760 File Offset: 0x001FDB60
		public DataViewRowState RecordStates
		{
			get
			{
				return this.recordStates;
			}
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000D2B RID: 3371 RVA: 0x001FE774 File Offset: 0x001FDB74
		public IFilter RowFilter
		{
			get
			{
				return (IFilter)((this.rowFilter != null) ? this.rowFilter.Target : null);
			}
		}

		// Token: 0x06000D2C RID: 3372 RVA: 0x001FE79C File Offset: 0x001FDB9C
		public int GetRecord(int recordIndex)
		{
			return this.records[recordIndex];
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000D2D RID: 3373 RVA: 0x001FE7B8 File Offset: 0x001FDBB8
		public bool HasDuplicates
		{
			get
			{
				return this.records.HasDuplicates;
			}
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000D2E RID: 3374 RVA: 0x001FE7D0 File Offset: 0x001FDBD0
		public int RecordCount
		{
			get
			{
				return this.recordCount;
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000D2F RID: 3375 RVA: 0x001FE7E4 File Offset: 0x001FDBE4
		public bool IsSharable
		{
			get
			{
				return this.isSharable;
			}
		}

		// Token: 0x06000D30 RID: 3376 RVA: 0x001FE7F8 File Offset: 0x001FDBF8
		private bool AcceptRecord(int record)
		{
			return this.AcceptRecord(record, this.RowFilter);
		}

		// Token: 0x06000D31 RID: 3377 RVA: 0x001FE814 File Offset: 0x001FDC14
		private bool AcceptRecord(int record, IFilter filter)
		{
			Bid.Trace("<ds.Index.AcceptRecord|API> %d#, record=%d\n", this.ObjectID, record);
			if (filter == null)
			{
				return true;
			}
			DataRow dataRow = this.table.recordManager[record];
			if (dataRow == null)
			{
				return true;
			}
			DataRowVersion dataRowVersion = DataRowVersion.Default;
			if (dataRow.oldRecord == record)
			{
				dataRowVersion = DataRowVersion.Original;
			}
			else if (dataRow.newRecord == record)
			{
				dataRowVersion = DataRowVersion.Current;
			}
			else if (dataRow.tempRecord == record)
			{
				dataRowVersion = DataRowVersion.Proposed;
			}
			return filter.Invoke(dataRow, dataRowVersion);
		}

		// Token: 0x06000D32 RID: 3378 RVA: 0x001FE890 File Offset: 0x001FDC90
		internal void ListChangedAdd(DataViewListener listener)
		{
			this._listeners.Add(listener);
		}

		// Token: 0x06000D33 RID: 3379 RVA: 0x001FE8AC File Offset: 0x001FDCAC
		internal void ListChangedRemove(DataViewListener listener)
		{
			this._listeners.Remove(listener);
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06000D34 RID: 3380 RVA: 0x001FE8C8 File Offset: 0x001FDCC8
		public int RefCount
		{
			get
			{
				return this.refCount;
			}
		}

		// Token: 0x06000D35 RID: 3381 RVA: 0x001FE8DC File Offset: 0x001FDCDC
		public void AddRef()
		{
			Bid.Trace("<ds.Index.AddRef|API> %d#\n", this.ObjectID);
			LockCookie lockCookie = this.table.indexesLock.UpgradeToWriterLock(-1);
			try
			{
				if (this.refCount == 0)
				{
					this.table.ShadowIndexCopy();
					this.table.indexes.Add(this);
				}
				this.refCount++;
			}
			finally
			{
				this.table.indexesLock.DowngradeFromWriterLock(ref lockCookie);
			}
		}

		// Token: 0x06000D36 RID: 3382 RVA: 0x001FE970 File Offset: 0x001FDD70
		public int RemoveRef()
		{
			Bid.Trace("<ds.Index.RemoveRef|API> %d#\n", this.ObjectID);
			LockCookie lockCookie = this.table.indexesLock.UpgradeToWriterLock(-1);
			int num;
			try
			{
				num = --this.refCount;
				if (this.refCount <= 0)
				{
					this.table.ShadowIndexCopy();
					this.table.indexes.Remove(this);
				}
			}
			finally
			{
				this.table.indexesLock.DowngradeFromWriterLock(ref lockCookie);
			}
			return num;
		}

		// Token: 0x06000D37 RID: 3383 RVA: 0x001FEA0C File Offset: 0x001FDE0C
		private void ApplyChangeAction(int record, int action, int changeRecord)
		{
			if (action != 0)
			{
				if (action > 0)
				{
					if (this.AcceptRecord(record))
					{
						this.InsertRecord(record, true);
						return;
					}
				}
				else
				{
					if (this._comparison != null && -1 != record)
					{
						this.DeleteRecord(this.GetIndex(record, changeRecord));
						return;
					}
					this.DeleteRecord(this.GetIndex(record));
				}
			}
		}

		// Token: 0x06000D38 RID: 3384 RVA: 0x001FEA5C File Offset: 0x001FDE5C
		public bool CheckUnique()
		{
			return !this.HasDuplicates;
		}

		// Token: 0x06000D39 RID: 3385 RVA: 0x001FEA74 File Offset: 0x001FDE74
		private int CompareRecords(int record1, int record2)
		{
			if (this._comparison != null)
			{
				return this.CompareDataRows(record1, record2);
			}
			if (0 < this.IndexFields.Length)
			{
				int i = 0;
				while (i < this.IndexFields.Length)
				{
					int num = this.IndexFields[i].Column.Compare(record1, record2);
					if (num != 0)
					{
						if (!this.IndexFields[i].IsDescending)
						{
							return num;
						}
						return -num;
					}
					else
					{
						i++;
					}
				}
				return 0;
			}
			return this.table.Rows.IndexOf(this.table.recordManager[record1]).CompareTo(this.table.Rows.IndexOf(this.table.recordManager[record2]));
		}

		// Token: 0x06000D3A RID: 3386 RVA: 0x001FEB3C File Offset: 0x001FDF3C
		private int CompareDataRows(int record1, int record2)
		{
			return this._comparison(this.table.recordManager[record1], this.table.recordManager[record2]);
		}

		// Token: 0x06000D3B RID: 3387 RVA: 0x001FEB78 File Offset: 0x001FDF78
		private int CompareDuplicateRecords(int record1, int record2)
		{
			if (this.table.recordManager[record1] == null)
			{
				if (this.table.recordManager[record2] != null)
				{
					return -1;
				}
				return 0;
			}
			else
			{
				if (this.table.recordManager[record2] == null)
				{
					return 1;
				}
				int num = this.table.recordManager[record1].rowID.CompareTo(this.table.recordManager[record2].rowID);
				if (num == 0 && record1 != record2)
				{
					num = ((int)this.table.recordManager[record1].GetRecordState(record1)).CompareTo((int)this.table.recordManager[record2].GetRecordState(record2));
				}
				return num;
			}
		}

		// Token: 0x06000D3C RID: 3388 RVA: 0x001FEC38 File Offset: 0x001FE038
		private int CompareRecordToKey(int record1, object[] vals)
		{
			int i = 0;
			while (i < this.IndexFields.Length)
			{
				int num = this.IndexFields[i].Column.CompareValueTo(record1, vals[i]);
				if (num != 0)
				{
					if (!this.IndexFields[i].IsDescending)
					{
						return num;
					}
					return -num;
				}
				else
				{
					i++;
				}
			}
			return 0;
		}

		// Token: 0x06000D3D RID: 3389 RVA: 0x001FEC9C File Offset: 0x001FE09C
		public void DeleteRecordFromIndex(int recordIndex)
		{
			this.DeleteRecord(recordIndex, false);
		}

		// Token: 0x06000D3E RID: 3390 RVA: 0x001FECB4 File Offset: 0x001FE0B4
		private void DeleteRecord(int recordIndex)
		{
			this.DeleteRecord(recordIndex, true);
		}

		// Token: 0x06000D3F RID: 3391 RVA: 0x001FECCC File Offset: 0x001FE0CC
		private void DeleteRecord(int recordIndex, bool fireEvent)
		{
			Bid.Trace("<ds.Index.DeleteRecord|INFO> %d#, recordIndex=%d, fireEvent=%d{bool}\n", this.ObjectID, recordIndex, fireEvent);
			if (recordIndex >= 0)
			{
				this.recordCount--;
				int num = this.records.DeleteByIndex(recordIndex);
				this.MaintainDataView(ListChangedType.ItemDeleted, num, !fireEvent);
				if (fireEvent)
				{
					this.OnListChanged(ListChangedType.ItemDeleted, recordIndex);
				}
			}
		}

		// Token: 0x06000D40 RID: 3392 RVA: 0x001FED24 File Offset: 0x001FE124
		public RBTree<int>.RBTreeEnumerator GetEnumerator(int startIndex)
		{
			return new RBTree<int>.RBTreeEnumerator(this.records, startIndex);
		}

		// Token: 0x06000D41 RID: 3393 RVA: 0x001FED40 File Offset: 0x001FE140
		public int GetIndex(int record)
		{
			return this.records.GetIndexByKey(record);
		}

		// Token: 0x06000D42 RID: 3394 RVA: 0x001FED5C File Offset: 0x001FE15C
		private int GetIndex(int record, int changeRecord)
		{
			DataRow dataRow = this.table.recordManager[record];
			int newRecord = dataRow.newRecord;
			int oldRecord = dataRow.oldRecord;
			int indexByKey;
			try
			{
				switch (changeRecord)
				{
				case 1:
					dataRow.newRecord = record;
					break;
				case 2:
					dataRow.oldRecord = record;
					break;
				}
				indexByKey = this.records.GetIndexByKey(record);
			}
			finally
			{
				switch (changeRecord)
				{
				case 1:
					dataRow.newRecord = newRecord;
					break;
				case 2:
					dataRow.oldRecord = oldRecord;
					break;
				}
			}
			return indexByKey;
		}

		// Token: 0x06000D43 RID: 3395 RVA: 0x001FEE04 File Offset: 0x001FE204
		public object[] GetUniqueKeyValues()
		{
			if (this.IndexFields == null || this.IndexFields.Length == 0)
			{
				return Index.zeroObjects;
			}
			List<object[]> list = new List<object[]>();
			this.GetUniqueKeyValues(list, this.records.root);
			return list.ToArray();
		}

		// Token: 0x06000D44 RID: 3396 RVA: 0x001FEE48 File Offset: 0x001FE248
		public int FindRecord(int record)
		{
			int num = this.records.Search(record);
			if (num != 0)
			{
				return this.records.GetIndexByNode(num);
			}
			return -1;
		}

		// Token: 0x06000D45 RID: 3397 RVA: 0x001FEE74 File Offset: 0x001FE274
		public int FindRecordByKey(object key)
		{
			int num = this.FindNodeByKey(key);
			if (num != 0)
			{
				return this.records.GetIndexByNode(num);
			}
			return -1;
		}

		// Token: 0x06000D46 RID: 3398 RVA: 0x001FEE9C File Offset: 0x001FE29C
		public int FindRecordByKey(object[] key)
		{
			int num = this.FindNodeByKeys(key);
			if (num != 0)
			{
				return this.records.GetIndexByNode(num);
			}
			return -1;
		}

		// Token: 0x06000D47 RID: 3399 RVA: 0x001FEEC4 File Offset: 0x001FE2C4
		private int FindNodeByKey(object originalKey)
		{
			if (this.IndexFields.Length != 1)
			{
				throw ExceptionBuilder.IndexKeyLength(this.IndexFields.Length, 1);
			}
			int num = this.records.root;
			if (num != 0)
			{
				DataColumn column = this.IndexFields[0].Column;
				object obj = column.ConvertValue(originalKey);
				num = this.records.root;
				if (this.IndexFields[0].IsDescending)
				{
					while (num != 0)
					{
						int num2 = column.CompareValueTo(this.records.Key(num), obj);
						if (num2 == 0)
						{
							break;
						}
						if (num2 < 0)
						{
							num = this.records.Left(num);
						}
						else
						{
							num = this.records.Right(num);
						}
					}
				}
				else
				{
					while (num != 0)
					{
						int num2 = column.CompareValueTo(this.records.Key(num), obj);
						if (num2 == 0)
						{
							break;
						}
						if (num2 > 0)
						{
							num = this.records.Left(num);
						}
						else
						{
							num = this.records.Right(num);
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06000D48 RID: 3400 RVA: 0x001FEFBC File Offset: 0x001FE3BC
		private int FindNodeByKeys(object[] originalKey)
		{
			int num = ((originalKey != null) ? originalKey.Length : 0);
			if (num == 0 || this.IndexFields.Length != num)
			{
				throw ExceptionBuilder.IndexKeyLength(this.IndexFields.Length, num);
			}
			int num2 = this.records.root;
			if (num2 != 0)
			{
				object[] array = new object[originalKey.Length];
				for (int i = 0; i < originalKey.Length; i++)
				{
					array[i] = this.IndexFields[i].Column.ConvertValue(originalKey[i]);
				}
				num2 = this.records.root;
				while (num2 != 0)
				{
					num = this.CompareRecordToKey(this.records.Key(num2), array);
					if (num == 0)
					{
						break;
					}
					if (num > 0)
					{
						num2 = this.records.Left(num2);
					}
					else
					{
						num2 = this.records.Right(num2);
					}
				}
			}
			return num2;
		}

		// Token: 0x06000D49 RID: 3401 RVA: 0x001FF084 File Offset: 0x001FE484
		private int FindNodeByKeyRecord(int record)
		{
			int num = this.records.root;
			if (num != 0)
			{
				num = this.records.root;
				while (num != 0)
				{
					int num2 = this.CompareRecords(this.records.Key(num), record);
					if (num2 == 0)
					{
						break;
					}
					if (num2 > 0)
					{
						num = this.records.Left(num);
					}
					else
					{
						num = this.records.Right(num);
					}
				}
			}
			return num;
		}

		// Token: 0x06000D4A RID: 3402 RVA: 0x001FF0EC File Offset: 0x001FE4EC
		internal Range FindRecords<TKey, TRow>(Index.ComparisonBySelector<TKey, TRow> comparison, TKey key) where TRow : DataRow
		{
			int num = this.records.root;
			while (num != 0)
			{
				int num2 = comparison(key, (TRow)((object)this.table.recordManager[this.records.Key(num)]));
				if (num2 == 0)
				{
					break;
				}
				if (num2 < 0)
				{
					num = this.records.Left(num);
				}
				else
				{
					num = this.records.Right(num);
				}
			}
			return this.GetRangeFromNode(num);
		}

		// Token: 0x06000D4B RID: 3403 RVA: 0x001FF160 File Offset: 0x001FE560
		private Range GetRangeFromNode(int nodeId)
		{
			if (nodeId == 0)
			{
				return default(Range);
			}
			int indexByNode = this.records.GetIndexByNode(nodeId);
			if (this.records.Next(nodeId) == 0)
			{
				return new Range(indexByNode, indexByNode);
			}
			int num = this.records.SubTreeSize(this.records.Next(nodeId));
			return new Range(indexByNode, indexByNode + num - 1);
		}

		// Token: 0x06000D4C RID: 3404 RVA: 0x001FF1C0 File Offset: 0x001FE5C0
		public Range FindRecords(object key)
		{
			int num = this.FindNodeByKey(key);
			return this.GetRangeFromNode(num);
		}

		// Token: 0x06000D4D RID: 3405 RVA: 0x001FF1DC File Offset: 0x001FE5DC
		public Range FindRecords(object[] key)
		{
			int num = this.FindNodeByKeys(key);
			return this.GetRangeFromNode(num);
		}

		// Token: 0x06000D4E RID: 3406 RVA: 0x001FF1F8 File Offset: 0x001FE5F8
		internal void FireResetEvent()
		{
			Bid.Trace("<ds.Index.FireResetEvent|API> %d#\n", this.ObjectID);
			if (this.DoListChanged)
			{
				this.OnListChanged(DataView.ResetEventArgs);
			}
		}

		// Token: 0x06000D4F RID: 3407 RVA: 0x001FF228 File Offset: 0x001FE628
		private int GetChangeAction(DataViewRowState oldState, DataViewRowState newState)
		{
			int num = (((this.recordStates & oldState) == DataViewRowState.None) ? 0 : 1);
			int num2 = (((this.recordStates & newState) == DataViewRowState.None) ? 0 : 1);
			return num2 - num;
		}

		// Token: 0x06000D50 RID: 3408 RVA: 0x001FF258 File Offset: 0x001FE658
		private static int GetReplaceAction(DataViewRowState oldState)
		{
			if ((DataViewRowState.CurrentRows & oldState) != DataViewRowState.None)
			{
				return 1;
			}
			if ((DataViewRowState.OriginalRows & oldState) == DataViewRowState.None)
			{
				return 0;
			}
			return 2;
		}

		// Token: 0x06000D51 RID: 3409 RVA: 0x001FF278 File Offset: 0x001FE678
		public DataRow GetRow(int i)
		{
			return this.table.recordManager[this.GetRecord(i)];
		}

		// Token: 0x06000D52 RID: 3410 RVA: 0x001FF29C File Offset: 0x001FE69C
		public DataRow[] GetRows(object[] values)
		{
			return this.GetRows(this.FindRecords(values));
		}

		// Token: 0x06000D53 RID: 3411 RVA: 0x001FF2B8 File Offset: 0x001FE6B8
		public DataRow[] GetRows(Range range)
		{
			DataRow[] array = this.table.NewRowArray(range.Count);
			if (0 < array.Length)
			{
				RBTree<int>.RBTreeEnumerator enumerator = this.GetEnumerator(range.Min);
				int num = 0;
				while (num < array.Length && enumerator.MoveNext())
				{
					array[num] = this.table.recordManager[enumerator.Current];
					num++;
				}
			}
			return array;
		}

		// Token: 0x06000D54 RID: 3412 RVA: 0x001FF320 File Offset: 0x001FE720
		private void InitRecords(IFilter filter)
		{
			DataViewRowState dataViewRowState = this.recordStates;
			bool flag = 0 == this.IndexFields.Length;
			this.records = new Index.IndexTree(this);
			this.recordCount = 0;
			foreach (object obj in this.table.Rows)
			{
				DataRow dataRow = (DataRow)obj;
				int num = -1;
				if (dataRow.oldRecord == dataRow.newRecord)
				{
					if ((dataViewRowState & DataViewRowState.Unchanged) != DataViewRowState.None)
					{
						num = dataRow.oldRecord;
					}
				}
				else if (dataRow.oldRecord == -1)
				{
					if ((dataViewRowState & DataViewRowState.Added) != DataViewRowState.None)
					{
						num = dataRow.newRecord;
					}
				}
				else if (dataRow.newRecord == -1)
				{
					if ((dataViewRowState & DataViewRowState.Deleted) != DataViewRowState.None)
					{
						num = dataRow.oldRecord;
					}
				}
				else if ((dataViewRowState & DataViewRowState.ModifiedCurrent) != DataViewRowState.None)
				{
					num = dataRow.newRecord;
				}
				else if ((dataViewRowState & DataViewRowState.ModifiedOriginal) != DataViewRowState.None)
				{
					num = dataRow.oldRecord;
				}
				if (num != -1 && this.AcceptRecord(num, filter))
				{
					this.records.InsertAt(-1, num, flag);
					this.recordCount++;
				}
			}
		}

		// Token: 0x06000D55 RID: 3413 RVA: 0x001FF448 File Offset: 0x001FE848
		public int InsertRecordToIndex(int record)
		{
			int num = -1;
			if (this.AcceptRecord(record))
			{
				num = this.InsertRecord(record, false);
			}
			return num;
		}

		// Token: 0x06000D56 RID: 3414 RVA: 0x001FF46C File Offset: 0x001FE86C
		private int InsertRecord(int record, bool fireEvent)
		{
			Bid.Trace("<ds.Index.InsertRecord|INFO> %d#, record=%d, fireEvent=%d{bool}\n", this.ObjectID, record, fireEvent);
			bool flag = false;
			if (this.IndexFields.Length == 0 && this.table != null)
			{
				DataRow dataRow = this.table.recordManager[record];
				flag = this.table.Rows.IndexOf(dataRow) + 1 == this.table.Rows.Count;
			}
			int num = this.records.InsertAt(-1, record, flag);
			this.recordCount++;
			this.MaintainDataView(ListChangedType.ItemAdded, record, !fireEvent);
			if (fireEvent)
			{
				if (this.DoListChanged)
				{
					this.OnListChanged(ListChangedType.ItemAdded, this.records.GetIndexByNode(num));
				}
				return 0;
			}
			return this.records.GetIndexByNode(num);
		}

		// Token: 0x06000D57 RID: 3415 RVA: 0x001FF52C File Offset: 0x001FE92C
		public bool IsKeyInIndex(object key)
		{
			int num = this.FindNodeByKey(key);
			return 0 != num;
		}

		// Token: 0x06000D58 RID: 3416 RVA: 0x001FF548 File Offset: 0x001FE948
		public bool IsKeyInIndex(object[] key)
		{
			int num = this.FindNodeByKeys(key);
			return 0 != num;
		}

		// Token: 0x06000D59 RID: 3417 RVA: 0x001FF564 File Offset: 0x001FE964
		public bool IsKeyRecordInIndex(int record)
		{
			int num = this.FindNodeByKeyRecord(record);
			return 0 != num;
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06000D5A RID: 3418 RVA: 0x001FF580 File Offset: 0x001FE980
		private bool DoListChanged
		{
			get
			{
				return !this.suspendEvents && this._listeners.HasListeners && !this.table.AreIndexEventsSuspended;
			}
		}

		// Token: 0x06000D5B RID: 3419 RVA: 0x001FF5B4 File Offset: 0x001FE9B4
		private void OnListChanged(ListChangedType changedType, int newIndex, int oldIndex)
		{
			if (this.DoListChanged)
			{
				this.OnListChanged(new ListChangedEventArgs(changedType, newIndex, oldIndex));
			}
		}

		// Token: 0x06000D5C RID: 3420 RVA: 0x001FF5D8 File Offset: 0x001FE9D8
		private void OnListChanged(ListChangedType changedType, int index)
		{
			if (this.DoListChanged)
			{
				this.OnListChanged(new ListChangedEventArgs(changedType, index));
			}
		}

		// Token: 0x06000D5D RID: 3421 RVA: 0x001FF5FC File Offset: 0x001FE9FC
		private void OnListChanged(ListChangedEventArgs e)
		{
			Bid.Trace("<ds.Index.OnListChanged|INFO> %d#\n", this.ObjectID);
			this._listeners.Notify<ListChangedEventArgs, bool, bool>(e, false, false, delegate(DataViewListener listener, ListChangedEventArgs args, bool arg2, bool arg3)
			{
				listener.IndexListChanged(args);
			});
		}

		// Token: 0x06000D5E RID: 3422 RVA: 0x001FF644 File Offset: 0x001FEA44
		private void MaintainDataView(ListChangedType changedType, int record, bool trackAddRemove)
		{
			this._listeners.Notify<ListChangedType, DataRow, bool>(changedType, (0 <= record) ? this.table.recordManager[record] : null, trackAddRemove, delegate(DataViewListener listener, ListChangedType type, DataRow row, bool track)
			{
				listener.MaintainDataView(changedType, row, track);
			});
		}

		// Token: 0x06000D5F RID: 3423 RVA: 0x001FF694 File Offset: 0x001FEA94
		public void Reset()
		{
			Bid.Trace("<ds.Index.Reset|API> %d#\n", this.ObjectID);
			this.InitRecords(this.RowFilter);
			this.MaintainDataView(ListChangedType.Reset, -1, false);
			this.FireResetEvent();
		}

		// Token: 0x06000D60 RID: 3424 RVA: 0x001FF6CC File Offset: 0x001FEACC
		public void RecordChanged(int record)
		{
			Bid.Trace("<ds.Index.RecordChanged|API> %d#, record=%d\n", this.ObjectID, record);
			if (this.DoListChanged)
			{
				int index = this.GetIndex(record);
				if (index >= 0)
				{
					this.OnListChanged(ListChangedType.ItemChanged, index);
				}
			}
		}

		// Token: 0x06000D61 RID: 3425 RVA: 0x001FF708 File Offset: 0x001FEB08
		public void RecordChanged(int oldIndex, int newIndex)
		{
			Bid.Trace("<ds.Index.RecordChanged|API> %d#, oldIndex=%d, newIndex=%d\n", this.ObjectID, oldIndex, newIndex);
			if (oldIndex > -1 || newIndex > -1)
			{
				if (oldIndex == newIndex)
				{
					this.OnListChanged(ListChangedType.ItemChanged, newIndex, oldIndex);
					return;
				}
				if (oldIndex == -1)
				{
					this.OnListChanged(ListChangedType.ItemAdded, newIndex, oldIndex);
					return;
				}
				if (newIndex == -1)
				{
					this.OnListChanged(ListChangedType.ItemDeleted, oldIndex);
					return;
				}
				this.OnListChanged(ListChangedType.ItemMoved, newIndex, oldIndex);
			}
		}

		// Token: 0x06000D62 RID: 3426 RVA: 0x001FF764 File Offset: 0x001FEB64
		public void RecordStateChanged(int record, DataViewRowState oldState, DataViewRowState newState)
		{
			Bid.Trace("<ds.Index.RecordStateChanged|API> %d#, record=%d, oldState=%d{ds.DataViewRowState}, newState=%d{ds.DataViewRowState}\n", this.ObjectID, record, (int)oldState, (int)newState);
			int changeAction = this.GetChangeAction(oldState, newState);
			this.ApplyChangeAction(record, changeAction, Index.GetReplaceAction(oldState));
		}

		// Token: 0x06000D63 RID: 3427 RVA: 0x001FF79C File Offset: 0x001FEB9C
		public void RecordStateChanged(int oldRecord, DataViewRowState oldOldState, DataViewRowState oldNewState, int newRecord, DataViewRowState newOldState, DataViewRowState newNewState)
		{
			Bid.Trace("<ds.Index.RecordStateChanged|API> %d#, oldRecord=%d, oldOldState=%d{ds.DataViewRowState}, oldNewState=%d{ds.DataViewRowState}, newRecord=%d, newOldState=%d{ds.DataViewRowState}, newNewState=%d{ds.DataViewRowState}\n", this.ObjectID, oldRecord, (int)oldOldState, (int)oldNewState, newRecord, (int)newOldState, (int)newNewState);
			int changeAction = this.GetChangeAction(oldOldState, oldNewState);
			int changeAction2 = this.GetChangeAction(newOldState, newNewState);
			if (changeAction != -1 || changeAction2 != 1 || !this.AcceptRecord(newRecord))
			{
				this.ApplyChangeAction(oldRecord, changeAction, Index.GetReplaceAction(oldOldState));
				this.ApplyChangeAction(newRecord, changeAction2, Index.GetReplaceAction(newOldState));
				return;
			}
			int num;
			if (this._comparison != null && changeAction < 0)
			{
				num = this.GetIndex(oldRecord, Index.GetReplaceAction(oldOldState));
			}
			else
			{
				num = this.GetIndex(oldRecord);
			}
			if (this._comparison == null && num != -1 && this.CompareRecords(oldRecord, newRecord) == 0)
			{
				this.records.UpdateNodeKey(oldRecord, newRecord);
				int index = this.GetIndex(newRecord);
				this.OnListChanged(ListChangedType.ItemChanged, index, index);
				return;
			}
			this.suspendEvents = true;
			if (num != -1)
			{
				this.records.DeleteByIndex(num);
				this.recordCount--;
			}
			this.records.Insert(newRecord);
			this.recordCount++;
			this.suspendEvents = false;
			int index2 = this.GetIndex(newRecord);
			if (num == index2)
			{
				this.OnListChanged(ListChangedType.ItemChanged, index2, num);
				return;
			}
			if (num == -1)
			{
				this.MaintainDataView(ListChangedType.ItemAdded, newRecord, false);
				this.OnListChanged(ListChangedType.ItemAdded, this.GetIndex(newRecord));
				return;
			}
			this.OnListChanged(ListChangedType.ItemMoved, index2, num);
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06000D64 RID: 3428 RVA: 0x001FF8F8 File Offset: 0x001FECF8
		internal DataTable Table
		{
			get
			{
				return this.table;
			}
		}

		// Token: 0x06000D65 RID: 3429 RVA: 0x001FF90C File Offset: 0x001FED0C
		private void GetUniqueKeyValues(List<object[]> list, int curNodeId)
		{
			if (curNodeId != 0)
			{
				this.GetUniqueKeyValues(list, this.records.Left(curNodeId));
				int num = this.records.Key(curNodeId);
				object[] array = new object[this.IndexFields.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = this.IndexFields[i].Column[num];
				}
				list.Add(array);
				this.GetUniqueKeyValues(list, this.records.Right(curNodeId));
			}
		}

		// Token: 0x06000D66 RID: 3430 RVA: 0x001FF994 File Offset: 0x001FED94
		internal static int IndexOfReference<T>(List<T> list, T item) where T : class
		{
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					if (object.ReferenceEquals(list[i], item))
					{
						return i;
					}
				}
			}
			return -1;
		}

		// Token: 0x06000D67 RID: 3431 RVA: 0x001FF9D4 File Offset: 0x001FEDD4
		internal static bool ContainsReference<T>(List<T> list, T item) where T : class
		{
			return 0 <= Index.IndexOfReference<T>(list, item);
		}

		// Token: 0x040008FE RID: 2302
		private const int DoNotReplaceCompareRecord = 0;

		// Token: 0x040008FF RID: 2303
		private const int ReplaceNewRecordForCompare = 1;

		// Token: 0x04000900 RID: 2304
		private const int ReplaceOldRecordForCompare = 2;

		// Token: 0x04000901 RID: 2305
		internal const int MaskBits = 2147483647;

		// Token: 0x04000902 RID: 2306
		private readonly DataTable table;

		// Token: 0x04000903 RID: 2307
		internal readonly int[] IndexDesc;

		// Token: 0x04000904 RID: 2308
		internal readonly IndexField[] IndexFields;

		// Token: 0x04000905 RID: 2309
		private readonly Comparison<DataRow> _comparison;

		// Token: 0x04000906 RID: 2310
		private readonly DataViewRowState recordStates;

		// Token: 0x04000907 RID: 2311
		private WeakReference rowFilter;

		// Token: 0x04000908 RID: 2312
		private Index.IndexTree records;

		// Token: 0x04000909 RID: 2313
		private int recordCount;

		// Token: 0x0400090A RID: 2314
		private int refCount;

		// Token: 0x0400090B RID: 2315
		private Listeners<DataViewListener> _listeners;

		// Token: 0x0400090C RID: 2316
		private bool suspendEvents;

		// Token: 0x0400090D RID: 2317
		private static readonly object[] zeroObjects = new object[0];

		// Token: 0x0400090E RID: 2318
		private readonly bool isSharable;

		// Token: 0x0400090F RID: 2319
		private readonly bool _hasRemoteAggregate;

		// Token: 0x04000910 RID: 2320
		private static int _objectTypeCount;

		// Token: 0x04000911 RID: 2321
		private readonly int _objectID = Interlocked.Increment(ref Index._objectTypeCount);

		// Token: 0x04000912 RID: 2322
		[CompilerGenerated]
		private static Listeners<DataViewListener>.Func<DataViewListener, bool> <>9__CachedAnonymousMethodDelegate1;

		// Token: 0x04000913 RID: 2323
		[CompilerGenerated]
		private static Listeners<DataViewListener>.Action<DataViewListener, ListChangedEventArgs, bool, bool> <>9__CachedAnonymousMethodDelegate3;

		// Token: 0x020000DD RID: 221
		private sealed class IndexTree : RBTree<int>
		{
			// Token: 0x06000D6B RID: 3435 RVA: 0x001FFA30 File Offset: 0x001FEE30
			internal IndexTree(Index index)
				: base(TreeAccessMethod.KEY_SEARCH_AND_INDEX)
			{
				this._index = index;
			}

			// Token: 0x06000D6C RID: 3436 RVA: 0x001FFA4C File Offset: 0x001FEE4C
			protected override int CompareNode(int record1, int record2)
			{
				return this._index.CompareRecords(record1, record2);
			}

			// Token: 0x06000D6D RID: 3437 RVA: 0x001FFA68 File Offset: 0x001FEE68
			protected override int CompareSateliteTreeNode(int record1, int record2)
			{
				return this._index.CompareDuplicateRecords(record1, record2);
			}

			// Token: 0x04000914 RID: 2324
			private readonly Index _index;
		}

		// Token: 0x020000DE RID: 222
		// (Invoke) Token: 0x06000D6F RID: 3439
		internal delegate int ComparisonBySelector<TKey, TRow>(TKey key, TRow row) where TRow : DataRow;
	}
}
