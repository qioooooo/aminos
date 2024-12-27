using System;
using System.Data.Common;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;

namespace System.Data.Odbc
{
	// Token: 0x020001ED RID: 493
	[Serializable]
	public sealed class OdbcException : DbException
	{
		// Token: 0x06001B92 RID: 7058 RVA: 0x00248444 File Offset: 0x00247844
		internal static OdbcException CreateException(OdbcErrorCollection errors, ODBC32.RetCode retcode)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (object obj in errors)
			{
				OdbcError odbcError = (OdbcError)obj;
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(Environment.NewLine);
				}
				stringBuilder.Append(Res.GetString("Odbc_ExceptionMessage", new object[]
				{
					ODBC32.RetcodeToString(retcode),
					odbcError.SQLState,
					odbcError.Message
				}));
			}
			return new OdbcException(stringBuilder.ToString(), errors);
		}

		// Token: 0x06001B93 RID: 7059 RVA: 0x00248500 File Offset: 0x00247900
		internal OdbcException(string message, OdbcErrorCollection errors)
			: base(message)
		{
			this.odbcErrors = errors;
			base.HResult = -2146232009;
		}

		// Token: 0x06001B94 RID: 7060 RVA: 0x00248534 File Offset: 0x00247934
		private OdbcException(SerializationInfo si, StreamingContext sc)
			: base(si, sc)
		{
			this._retcode = (ODBC32.RETCODE)si.GetValue("odbcRetcode", typeof(ODBC32.RETCODE));
			this.odbcErrors = (OdbcErrorCollection)si.GetValue("odbcErrors", typeof(OdbcErrorCollection));
			base.HResult = -2146232009;
		}

		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x06001B95 RID: 7061 RVA: 0x002485A0 File Offset: 0x002479A0
		public OdbcErrorCollection Errors
		{
			get
			{
				return this.odbcErrors;
			}
		}

		// Token: 0x06001B96 RID: 7062 RVA: 0x002485B4 File Offset: 0x002479B4
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo si, StreamingContext context)
		{
			if (si == null)
			{
				throw new ArgumentNullException("si");
			}
			si.AddValue("odbcRetcode", this._retcode, typeof(ODBC32.RETCODE));
			si.AddValue("odbcErrors", this.odbcErrors, typeof(OdbcErrorCollection));
			base.GetObjectData(si, context);
		}

		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x06001B97 RID: 7063 RVA: 0x00248614 File Offset: 0x00247A14
		public override string Source
		{
			get
			{
				if (0 >= this.Errors.Count)
				{
					return "";
				}
				string source = this.Errors[0].Source;
				if (!ADP.IsEmpty(source))
				{
					return source;
				}
				return "";
			}
		}

		// Token: 0x0400100C RID: 4108
		private OdbcErrorCollection odbcErrors = new OdbcErrorCollection();

		// Token: 0x0400100D RID: 4109
		private ODBC32.RETCODE _retcode;
	}
}
