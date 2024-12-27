using System;
using System.Data.ProviderBase;

namespace System.Data.SqlClient
{
	// Token: 0x02000308 RID: 776
	internal sealed class SqlReferenceCollection : DbReferenceCollection
	{
		// Token: 0x170006B5 RID: 1717
		// (get) Token: 0x060028BC RID: 10428 RVA: 0x00290A44 File Offset: 0x0028FE44
		internal bool MayHaveDataReader
		{
			get
			{
				return 0 != this._dataReaderCount;
			}
		}

		// Token: 0x060028BD RID: 10429 RVA: 0x00290A60 File Offset: 0x0028FE60
		public override void Add(object value, int tag)
		{
			this._dataReaderCount++;
			base.AddItem(value, tag);
		}

		// Token: 0x060028BE RID: 10430 RVA: 0x00290A84 File Offset: 0x0028FE84
		internal void Deactivate()
		{
			if (this.MayHaveDataReader)
			{
				base.Notify(0);
				this._dataReaderCount = 0;
			}
			base.Purge();
		}

		// Token: 0x060028BF RID: 10431 RVA: 0x00290AB0 File Offset: 0x0028FEB0
		internal SqlDataReader FindLiveReader(SqlCommand command)
		{
			if (this.MayHaveDataReader)
			{
				foreach (object obj in base.Filter(1))
				{
					SqlDataReader sqlDataReader = (SqlDataReader)obj;
					if (sqlDataReader != null && !sqlDataReader.IsClosed && (command == null || command == sqlDataReader.Command))
					{
						return sqlDataReader;
					}
				}
			}
			return null;
		}

		// Token: 0x060028C0 RID: 10432 RVA: 0x00290B38 File Offset: 0x0028FF38
		protected override bool NotifyItem(int message, int tag, object value)
		{
			SqlDataReader sqlDataReader = (SqlDataReader)value;
			if (!sqlDataReader.IsClosed)
			{
				sqlDataReader.CloseReaderFromConnection();
			}
			return false;
		}

		// Token: 0x060028C1 RID: 10433 RVA: 0x00290B5C File Offset: 0x0028FF5C
		public override void Remove(object value)
		{
			this._dataReaderCount--;
			base.RemoveItem(value);
		}

		// Token: 0x0400196D RID: 6509
		internal const int DataReaderTag = 1;

		// Token: 0x0400196E RID: 6510
		private int _dataReaderCount;
	}
}
