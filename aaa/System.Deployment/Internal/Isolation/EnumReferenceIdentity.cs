using System;
using System.Collections;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000113 RID: 275
	internal sealed class EnumReferenceIdentity : IEnumerator
	{
		// Token: 0x0600067B RID: 1659 RVA: 0x0001F5DC File Offset: 0x0001E5DC
		internal EnumReferenceIdentity(IEnumReferenceIdentity e)
		{
			this._enum = e;
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x0001F5F7 File Offset: 0x0001E5F7
		private ReferenceIdentity GetCurrent()
		{
			if (this._current == null)
			{
				throw new InvalidOperationException();
			}
			return new ReferenceIdentity(this._current);
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x0600067D RID: 1661 RVA: 0x0001F612 File Offset: 0x0001E612
		object IEnumerator.Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x0600067E RID: 1662 RVA: 0x0001F61A File Offset: 0x0001E61A
		public ReferenceIdentity Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x0001F622 File Offset: 0x0001E622
		public IEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x0001F625 File Offset: 0x0001E625
		public bool MoveNext()
		{
			if (this._enum.Next(1U, this._fetchList) == 1U)
			{
				this._current = this._fetchList[0];
				return true;
			}
			this._current = null;
			return false;
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x0001F654 File Offset: 0x0001E654
		public void Reset()
		{
			this._current = null;
			this._enum.Reset();
		}

		// Token: 0x04000507 RID: 1287
		private IEnumReferenceIdentity _enum;

		// Token: 0x04000508 RID: 1288
		private IReferenceIdentity _current;

		// Token: 0x04000509 RID: 1289
		private IReferenceIdentity[] _fetchList = new IReferenceIdentity[1];
	}
}
