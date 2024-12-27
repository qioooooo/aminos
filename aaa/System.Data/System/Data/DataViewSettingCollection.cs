using System;
using System.Collections;
using System.ComponentModel;

namespace System.Data
{
	// Token: 0x020000AF RID: 175
	[Editor("Microsoft.VSDesigner.Data.Design.DataViewSettingsCollectionEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public class DataViewSettingCollection : ICollection, IEnumerable
	{
		// Token: 0x06000C04 RID: 3076 RVA: 0x001F95E0 File Offset: 0x001F89E0
		internal DataViewSettingCollection(DataViewManager dataViewManager)
		{
			if (dataViewManager == null)
			{
				throw ExceptionBuilder.ArgumentNull("dataViewManager");
			}
			this.dataViewManager = dataViewManager;
		}

		// Token: 0x170001AA RID: 426
		public virtual DataViewSetting this[DataTable table]
		{
			get
			{
				if (table == null)
				{
					throw ExceptionBuilder.ArgumentNull("table");
				}
				DataViewSetting dataViewSetting = (DataViewSetting)this.list[table];
				if (dataViewSetting == null)
				{
					dataViewSetting = new DataViewSetting();
					this[table] = dataViewSetting;
				}
				return dataViewSetting;
			}
			set
			{
				if (table == null)
				{
					throw ExceptionBuilder.ArgumentNull("table");
				}
				value.SetDataViewManager(this.dataViewManager);
				value.SetDataTable(table);
				this.list[table] = value;
			}
		}

		// Token: 0x06000C07 RID: 3079 RVA: 0x001F9690 File Offset: 0x001F8A90
		private DataTable GetTable(string tableName)
		{
			DataTable dataTable = null;
			DataSet dataSet = this.dataViewManager.DataSet;
			if (dataSet != null)
			{
				dataTable = dataSet.Tables[tableName];
			}
			return dataTable;
		}

		// Token: 0x06000C08 RID: 3080 RVA: 0x001F96BC File Offset: 0x001F8ABC
		private DataTable GetTable(int index)
		{
			DataTable dataTable = null;
			DataSet dataSet = this.dataViewManager.DataSet;
			if (dataSet != null)
			{
				dataTable = dataSet.Tables[index];
			}
			return dataTable;
		}

		// Token: 0x170001AB RID: 427
		public virtual DataViewSetting this[string tableName]
		{
			get
			{
				DataTable table = this.GetTable(tableName);
				if (table != null)
				{
					return this[table];
				}
				return null;
			}
		}

		// Token: 0x170001AC RID: 428
		public virtual DataViewSetting this[int index]
		{
			get
			{
				DataTable table = this.GetTable(index);
				if (table != null)
				{
					return this[table];
				}
				return null;
			}
			set
			{
				DataTable table = this.GetTable(index);
				if (table != null)
				{
					this[table] = value;
				}
			}
		}

		// Token: 0x06000C0C RID: 3084 RVA: 0x001F9750 File Offset: 0x001F8B50
		public void CopyTo(Array ar, int index)
		{
			foreach (object obj in this)
			{
				ar.SetValue(obj, index++);
			}
		}

		// Token: 0x06000C0D RID: 3085 RVA: 0x001F9780 File Offset: 0x001F8B80
		public void CopyTo(DataViewSetting[] ar, int index)
		{
			foreach (object obj in this)
			{
				ar.SetValue(obj, index++);
			}
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06000C0E RID: 3086 RVA: 0x001F97B0 File Offset: 0x001F8BB0
		[Browsable(false)]
		public virtual int Count
		{
			get
			{
				DataSet dataSet = this.dataViewManager.DataSet;
				if (dataSet != null)
				{
					return dataSet.Tables.Count;
				}
				return 0;
			}
		}

		// Token: 0x06000C0F RID: 3087 RVA: 0x001F97DC File Offset: 0x001F8BDC
		public IEnumerator GetEnumerator()
		{
			return new DataViewSettingCollection.DataViewSettingsEnumerator(this.dataViewManager);
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x06000C10 RID: 3088 RVA: 0x001F97F4 File Offset: 0x001F8BF4
		[Browsable(false)]
		public bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x06000C11 RID: 3089 RVA: 0x001F9804 File Offset: 0x001F8C04
		[Browsable(false)]
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06000C12 RID: 3090 RVA: 0x001F9814 File Offset: 0x001F8C14
		[Browsable(false)]
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06000C13 RID: 3091 RVA: 0x001F9824 File Offset: 0x001F8C24
		internal void Remove(DataTable table)
		{
			this.list.Remove(table);
		}

		// Token: 0x04000867 RID: 2151
		private readonly DataViewManager dataViewManager;

		// Token: 0x04000868 RID: 2152
		private readonly Hashtable list = new Hashtable();

		// Token: 0x020000B0 RID: 176
		private sealed class DataViewSettingsEnumerator : IEnumerator
		{
			// Token: 0x06000C14 RID: 3092 RVA: 0x001F9840 File Offset: 0x001F8C40
			public DataViewSettingsEnumerator(DataViewManager dvm)
			{
				DataSet dataSet = dvm.DataSet;
				if (dataSet != null)
				{
					this.dataViewSettings = dvm.DataViewSettings;
					this.tableEnumerator = dvm.DataSet.Tables.GetEnumerator();
					return;
				}
				this.dataViewSettings = null;
				this.tableEnumerator = DataSet.zeroTables.GetEnumerator();
			}

			// Token: 0x06000C15 RID: 3093 RVA: 0x001F9898 File Offset: 0x001F8C98
			public bool MoveNext()
			{
				return this.tableEnumerator.MoveNext();
			}

			// Token: 0x06000C16 RID: 3094 RVA: 0x001F98B0 File Offset: 0x001F8CB0
			public void Reset()
			{
				this.tableEnumerator.Reset();
			}

			// Token: 0x170001B1 RID: 433
			// (get) Token: 0x06000C17 RID: 3095 RVA: 0x001F98C8 File Offset: 0x001F8CC8
			public object Current
			{
				get
				{
					return this.dataViewSettings[(DataTable)this.tableEnumerator.Current];
				}
			}

			// Token: 0x04000869 RID: 2153
			private DataViewSettingCollection dataViewSettings;

			// Token: 0x0400086A RID: 2154
			private IEnumerator tableEnumerator;
		}
	}
}
