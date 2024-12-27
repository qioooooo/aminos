using System;
using System.ComponentModel;
using System.Data.Common;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace System.Data.OleDb
{
	// Token: 0x02000221 RID: 545
	[DefaultEvent("RowUpdated")]
	[ToolboxItem("Microsoft.VSDesigner.Data.VS.OleDbDataAdapterToolboxItem, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[Designer("Microsoft.VSDesigner.Data.VS.OleDbDataAdapterDesigner, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public sealed class OleDbDataAdapter : DbDataAdapter, IDbDataAdapter, IDataAdapter, ICloneable
	{
		// Token: 0x06001EFF RID: 7935 RVA: 0x0025A514 File Offset: 0x00259914
		public OleDbDataAdapter()
		{
			GC.SuppressFinalize(this);
		}

		// Token: 0x06001F00 RID: 7936 RVA: 0x0025A530 File Offset: 0x00259930
		public OleDbDataAdapter(OleDbCommand selectCommand)
			: this()
		{
			this.SelectCommand = selectCommand;
		}

		// Token: 0x06001F01 RID: 7937 RVA: 0x0025A54C File Offset: 0x0025994C
		public OleDbDataAdapter(string selectCommandText, string selectConnectionString)
			: this()
		{
			OleDbConnection oleDbConnection = new OleDbConnection(selectConnectionString);
			this.SelectCommand = new OleDbCommand(selectCommandText, oleDbConnection);
		}

		// Token: 0x06001F02 RID: 7938 RVA: 0x0025A574 File Offset: 0x00259974
		public OleDbDataAdapter(string selectCommandText, OleDbConnection selectConnection)
			: this()
		{
			this.SelectCommand = new OleDbCommand(selectCommandText, selectConnection);
		}

		// Token: 0x06001F03 RID: 7939 RVA: 0x0025A594 File Offset: 0x00259994
		private OleDbDataAdapter(OleDbDataAdapter from)
			: base(from)
		{
			GC.SuppressFinalize(this);
		}

		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x06001F04 RID: 7940 RVA: 0x0025A5B0 File Offset: 0x002599B0
		// (set) Token: 0x06001F05 RID: 7941 RVA: 0x0025A5C4 File Offset: 0x002599C4
		[DefaultValue(null)]
		[ResDescription("DbDataAdapter_DeleteCommand")]
		[Editor("Microsoft.VSDesigner.Data.Design.DBCommandEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[ResCategory("DataCategory_Update")]
		public new OleDbCommand DeleteCommand
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

		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x06001F06 RID: 7942 RVA: 0x0025A5D8 File Offset: 0x002599D8
		// (set) Token: 0x06001F07 RID: 7943 RVA: 0x0025A5EC File Offset: 0x002599EC
		IDbCommand IDbDataAdapter.DeleteCommand
		{
			get
			{
				return this._deleteCommand;
			}
			set
			{
				this._deleteCommand = (OleDbCommand)value;
			}
		}

		// Token: 0x17000439 RID: 1081
		// (get) Token: 0x06001F08 RID: 7944 RVA: 0x0025A608 File Offset: 0x00259A08
		// (set) Token: 0x06001F09 RID: 7945 RVA: 0x0025A61C File Offset: 0x00259A1C
		[DefaultValue(null)]
		[ResCategory("DataCategory_Update")]
		[Editor("Microsoft.VSDesigner.Data.Design.DBCommandEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[ResDescription("DbDataAdapter_InsertCommand")]
		public new OleDbCommand InsertCommand
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

		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x06001F0A RID: 7946 RVA: 0x0025A630 File Offset: 0x00259A30
		// (set) Token: 0x06001F0B RID: 7947 RVA: 0x0025A644 File Offset: 0x00259A44
		IDbCommand IDbDataAdapter.InsertCommand
		{
			get
			{
				return this._insertCommand;
			}
			set
			{
				this._insertCommand = (OleDbCommand)value;
			}
		}

		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x06001F0C RID: 7948 RVA: 0x0025A660 File Offset: 0x00259A60
		// (set) Token: 0x06001F0D RID: 7949 RVA: 0x0025A674 File Offset: 0x00259A74
		[ResDescription("DbDataAdapter_SelectCommand")]
		[ResCategory("DataCategory_Fill")]
		[DefaultValue(null)]
		[Editor("Microsoft.VSDesigner.Data.Design.DBCommandEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		public new OleDbCommand SelectCommand
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

		// Token: 0x1700043C RID: 1084
		// (get) Token: 0x06001F0E RID: 7950 RVA: 0x0025A688 File Offset: 0x00259A88
		// (set) Token: 0x06001F0F RID: 7951 RVA: 0x0025A69C File Offset: 0x00259A9C
		IDbCommand IDbDataAdapter.SelectCommand
		{
			get
			{
				return this._selectCommand;
			}
			set
			{
				this._selectCommand = (OleDbCommand)value;
			}
		}

		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x06001F10 RID: 7952 RVA: 0x0025A6B8 File Offset: 0x00259AB8
		// (set) Token: 0x06001F11 RID: 7953 RVA: 0x0025A6CC File Offset: 0x00259ACC
		[Editor("Microsoft.VSDesigner.Data.Design.DBCommandEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[ResDescription("DbDataAdapter_UpdateCommand")]
		[ResCategory("DataCategory_Update")]
		[DefaultValue(null)]
		public new OleDbCommand UpdateCommand
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

		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x06001F12 RID: 7954 RVA: 0x0025A6E0 File Offset: 0x00259AE0
		// (set) Token: 0x06001F13 RID: 7955 RVA: 0x0025A6F4 File Offset: 0x00259AF4
		IDbCommand IDbDataAdapter.UpdateCommand
		{
			get
			{
				return this._updateCommand;
			}
			set
			{
				this._updateCommand = (OleDbCommand)value;
			}
		}

		// Token: 0x14000027 RID: 39
		// (add) Token: 0x06001F14 RID: 7956 RVA: 0x0025A710 File Offset: 0x00259B10
		// (remove) Token: 0x06001F15 RID: 7957 RVA: 0x0025A730 File Offset: 0x00259B30
		[ResCategory("DataCategory_Update")]
		[ResDescription("DbDataAdapter_RowUpdated")]
		public event OleDbRowUpdatedEventHandler RowUpdated
		{
			add
			{
				base.Events.AddHandler(OleDbDataAdapter.EventRowUpdated, value);
			}
			remove
			{
				base.Events.RemoveHandler(OleDbDataAdapter.EventRowUpdated, value);
			}
		}

		// Token: 0x14000028 RID: 40
		// (add) Token: 0x06001F16 RID: 7958 RVA: 0x0025A750 File Offset: 0x00259B50
		// (remove) Token: 0x06001F17 RID: 7959 RVA: 0x0025A7B4 File Offset: 0x00259BB4
		[ResDescription("DbDataAdapter_RowUpdating")]
		[ResCategory("DataCategory_Update")]
		public event OleDbRowUpdatingEventHandler RowUpdating
		{
			add
			{
				OleDbRowUpdatingEventHandler oleDbRowUpdatingEventHandler = (OleDbRowUpdatingEventHandler)base.Events[OleDbDataAdapter.EventRowUpdating];
				if (oleDbRowUpdatingEventHandler != null && value.Target is DbCommandBuilder)
				{
					OleDbRowUpdatingEventHandler oleDbRowUpdatingEventHandler2 = (OleDbRowUpdatingEventHandler)ADP.FindBuilder(oleDbRowUpdatingEventHandler);
					if (oleDbRowUpdatingEventHandler2 != null)
					{
						base.Events.RemoveHandler(OleDbDataAdapter.EventRowUpdating, oleDbRowUpdatingEventHandler2);
					}
				}
				base.Events.AddHandler(OleDbDataAdapter.EventRowUpdating, value);
			}
			remove
			{
				base.Events.RemoveHandler(OleDbDataAdapter.EventRowUpdating, value);
			}
		}

		// Token: 0x06001F18 RID: 7960 RVA: 0x0025A7D4 File Offset: 0x00259BD4
		object ICloneable.Clone()
		{
			return new OleDbDataAdapter(this);
		}

		// Token: 0x06001F19 RID: 7961 RVA: 0x0025A7E8 File Offset: 0x00259BE8
		protected override RowUpdatedEventArgs CreateRowUpdatedEvent(DataRow dataRow, IDbCommand command, StatementType statementType, DataTableMapping tableMapping)
		{
			return new OleDbRowUpdatedEventArgs(dataRow, command, statementType, tableMapping);
		}

		// Token: 0x06001F1A RID: 7962 RVA: 0x0025A800 File Offset: 0x00259C00
		protected override RowUpdatingEventArgs CreateRowUpdatingEvent(DataRow dataRow, IDbCommand command, StatementType statementType, DataTableMapping tableMapping)
		{
			return new OleDbRowUpdatingEventArgs(dataRow, command, statementType, tableMapping);
		}

		// Token: 0x06001F1B RID: 7963 RVA: 0x0025A818 File Offset: 0x00259C18
		internal static void FillDataTable(OleDbDataReader dataReader, params DataTable[] dataTables)
		{
			OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter();
			oleDbDataAdapter.Fill(dataTables, dataReader, 0, 0);
		}

		// Token: 0x06001F1C RID: 7964 RVA: 0x0025A838 File Offset: 0x00259C38
		public int Fill(DataTable dataTable, object ADODBRecordSet)
		{
			PermissionSet permissionSet = new PermissionSet(PermissionState.None);
			permissionSet.AddPermission(OleDbConnection.ExecutePermission);
			permissionSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.UnmanagedCode));
			permissionSet.Demand();
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<oledb.OleDbDataAdapter.Fill|API> %d#, dataTable, ADODBRecordSet\n", base.ObjectID);
			int num;
			try
			{
				if (dataTable == null)
				{
					throw ADP.ArgumentNull("dataTable");
				}
				if (ADODBRecordSet == null)
				{
					throw ADP.ArgumentNull("adodb");
				}
				num = this.FillFromADODB(dataTable, ADODBRecordSet, null, false);
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return num;
		}

		// Token: 0x06001F1D RID: 7965 RVA: 0x0025A8CC File Offset: 0x00259CCC
		public int Fill(DataSet dataSet, object ADODBRecordSet, string srcTable)
		{
			PermissionSet permissionSet = new PermissionSet(PermissionState.None);
			permissionSet.AddPermission(OleDbConnection.ExecutePermission);
			permissionSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.UnmanagedCode));
			permissionSet.Demand();
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<oledb.OleDbDataAdapter.Fill|API> %d#, dataSet, ADODBRecordSet, srcTable='%ls'\n", base.ObjectID, srcTable);
			int num;
			try
			{
				if (dataSet == null)
				{
					throw ADP.ArgumentNull("dataSet");
				}
				if (ADODBRecordSet == null)
				{
					throw ADP.ArgumentNull("adodb");
				}
				if (ADP.IsEmpty(srcTable))
				{
					throw ADP.FillRequiresSourceTableName("srcTable");
				}
				num = this.FillFromADODB(dataSet, ADODBRecordSet, srcTable, true);
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return num;
		}

		// Token: 0x06001F1E RID: 7966 RVA: 0x0025A974 File Offset: 0x00259D74
		private int FillFromADODB(object data, object adodb, string srcTable, bool multipleResults)
		{
			bool flag = multipleResults;
			Bid.Trace("<oledb.IUnknown.QueryInterface|API|OLEDB|ADODB> ADORecordsetConstruction\n");
			UnsafeNativeMethods.ADORecordsetConstruction adorecordsetConstruction = adodb as UnsafeNativeMethods.ADORecordsetConstruction;
			UnsafeNativeMethods.ADORecordConstruction adorecordConstruction = null;
			if (adorecordsetConstruction != null)
			{
				if (multipleResults)
				{
					Bid.Trace("<oledb.Recordset15.get_ActiveConnection|API|ADODB>\n");
					if (((UnsafeNativeMethods.Recordset15)adodb).get_ActiveConnection() == null)
					{
						multipleResults = false;
					}
				}
			}
			else
			{
				Bid.Trace("<oledb.IUnknown.QueryInterface|API|OLEDB|ADODB> ADORecordConstruction\n");
				adorecordConstruction = adodb as UnsafeNativeMethods.ADORecordConstruction;
				if (adorecordConstruction != null)
				{
					multipleResults = false;
				}
			}
			int num = 0;
			if (adorecordsetConstruction != null)
			{
				int num2 = 0;
				object[] array = new object[1];
				OleDbHResult oleDbHResult;
				for (;;)
				{
					string text = null;
					if (data is DataSet)
					{
						text = OleDbDataAdapter.GetSourceTableName(srcTable, num2);
					}
					bool flag2;
					num += this.FillFromRecordset(data, adorecordsetConstruction, text, out flag2);
					if (!multipleResults)
					{
						goto IL_0120;
					}
					array[0] = DBNull.Value;
					Bid.Trace("<oledb.Recordset15.NextRecordset|API|ADODB>\n");
					object obj;
					object obj2;
					oleDbHResult = ((UnsafeNativeMethods.Recordset15)adodb).NextRecordset(out obj, out obj2);
					Bid.Trace("<oledb.Recordset15.NextRecordset|API|ADODB|RET> %08X{HRESULT}\n", oleDbHResult);
					if (OleDbHResult.S_OK > oleDbHResult)
					{
						break;
					}
					adodb = obj2;
					if (adodb == null)
					{
						goto IL_0120;
					}
					Bid.Trace("<oledb.IUnknown.QueryInterface|API|OLEDB|ADODB> ADORecordsetConstruction\n");
					adorecordsetConstruction = (UnsafeNativeMethods.ADORecordsetConstruction)adodb;
					if (flag2)
					{
						num2++;
					}
					if (adorecordsetConstruction == null)
					{
						goto IL_0120;
					}
				}
				if ((OleDbHResult)(-2146825037) != oleDbHResult)
				{
					UnsafeNativeMethods.IErrorInfo errorInfo = null;
					UnsafeNativeMethods.GetErrorInfo(0, out errorInfo);
					string empty = string.Empty;
					if (errorInfo != null)
					{
						ODB.GetErrorDescription(errorInfo, oleDbHResult, out empty);
					}
					throw new COMException(empty, (int)oleDbHResult);
				}
				IL_0120:
				if (adorecordsetConstruction != null && (flag || adodb == null))
				{
					this.FillClose(true, adorecordsetConstruction);
				}
			}
			else
			{
				if (adorecordConstruction == null)
				{
					throw ODB.Fill_NotADODB("adodb");
				}
				num = this.FillFromRecord(data, adorecordConstruction, srcTable);
				if (flag)
				{
					this.FillClose(false, adorecordConstruction);
				}
			}
			return num;
		}

		// Token: 0x06001F1F RID: 7967 RVA: 0x0025AADC File Offset: 0x00259EDC
		private int FillFromRecordset(object data, UnsafeNativeMethods.ADORecordsetConstruction recordset, string srcTable, out bool incrementResultCount)
		{
			incrementResultCount = false;
			object obj = null;
			IntPtr chapter;
			try
			{
				Bid.Trace("<oledb.ADORecordsetConstruction.get_Rowset|API|ADODB>\n");
				obj = recordset.get_Rowset();
				Bid.Trace("<oledb.ADORecordsetConstruction.get_Rowset|API|ADODB|RET> %08X{HRESULT}\n", 0);
				Bid.Trace("<oledb.ADORecordsetConstruction.get_Chapter|API|ADODB>\n");
				chapter = recordset.get_Chapter();
				Bid.Trace("<oledb.ADORecordsetConstruction.get_Chapter|API|ADODB|RET> %08X{HRESULT}\n", 0);
			}
			catch (Exception ex)
			{
				if (!ADP.IsCatchableExceptionType(ex))
				{
					throw;
				}
				throw ODB.Fill_EmptyRecordSet("ADODBRecordSet", ex);
			}
			if (obj != null)
			{
				CommandBehavior commandBehavior = ((MissingSchemaAction.AddWithKey != base.MissingSchemaAction) ? CommandBehavior.Default : CommandBehavior.KeyInfo);
				commandBehavior |= CommandBehavior.SequentialAccess;
				OleDbDataReader oleDbDataReader = null;
				try
				{
					ChapterHandle chapterHandle = ChapterHandle.CreateChapterHandle(chapter);
					oleDbDataReader = new OleDbDataReader(null, null, 0, commandBehavior);
					oleDbDataReader.InitializeIRowset(obj, chapterHandle, ADP.RecordsUnaffected);
					oleDbDataReader.BuildMetaInfo();
					incrementResultCount = 0 < oleDbDataReader.FieldCount;
					if (incrementResultCount)
					{
						if (data is DataTable)
						{
							return base.Fill((DataTable)data, oleDbDataReader);
						}
						return base.Fill((DataSet)data, srcTable, oleDbDataReader, 0, 0);
					}
				}
				finally
				{
					if (oleDbDataReader != null)
					{
						oleDbDataReader.Close();
					}
				}
				return 0;
			}
			return 0;
		}

		// Token: 0x06001F20 RID: 7968 RVA: 0x0025AC08 File Offset: 0x0025A008
		private int FillFromRecord(object data, UnsafeNativeMethods.ADORecordConstruction record, string srcTable)
		{
			object obj = null;
			try
			{
				Bid.Trace("<oledb.ADORecordConstruction.get_Row|API|ADODB>\n");
				obj = record.get_Row();
				Bid.Trace("<oledb.ADORecordConstruction.get_Row|API|ADODB|RET> %08X{HRESULT}\n", 0);
			}
			catch (Exception ex)
			{
				if (!ADP.IsCatchableExceptionType(ex))
				{
					throw;
				}
				throw ODB.Fill_EmptyRecord("adodb", ex);
			}
			if (obj != null)
			{
				CommandBehavior commandBehavior = ((MissingSchemaAction.AddWithKey != base.MissingSchemaAction) ? CommandBehavior.Default : CommandBehavior.KeyInfo);
				commandBehavior |= CommandBehavior.SingleRow | CommandBehavior.SequentialAccess;
				OleDbDataReader oleDbDataReader = null;
				try
				{
					oleDbDataReader = new OleDbDataReader(null, null, 0, commandBehavior);
					oleDbDataReader.InitializeIRow(obj, ADP.RecordsUnaffected);
					oleDbDataReader.BuildMetaInfo();
					if (data is DataTable)
					{
						return base.Fill((DataTable)data, oleDbDataReader);
					}
					return base.Fill((DataSet)data, srcTable, oleDbDataReader, 0, 0);
				}
				finally
				{
					if (oleDbDataReader != null)
					{
						oleDbDataReader.Close();
					}
				}
				return 0;
			}
			return 0;
		}

		// Token: 0x06001F21 RID: 7969 RVA: 0x0025ACF4 File Offset: 0x0025A0F4
		private void FillClose(bool isrecordset, object value)
		{
			OleDbHResult oleDbHResult;
			if (isrecordset)
			{
				Bid.Trace("<oledb.Recordset15.Close|API|ADODB>\n");
				oleDbHResult = ((UnsafeNativeMethods.Recordset15)value).Close();
				Bid.Trace("<oledb.Recordset15.Close|API|ADODB|RET> %08X{HRESULT}\n", oleDbHResult);
			}
			else
			{
				Bid.Trace("<oledb._ADORecord.Close|API|ADODB>\n");
				oleDbHResult = ((UnsafeNativeMethods._ADORecord)value).Close();
				Bid.Trace("<oledb._ADORecord.Close|API|ADODB|RET> %08X{HRESULT}\n", oleDbHResult);
			}
			if (OleDbHResult.S_OK < oleDbHResult && (OleDbHResult)(-2146824584) != oleDbHResult)
			{
				UnsafeNativeMethods.IErrorInfo errorInfo = null;
				UnsafeNativeMethods.GetErrorInfo(0, out errorInfo);
				string empty = string.Empty;
				if (errorInfo != null)
				{
					ODB.GetErrorDescription(errorInfo, oleDbHResult, out empty);
				}
				throw new COMException(empty, (int)oleDbHResult);
			}
		}

		// Token: 0x06001F22 RID: 7970 RVA: 0x0025AD7C File Offset: 0x0025A17C
		protected override void OnRowUpdated(RowUpdatedEventArgs value)
		{
			OleDbRowUpdatedEventHandler oleDbRowUpdatedEventHandler = (OleDbRowUpdatedEventHandler)base.Events[OleDbDataAdapter.EventRowUpdated];
			if (oleDbRowUpdatedEventHandler != null && value is OleDbRowUpdatedEventArgs)
			{
				oleDbRowUpdatedEventHandler(this, (OleDbRowUpdatedEventArgs)value);
			}
			base.OnRowUpdated(value);
		}

		// Token: 0x06001F23 RID: 7971 RVA: 0x0025ADC0 File Offset: 0x0025A1C0
		protected override void OnRowUpdating(RowUpdatingEventArgs value)
		{
			OleDbRowUpdatingEventHandler oleDbRowUpdatingEventHandler = (OleDbRowUpdatingEventHandler)base.Events[OleDbDataAdapter.EventRowUpdating];
			if (oleDbRowUpdatingEventHandler != null && value is OleDbRowUpdatingEventArgs)
			{
				oleDbRowUpdatingEventHandler(this, (OleDbRowUpdatingEventArgs)value);
			}
			base.OnRowUpdating(value);
		}

		// Token: 0x06001F24 RID: 7972 RVA: 0x0025AE04 File Offset: 0x0025A204
		private static string GetSourceTableName(string srcTable, int index)
		{
			if (index == 0)
			{
				return srcTable;
			}
			return srcTable + index.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x040012A6 RID: 4774
		private static readonly object EventRowUpdated = new object();

		// Token: 0x040012A7 RID: 4775
		private static readonly object EventRowUpdating = new object();

		// Token: 0x040012A8 RID: 4776
		private OleDbCommand _deleteCommand;

		// Token: 0x040012A9 RID: 4777
		private OleDbCommand _insertCommand;

		// Token: 0x040012AA RID: 4778
		private OleDbCommand _selectCommand;

		// Token: 0x040012AB RID: 4779
		private OleDbCommand _updateCommand;
	}
}
