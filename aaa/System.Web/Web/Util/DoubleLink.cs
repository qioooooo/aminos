using System;

namespace System.Web.Util
{
	// Token: 0x020000C5 RID: 197
	internal class DoubleLink
	{
		// Token: 0x060008EB RID: 2283 RVA: 0x00028A20 File Offset: 0x00027A20
		internal DoubleLink()
		{
			this._prev = this;
			this._next = this;
		}

		// Token: 0x060008EC RID: 2284 RVA: 0x00028A43 File Offset: 0x00027A43
		internal DoubleLink(object item)
			: this()
		{
			this.Item = item;
		}

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x060008ED RID: 2285 RVA: 0x00028A52 File Offset: 0x00027A52
		internal DoubleLink Next
		{
			get
			{
				return this._next;
			}
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x00028A5A File Offset: 0x00027A5A
		internal void InsertAfter(DoubleLink after)
		{
			this._prev = after;
			this._next = after._next;
			after._next = this;
			this._next._prev = this;
		}

		// Token: 0x060008EF RID: 2287 RVA: 0x00028A82 File Offset: 0x00027A82
		internal void InsertBefore(DoubleLink before)
		{
			this._prev = before._prev;
			this._next = before;
			before._prev = this;
			this._prev._next = this;
		}

		// Token: 0x060008F0 RID: 2288 RVA: 0x00028AAC File Offset: 0x00027AAC
		internal void Remove()
		{
			this._prev._next = this._next;
			this._next._prev = this._prev;
			this._prev = this;
			this._next = this;
		}

		// Token: 0x04001226 RID: 4646
		internal DoubleLink _next;

		// Token: 0x04001227 RID: 4647
		internal DoubleLink _prev;

		// Token: 0x04001228 RID: 4648
		internal object Item;
	}
}
