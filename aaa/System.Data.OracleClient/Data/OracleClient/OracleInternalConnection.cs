using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.ProviderBase;
using System.Globalization;
using System.Text;
using System.Transactions;

namespace System.Data.OracleClient
{
	// Token: 0x0200006A RID: 106
	internal sealed class OracleInternalConnection : DbConnectionInternal
	{
		// Token: 0x060004F3 RID: 1267 RVA: 0x00067B9C File Offset: 0x00066F9C
		internal OracleInternalConnection(OracleConnectionString connectionOptions)
		{
			this._connectionOptions = connectionOptions;
			string userId = connectionOptions.UserId;
			string password = connectionOptions.Password;
			string dataSource = connectionOptions.DataSource;
			bool integratedSecurity = connectionOptions.IntegratedSecurity;
			bool unicode = connectionOptions.Unicode;
			bool omitOracleConnectionName = this._connectionOptions.OmitOracleConnectionName;
			this._connectionIsOpen = this.OpenOnLocalTransaction(userId, password, dataSource, integratedSecurity, unicode, omitOracleConnectionName);
			if (this.UnicodeEnabled)
			{
				this._encodingDatabase = Encoding.Unicode;
			}
			else if (this.ServerVersionAtLeastOracle8i)
			{
				this._encodingDatabase = new OracleEncoding(this);
			}
			else
			{
				this._encodingDatabase = Encoding.Default;
			}
			this._encodingNational = Encoding.Unicode;
			if (connectionOptions.Enlist && !connectionOptions.Pooling)
			{
				Transaction currentTransaction = ADP.GetCurrentTransaction();
				if (null != currentTransaction)
				{
					this.Enlist(userId, password, dataSource, currentTransaction, false);
				}
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060004F4 RID: 1268 RVA: 0x00067C74 File Offset: 0x00067074
		internal OciEnvironmentHandle EnvironmentHandle
		{
			get
			{
				return this._environmentHandle;
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060004F5 RID: 1269 RVA: 0x00067C88 File Offset: 0x00067088
		internal OciErrorHandle ErrorHandle
		{
			get
			{
				return this._errorHandle;
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060004F6 RID: 1270 RVA: 0x00067C9C File Offset: 0x0006709C
		internal bool HasTransaction
		{
			get
			{
				TransactionState transactionState = this.TransactionState;
				return TransactionState.LocalStarted == transactionState || TransactionState.GlobalStarted == transactionState;
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060004F7 RID: 1271 RVA: 0x00067CC0 File Offset: 0x000670C0
		public override string ServerVersion
		{
			get
			{
				if (this._serverVersionString == null)
				{
					string text = "no version available";
					NativeBuffer nativeBuffer = null;
					try
					{
						nativeBuffer = new NativeBuffer_ServerVersion(500);
						int num = TracedNativeMethods.OCIServerVersion(this.ServiceContextHandle, this.ErrorHandle, nativeBuffer);
						if (num != 0)
						{
							throw ADP.OracleError(this.ErrorHandle, num);
						}
						if (num == 0)
						{
							text = this.ServiceContextHandle.PtrToString(nativeBuffer);
						}
						this._serverVersion = OracleInternalConnection.ParseServerVersion(text);
						this._serverVersionString = string.Format(null, "{0}.{1}.{2}.{3}.{4} {5}", new object[]
						{
							(this._serverVersion >> 32) & 255L,
							(this._serverVersion >> 24) & 255L,
							(this._serverVersion >> 16) & 255L,
							(this._serverVersion >> 8) & 255L,
							this._serverVersion & 255L,
							text
						});
						this._serverVersionStringNormalized = string.Format(null, "{0:00}.{1:00}.{2:00}.{3:00}.{4:00} ", new object[]
						{
							(this._serverVersion >> 32) & 255L,
							(this._serverVersion >> 24) & 255L,
							(this._serverVersion >> 16) & 255L,
							(this._serverVersion >> 8) & 255L,
							this._serverVersion & 255L
						});
					}
					finally
					{
						if (nativeBuffer != null)
						{
							nativeBuffer.Dispose();
							nativeBuffer = null;
						}
					}
				}
				return this._serverVersionString;
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060004F8 RID: 1272 RVA: 0x00067E7C File Offset: 0x0006727C
		public override string ServerVersionNormalized
		{
			get
			{
				if (this._serverVersionStringNormalized == null)
				{
					string serverVersion = this.ServerVersion;
				}
				return this._serverVersionStringNormalized;
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x060004F9 RID: 1273 RVA: 0x00067EA0 File Offset: 0x000672A0
		internal bool ServerVersionAtLeastOracle8
		{
			get
			{
				return this.ServerVersionNumber >= 34359738368L;
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x060004FA RID: 1274 RVA: 0x00067EC4 File Offset: 0x000672C4
		internal bool ServerVersionAtLeastOracle8i
		{
			get
			{
				return this.ServerVersionNumber >= 34376515584L;
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060004FB RID: 1275 RVA: 0x00067EE8 File Offset: 0x000672E8
		internal bool ServerVersionAtLeastOracle9i
		{
			get
			{
				return this.ServerVersionNumber >= 38654705664L;
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060004FC RID: 1276 RVA: 0x00067F0C File Offset: 0x0006730C
		internal long ServerVersionNumber
		{
			get
			{
				if (0L == this._serverVersion)
				{
					string serverVersion = this.ServerVersion;
				}
				return this._serverVersion;
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060004FD RID: 1277 RVA: 0x00067F30 File Offset: 0x00067330
		internal OciServiceContextHandle ServiceContextHandle
		{
			get
			{
				return this._serviceContextHandle;
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060004FE RID: 1278 RVA: 0x00067F44 File Offset: 0x00067344
		internal OciSessionHandle SessionHandle
		{
			get
			{
				return this._sessionHandle;
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060004FF RID: 1279 RVA: 0x00067F58 File Offset: 0x00067358
		// (set) Token: 0x06000500 RID: 1280 RVA: 0x00067FB0 File Offset: 0x000673B0
		internal OracleTransaction Transaction
		{
			get
			{
				if (this._transaction != null && this._transaction.IsAlive)
				{
					if (((OracleTransaction)this._transaction.Target).Connection != null)
					{
						return (OracleTransaction)this._transaction.Target;
					}
					this._transaction.Target = null;
				}
				return null;
			}
			set
			{
				if (value == null)
				{
					this._transaction = null;
					return;
				}
				if (this._transaction != null)
				{
					this._transaction.Target = value;
					return;
				}
				this._transaction = new WeakReference(value);
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06000501 RID: 1281 RVA: 0x00067FEC File Offset: 0x000673EC
		// (set) Token: 0x06000502 RID: 1282 RVA: 0x00068000 File Offset: 0x00067400
		internal TransactionState TransactionState
		{
			get
			{
				return this._transactionState;
			}
			set
			{
				this._transactionState = value;
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000503 RID: 1283 RVA: 0x00068014 File Offset: 0x00067414
		internal bool UnicodeEnabled
		{
			get
			{
				return OCI.ClientVersionAtLeastOracle9i && (this.EnvironmentHandle == null || this.EnvironmentHandle.IsUnicode);
			}
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x00068040 File Offset: 0x00067440
		protected override void Activate(Transaction transaction)
		{
			bool flag = null != transaction;
			OracleConnectionString connectionOptions = this._connectionOptions;
			if (flag && connectionOptions.Enlist)
			{
				if (!transaction.Equals(base.EnlistedTransaction))
				{
					this.Enlist(connectionOptions.UserId, connectionOptions.Password, connectionOptions.DataSource, transaction, false);
					return;
				}
			}
			else if (!flag && this._enlistContext != null)
			{
				this.UnEnlist();
			}
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x000680A4 File Offset: 0x000674A4
		public override DbTransaction BeginTransaction(IsolationLevel il)
		{
			return this.BeginOracleTransaction(il);
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x000680B8 File Offset: 0x000674B8
		internal OracleTransaction BeginOracleTransaction(IsolationLevel il)
		{
			OracleConnection.ExecutePermission.Demand();
			if (this.TransactionState != TransactionState.AutoCommit)
			{
				throw ADP.NoParallelTransactions();
			}
			this.RollbackDeadTransaction();
			OracleTransaction oracleTransaction = new OracleTransaction(this.ProxyConnection(), il);
			this.Transaction = oracleTransaction;
			return oracleTransaction;
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x000680F8 File Offset: 0x000674F8
		private void CreateDeferredInfoMessage(OciErrorHandle errorHandle, int rc)
		{
			OracleException ex = OracleException.CreateException(errorHandle, rc);
			OracleInfoMessageEventArgs oracleInfoMessageEventArgs = new OracleInfoMessageEventArgs(ex);
			List<OracleInfoMessageEventArgs> list = this._deferredInfoMessageCollection;
			if (list == null)
			{
				list = (this._deferredInfoMessageCollection = new List<OracleInfoMessageEventArgs>());
			}
			list.Add(oracleInfoMessageEventArgs);
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x00068134 File Offset: 0x00067534
		internal void ConnectionIsBroken()
		{
			base.DoomThisConnection();
			OracleConnection oracleConnection = (OracleConnection)base.Owner;
			if (oracleConnection != null)
			{
				oracleConnection.Close();
				return;
			}
			this.Dispose();
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x00068164 File Offset: 0x00067564
		internal void Commit()
		{
			int num = TracedNativeMethods.OCITransCommit(this.ServiceContextHandle, this.ErrorHandle, OCI.MODE.OCI_DEFAULT);
			if (num != 0)
			{
				OracleException.Check(this.ErrorHandle, num);
			}
			this.TransactionState = TransactionState.AutoCommit;
			this.Transaction = null;
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x000681A4 File Offset: 0x000675A4
		protected override void Deactivate()
		{
			if (!base.IsConnectionDoomed && this.ErrorHandle != null && this.ErrorHandle.ConnectionIsBroken)
			{
				this.ConnectionIsBroken();
			}
			if (TransactionState.LocalStarted == this.TransactionState)
			{
				try
				{
					this.Rollback();
				}
				catch (Exception ex)
				{
					if (!ADP.IsCatchableExceptionType(ex))
					{
						throw;
					}
					ADP.TraceException(ex);
					base.DoomThisConnection();
				}
			}
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x0006821C File Offset: 0x0006761C
		public override void Dispose()
		{
			this.Deactivate();
			OciEnlistContext.SafeDispose(ref this._enlistContext);
			OciHandle.SafeDispose(ref this._sessionHandle);
			OciHandle.SafeDispose(ref this._serviceContextHandle);
			OciHandle.SafeDispose(ref this._serverHandle);
			OciHandle.SafeDispose(ref this._errorHandle);
			OciHandle.SafeDispose(ref this._environmentHandle);
			if (this._scratchBuffer != null)
			{
				this._scratchBuffer.Dispose();
			}
			this._scratchBuffer = null;
			this._encodingDatabase = null;
			this._encodingNational = null;
			this._transaction = null;
			this._serverVersionString = null;
			base.Dispose();
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x000682B0 File Offset: 0x000676B0
		private void Enlist(string userName, string password, string serverName, Transaction transaction, bool manualEnlistment)
		{
			this.UnEnlist();
			if (!OCI.ClientVersionAtLeastOracle9i)
			{
				throw ADP.DistribTxRequiresOracle9i();
			}
			if (null != transaction)
			{
				if (this.HasTransaction)
				{
					throw ADP.TransactionPresent();
				}
				byte[] bytes = Encoding.Default.GetBytes(password);
				byte[] bytes2 = Encoding.Default.GetBytes(userName);
				byte[] bytes3 = Encoding.Default.GetBytes(serverName);
				this._enlistContext = new OciEnlistContext(bytes2, bytes, bytes3, this.ServiceContextHandle, this.ErrorHandle);
				this._enlistContext.Join(this, transaction);
				this.TransactionState = TransactionState.GlobalStarted;
			}
			else
			{
				this.TransactionState = TransactionState.AutoCommit;
			}
			base.EnlistedTransaction = transaction;
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x0006834C File Offset: 0x0006774C
		public override void EnlistTransaction(Transaction transaction)
		{
			OracleConnectionString connectionOptions = this._connectionOptions;
			this.RollbackDeadTransaction();
			this.Enlist(connectionOptions.UserId, connectionOptions.Password, connectionOptions.DataSource, transaction, true);
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x00068380 File Offset: 0x00067780
		internal void FireDeferredInfoMessageEvents(OracleConnection outerConnection)
		{
			List<OracleInfoMessageEventArgs> deferredInfoMessageCollection = this._deferredInfoMessageCollection;
			this._deferredInfoMessageCollection = null;
			if (deferredInfoMessageCollection != null)
			{
				foreach (OracleInfoMessageEventArgs oracleInfoMessageEventArgs in deferredInfoMessageCollection)
				{
					if (oracleInfoMessageEventArgs != null)
					{
						outerConnection.OnInfoMessage(oracleInfoMessageEventArgs);
					}
				}
			}
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x000683F0 File Offset: 0x000677F0
		internal byte[] GetBytes(string value, bool useNationalCharacterSet)
		{
			byte[] array;
			if (useNationalCharacterSet)
			{
				array = this._encodingNational.GetBytes(value);
			}
			else
			{
				array = this._encodingDatabase.GetBytes(value);
			}
			return array;
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x00068420 File Offset: 0x00067820
		internal NativeBuffer GetScratchBuffer(int minSize)
		{
			NativeBuffer nativeBuffer = this._scratchBuffer;
			if (nativeBuffer == null || nativeBuffer.Length < minSize)
			{
				if (nativeBuffer != null)
				{
					nativeBuffer.Dispose();
				}
				nativeBuffer = new NativeBuffer_ScratchBuffer(minSize);
				this._scratchBuffer = nativeBuffer;
			}
			return nativeBuffer;
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x00068458 File Offset: 0x00067858
		internal string GetString(byte[] bytearray)
		{
			return this._encodingDatabase.GetString(bytearray);
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x00068474 File Offset: 0x00067874
		internal string GetString(byte[] bytearray, bool useNationalCharacterSet)
		{
			if (useNationalCharacterSet)
			{
				return this._encodingNational.GetString(bytearray);
			}
			return this._encodingDatabase.GetString(bytearray);
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x000684A0 File Offset: 0x000678A0
		internal TimeSpan GetServerTimeZoneAdjustmentToUTC(OracleConnection connection)
		{
			TimeSpan timeSpan = this._serverTimeZoneAdjustment;
			if (TimeSpan.MinValue == timeSpan)
			{
				if (this.ServerVersionAtLeastOracle9i)
				{
					string text = (string)new OracleCommand
					{
						Connection = connection,
						Transaction = this.Transaction,
						CommandText = "select tz_offset(dbtimezone) from dual"
					}.ExecuteScalar();
					int num = int.Parse(text.Substring(0, 3), CultureInfo.InvariantCulture);
					int num2 = int.Parse(text.Substring(4, 2), CultureInfo.InvariantCulture);
					timeSpan = new TimeSpan(num, num2, 0);
				}
				else
				{
					timeSpan = TimeSpan.Zero;
				}
				this._serverTimeZoneAdjustment = timeSpan;
			}
			return this._serverTimeZoneAdjustment;
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x00068544 File Offset: 0x00067944
		private bool OpenOnLocalTransaction(string userName, string password, string serverName, bool integratedSecurity, bool unicode, bool omitOracleConnectionName)
		{
			int num = 0;
			IntPtr zero = IntPtr.Zero;
			OCI.MODE mode = OCI.MODE.OCI_THREADED | OCI.MODE.OCI_OBJECT;
			OCI.DetermineClientVersion();
			if (unicode)
			{
				if (OCI.ClientVersionAtLeastOracle9i)
				{
					mode |= OCI.MODE.OCI_UTF16;
				}
				else
				{
					unicode = false;
				}
			}
			this._environmentHandle = new OciEnvironmentHandle(mode, unicode);
			if (this._environmentHandle.IsInvalid)
			{
				throw ADP.CouldNotCreateEnvironment("OCIEnvCreate", num);
			}
			this._errorHandle = new OciErrorHandle(this._environmentHandle);
			this._serverHandle = new OciServerHandle(this._errorHandle);
			this._sessionHandle = new OciSessionHandle(this._serverHandle);
			this._serviceContextHandle = new OciServiceContextHandle(this._sessionHandle);
			try
			{
				num = TracedNativeMethods.OCIServerAttach(this._serverHandle, this._errorHandle, serverName, serverName.Length, OCI.MODE.OCI_DEFAULT);
				if (num != 0)
				{
					if (1 == num)
					{
						this.CreateDeferredInfoMessage(this.ErrorHandle, num);
					}
					else
					{
						OracleException.Check(this.ErrorHandle, num);
					}
				}
				this._serviceContextHandle.SetAttribute(OCI.ATTR.OCI_ATTR_SERVER, this._serverHandle, this._errorHandle);
				OCI.CRED cred;
				if (integratedSecurity)
				{
					cred = OCI.CRED.OCI_CRED_EXT;
				}
				else
				{
					cred = OCI.CRED.OCI_CRED_RDBMS;
					this._sessionHandle.SetAttribute(OCI.ATTR.OCI_ATTR_USERNAME, userName, this._errorHandle);
					if (password != null)
					{
						this._sessionHandle.SetAttribute(OCI.ATTR.OCI_ATTR_PASSWORD, password, this._errorHandle);
					}
				}
				if (!omitOracleConnectionName)
				{
					string text = this._connectionOptions.DataSource;
					if (text.Length > 16)
					{
						text = text.Substring(0, 16);
					}
					this._serverHandle.SetAttribute(OCI.ATTR.OCI_ATTR_EXTERNAL_NAME, text, this._errorHandle);
					this._serverHandle.SetAttribute(OCI.ATTR.OCI_ATTR_INTERNAL_NAME, text, this._errorHandle);
				}
				num = TracedNativeMethods.OCISessionBegin(this._serviceContextHandle, this._errorHandle, this._sessionHandle, cred, OCI.MODE.OCI_DEFAULT);
				if (num != 0)
				{
					if (1 == num)
					{
						this.CreateDeferredInfoMessage(this.ErrorHandle, num);
					}
					else
					{
						OracleException.Check(this.ErrorHandle, num);
					}
				}
				this._serviceContextHandle.SetAttribute(OCI.ATTR.OCI_ATTR_SESSION, this._sessionHandle, this._errorHandle);
			}
			catch (OracleException)
			{
				OciHandle.SafeDispose(ref this._serviceContextHandle);
				OciHandle.SafeDispose(ref this._sessionHandle);
				OciHandle.SafeDispose(ref this._serverHandle);
				OciHandle.SafeDispose(ref this._errorHandle);
				OciHandle.SafeDispose(ref this._environmentHandle);
				throw;
			}
			return true;
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x00068768 File Offset: 0x00067B68
		internal static long ParseServerVersion(string versionString)
		{
			OracleInternalConnection.PARSERSTATE parserstate = OracleInternalConnection.PARSERSTATE.NOTHINGYET;
			int num = 0;
			int num2 = 0;
			long num3 = 0L;
			versionString += "0.0.0.0.0 ";
			for (int i = 0; i < versionString.Length; i++)
			{
				switch (parserstate)
				{
				case OracleInternalConnection.PARSERSTATE.NOTHINGYET:
					if (char.IsDigit(versionString, i))
					{
						parserstate = OracleInternalConnection.PARSERSTATE.DIGIT;
						num = i;
					}
					break;
				case OracleInternalConnection.PARSERSTATE.PERIOD:
					if (char.IsDigit(versionString, i))
					{
						parserstate = OracleInternalConnection.PARSERSTATE.DIGIT;
						num = i;
					}
					else
					{
						parserstate = OracleInternalConnection.PARSERSTATE.NOTHINGYET;
						num2 = 0;
						num3 = 0L;
					}
					break;
				case OracleInternalConnection.PARSERSTATE.DIGIT:
					if ("." == versionString.Substring(i, 1) || 4 == num2)
					{
						num2++;
						parserstate = OracleInternalConnection.PARSERSTATE.PERIOD;
						long num4 = (long)int.Parse(versionString.Substring(num, i - num), CultureInfo.InvariantCulture);
						num3 = (num3 << 8) + num4;
						if (5 == num2)
						{
							return num3;
						}
					}
					else if (!char.IsDigit(versionString, i))
					{
						parserstate = OracleInternalConnection.PARSERSTATE.NOTHINGYET;
						num2 = 0;
						num3 = 0L;
					}
					break;
				}
			}
			return 0L;
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x00068844 File Offset: 0x00067C44
		private OracleConnection ProxyConnection()
		{
			OracleConnection oracleConnection = (OracleConnection)base.Owner;
			if (oracleConnection == null)
			{
				throw ADP.InvalidOperation("internal connection without a proxy?");
			}
			return oracleConnection;
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x0006886C File Offset: 0x00067C6C
		internal void Rollback()
		{
			if (TransactionState.GlobalStarted != this._transactionState)
			{
				int num = TracedNativeMethods.OCITransRollback(this.ServiceContextHandle, this.ErrorHandle, OCI.MODE.OCI_DEFAULT);
				if (num != 0)
				{
					OracleException.Check(this.ErrorHandle, num);
				}
				this.TransactionState = TransactionState.AutoCommit;
			}
			this.Transaction = null;
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x000688B4 File Offset: 0x00067CB4
		internal void RollbackDeadTransaction()
		{
			if (this._transaction != null && !this._transaction.IsAlive)
			{
				this.Rollback();
			}
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x000688DC File Offset: 0x00067CDC
		private void UnEnlist()
		{
			if (this._enlistContext != null)
			{
				this.TransactionState = TransactionState.AutoCommit;
				this._enlistContext.Join(this, null);
				OciEnlistContext.SafeDispose(ref this._enlistContext);
				this.Transaction = null;
			}
		}

		// Token: 0x04000440 RID: 1088
		private OracleConnectionString _connectionOptions;

		// Token: 0x04000441 RID: 1089
		private OciEnvironmentHandle _environmentHandle;

		// Token: 0x04000442 RID: 1090
		private OciErrorHandle _errorHandle;

		// Token: 0x04000443 RID: 1091
		private OciServerHandle _serverHandle;

		// Token: 0x04000444 RID: 1092
		private OciServiceContextHandle _serviceContextHandle;

		// Token: 0x04000445 RID: 1093
		private OciSessionHandle _sessionHandle;

		// Token: 0x04000446 RID: 1094
		private OciEnlistContext _enlistContext;

		// Token: 0x04000447 RID: 1095
		private bool _connectionIsOpen;

		// Token: 0x04000448 RID: 1096
		private WeakReference _transaction;

		// Token: 0x04000449 RID: 1097
		private TransactionState _transactionState;

		// Token: 0x0400044A RID: 1098
		private List<OracleInfoMessageEventArgs> _deferredInfoMessageCollection;

		// Token: 0x0400044B RID: 1099
		private long _serverVersion;

		// Token: 0x0400044C RID: 1100
		private string _serverVersionString;

		// Token: 0x0400044D RID: 1101
		private string _serverVersionStringNormalized;

		// Token: 0x0400044E RID: 1102
		private TimeSpan _serverTimeZoneAdjustment = TimeSpan.MinValue;

		// Token: 0x0400044F RID: 1103
		private NativeBuffer _scratchBuffer;

		// Token: 0x04000450 RID: 1104
		private Encoding _encodingDatabase;

		// Token: 0x04000451 RID: 1105
		private Encoding _encodingNational;

		// Token: 0x0200006B RID: 107
		internal enum PARSERSTATE
		{
			// Token: 0x04000453 RID: 1107
			NOTHINGYET = 1,
			// Token: 0x04000454 RID: 1108
			PERIOD,
			// Token: 0x04000455 RID: 1109
			DIGIT
		}
	}
}
