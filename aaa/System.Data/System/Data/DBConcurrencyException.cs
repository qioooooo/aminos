using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Data
{
	// Token: 0x020000B1 RID: 177
	[Serializable]
	public sealed class DBConcurrencyException : SystemException
	{
		// Token: 0x06000C18 RID: 3096 RVA: 0x001F98F0 File Offset: 0x001F8CF0
		public DBConcurrencyException()
			: this(Res.GetString("ADP_DBConcurrencyExceptionMessage"), null)
		{
		}

		// Token: 0x06000C19 RID: 3097 RVA: 0x001F9910 File Offset: 0x001F8D10
		public DBConcurrencyException(string message)
			: this(message, null)
		{
		}

		// Token: 0x06000C1A RID: 3098 RVA: 0x001F9928 File Offset: 0x001F8D28
		public DBConcurrencyException(string message, Exception inner)
			: base(message, inner)
		{
			base.HResult = -2146232011;
		}

		// Token: 0x06000C1B RID: 3099 RVA: 0x001F9948 File Offset: 0x001F8D48
		public DBConcurrencyException(string message, Exception inner, DataRow[] dataRows)
			: base(message, inner)
		{
			base.HResult = -2146232011;
			this._dataRows = dataRows;
		}

		// Token: 0x06000C1C RID: 3100 RVA: 0x001F9970 File Offset: 0x001F8D70
		private DBConcurrencyException(SerializationInfo si, StreamingContext sc)
			: base(si, sc)
		{
		}

		// Token: 0x06000C1D RID: 3101 RVA: 0x001F9988 File Offset: 0x001F8D88
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo si, StreamingContext context)
		{
			if (si == null)
			{
				throw new ArgumentNullException("si");
			}
			base.GetObjectData(si, context);
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06000C1E RID: 3102 RVA: 0x001F99AC File Offset: 0x001F8DAC
		// (set) Token: 0x06000C1F RID: 3103 RVA: 0x001F99D0 File Offset: 0x001F8DD0
		public DataRow Row
		{
			get
			{
				DataRow[] dataRows = this._dataRows;
				if (dataRows == null || 0 >= dataRows.Length)
				{
					return null;
				}
				return dataRows[0];
			}
			set
			{
				this._dataRows = new DataRow[] { value };
			}
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06000C20 RID: 3104 RVA: 0x001F99F0 File Offset: 0x001F8DF0
		public int RowCount
		{
			get
			{
				DataRow[] dataRows = this._dataRows;
				if (dataRows == null)
				{
					return 0;
				}
				return dataRows.Length;
			}
		}

		// Token: 0x06000C21 RID: 3105 RVA: 0x001F9A0C File Offset: 0x001F8E0C
		public void CopyToRows(DataRow[] array)
		{
			this.CopyToRows(array, 0);
		}

		// Token: 0x06000C22 RID: 3106 RVA: 0x001F9A24 File Offset: 0x001F8E24
		public void CopyToRows(DataRow[] array, int arrayIndex)
		{
			DataRow[] dataRows = this._dataRows;
			if (dataRows != null)
			{
				dataRows.CopyTo(array, arrayIndex);
			}
		}

		// Token: 0x0400086B RID: 2155
		private DataRow[] _dataRows;
	}
}
