using System;
using System.Collections;

namespace System.Xml
{
	// Token: 0x020000DB RID: 219
	internal class XmlElementListEnumerator : IEnumerator
	{
		// Token: 0x06000D89 RID: 3465 RVA: 0x0003C14D File Offset: 0x0003B14D
		public XmlElementListEnumerator(XmlElementList list)
		{
			this.list = list;
			this.curElem = null;
			this.changeCount = list.ChangeCount;
		}

		// Token: 0x06000D8A RID: 3466 RVA: 0x0003C170 File Offset: 0x0003B170
		public bool MoveNext()
		{
			if (this.list.ChangeCount != this.changeCount)
			{
				throw new InvalidOperationException(Res.GetString("Xdom_Enum_ElementList"));
			}
			this.curElem = this.list.GetNextNode(this.curElem);
			return this.curElem != null;
		}

		// Token: 0x06000D8B RID: 3467 RVA: 0x0003C1C3 File Offset: 0x0003B1C3
		public void Reset()
		{
			this.curElem = null;
			this.changeCount = this.list.ChangeCount;
		}

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06000D8C RID: 3468 RVA: 0x0003C1DD File Offset: 0x0003B1DD
		public object Current
		{
			get
			{
				return this.curElem;
			}
		}

		// Token: 0x04000954 RID: 2388
		private XmlElementList list;

		// Token: 0x04000955 RID: 2389
		private XmlNode curElem;

		// Token: 0x04000956 RID: 2390
		private int changeCount;
	}
}
