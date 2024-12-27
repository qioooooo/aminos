using System;
using System.Threading;

namespace System.Configuration
{
	// Token: 0x02000094 RID: 148
	[Serializable]
	internal struct SafeBitVector32
	{
		// Token: 0x06000566 RID: 1382 RVA: 0x0001AB12 File Offset: 0x00019B12
		internal SafeBitVector32(int data)
		{
			this._data = data;
		}

		// Token: 0x17000187 RID: 391
		internal bool this[int bit]
		{
			get
			{
				int data = this._data;
				return (data & bit) == bit;
			}
			set
			{
				int data;
				int num2;
				do
				{
					data = this._data;
					int num;
					if (value)
					{
						num = data | bit;
					}
					else
					{
						num = data & ~bit;
					}
					num2 = Interlocked.CompareExchange(ref this._data, num, data);
				}
				while (num2 != data);
			}
		}

		// Token: 0x04000387 RID: 903
		private volatile int _data;
	}
}
