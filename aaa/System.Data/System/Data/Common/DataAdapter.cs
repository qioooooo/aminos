using System;
using System.ComponentModel;
using System.Data.ProviderBase;
using System.Globalization;
using System.Reflection;
using System.Security.Permissions;
using System.Threading;

namespace System.Data.Common
{
	// Token: 0x02000117 RID: 279
	public class DataAdapter : Component, IDataAdapter
	{
		// Token: 0x060011B8 RID: 4536 RVA: 0x0021CE60 File Offset: 0x0021C260
		protected DataAdapter()
		{
			GC.SuppressFinalize(this);
		}

		// Token: 0x060011B9 RID: 4537 RVA: 0x0021CEAC File Offset: 0x0021C2AC
		protected DataAdapter(DataAdapter from)
		{
			this.CloneFrom(from);
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x060011BA RID: 4538 RVA: 0x0021CEFC File Offset: 0x0021C2FC
		// (set) Token: 0x060011BB RID: 4539 RVA: 0x0021CF10 File Offset: 0x0021C310
		[DefaultValue(true)]
		[ResDescription("DataAdapter_AcceptChangesDuringFill")]
		[ResCategory("DataCategory_Fill")]
		public bool AcceptChangesDuringFill
		{
			get
			{
				return this._acceptChangesDuringFill;
			}
			set
			{
				this._acceptChangesDuringFill = value;
			}
		}

		// Token: 0x060011BC RID: 4540 RVA: 0x0021CF24 File Offset: 0x0021C324
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual bool ShouldSerializeAcceptChangesDuringFill()
		{
			return (LoadOption)0 == this._fillLoadOption;
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x060011BD RID: 4541 RVA: 0x0021CF3C File Offset: 0x0021C33C
		// (set) Token: 0x060011BE RID: 4542 RVA: 0x0021CF50 File Offset: 0x0021C350
		[DefaultValue(true)]
		[ResCategory("DataCategory_Update")]
		[ResDescription("DataAdapter_AcceptChangesDuringUpdate")]
		public bool AcceptChangesDuringUpdate
		{
			get
			{
				return this._acceptChangesDuringUpdate;
			}
			set
			{
				this._acceptChangesDuringUpdate = value;
			}
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x060011BF RID: 4543 RVA: 0x0021CF64 File Offset: 0x0021C364
		// (set) Token: 0x060011C0 RID: 4544 RVA: 0x0021CF78 File Offset: 0x0021C378
		[ResDescription("DataAdapter_ContinueUpdateOnError")]
		[DefaultValue(false)]
		[ResCategory("DataCategory_Update")]
		public bool ContinueUpdateOnError
		{
			get
			{
				return this._continueUpdateOnError;
			}
			set
			{
				this._continueUpdateOnError = value;
			}
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x060011C1 RID: 4545 RVA: 0x0021CF8C File Offset: 0x0021C38C
		// (set) Token: 0x060011C2 RID: 4546 RVA: 0x0021CFAC File Offset: 0x0021C3AC
		[RefreshProperties(RefreshProperties.All)]
		[ResDescription("DataAdapter_FillLoadOption")]
		[ResCategory("DataCategory_Fill")]
		public LoadOption FillLoadOption
		{
			get
			{
				if (this._fillLoadOption == (LoadOption)0)
				{
					return LoadOption.OverwriteChanges;
				}
				return this._fillLoadOption;
			}
			set
			{
				switch (value)
				{
				case (LoadOption)0:
				case LoadOption.OverwriteChanges:
				case LoadOption.PreserveChanges:
				case LoadOption.Upsert:
					this._fillLoadOption = value;
					return;
				default:
					throw ADP.InvalidLoadOption(value);
				}
			}
		}

		// Token: 0x060011C3 RID: 4547 RVA: 0x0021CFE4 File Offset: 0x0021C3E4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetFillLoadOption()
		{
			this._fillLoadOption = (LoadOption)0;
		}

		// Token: 0x060011C4 RID: 4548 RVA: 0x0021CFF8 File Offset: 0x0021C3F8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual bool ShouldSerializeFillLoadOption()
		{
			return (LoadOption)0 != this._fillLoadOption;
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x060011C5 RID: 4549 RVA: 0x0021D014 File Offset: 0x0021C414
		// (set) Token: 0x060011C6 RID: 4550 RVA: 0x0021D028 File Offset: 0x0021C428
		[ResCategory("DataCategory_Mapping")]
		[DefaultValue(MissingMappingAction.Passthrough)]
		[ResDescription("DataAdapter_MissingMappingAction")]
		public MissingMappingAction MissingMappingAction
		{
			get
			{
				return this._missingMappingAction;
			}
			set
			{
				switch (value)
				{
				case MissingMappingAction.Passthrough:
				case MissingMappingAction.Ignore:
				case MissingMappingAction.Error:
					this._missingMappingAction = value;
					return;
				default:
					throw ADP.InvalidMissingMappingAction(value);
				}
			}
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x060011C7 RID: 4551 RVA: 0x0021D05C File Offset: 0x0021C45C
		// (set) Token: 0x060011C8 RID: 4552 RVA: 0x0021D070 File Offset: 0x0021C470
		[DefaultValue(MissingSchemaAction.Add)]
		[ResDescription("DataAdapter_MissingSchemaAction")]
		[ResCategory("DataCategory_Mapping")]
		public MissingSchemaAction MissingSchemaAction
		{
			get
			{
				return this._missingSchemaAction;
			}
			set
			{
				switch (value)
				{
				case MissingSchemaAction.Add:
				case MissingSchemaAction.Ignore:
				case MissingSchemaAction.Error:
				case MissingSchemaAction.AddWithKey:
					this._missingSchemaAction = value;
					return;
				default:
					throw ADP.InvalidMissingSchemaAction(value);
				}
			}
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x060011C9 RID: 4553 RVA: 0x0021D0A8 File Offset: 0x0021C4A8
		internal int ObjectID
		{
			get
			{
				return this._objectID;
			}
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x060011CA RID: 4554 RVA: 0x0021D0BC File Offset: 0x0021C4BC
		// (set) Token: 0x060011CB RID: 4555 RVA: 0x0021D0D0 File Offset: 0x0021C4D0
		[ResDescription("DataAdapter_ReturnProviderSpecificTypes")]
		[ResCategory("DataCategory_Fill")]
		[DefaultValue(false)]
		public virtual bool ReturnProviderSpecificTypes
		{
			get
			{
				return this._returnProviderSpecificTypes;
			}
			set
			{
				this._returnProviderSpecificTypes = value;
			}
		}

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x060011CC RID: 4556 RVA: 0x0021D0E4 File Offset: 0x0021C4E4
		[ResDescription("DataAdapter_TableMappings")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[ResCategory("DataCategory_Mapping")]
		public DataTableMappingCollection TableMappings
		{
			get
			{
				DataTableMappingCollection dataTableMappingCollection = this._tableMappings;
				if (dataTableMappingCollection == null)
				{
					dataTableMappingCollection = this.CreateTableMappings();
					if (dataTableMappingCollection == null)
					{
						dataTableMappingCollection = new DataTableMappingCollection();
					}
					this._tableMappings = dataTableMappingCollection;
				}
				return dataTableMappingCollection;
			}
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x060011CD RID: 4557 RVA: 0x0021D114 File Offset: 0x0021C514
		ITableMappingCollection IDataAdapter.TableMappings
		{
			get
			{
				return this.TableMappings;
			}
		}

		// Token: 0x060011CE RID: 4558 RVA: 0x0021D128 File Offset: 0x0021C528
		protected virtual bool ShouldSerializeTableMappings()
		{
			return true;
		}

		// Token: 0x060011CF RID: 4559 RVA: 0x0021D138 File Offset: 0x0021C538
		protected bool HasTableMappings()
		{
			return this._tableMappings != null && 0 < this.TableMappings.Count;
		}

		// Token: 0x14000021 RID: 33
		// (add) Token: 0x060011D0 RID: 4560 RVA: 0x0021D160 File Offset: 0x0021C560
		// (remove) Token: 0x060011D1 RID: 4561 RVA: 0x0021D188 File Offset: 0x0021C588
		[ResCategory("DataCategory_Fill")]
		[ResDescription("DataAdapter_FillError")]
		public event FillErrorEventHandler FillError
		{
			add
			{
				this._hasFillErrorHandler = true;
				base.Events.AddHandler(DataAdapter.EventFillError, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataAdapter.EventFillError, value);
			}
		}

		// Token: 0x060011D2 RID: 4562 RVA: 0x0021D1A8 File Offset: 0x0021C5A8
		[Obsolete("CloneInternals() has been deprecated.  Use the DataAdapter(DataAdapter from) constructor.  http://go.microsoft.com/fwlink/?linkid=14202")]
		[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		protected virtual DataAdapter CloneInternals()
		{
			DataAdapter dataAdapter = (DataAdapter)Activator.CreateInstance(base.GetType(), BindingFlags.Instance | BindingFlags.Public, null, null, CultureInfo.InvariantCulture, null);
			dataAdapter.CloneFrom(this);
			return dataAdapter;
		}

		// Token: 0x060011D3 RID: 4563 RVA: 0x0021D1D8 File Offset: 0x0021C5D8
		private void CloneFrom(DataAdapter from)
		{
			this._acceptChangesDuringUpdate = from._acceptChangesDuringUpdate;
			this._acceptChangesDuringUpdateAfterInsert = from._acceptChangesDuringUpdateAfterInsert;
			this._continueUpdateOnError = from._continueUpdateOnError;
			this._returnProviderSpecificTypes = from._returnProviderSpecificTypes;
			this._acceptChangesDuringFill = from._acceptChangesDuringFill;
			this._fillLoadOption = from._fillLoadOption;
			this._missingMappingAction = from._missingMappingAction;
			this._missingSchemaAction = from._missingSchemaAction;
			if (from._tableMappings != null && 0 < from.TableMappings.Count)
			{
				DataTableMappingCollection tableMappings = this.TableMappings;
				foreach (object obj in from.TableMappings)
				{
					tableMappings.Add((obj is ICloneable) ? ((ICloneable)obj).Clone() : obj);
				}
			}
		}

		// Token: 0x060011D4 RID: 4564 RVA: 0x0021D2CC File Offset: 0x0021C6CC
		protected virtual DataTableMappingCollection CreateTableMappings()
		{
			Bid.Trace("<comm.DataAdapter.CreateTableMappings|API> %d#\n", this.ObjectID);
			return new DataTableMappingCollection();
		}

		// Token: 0x060011D5 RID: 4565 RVA: 0x0021D2F0 File Offset: 0x0021C6F0
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this._tableMappings = null;
			}
			base.Dispose(disposing);
		}

		// Token: 0x060011D6 RID: 4566 RVA: 0x0021D310 File Offset: 0x0021C710
		public virtual DataTable[] FillSchema(DataSet dataSet, SchemaType schemaType)
		{
			throw ADP.NotSupported();
		}

		// Token: 0x060011D7 RID: 4567 RVA: 0x0021D324 File Offset: 0x0021C724
		protected virtual DataTable[] FillSchema(DataSet dataSet, SchemaType schemaType, string srcTable, IDataReader dataReader)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<comm.DataAdapter.FillSchema|API> %d#, dataSet, schemaType=%d{ds.SchemaType}, srcTable, dataReader\n", this.ObjectID, (int)schemaType);
			DataTable[] array;
			try
			{
				if (dataSet == null)
				{
					throw ADP.ArgumentNull("dataSet");
				}
				if (SchemaType.Source != schemaType && SchemaType.Mapped != schemaType)
				{
					throw ADP.InvalidSchemaType(schemaType);
				}
				if (ADP.IsEmpty(srcTable))
				{
					throw ADP.FillSchemaRequiresSourceTableName("srcTable");
				}
				if (dataReader == null || dataReader.IsClosed)
				{
					throw ADP.FillRequires("dataReader");
				}
				object obj = this.FillSchemaFromReader(dataSet, null, schemaType, srcTable, dataReader);
				array = (DataTable[])obj;
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return array;
		}

		// Token: 0x060011D8 RID: 4568 RVA: 0x0021D3C8 File Offset: 0x0021C7C8
		protected virtual DataTable FillSchema(DataTable dataTable, SchemaType schemaType, IDataReader dataReader)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<comm.DataAdapter.FillSchema|API> %d#, dataTable, schemaType, dataReader\n", this.ObjectID);
			DataTable dataTable2;
			try
			{
				if (dataTable == null)
				{
					throw ADP.ArgumentNull("dataTable");
				}
				if (SchemaType.Source != schemaType && SchemaType.Mapped != schemaType)
				{
					throw ADP.InvalidSchemaType(schemaType);
				}
				if (dataReader == null || dataReader.IsClosed)
				{
					throw ADP.FillRequires("dataReader");
				}
				object obj = this.FillSchemaFromReader(null, dataTable, schemaType, null, dataReader);
				dataTable2 = (DataTable)obj;
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return dataTable2;
		}

		// Token: 0x060011D9 RID: 4569 RVA: 0x0021D454 File Offset: 0x0021C854
		internal object FillSchemaFromReader(DataSet dataset, DataTable datatable, SchemaType schemaType, string srcTable, IDataReader dataReader)
		{
			DataTable[] array = null;
			int num = 0;
			SchemaMapping schemaMapping;
			for (;;)
			{
				DataReaderContainer dataReaderContainer = DataReaderContainer.Create(dataReader, this.ReturnProviderSpecificTypes);
				if (0 < dataReaderContainer.FieldCount)
				{
					string text = null;
					if (dataset != null)
					{
						text = DataAdapter.GetSourceTableName(srcTable, num);
						num++;
					}
					schemaMapping = new SchemaMapping(this, dataset, datatable, dataReaderContainer, true, schemaType, text, false, null, null);
					if (datatable != null)
					{
						break;
					}
					if (schemaMapping.DataTable != null)
					{
						if (array == null)
						{
							array = new DataTable[] { schemaMapping.DataTable };
						}
						else
						{
							array = DataAdapter.AddDataTableToArray(array, schemaMapping.DataTable);
						}
					}
				}
				if (!dataReader.NextResult())
				{
					goto Block_6;
				}
			}
			return schemaMapping.DataTable;
			Block_6:
			object obj = array;
			if (obj == null && datatable == null)
			{
				obj = new DataTable[0];
			}
			return obj;
		}

		// Token: 0x060011DA RID: 4570 RVA: 0x0021D4FC File Offset: 0x0021C8FC
		public virtual int Fill(DataSet dataSet)
		{
			throw ADP.NotSupported();
		}

		// Token: 0x060011DB RID: 4571 RVA: 0x0021D510 File Offset: 0x0021C910
		protected virtual int Fill(DataSet dataSet, string srcTable, IDataReader dataReader, int startRecord, int maxRecords)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<comm.DataAdapter.Fill|API> %d#, dataSet, srcTable, dataReader, startRecord, maxRecords\n", this.ObjectID);
			int num;
			try
			{
				if (dataSet == null)
				{
					throw ADP.FillRequires("dataSet");
				}
				if (ADP.IsEmpty(srcTable))
				{
					throw ADP.FillRequiresSourceTableName("srcTable");
				}
				if (dataReader == null)
				{
					throw ADP.FillRequires("dataReader");
				}
				if (startRecord < 0)
				{
					throw ADP.InvalidStartRecord("startRecord", startRecord);
				}
				if (maxRecords < 0)
				{
					throw ADP.InvalidMaxRecords("maxRecords", maxRecords);
				}
				if (dataReader.IsClosed)
				{
					num = 0;
				}
				else
				{
					DataReaderContainer dataReaderContainer = DataReaderContainer.Create(dataReader, this.ReturnProviderSpecificTypes);
					num = this.FillFromReader(dataSet, null, srcTable, dataReaderContainer, startRecord, maxRecords, null, null);
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return num;
		}

		// Token: 0x060011DC RID: 4572 RVA: 0x0021D5D4 File Offset: 0x0021C9D4
		protected virtual int Fill(DataTable dataTable, IDataReader dataReader)
		{
			DataTable[] array = new DataTable[] { dataTable };
			return this.Fill(array, dataReader, 0, 0);
		}

		// Token: 0x060011DD RID: 4573 RVA: 0x0021D5F8 File Offset: 0x0021C9F8
		protected virtual int Fill(DataTable[] dataTables, IDataReader dataReader, int startRecord, int maxRecords)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<comm.DataAdapter.Fill|API> %d#, dataTables[], dataReader, startRecord, maxRecords\n", this.ObjectID);
			int num4;
			try
			{
				ADP.CheckArgumentLength(dataTables, "tables");
				if (dataTables == null || dataTables.Length == 0 || dataTables[0] == null)
				{
					throw ADP.FillRequires("dataTable");
				}
				if (dataReader == null)
				{
					throw ADP.FillRequires("dataReader");
				}
				if (1 < dataTables.Length && (startRecord != 0 || maxRecords != 0))
				{
					throw ADP.NotSupported();
				}
				int num = 0;
				bool flag = false;
				DataSet dataSet = dataTables[0].DataSet;
				try
				{
					if (dataSet != null)
					{
						flag = dataSet.EnforceConstraints;
						dataSet.EnforceConstraints = false;
					}
					int num2 = 0;
					while (num2 < dataTables.Length && !dataReader.IsClosed)
					{
						DataReaderContainer dataReaderContainer = DataReaderContainer.Create(dataReader, this.ReturnProviderSpecificTypes);
						if (dataReaderContainer.FieldCount > 0)
						{
							if (0 < num2 && !this.FillNextResult(dataReaderContainer))
							{
								break;
							}
							int num3 = this.FillFromReader(null, dataTables[num2], null, dataReaderContainer, startRecord, maxRecords, null, null);
							if (num2 == 0)
							{
								num = num3;
							}
						}
						num2++;
					}
				}
				catch (ConstraintException)
				{
					flag = false;
					throw;
				}
				finally
				{
					if (flag)
					{
						dataSet.EnforceConstraints = true;
					}
				}
				num4 = num;
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return num4;
		}

		// Token: 0x060011DE RID: 4574 RVA: 0x0021D740 File Offset: 0x0021CB40
		internal int FillFromReader(DataSet dataset, DataTable datatable, string srcTable, DataReaderContainer dataReader, int startRecord, int maxRecords, DataColumn parentChapterColumn, object parentChapterValue)
		{
			int num = 0;
			int num2 = 0;
			do
			{
				if (0 < dataReader.FieldCount)
				{
					SchemaMapping schemaMapping = this.FillMapping(dataset, datatable, srcTable, dataReader, num2, parentChapterColumn, parentChapterValue);
					num2++;
					if (schemaMapping != null && schemaMapping.DataValues != null && schemaMapping.DataTable != null)
					{
						schemaMapping.DataTable.BeginLoadData();
						try
						{
							if (1 == num2 && (0 < startRecord || 0 < maxRecords))
							{
								num = this.FillLoadDataRowChunk(schemaMapping, startRecord, maxRecords);
							}
							else
							{
								int num3 = this.FillLoadDataRow(schemaMapping);
								if (1 == num2)
								{
									num = num3;
								}
							}
						}
						finally
						{
							schemaMapping.DataTable.EndLoadData();
						}
						if (datatable != null)
						{
							break;
						}
					}
				}
			}
			while (this.FillNextResult(dataReader));
			return num;
		}

		// Token: 0x060011DF RID: 4575 RVA: 0x0021D7F4 File Offset: 0x0021CBF4
		private int FillLoadDataRowChunk(SchemaMapping mapping, int startRecord, int maxRecords)
		{
			DataReaderContainer dataReader = mapping.DataReader;
			while (0 < startRecord)
			{
				if (!dataReader.Read())
				{
					return 0;
				}
				startRecord--;
			}
			int i = 0;
			if (0 < maxRecords)
			{
				while (i < maxRecords)
				{
					if (!dataReader.Read())
					{
						break;
					}
					if (this._hasFillErrorHandler)
					{
						try
						{
							mapping.LoadDataRowWithClear();
							i++;
							continue;
						}
						catch (Exception ex)
						{
							if (!ADP.IsCatchableExceptionType(ex))
							{
								throw;
							}
							ADP.TraceExceptionForCapture(ex);
							this.OnFillErrorHandler(ex, mapping.DataTable, mapping.DataValues);
							continue;
						}
					}
					mapping.LoadDataRow();
					i++;
				}
			}
			else
			{
				i = this.FillLoadDataRow(mapping);
			}
			return i;
		}

		// Token: 0x060011E0 RID: 4576 RVA: 0x0021D89C File Offset: 0x0021CC9C
		private int FillLoadDataRow(SchemaMapping mapping)
		{
			int num = 0;
			DataReaderContainer dataReader = mapping.DataReader;
			if (this._hasFillErrorHandler)
			{
				while (dataReader.Read())
				{
					try
					{
						mapping.LoadDataRowWithClear();
						num++;
					}
					catch (Exception ex)
					{
						if (!ADP.IsCatchableExceptionType(ex))
						{
							throw;
						}
						ADP.TraceExceptionForCapture(ex);
						this.OnFillErrorHandler(ex, mapping.DataTable, mapping.DataValues);
					}
				}
			}
			else
			{
				while (dataReader.Read())
				{
					mapping.LoadDataRow();
					num++;
				}
			}
			return num;
		}

		// Token: 0x060011E1 RID: 4577 RVA: 0x0021D928 File Offset: 0x0021CD28
		private SchemaMapping FillMappingInternal(DataSet dataset, DataTable datatable, string srcTable, DataReaderContainer dataReader, int schemaCount, DataColumn parentChapterColumn, object parentChapterValue)
		{
			bool flag = MissingSchemaAction.AddWithKey == this.MissingSchemaAction;
			string text = null;
			if (dataset != null)
			{
				text = DataAdapter.GetSourceTableName(srcTable, schemaCount);
			}
			return new SchemaMapping(this, dataset, datatable, dataReader, flag, SchemaType.Mapped, text, true, parentChapterColumn, parentChapterValue);
		}

		// Token: 0x060011E2 RID: 4578 RVA: 0x0021D960 File Offset: 0x0021CD60
		private SchemaMapping FillMapping(DataSet dataset, DataTable datatable, string srcTable, DataReaderContainer dataReader, int schemaCount, DataColumn parentChapterColumn, object parentChapterValue)
		{
			SchemaMapping schemaMapping = null;
			if (this._hasFillErrorHandler)
			{
				try
				{
					return this.FillMappingInternal(dataset, datatable, srcTable, dataReader, schemaCount, parentChapterColumn, parentChapterValue);
				}
				catch (Exception ex)
				{
					if (!ADP.IsCatchableExceptionType(ex))
					{
						throw;
					}
					ADP.TraceExceptionForCapture(ex);
					this.OnFillErrorHandler(ex, null, null);
					return schemaMapping;
				}
			}
			schemaMapping = this.FillMappingInternal(dataset, datatable, srcTable, dataReader, schemaCount, parentChapterColumn, parentChapterValue);
			return schemaMapping;
		}

		// Token: 0x060011E3 RID: 4579 RVA: 0x0021D9D8 File Offset: 0x0021CDD8
		private bool FillNextResult(DataReaderContainer dataReader)
		{
			bool flag = true;
			if (this._hasFillErrorHandler)
			{
				try
				{
					return dataReader.NextResult();
				}
				catch (Exception ex)
				{
					if (!ADP.IsCatchableExceptionType(ex))
					{
						throw;
					}
					ADP.TraceExceptionForCapture(ex);
					this.OnFillErrorHandler(ex, null, null);
					return flag;
				}
			}
			flag = dataReader.NextResult();
			return flag;
		}

		// Token: 0x060011E4 RID: 4580 RVA: 0x0021DA38 File Offset: 0x0021CE38
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public virtual IDataParameter[] GetFillParameters()
		{
			return new IDataParameter[0];
		}

		// Token: 0x060011E5 RID: 4581 RVA: 0x0021DA4C File Offset: 0x0021CE4C
		internal DataTableMapping GetTableMappingBySchemaAction(string sourceTableName, string dataSetTableName, MissingMappingAction mappingAction)
		{
			return DataTableMappingCollection.GetTableMappingBySchemaAction(this._tableMappings, sourceTableName, dataSetTableName, mappingAction);
		}

		// Token: 0x060011E6 RID: 4582 RVA: 0x0021DA68 File Offset: 0x0021CE68
		internal int IndexOfDataSetTable(string dataSetTable)
		{
			if (this._tableMappings != null)
			{
				return this.TableMappings.IndexOfDataSetTable(dataSetTable);
			}
			return -1;
		}

		// Token: 0x060011E7 RID: 4583 RVA: 0x0021DA8C File Offset: 0x0021CE8C
		protected virtual void OnFillError(FillErrorEventArgs value)
		{
			FillErrorEventHandler fillErrorEventHandler = (FillErrorEventHandler)base.Events[DataAdapter.EventFillError];
			if (fillErrorEventHandler != null)
			{
				fillErrorEventHandler(this, value);
			}
		}

		// Token: 0x060011E8 RID: 4584 RVA: 0x0021DABC File Offset: 0x0021CEBC
		private void OnFillErrorHandler(Exception e, DataTable dataTable, object[] dataValues)
		{
			FillErrorEventArgs fillErrorEventArgs = new FillErrorEventArgs(dataTable, dataValues);
			fillErrorEventArgs.Errors = e;
			this.OnFillError(fillErrorEventArgs);
			if (fillErrorEventArgs.Continue)
			{
				return;
			}
			if (fillErrorEventArgs.Errors != null)
			{
				throw fillErrorEventArgs.Errors;
			}
			throw e;
		}

		// Token: 0x060011E9 RID: 4585 RVA: 0x0021DAF8 File Offset: 0x0021CEF8
		public virtual int Update(DataSet dataSet)
		{
			throw ADP.NotSupported();
		}

		// Token: 0x060011EA RID: 4586 RVA: 0x0021DB0C File Offset: 0x0021CF0C
		private static DataTable[] AddDataTableToArray(DataTable[] tables, DataTable newTable)
		{
			for (int i = 0; i < tables.Length; i++)
			{
				if (tables[i] == newTable)
				{
					return tables;
				}
			}
			DataTable[] array = new DataTable[tables.Length + 1];
			for (int j = 0; j < tables.Length; j++)
			{
				array[j] = tables[j];
			}
			array[tables.Length] = newTable;
			return array;
		}

		// Token: 0x060011EB RID: 4587 RVA: 0x0021DB58 File Offset: 0x0021CF58
		private static string GetSourceTableName(string srcTable, int index)
		{
			if (index == 0)
			{
				return srcTable;
			}
			return srcTable + index.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x04000B6C RID: 2924
		private static readonly object EventFillError = new object();

		// Token: 0x04000B6D RID: 2925
		private bool _acceptChangesDuringUpdate = true;

		// Token: 0x04000B6E RID: 2926
		private bool _acceptChangesDuringUpdateAfterInsert = true;

		// Token: 0x04000B6F RID: 2927
		private bool _continueUpdateOnError;

		// Token: 0x04000B70 RID: 2928
		private bool _hasFillErrorHandler;

		// Token: 0x04000B71 RID: 2929
		private bool _returnProviderSpecificTypes;

		// Token: 0x04000B72 RID: 2930
		private bool _acceptChangesDuringFill = true;

		// Token: 0x04000B73 RID: 2931
		private LoadOption _fillLoadOption;

		// Token: 0x04000B74 RID: 2932
		private MissingMappingAction _missingMappingAction = MissingMappingAction.Passthrough;

		// Token: 0x04000B75 RID: 2933
		private MissingSchemaAction _missingSchemaAction = MissingSchemaAction.Add;

		// Token: 0x04000B76 RID: 2934
		private DataTableMappingCollection _tableMappings;

		// Token: 0x04000B77 RID: 2935
		private static int _objectTypeCount;

		// Token: 0x04000B78 RID: 2936
		internal readonly int _objectID = Interlocked.Increment(ref DataAdapter._objectTypeCount);
	}
}
