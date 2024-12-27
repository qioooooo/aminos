using System;

namespace System.Runtime.Serialization
{
	// Token: 0x02000352 RID: 850
	[Serializable]
	internal class LongList
	{
		// Token: 0x06002219 RID: 8729 RVA: 0x00056A16 File Offset: 0x00055A16
		internal LongList()
			: this(2)
		{
		}

		// Token: 0x0600221A RID: 8730 RVA: 0x00056A1F File Offset: 0x00055A1F
		internal LongList(int startingSize)
		{
			this.m_count = 0;
			this.m_totalItems = 0;
			this.m_values = new long[startingSize];
		}

		// Token: 0x0600221B RID: 8731 RVA: 0x00056A44 File Offset: 0x00055A44
		internal void Add(long value)
		{
			if (this.m_totalItems == this.m_values.Length)
			{
				this.EnlargeArray();
			}
			this.m_values[this.m_totalItems++] = value;
			this.m_count++;
		}

		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x0600221C RID: 8732 RVA: 0x00056A8E File Offset: 0x00055A8E
		internal int Count
		{
			get
			{
				return this.m_count;
			}
		}

		// Token: 0x0600221D RID: 8733 RVA: 0x00056A96 File Offset: 0x00055A96
		internal void StartEnumeration()
		{
			this.m_currentItem = -1;
		}

		// Token: 0x0600221E RID: 8734 RVA: 0x00056AA0 File Offset: 0x00055AA0
		internal bool MoveNext()
		{
			while (++this.m_currentItem < this.m_totalItems && this.m_values[this.m_currentItem] == -1L)
			{
			}
			return this.m_currentItem != this.m_totalItems;
		}

		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x0600221F RID: 8735 RVA: 0x00056AE8 File Offset: 0x00055AE8
		internal long Current
		{
			get
			{
				return this.m_values[this.m_currentItem];
			}
		}

		// Token: 0x06002220 RID: 8736 RVA: 0x00056AF8 File Offset: 0x00055AF8
		internal bool RemoveElement(long value)
		{
			int num = 0;
			while (num < this.m_totalItems && this.m_values[num] != value)
			{
				num++;
			}
			if (num == this.m_totalItems)
			{
				return false;
			}
			this.m_values[num] = -1L;
			return true;
		}

		// Token: 0x06002221 RID: 8737 RVA: 0x00056B38 File Offset: 0x00055B38
		private void EnlargeArray()
		{
			int num = this.m_values.Length * 2;
			if (num < 0)
			{
				if (num == 2147483647)
				{
					throw new SerializationException(Environment.GetResourceString("Serialization_TooManyElements"));
				}
				num = int.MaxValue;
			}
			long[] array = new long[num];
			Array.Copy(this.m_values, array, this.m_count);
			this.m_values = array;
		}

		// Token: 0x04000E2A RID: 3626
		private const int InitialSize = 2;

		// Token: 0x04000E2B RID: 3627
		private long[] m_values;

		// Token: 0x04000E2C RID: 3628
		private int m_count;

		// Token: 0x04000E2D RID: 3629
		private int m_totalItems;

		// Token: 0x04000E2E RID: 3630
		private int m_currentItem;
	}
}
