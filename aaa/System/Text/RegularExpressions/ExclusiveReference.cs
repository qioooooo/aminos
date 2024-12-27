using System;
using System.Threading;

namespace System.Text.RegularExpressions
{
	// Token: 0x0200000D RID: 13
	internal sealed class ExclusiveReference
	{
		// Token: 0x0600006B RID: 107 RVA: 0x00003188 File Offset: 0x00002188
		internal object Get()
		{
			if (Interlocked.Exchange(ref this._locked, 1) != 0)
			{
				return null;
			}
			object @ref = this._ref;
			if (@ref == null)
			{
				this._locked = 0;
				return null;
			}
			this._obj = @ref;
			return @ref;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x000031C0 File Offset: 0x000021C0
		internal void Release(object obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			if (this._obj == obj)
			{
				this._obj = null;
				this._locked = 0;
				return;
			}
			if (this._obj == null && Interlocked.Exchange(ref this._locked, 1) == 0)
			{
				if (this._ref == null)
				{
					this._ref = (RegexRunner)obj;
				}
				this._locked = 0;
			}
		}

		// Token: 0x0400063D RID: 1597
		private RegexRunner _ref;

		// Token: 0x0400063E RID: 1598
		private object _obj;

		// Token: 0x0400063F RID: 1599
		private int _locked;
	}
}
