using System;
using System.Runtime.CompilerServices;

namespace System
{
	// Token: 0x0200009C RID: 156
	[Serializable]
	internal struct Currency
	{
		// Token: 0x06000946 RID: 2374 RVA: 0x0001C4A0 File Offset: 0x0001B4A0
		public Currency(decimal value)
		{
			this.m_value = decimal.ToCurrency(value).m_value;
		}

		// Token: 0x06000947 RID: 2375 RVA: 0x0001C4B3 File Offset: 0x0001B4B3
		internal Currency(long value, int ignored)
		{
			this.m_value = value;
		}

		// Token: 0x06000948 RID: 2376 RVA: 0x0001C4BC File Offset: 0x0001B4BC
		public static Currency FromOACurrency(long cy)
		{
			return new Currency(cy, 0);
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x0001C4C5 File Offset: 0x0001B4C5
		public long ToOACurrency()
		{
			return this.m_value;
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x0001C4D0 File Offset: 0x0001B4D0
		public static decimal ToDecimal(Currency c)
		{
			decimal num = 0m;
			Currency.FCallToDecimal(ref num, c);
			return num;
		}

		// Token: 0x0600094B RID: 2379
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void FCallToDecimal(ref decimal result, Currency c);

		// Token: 0x0400036A RID: 874
		internal long m_value;
	}
}
