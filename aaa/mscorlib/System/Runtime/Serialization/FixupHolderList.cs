using System;

namespace System.Runtime.Serialization
{
	// Token: 0x02000351 RID: 849
	[Serializable]
	internal class FixupHolderList
	{
		// Token: 0x06002214 RID: 8724 RVA: 0x00056905 File Offset: 0x00055905
		internal FixupHolderList()
			: this(2)
		{
		}

		// Token: 0x06002215 RID: 8725 RVA: 0x0005690E File Offset: 0x0005590E
		internal FixupHolderList(int startingSize)
		{
			this.m_count = 0;
			this.m_values = new FixupHolder[startingSize];
		}

		// Token: 0x06002216 RID: 8726 RVA: 0x0005692C File Offset: 0x0005592C
		internal virtual void Add(long id, object fixupInfo)
		{
			if (this.m_count == this.m_values.Length)
			{
				this.EnlargeArray();
			}
			this.m_values[this.m_count].m_id = id;
			this.m_values[this.m_count++].m_fixupInfo = fixupInfo;
		}

		// Token: 0x06002217 RID: 8727 RVA: 0x00056980 File Offset: 0x00055980
		internal virtual void Add(FixupHolder fixup)
		{
			if (this.m_count == this.m_values.Length)
			{
				this.EnlargeArray();
			}
			this.m_values[this.m_count++] = fixup;
		}

		// Token: 0x06002218 RID: 8728 RVA: 0x000569BC File Offset: 0x000559BC
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
			FixupHolder[] array = new FixupHolder[num];
			Array.Copy(this.m_values, array, this.m_count);
			this.m_values = array;
		}

		// Token: 0x04000E27 RID: 3623
		internal const int InitialSize = 2;

		// Token: 0x04000E28 RID: 3624
		internal FixupHolder[] m_values;

		// Token: 0x04000E29 RID: 3625
		internal int m_count;
	}
}
