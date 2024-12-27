using System;
using System.ComponentModel;

namespace System.Management
{
	// Token: 0x02000037 RID: 55
	[TypeConverter(typeof(ManagementQueryConverter))]
	public abstract class ManagementQuery : ICloneable
	{
		// Token: 0x1400000B RID: 11
		// (add) Token: 0x060001D5 RID: 469 RVA: 0x00009CF8 File Offset: 0x00008CF8
		// (remove) Token: 0x060001D6 RID: 470 RVA: 0x00009D11 File Offset: 0x00008D11
		internal event IdentifierChangedEventHandler IdentifierChanged;

		// Token: 0x060001D7 RID: 471 RVA: 0x00009D2A File Offset: 0x00008D2A
		internal void FireIdentifierChanged()
		{
			if (this.IdentifierChanged != null)
			{
				this.IdentifierChanged(this, null);
			}
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x00009D41 File Offset: 0x00008D41
		internal void SetQueryString(string qString)
		{
			this.queryString = qString;
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x00009D4A File Offset: 0x00008D4A
		internal ManagementQuery()
			: this("WQL", null)
		{
		}

		// Token: 0x060001DA RID: 474 RVA: 0x00009D58 File Offset: 0x00008D58
		internal ManagementQuery(string query)
			: this("WQL", query)
		{
		}

		// Token: 0x060001DB RID: 475 RVA: 0x00009D66 File Offset: 0x00008D66
		internal ManagementQuery(string language, string query)
		{
			this.QueryLanguage = language;
			this.QueryString = query;
		}

		// Token: 0x060001DC RID: 476 RVA: 0x00009D7C File Offset: 0x00008D7C
		protected internal virtual void ParseQuery(string query)
		{
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060001DD RID: 477 RVA: 0x00009D7E File Offset: 0x00008D7E
		// (set) Token: 0x060001DE RID: 478 RVA: 0x00009D94 File Offset: 0x00008D94
		public virtual string QueryString
		{
			get
			{
				if (this.queryString == null)
				{
					return string.Empty;
				}
				return this.queryString;
			}
			set
			{
				if (this.queryString != value)
				{
					this.ParseQuery(value);
					this.queryString = value;
					this.FireIdentifierChanged();
				}
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060001DF RID: 479 RVA: 0x00009DB8 File Offset: 0x00008DB8
		// (set) Token: 0x060001E0 RID: 480 RVA: 0x00009DCE File Offset: 0x00008DCE
		public virtual string QueryLanguage
		{
			get
			{
				if (this.queryLanguage == null)
				{
					return string.Empty;
				}
				return this.queryLanguage;
			}
			set
			{
				if (this.queryLanguage != value)
				{
					this.queryLanguage = value;
					this.FireIdentifierChanged();
				}
			}
		}

		// Token: 0x060001E1 RID: 481
		public abstract object Clone();

		// Token: 0x060001E2 RID: 482 RVA: 0x00009DEC File Offset: 0x00008DEC
		internal static void ParseToken(ref string q, string token, string op, ref bool bTokenFound, ref string tokenValue)
		{
			if (bTokenFound)
			{
				throw new ArgumentException(RC.GetString("INVALID_QUERY_DUP_TOKEN"));
			}
			bTokenFound = true;
			q = q.Remove(0, token.Length).TrimStart(null);
			if (op != null)
			{
				if (q.IndexOf(op, StringComparison.Ordinal) != 0)
				{
					throw new ArgumentException(RC.GetString("INVALID_QUERY"));
				}
				q = q.Remove(0, op.Length).TrimStart(null);
			}
			if (q.Length == 0)
			{
				throw new ArgumentException(RC.GetString("INVALID_QUERY_NULL_TOKEN"));
			}
			int num;
			if (-1 == (num = q.IndexOf(' ')))
			{
				num = q.Length;
			}
			tokenValue = q.Substring(0, num);
			q = q.Remove(0, tokenValue.Length).TrimStart(null);
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x00009EAD File Offset: 0x00008EAD
		internal static void ParseToken(ref string q, string token, ref bool bTokenFound)
		{
			if (bTokenFound)
			{
				throw new ArgumentException(RC.GetString("INVALID_QUERY_DUP_TOKEN"));
			}
			bTokenFound = true;
			q = q.Remove(0, token.Length).TrimStart(null);
		}

		// Token: 0x0400014F RID: 335
		internal const string DEFAULTQUERYLANGUAGE = "WQL";

		// Token: 0x04000150 RID: 336
		internal static readonly string tokenSelect = "select ";

		// Token: 0x04000152 RID: 338
		private string queryLanguage;

		// Token: 0x04000153 RID: 339
		private string queryString;
	}
}
