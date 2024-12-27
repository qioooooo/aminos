using System;
using System.Collections;

namespace System.Xml
{
	// Token: 0x020000DC RID: 220
	internal class XmlEmptyElementListEnumerator : IEnumerator
	{
		// Token: 0x06000D8D RID: 3469 RVA: 0x0003C1E5 File Offset: 0x0003B1E5
		public XmlEmptyElementListEnumerator(XmlElementList list)
		{
		}

		// Token: 0x06000D8E RID: 3470 RVA: 0x0003C1ED File Offset: 0x0003B1ED
		public bool MoveNext()
		{
			return false;
		}

		// Token: 0x06000D8F RID: 3471 RVA: 0x0003C1F0 File Offset: 0x0003B1F0
		public void Reset()
		{
		}

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x06000D90 RID: 3472 RVA: 0x0003C1F2 File Offset: 0x0003B1F2
		public object Current
		{
			get
			{
				return null;
			}
		}
	}
}
