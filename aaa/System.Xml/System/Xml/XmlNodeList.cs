using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace System.Xml
{
	// Token: 0x020000D0 RID: 208
	public abstract class XmlNodeList : IEnumerable
	{
		// Token: 0x06000C5B RID: 3163
		public abstract XmlNode Item(int index);

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x06000C5C RID: 3164
		public abstract int Count { get; }

		// Token: 0x06000C5D RID: 3165
		public abstract IEnumerator GetEnumerator();

		// Token: 0x170002CD RID: 717
		[IndexerName("ItemOf")]
		public virtual XmlNode this[int i]
		{
			get
			{
				return this.Item(i);
			}
		}
	}
}
