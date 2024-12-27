using System;
using System.Globalization;

namespace System.Xml
{
	// Token: 0x020000F5 RID: 245
	internal struct BinXmlSqlMoney
	{
		// Token: 0x06000EE4 RID: 3812 RVA: 0x00041806 File Offset: 0x00040806
		public BinXmlSqlMoney(int v)
		{
			this.data = (long)v;
		}

		// Token: 0x06000EE5 RID: 3813 RVA: 0x00041810 File Offset: 0x00040810
		public BinXmlSqlMoney(long v)
		{
			this.data = v;
		}

		// Token: 0x06000EE6 RID: 3814 RVA: 0x0004181C File Offset: 0x0004081C
		public decimal ToDecimal()
		{
			bool flag;
			ulong num;
			if (this.data < 0L)
			{
				flag = true;
				num = (ulong)(-(ulong)this.data);
			}
			else
			{
				flag = false;
				num = (ulong)this.data;
			}
			return new decimal((int)num, (int)(num >> 32), 0, flag, 4);
		}

		// Token: 0x06000EE7 RID: 3815 RVA: 0x00041858 File Offset: 0x00040858
		public override string ToString()
		{
			return this.ToDecimal().ToString("#0.00##", CultureInfo.InvariantCulture);
		}

		// Token: 0x04000A07 RID: 2567
		private long data;
	}
}
