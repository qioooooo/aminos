using System;
using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
	// Token: 0x020005CD RID: 1485
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter, Inherited = false)]
	[ComVisible(true)]
	[Serializable]
	public sealed class DecimalConstantAttribute : Attribute
	{
		// Token: 0x0600378E RID: 14222 RVA: 0x000BB7F9 File Offset: 0x000BA7F9
		[CLSCompliant(false)]
		public DecimalConstantAttribute(byte scale, byte sign, uint hi, uint mid, uint low)
		{
			this.dec = new decimal((int)low, (int)mid, (int)hi, sign != 0, scale);
		}

		// Token: 0x0600378F RID: 14223 RVA: 0x000BB819 File Offset: 0x000BA819
		public DecimalConstantAttribute(byte scale, byte sign, int hi, int mid, int low)
		{
			this.dec = new decimal(low, mid, hi, sign != 0, scale);
		}

		// Token: 0x17000966 RID: 2406
		// (get) Token: 0x06003790 RID: 14224 RVA: 0x000BB839 File Offset: 0x000BA839
		public decimal Value
		{
			get
			{
				return this.dec;
			}
		}

		// Token: 0x04001C97 RID: 7319
		private decimal dec;
	}
}
