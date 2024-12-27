using System;
using System.ComponentModel;
using System.Data.Common;

namespace System.Data.Odbc
{
	// Token: 0x020001E4 RID: 484
	[DefaultEvent("RowUpdated")]
	[ToolboxItem("Microsoft.VSDesigner.Data.VS.OdbcDataAdapterToolboxItem, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[Designer("Microsoft.VSDesigner.Data.VS.OdbcDataAdapterDesigner, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public sealed class OdbcDataAdapter : DbDataAdapter, IDbDataAdapter, IDataAdapter, ICloneable
	{
		// Token: 0x06001AFA RID: 6906 RVA: 0x00244B08 File Offset: 0x00243F08
		public OdbcDataAdapter()
		{
			GC.SuppressFinalize(this);
		}

		// Token: 0x06001AFB RID: 6907 RVA: 0x00244B24 File Offset: 0x00243F24
		public OdbcDataAdapter(OdbcCommand selectCommand)
			: this()
		{
			this.SelectCommand = selectCommand;
		}

		// Token: 0x06001AFC RID: 6908 RVA: 0x00244B40 File Offset: 0x00243F40
		public OdbcDataAdapter(string selectCommandText, OdbcConnection selectConnection)
			: this()
		{
			this.SelectCommand = new OdbcCommand(selectCommandText, selectConnection);
		}

		// Token: 0x06001AFD RID: 6909 RVA: 0x00244B60 File Offset: 0x00243F60
		public OdbcDataAdapter(string selectCommandText, string selectConnectionString)
			: this()
		{
			OdbcConnection odbcConnection = new OdbcConnection(selectConnectionString);
			this.SelectCommand = new OdbcCommand(selectCommandText, odbcConnection);
		}

		// Token: 0x06001AFE RID: 6910 RVA: 0x00244B88 File Offset: 0x00243F88
		private OdbcDataAdapter(OdbcDataAdapter from)
			: base(from)
		{
			GC.SuppressFinalize(this);
		}

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x06001AFF RID: 6911 RVA: 0x00244BA4 File Offset: 0x00243FA4
		// (set) Token: 0x06001B00 RID: 6912 RVA: 0x00244BB8 File Offset: 0x00243FB8
		[ResDescription("DbDataAdapter_DeleteCommand")]
		[Editor("Microsoft.VSDesigner.Data.Design.DBCommandEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[ResCategory("DataCategory_Update")]
		[DefaultValue(null)]
		public new OdbcCommand DeleteCommand
		{
			get
			{
				return this._deleteCommand;
			}
			set
			{
				this._deleteCommand = value;
			}
		}

		// Token: 0x17000390 RID: 912
		// (get) Token: 0x06001B01 RID: 6913 RVA: 0x00244BCC File Offset: 0x00243FCC
		// (set) Token: 0x06001B02 RID: 6914 RVA: 0x00244BE0 File Offset: 0x00243FE0
		IDbCommand IDbDataAdapter.DeleteCommand
		{
			get
			{
				return this._deleteCommand;
			}
			set
			{
				this._deleteCommand = (OdbcCommand)value;
			}
		}

		// Token: 0x17000391 RID: 913
		// (get) Token: 0x06001B03 RID: 6915 RVA: 0x00244BFC File Offset: 0x00243FFC
		// (set) Token: 0x06001B04 RID: 6916 RVA: 0x00244C10 File Offset: 0x00244010
		[DefaultValue(null)]
		[Editor("Microsoft.VSDesigner.Data.Design.DBCommandEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[ResCategory("DataCategory_Update")]
		[ResDescription("DbDataAdapter_InsertCommand")]
		public new OdbcCommand InsertCommand
		{
			get
			{
				return this._insertCommand;
			}
			set
			{
				this._insertCommand = value;
			}
		}

		// Token: 0x17000392 RID: 914
		// (get) Token: 0x06001B05 RID: 6917 RVA: 0x00244C24 File Offset: 0x00244024
		// (set) Token: 0x06001B06 RID: 6918 RVA: 0x00244C38 File Offset: 0x00244038
		IDbCommand IDbDataAdapter.InsertCommand
		{
			get
			{
				return this._insertCommand;
			}
			set
			{
				this._insertCommand = (OdbcCommand)value;
			}
		}

		// Token: 0x17000393 RID: 915
		// (get) Token: 0x06001B07 RID: 6919 RVA: 0x00244C54 File Offset: 0x00244054
		// (set) Token: 0x06001B08 RID: 6920 RVA: 0x00244C68 File Offset: 0x00244068
		[Editor("Microsoft.VSDesigner.Data.Design.DBCommandEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[ResCategory("DataCategory_Fill")]
		[ResDescription("DbDataAdapter_SelectCommand")]
		[DefaultValue(null)]
		public new OdbcCommand SelectCommand
		{
			get
			{
				return this._selectCommand;
			}
			set
			{
				this._selectCommand = value;
			}
		}

		// Token: 0x17000394 RID: 916
		// (get) Token: 0x06001B09 RID: 6921 RVA: 0x00244C7C File Offset: 0x0024407C
		// (set) Token: 0x06001B0A RID: 6922 RVA: 0x00244C90 File Offset: 0x00244090
		IDbCommand IDbDataAdapter.SelectCommand
		{
			get
			{
				return this._selectCommand;
			}
			set
			{
				this._selectCommand = (OdbcCommand)value;
			}
		}

		// Token: 0x17000395 RID: 917
		// (get) Token: 0x06001B0B RID: 6923 RVA: 0x00244CAC File Offset: 0x002440AC
		// (set) Token: 0x06001B0C RID: 6924 RVA: 0x00244CC0 File Offset: 0x002440C0
		[ResCategory("DataCategory_Update")]
		[Editor("Microsoft.VSDesigner.Data.Design.DBCommandEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[ResDescription("DbDataAdapter_UpdateCommand")]
		[DefaultValue(null)]
		public new OdbcCommand UpdateCommand
		{
			get
			{
				return this._updateCommand;
			}
			set
			{
				this._updateCommand = value;
			}
		}

		// Token: 0x17000396 RID: 918
		// (get) Token: 0x06001B0D RID: 6925 RVA: 0x00244CD4 File Offset: 0x002440D4
		// (set) Token: 0x06001B0E RID: 6926 RVA: 0x00244CE8 File Offset: 0x002440E8
		IDbCommand IDbDataAdapter.UpdateCommand
		{
			get
			{
				return this._updateCommand;
			}
			set
			{
				this._updateCommand = (OdbcCommand)value;
			}
		}

		// Token: 0x14000024 RID: 36
		// (add) Token: 0x06001B0F RID: 6927 RVA: 0x00244D04 File Offset: 0x00244104
		// (remove) Token: 0x06001B10 RID: 6928 RVA: 0x00244D24 File Offset: 0x00244124
		[ResDescription("DbDataAdapter_RowUpdated")]
		[ResCategory("DataCategory_Update")]
		public event OdbcRowUpdatedEventHandler RowUpdated
		{
			add
			{
				base.Events.AddHandler(OdbcDataAdapter.EventRowUpdated, value);
			}
			remove
			{
				base.Events.RemoveHandler(OdbcDataAdapter.EventRowUpdated, value);
			}
		}

		// Token: 0x14000025 RID: 37
		// (add) Token: 0x06001B11 RID: 6929 RVA: 0x00244D44 File Offset: 0x00244144
		// (remove) Token: 0x06001B12 RID: 6930 RVA: 0x00244DA8 File Offset: 0x002441A8
		[ResDescription("DbDataAdapter_RowUpdating")]
		[ResCategory("DataCategory_Update")]
		public event OdbcRowUpdatingEventHandler RowUpdating
		{
			add
			{
				OdbcRowUpdatingEventHandler odbcRowUpdatingEventHandler = (OdbcRowUpdatingEventHandler)base.Events[OdbcDataAdapter.EventRowUpdating];
				if (odbcRowUpdatingEventHandler != null && value.Target is OdbcCommandBuilder)
				{
					OdbcRowUpdatingEventHandler odbcRowUpdatingEventHandler2 = (OdbcRowUpdatingEventHandler)ADP.FindBuilder(odbcRowUpdatingEventHandler);
					if (odbcRowUpdatingEventHandler2 != null)
					{
						base.Events.RemoveHandler(OdbcDataAdapter.EventRowUpdating, odbcRowUpdatingEventHandler2);
					}
				}
				base.Events.AddHandler(OdbcDataAdapter.EventRowUpdating, value);
			}
			remove
			{
				base.Events.RemoveHandler(OdbcDataAdapter.EventRowUpdating, value);
			}
		}

		// Token: 0x06001B13 RID: 6931 RVA: 0x00244DC8 File Offset: 0x002441C8
		object ICloneable.Clone()
		{
			return new OdbcDataAdapter(this);
		}

		// Token: 0x06001B14 RID: 6932 RVA: 0x00244DDC File Offset: 0x002441DC
		protected override RowUpdatedEventArgs CreateRowUpdatedEvent(DataRow dataRow, IDbCommand command, StatementType statementType, DataTableMapping tableMapping)
		{
			return new OdbcRowUpdatedEventArgs(dataRow, command, statementType, tableMapping);
		}

		// Token: 0x06001B15 RID: 6933 RVA: 0x00244DF4 File Offset: 0x002441F4
		protected override RowUpdatingEventArgs CreateRowUpdatingEvent(DataRow dataRow, IDbCommand command, StatementType statementType, DataTableMapping tableMapping)
		{
			return new OdbcRowUpdatingEventArgs(dataRow, command, statementType, tableMapping);
		}

		// Token: 0x06001B16 RID: 6934 RVA: 0x00244E0C File Offset: 0x0024420C
		protected override void OnRowUpdated(RowUpdatedEventArgs value)
		{
			OdbcRowUpdatedEventHandler odbcRowUpdatedEventHandler = (OdbcRowUpdatedEventHandler)base.Events[OdbcDataAdapter.EventRowUpdated];
			if (odbcRowUpdatedEventHandler != null && value is OdbcRowUpdatedEventArgs)
			{
				odbcRowUpdatedEventHandler(this, (OdbcRowUpdatedEventArgs)value);
			}
			base.OnRowUpdated(value);
		}

		// Token: 0x06001B17 RID: 6935 RVA: 0x00244E50 File Offset: 0x00244250
		protected override void OnRowUpdating(RowUpdatingEventArgs value)
		{
			OdbcRowUpdatingEventHandler odbcRowUpdatingEventHandler = (OdbcRowUpdatingEventHandler)base.Events[OdbcDataAdapter.EventRowUpdating];
			if (odbcRowUpdatingEventHandler != null && value is OdbcRowUpdatingEventArgs)
			{
				odbcRowUpdatingEventHandler(this, (OdbcRowUpdatingEventArgs)value);
			}
			base.OnRowUpdating(value);
		}

		// Token: 0x04000FD0 RID: 4048
		private static readonly object EventRowUpdated = new object();

		// Token: 0x04000FD1 RID: 4049
		private static readonly object EventRowUpdating = new object();

		// Token: 0x04000FD2 RID: 4050
		private OdbcCommand _deleteCommand;

		// Token: 0x04000FD3 RID: 4051
		private OdbcCommand _insertCommand;

		// Token: 0x04000FD4 RID: 4052
		private OdbcCommand _selectCommand;

		// Token: 0x04000FD5 RID: 4053
		private OdbcCommand _updateCommand;
	}
}
