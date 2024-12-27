using System;
using System.Data.ProviderBase;

namespace System.Data.OleDb
{
	// Token: 0x02000217 RID: 535
	internal sealed class OleDbConnectionPoolGroupProviderInfo : DbConnectionPoolGroupProviderInfo
	{
		// Token: 0x06001EB4 RID: 7860 RVA: 0x00258968 File Offset: 0x00257D68
		internal OleDbConnectionPoolGroupProviderInfo()
		{
		}

		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x06001EB5 RID: 7861 RVA: 0x0025897C File Offset: 0x00257D7C
		internal bool HasQuoteFix
		{
			get
			{
				return this._hasQuoteFix;
			}
		}

		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x06001EB6 RID: 7862 RVA: 0x00258990 File Offset: 0x00257D90
		internal string QuotePrefix
		{
			get
			{
				return this._quotePrefix;
			}
		}

		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x06001EB7 RID: 7863 RVA: 0x002589A4 File Offset: 0x00257DA4
		internal string QuoteSuffix
		{
			get
			{
				return this._quoteSuffix;
			}
		}

		// Token: 0x06001EB8 RID: 7864 RVA: 0x002589B8 File Offset: 0x00257DB8
		internal void SetQuoteFix(string prefix, string suffix)
		{
			this._quotePrefix = prefix;
			this._quoteSuffix = suffix;
			this._hasQuoteFix = true;
		}

		// Token: 0x0400126F RID: 4719
		private bool _hasQuoteFix;

		// Token: 0x04001270 RID: 4720
		private string _quotePrefix;

		// Token: 0x04001271 RID: 4721
		private string _quoteSuffix;
	}
}
