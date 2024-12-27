using System;
using System.Collections;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020000E8 RID: 232
	internal sealed class EnumReferenceIdentity : IEnumerator
	{
		// Token: 0x06000391 RID: 913 RVA: 0x00007F24 File Offset: 0x00006F24
		internal EnumReferenceIdentity(IEnumReferenceIdentity e)
		{
			this._enum = e;
		}

		// Token: 0x06000392 RID: 914 RVA: 0x00007F3F File Offset: 0x00006F3F
		private ReferenceIdentity GetCurrent()
		{
			if (this._current == null)
			{
				throw new InvalidOperationException();
			}
			return new ReferenceIdentity(this._current);
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000393 RID: 915 RVA: 0x00007F5A File Offset: 0x00006F5A
		object IEnumerator.Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000394 RID: 916 RVA: 0x00007F62 File Offset: 0x00006F62
		public ReferenceIdentity Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x06000395 RID: 917 RVA: 0x00007F6A File Offset: 0x00006F6A
		public IEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x06000396 RID: 918 RVA: 0x00007F6D File Offset: 0x00006F6D
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

		// Token: 0x06000397 RID: 919 RVA: 0x00007F9C File Offset: 0x00006F9C
		public void Reset()
		{
			this._current = null;
			this._enum.Reset();
		}

		// Token: 0x04000D93 RID: 3475
		private IEnumReferenceIdentity _enum;

		// Token: 0x04000D94 RID: 3476
		private IReferenceIdentity _current;

		// Token: 0x04000D95 RID: 3477
		private IReferenceIdentity[] _fetchList = new IReferenceIdentity[1];
	}
}
