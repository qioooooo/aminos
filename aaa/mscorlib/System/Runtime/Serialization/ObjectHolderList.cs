using System;

namespace System.Runtime.Serialization
{
	// Token: 0x02000353 RID: 851
	internal class ObjectHolderList
	{
		// Token: 0x06002222 RID: 8738 RVA: 0x00056B92 File Offset: 0x00055B92
		internal ObjectHolderList()
			: this(8)
		{
		}

		// Token: 0x06002223 RID: 8739 RVA: 0x00056B9B File Offset: 0x00055B9B
		internal ObjectHolderList(int startingSize)
		{
			this.m_count = 0;
			this.m_values = new ObjectHolder[startingSize];
		}

		// Token: 0x06002224 RID: 8740 RVA: 0x00056BB8 File Offset: 0x00055BB8
		internal virtual void Add(ObjectHolder value)
		{
			if (this.m_count == this.m_values.Length)
			{
				this.EnlargeArray();
			}
			this.m_values[this.m_count++] = value;
		}

		// Token: 0x06002225 RID: 8741 RVA: 0x00056BF4 File Offset: 0x00055BF4
		internal ObjectHolderListEnumerator GetFixupEnumerator()
		{
			return new ObjectHolderListEnumerator(this, true);
		}

		// Token: 0x06002226 RID: 8742 RVA: 0x00056C00 File Offset: 0x00055C00
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
			ObjectHolder[] array = new ObjectHolder[num];
			Array.Copy(this.m_values, array, this.m_count);
			this.m_values = array;
		}

		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x06002227 RID: 8743 RVA: 0x00056C5A File Offset: 0x00055C5A
		internal int Version
		{
			get
			{
				return this.m_count;
			}
		}

		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x06002228 RID: 8744 RVA: 0x00056C62 File Offset: 0x00055C62
		internal int Count
		{
			get
			{
				return this.m_count;
			}
		}

		// Token: 0x04000E2F RID: 3631
		internal const int DefaultInitialSize = 8;

		// Token: 0x04000E30 RID: 3632
		internal ObjectHolder[] m_values;

		// Token: 0x04000E31 RID: 3633
		internal int m_count;
	}
}
