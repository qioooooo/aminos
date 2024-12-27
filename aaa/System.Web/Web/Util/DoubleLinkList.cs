using System;

namespace System.Web.Util
{
	// Token: 0x0200075D RID: 1885
	internal class DoubleLinkList : DoubleLink
	{
		// Token: 0x06005BCA RID: 23498 RVA: 0x00170685 File Offset: 0x0016F685
		internal DoubleLinkList()
		{
		}

		// Token: 0x06005BCB RID: 23499 RVA: 0x0017068D File Offset: 0x0016F68D
		internal bool IsEmpty()
		{
			return this._next == this;
		}

		// Token: 0x06005BCC RID: 23500 RVA: 0x00170698 File Offset: 0x0016F698
		internal virtual void InsertHead(DoubleLink entry)
		{
			entry.InsertAfter(this);
		}

		// Token: 0x06005BCD RID: 23501 RVA: 0x001706A1 File Offset: 0x0016F6A1
		internal virtual void InsertTail(DoubleLink entry)
		{
			entry.InsertBefore(this);
		}

		// Token: 0x06005BCE RID: 23502 RVA: 0x001706AA File Offset: 0x0016F6AA
		internal DoubleLinkListEnumerator GetEnumerator()
		{
			return new DoubleLinkListEnumerator(this);
		}

		// Token: 0x170017A9 RID: 6057
		// (get) Token: 0x06005BCF RID: 23503 RVA: 0x001706B4 File Offset: 0x0016F6B4
		internal int Length
		{
			get
			{
				int num = 0;
				DoubleLinkListEnumerator enumerator = this.GetEnumerator();
				while (enumerator.MoveNext())
				{
					num++;
				}
				return num;
			}
		}
	}
}
