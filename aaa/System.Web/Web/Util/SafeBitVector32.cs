using System;
using System.Threading;

namespace System.Web.Util
{
	// Token: 0x0200077E RID: 1918
	[Serializable]
	internal struct SafeBitVector32
	{
		// Token: 0x06005CAE RID: 23726 RVA: 0x001737D0 File Offset: 0x001727D0
		internal SafeBitVector32(int data)
		{
			this._data = data;
		}

		// Token: 0x170017CB RID: 6091
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

		// Token: 0x06005CB1 RID: 23729 RVA: 0x00173830 File Offset: 0x00172830
		internal bool ChangeValue(int bit, bool value)
		{
			for (;;)
			{
				int data = this._data;
				int num;
				if (value)
				{
					num = data | bit;
				}
				else
				{
					num = data & ~bit;
				}
				if (data == num)
				{
					break;
				}
				int num2 = Interlocked.CompareExchange(ref this._data, num, data);
				if (num2 == data)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04003187 RID: 12679
		private volatile int _data;
	}
}
