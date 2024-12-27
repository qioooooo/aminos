using System;
using System.Collections.Generic;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x0200012F RID: 303
	internal sealed class ClonableStack<T> : List<T>
	{
		// Token: 0x060011B3 RID: 4531 RVA: 0x0004E622 File Offset: 0x0004D622
		public ClonableStack()
		{
		}

		// Token: 0x060011B4 RID: 4532 RVA: 0x0004E62A File Offset: 0x0004D62A
		public ClonableStack(int capacity)
			: base(capacity)
		{
		}

		// Token: 0x060011B5 RID: 4533 RVA: 0x0004E633 File Offset: 0x0004D633
		private ClonableStack(IEnumerable<T> collection)
			: base(collection)
		{
		}

		// Token: 0x060011B6 RID: 4534 RVA: 0x0004E63C File Offset: 0x0004D63C
		public void Push(T value)
		{
			base.Add(value);
		}

		// Token: 0x060011B7 RID: 4535 RVA: 0x0004E648 File Offset: 0x0004D648
		public T Pop()
		{
			int num = base.Count - 1;
			T t = base[num];
			base.RemoveAt(num);
			return t;
		}

		// Token: 0x060011B8 RID: 4536 RVA: 0x0004E66E File Offset: 0x0004D66E
		public T Peek()
		{
			return base[base.Count - 1];
		}

		// Token: 0x060011B9 RID: 4537 RVA: 0x0004E67E File Offset: 0x0004D67E
		public ClonableStack<T> Clone()
		{
			return new ClonableStack<T>(this);
		}
	}
}
