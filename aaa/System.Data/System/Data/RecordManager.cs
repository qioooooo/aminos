using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;

namespace System.Data
{
	// Token: 0x020000D2 RID: 210
	internal sealed class RecordManager
	{
		// Token: 0x06000CE4 RID: 3300 RVA: 0x001FC3FC File Offset: 0x001FB7FC
		internal RecordManager(DataTable table)
		{
			if (table == null)
			{
				throw ExceptionBuilder.ArgumentNull("table");
			}
			this.table = table;
		}

		// Token: 0x06000CE5 RID: 3301 RVA: 0x001FC438 File Offset: 0x001FB838
		private void GrowRecordCapacity()
		{
			if (RecordManager.NewCapacity(this.recordCapacity) < this.NormalizedMinimumCapacity(this.minimumCapacity))
			{
				this.RecordCapacity = this.NormalizedMinimumCapacity(this.minimumCapacity);
			}
			else
			{
				this.RecordCapacity = RecordManager.NewCapacity(this.recordCapacity);
			}
			DataRow[] array = this.table.NewRowArray(this.recordCapacity);
			if (this.rows != null)
			{
				Array.Copy(this.rows, 0, array, 0, Math.Min(this.lastFreeRecord, this.rows.Length));
			}
			this.rows = array;
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000CE6 RID: 3302 RVA: 0x001FC4C8 File Offset: 0x001FB8C8
		internal int LastFreeRecord
		{
			get
			{
				return this.lastFreeRecord;
			}
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06000CE7 RID: 3303 RVA: 0x001FC4DC File Offset: 0x001FB8DC
		// (set) Token: 0x06000CE8 RID: 3304 RVA: 0x001FC4F0 File Offset: 0x001FB8F0
		internal int MinimumCapacity
		{
			get
			{
				return this.minimumCapacity;
			}
			set
			{
				if (this.minimumCapacity != value)
				{
					if (value < 0)
					{
						throw ExceptionBuilder.NegativeMinimumCapacity();
					}
					this.minimumCapacity = value;
				}
			}
		}

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06000CE9 RID: 3305 RVA: 0x001FC518 File Offset: 0x001FB918
		// (set) Token: 0x06000CEA RID: 3306 RVA: 0x001FC52C File Offset: 0x001FB92C
		internal int RecordCapacity
		{
			get
			{
				return this.recordCapacity;
			}
			set
			{
				if (this.recordCapacity != value)
				{
					for (int i = 0; i < this.table.Columns.Count; i++)
					{
						this.table.Columns[i].SetCapacity(value);
					}
					this.recordCapacity = value;
				}
			}
		}

		// Token: 0x06000CEB RID: 3307 RVA: 0x001FC57C File Offset: 0x001FB97C
		internal static int NewCapacity(int capacity)
		{
			if (capacity >= 128)
			{
				return capacity + capacity;
			}
			return 128;
		}

		// Token: 0x06000CEC RID: 3308 RVA: 0x001FC59C File Offset: 0x001FB99C
		private int NormalizedMinimumCapacity(int capacity)
		{
			if (capacity >= 1014)
			{
				return (capacity + 10 >> 10) + 1 << 10;
			}
			if (capacity >= 246)
			{
				return 1024;
			}
			if (capacity < 54)
			{
				return 64;
			}
			return 256;
		}

		// Token: 0x06000CED RID: 3309 RVA: 0x001FC5DC File Offset: 0x001FB9DC
		internal int NewRecordBase()
		{
			int num;
			if (this.freeRecordList.Count != 0)
			{
				num = this.freeRecordList[this.freeRecordList.Count - 1];
				this.freeRecordList.RemoveAt(this.freeRecordList.Count - 1);
			}
			else
			{
				if (this.lastFreeRecord >= this.recordCapacity)
				{
					this.GrowRecordCapacity();
				}
				num = this.lastFreeRecord;
				this.lastFreeRecord++;
			}
			return num;
		}

		// Token: 0x06000CEE RID: 3310 RVA: 0x001FC654 File Offset: 0x001FBA54
		internal void FreeRecord(ref int record)
		{
			if (-1 != record)
			{
				this[record] = null;
				int count = this.table.columnCollection.Count;
				for (int i = 0; i < count; i++)
				{
					this.table.columnCollection[i].FreeRecord(record);
				}
				if (this.lastFreeRecord == record + 1)
				{
					this.lastFreeRecord--;
				}
				else if (record < this.lastFreeRecord)
				{
					this.freeRecordList.Add(record);
				}
				record = -1;
			}
		}

		// Token: 0x06000CEF RID: 3311 RVA: 0x001FC6E0 File Offset: 0x001FBAE0
		internal void Clear(bool clearAll)
		{
			if (clearAll)
			{
				for (int i = 0; i < this.recordCapacity; i++)
				{
					this.rows[i] = null;
				}
				int count = this.table.columnCollection.Count;
				for (int j = 0; j < count; j++)
				{
					DataColumn dataColumn = this.table.columnCollection[j];
					for (int k = 0; k < this.recordCapacity; k++)
					{
						dataColumn.FreeRecord(k);
					}
				}
				this.lastFreeRecord = 0;
				this.freeRecordList.Clear();
				return;
			}
			this.freeRecordList.Capacity = this.freeRecordList.Count + this.table.Rows.Count;
			for (int l = 0; l < this.recordCapacity; l++)
			{
				if (this.rows[l] != null && this.rows[l].rowID != -1L)
				{
					int num = l;
					this.FreeRecord(ref num);
				}
			}
		}

		// Token: 0x170001F3 RID: 499
		internal DataRow this[int record]
		{
			get
			{
				return this.rows[record];
			}
			set
			{
				this.rows[record] = value;
			}
		}

		// Token: 0x06000CF2 RID: 3314 RVA: 0x001FC7FC File Offset: 0x001FBBFC
		internal void SetKeyValues(int record, DataKey key, object[] keyValues)
		{
			for (int i = 0; i < keyValues.Length; i++)
			{
				key.ColumnsReference[i][record] = keyValues[i];
			}
		}

		// Token: 0x06000CF3 RID: 3315 RVA: 0x001FC82C File Offset: 0x001FBC2C
		internal int ImportRecord(DataTable src, int record)
		{
			return this.CopyRecord(src, record, -1);
		}

		// Token: 0x06000CF4 RID: 3316 RVA: 0x001FC844 File Offset: 0x001FBC44
		internal int CopyRecord(DataTable src, int record, int copy)
		{
			if (record == -1)
			{
				return copy;
			}
			int num = -1;
			try
			{
				if (copy == -1)
				{
					num = this.table.NewUninitializedRecord();
				}
				else
				{
					num = copy;
				}
				int count = this.table.Columns.Count;
				for (int i = 0; i < count; i++)
				{
					DataColumn dataColumn = this.table.Columns[i];
					DataColumn dataColumn2 = src.Columns[dataColumn.ColumnName];
					if (dataColumn2 != null)
					{
						object obj = dataColumn2[record];
						ICloneable cloneable = obj as ICloneable;
						if (cloneable != null)
						{
							dataColumn[num] = cloneable.Clone();
						}
						else
						{
							dataColumn[num] = obj;
						}
					}
					else if (-1 == copy)
					{
						dataColumn.Init(num);
					}
				}
			}
			catch (Exception ex)
			{
				if (ADP.IsCatchableOrSecurityExceptionType(ex) && -1 == copy)
				{
					this.FreeRecord(ref num);
				}
				throw;
			}
			return num;
		}

		// Token: 0x06000CF5 RID: 3317 RVA: 0x001FC92C File Offset: 0x001FBD2C
		internal void SetRowCache(DataRow[] newRows)
		{
			this.rows = newRows;
			this.lastFreeRecord = this.rows.Length;
			this.recordCapacity = this.lastFreeRecord;
		}

		// Token: 0x06000CF6 RID: 3318 RVA: 0x001FC95C File Offset: 0x001FBD5C
		[Conditional("DEBUG")]
		internal void VerifyRecord(int record)
		{
		}

		// Token: 0x06000CF7 RID: 3319 RVA: 0x001FC96C File Offset: 0x001FBD6C
		[Conditional("DEBUG")]
		internal void VerifyRecord(int record, DataRow row)
		{
		}

		// Token: 0x040008DA RID: 2266
		private readonly DataTable table;

		// Token: 0x040008DB RID: 2267
		private int lastFreeRecord;

		// Token: 0x040008DC RID: 2268
		private int minimumCapacity = 50;

		// Token: 0x040008DD RID: 2269
		private int recordCapacity;

		// Token: 0x040008DE RID: 2270
		private readonly List<int> freeRecordList = new List<int>();

		// Token: 0x040008DF RID: 2271
		private DataRow[] rows;
	}
}
