using System;
using System.Collections;

namespace System.Web.Util
{
	// Token: 0x0200075E RID: 1886
	internal class DoubleLinkListEnumerator : IEnumerator
	{
		// Token: 0x06005BD0 RID: 23504 RVA: 0x001706D9 File Offset: 0x0016F6D9
		internal DoubleLinkListEnumerator(DoubleLinkList list)
		{
			this._list = list;
			this._current = list;
		}

		// Token: 0x06005BD1 RID: 23505 RVA: 0x001706EF File Offset: 0x0016F6EF
		public void Reset()
		{
			this._current = this._list;
		}

		// Token: 0x06005BD2 RID: 23506 RVA: 0x001706FD File Offset: 0x0016F6FD
		public bool MoveNext()
		{
			if (this._current.Next == this._list)
			{
				this._current = null;
				return false;
			}
			this._current = this._current.Next;
			return true;
		}

		// Token: 0x170017AA RID: 6058
		// (get) Token: 0x06005BD3 RID: 23507 RVA: 0x0017072D File Offset: 0x0016F72D
		public object Current
		{
			get
			{
				if (this._current == null || this._current == this._list)
				{
					throw new InvalidOperationException();
				}
				return this._current.Item;
			}
		}

		// Token: 0x06005BD4 RID: 23508 RVA: 0x00170756 File Offset: 0x0016F756
		internal DoubleLink GetDoubleLink()
		{
			return this._current;
		}

		// Token: 0x04003120 RID: 12576
		private DoubleLinkList _list;

		// Token: 0x04003121 RID: 12577
		private DoubleLink _current;
	}
}
