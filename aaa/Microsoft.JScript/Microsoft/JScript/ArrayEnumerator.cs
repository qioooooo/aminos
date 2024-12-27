using System;
using System.Collections;
using System.Globalization;

namespace Microsoft.JScript
{
	// Token: 0x02000010 RID: 16
	internal class ArrayEnumerator : IEnumerator
	{
		// Token: 0x060000B9 RID: 185 RVA: 0x00004EB5 File Offset: 0x00003EB5
		internal ArrayEnumerator(ArrayObject arrayOb, IEnumerator denseEnum)
		{
			this.curr = -1;
			this.doDenseEnum = false;
			this.didDenseEnum = false;
			this.arrayOb = arrayOb;
			this.denseEnum = denseEnum;
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00004EE0 File Offset: 0x00003EE0
		public virtual bool MoveNext()
		{
			if (this.doDenseEnum)
			{
				if (this.denseEnum.MoveNext())
				{
					return true;
				}
				this.doDenseEnum = false;
				this.didDenseEnum = true;
			}
			int num = this.curr + 1;
			if ((long)num >= (long)((ulong)this.arrayOb.len) || (long)num >= (long)((ulong)this.arrayOb.denseArrayLength))
			{
				this.doDenseEnum = !this.didDenseEnum;
				return this.denseEnum.MoveNext();
			}
			this.curr = num;
			return !(this.arrayOb.GetValueAtIndex((uint)num) is Missing) || this.MoveNext();
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x060000BB RID: 187 RVA: 0x00004F78 File Offset: 0x00003F78
		public virtual object Current
		{
			get
			{
				if (this.doDenseEnum)
				{
					return this.denseEnum.Current;
				}
				if ((long)this.curr >= (long)((ulong)this.arrayOb.len) || (long)this.curr >= (long)((ulong)this.arrayOb.denseArrayLength))
				{
					return this.denseEnum.Current;
				}
				return this.curr.ToString(CultureInfo.InvariantCulture);
			}
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00004FDF File Offset: 0x00003FDF
		public virtual void Reset()
		{
			this.curr = -1;
			this.doDenseEnum = false;
			this.didDenseEnum = false;
			this.denseEnum.Reset();
		}

		// Token: 0x0400002C RID: 44
		private int curr;

		// Token: 0x0400002D RID: 45
		private bool doDenseEnum;

		// Token: 0x0400002E RID: 46
		private bool didDenseEnum;

		// Token: 0x0400002F RID: 47
		private ArrayObject arrayOb;

		// Token: 0x04000030 RID: 48
		private IEnumerator denseEnum;
	}
}
