using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000508 RID: 1288
	[ComVisible(true)]
	[Serializable]
	public sealed class CurrencyWrapper
	{
		// Token: 0x06003287 RID: 12935 RVA: 0x000AB70B File Offset: 0x000AA70B
		public CurrencyWrapper(decimal obj)
		{
			this.m_WrappedObject = obj;
		}

		// Token: 0x06003288 RID: 12936 RVA: 0x000AB71A File Offset: 0x000AA71A
		public CurrencyWrapper(object obj)
		{
			if (!(obj is decimal))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeDecimal"), "obj");
			}
			this.m_WrappedObject = (decimal)obj;
		}

		// Token: 0x170008DE RID: 2270
		// (get) Token: 0x06003289 RID: 12937 RVA: 0x000AB74B File Offset: 0x000AA74B
		public decimal WrappedObject
		{
			get
			{
				return this.m_WrappedObject;
			}
		}

		// Token: 0x040019A9 RID: 6569
		private decimal m_WrappedObject;
	}
}
