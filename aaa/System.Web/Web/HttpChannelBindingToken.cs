using System;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Permissions;

namespace System.Web
{
	// Token: 0x02000062 RID: 98
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	internal sealed class HttpChannelBindingToken : ChannelBinding
	{
		// Token: 0x060003B0 RID: 944 RVA: 0x000112EF File Offset: 0x000102EF
		internal HttpChannelBindingToken(IntPtr token, int tokenSize)
		{
			base.SetHandle(token);
			this._size = tokenSize;
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x00011305 File Offset: 0x00010305
		protected override bool ReleaseHandle()
		{
			base.SetHandle(IntPtr.Zero);
			this._size = 0;
			return true;
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x060003B2 RID: 946 RVA: 0x0001131A File Offset: 0x0001031A
		public override int Size
		{
			get
			{
				return this._size;
			}
		}

		// Token: 0x04000FCD RID: 4045
		private int _size;
	}
}
