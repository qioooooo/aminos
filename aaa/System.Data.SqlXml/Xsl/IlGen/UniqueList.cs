using System;
using System.Collections.Generic;

namespace System.Xml.Xsl.IlGen
{
	// Token: 0x0200002B RID: 43
	internal class UniqueList<T>
	{
		// Token: 0x060001D0 RID: 464 RVA: 0x0000D7F4 File Offset: 0x0000C7F4
		public int Add(T value)
		{
			int num;
			if (!this.lookup.ContainsKey(value))
			{
				num = this.list.Count;
				this.lookup.Add(value, num);
				this.list.Add(value);
			}
			else
			{
				num = this.lookup[value];
			}
			return num;
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x0000D844 File Offset: 0x0000C844
		public T[] ToArray()
		{
			return this.list.ToArray();
		}

		// Token: 0x04000286 RID: 646
		private Dictionary<T, int> lookup = new Dictionary<T, int>();

		// Token: 0x04000287 RID: 647
		private List<T> list = new List<T>();
	}
}
