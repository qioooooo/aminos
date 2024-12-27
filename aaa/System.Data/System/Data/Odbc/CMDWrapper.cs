using System;
using System.Data.Common;

namespace System.Data.Odbc
{
	// Token: 0x020001D4 RID: 468
	internal sealed class CMDWrapper
	{
		// Token: 0x060019B0 RID: 6576 RVA: 0x002405F8 File Offset: 0x0023F9F8
		internal CMDWrapper(OdbcConnection connection)
		{
			this._connection = connection;
		}

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x060019B1 RID: 6577 RVA: 0x00240614 File Offset: 0x0023FA14
		// (set) Token: 0x060019B2 RID: 6578 RVA: 0x00240628 File Offset: 0x0023FA28
		internal bool Canceling
		{
			get
			{
				return this._canceling;
			}
			set
			{
				this._canceling = value;
			}
		}

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x060019B3 RID: 6579 RVA: 0x0024063C File Offset: 0x0023FA3C
		internal OdbcConnection Connection
		{
			get
			{
				return this._connection;
			}
		}

		// Token: 0x17000345 RID: 837
		// (set) Token: 0x060019B4 RID: 6580 RVA: 0x00240650 File Offset: 0x0023FA50
		internal bool HasBoundColumns
		{
			set
			{
				this._hasBoundColumns = value;
			}
		}

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x060019B5 RID: 6581 RVA: 0x00240664 File Offset: 0x0023FA64
		internal OdbcStatementHandle StatementHandle
		{
			get
			{
				return this._stmt;
			}
		}

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x060019B6 RID: 6582 RVA: 0x00240678 File Offset: 0x0023FA78
		internal OdbcStatementHandle KeyInfoStatement
		{
			get
			{
				return this._keyinfostmt;
			}
		}

		// Token: 0x060019B7 RID: 6583 RVA: 0x0024068C File Offset: 0x0023FA8C
		internal void CreateKeyInfoStatementHandle()
		{
			this.DisposeKeyInfoStatementHandle();
			this._keyinfostmt = this._connection.CreateStatementHandle();
		}

		// Token: 0x060019B8 RID: 6584 RVA: 0x002406B0 File Offset: 0x0023FAB0
		internal void CreateStatementHandle()
		{
			this.DisposeStatementHandle();
			this._stmt = this._connection.CreateStatementHandle();
		}

		// Token: 0x060019B9 RID: 6585 RVA: 0x002406D4 File Offset: 0x0023FAD4
		internal void Dispose()
		{
			if (this._dataReaderBuf != null)
			{
				this._dataReaderBuf.Dispose();
				this._dataReaderBuf = null;
			}
			this.DisposeStatementHandle();
			CNativeBuffer nativeParameterBuffer = this._nativeParameterBuffer;
			this._nativeParameterBuffer = null;
			if (nativeParameterBuffer != null)
			{
				nativeParameterBuffer.Dispose();
			}
			this._ssKeyInfoModeOn = false;
			this._ssKeyInfoModeOff = false;
		}

		// Token: 0x060019BA RID: 6586 RVA: 0x00240728 File Offset: 0x0023FB28
		private void DisposeDescriptorHandle()
		{
			OdbcDescriptorHandle hdesc = this._hdesc;
			if (hdesc != null)
			{
				this._hdesc = null;
				hdesc.Dispose();
			}
		}

		// Token: 0x060019BB RID: 6587 RVA: 0x0024074C File Offset: 0x0023FB4C
		internal void DisposeStatementHandle()
		{
			this.DisposeKeyInfoStatementHandle();
			this.DisposeDescriptorHandle();
			OdbcStatementHandle stmt = this._stmt;
			if (stmt != null)
			{
				this._stmt = null;
				stmt.Dispose();
			}
		}

		// Token: 0x060019BC RID: 6588 RVA: 0x0024077C File Offset: 0x0023FB7C
		internal void DisposeKeyInfoStatementHandle()
		{
			OdbcStatementHandle keyinfostmt = this._keyinfostmt;
			if (keyinfostmt != null)
			{
				this._keyinfostmt = null;
				keyinfostmt.Dispose();
			}
		}

		// Token: 0x060019BD RID: 6589 RVA: 0x002407A0 File Offset: 0x0023FBA0
		internal void FreeStatementHandle(ODBC32.STMT stmt)
		{
			this.DisposeDescriptorHandle();
			OdbcStatementHandle stmt2 = this._stmt;
			if (stmt2 != null)
			{
				try
				{
					ODBC32.RetCode retCode = stmt2.FreeStatement(stmt);
					this.StatementErrorHandler(retCode);
				}
				catch (Exception ex)
				{
					if (ADP.IsCatchableExceptionType(ex))
					{
						this._stmt = null;
						stmt2.Dispose();
					}
					throw;
				}
			}
		}

		// Token: 0x060019BE RID: 6590 RVA: 0x00240804 File Offset: 0x0023FC04
		internal void FreeKeyInfoStatementHandle(ODBC32.STMT stmt)
		{
			OdbcStatementHandle keyinfostmt = this._keyinfostmt;
			if (keyinfostmt != null)
			{
				try
				{
					keyinfostmt.FreeStatement(stmt);
				}
				catch (Exception ex)
				{
					if (ADP.IsCatchableExceptionType(ex))
					{
						this._keyinfostmt = null;
						keyinfostmt.Dispose();
					}
					throw;
				}
			}
		}

		// Token: 0x060019BF RID: 6591 RVA: 0x0024085C File Offset: 0x0023FC5C
		internal OdbcDescriptorHandle GetDescriptorHandle(ODBC32.SQL_ATTR attribute)
		{
			OdbcDescriptorHandle odbcDescriptorHandle = this._hdesc;
			if (this._hdesc == null)
			{
				odbcDescriptorHandle = (this._hdesc = new OdbcDescriptorHandle(this._stmt, attribute));
			}
			return odbcDescriptorHandle;
		}

		// Token: 0x060019C0 RID: 6592 RVA: 0x00240890 File Offset: 0x0023FC90
		internal string GetDiagSqlState()
		{
			string text;
			this._stmt.GetDiagnosticField(out text);
			return text;
		}

		// Token: 0x060019C1 RID: 6593 RVA: 0x002408AC File Offset: 0x0023FCAC
		internal void StatementErrorHandler(ODBC32.RetCode retcode)
		{
			switch (retcode)
			{
			case ODBC32.RetCode.SUCCESS:
			case ODBC32.RetCode.SUCCESS_WITH_INFO:
				this._connection.HandleErrorNoThrow(this._stmt, retcode);
				return;
			default:
				throw this._connection.HandleErrorNoThrow(this._stmt, retcode);
			}
		}

		// Token: 0x060019C2 RID: 6594 RVA: 0x002408F4 File Offset: 0x0023FCF4
		internal void UnbindStmtColumns()
		{
			if (this._hasBoundColumns)
			{
				this.FreeStatementHandle(ODBC32.STMT.UNBIND);
				this._hasBoundColumns = false;
			}
		}

		// Token: 0x04000F78 RID: 3960
		private OdbcStatementHandle _stmt;

		// Token: 0x04000F79 RID: 3961
		private OdbcStatementHandle _keyinfostmt;

		// Token: 0x04000F7A RID: 3962
		internal OdbcDescriptorHandle _hdesc;

		// Token: 0x04000F7B RID: 3963
		internal CNativeBuffer _nativeParameterBuffer;

		// Token: 0x04000F7C RID: 3964
		internal CNativeBuffer _dataReaderBuf;

		// Token: 0x04000F7D RID: 3965
		private readonly OdbcConnection _connection;

		// Token: 0x04000F7E RID: 3966
		private bool _canceling;

		// Token: 0x04000F7F RID: 3967
		internal bool _hasBoundColumns;

		// Token: 0x04000F80 RID: 3968
		internal bool _ssKeyInfoModeOn;

		// Token: 0x04000F81 RID: 3969
		internal bool _ssKeyInfoModeOff;
	}
}
