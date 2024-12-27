using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.ProviderBase;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;
using System.Transactions;

namespace System.Data.OleDb
{
	// Token: 0x02000216 RID: 534
	internal sealed class OleDbConnectionInternal : DbConnectionInternal, IDisposable
	{
		// Token: 0x06001E8C RID: 7820 RVA: 0x002578A4 File Offset: 0x00256CA4
		internal OleDbConnectionInternal(OleDbConnectionString constr, OleDbConnection connection)
		{
			this.ConnectionString = constr;
			if (constr.PossiblePrompt && !Environment.UserInteractive)
			{
				throw ODB.PossiblePromptNotUserInteractive();
			}
			OleDbServicesWrapper objectPool = OleDbConnectionInternal.GetObjectPool();
			this._datasrcwrp = new DataSourceWrapper();
			objectPool.GetDataSource(constr, ref this._datasrcwrp);
			if (connection == null)
			{
				return;
			}
			this._sessionwrp = new SessionWrapper();
			OleDbHResult oleDbHResult = this._datasrcwrp.InitializeAndCreateSession(constr, ref this._sessionwrp);
			if (OleDbHResult.S_OK <= oleDbHResult && !this._sessionwrp.IsInvalid)
			{
				OleDbConnection.ProcessResults(oleDbHResult, connection, connection);
				return;
			}
			Exception ex = OleDbConnection.ProcessResults(oleDbHResult, null, null);
			throw ex;
		}

		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x06001E8D RID: 7821 RVA: 0x00257938 File Offset: 0x00256D38
		internal OleDbConnection Connection
		{
			get
			{
				return (OleDbConnection)base.Owner;
			}
		}

		// Token: 0x17000422 RID: 1058
		// (get) Token: 0x06001E8E RID: 7822 RVA: 0x00257950 File Offset: 0x00256D50
		internal bool HasSession
		{
			get
			{
				return null != this._sessionwrp;
			}
		}

		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x06001E8F RID: 7823 RVA: 0x0025796C File Offset: 0x00256D6C
		// (set) Token: 0x06001E90 RID: 7824 RVA: 0x00257998 File Offset: 0x00256D98
		internal OleDbTransaction LocalTransaction
		{
			get
			{
				OleDbTransaction oleDbTransaction = null;
				if (this.weakTransaction != null)
				{
					oleDbTransaction = (OleDbTransaction)this.weakTransaction.Target;
				}
				return oleDbTransaction;
			}
			set
			{
				this.weakTransaction = null;
				if (value != null)
				{
					this.weakTransaction = new WeakReference(value);
				}
			}
		}

		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x06001E91 RID: 7825 RVA: 0x002579BC File Offset: 0x00256DBC
		private string Provider
		{
			get
			{
				return this.ConnectionString.Provider;
			}
		}

		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x06001E92 RID: 7826 RVA: 0x002579D4 File Offset: 0x00256DD4
		public override string ServerVersion
		{
			get
			{
				object dataSourceValue = this.GetDataSourceValue(OleDbPropertySetGuid.DataSourceInfo, 41);
				return Convert.ToString(dataSourceValue, CultureInfo.InvariantCulture);
			}
		}

		// Token: 0x06001E93 RID: 7827 RVA: 0x002579FC File Offset: 0x00256DFC
		internal IDBPropertiesWrapper IDBProperties()
		{
			return this._datasrcwrp.IDBProperties(this);
		}

		// Token: 0x06001E94 RID: 7828 RVA: 0x00257A18 File Offset: 0x00256E18
		internal IOpenRowsetWrapper IOpenRowset()
		{
			return this._sessionwrp.IOpenRowset(this);
		}

		// Token: 0x06001E95 RID: 7829 RVA: 0x00257A34 File Offset: 0x00256E34
		private IDBInfoWrapper IDBInfo()
		{
			return this._datasrcwrp.IDBInfo(this);
		}

		// Token: 0x06001E96 RID: 7830 RVA: 0x00257A50 File Offset: 0x00256E50
		internal IDBSchemaRowsetWrapper IDBSchemaRowset()
		{
			return this._sessionwrp.IDBSchemaRowset(this);
		}

		// Token: 0x06001E97 RID: 7831 RVA: 0x00257A6C File Offset: 0x00256E6C
		internal ITransactionJoinWrapper ITransactionJoin()
		{
			return this._sessionwrp.ITransactionJoin(this);
		}

		// Token: 0x06001E98 RID: 7832 RVA: 0x00257A88 File Offset: 0x00256E88
		internal UnsafeNativeMethods.ICommandText ICommandText()
		{
			object obj = null;
			OleDbHResult oleDbHResult = this._sessionwrp.CreateCommand(ref obj);
			if (oleDbHResult < OleDbHResult.S_OK)
			{
				if (OleDbHResult.E_NOINTERFACE != oleDbHResult)
				{
					this.ProcessResults(oleDbHResult);
				}
				else
				{
					SafeNativeMethods.Wrapper.ClearErrorInfo();
				}
			}
			return (UnsafeNativeMethods.ICommandText)obj;
		}

		// Token: 0x06001E99 RID: 7833 RVA: 0x00257AC8 File Offset: 0x00256EC8
		protected override void Activate(Transaction transaction)
		{
			throw ADP.NotSupported();
		}

		// Token: 0x06001E9A RID: 7834 RVA: 0x00257ADC File Offset: 0x00256EDC
		public override DbTransaction BeginTransaction(IsolationLevel isolationLevel)
		{
			OleDbConnection.ExecutePermission.Demand();
			OleDbConnection connection = this.Connection;
			if (this.LocalTransaction != null)
			{
				throw ADP.ParallelTransactionsNotSupported(connection);
			}
			object obj = null;
			OleDbTransaction oleDbTransaction;
			try
			{
				oleDbTransaction = new OleDbTransaction(connection, null, isolationLevel);
				Bid.Trace("<oledb.IUnknown.QueryInterface|API|OLEDB|session> %d#, ITransactionLocal\n", base.ObjectID);
				obj = this._sessionwrp.ComWrapper();
				UnsafeNativeMethods.ITransactionLocal transactionLocal = obj as UnsafeNativeMethods.ITransactionLocal;
				if (transactionLocal == null)
				{
					throw ODB.TransactionsNotSupported(this.Provider, null);
				}
				oleDbTransaction.BeginInternal(transactionLocal);
			}
			finally
			{
				if (obj != null)
				{
					Marshal.ReleaseComObject(obj);
				}
			}
			this.LocalTransaction = oleDbTransaction;
			return oleDbTransaction;
		}

		// Token: 0x06001E9B RID: 7835 RVA: 0x00257B80 File Offset: 0x00256F80
		protected override DbReferenceCollection CreateReferenceCollection()
		{
			return new OleDbReferenceCollection();
		}

		// Token: 0x06001E9C RID: 7836 RVA: 0x00257B94 File Offset: 0x00256F94
		protected override void Deactivate()
		{
			base.NotifyWeakReference(0);
			if (this._forcedAutomaticEnlistment)
			{
				this.EnlistTransactionInternal(null, false);
			}
			OleDbTransaction localTransaction = this.LocalTransaction;
			if (localTransaction != null)
			{
				this.LocalTransaction = null;
				localTransaction.Dispose();
			}
		}

		// Token: 0x06001E9D RID: 7837 RVA: 0x00257BD0 File Offset: 0x00256FD0
		public override void Dispose()
		{
			if (this._sessionwrp != null)
			{
				this._sessionwrp.Dispose();
			}
			if (this._datasrcwrp != null)
			{
				this._datasrcwrp.Dispose();
			}
			base.Dispose();
		}

		// Token: 0x06001E9E RID: 7838 RVA: 0x00257C0C File Offset: 0x0025700C
		public override void EnlistTransaction(Transaction transaction)
		{
			OleDbConnection connection = this.Connection;
			if (this.LocalTransaction != null)
			{
				throw ADP.LocalTransactionPresent();
			}
			this.EnlistTransactionInternal(transaction, false);
		}

		// Token: 0x06001E9F RID: 7839 RVA: 0x00257C38 File Offset: 0x00257038
		internal void EnlistTransactionInternal(Transaction transaction, bool forcedAutomatic)
		{
			IDtcTransaction oletxTransaction = ADP.GetOletxTransaction(transaction);
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<oledb.ITransactionJoin.JoinTransaction|API|OLEDB> %d#\n", base.ObjectID);
			try
			{
				using (ITransactionJoinWrapper transactionJoinWrapper = this.ITransactionJoin())
				{
					if (transactionJoinWrapper.Value == null)
					{
						throw ODB.TransactionsNotSupported(this.Provider, null);
					}
					transactionJoinWrapper.Value.JoinTransaction(oletxTransaction, -1, 0, IntPtr.Zero);
					this._forcedAutomaticEnlistment = forcedAutomatic;
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			base.EnlistedTransaction = transaction;
		}

		// Token: 0x06001EA0 RID: 7840 RVA: 0x00257CEC File Offset: 0x002570EC
		internal object GetDataSourceValue(Guid propertySet, int propertyID)
		{
			object obj = this.GetDataSourcePropertyValue(propertySet, propertyID);
			if (obj is OleDbPropertyStatus || Convert.IsDBNull(obj))
			{
				obj = null;
			}
			return obj;
		}

		// Token: 0x06001EA1 RID: 7841 RVA: 0x00257D18 File Offset: 0x00257118
		internal object GetDataSourcePropertyValue(Guid propertySet, int propertyID)
		{
			tagDBPROP[] propertySet2;
			using (IDBPropertiesWrapper idbpropertiesWrapper = this.IDBProperties())
			{
				using (PropertyIDSet propertyIDSet = new PropertyIDSet(propertySet, propertyID))
				{
					OleDbHResult oleDbHResult;
					using (DBPropSet dbpropSet = new DBPropSet(idbpropertiesWrapper.Value, propertyIDSet, out oleDbHResult))
					{
						if (oleDbHResult < OleDbHResult.S_OK)
						{
							SafeNativeMethods.Wrapper.ClearErrorInfo();
						}
						propertySet2 = dbpropSet.GetPropertySet(0, out propertySet);
					}
				}
			}
			if (propertySet2[0].dwStatus == OleDbPropertyStatus.Ok)
			{
				return propertySet2[0].vValue;
			}
			return propertySet2[0].dwStatus;
		}

		// Token: 0x06001EA2 RID: 7842 RVA: 0x00257DEC File Offset: 0x002571EC
		internal DataTable BuildInfoLiterals()
		{
			DataTable dataTable;
			using (IDBInfoWrapper idbinfoWrapper = this.IDBInfo())
			{
				UnsafeNativeMethods.IDBInfo value = idbinfoWrapper.Value;
				if (value == null)
				{
					dataTable = null;
				}
				else
				{
					DataTable dataTable2 = new DataTable("DbInfoLiterals");
					dataTable2.Locale = CultureInfo.InvariantCulture;
					DataColumn dataColumn = new DataColumn("LiteralName", typeof(string));
					DataColumn dataColumn2 = new DataColumn("LiteralValue", typeof(string));
					DataColumn dataColumn3 = new DataColumn("InvalidChars", typeof(string));
					DataColumn dataColumn4 = new DataColumn("InvalidStartingChars", typeof(string));
					DataColumn dataColumn5 = new DataColumn("Literal", typeof(int));
					DataColumn dataColumn6 = new DataColumn("Maxlen", typeof(int));
					dataTable2.Columns.Add(dataColumn);
					dataTable2.Columns.Add(dataColumn2);
					dataTable2.Columns.Add(dataColumn3);
					dataTable2.Columns.Add(dataColumn4);
					dataTable2.Columns.Add(dataColumn5);
					dataTable2.Columns.Add(dataColumn6);
					int num = 0;
					IntPtr ptrZero = ADP.PtrZero;
					OleDbHResult oleDbHResult;
					using (new DualCoTaskMem(value, null, out num, out ptrZero, out oleDbHResult))
					{
						if (OleDbHResult.DB_E_ERRORSOCCURRED != oleDbHResult)
						{
							long num2 = ptrZero.ToInt64();
							tagDBLITERALINFO tagDBLITERALINFO = new tagDBLITERALINFO();
							int i = 0;
							while (i < num)
							{
								Marshal.PtrToStructure((IntPtr)num2, tagDBLITERALINFO);
								DataRow dataRow = dataTable2.NewRow();
								dataRow[dataColumn] = ((OleDbLiteral)tagDBLITERALINFO.it).ToString();
								dataRow[dataColumn2] = tagDBLITERALINFO.pwszLiteralValue;
								dataRow[dataColumn3] = tagDBLITERALINFO.pwszInvalidChars;
								dataRow[dataColumn4] = tagDBLITERALINFO.pwszInvalidStartingChars;
								dataRow[dataColumn5] = tagDBLITERALINFO.it;
								dataRow[dataColumn6] = tagDBLITERALINFO.cchMaxLen;
								dataTable2.Rows.Add(dataRow);
								dataRow.AcceptChanges();
								i++;
								num2 += (long)ODB.SizeOf_tagDBLITERALINFO;
							}
							if (oleDbHResult < OleDbHResult.S_OK)
							{
								this.ProcessResults(oleDbHResult);
							}
						}
						else
						{
							SafeNativeMethods.Wrapper.ClearErrorInfo();
						}
					}
					dataTable = dataTable2;
				}
			}
			return dataTable;
		}

		// Token: 0x06001EA3 RID: 7843 RVA: 0x0025804C File Offset: 0x0025744C
		internal DataTable BuildInfoKeywords()
		{
			DataTable dataTable = new DataTable("DbInfoKeywords");
			dataTable.Locale = CultureInfo.InvariantCulture;
			DataColumn dataColumn = new DataColumn("Keyword", typeof(string));
			dataTable.Columns.Add(dataColumn);
			if (!this.AddInfoKeywordsToTable(dataTable, dataColumn))
			{
				dataTable = null;
			}
			return dataTable;
		}

		// Token: 0x06001EA4 RID: 7844 RVA: 0x002580A0 File Offset: 0x002574A0
		internal bool AddInfoKeywordsToTable(DataTable table, DataColumn keyword)
		{
			bool flag;
			using (IDBInfoWrapper idbinfoWrapper = this.IDBInfo())
			{
				UnsafeNativeMethods.IDBInfo value = idbinfoWrapper.Value;
				if (value == null)
				{
					flag = false;
				}
				else
				{
					Bid.Trace("<oledb.IDBInfo.GetKeywords|API|OLEDB> %d#\n", base.ObjectID);
					string text;
					OleDbHResult keywords = value.GetKeywords(out text);
					Bid.Trace("<oledb.IDBInfo.GetKeywords|API|OLEDB|RET> %08X{HRESULT}\n", keywords);
					if (keywords < OleDbHResult.S_OK)
					{
						this.ProcessResults(keywords);
					}
					if (text != null)
					{
						string[] array = text.Split(new char[] { ',' });
						for (int i = 0; i < array.Length; i++)
						{
							DataRow dataRow = table.NewRow();
							dataRow[keyword] = array[i];
							table.Rows.Add(dataRow);
							dataRow.AcceptChanges();
						}
					}
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x06001EA5 RID: 7845 RVA: 0x00258178 File Offset: 0x00257578
		internal DataTable BuildSchemaGuids()
		{
			DataTable dataTable = new DataTable("SchemaGuids");
			dataTable.Locale = CultureInfo.InvariantCulture;
			DataColumn dataColumn = new DataColumn("Schema", typeof(Guid));
			DataColumn dataColumn2 = new DataColumn("RestrictionSupport", typeof(int));
			dataTable.Columns.Add(dataColumn);
			dataTable.Columns.Add(dataColumn2);
			SchemaSupport[] schemaRowsetInformation = this.GetSchemaRowsetInformation();
			if (schemaRowsetInformation != null)
			{
				object[] array = new object[2];
				dataTable.BeginLoadData();
				for (int i = 0; i < schemaRowsetInformation.Length; i++)
				{
					array[0] = schemaRowsetInformation[i]._schemaRowset;
					array[1] = schemaRowsetInformation[i]._restrictions;
					dataTable.LoadDataRow(array, LoadOption.OverwriteChanges);
				}
				dataTable.EndLoadData();
			}
			return dataTable;
		}

		// Token: 0x06001EA6 RID: 7846 RVA: 0x00258240 File Offset: 0x00257640
		internal string GetLiteralInfo(int literal)
		{
			string text;
			using (IDBInfoWrapper idbinfoWrapper = this.IDBInfo())
			{
				UnsafeNativeMethods.IDBInfo value = idbinfoWrapper.Value;
				if (value == null)
				{
					text = null;
				}
				else
				{
					string text2 = null;
					IntPtr ptrZero = ADP.PtrZero;
					int num = 0;
					OleDbHResult oleDbHResult;
					using (new DualCoTaskMem(value, new int[] { literal }, out num, out ptrZero, out oleDbHResult))
					{
						if (OleDbHResult.DB_E_ERRORSOCCURRED != oleDbHResult)
						{
							if (1 == num && Marshal.ReadInt32(ptrZero, ODB.OffsetOf_tagDBLITERALINFO_it) == literal)
							{
								text2 = Marshal.PtrToStringUni(Marshal.ReadIntPtr(ptrZero, 0));
							}
							if (oleDbHResult < OleDbHResult.S_OK)
							{
								this.ProcessResults(oleDbHResult);
							}
						}
						else
						{
							SafeNativeMethods.Wrapper.ClearErrorInfo();
						}
					}
					text = text2;
				}
			}
			return text;
		}

		// Token: 0x06001EA7 RID: 7847 RVA: 0x00258324 File Offset: 0x00257724
		internal SchemaSupport[] GetSchemaRowsetInformation()
		{
			OleDbConnectionString connectionString = this.ConnectionString;
			SchemaSupport[] array = connectionString.SchemaSupport;
			if (array != null)
			{
				return array;
			}
			SchemaSupport[] array2;
			using (IDBSchemaRowsetWrapper idbschemaRowsetWrapper = this.IDBSchemaRowset())
			{
				UnsafeNativeMethods.IDBSchemaRowset value = idbschemaRowsetWrapper.Value;
				if (value == null)
				{
					array2 = null;
				}
				else
				{
					int num = 0;
					IntPtr ptrZero = ADP.PtrZero;
					IntPtr ptrZero2 = ADP.PtrZero;
					OleDbHResult oleDbHResult;
					using (new DualCoTaskMem(value, out num, out ptrZero, out ptrZero2, out oleDbHResult))
					{
						if (oleDbHResult < OleDbHResult.S_OK)
						{
							this.ProcessResults(oleDbHResult);
						}
						array = new SchemaSupport[num];
						if (ADP.PtrZero != ptrZero)
						{
							int i = 0;
							int num2 = 0;
							while (i < array.Length)
							{
								IntPtr intPtr = ADP.IntPtrOffset(ptrZero, i * ODB.SizeOf_Guid);
								array[i]._schemaRowset = (Guid)Marshal.PtrToStructure(intPtr, typeof(Guid));
								i++;
								num2 += ODB.SizeOf_Guid;
							}
						}
						if (ADP.PtrZero != ptrZero2)
						{
							for (int j = 0; j < array.Length; j++)
							{
								array[j]._restrictions = Marshal.ReadInt32(ptrZero2, j * 4);
							}
						}
					}
					connectionString.SchemaSupport = array;
					array2 = array;
				}
			}
			return array2;
		}

		// Token: 0x06001EA8 RID: 7848 RVA: 0x00258488 File Offset: 0x00257888
		internal DataTable GetSchemaRowset(Guid schema, object[] restrictions)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<oledb.OleDbConnectionInternal.GetSchemaRowset|INFO> %d#, schema=%p{GUID}, restrictions\n", base.ObjectID, schema);
			DataTable dataTable2;
			try
			{
				if (restrictions == null)
				{
					restrictions = new object[0];
				}
				DataTable dataTable = null;
				using (IDBSchemaRowsetWrapper idbschemaRowsetWrapper = this.IDBSchemaRowset())
				{
					UnsafeNativeMethods.IDBSchemaRowset value = idbschemaRowsetWrapper.Value;
					if (value == null)
					{
						throw ODB.SchemaRowsetsNotSupported(this.Provider);
					}
					UnsafeNativeMethods.IRowset rowset = null;
					Bid.Trace("<oledb.IDBSchemaRowset.GetRowset|API|OLEDB> %d#\n", base.ObjectID);
					OleDbHResult rowset2 = value.GetRowset(ADP.PtrZero, ref schema, restrictions.Length, restrictions, ref ODB.IID_IRowset, 0, ADP.PtrZero, out rowset);
					Bid.Trace("<oledb.IDBSchemaRowset.GetRowset|API|OLEDB|RET> %08X{HRESULT}\n", rowset2);
					if (rowset2 < OleDbHResult.S_OK)
					{
						this.ProcessResults(rowset2);
					}
					if (rowset != null)
					{
						using (OleDbDataReader oleDbDataReader = new OleDbDataReader(this.Connection, null, 0, CommandBehavior.Default))
						{
							oleDbDataReader.InitializeIRowset(rowset, ChapterHandle.DB_NULL_HCHAPTER, IntPtr.Zero);
							oleDbDataReader.BuildMetaInfo();
							oleDbDataReader.HasRowsRead();
							dataTable = new DataTable();
							dataTable.Locale = CultureInfo.InvariantCulture;
							dataTable.TableName = OleDbSchemaGuid.GetTextFromValue(schema);
							OleDbDataAdapter.FillDataTable(oleDbDataReader, new DataTable[] { dataTable });
						}
					}
					dataTable2 = dataTable;
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return dataTable2;
		}

		// Token: 0x06001EA9 RID: 7849 RVA: 0x002585F8 File Offset: 0x002579F8
		internal bool HasLiveReader(OleDbCommand cmd)
		{
			bool flag = false;
			DbReferenceCollection referenceCollection = base.ReferenceCollection;
			if (referenceCollection != null)
			{
				foreach (object obj in referenceCollection.Filter(2))
				{
					OleDbDataReader oleDbDataReader = (OleDbDataReader)obj;
					if (oleDbDataReader != null && cmd == oleDbDataReader.Command)
					{
						flag = true;
						break;
					}
				}
			}
			return flag;
		}

		// Token: 0x06001EAA RID: 7850 RVA: 0x0025867C File Offset: 0x00257A7C
		private void ProcessResults(OleDbHResult hr)
		{
			OleDbConnection connection = this.Connection;
			Exception ex = OleDbConnection.ProcessResults(hr, connection, connection);
			if (ex != null)
			{
				throw ex;
			}
		}

		// Token: 0x06001EAB RID: 7851 RVA: 0x002586A0 File Offset: 0x00257AA0
		internal bool SupportSchemaRowset(Guid schema)
		{
			SchemaSupport[] schemaRowsetInformation = this.GetSchemaRowsetInformation();
			if (schemaRowsetInformation != null)
			{
				for (int i = 0; i < schemaRowsetInformation.Length; i++)
				{
					if (schema == schemaRowsetInformation[i]._schemaRowset)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001EAC RID: 7852 RVA: 0x002586DC File Offset: 0x00257ADC
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
		private static object CreateInstanceDataLinks()
		{
			Type typeFromCLSID = Type.GetTypeFromCLSID(ODB.CLSID_DataLinks, true);
			return Activator.CreateInstance(typeFromCLSID, BindingFlags.Instance | BindingFlags.Public, null, null, CultureInfo.InvariantCulture, null);
		}

		// Token: 0x06001EAD RID: 7853 RVA: 0x00258708 File Offset: 0x00257B08
		private static OleDbServicesWrapper GetObjectPool()
		{
			OleDbServicesWrapper oleDbServicesWrapper = OleDbConnectionInternal.idataInitialize;
			if (oleDbServicesWrapper == null)
			{
				lock (OleDbConnectionInternal.dataInitializeLock)
				{
					oleDbServicesWrapper = OleDbConnectionInternal.idataInitialize;
					if (oleDbServicesWrapper == null)
					{
						OleDbConnectionInternal.VersionCheck();
						object obj2;
						try
						{
							obj2 = OleDbConnectionInternal.CreateInstanceDataLinks();
						}
						catch (Exception ex)
						{
							if (!ADP.IsCatchableExceptionType(ex))
							{
								throw;
							}
							throw ODB.MDACNotAvailable(ex);
						}
						if (obj2 == null)
						{
							throw ODB.MDACNotAvailable(null);
						}
						oleDbServicesWrapper = new OleDbServicesWrapper(obj2);
						OleDbConnectionInternal.idataInitialize = oleDbServicesWrapper;
					}
				}
			}
			return oleDbServicesWrapper;
		}

		// Token: 0x06001EAE RID: 7854 RVA: 0x002587B0 File Offset: 0x00257BB0
		private static void VersionCheck()
		{
			if (ApartmentState.Unknown == Thread.CurrentThread.GetApartmentState())
			{
				OleDbConnectionInternal.SetMTAApartmentState();
			}
			ADP.CheckVersionMDAC(false);
		}

		// Token: 0x06001EAF RID: 7855 RVA: 0x002587D8 File Offset: 0x00257BD8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static void SetMTAApartmentState()
		{
			Thread.CurrentThread.SetApartmentState(ApartmentState.MTA);
		}

		// Token: 0x06001EB0 RID: 7856 RVA: 0x002587F0 File Offset: 0x00257BF0
		public static void ReleaseObjectPool()
		{
			OleDbConnectionInternal.idataInitialize = null;
		}

		// Token: 0x06001EB1 RID: 7857 RVA: 0x00258808 File Offset: 0x00257C08
		internal OleDbTransaction ValidateTransaction(OleDbTransaction transaction, string method)
		{
			if (this.weakTransaction != null)
			{
				OleDbTransaction oleDbTransaction = (OleDbTransaction)this.weakTransaction.Target;
				if (oleDbTransaction != null && this.weakTransaction.IsAlive)
				{
					oleDbTransaction = OleDbTransaction.TransactionUpdate(oleDbTransaction);
				}
				if (oleDbTransaction != null)
				{
					if (transaction == null)
					{
						throw ADP.TransactionRequired(method);
					}
					OleDbTransaction oleDbTransaction2 = OleDbTransaction.TransactionLast(oleDbTransaction);
					if (oleDbTransaction2 == transaction)
					{
						return transaction;
					}
					if (oleDbTransaction2.Connection != transaction.Connection)
					{
						throw ADP.TransactionConnectionMismatch();
					}
					throw ADP.TransactionCompleted();
				}
				else
				{
					this.weakTransaction = null;
				}
			}
			else if (transaction != null && transaction.Connection != null)
			{
				throw ADP.TransactionConnectionMismatch();
			}
			return null;
		}

		// Token: 0x06001EB2 RID: 7858 RVA: 0x00258894 File Offset: 0x00257C94
		internal Dictionary<string, OleDbPropertyInfo> GetPropertyInfo(Guid[] propertySets)
		{
			bool hasSession = this.HasSession;
			Dictionary<string, OleDbPropertyInfo> dictionary = null;
			if (propertySets == null)
			{
				propertySets = new Guid[0];
			}
			using (PropertyIDSet propertyIDSet = new PropertyIDSet(propertySets))
			{
				using (IDBPropertiesWrapper idbpropertiesWrapper = this.IDBProperties())
				{
					using (PropertyInfoSet propertyInfoSet = new PropertyInfoSet(idbpropertiesWrapper.Value, propertyIDSet))
					{
						dictionary = propertyInfoSet.GetValues();
					}
				}
			}
			return dictionary;
		}

		// Token: 0x04001268 RID: 4712
		private static volatile OleDbServicesWrapper idataInitialize;

		// Token: 0x04001269 RID: 4713
		private static object dataInitializeLock = new object();

		// Token: 0x0400126A RID: 4714
		internal readonly OleDbConnectionString ConnectionString;

		// Token: 0x0400126B RID: 4715
		private readonly DataSourceWrapper _datasrcwrp;

		// Token: 0x0400126C RID: 4716
		private readonly SessionWrapper _sessionwrp;

		// Token: 0x0400126D RID: 4717
		private WeakReference weakTransaction;

		// Token: 0x0400126E RID: 4718
		private bool _forcedAutomaticEnlistment;
	}
}
