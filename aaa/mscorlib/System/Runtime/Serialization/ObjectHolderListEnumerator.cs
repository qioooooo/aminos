using System;

namespace System.Runtime.Serialization
{
	// Token: 0x02000354 RID: 852
	internal class ObjectHolderListEnumerator
	{
		// Token: 0x06002229 RID: 8745 RVA: 0x00056C6A File Offset: 0x00055C6A
		internal ObjectHolderListEnumerator(ObjectHolderList list, bool isFixupEnumerator)
		{
			this.m_list = list;
			this.m_startingVersion = this.m_list.Version;
			this.m_currPos = -1;
			this.m_isFixupEnumerator = isFixupEnumerator;
		}

		// Token: 0x0600222A RID: 8746 RVA: 0x00056C98 File Offset: 0x00055C98
		internal bool MoveNext()
		{
			if (this.m_isFixupEnumerator)
			{
				while (++this.m_currPos < this.m_list.Count && this.m_list.m_values[this.m_currPos].CompletelyFixed)
				{
				}
				return this.m_currPos != this.m_list.Count;
			}
			this.m_currPos++;
			return this.m_currPos != this.m_list.Count;
		}

		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x0600222B RID: 8747 RVA: 0x00056D1F File Offset: 0x00055D1F
		internal ObjectHolder Current
		{
			get
			{
				return this.m_list.m_values[this.m_currPos];
			}
		}

		// Token: 0x04000E32 RID: 3634
		private bool m_isFixupEnumerator;

		// Token: 0x04000E33 RID: 3635
		private ObjectHolderList m_list;

		// Token: 0x04000E34 RID: 3636
		private int m_startingVersion;

		// Token: 0x04000E35 RID: 3637
		private int m_currPos;
	}
}
