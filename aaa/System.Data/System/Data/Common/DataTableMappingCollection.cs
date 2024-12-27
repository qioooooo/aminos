using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace System.Data.Common
{
	// Token: 0x02000122 RID: 290
	[Editor("Microsoft.VSDesigner.Data.Design.DataTableMappingCollectionEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[ListBindable(false)]
	public sealed class DataTableMappingCollection : MarshalByRefObject, ITableMappingCollection, IList, ICollection, IEnumerable
	{
		// Token: 0x17000268 RID: 616
		// (get) Token: 0x06001293 RID: 4755 RVA: 0x0021F2B0 File Offset: 0x0021E6B0
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06001294 RID: 4756 RVA: 0x0021F2C0 File Offset: 0x0021E6C0
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06001295 RID: 4757 RVA: 0x0021F2D0 File Offset: 0x0021E6D0
		bool IList.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06001296 RID: 4758 RVA: 0x0021F2E0 File Offset: 0x0021E6E0
		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700026C RID: 620
		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				this.ValidateType(value);
				this[index] = (DataTableMapping)value;
			}
		}

		// Token: 0x1700026D RID: 621
		object ITableMappingCollection.this[string index]
		{
			get
			{
				return this[index];
			}
			set
			{
				this.ValidateType(value);
				this[index] = (DataTableMapping)value;
			}
		}

		// Token: 0x0600129B RID: 4763 RVA: 0x0021F360 File Offset: 0x0021E760
		ITableMapping ITableMappingCollection.Add(string sourceTableName, string dataSetTableName)
		{
			return this.Add(sourceTableName, dataSetTableName);
		}

		// Token: 0x0600129C RID: 4764 RVA: 0x0021F378 File Offset: 0x0021E778
		ITableMapping ITableMappingCollection.GetByDataSetTable(string dataSetTableName)
		{
			return this.GetByDataSetTable(dataSetTableName);
		}

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x0600129D RID: 4765 RVA: 0x0021F38C File Offset: 0x0021E78C
		[Browsable(false)]
		[ResDescription("DataTableMappings_Count")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int Count
		{
			get
			{
				if (this.items == null)
				{
					return 0;
				}
				return this.items.Count;
			}
		}

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x0600129E RID: 4766 RVA: 0x0021F3B0 File Offset: 0x0021E7B0
		private Type ItemType
		{
			get
			{
				return typeof(DataTableMapping);
			}
		}

		// Token: 0x17000270 RID: 624
		[ResDescription("DataTableMappings_Item")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DataTableMapping this[int index]
		{
			get
			{
				this.RangeCheck(index);
				return this.items[index];
			}
			set
			{
				this.RangeCheck(index);
				this.Replace(index, value);
			}
		}

		// Token: 0x17000271 RID: 625
		[ResDescription("DataTableMappings_Item")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DataTableMapping this[string sourceTable]
		{
			get
			{
				int num = this.RangeCheck(sourceTable);
				return this.items[num];
			}
			set
			{
				int num = this.RangeCheck(sourceTable);
				this.Replace(num, value);
			}
		}

		// Token: 0x060012A3 RID: 4771 RVA: 0x0021F448 File Offset: 0x0021E848
		public int Add(object value)
		{
			this.ValidateType(value);
			this.Add((DataTableMapping)value);
			return this.Count - 1;
		}

		// Token: 0x060012A4 RID: 4772 RVA: 0x0021F474 File Offset: 0x0021E874
		private DataTableMapping Add(DataTableMapping value)
		{
			this.AddWithoutEvents(value);
			return value;
		}

		// Token: 0x060012A5 RID: 4773 RVA: 0x0021F48C File Offset: 0x0021E88C
		public void AddRange(DataTableMapping[] values)
		{
			this.AddEnumerableRange(values, false);
		}

		// Token: 0x060012A6 RID: 4774 RVA: 0x0021F4A4 File Offset: 0x0021E8A4
		public void AddRange(Array values)
		{
			this.AddEnumerableRange(values, false);
		}

		// Token: 0x060012A7 RID: 4775 RVA: 0x0021F4BC File Offset: 0x0021E8BC
		private void AddEnumerableRange(IEnumerable values, bool doClone)
		{
			if (values == null)
			{
				throw ADP.ArgumentNull("values");
			}
			foreach (object obj in values)
			{
				this.ValidateType(obj);
			}
			if (doClone)
			{
				using (IEnumerator enumerator2 = values.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						object obj2 = enumerator2.Current;
						ICloneable cloneable = (ICloneable)obj2;
						this.AddWithoutEvents(cloneable.Clone() as DataTableMapping);
					}
					return;
				}
			}
			foreach (object obj3 in values)
			{
				DataTableMapping dataTableMapping = (DataTableMapping)obj3;
				this.AddWithoutEvents(dataTableMapping);
			}
		}

		// Token: 0x060012A8 RID: 4776 RVA: 0x0021F5DC File Offset: 0x0021E9DC
		public DataTableMapping Add(string sourceTable, string dataSetTable)
		{
			return this.Add(new DataTableMapping(sourceTable, dataSetTable));
		}

		// Token: 0x060012A9 RID: 4777 RVA: 0x0021F5F8 File Offset: 0x0021E9F8
		private void AddWithoutEvents(DataTableMapping value)
		{
			this.Validate(-1, value);
			value.Parent = this;
			this.ArrayList().Add(value);
		}

		// Token: 0x060012AA RID: 4778 RVA: 0x0021F620 File Offset: 0x0021EA20
		private List<DataTableMapping> ArrayList()
		{
			if (this.items == null)
			{
				this.items = new List<DataTableMapping>();
			}
			return this.items;
		}

		// Token: 0x060012AB RID: 4779 RVA: 0x0021F648 File Offset: 0x0021EA48
		public void Clear()
		{
			if (0 < this.Count)
			{
				this.ClearWithoutEvents();
			}
		}

		// Token: 0x060012AC RID: 4780 RVA: 0x0021F664 File Offset: 0x0021EA64
		private void ClearWithoutEvents()
		{
			if (this.items != null)
			{
				foreach (DataTableMapping dataTableMapping in this.items)
				{
					dataTableMapping.Parent = null;
				}
				this.items.Clear();
			}
		}

		// Token: 0x060012AD RID: 4781 RVA: 0x0021F6D8 File Offset: 0x0021EAD8
		public bool Contains(string value)
		{
			return -1 != this.IndexOf(value);
		}

		// Token: 0x060012AE RID: 4782 RVA: 0x0021F6F4 File Offset: 0x0021EAF4
		public bool Contains(object value)
		{
			return -1 != this.IndexOf(value);
		}

		// Token: 0x060012AF RID: 4783 RVA: 0x0021F710 File Offset: 0x0021EB10
		public void CopyTo(Array array, int index)
		{
			((ICollection)this.ArrayList()).CopyTo(array, index);
		}

		// Token: 0x060012B0 RID: 4784 RVA: 0x0021F72C File Offset: 0x0021EB2C
		public void CopyTo(DataTableMapping[] array, int index)
		{
			this.ArrayList().CopyTo(array, index);
		}

		// Token: 0x060012B1 RID: 4785 RVA: 0x0021F748 File Offset: 0x0021EB48
		public DataTableMapping GetByDataSetTable(string dataSetTable)
		{
			int num = this.IndexOfDataSetTable(dataSetTable);
			if (0 > num)
			{
				throw ADP.TablesDataSetTable(dataSetTable);
			}
			return this.items[num];
		}

		// Token: 0x060012B2 RID: 4786 RVA: 0x0021F774 File Offset: 0x0021EB74
		public IEnumerator GetEnumerator()
		{
			return this.ArrayList().GetEnumerator();
		}

		// Token: 0x060012B3 RID: 4787 RVA: 0x0021F794 File Offset: 0x0021EB94
		public int IndexOf(object value)
		{
			if (value != null)
			{
				this.ValidateType(value);
				for (int i = 0; i < this.Count; i++)
				{
					if (this.items[i] == value)
					{
						return i;
					}
				}
			}
			return -1;
		}

		// Token: 0x060012B4 RID: 4788 RVA: 0x0021F7D0 File Offset: 0x0021EBD0
		public int IndexOf(string sourceTable)
		{
			if (!ADP.IsEmpty(sourceTable))
			{
				for (int i = 0; i < this.Count; i++)
				{
					string sourceTable2 = this.items[i].SourceTable;
					if (sourceTable2 != null && ADP.SrcCompare(sourceTable, sourceTable2) == 0)
					{
						return i;
					}
				}
			}
			return -1;
		}

		// Token: 0x060012B5 RID: 4789 RVA: 0x0021F818 File Offset: 0x0021EC18
		public int IndexOfDataSetTable(string dataSetTable)
		{
			if (!ADP.IsEmpty(dataSetTable))
			{
				for (int i = 0; i < this.Count; i++)
				{
					string dataSetTable2 = this.items[i].DataSetTable;
					if (dataSetTable2 != null && ADP.DstCompare(dataSetTable, dataSetTable2) == 0)
					{
						return i;
					}
				}
			}
			return -1;
		}

		// Token: 0x060012B6 RID: 4790 RVA: 0x0021F860 File Offset: 0x0021EC60
		public void Insert(int index, object value)
		{
			this.ValidateType(value);
			this.Insert(index, (DataTableMapping)value);
		}

		// Token: 0x060012B7 RID: 4791 RVA: 0x0021F884 File Offset: 0x0021EC84
		public void Insert(int index, DataTableMapping value)
		{
			if (value == null)
			{
				throw ADP.TablesAddNullAttempt("value");
			}
			this.Validate(-1, value);
			value.Parent = this;
			this.ArrayList().Insert(index, value);
		}

		// Token: 0x060012B8 RID: 4792 RVA: 0x0021F8BC File Offset: 0x0021ECBC
		private void RangeCheck(int index)
		{
			if (index < 0 || this.Count <= index)
			{
				throw ADP.TablesIndexInt32(index, this);
			}
		}

		// Token: 0x060012B9 RID: 4793 RVA: 0x0021F8E0 File Offset: 0x0021ECE0
		private int RangeCheck(string sourceTable)
		{
			int num = this.IndexOf(sourceTable);
			if (num < 0)
			{
				throw ADP.TablesSourceIndex(sourceTable);
			}
			return num;
		}

		// Token: 0x060012BA RID: 4794 RVA: 0x0021F904 File Offset: 0x0021ED04
		public void RemoveAt(int index)
		{
			this.RangeCheck(index);
			this.RemoveIndex(index);
		}

		// Token: 0x060012BB RID: 4795 RVA: 0x0021F920 File Offset: 0x0021ED20
		public void RemoveAt(string sourceTable)
		{
			int num = this.RangeCheck(sourceTable);
			this.RemoveIndex(num);
		}

		// Token: 0x060012BC RID: 4796 RVA: 0x0021F93C File Offset: 0x0021ED3C
		private void RemoveIndex(int index)
		{
			this.items[index].Parent = null;
			this.items.RemoveAt(index);
		}

		// Token: 0x060012BD RID: 4797 RVA: 0x0021F968 File Offset: 0x0021ED68
		public void Remove(object value)
		{
			this.ValidateType(value);
			this.Remove((DataTableMapping)value);
		}

		// Token: 0x060012BE RID: 4798 RVA: 0x0021F988 File Offset: 0x0021ED88
		public void Remove(DataTableMapping value)
		{
			if (value == null)
			{
				throw ADP.TablesAddNullAttempt("value");
			}
			int num = this.IndexOf(value);
			if (-1 != num)
			{
				this.RemoveIndex(num);
				return;
			}
			throw ADP.CollectionRemoveInvalidObject(this.ItemType, this);
		}

		// Token: 0x060012BF RID: 4799 RVA: 0x0021F9C4 File Offset: 0x0021EDC4
		private void Replace(int index, DataTableMapping newValue)
		{
			this.Validate(index, newValue);
			this.items[index].Parent = null;
			newValue.Parent = this;
			this.items[index] = newValue;
		}

		// Token: 0x060012C0 RID: 4800 RVA: 0x0021FA00 File Offset: 0x0021EE00
		private void ValidateType(object value)
		{
			if (value == null)
			{
				throw ADP.TablesAddNullAttempt("value");
			}
			if (!this.ItemType.IsInstanceOfType(value))
			{
				throw ADP.NotADataTableMapping(value);
			}
		}

		// Token: 0x060012C1 RID: 4801 RVA: 0x0021FA30 File Offset: 0x0021EE30
		private void Validate(int index, DataTableMapping value)
		{
			if (value == null)
			{
				throw ADP.TablesAddNullAttempt("value");
			}
			if (value.Parent != null)
			{
				if (this != value.Parent)
				{
					throw ADP.TablesIsNotParent(this);
				}
				if (index != this.IndexOf(value))
				{
					throw ADP.TablesIsParent(this);
				}
			}
			string text = value.SourceTable;
			if (ADP.IsEmpty(text))
			{
				index = 1;
				do
				{
					text = "SourceTable" + index.ToString(CultureInfo.InvariantCulture);
					index++;
				}
				while (-1 != this.IndexOf(text));
				value.SourceTable = text;
				return;
			}
			this.ValidateSourceTable(index, text);
		}

		// Token: 0x060012C2 RID: 4802 RVA: 0x0021FABC File Offset: 0x0021EEBC
		internal void ValidateSourceTable(int index, string value)
		{
			int num = this.IndexOf(value);
			if (-1 != num && index != num)
			{
				throw ADP.TablesUniqueSourceTable(value);
			}
		}

		// Token: 0x060012C3 RID: 4803 RVA: 0x0021FAE0 File Offset: 0x0021EEE0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static DataTableMapping GetTableMappingBySchemaAction(DataTableMappingCollection tableMappings, string sourceTable, string dataSetTable, MissingMappingAction mappingAction)
		{
			if (tableMappings != null)
			{
				int num = tableMappings.IndexOf(sourceTable);
				if (-1 != num)
				{
					return tableMappings.items[num];
				}
			}
			if (ADP.IsEmpty(sourceTable))
			{
				throw ADP.InvalidSourceTable("sourceTable");
			}
			switch (mappingAction)
			{
			case MissingMappingAction.Passthrough:
				return new DataTableMapping(sourceTable, dataSetTable);
			case MissingMappingAction.Ignore:
				return null;
			case MissingMappingAction.Error:
				throw ADP.MissingTableMapping(sourceTable);
			default:
				throw ADP.InvalidMissingMappingAction(mappingAction);
			}
		}

		// Token: 0x04000BB1 RID: 2993
		private List<DataTableMapping> items;
	}
}
