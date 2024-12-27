using System;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Transactions;

namespace System.Data.Odbc
{
	// Token: 0x020001DA RID: 474
	internal sealed class OdbcConnectionHandle : OdbcHandle
	{
		// Token: 0x06001A6E RID: 6766 RVA: 0x00243260 File Offset: 0x00242660
		internal OdbcConnectionHandle(OdbcConnection connection, OdbcConnectionString constr, OdbcEnvironmentHandle environmentHandle)
			: base(ODBC32.SQL_HANDLE.DBC, environmentHandle)
		{
			if (connection == null)
			{
				throw ADP.ArgumentNull("connection");
			}
			if (constr == null)
			{
				throw ADP.ArgumentNull("constr");
			}
			int connectionTimeout = connection.ConnectionTimeout;
			ODBC32.RetCode retCode = this.SetConnectionAttribute2(ODBC32.SQL_ATTR.LOGIN_TIMEOUT, (IntPtr)connectionTimeout, -5);
			string text = constr.UsersConnectionString(false);
			retCode = this.Connect(text);
			connection.HandleError(this, retCode);
		}

		// Token: 0x06001A6F RID: 6767 RVA: 0x002432C4 File Offset: 0x002426C4
		private ODBC32.RetCode AutoCommitOff()
		{
			RuntimeHelpers.PrepareConstrainedRegions();
			ODBC32.RetCode retCode;
			try
			{
			}
			finally
			{
				retCode = UnsafeNativeMethods.SQLSetConnectAttrW(this, ODBC32.SQL_ATTR.AUTOCOMMIT, ODBC32.SQL_AUTOCOMMIT_OFF, -5);
				switch (retCode)
				{
				case ODBC32.RetCode.SUCCESS:
				case ODBC32.RetCode.SUCCESS_WITH_INFO:
					this._handleState = OdbcConnectionHandle.HandleState.Transacted;
					break;
				}
			}
			ODBC.TraceODBC(3, "SQLSetConnectAttrW", retCode);
			return retCode;
		}

		// Token: 0x06001A70 RID: 6768 RVA: 0x0024332C File Offset: 0x0024272C
		internal ODBC32.RetCode BeginTransaction(ref IsolationLevel isolevel)
		{
			ODBC32.RetCode retCode = ODBC32.RetCode.SUCCESS;
			if (IsolationLevel.Unspecified != isolevel)
			{
				IsolationLevel isolationLevel = isolevel;
				ODBC32.SQL_TRANSACTION sql_TRANSACTION;
				if (isolationLevel <= IsolationLevel.ReadCommitted)
				{
					if (isolationLevel == IsolationLevel.Chaos)
					{
						throw ODBC.NotSupportedIsolationLevel(isolevel);
					}
					if (isolationLevel == IsolationLevel.ReadUncommitted)
					{
						sql_TRANSACTION = ODBC32.SQL_TRANSACTION.READ_UNCOMMITTED;
						goto IL_006B;
					}
					if (isolationLevel == IsolationLevel.ReadCommitted)
					{
						sql_TRANSACTION = ODBC32.SQL_TRANSACTION.READ_COMMITTED;
						goto IL_006B;
					}
				}
				else
				{
					if (isolationLevel == IsolationLevel.RepeatableRead)
					{
						sql_TRANSACTION = ODBC32.SQL_TRANSACTION.REPEATABLE_READ;
						goto IL_006B;
					}
					if (isolationLevel == IsolationLevel.Serializable)
					{
						sql_TRANSACTION = ODBC32.SQL_TRANSACTION.SERIALIZABLE;
						goto IL_006B;
					}
					if (isolationLevel == IsolationLevel.Snapshot)
					{
						sql_TRANSACTION = ODBC32.SQL_TRANSACTION.SNAPSHOT;
						goto IL_006B;
					}
				}
				throw ADP.InvalidIsolationLevel(isolevel);
				IL_006B:
				retCode = this.SetConnectionAttribute2(ODBC32.SQL_ATTR.TXN_ISOLATION, (IntPtr)((long)sql_TRANSACTION), -6);
				if (ODBC32.RetCode.SUCCESS_WITH_INFO == retCode)
				{
					isolevel = IsolationLevel.Unspecified;
				}
			}
			switch (retCode)
			{
			case ODBC32.RetCode.SUCCESS:
			case ODBC32.RetCode.SUCCESS_WITH_INFO:
				retCode = this.AutoCommitOff();
				this._handleState = OdbcConnectionHandle.HandleState.TransactionInProgress;
				break;
			}
			return retCode;
		}

		// Token: 0x06001A71 RID: 6769 RVA: 0x002433E0 File Offset: 0x002427E0
		internal ODBC32.RetCode CompleteTransaction(short transactionOperation)
		{
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			ODBC32.RetCode retCode2;
			try
			{
				base.DangerousAddRef(ref flag);
				ODBC32.RetCode retCode = this.CompleteTransaction(transactionOperation, this.handle);
				retCode2 = retCode;
			}
			finally
			{
				if (flag)
				{
					base.DangerousRelease();
				}
			}
			return retCode2;
		}

		// Token: 0x06001A72 RID: 6770 RVA: 0x00243438 File Offset: 0x00242838
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		private ODBC32.RetCode CompleteTransaction(short transactionOperation, IntPtr handle)
		{
			ODBC32.RetCode retCode = ODBC32.RetCode.SUCCESS;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				if (OdbcConnectionHandle.HandleState.TransactionInProgress == this._handleState)
				{
					retCode = UnsafeNativeMethods.SQLEndTran(base.HandleType, handle, transactionOperation);
					if (retCode == ODBC32.RetCode.SUCCESS || ODBC32.RetCode.SUCCESS_WITH_INFO == retCode)
					{
						this._handleState = OdbcConnectionHandle.HandleState.Transacted;
					}
					Bid.TraceSqlReturn("<odbc.SQLEndTran|API|ODBC|RET> %08X{SQLRETURN}\n", retCode);
				}
				if (OdbcConnectionHandle.HandleState.Transacted == this._handleState)
				{
					retCode = UnsafeNativeMethods.SQLSetConnectAttrW(handle, ODBC32.SQL_ATTR.AUTOCOMMIT, ODBC32.SQL_AUTOCOMMIT_ON, -5);
					this._handleState = OdbcConnectionHandle.HandleState.Connected;
					Bid.TraceSqlReturn("<odbc.SQLSetConnectAttr|API|ODBC|RET> %08X{SQLRETURN}\n", retCode);
				}
			}
			return retCode;
		}

		// Token: 0x06001A73 RID: 6771 RVA: 0x002434C8 File Offset: 0x002428C8
		private ODBC32.RetCode Connect(string connectionString)
		{
			RuntimeHelpers.PrepareConstrainedRegions();
			ODBC32.RetCode retCode;
			try
			{
			}
			finally
			{
				short num;
				retCode = UnsafeNativeMethods.SQLDriverConnectW(this, ADP.PtrZero, connectionString, -3, ADP.PtrZero, 0, out num, 0);
				switch (retCode)
				{
				case ODBC32.RetCode.SUCCESS:
				case ODBC32.RetCode.SUCCESS_WITH_INFO:
					this._handleState = OdbcConnectionHandle.HandleState.Connected;
					break;
				}
			}
			ODBC.TraceODBC(3, "SQLDriverConnectW", retCode);
			return retCode;
		}

		// Token: 0x06001A74 RID: 6772 RVA: 0x00243538 File Offset: 0x00242938
		protected override bool ReleaseHandle()
		{
			ODBC32.RetCode retCode = this.CompleteTransaction(1, this.handle);
			if (OdbcConnectionHandle.HandleState.Connected == this._handleState || OdbcConnectionHandle.HandleState.TransactionInProgress == this._handleState)
			{
				retCode = UnsafeNativeMethods.SQLDisconnect(this.handle);
				this._handleState = OdbcConnectionHandle.HandleState.Allocated;
				Bid.TraceSqlReturn("<odbc.SQLDisconnect|API|ODBC|RET> %08X{SQLRETURN}\n", retCode);
			}
			return base.ReleaseHandle();
		}

		// Token: 0x06001A75 RID: 6773 RVA: 0x0024358C File Offset: 0x0024298C
		internal ODBC32.RetCode GetConnectionAttribute(ODBC32.SQL_ATTR attribute, byte[] buffer, out int cbActual)
		{
			ODBC32.RetCode retCode = UnsafeNativeMethods.SQLGetConnectAttrW(this, attribute, buffer, buffer.Length, out cbActual);
			Bid.Trace("<odbc.SQLGetConnectAttr|ODBC> SQLRETURN=%d, Attribute=%d, BufferLength=%d, StringLength=%d\n", (int)retCode, (int)attribute, buffer.Length, cbActual);
			return retCode;
		}

		// Token: 0x06001A76 RID: 6774 RVA: 0x002435B8 File Offset: 0x002429B8
		internal ODBC32.RetCode GetFunctions(ODBC32.SQL_API fFunction, out short fExists)
		{
			ODBC32.RetCode retCode = UnsafeNativeMethods.SQLGetFunctions(this, fFunction, out fExists);
			ODBC.TraceODBC(3, "SQLGetFunctions", retCode);
			return retCode;
		}

		// Token: 0x06001A77 RID: 6775 RVA: 0x002435DC File Offset: 0x002429DC
		internal ODBC32.RetCode GetInfo2(ODBC32.SQL_INFO info, byte[] buffer, out short cbActual)
		{
			ODBC32.RetCode retCode = UnsafeNativeMethods.SQLGetInfoW(this, info, buffer, checked((short)buffer.Length), out cbActual);
			Bid.Trace("<odbc.SQLGetInfo|ODBC> SQLRETURN=%d, InfoType=%d, BufferLength=%d, StringLength=%d\n", (int)retCode, (int)info, buffer.Length, (int)cbActual);
			return retCode;
		}

		// Token: 0x06001A78 RID: 6776 RVA: 0x0024360C File Offset: 0x00242A0C
		internal ODBC32.RetCode GetInfo1(ODBC32.SQL_INFO info, byte[] buffer)
		{
			ODBC32.RetCode retCode = UnsafeNativeMethods.SQLGetInfoW(this, info, buffer, checked((short)buffer.Length), ADP.PtrZero);
			Bid.Trace("<odbc.SQLGetInfo|ODBC> SQLRETURN=%d, InfoType=%d, BufferLength=%d\n", (int)retCode, (int)info, buffer.Length);
			return retCode;
		}

		// Token: 0x06001A79 RID: 6777 RVA: 0x0024363C File Offset: 0x00242A3C
		internal ODBC32.RetCode SetConnectionAttribute2(ODBC32.SQL_ATTR attribute, IntPtr value, int length)
		{
			ODBC32.RetCode retCode = UnsafeNativeMethods.SQLSetConnectAttrW(this, attribute, value, length);
			ODBC.TraceODBC(3, "SQLSetConnectAttrW", retCode);
			return retCode;
		}

		// Token: 0x06001A7A RID: 6778 RVA: 0x00243660 File Offset: 0x00242A60
		internal ODBC32.RetCode SetConnectionAttribute3(ODBC32.SQL_ATTR attribute, string buffer, int length)
		{
			ODBC32.RetCode retCode = UnsafeNativeMethods.SQLSetConnectAttrW(this, attribute, buffer, length);
			Bid.Trace("<odbc.SQLSetConnectAttr|ODBC> SQLRETURN=%d, Attribute=%d, BufferLength=%d\n", (int)retCode, (int)attribute, buffer.Length);
			return retCode;
		}

		// Token: 0x06001A7B RID: 6779 RVA: 0x0024368C File Offset: 0x00242A8C
		internal ODBC32.RetCode SetConnectionAttribute4(ODBC32.SQL_ATTR attribute, IDtcTransaction transaction, int length)
		{
			ODBC32.RetCode retCode = UnsafeNativeMethods.SQLSetConnectAttrW(this, attribute, transaction, length);
			ODBC.TraceODBC(3, "SQLSetConnectAttrW", retCode);
			return retCode;
		}

		// Token: 0x04000F9D RID: 3997
		private OdbcConnectionHandle.HandleState _handleState;

		// Token: 0x020001DB RID: 475
		private enum HandleState
		{
			// Token: 0x04000F9F RID: 3999
			Allocated,
			// Token: 0x04000FA0 RID: 4000
			Connected,
			// Token: 0x04000FA1 RID: 4001
			Transacted,
			// Token: 0x04000FA2 RID: 4002
			TransactionInProgress
		}
	}
}
