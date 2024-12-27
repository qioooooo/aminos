using System;
using System.Data.ProviderBase;

namespace System.Data.Odbc
{
	// Token: 0x02000200 RID: 512
	internal sealed class OdbcReferenceCollection : DbReferenceCollection
	{
		// Token: 0x06001C7B RID: 7291 RVA: 0x0024D50C File Offset: 0x0024C90C
		public override void Add(object value, int tag)
		{
			base.AddItem(value, tag);
		}

		// Token: 0x06001C7C RID: 7292 RVA: 0x0024D524 File Offset: 0x0024C924
		protected override bool NotifyItem(int message, int tag, object value)
		{
			switch (message)
			{
			case 0:
				if (1 == tag)
				{
					((OdbcCommand)value).CloseFromConnection();
				}
				break;
			case 1:
				if (1 == tag)
				{
					((OdbcCommand)value).RecoverFromConnection();
				}
				break;
			}
			return false;
		}

		// Token: 0x06001C7D RID: 7293 RVA: 0x0024D568 File Offset: 0x0024C968
		public override void Remove(object value)
		{
			base.RemoveItem(value);
		}

		// Token: 0x04001065 RID: 4197
		internal const int Closing = 0;

		// Token: 0x04001066 RID: 4198
		internal const int Recover = 1;

		// Token: 0x04001067 RID: 4199
		internal const int CommandTag = 1;
	}
}
