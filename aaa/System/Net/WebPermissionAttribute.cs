using System;
using System.Security;
using System.Security.Permissions;

namespace System.Net
{
	// Token: 0x020004A8 RID: 1192
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public sealed class WebPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x06002479 RID: 9337 RVA: 0x0008F60A File Offset: 0x0008E60A
		public WebPermissionAttribute(SecurityAction action)
			: base(action)
		{
		}

		// Token: 0x1700078E RID: 1934
		// (get) Token: 0x0600247A RID: 9338 RVA: 0x0008F613 File Offset: 0x0008E613
		// (set) Token: 0x0600247B RID: 9339 RVA: 0x0008F620 File Offset: 0x0008E620
		public string Connect
		{
			get
			{
				return this.m_connect as string;
			}
			set
			{
				if (this.m_connect != null)
				{
					throw new ArgumentException(SR.GetString("net_perm_attrib_multi", new object[] { "Connect", value }), "value");
				}
				this.m_connect = value;
			}
		}

		// Token: 0x1700078F RID: 1935
		// (get) Token: 0x0600247C RID: 9340 RVA: 0x0008F665 File Offset: 0x0008E665
		// (set) Token: 0x0600247D RID: 9341 RVA: 0x0008F674 File Offset: 0x0008E674
		public string Accept
		{
			get
			{
				return this.m_accept as string;
			}
			set
			{
				if (this.m_accept != null)
				{
					throw new ArgumentException(SR.GetString("net_perm_attrib_multi", new object[] { "Accept", value }), "value");
				}
				this.m_accept = value;
			}
		}

		// Token: 0x17000790 RID: 1936
		// (get) Token: 0x0600247E RID: 9342 RVA: 0x0008F6B9 File Offset: 0x0008E6B9
		// (set) Token: 0x0600247F RID: 9343 RVA: 0x0008F6F8 File Offset: 0x0008E6F8
		public string ConnectPattern
		{
			get
			{
				if (this.m_connect is DelayedRegex)
				{
					return this.m_connect.ToString();
				}
				if (!(this.m_connect is bool) || !(bool)this.m_connect)
				{
					return null;
				}
				return ".*";
			}
			set
			{
				if (this.m_connect != null)
				{
					throw new ArgumentException(SR.GetString("net_perm_attrib_multi", new object[] { "ConnectPatern", value }), "value");
				}
				if (value == ".*")
				{
					this.m_connect = true;
					return;
				}
				this.m_connect = new DelayedRegex(value);
			}
		}

		// Token: 0x17000791 RID: 1937
		// (get) Token: 0x06002480 RID: 9344 RVA: 0x0008F75C File Offset: 0x0008E75C
		// (set) Token: 0x06002481 RID: 9345 RVA: 0x0008F798 File Offset: 0x0008E798
		public string AcceptPattern
		{
			get
			{
				if (this.m_accept is DelayedRegex)
				{
					return this.m_accept.ToString();
				}
				if (!(this.m_accept is bool) || !(bool)this.m_accept)
				{
					return null;
				}
				return ".*";
			}
			set
			{
				if (this.m_accept != null)
				{
					throw new ArgumentException(SR.GetString("net_perm_attrib_multi", new object[] { "AcceptPattern", value }), "value");
				}
				if (value == ".*")
				{
					this.m_accept = true;
					return;
				}
				this.m_accept = new DelayedRegex(value);
			}
		}

		// Token: 0x06002482 RID: 9346 RVA: 0x0008F7FC File Offset: 0x0008E7FC
		public override IPermission CreatePermission()
		{
			WebPermission webPermission;
			if (base.Unrestricted)
			{
				webPermission = new WebPermission(PermissionState.Unrestricted);
			}
			else
			{
				NetworkAccess networkAccess = (NetworkAccess)0;
				if (this.m_connect is bool)
				{
					if ((bool)this.m_connect)
					{
						networkAccess |= NetworkAccess.Connect;
					}
					this.m_connect = null;
				}
				if (this.m_accept is bool)
				{
					if ((bool)this.m_accept)
					{
						networkAccess |= NetworkAccess.Accept;
					}
					this.m_accept = null;
				}
				webPermission = new WebPermission(networkAccess);
				if (this.m_accept != null)
				{
					if (this.m_accept is DelayedRegex)
					{
						webPermission.AddAsPattern(NetworkAccess.Accept, (DelayedRegex)this.m_accept);
					}
					else
					{
						webPermission.AddPermission(NetworkAccess.Accept, (string)this.m_accept);
					}
				}
				if (this.m_connect != null)
				{
					if (this.m_connect is DelayedRegex)
					{
						webPermission.AddAsPattern(NetworkAccess.Connect, (DelayedRegex)this.m_connect);
					}
					else
					{
						webPermission.AddPermission(NetworkAccess.Connect, (string)this.m_connect);
					}
				}
			}
			return webPermission;
		}

		// Token: 0x040024BD RID: 9405
		private object m_accept;

		// Token: 0x040024BE RID: 9406
		private object m_connect;
	}
}
