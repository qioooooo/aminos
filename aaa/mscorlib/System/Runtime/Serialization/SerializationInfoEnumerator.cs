using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.Runtime.Serialization
{
	// Token: 0x02000364 RID: 868
	[ComVisible(true)]
	public sealed class SerializationInfoEnumerator : IEnumerator
	{
		// Token: 0x06002296 RID: 8854 RVA: 0x00057EDD File Offset: 0x00056EDD
		internal SerializationInfoEnumerator(string[] members, object[] info, Type[] types, int numItems)
		{
			this.m_members = members;
			this.m_data = info;
			this.m_types = types;
			this.m_numItems = numItems - 1;
			this.m_currItem = -1;
			this.m_current = false;
		}

		// Token: 0x06002297 RID: 8855 RVA: 0x00057F12 File Offset: 0x00056F12
		public bool MoveNext()
		{
			if (this.m_currItem < this.m_numItems)
			{
				this.m_currItem++;
				this.m_current = true;
			}
			else
			{
				this.m_current = false;
			}
			return this.m_current;
		}

		// Token: 0x17000620 RID: 1568
		// (get) Token: 0x06002298 RID: 8856 RVA: 0x00057F48 File Offset: 0x00056F48
		object IEnumerator.Current
		{
			get
			{
				if (!this.m_current)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumOpCantHappen"));
				}
				return new SerializationEntry(this.m_members[this.m_currItem], this.m_data[this.m_currItem], this.m_types[this.m_currItem]);
			}
		}

		// Token: 0x17000621 RID: 1569
		// (get) Token: 0x06002299 RID: 8857 RVA: 0x00057FA0 File Offset: 0x00056FA0
		public SerializationEntry Current
		{
			get
			{
				if (!this.m_current)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumOpCantHappen"));
				}
				return new SerializationEntry(this.m_members[this.m_currItem], this.m_data[this.m_currItem], this.m_types[this.m_currItem]);
			}
		}

		// Token: 0x0600229A RID: 8858 RVA: 0x00057FF1 File Offset: 0x00056FF1
		public void Reset()
		{
			this.m_currItem = -1;
			this.m_current = false;
		}

		// Token: 0x17000622 RID: 1570
		// (get) Token: 0x0600229B RID: 8859 RVA: 0x00058001 File Offset: 0x00057001
		public string Name
		{
			get
			{
				if (!this.m_current)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumOpCantHappen"));
				}
				return this.m_members[this.m_currItem];
			}
		}

		// Token: 0x17000623 RID: 1571
		// (get) Token: 0x0600229C RID: 8860 RVA: 0x00058028 File Offset: 0x00057028
		public object Value
		{
			get
			{
				if (!this.m_current)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumOpCantHappen"));
				}
				return this.m_data[this.m_currItem];
			}
		}

		// Token: 0x17000624 RID: 1572
		// (get) Token: 0x0600229D RID: 8861 RVA: 0x0005804F File Offset: 0x0005704F
		public Type ObjectType
		{
			get
			{
				if (!this.m_current)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumOpCantHappen"));
				}
				return this.m_types[this.m_currItem];
			}
		}

		// Token: 0x04000E57 RID: 3671
		private string[] m_members;

		// Token: 0x04000E58 RID: 3672
		private object[] m_data;

		// Token: 0x04000E59 RID: 3673
		private Type[] m_types;

		// Token: 0x04000E5A RID: 3674
		private int m_numItems;

		// Token: 0x04000E5B RID: 3675
		private int m_currItem;

		// Token: 0x04000E5C RID: 3676
		private bool m_current;
	}
}
