using System;
using System.Security.Permissions;

namespace Microsoft.JScript
{
	// Token: 0x02000096 RID: 150
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	internal class TokenColorInfoList : ITokenEnumerator
	{
		// Token: 0x060006AB RID: 1707 RVA: 0x0002EE4D File Offset: 0x0002DE4D
		internal TokenColorInfoList()
		{
			this._head = null;
			this._current = null;
			this._atEnd = true;
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x0002EE6C File Offset: 0x0002DE6C
		internal void Add(Context token)
		{
			if (this._head == null)
			{
				TokenColorInfo tokenColorInfo = new TokenColorInfo(token);
				this._head = tokenColorInfo;
			}
			else
			{
				TokenColorInfo tokenColorInfo = this._head.Clone();
				this._head._next = tokenColorInfo;
				this._head.UpdateToken(token);
				this._head = tokenColorInfo;
			}
			this._current = this._head;
			this._atEnd = false;
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x0002EED4 File Offset: 0x0002DED4
		public virtual ITokenColorInfo GetNext()
		{
			if (this._atEnd)
			{
				return null;
			}
			ITokenColorInfo current = this._current;
			this._current = this._current._next;
			this._atEnd = this._current == this._head;
			return current;
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x0002EF18 File Offset: 0x0002DF18
		public virtual void Reset()
		{
			this._current = this._head;
			if (this._current != null)
			{
				this._atEnd = false;
			}
		}

		// Token: 0x04000307 RID: 775
		private TokenColorInfo _head;

		// Token: 0x04000308 RID: 776
		private TokenColorInfo _current;

		// Token: 0x04000309 RID: 777
		private bool _atEnd;
	}
}
