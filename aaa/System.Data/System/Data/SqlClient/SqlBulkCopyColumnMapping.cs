using System;
using System.Data.Common;

namespace System.Data.SqlClient
{
	// Token: 0x020002B6 RID: 694
	public sealed class SqlBulkCopyColumnMapping
	{
		// Token: 0x1700053A RID: 1338
		// (get) Token: 0x0600233F RID: 9023 RVA: 0x00271824 File Offset: 0x00270C24
		// (set) Token: 0x06002340 RID: 9024 RVA: 0x00271848 File Offset: 0x00270C48
		public string DestinationColumn
		{
			get
			{
				if (this._destinationColumnName != null)
				{
					return this._destinationColumnName;
				}
				return string.Empty;
			}
			set
			{
				this._destinationColumnOrdinal = (this._internalDestinationColumnOrdinal = -1);
				this._destinationColumnName = value;
			}
		}

		// Token: 0x1700053B RID: 1339
		// (get) Token: 0x06002341 RID: 9025 RVA: 0x0027186C File Offset: 0x00270C6C
		// (set) Token: 0x06002342 RID: 9026 RVA: 0x00271880 File Offset: 0x00270C80
		public int DestinationOrdinal
		{
			get
			{
				return this._destinationColumnOrdinal;
			}
			set
			{
				if (value >= 0)
				{
					this._destinationColumnName = null;
					this._internalDestinationColumnOrdinal = value;
					this._destinationColumnOrdinal = value;
					return;
				}
				throw ADP.IndexOutOfRange(value);
			}
		}

		// Token: 0x1700053C RID: 1340
		// (get) Token: 0x06002343 RID: 9027 RVA: 0x002718B0 File Offset: 0x00270CB0
		// (set) Token: 0x06002344 RID: 9028 RVA: 0x002718D4 File Offset: 0x00270CD4
		public string SourceColumn
		{
			get
			{
				if (this._sourceColumnName != null)
				{
					return this._sourceColumnName;
				}
				return string.Empty;
			}
			set
			{
				this._sourceColumnOrdinal = (this._internalSourceColumnOrdinal = -1);
				this._sourceColumnName = value;
			}
		}

		// Token: 0x1700053D RID: 1341
		// (get) Token: 0x06002345 RID: 9029 RVA: 0x002718F8 File Offset: 0x00270CF8
		// (set) Token: 0x06002346 RID: 9030 RVA: 0x0027190C File Offset: 0x00270D0C
		public int SourceOrdinal
		{
			get
			{
				return this._sourceColumnOrdinal;
			}
			set
			{
				if (value >= 0)
				{
					this._sourceColumnName = null;
					this._internalSourceColumnOrdinal = value;
					this._sourceColumnOrdinal = value;
					return;
				}
				throw ADP.IndexOutOfRange(value);
			}
		}

		// Token: 0x06002347 RID: 9031 RVA: 0x0027193C File Offset: 0x00270D3C
		public SqlBulkCopyColumnMapping()
		{
			this._internalSourceColumnOrdinal = -1;
		}

		// Token: 0x06002348 RID: 9032 RVA: 0x00271958 File Offset: 0x00270D58
		public SqlBulkCopyColumnMapping(string sourceColumn, string destinationColumn)
		{
			this.SourceColumn = sourceColumn;
			this.DestinationColumn = destinationColumn;
		}

		// Token: 0x06002349 RID: 9033 RVA: 0x0027197C File Offset: 0x00270D7C
		public SqlBulkCopyColumnMapping(int sourceColumnOrdinal, string destinationColumn)
		{
			this.SourceOrdinal = sourceColumnOrdinal;
			this.DestinationColumn = destinationColumn;
		}

		// Token: 0x0600234A RID: 9034 RVA: 0x002719A0 File Offset: 0x00270DA0
		public SqlBulkCopyColumnMapping(string sourceColumn, int destinationOrdinal)
		{
			this.SourceColumn = sourceColumn;
			this.DestinationOrdinal = destinationOrdinal;
		}

		// Token: 0x0600234B RID: 9035 RVA: 0x002719C4 File Offset: 0x00270DC4
		public SqlBulkCopyColumnMapping(int sourceColumnOrdinal, int destinationOrdinal)
		{
			this.SourceOrdinal = sourceColumnOrdinal;
			this.DestinationOrdinal = destinationOrdinal;
		}

		// Token: 0x040016E4 RID: 5860
		internal string _destinationColumnName;

		// Token: 0x040016E5 RID: 5861
		internal int _destinationColumnOrdinal;

		// Token: 0x040016E6 RID: 5862
		internal string _sourceColumnName;

		// Token: 0x040016E7 RID: 5863
		internal int _sourceColumnOrdinal;

		// Token: 0x040016E8 RID: 5864
		internal int _internalDestinationColumnOrdinal;

		// Token: 0x040016E9 RID: 5865
		internal int _internalSourceColumnOrdinal;
	}
}
