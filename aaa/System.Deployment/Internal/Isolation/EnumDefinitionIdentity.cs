using System;
using System.Collections;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000111 RID: 273
	internal sealed class EnumDefinitionIdentity : IEnumerator
	{
		// Token: 0x06000670 RID: 1648 RVA: 0x0001F547 File Offset: 0x0001E547
		internal EnumDefinitionIdentity(IEnumDefinitionIdentity e)
		{
			if (e == null)
			{
				throw new ArgumentNullException();
			}
			this._enum = e;
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x0001F56B File Offset: 0x0001E56B
		private DefinitionIdentity GetCurrent()
		{
			if (this._current == null)
			{
				throw new InvalidOperationException();
			}
			return new DefinitionIdentity(this._current);
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000672 RID: 1650 RVA: 0x0001F586 File Offset: 0x0001E586
		object IEnumerator.Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x06000673 RID: 1651 RVA: 0x0001F58E File Offset: 0x0001E58E
		public DefinitionIdentity Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x0001F596 File Offset: 0x0001E596
		public IEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x0001F599 File Offset: 0x0001E599
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

		// Token: 0x06000676 RID: 1654 RVA: 0x0001F5C8 File Offset: 0x0001E5C8
		public void Reset()
		{
			this._current = null;
			this._enum.Reset();
		}

		// Token: 0x04000504 RID: 1284
		private IEnumDefinitionIdentity _enum;

		// Token: 0x04000505 RID: 1285
		private IDefinitionIdentity _current;

		// Token: 0x04000506 RID: 1286
		private IDefinitionIdentity[] _fetchList = new IDefinitionIdentity[1];
	}
}
