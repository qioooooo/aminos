using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;

namespace System.Xml.Serialization
{
	// Token: 0x02000321 RID: 801
	public class XmlSchemaEnumerator : IEnumerator<XmlSchema>, IDisposable, IEnumerator
	{
		// Token: 0x0600264D RID: 9805 RVA: 0x000BADFF File Offset: 0x000B9DFF
		public XmlSchemaEnumerator(XmlSchemas list)
		{
			this.list = list;
			this.idx = -1;
			this.end = list.Count - 1;
		}

		// Token: 0x0600264E RID: 9806 RVA: 0x000BAE23 File Offset: 0x000B9E23
		public void Dispose()
		{
		}

		// Token: 0x0600264F RID: 9807 RVA: 0x000BAE25 File Offset: 0x000B9E25
		public bool MoveNext()
		{
			if (this.idx >= this.end)
			{
				return false;
			}
			this.idx++;
			return true;
		}

		// Token: 0x17000955 RID: 2389
		// (get) Token: 0x06002650 RID: 9808 RVA: 0x000BAE46 File Offset: 0x000B9E46
		public XmlSchema Current
		{
			get
			{
				return this.list[this.idx];
			}
		}

		// Token: 0x17000956 RID: 2390
		// (get) Token: 0x06002651 RID: 9809 RVA: 0x000BAE59 File Offset: 0x000B9E59
		object IEnumerator.Current
		{
			get
			{
				return this.list[this.idx];
			}
		}

		// Token: 0x06002652 RID: 9810 RVA: 0x000BAE6C File Offset: 0x000B9E6C
		void IEnumerator.Reset()
		{
			this.idx = -1;
		}

		// Token: 0x040015CD RID: 5581
		private XmlSchemas list;

		// Token: 0x040015CE RID: 5582
		private int idx;

		// Token: 0x040015CF RID: 5583
		private int end;
	}
}
