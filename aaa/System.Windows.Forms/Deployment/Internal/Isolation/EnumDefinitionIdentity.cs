using System;
using System.Collections;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020000E6 RID: 230
	internal sealed class EnumDefinitionIdentity : IEnumerator
	{
		// Token: 0x06000386 RID: 902 RVA: 0x00007E8F File Offset: 0x00006E8F
		internal EnumDefinitionIdentity(IEnumDefinitionIdentity e)
		{
			if (e == null)
			{
				throw new ArgumentNullException();
			}
			this._enum = e;
		}

		// Token: 0x06000387 RID: 903 RVA: 0x00007EB3 File Offset: 0x00006EB3
		private DefinitionIdentity GetCurrent()
		{
			if (this._current == null)
			{
				throw new InvalidOperationException();
			}
			return new DefinitionIdentity(this._current);
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000388 RID: 904 RVA: 0x00007ECE File Offset: 0x00006ECE
		object IEnumerator.Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000389 RID: 905 RVA: 0x00007ED6 File Offset: 0x00006ED6
		public DefinitionIdentity Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x0600038A RID: 906 RVA: 0x00007EDE File Offset: 0x00006EDE
		public IEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x0600038B RID: 907 RVA: 0x00007EE1 File Offset: 0x00006EE1
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

		// Token: 0x0600038C RID: 908 RVA: 0x00007F10 File Offset: 0x00006F10
		public void Reset()
		{
			this._current = null;
			this._enum.Reset();
		}

		// Token: 0x04000D90 RID: 3472
		private IEnumDefinitionIdentity _enum;

		// Token: 0x04000D91 RID: 3473
		private IDefinitionIdentity _current;

		// Token: 0x04000D92 RID: 3474
		private IDefinitionIdentity[] _fetchList = new IDefinitionIdentity[1];
	}
}
