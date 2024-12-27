using System;
using System.Collections.Generic;

namespace System.Data
{
	// Token: 0x020000DF RID: 223
	internal sealed class Listeners<TElem> where TElem : class
	{
		// Token: 0x06000D72 RID: 3442 RVA: 0x001FFA84 File Offset: 0x001FEE84
		internal Listeners(int ObjectID, Listeners<TElem>.Func<TElem, bool> notifyFilter)
		{
			this.listeners = new List<TElem>();
			this.filter = notifyFilter;
			this.ObjectID = ObjectID;
			this._listenerReaderCount = 0;
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06000D73 RID: 3443 RVA: 0x001FFAB8 File Offset: 0x001FEEB8
		internal bool HasListeners
		{
			get
			{
				return 0 < this.listeners.Count;
			}
		}

		// Token: 0x06000D74 RID: 3444 RVA: 0x001FFAD4 File Offset: 0x001FEED4
		internal void Add(TElem listener)
		{
			this.listeners.Add(listener);
		}

		// Token: 0x06000D75 RID: 3445 RVA: 0x001FFAF0 File Offset: 0x001FEEF0
		internal int IndexOfReference(TElem listener)
		{
			return Index.IndexOfReference<TElem>(this.listeners, listener);
		}

		// Token: 0x06000D76 RID: 3446 RVA: 0x001FFB0C File Offset: 0x001FEF0C
		internal void Remove(TElem listener)
		{
			int num = this.IndexOfReference(listener);
			this.listeners[num] = default(TElem);
			if (this._listenerReaderCount == 0)
			{
				this.listeners.RemoveAt(num);
				this.listeners.TrimExcess();
			}
		}

		// Token: 0x06000D77 RID: 3447 RVA: 0x001FFB58 File Offset: 0x001FEF58
		internal void Notify<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3, Listeners<TElem>.Action<TElem, T1, T2, T3> action)
		{
			int count = this.listeners.Count;
			if (0 < count)
			{
				int num = -1;
				this._listenerReaderCount++;
				try
				{
					for (int i = 0; i < count; i++)
					{
						TElem telem = this.listeners[i];
						if (this.filter(telem))
						{
							action(telem, arg1, arg2, arg3);
						}
						else
						{
							this.listeners[i] = default(TElem);
							num = i;
						}
					}
				}
				finally
				{
					this._listenerReaderCount--;
				}
				if (this._listenerReaderCount == 0)
				{
					this.RemoveNullListeners(num);
				}
			}
		}

		// Token: 0x06000D78 RID: 3448 RVA: 0x001FFC10 File Offset: 0x001FF010
		private void RemoveNullListeners(int nullIndex)
		{
			int num = nullIndex;
			while (0 <= num)
			{
				if (this.listeners[num] == null)
				{
					this.listeners.RemoveAt(num);
				}
				num--;
			}
		}

		// Token: 0x04000915 RID: 2325
		private readonly List<TElem> listeners;

		// Token: 0x04000916 RID: 2326
		private readonly Listeners<TElem>.Func<TElem, bool> filter;

		// Token: 0x04000917 RID: 2327
		private readonly int ObjectID;

		// Token: 0x04000918 RID: 2328
		private int _listenerReaderCount;

		// Token: 0x020000E0 RID: 224
		// (Invoke) Token: 0x06000D7A RID: 3450
		internal delegate void Action<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);

		// Token: 0x020000E1 RID: 225
		// (Invoke) Token: 0x06000D7E RID: 3454
		internal delegate TResult Func<T1, TResult>(T1 arg1);
	}
}
