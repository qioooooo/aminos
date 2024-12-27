using System;
using System.Data.Common;

namespace System.Data.SqlClient
{
	// Token: 0x0200032E RID: 814
	internal struct MultiPartTableName
	{
		// Token: 0x06002A80 RID: 10880 RVA: 0x0029CF50 File Offset: 0x0029C350
		internal MultiPartTableName(string[] parts)
		{
			this._multipartName = null;
			this._serverName = parts[0];
			this._catalogName = parts[1];
			this._schemaName = parts[2];
			this._tableName = parts[3];
		}

		// Token: 0x06002A81 RID: 10881 RVA: 0x0029CF88 File Offset: 0x0029C388
		internal MultiPartTableName(string multipartName)
		{
			this._multipartName = multipartName;
			this._serverName = null;
			this._catalogName = null;
			this._schemaName = null;
			this._tableName = null;
		}

		// Token: 0x170006EF RID: 1775
		// (get) Token: 0x06002A82 RID: 10882 RVA: 0x0029CFB8 File Offset: 0x0029C3B8
		// (set) Token: 0x06002A83 RID: 10883 RVA: 0x0029CFD4 File Offset: 0x0029C3D4
		internal string ServerName
		{
			get
			{
				this.ParseMultipartName();
				return this._serverName;
			}
			set
			{
				this._serverName = value;
			}
		}

		// Token: 0x170006F0 RID: 1776
		// (get) Token: 0x06002A84 RID: 10884 RVA: 0x0029CFE8 File Offset: 0x0029C3E8
		// (set) Token: 0x06002A85 RID: 10885 RVA: 0x0029D004 File Offset: 0x0029C404
		internal string CatalogName
		{
			get
			{
				this.ParseMultipartName();
				return this._catalogName;
			}
			set
			{
				this._catalogName = value;
			}
		}

		// Token: 0x170006F1 RID: 1777
		// (get) Token: 0x06002A86 RID: 10886 RVA: 0x0029D018 File Offset: 0x0029C418
		// (set) Token: 0x06002A87 RID: 10887 RVA: 0x0029D034 File Offset: 0x0029C434
		internal string SchemaName
		{
			get
			{
				this.ParseMultipartName();
				return this._schemaName;
			}
			set
			{
				this._schemaName = value;
			}
		}

		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x06002A88 RID: 10888 RVA: 0x0029D048 File Offset: 0x0029C448
		// (set) Token: 0x06002A89 RID: 10889 RVA: 0x0029D064 File Offset: 0x0029C464
		internal string TableName
		{
			get
			{
				this.ParseMultipartName();
				return this._tableName;
			}
			set
			{
				this._tableName = value;
			}
		}

		// Token: 0x06002A8A RID: 10890 RVA: 0x0029D078 File Offset: 0x0029C478
		private void ParseMultipartName()
		{
			if (this._multipartName != null)
			{
				string[] array = MultipartIdentifier.ParseMultipartIdentifier(this._multipartName, "[\"", "]\"", "SQL_TDSParserTableName", false);
				this._serverName = array[0];
				this._catalogName = array[1];
				this._schemaName = array[2];
				this._tableName = array[3];
				this._multipartName = null;
			}
		}

		// Token: 0x06002A8B RID: 10891 RVA: 0x0029D0D4 File Offset: 0x0029C4D4
		// Note: this type is marked as 'beforefieldinit'.
		static MultiPartTableName()
		{
			string[] array = new string[4];
			MultiPartTableName.Null = new MultiPartTableName(array);
		}

		// Token: 0x04001BEB RID: 7147
		private string _multipartName;

		// Token: 0x04001BEC RID: 7148
		private string _serverName;

		// Token: 0x04001BED RID: 7149
		private string _catalogName;

		// Token: 0x04001BEE RID: 7150
		private string _schemaName;

		// Token: 0x04001BEF RID: 7151
		private string _tableName;

		// Token: 0x04001BF0 RID: 7152
		internal static readonly MultiPartTableName Null;
	}
}
