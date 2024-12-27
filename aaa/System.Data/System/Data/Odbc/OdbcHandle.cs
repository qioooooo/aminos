using System;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Data.Odbc
{
	// Token: 0x020001D9 RID: 473
	internal abstract class OdbcHandle : SafeHandle
	{
		// Token: 0x06001A67 RID: 6759 RVA: 0x00242F7C File Offset: 0x0024237C
		protected OdbcHandle(ODBC32.SQL_HANDLE handleType, OdbcHandle parentHandle)
			: base(IntPtr.Zero, true)
		{
			this._handleType = handleType;
			bool flag = false;
			ODBC32.RetCode retCode = ODBC32.RetCode.SUCCESS;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				switch (handleType)
				{
				case ODBC32.SQL_HANDLE.ENV:
					retCode = UnsafeNativeMethods.SQLAllocHandle(handleType, IntPtr.Zero, out this.handle);
					break;
				case ODBC32.SQL_HANDLE.DBC:
				case ODBC32.SQL_HANDLE.STMT:
					parentHandle.DangerousAddRef(ref flag);
					retCode = UnsafeNativeMethods.SQLAllocHandle(handleType, parentHandle, out this.handle);
					break;
				}
			}
			finally
			{
				if (flag)
				{
					switch (handleType)
					{
					case ODBC32.SQL_HANDLE.DBC:
					case ODBC32.SQL_HANDLE.STMT:
						if (IntPtr.Zero != this.handle)
						{
							this._parentHandle = parentHandle;
						}
						else
						{
							parentHandle.DangerousRelease();
						}
						break;
					}
				}
			}
			Bid.TraceSqlReturn("<odbc.SQLAllocHandle|API|ODBC|RET> %08X{SQLRETURN}\n", retCode);
			if (ADP.PtrZero == this.handle || retCode != ODBC32.RetCode.SUCCESS)
			{
				throw ODBC.CantAllocateEnvironmentHandle(retCode);
			}
		}

		// Token: 0x06001A68 RID: 6760 RVA: 0x00243068 File Offset: 0x00242468
		internal OdbcHandle(OdbcStatementHandle parentHandle, ODBC32.SQL_ATTR attribute)
			: base(IntPtr.Zero, true)
		{
			this._handleType = ODBC32.SQL_HANDLE.DESC;
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			ODBC32.RetCode statementAttribute;
			try
			{
				parentHandle.DangerousAddRef(ref flag);
				int num;
				statementAttribute = parentHandle.GetStatementAttribute(attribute, out this.handle, out num);
			}
			finally
			{
				if (flag)
				{
					if (IntPtr.Zero != this.handle)
					{
						this._parentHandle = parentHandle;
					}
					else
					{
						parentHandle.DangerousRelease();
					}
				}
			}
			if (ADP.PtrZero == this.handle)
			{
				throw ODBC.FailedToGetDescriptorHandle(statementAttribute);
			}
		}

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x06001A69 RID: 6761 RVA: 0x00243104 File Offset: 0x00242504
		internal ODBC32.SQL_HANDLE HandleType
		{
			get
			{
				return this._handleType;
			}
		}

		// Token: 0x17000361 RID: 865
		// (get) Token: 0x06001A6A RID: 6762 RVA: 0x00243118 File Offset: 0x00242518
		public override bool IsInvalid
		{
			get
			{
				return IntPtr.Zero == this.handle;
			}
		}

		// Token: 0x06001A6B RID: 6763 RVA: 0x00243138 File Offset: 0x00242538
		protected override bool ReleaseHandle()
		{
			IntPtr handle = this.handle;
			this.handle = IntPtr.Zero;
			if (IntPtr.Zero != handle)
			{
				ODBC32.SQL_HANDLE handleType = this.HandleType;
				switch (handleType)
				{
				case ODBC32.SQL_HANDLE.ENV:
				case ODBC32.SQL_HANDLE.DBC:
				case ODBC32.SQL_HANDLE.STMT:
				{
					ODBC32.RetCode retCode = UnsafeNativeMethods.SQLFreeHandle(handleType, handle);
					Bid.TraceSqlReturn("<odbc.SQLFreeHandle|API|ODBC|RET> %08X{SQLRETURN}\n", retCode);
					break;
				}
				}
			}
			OdbcHandle parentHandle = this._parentHandle;
			this._parentHandle = null;
			if (parentHandle != null)
			{
				parentHandle.DangerousRelease();
			}
			return true;
		}

		// Token: 0x06001A6C RID: 6764 RVA: 0x002431B8 File Offset: 0x002425B8
		internal ODBC32.RetCode GetDiagnosticField(out string sqlState)
		{
			StringBuilder stringBuilder = new StringBuilder(6);
			short num;
			ODBC32.RetCode retCode = UnsafeNativeMethods.SQLGetDiagFieldW(this.HandleType, this, 1, 4, stringBuilder, checked((short)(2 * stringBuilder.Capacity)), out num);
			ODBC.TraceODBC(3, "SQLGetDiagFieldW", retCode);
			if (retCode == ODBC32.RetCode.SUCCESS || retCode == ODBC32.RetCode.SUCCESS_WITH_INFO)
			{
				sqlState = stringBuilder.ToString();
			}
			else
			{
				sqlState = ADP.StrEmpty;
			}
			return retCode;
		}

		// Token: 0x06001A6D RID: 6765 RVA: 0x0024320C File Offset: 0x0024260C
		internal ODBC32.RetCode GetDiagnosticRecord(short record, out string sqlState, StringBuilder message, out int nativeError, out short cchActual)
		{
			StringBuilder stringBuilder = new StringBuilder(5);
			ODBC32.RetCode retCode = UnsafeNativeMethods.SQLGetDiagRecW(this.HandleType, this, record, stringBuilder, out nativeError, message, checked((short)message.Capacity), out cchActual);
			ODBC.TraceODBC(3, "SQLGetDiagRecW", retCode);
			if (retCode == ODBC32.RetCode.SUCCESS || retCode == ODBC32.RetCode.SUCCESS_WITH_INFO)
			{
				sqlState = stringBuilder.ToString();
			}
			else
			{
				sqlState = ADP.StrEmpty;
			}
			return retCode;
		}

		// Token: 0x04000F9B RID: 3995
		private ODBC32.SQL_HANDLE _handleType;

		// Token: 0x04000F9C RID: 3996
		private OdbcHandle _parentHandle;
	}
}
