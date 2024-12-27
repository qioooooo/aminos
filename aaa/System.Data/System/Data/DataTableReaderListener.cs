using System;
using System.ComponentModel;

namespace System.Data
{
	// Token: 0x020000A4 RID: 164
	internal sealed class DataTableReaderListener
	{
		// Token: 0x06000B14 RID: 2836 RVA: 0x001F5EF0 File Offset: 0x001F52F0
		internal DataTableReaderListener(DataTableReader reader)
		{
			if (reader == null)
			{
				throw ExceptionBuilder.ArgumentNull("DataTableReader");
			}
			if (this.currentDataTable != null)
			{
				this.UnSubscribeEvents();
			}
			this.readerWeak = new WeakReference(reader);
			this.currentDataTable = reader.CurrentDataTable;
			if (this.currentDataTable != null)
			{
				this.SubscribeEvents();
			}
		}

		// Token: 0x06000B15 RID: 2837 RVA: 0x001F5F48 File Offset: 0x001F5348
		internal void CleanUp()
		{
			this.UnSubscribeEvents();
		}

		// Token: 0x06000B16 RID: 2838 RVA: 0x001F5F5C File Offset: 0x001F535C
		internal void UpdataTable(DataTable datatable)
		{
			if (datatable == null)
			{
				throw ExceptionBuilder.ArgumentNull("DataTable");
			}
			this.UnSubscribeEvents();
			this.currentDataTable = datatable;
			this.SubscribeEvents();
		}

		// Token: 0x06000B17 RID: 2839 RVA: 0x001F5F8C File Offset: 0x001F538C
		private void SubscribeEvents()
		{
			if (this.currentDataTable == null)
			{
				return;
			}
			if (this.isSubscribed)
			{
				return;
			}
			this.currentDataTable.Columns.ColumnPropertyChanged += this.SchemaChanged;
			this.currentDataTable.Columns.CollectionChanged += this.SchemaChanged;
			this.currentDataTable.RowChanged += this.DataChanged;
			this.currentDataTable.RowDeleted += this.DataChanged;
			this.currentDataTable.TableCleared += this.DataTableCleared;
			this.isSubscribed = true;
		}

		// Token: 0x06000B18 RID: 2840 RVA: 0x001F6030 File Offset: 0x001F5430
		private void UnSubscribeEvents()
		{
			if (this.currentDataTable == null)
			{
				return;
			}
			if (!this.isSubscribed)
			{
				return;
			}
			this.currentDataTable.Columns.ColumnPropertyChanged -= this.SchemaChanged;
			this.currentDataTable.Columns.CollectionChanged -= this.SchemaChanged;
			this.currentDataTable.RowChanged -= this.DataChanged;
			this.currentDataTable.RowDeleted -= this.DataChanged;
			this.currentDataTable.TableCleared -= this.DataTableCleared;
			this.isSubscribed = false;
		}

		// Token: 0x06000B19 RID: 2841 RVA: 0x001F60D4 File Offset: 0x001F54D4
		private void DataTableCleared(object sender, DataTableClearEventArgs e)
		{
			DataTableReader dataTableReader = (DataTableReader)this.readerWeak.Target;
			if (dataTableReader != null)
			{
				dataTableReader.DataTableCleared();
				return;
			}
			this.UnSubscribeEvents();
		}

		// Token: 0x06000B1A RID: 2842 RVA: 0x001F6104 File Offset: 0x001F5504
		private void SchemaChanged(object sender, CollectionChangeEventArgs e)
		{
			DataTableReader dataTableReader = (DataTableReader)this.readerWeak.Target;
			if (dataTableReader != null)
			{
				dataTableReader.SchemaChanged();
				return;
			}
			this.UnSubscribeEvents();
		}

		// Token: 0x06000B1B RID: 2843 RVA: 0x001F6134 File Offset: 0x001F5534
		private void DataChanged(object sender, DataRowChangeEventArgs args)
		{
			DataTableReader dataTableReader = (DataTableReader)this.readerWeak.Target;
			if (dataTableReader != null)
			{
				dataTableReader.DataChanged(args);
				return;
			}
			this.UnSubscribeEvents();
		}

		// Token: 0x04000827 RID: 2087
		private DataTable currentDataTable;

		// Token: 0x04000828 RID: 2088
		private bool isSubscribed;

		// Token: 0x04000829 RID: 2089
		private WeakReference readerWeak;
	}
}
