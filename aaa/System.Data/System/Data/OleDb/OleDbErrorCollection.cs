using System;
using System.Collections;
using System.ComponentModel;
using System.Data.Common;

namespace System.Data.OleDb
{
	// Token: 0x02000226 RID: 550
	[ListBindable(false)]
	[Serializable]
	public sealed class OleDbErrorCollection : ICollection, IEnumerable
	{
		// Token: 0x06001F94 RID: 8084 RVA: 0x0025E470 File Offset: 0x0025D870
		internal OleDbErrorCollection(UnsafeNativeMethods.IErrorInfo errorInfo)
		{
			ArrayList arrayList = new ArrayList();
			Bid.Trace("<oledb.IUnknown.QueryInterface|API|OS> IErrorRecords\n");
			UnsafeNativeMethods.IErrorRecords errorRecords = errorInfo as UnsafeNativeMethods.IErrorRecords;
			if (errorRecords != null)
			{
				int recordCount = errorRecords.GetRecordCount();
				Bid.Trace("<oledb.IErrorRecords.GetRecordCount|API|OS|RET> RecordCount=%d\n", recordCount);
				for (int i = 0; i < recordCount; i++)
				{
					OleDbError oleDbError = new OleDbError(errorRecords, i);
					arrayList.Add(oleDbError);
				}
			}
			this.items = arrayList;
		}

		// Token: 0x1700044D RID: 1101
		// (get) Token: 0x06001F95 RID: 8085 RVA: 0x0025E4D4 File Offset: 0x0025D8D4
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700044E RID: 1102
		// (get) Token: 0x06001F96 RID: 8086 RVA: 0x0025E4E4 File Offset: 0x0025D8E4
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x1700044F RID: 1103
		// (get) Token: 0x06001F97 RID: 8087 RVA: 0x0025E4F4 File Offset: 0x0025D8F4
		public int Count
		{
			get
			{
				ArrayList arrayList = this.items;
				if (arrayList == null)
				{
					return 0;
				}
				return arrayList.Count;
			}
		}

		// Token: 0x17000450 RID: 1104
		public OleDbError this[int index]
		{
			get
			{
				return this.items[index] as OleDbError;
			}
		}

		// Token: 0x06001F99 RID: 8089 RVA: 0x0025E534 File Offset: 0x0025D934
		internal void AddRange(ICollection c)
		{
			this.items.AddRange(c);
		}

		// Token: 0x06001F9A RID: 8090 RVA: 0x0025E550 File Offset: 0x0025D950
		public void CopyTo(Array array, int index)
		{
			this.items.CopyTo(array, index);
		}

		// Token: 0x06001F9B RID: 8091 RVA: 0x0025E56C File Offset: 0x0025D96C
		public void CopyTo(OleDbError[] array, int index)
		{
			this.items.CopyTo(array, index);
		}

		// Token: 0x06001F9C RID: 8092 RVA: 0x0025E588 File Offset: 0x0025D988
		public IEnumerator GetEnumerator()
		{
			return this.items.GetEnumerator();
		}

		// Token: 0x040012E5 RID: 4837
		private readonly ArrayList items;
	}
}
