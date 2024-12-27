using System;
using System.Collections;
using System.ComponentModel;
using System.Data.Common;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;

namespace System.Data.SqlClient
{
	// Token: 0x020002F4 RID: 756
	[Serializable]
	public sealed class SqlException : DbException
	{
		// Token: 0x0600273C RID: 10044 RVA: 0x00289B50 File Offset: 0x00288F50
		private SqlException(string message, SqlErrorCollection errorCollection)
			: base(message)
		{
			base.HResult = -2146232060;
			this._errors = errorCollection;
		}

		// Token: 0x0600273D RID: 10045 RVA: 0x00289B78 File Offset: 0x00288F78
		private SqlException(SerializationInfo si, StreamingContext sc)
			: base(si, sc)
		{
			this._errors = (SqlErrorCollection)si.GetValue("Errors", typeof(SqlErrorCollection));
			base.HResult = -2146232060;
		}

		// Token: 0x0600273E RID: 10046 RVA: 0x00289BB8 File Offset: 0x00288FB8
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo si, StreamingContext context)
		{
			if (si == null)
			{
				throw new ArgumentNullException("si");
			}
			si.AddValue("Errors", this._errors, typeof(SqlErrorCollection));
			base.GetObjectData(si, context);
		}

		// Token: 0x1700063C RID: 1596
		// (get) Token: 0x0600273F RID: 10047 RVA: 0x00289BF8 File Offset: 0x00288FF8
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public SqlErrorCollection Errors
		{
			get
			{
				if (this._errors == null)
				{
					this._errors = new SqlErrorCollection();
				}
				return this._errors;
			}
		}

		// Token: 0x06002740 RID: 10048 RVA: 0x00289C20 File Offset: 0x00289020
		private bool ShouldSerializeErrors()
		{
			return this._errors != null && 0 < this._errors.Count;
		}

		// Token: 0x1700063D RID: 1597
		// (get) Token: 0x06002741 RID: 10049 RVA: 0x00289C48 File Offset: 0x00289048
		public byte Class
		{
			get
			{
				return this.Errors[0].Class;
			}
		}

		// Token: 0x1700063E RID: 1598
		// (get) Token: 0x06002742 RID: 10050 RVA: 0x00289C68 File Offset: 0x00289068
		public int LineNumber
		{
			get
			{
				return this.Errors[0].LineNumber;
			}
		}

		// Token: 0x1700063F RID: 1599
		// (get) Token: 0x06002743 RID: 10051 RVA: 0x00289C88 File Offset: 0x00289088
		public int Number
		{
			get
			{
				return this.Errors[0].Number;
			}
		}

		// Token: 0x17000640 RID: 1600
		// (get) Token: 0x06002744 RID: 10052 RVA: 0x00289CA8 File Offset: 0x002890A8
		public string Procedure
		{
			get
			{
				return this.Errors[0].Procedure;
			}
		}

		// Token: 0x17000641 RID: 1601
		// (get) Token: 0x06002745 RID: 10053 RVA: 0x00289CC8 File Offset: 0x002890C8
		public string Server
		{
			get
			{
				return this.Errors[0].Server;
			}
		}

		// Token: 0x17000642 RID: 1602
		// (get) Token: 0x06002746 RID: 10054 RVA: 0x00289CE8 File Offset: 0x002890E8
		public byte State
		{
			get
			{
				return this.Errors[0].State;
			}
		}

		// Token: 0x17000643 RID: 1603
		// (get) Token: 0x06002747 RID: 10055 RVA: 0x00289D08 File Offset: 0x00289108
		public override string Source
		{
			get
			{
				return this.Errors[0].Source;
			}
		}

		// Token: 0x06002748 RID: 10056 RVA: 0x00289D28 File Offset: 0x00289128
		internal static SqlException CreateException(SqlErrorCollection errorCollection, string serverVersion)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < errorCollection.Count; i++)
			{
				if (i > 0)
				{
					stringBuilder.Append(Environment.NewLine);
				}
				stringBuilder.Append(errorCollection[i].Message);
			}
			SqlException ex = new SqlException(stringBuilder.ToString(), errorCollection);
			ex.Data.Add("HelpLink.ProdName", "Microsoft SQL Server");
			if (!ADP.IsEmpty(serverVersion))
			{
				ex.Data.Add("HelpLink.ProdVer", serverVersion);
			}
			ex.Data.Add("HelpLink.EvtSrc", "MSSQLServer");
			ex.Data.Add("HelpLink.EvtID", errorCollection[0].Number.ToString(CultureInfo.InvariantCulture));
			ex.Data.Add("HelpLink.BaseHelpUrl", "http://go.microsoft.com/fwlink");
			ex.Data.Add("HelpLink.LinkId", "20476");
			return ex;
		}

		// Token: 0x06002749 RID: 10057 RVA: 0x00289E14 File Offset: 0x00289214
		internal SqlException InternalClone()
		{
			SqlException ex = new SqlException(this.Message, this._errors);
			if (this.Data != null)
			{
				foreach (object obj in this.Data)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					ex.Data.Add(dictionaryEntry.Key, dictionaryEntry.Value);
				}
			}
			ex._doNotReconnect = this._doNotReconnect;
			return ex;
		}

		// Token: 0x040018E2 RID: 6370
		private SqlErrorCollection _errors;

		// Token: 0x040018E3 RID: 6371
		internal bool _doNotReconnect;
	}
}
