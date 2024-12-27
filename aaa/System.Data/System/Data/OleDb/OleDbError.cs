using System;
using System.Data.Common;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Data.OleDb
{
	// Token: 0x02000225 RID: 549
	[Serializable]
	public sealed class OleDbError
	{
		// Token: 0x06001F8E RID: 8078 RVA: 0x0025E204 File Offset: 0x0025D604
		internal OleDbError(UnsafeNativeMethods.IErrorRecords errorRecords, int index)
		{
			int num = CultureInfo.CurrentCulture.LCID;
			Bid.Trace("<oledb.IErrorRecords.GetErrorInfo|API|OS>\n");
			UnsafeNativeMethods.IErrorInfo errorInfo = errorRecords.GetErrorInfo(index, num);
			OleDbHResult oleDbHResult;
			if (errorInfo != null)
			{
				Bid.Trace("<oledb.IErrorInfo.GetDescription|API|OS>\n");
				oleDbHResult = errorInfo.GetDescription(out this.message);
				Bid.Trace("<oledb.IErrorInfo.GetDescription|API|OS|RET> Message='%ls'\n", this.message);
				if (OleDbHResult.DB_E_NOLOCALE == oleDbHResult)
				{
					Bid.Trace("<oledb.ReleaseComObject|API|OS> ErrorInfo\n");
					Marshal.ReleaseComObject(errorInfo);
					Bid.Trace("<oledb.Kernel32.GetUserDefaultLCID|API|OS>\n");
					num = SafeNativeMethods.GetUserDefaultLCID();
					Bid.Trace("<oledb.IErrorRecords.GetErrorInfo|API|OS> LCID=%d\n", num);
					errorInfo = errorRecords.GetErrorInfo(index, num);
					if (errorInfo != null)
					{
						Bid.Trace("<oledb.IErrorInfo.GetDescription|API|OS>\n");
						oleDbHResult = errorInfo.GetDescription(out this.message);
						Bid.Trace("<oledb.IErrorInfo.GetDescription|API|OS|RET> Message='%ls'\n", this.message);
					}
				}
				if (oleDbHResult < OleDbHResult.S_OK && ADP.IsEmpty(this.message))
				{
					this.message = ODB.FailedGetDescription(oleDbHResult);
				}
				if (errorInfo != null)
				{
					Bid.Trace("<oledb.IErrorInfo.GetSource|API|OS>\n");
					oleDbHResult = errorInfo.GetSource(out this.source);
					Bid.Trace("<oledb.IErrorInfo.GetSource|API|OS|RET> Source='%ls'\n", this.source);
					if (OleDbHResult.DB_E_NOLOCALE == oleDbHResult)
					{
						Marshal.ReleaseComObject(errorInfo);
						Bid.Trace("<oledb.Kernel32.GetUserDefaultLCID|API|OS>\n");
						num = SafeNativeMethods.GetUserDefaultLCID();
						Bid.Trace("<oledb.IErrorRecords.GetErrorInfo|API|OS> LCID=%d\n", num);
						errorInfo = errorRecords.GetErrorInfo(index, num);
						if (errorInfo != null)
						{
							Bid.Trace("<oledb.IErrorInfo.GetSource|API|OS>\n");
							oleDbHResult = errorInfo.GetSource(out this.source);
							Bid.Trace("<oledb.IErrorInfo.GetSource|API|OS|RET> Source='%ls'\n", this.source);
						}
					}
					if (oleDbHResult < OleDbHResult.S_OK && ADP.IsEmpty(this.source))
					{
						this.source = ODB.FailedGetSource(oleDbHResult);
					}
					Bid.Trace("<oledb.Marshal.ReleaseComObject|API|OS> ErrorInfo\n");
					Marshal.ReleaseComObject(errorInfo);
				}
			}
			Bid.Trace("<oledb.IErrorRecords.GetCustomErrorObject|API|OS> IID_ISQLErrorInfo\n");
			UnsafeNativeMethods.ISQLErrorInfo isqlerrorInfo;
			oleDbHResult = errorRecords.GetCustomErrorObject(index, ref ODB.IID_ISQLErrorInfo, out isqlerrorInfo);
			if (isqlerrorInfo != null)
			{
				Bid.Trace("<oledb.ISQLErrorInfo.GetSQLInfo|API|OS>\n");
				this.nativeError = isqlerrorInfo.GetSQLInfo(out this.sqlState);
				Bid.Trace("<oledb.ReleaseComObject|API|OS> SQLErrorInfo\n");
				Marshal.ReleaseComObject(isqlerrorInfo);
			}
		}

		// Token: 0x17000449 RID: 1097
		// (get) Token: 0x06001F8F RID: 8079 RVA: 0x0025E3E8 File Offset: 0x0025D7E8
		public string Message
		{
			get
			{
				string text = this.message;
				if (text == null)
				{
					return ADP.StrEmpty;
				}
				return text;
			}
		}

		// Token: 0x1700044A RID: 1098
		// (get) Token: 0x06001F90 RID: 8080 RVA: 0x0025E408 File Offset: 0x0025D808
		public int NativeError
		{
			get
			{
				return this.nativeError;
			}
		}

		// Token: 0x1700044B RID: 1099
		// (get) Token: 0x06001F91 RID: 8081 RVA: 0x0025E41C File Offset: 0x0025D81C
		public string Source
		{
			get
			{
				string text = this.source;
				if (text == null)
				{
					return ADP.StrEmpty;
				}
				return text;
			}
		}

		// Token: 0x1700044C RID: 1100
		// (get) Token: 0x06001F92 RID: 8082 RVA: 0x0025E43C File Offset: 0x0025D83C
		public string SQLState
		{
			get
			{
				string text = this.sqlState;
				if (text == null)
				{
					return ADP.StrEmpty;
				}
				return text;
			}
		}

		// Token: 0x06001F93 RID: 8083 RVA: 0x0025E45C File Offset: 0x0025D85C
		public override string ToString()
		{
			return this.Message;
		}

		// Token: 0x040012E1 RID: 4833
		private readonly string message;

		// Token: 0x040012E2 RID: 4834
		private readonly string source;

		// Token: 0x040012E3 RID: 4835
		private readonly string sqlState;

		// Token: 0x040012E4 RID: 4836
		private readonly int nativeError;
	}
}
