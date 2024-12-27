using System;
using System.Collections;

namespace System.Xml
{
	// Token: 0x020000F2 RID: 242
	internal class XmlNodeListEnumerator : IEnumerator
	{
		// Token: 0x06000ECB RID: 3787 RVA: 0x00040CD6 File Offset: 0x0003FCD6
		public XmlNodeListEnumerator(XPathNodeList list)
		{
			this.list = list;
			this.index = -1;
			this.valid = false;
		}

		// Token: 0x06000ECC RID: 3788 RVA: 0x00040CF3 File Offset: 0x0003FCF3
		public void Reset()
		{
			this.index = -1;
		}

		// Token: 0x06000ECD RID: 3789 RVA: 0x00040CFC File Offset: 0x0003FCFC
		public bool MoveNext()
		{
			this.index++;
			int num = this.list.ReadUntil(this.index + 1);
			if (num - 1 < this.index)
			{
				return false;
			}
			this.valid = this.list[this.index] != null;
			return this.valid;
		}

		// Token: 0x170003AD RID: 941
		// (get) Token: 0x06000ECE RID: 3790 RVA: 0x00040D5B File Offset: 0x0003FD5B
		public object Current
		{
			get
			{
				if (this.valid)
				{
					return this.list[this.index];
				}
				return null;
			}
		}

		// Token: 0x040009AC RID: 2476
		private XPathNodeList list;

		// Token: 0x040009AD RID: 2477
		private int index;

		// Token: 0x040009AE RID: 2478
		private bool valid;
	}
}
