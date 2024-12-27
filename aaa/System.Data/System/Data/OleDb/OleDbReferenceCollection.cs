using System;
using System.Data.ProviderBase;

namespace System.Data.OleDb
{
	// Token: 0x02000239 RID: 569
	internal sealed class OleDbReferenceCollection : DbReferenceCollection
	{
		// Token: 0x06002053 RID: 8275 RVA: 0x00261FB0 File Offset: 0x002613B0
		public override void Add(object value, int tag)
		{
			base.AddItem(value, tag);
		}

		// Token: 0x06002054 RID: 8276 RVA: 0x00261FC8 File Offset: 0x002613C8
		protected override bool NotifyItem(int message, int tag, object value)
		{
			bool flag = -1 == message;
			if (1 == tag)
			{
				((OleDbCommand)value).CloseCommandFromConnection(flag);
			}
			else if (2 == tag)
			{
				((OleDbDataReader)value).CloseReaderFromConnection(flag);
			}
			return false;
		}

		// Token: 0x06002055 RID: 8277 RVA: 0x00262000 File Offset: 0x00261400
		public override void Remove(object value)
		{
			base.RemoveItem(value);
		}

		// Token: 0x0400146E RID: 5230
		internal const int Closing = 0;

		// Token: 0x0400146F RID: 5231
		internal const int Canceling = -1;

		// Token: 0x04001470 RID: 5232
		internal const int CommandTag = 1;

		// Token: 0x04001471 RID: 5233
		internal const int DataReaderTag = 2;
	}
}
