using System;
using System.Threading;

namespace System.Text.RegularExpressions
{
	// Token: 0x0200000E RID: 14
	internal sealed class SharedReference
	{
		// Token: 0x0600006E RID: 110 RVA: 0x0000322C File Offset: 0x0000222C
		internal object Get()
		{
			if (Interlocked.Exchange(ref this._locked, 1) == 0)
			{
				object target = this._ref.Target;
				this._locked = 0;
				return target;
			}
			return null;
		}

		// Token: 0x0600006F RID: 111 RVA: 0x0000325D File Offset: 0x0000225D
		internal void Cache(object obj)
		{
			if (Interlocked.Exchange(ref this._locked, 1) == 0)
			{
				this._ref.Target = obj;
				this._locked = 0;
			}
		}

		// Token: 0x04000640 RID: 1600
		private WeakReference _ref = new WeakReference(null);

		// Token: 0x04000641 RID: 1601
		private int _locked;
	}
}
