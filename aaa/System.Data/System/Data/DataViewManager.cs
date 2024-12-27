using System;
using System.Collections;
using System.ComponentModel;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace System.Data
{
	// Token: 0x020000AB RID: 171
	[Designer("Microsoft.VSDesigner.Data.VS.DataViewManagerDesigner, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public class DataViewManager : MarshalByValueComponent, IBindingList, IList, ICollection, IEnumerable, ITypedList
	{
		// Token: 0x06000BB7 RID: 2999 RVA: 0x001F8A50 File Offset: 0x001F7E50
		public DataViewManager()
			: this(null, false)
		{
		}

		// Token: 0x06000BB8 RID: 3000 RVA: 0x001F8A68 File Offset: 0x001F7E68
		public DataViewManager(DataSet dataSet)
			: this(dataSet, false)
		{
		}

		// Token: 0x06000BB9 RID: 3001 RVA: 0x001F8A80 File Offset: 0x001F7E80
		internal DataViewManager(DataSet dataSet, bool locked)
		{
			GC.SuppressFinalize(this);
			this.dataSet = dataSet;
			if (this.dataSet != null)
			{
				this.dataSet.Tables.CollectionChanged += this.TableCollectionChanged;
				this.dataSet.Relations.CollectionChanged += this.RelationCollectionChanged;
			}
			this.locked = locked;
			this.item = new DataViewManagerListItemTypeDescriptor(this);
			this.dataViewSettingsCollection = new DataViewSettingCollection(this);
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x06000BBA RID: 3002 RVA: 0x001F8B04 File Offset: 0x001F7F04
		// (set) Token: 0x06000BBB RID: 3003 RVA: 0x001F8B18 File Offset: 0x001F7F18
		[ResDescription("DataViewManagerDataSetDescr")]
		[DefaultValue(null)]
		public DataSet DataSet
		{
			get
			{
				return this.dataSet;
			}
			set
			{
				if (value == null)
				{
					throw ExceptionBuilder.SetFailed("DataSet to null");
				}
				if (this.locked)
				{
					throw ExceptionBuilder.SetDataSetFailed();
				}
				if (this.dataSet != null)
				{
					if (this.nViews > 0)
					{
						throw ExceptionBuilder.CanNotSetDataSet();
					}
					this.dataSet.Tables.CollectionChanged -= this.TableCollectionChanged;
					this.dataSet.Relations.CollectionChanged -= this.RelationCollectionChanged;
				}
				this.dataSet = value;
				this.dataSet.Tables.CollectionChanged += this.TableCollectionChanged;
				this.dataSet.Relations.CollectionChanged += this.RelationCollectionChanged;
				this.dataViewSettingsCollection = new DataViewSettingCollection(this);
				this.item.Reset();
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x06000BBC RID: 3004 RVA: 0x001F8BEC File Offset: 0x001F7FEC
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[ResDescription("DataViewManagerTableSettingsDescr")]
		public DataViewSettingCollection DataViewSettings
		{
			get
			{
				return this.dataViewSettingsCollection;
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06000BBD RID: 3005 RVA: 0x001F8C00 File Offset: 0x001F8000
		// (set) Token: 0x06000BBE RID: 3006 RVA: 0x001F8CEC File Offset: 0x001F80EC
		public string DataViewSettingCollectionString
		{
			get
			{
				if (this.dataSet == null)
				{
					return "";
				}
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("<DataViewSettingCollectionString>");
				foreach (object obj in this.dataSet.Tables)
				{
					DataTable dataTable = (DataTable)obj;
					DataViewSetting dataViewSetting = this.dataViewSettingsCollection[dataTable];
					stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "<{0} Sort=\"{1}\" RowFilter=\"{2}\" RowStateFilter=\"{3}\"/>", new object[] { dataTable.EncodedTableName, dataViewSetting.Sort, dataViewSetting.RowFilter, dataViewSetting.RowStateFilter });
				}
				stringBuilder.Append("</DataViewSettingCollectionString>");
				return stringBuilder.ToString();
			}
			set
			{
				if (value == null || value.Length == 0)
				{
					return;
				}
				XmlTextReader xmlTextReader = new XmlTextReader(new StringReader(value));
				xmlTextReader.WhitespaceHandling = WhitespaceHandling.None;
				xmlTextReader.Read();
				if (xmlTextReader.Name != "DataViewSettingCollectionString")
				{
					throw ExceptionBuilder.SetFailed("DataViewSettingCollectionString");
				}
				while (xmlTextReader.Read())
				{
					if (xmlTextReader.NodeType == XmlNodeType.Element)
					{
						string text = XmlConvert.DecodeName(xmlTextReader.LocalName);
						if (xmlTextReader.MoveToAttribute("Sort"))
						{
							this.dataViewSettingsCollection[text].Sort = xmlTextReader.Value;
						}
						if (xmlTextReader.MoveToAttribute("RowFilter"))
						{
							this.dataViewSettingsCollection[text].RowFilter = xmlTextReader.Value;
						}
						if (xmlTextReader.MoveToAttribute("RowStateFilter"))
						{
							this.dataViewSettingsCollection[text].RowStateFilter = (DataViewRowState)Enum.Parse(typeof(DataViewRowState), xmlTextReader.Value);
						}
					}
				}
			}
		}

		// Token: 0x06000BBF RID: 3007 RVA: 0x001F8DE4 File Offset: 0x001F81E4
		IEnumerator IEnumerable.GetEnumerator()
		{
			DataViewManagerListItemTypeDescriptor[] array = new DataViewManagerListItemTypeDescriptor[1];
			((ICollection)this).CopyTo(array, 0);
			return array.GetEnumerator();
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000BC0 RID: 3008 RVA: 0x001F8E08 File Offset: 0x001F8208
		int ICollection.Count
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06000BC1 RID: 3009 RVA: 0x001F8E18 File Offset: 0x001F8218
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000BC2 RID: 3010 RVA: 0x001F8E28 File Offset: 0x001F8228
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06000BC3 RID: 3011 RVA: 0x001F8E38 File Offset: 0x001F8238
		bool IList.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06000BC4 RID: 3012 RVA: 0x001F8E48 File Offset: 0x001F8248
		bool IList.IsFixedSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000BC5 RID: 3013 RVA: 0x001F8E58 File Offset: 0x001F8258
		void ICollection.CopyTo(Array array, int index)
		{
			array.SetValue(new DataViewManagerListItemTypeDescriptor(this), index);
		}

		// Token: 0x1700019A RID: 410
		object IList.this[int index]
		{
			get
			{
				return this.item;
			}
			set
			{
				throw ExceptionBuilder.CannotModifyCollection();
			}
		}

		// Token: 0x06000BC8 RID: 3016 RVA: 0x001F8E9C File Offset: 0x001F829C
		int IList.Add(object value)
		{
			throw ExceptionBuilder.CannotModifyCollection();
		}

		// Token: 0x06000BC9 RID: 3017 RVA: 0x001F8EB0 File Offset: 0x001F82B0
		void IList.Clear()
		{
			throw ExceptionBuilder.CannotModifyCollection();
		}

		// Token: 0x06000BCA RID: 3018 RVA: 0x001F8EC4 File Offset: 0x001F82C4
		bool IList.Contains(object value)
		{
			return value == this.item;
		}

		// Token: 0x06000BCB RID: 3019 RVA: 0x001F8EDC File Offset: 0x001F82DC
		int IList.IndexOf(object value)
		{
			if (value != this.item)
			{
				return -1;
			}
			return 1;
		}

		// Token: 0x06000BCC RID: 3020 RVA: 0x001F8EF8 File Offset: 0x001F82F8
		void IList.Insert(int index, object value)
		{
			throw ExceptionBuilder.CannotModifyCollection();
		}

		// Token: 0x06000BCD RID: 3021 RVA: 0x001F8F0C File Offset: 0x001F830C
		void IList.Remove(object value)
		{
			throw ExceptionBuilder.CannotModifyCollection();
		}

		// Token: 0x06000BCE RID: 3022 RVA: 0x001F8F20 File Offset: 0x001F8320
		void IList.RemoveAt(int index)
		{
			throw ExceptionBuilder.CannotModifyCollection();
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06000BCF RID: 3023 RVA: 0x001F8F34 File Offset: 0x001F8334
		bool IBindingList.AllowNew
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000BD0 RID: 3024 RVA: 0x001F8F44 File Offset: 0x001F8344
		object IBindingList.AddNew()
		{
			throw DataViewManager.NotSupported;
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x06000BD1 RID: 3025 RVA: 0x001F8F58 File Offset: 0x001F8358
		bool IBindingList.AllowEdit
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x06000BD2 RID: 3026 RVA: 0x001F8F68 File Offset: 0x001F8368
		bool IBindingList.AllowRemove
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x06000BD3 RID: 3027 RVA: 0x001F8F78 File Offset: 0x001F8378
		bool IBindingList.SupportsChangeNotification
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x06000BD4 RID: 3028 RVA: 0x001F8F88 File Offset: 0x001F8388
		bool IBindingList.SupportsSearching
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x06000BD5 RID: 3029 RVA: 0x001F8F98 File Offset: 0x001F8398
		bool IBindingList.SupportsSorting
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x06000BD6 RID: 3030 RVA: 0x001F8FA8 File Offset: 0x001F83A8
		bool IBindingList.IsSorted
		{
			get
			{
				throw DataViewManager.NotSupported;
			}
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x06000BD7 RID: 3031 RVA: 0x001F8FBC File Offset: 0x001F83BC
		PropertyDescriptor IBindingList.SortProperty
		{
			get
			{
				throw DataViewManager.NotSupported;
			}
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x06000BD8 RID: 3032 RVA: 0x001F8FD0 File Offset: 0x001F83D0
		ListSortDirection IBindingList.SortDirection
		{
			get
			{
				throw DataViewManager.NotSupported;
			}
		}

		// Token: 0x14000020 RID: 32
		// (add) Token: 0x06000BD9 RID: 3033 RVA: 0x001F8FE4 File Offset: 0x001F83E4
		// (remove) Token: 0x06000BDA RID: 3034 RVA: 0x001F9008 File Offset: 0x001F8408
		public event ListChangedEventHandler ListChanged
		{
			add
			{
				this.onListChanged = (ListChangedEventHandler)Delegate.Combine(this.onListChanged, value);
			}
			remove
			{
				this.onListChanged = (ListChangedEventHandler)Delegate.Remove(this.onListChanged, value);
			}
		}

		// Token: 0x06000BDB RID: 3035 RVA: 0x001F902C File Offset: 0x001F842C
		void IBindingList.AddIndex(PropertyDescriptor property)
		{
		}

		// Token: 0x06000BDC RID: 3036 RVA: 0x001F903C File Offset: 0x001F843C
		void IBindingList.ApplySort(PropertyDescriptor property, ListSortDirection direction)
		{
			throw DataViewManager.NotSupported;
		}

		// Token: 0x06000BDD RID: 3037 RVA: 0x001F9050 File Offset: 0x001F8450
		int IBindingList.Find(PropertyDescriptor property, object key)
		{
			throw DataViewManager.NotSupported;
		}

		// Token: 0x06000BDE RID: 3038 RVA: 0x001F9064 File Offset: 0x001F8464
		void IBindingList.RemoveIndex(PropertyDescriptor property)
		{
		}

		// Token: 0x06000BDF RID: 3039 RVA: 0x001F9074 File Offset: 0x001F8474
		void IBindingList.RemoveSort()
		{
			throw DataViewManager.NotSupported;
		}

		// Token: 0x06000BE0 RID: 3040 RVA: 0x001F9088 File Offset: 0x001F8488
		string ITypedList.GetListName(PropertyDescriptor[] listAccessors)
		{
			DataSet dataSet = this.DataSet;
			if (dataSet == null)
			{
				throw ExceptionBuilder.CanNotUseDataViewManager();
			}
			if (listAccessors == null || listAccessors.Length == 0)
			{
				return dataSet.DataSetName;
			}
			DataTable dataTable = dataSet.FindTable(null, listAccessors, 0);
			if (dataTable != null)
			{
				return dataTable.TableName;
			}
			return string.Empty;
		}

		// Token: 0x06000BE1 RID: 3041 RVA: 0x001F90D0 File Offset: 0x001F84D0
		PropertyDescriptorCollection ITypedList.GetItemProperties(PropertyDescriptor[] listAccessors)
		{
			DataSet dataSet = this.DataSet;
			if (dataSet == null)
			{
				throw ExceptionBuilder.CanNotUseDataViewManager();
			}
			if (listAccessors == null || listAccessors.Length == 0)
			{
				return ((ICustomTypeDescriptor)new DataViewManagerListItemTypeDescriptor(this)).GetProperties();
			}
			DataTable dataTable = dataSet.FindTable(null, listAccessors, 0);
			if (dataTable != null)
			{
				return dataTable.GetPropertyDescriptorCollection(null);
			}
			return new PropertyDescriptorCollection(null);
		}

		// Token: 0x06000BE2 RID: 3042 RVA: 0x001F911C File Offset: 0x001F851C
		public DataView CreateDataView(DataTable table)
		{
			if (this.dataSet == null)
			{
				throw ExceptionBuilder.CanNotUseDataViewManager();
			}
			DataView dataView = new DataView(table);
			dataView.SetDataViewManager(this);
			return dataView;
		}

		// Token: 0x06000BE3 RID: 3043 RVA: 0x001F9148 File Offset: 0x001F8548
		protected virtual void OnListChanged(ListChangedEventArgs e)
		{
			try
			{
				if (this.onListChanged != null)
				{
					this.onListChanged(this, e);
				}
			}
			catch (Exception ex)
			{
				if (!ADP.IsCatchableExceptionType(ex))
				{
					throw;
				}
				ExceptionBuilder.TraceExceptionWithoutRethrow(ex);
			}
		}

		// Token: 0x06000BE4 RID: 3044 RVA: 0x001F919C File Offset: 0x001F859C
		protected virtual void TableCollectionChanged(object sender, CollectionChangeEventArgs e)
		{
			PropertyDescriptor propertyDescriptor = null;
			this.OnListChanged((e.Action == CollectionChangeAction.Add) ? new ListChangedEventArgs(ListChangedType.PropertyDescriptorAdded, new DataTablePropertyDescriptor((DataTable)e.Element)) : ((e.Action == CollectionChangeAction.Refresh) ? new ListChangedEventArgs(ListChangedType.PropertyDescriptorChanged, propertyDescriptor) : ((e.Action == CollectionChangeAction.Remove) ? new ListChangedEventArgs(ListChangedType.PropertyDescriptorDeleted, new DataTablePropertyDescriptor((DataTable)e.Element)) : null)));
		}

		// Token: 0x06000BE5 RID: 3045 RVA: 0x001F9208 File Offset: 0x001F8608
		protected virtual void RelationCollectionChanged(object sender, CollectionChangeEventArgs e)
		{
			DataRelationPropertyDescriptor dataRelationPropertyDescriptor = null;
			this.OnListChanged((e.Action == CollectionChangeAction.Add) ? new ListChangedEventArgs(ListChangedType.PropertyDescriptorAdded, new DataRelationPropertyDescriptor((DataRelation)e.Element)) : ((e.Action == CollectionChangeAction.Refresh) ? new ListChangedEventArgs(ListChangedType.PropertyDescriptorChanged, dataRelationPropertyDescriptor) : ((e.Action == CollectionChangeAction.Remove) ? new ListChangedEventArgs(ListChangedType.PropertyDescriptorDeleted, new DataRelationPropertyDescriptor((DataRelation)e.Element)) : null)));
		}

		// Token: 0x0400084F RID: 2127
		private DataViewSettingCollection dataViewSettingsCollection;

		// Token: 0x04000850 RID: 2128
		private DataSet dataSet;

		// Token: 0x04000851 RID: 2129
		private DataViewManagerListItemTypeDescriptor item;

		// Token: 0x04000852 RID: 2130
		private bool locked;

		// Token: 0x04000853 RID: 2131
		internal int nViews;

		// Token: 0x04000854 RID: 2132
		private ListChangedEventHandler onListChanged;

		// Token: 0x04000855 RID: 2133
		private static NotSupportedException NotSupported = new NotSupportedException();
	}
}
