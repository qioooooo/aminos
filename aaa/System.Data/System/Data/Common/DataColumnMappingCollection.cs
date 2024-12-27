using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace System.Data.Common
{
	// Token: 0x0200011B RID: 283
	public sealed class DataColumnMappingCollection : MarshalByRefObject, IColumnMappingCollection, IList, ICollection, IEnumerable
	{
		// Token: 0x17000253 RID: 595
		// (get) Token: 0x060011FF RID: 4607 RVA: 0x0021DEB8 File Offset: 0x0021D2B8
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06001200 RID: 4608 RVA: 0x0021DEC8 File Offset: 0x0021D2C8
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x06001201 RID: 4609 RVA: 0x0021DED8 File Offset: 0x0021D2D8
		bool IList.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x06001202 RID: 4610 RVA: 0x0021DEE8 File Offset: 0x0021D2E8
		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000257 RID: 599
		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				this.ValidateType(value);
				this[index] = (DataColumnMapping)value;
			}
		}

		// Token: 0x17000258 RID: 600
		object IColumnMappingCollection.this[string index]
		{
			get
			{
				return this[index];
			}
			set
			{
				this.ValidateType(value);
				this[index] = (DataColumnMapping)value;
			}
		}

		// Token: 0x06001207 RID: 4615 RVA: 0x0021DF68 File Offset: 0x0021D368
		IColumnMapping IColumnMappingCollection.Add(string sourceColumnName, string dataSetColumnName)
		{
			return this.Add(sourceColumnName, dataSetColumnName);
		}

		// Token: 0x06001208 RID: 4616 RVA: 0x0021DF80 File Offset: 0x0021D380
		IColumnMapping IColumnMappingCollection.GetByDataSetColumn(string dataSetColumnName)
		{
			return this.GetByDataSetColumn(dataSetColumnName);
		}

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06001209 RID: 4617 RVA: 0x0021DF94 File Offset: 0x0021D394
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[ResDescription("DataColumnMappings_Count")]
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

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x0600120A RID: 4618 RVA: 0x0021DFB8 File Offset: 0x0021D3B8
		private Type ItemType
		{
			get
			{
				return typeof(DataColumnMapping);
			}
		}

		// Token: 0x1700025B RID: 603
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[ResDescription("DataColumnMappings_Item")]
		public DataColumnMapping this[int index]
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

		// Token: 0x1700025C RID: 604
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[ResDescription("DataColumnMappings_Item")]
		public DataColumnMapping this[string sourceColumn]
		{
			get
			{
				int num = this.RangeCheck(sourceColumn);
				return this.items[num];
			}
			set
			{
				int num = this.RangeCheck(sourceColumn);
				this.Replace(num, value);
			}
		}

		// Token: 0x0600120F RID: 4623 RVA: 0x0021E050 File Offset: 0x0021D450
		public int Add(object value)
		{
			this.ValidateType(value);
			this.Add((DataColumnMapping)value);
			return this.Count - 1;
		}

		// Token: 0x06001210 RID: 4624 RVA: 0x0021E07C File Offset: 0x0021D47C
		private DataColumnMapping Add(DataColumnMapping value)
		{
			this.AddWithoutEvents(value);
			return value;
		}

		// Token: 0x06001211 RID: 4625 RVA: 0x0021E094 File Offset: 0x0021D494
		public DataColumnMapping Add(string sourceColumn, string dataSetColumn)
		{
			return this.Add(new DataColumnMapping(sourceColumn, dataSetColumn));
		}

		// Token: 0x06001212 RID: 4626 RVA: 0x0021E0B0 File Offset: 0x0021D4B0
		public void AddRange(DataColumnMapping[] values)
		{
			this.AddEnumerableRange(values, false);
		}

		// Token: 0x06001213 RID: 4627 RVA: 0x0021E0C8 File Offset: 0x0021D4C8
		public void AddRange(Array values)
		{
			this.AddEnumerableRange(values, false);
		}

		// Token: 0x06001214 RID: 4628 RVA: 0x0021E0E0 File Offset: 0x0021D4E0
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
						this.AddWithoutEvents(cloneable.Clone() as DataColumnMapping);
					}
					return;
				}
			}
			foreach (object obj3 in values)
			{
				DataColumnMapping dataColumnMapping = (DataColumnMapping)obj3;
				this.AddWithoutEvents(dataColumnMapping);
			}
		}

		// Token: 0x06001215 RID: 4629 RVA: 0x0021E200 File Offset: 0x0021D600
		private void AddWithoutEvents(DataColumnMapping value)
		{
			this.Validate(-1, value);
			value.Parent = this;
			this.ArrayList().Add(value);
		}

		// Token: 0x06001216 RID: 4630 RVA: 0x0021E228 File Offset: 0x0021D628
		private List<DataColumnMapping> ArrayList()
		{
			if (this.items == null)
			{
				this.items = new List<DataColumnMapping>();
			}
			return this.items;
		}

		// Token: 0x06001217 RID: 4631 RVA: 0x0021E250 File Offset: 0x0021D650
		public void Clear()
		{
			if (0 < this.Count)
			{
				this.ClearWithoutEvents();
			}
		}

		// Token: 0x06001218 RID: 4632 RVA: 0x0021E26C File Offset: 0x0021D66C
		private void ClearWithoutEvents()
		{
			if (this.items != null)
			{
				foreach (DataColumnMapping dataColumnMapping in this.items)
				{
					dataColumnMapping.Parent = null;
				}
				this.items.Clear();
			}
		}

		// Token: 0x06001219 RID: 4633 RVA: 0x0021E2E0 File Offset: 0x0021D6E0
		public bool Contains(string value)
		{
			return -1 != this.IndexOf(value);
		}

		// Token: 0x0600121A RID: 4634 RVA: 0x0021E2FC File Offset: 0x0021D6FC
		public bool Contains(object value)
		{
			return -1 != this.IndexOf(value);
		}

		// Token: 0x0600121B RID: 4635 RVA: 0x0021E318 File Offset: 0x0021D718
		public void CopyTo(Array array, int index)
		{
			((ICollection)this.ArrayList()).CopyTo(array, index);
		}

		// Token: 0x0600121C RID: 4636 RVA: 0x0021E334 File Offset: 0x0021D734
		public void CopyTo(DataColumnMapping[] array, int index)
		{
			this.ArrayList().CopyTo(array, index);
		}

		// Token: 0x0600121D RID: 4637 RVA: 0x0021E350 File Offset: 0x0021D750
		public DataColumnMapping GetByDataSetColumn(string value)
		{
			int num = this.IndexOfDataSetColumn(value);
			if (0 > num)
			{
				throw ADP.ColumnsDataSetColumn(value);
			}
			return this.items[num];
		}

		// Token: 0x0600121E RID: 4638 RVA: 0x0021E37C File Offset: 0x0021D77C
		public IEnumerator GetEnumerator()
		{
			return this.ArrayList().GetEnumerator();
		}

		// Token: 0x0600121F RID: 4639 RVA: 0x0021E39C File Offset: 0x0021D79C
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

		// Token: 0x06001220 RID: 4640 RVA: 0x0021E3D8 File Offset: 0x0021D7D8
		public int IndexOf(string sourceColumn)
		{
			if (!ADP.IsEmpty(sourceColumn))
			{
				int count = this.Count;
				for (int i = 0; i < count; i++)
				{
					if (ADP.SrcCompare(sourceColumn, this.items[i].SourceColumn) == 0)
					{
						return i;
					}
				}
			}
			return -1;
		}

		// Token: 0x06001221 RID: 4641 RVA: 0x0021E41C File Offset: 0x0021D81C
		public int IndexOfDataSetColumn(string dataSetColumn)
		{
			if (!ADP.IsEmpty(dataSetColumn))
			{
				int count = this.Count;
				for (int i = 0; i < count; i++)
				{
					if (ADP.DstCompare(dataSetColumn, this.items[i].DataSetColumn) == 0)
					{
						return i;
					}
				}
			}
			return -1;
		}

		// Token: 0x06001222 RID: 4642 RVA: 0x0021E460 File Offset: 0x0021D860
		public void Insert(int index, object value)
		{
			this.ValidateType(value);
			this.Insert(index, (DataColumnMapping)value);
		}

		// Token: 0x06001223 RID: 4643 RVA: 0x0021E484 File Offset: 0x0021D884
		public void Insert(int index, DataColumnMapping value)
		{
			if (value == null)
			{
				throw ADP.ColumnsAddNullAttempt("value");
			}
			this.Validate(-1, value);
			value.Parent = this;
			this.ArrayList().Insert(index, value);
		}

		// Token: 0x06001224 RID: 4644 RVA: 0x0021E4BC File Offset: 0x0021D8BC
		private void RangeCheck(int index)
		{
			if (index < 0 || this.Count <= index)
			{
				throw ADP.ColumnsIndexInt32(index, this);
			}
		}

		// Token: 0x06001225 RID: 4645 RVA: 0x0021E4E0 File Offset: 0x0021D8E0
		private int RangeCheck(string sourceColumn)
		{
			int num = this.IndexOf(sourceColumn);
			if (num < 0)
			{
				throw ADP.ColumnsIndexSource(sourceColumn);
			}
			return num;
		}

		// Token: 0x06001226 RID: 4646 RVA: 0x0021E504 File Offset: 0x0021D904
		public void RemoveAt(int index)
		{
			this.RangeCheck(index);
			this.RemoveIndex(index);
		}

		// Token: 0x06001227 RID: 4647 RVA: 0x0021E520 File Offset: 0x0021D920
		public void RemoveAt(string sourceColumn)
		{
			int num = this.RangeCheck(sourceColumn);
			this.RemoveIndex(num);
		}

		// Token: 0x06001228 RID: 4648 RVA: 0x0021E53C File Offset: 0x0021D93C
		private void RemoveIndex(int index)
		{
			this.items[index].Parent = null;
			this.items.RemoveAt(index);
		}

		// Token: 0x06001229 RID: 4649 RVA: 0x0021E568 File Offset: 0x0021D968
		public void Remove(object value)
		{
			this.ValidateType(value);
			this.Remove((DataColumnMapping)value);
		}

		// Token: 0x0600122A RID: 4650 RVA: 0x0021E588 File Offset: 0x0021D988
		public void Remove(DataColumnMapping value)
		{
			if (value == null)
			{
				throw ADP.ColumnsAddNullAttempt("value");
			}
			int num = this.IndexOf(value);
			if (-1 != num)
			{
				this.RemoveIndex(num);
				return;
			}
			throw ADP.CollectionRemoveInvalidObject(this.ItemType, this);
		}

		// Token: 0x0600122B RID: 4651 RVA: 0x0021E5C4 File Offset: 0x0021D9C4
		private void Replace(int index, DataColumnMapping newValue)
		{
			this.Validate(index, newValue);
			this.items[index].Parent = null;
			newValue.Parent = this;
			this.items[index] = newValue;
		}

		// Token: 0x0600122C RID: 4652 RVA: 0x0021E600 File Offset: 0x0021DA00
		private void ValidateType(object value)
		{
			if (value == null)
			{
				throw ADP.ColumnsAddNullAttempt("value");
			}
			if (!this.ItemType.IsInstanceOfType(value))
			{
				throw ADP.NotADataColumnMapping(value);
			}
		}

		// Token: 0x0600122D RID: 4653 RVA: 0x0021E630 File Offset: 0x0021DA30
		private void Validate(int index, DataColumnMapping value)
		{
			if (value == null)
			{
				throw ADP.ColumnsAddNullAttempt("value");
			}
			if (value.Parent != null)
			{
				if (this != value.Parent)
				{
					throw ADP.ColumnsIsNotParent(this);
				}
				if (index != this.IndexOf(value))
				{
					throw ADP.ColumnsIsParent(this);
				}
			}
			string text = value.SourceColumn;
			if (ADP.IsEmpty(text))
			{
				index = 1;
				do
				{
					text = "SourceColumn" + index.ToString(CultureInfo.InvariantCulture);
					index++;
				}
				while (-1 != this.IndexOf(text));
				value.SourceColumn = text;
				return;
			}
			this.ValidateSourceColumn(index, text);
		}

		// Token: 0x0600122E RID: 4654 RVA: 0x0021E6BC File Offset: 0x0021DABC
		internal void ValidateSourceColumn(int index, string value)
		{
			int num = this.IndexOf(value);
			if (-1 != num && index != num)
			{
				throw ADP.ColumnsUniqueSourceColumn(value);
			}
		}

		// Token: 0x0600122F RID: 4655 RVA: 0x0021E6E0 File Offset: 0x0021DAE0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static DataColumn GetDataColumn(DataColumnMappingCollection columnMappings, string sourceColumn, Type dataType, DataTable dataTable, MissingMappingAction mappingAction, MissingSchemaAction schemaAction)
		{
			if (columnMappings != null)
			{
				int num = columnMappings.IndexOf(sourceColumn);
				if (-1 != num)
				{
					return columnMappings.items[num].GetDataColumnBySchemaAction(dataTable, dataType, schemaAction);
				}
			}
			if (ADP.IsEmpty(sourceColumn))
			{
				throw ADP.InvalidSourceColumn("sourceColumn");
			}
			switch (mappingAction)
			{
			case MissingMappingAction.Passthrough:
				return DataColumnMapping.GetDataColumnBySchemaAction(sourceColumn, sourceColumn, dataTable, dataType, schemaAction);
			case MissingMappingAction.Ignore:
				return null;
			case MissingMappingAction.Error:
				throw ADP.MissingColumnMapping(sourceColumn);
			default:
				throw ADP.InvalidMissingMappingAction(mappingAction);
			}
		}

		// Token: 0x06001230 RID: 4656 RVA: 0x0021E75C File Offset: 0x0021DB5C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static DataColumnMapping GetColumnMappingBySchemaAction(DataColumnMappingCollection columnMappings, string sourceColumn, MissingMappingAction mappingAction)
		{
			if (columnMappings != null)
			{
				int num = columnMappings.IndexOf(sourceColumn);
				if (-1 != num)
				{
					return columnMappings.items[num];
				}
			}
			if (ADP.IsEmpty(sourceColumn))
			{
				throw ADP.InvalidSourceColumn("sourceColumn");
			}
			switch (mappingAction)
			{
			case MissingMappingAction.Passthrough:
				return new DataColumnMapping(sourceColumn, sourceColumn);
			case MissingMappingAction.Ignore:
				return null;
			case MissingMappingAction.Error:
				throw ADP.MissingColumnMapping(sourceColumn);
			default:
				throw ADP.InvalidMissingMappingAction(mappingAction);
			}
		}

		// Token: 0x04000B7C RID: 2940
		private List<DataColumnMapping> items;
	}
}
