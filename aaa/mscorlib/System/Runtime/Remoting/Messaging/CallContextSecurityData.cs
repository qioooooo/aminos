using System;
using System.Security.Principal;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x02000695 RID: 1685
	[Serializable]
	internal class CallContextSecurityData : ICloneable
	{
		// Token: 0x17000A47 RID: 2631
		// (get) Token: 0x06003D64 RID: 15716 RVA: 0x000D2A93 File Offset: 0x000D1A93
		// (set) Token: 0x06003D65 RID: 15717 RVA: 0x000D2A9B File Offset: 0x000D1A9B
		internal IPrincipal Principal
		{
			get
			{
				return this._principal;
			}
			set
			{
				this._principal = value;
			}
		}

		// Token: 0x17000A48 RID: 2632
		// (get) Token: 0x06003D66 RID: 15718 RVA: 0x000D2AA4 File Offset: 0x000D1AA4
		internal bool HasInfo
		{
			get
			{
				return null != this._principal;
			}
		}

		// Token: 0x06003D67 RID: 15719 RVA: 0x000D2AB4 File Offset: 0x000D1AB4
		public object Clone()
		{
			return new CallContextSecurityData
			{
				_principal = this._principal
			};
		}

		// Token: 0x04001F32 RID: 7986
		private IPrincipal _principal;
	}
}
