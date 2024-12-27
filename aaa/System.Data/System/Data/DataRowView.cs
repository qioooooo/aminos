using System;
using System.ComponentModel;

namespace System.Data
{
	// Token: 0x02000092 RID: 146
	public class DataRowView : ICustomTypeDescriptor, IEditableObject, IDataErrorInfo, INotifyPropertyChanged
	{
		// Token: 0x0600084D RID: 2125 RVA: 0x001E3AEC File Offset: 0x001E2EEC
		internal DataRowView(DataView dataView, DataRow row)
		{
			this.dataView = dataView;
			this._row = row;
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x001E3B10 File Offset: 0x001E2F10
		public override bool Equals(object other)
		{
			return object.ReferenceEquals(this, other);
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x001E3B24 File Offset: 0x001E2F24
		public override int GetHashCode()
		{
			return this.Row.GetHashCode();
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06000850 RID: 2128 RVA: 0x001E3B3C File Offset: 0x001E2F3C
		public DataView DataView
		{
			get
			{
				return this.dataView;
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000851 RID: 2129 RVA: 0x001E3B50 File Offset: 0x001E2F50
		internal int ObjectID
		{
			get
			{
				return this._row.ObjectID;
			}
		}

		// Token: 0x17000101 RID: 257
		public object this[int ndx]
		{
			get
			{
				return this.Row[ndx, this.RowVersionDefault];
			}
			set
			{
				if (!this.dataView.AllowEdit && !this.IsNew)
				{
					throw ExceptionBuilder.CanNotEdit();
				}
				this.SetColumnValue(this.dataView.Table.Columns[ndx], value);
			}
		}

		// Token: 0x17000102 RID: 258
		public object this[string property]
		{
			get
			{
				DataColumn dataColumn = this.dataView.Table.Columns[property];
				if (dataColumn != null)
				{
					return this.Row[dataColumn, this.RowVersionDefault];
				}
				if (this.dataView.Table.DataSet != null && this.dataView.Table.DataSet.Relations.Contains(property))
				{
					return this.CreateChildView(property);
				}
				throw ExceptionBuilder.PropertyNotFound(property, this.dataView.Table.TableName);
			}
			set
			{
				DataColumn dataColumn = this.dataView.Table.Columns[property];
				if (dataColumn == null)
				{
					throw ExceptionBuilder.SetFailed(property);
				}
				if (!this.dataView.AllowEdit && !this.IsNew)
				{
					throw ExceptionBuilder.CanNotEdit();
				}
				this.SetColumnValue(dataColumn, value);
			}
		}

		// Token: 0x17000103 RID: 259
		string IDataErrorInfo.this[string colName]
		{
			get
			{
				return this.Row.GetColumnError(colName);
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000857 RID: 2135 RVA: 0x001E3CC8 File Offset: 0x001E30C8
		string IDataErrorInfo.Error
		{
			get
			{
				return this.Row.RowError;
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000858 RID: 2136 RVA: 0x001E3CE0 File Offset: 0x001E30E0
		public DataRowVersion RowVersion
		{
			get
			{
				return this.RowVersionDefault & (DataRowVersion)(-1025);
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000859 RID: 2137 RVA: 0x001E3CFC File Offset: 0x001E30FC
		private DataRowVersion RowVersionDefault
		{
			get
			{
				return this.Row.GetDefaultRowVersion(this.dataView.RowStateFilter);
			}
		}

		// Token: 0x0600085A RID: 2138 RVA: 0x001E3D20 File Offset: 0x001E3120
		internal int GetRecord()
		{
			return this.Row.GetRecordFromVersion(this.RowVersionDefault);
		}

		// Token: 0x0600085B RID: 2139 RVA: 0x001E3D40 File Offset: 0x001E3140
		internal object GetColumnValue(DataColumn column)
		{
			return this.Row[column, this.RowVersionDefault];
		}

		// Token: 0x0600085C RID: 2140 RVA: 0x001E3D60 File Offset: 0x001E3160
		internal void SetColumnValue(DataColumn column, object value)
		{
			if (this.delayBeginEdit)
			{
				this.delayBeginEdit = false;
				this.Row.BeginEdit();
			}
			if (DataRowVersion.Original == this.RowVersionDefault)
			{
				throw ExceptionBuilder.SetFailed(column.ColumnName);
			}
			this.Row[column] = value;
		}

		// Token: 0x0600085D RID: 2141 RVA: 0x001E3DB0 File Offset: 0x001E31B0
		public DataView CreateChildView(DataRelation relation)
		{
			if (relation == null || relation.ParentKey.Table != this.DataView.Table)
			{
				throw ExceptionBuilder.CreateChildView();
			}
			int record = this.GetRecord();
			object[] keyValues = relation.ParentKey.GetKeyValues(record);
			RelatedView relatedView = new RelatedView(relation.ChildColumnsReference, keyValues);
			relatedView.SetIndex("", DataViewRowState.CurrentRows, null);
			relatedView.SetDataViewManager(this.DataView.DataViewManager);
			return relatedView;
		}

		// Token: 0x0600085E RID: 2142 RVA: 0x001E3E28 File Offset: 0x001E3228
		public DataView CreateChildView(string relationName)
		{
			return this.CreateChildView(this.DataView.Table.ChildRelations[relationName]);
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x0600085F RID: 2143 RVA: 0x001E3E54 File Offset: 0x001E3254
		public DataRow Row
		{
			get
			{
				return this._row;
			}
		}

		// Token: 0x06000860 RID: 2144 RVA: 0x001E3E68 File Offset: 0x001E3268
		public void BeginEdit()
		{
			this.delayBeginEdit = true;
		}

		// Token: 0x06000861 RID: 2145 RVA: 0x001E3E7C File Offset: 0x001E327C
		public void CancelEdit()
		{
			DataRow row = this.Row;
			if (this.IsNew)
			{
				this.dataView.FinishAddNew(false);
			}
			else
			{
				row.CancelEdit();
			}
			this.delayBeginEdit = false;
		}

		// Token: 0x06000862 RID: 2146 RVA: 0x001E3EB4 File Offset: 0x001E32B4
		public void EndEdit()
		{
			if (this.IsNew)
			{
				this.dataView.FinishAddNew(true);
			}
			else
			{
				this.Row.EndEdit();
			}
			this.delayBeginEdit = false;
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000863 RID: 2147 RVA: 0x001E3EEC File Offset: 0x001E32EC
		public bool IsNew
		{
			get
			{
				return this._row == this.dataView.addNewRow;
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000864 RID: 2148 RVA: 0x001E3F0C File Offset: 0x001E330C
		public bool IsEdit
		{
			get
			{
				return this.Row.HasVersion(DataRowVersion.Proposed) || this.delayBeginEdit;
			}
		}

		// Token: 0x06000865 RID: 2149 RVA: 0x001E3F34 File Offset: 0x001E3334
		public void Delete()
		{
			this.dataView.Delete(this.Row);
		}

		// Token: 0x06000866 RID: 2150 RVA: 0x001E3F54 File Offset: 0x001E3354
		AttributeCollection ICustomTypeDescriptor.GetAttributes()
		{
			return new AttributeCollection(null);
		}

		// Token: 0x06000867 RID: 2151 RVA: 0x001E3F68 File Offset: 0x001E3368
		string ICustomTypeDescriptor.GetClassName()
		{
			return null;
		}

		// Token: 0x06000868 RID: 2152 RVA: 0x001E3F78 File Offset: 0x001E3378
		string ICustomTypeDescriptor.GetComponentName()
		{
			return null;
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x001E3F88 File Offset: 0x001E3388
		TypeConverter ICustomTypeDescriptor.GetConverter()
		{
			return null;
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x001E3F98 File Offset: 0x001E3398
		EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
		{
			return null;
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x001E3FA8 File Offset: 0x001E33A8
		PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
		{
			return null;
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x001E3FB8 File Offset: 0x001E33B8
		object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
		{
			return null;
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x001E3FC8 File Offset: 0x001E33C8
		EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
		{
			return new EventDescriptorCollection(null);
		}

		// Token: 0x0600086E RID: 2158 RVA: 0x001E3FDC File Offset: 0x001E33DC
		EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
		{
			return new EventDescriptorCollection(null);
		}

		// Token: 0x0600086F RID: 2159 RVA: 0x001E3FF0 File Offset: 0x001E33F0
		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
		{
			return ((ICustomTypeDescriptor)this).GetProperties(null);
		}

		// Token: 0x06000870 RID: 2160 RVA: 0x001E4004 File Offset: 0x001E3404
		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
		{
			if (this.dataView.Table == null)
			{
				return DataRowView.zeroPropertyDescriptorCollection;
			}
			return this.dataView.Table.GetPropertyDescriptorCollection(attributes);
		}

		// Token: 0x06000871 RID: 2161 RVA: 0x001E4038 File Offset: 0x001E3438
		object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
		{
			return this;
		}

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x06000872 RID: 2162 RVA: 0x001E4048 File Offset: 0x001E3448
		// (remove) Token: 0x06000873 RID: 2163 RVA: 0x001E406C File Offset: 0x001E346C
		public event PropertyChangedEventHandler PropertyChanged
		{
			add
			{
				this.onPropertyChanged = (PropertyChangedEventHandler)Delegate.Combine(this.onPropertyChanged, value);
			}
			remove
			{
				this.onPropertyChanged = (PropertyChangedEventHandler)Delegate.Remove(this.onPropertyChanged, value);
			}
		}

		// Token: 0x06000874 RID: 2164 RVA: 0x001E4090 File Offset: 0x001E3490
		internal void RaisePropertyChangedEvent(string propName)
		{
			if (this.onPropertyChanged != null)
			{
				this.onPropertyChanged(this, new PropertyChangedEventArgs(propName));
			}
		}

		// Token: 0x04000783 RID: 1923
		private readonly DataView dataView;

		// Token: 0x04000784 RID: 1924
		private readonly DataRow _row;

		// Token: 0x04000785 RID: 1925
		private bool delayBeginEdit;

		// Token: 0x04000786 RID: 1926
		private static PropertyDescriptorCollection zeroPropertyDescriptorCollection = new PropertyDescriptorCollection(null);

		// Token: 0x04000787 RID: 1927
		private PropertyChangedEventHandler onPropertyChanged;
	}
}
