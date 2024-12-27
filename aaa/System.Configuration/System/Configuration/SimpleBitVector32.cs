using System;

namespace System.Configuration
{
	// Token: 0x0200009A RID: 154
	[Serializable]
	internal struct SimpleBitVector32
	{
		// Token: 0x06000614 RID: 1556 RVA: 0x0001C88E File Offset: 0x0001B88E
		internal SimpleBitVector32(int data)
		{
			this.data = data;
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06000615 RID: 1557 RVA: 0x0001C897 File Offset: 0x0001B897
		internal int Data
		{
			get
			{
				return this.data;
			}
		}

		// Token: 0x170001DD RID: 477
		internal bool this[int bit]
		{
			get
			{
				return (this.data & bit) == bit;
			}
			set
			{
				int num = this.data;
				if (value)
				{
					this.data = num | bit;
					return;
				}
				this.data = num & ~bit;
			}
		}

		// Token: 0x040003DB RID: 987
		private int data;
	}
}
