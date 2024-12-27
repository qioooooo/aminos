using System;
using System.Data.Common;
using System.Globalization;

namespace System.Data.SqlClient
{
	// Token: 0x020002FC RID: 764
	internal sealed class ServerInfo
	{
		// Token: 0x060027E6 RID: 10214 RVA: 0x0028C994 File Offset: 0x0028BD94
		internal ServerInfo(string userProtocol, string userServerName)
		{
			this._userProtocol = userProtocol;
			this._userServerName = userServerName;
			this.PreRoutingServerName = null;
		}

		// Token: 0x060027E7 RID: 10215 RVA: 0x0028C9BC File Offset: 0x0028BDBC
		internal ServerInfo(SqlConnectionString userOptions, RoutingInfo routing, string preRoutingServerName)
		{
			if (routing == null || routing.ServerName == null)
			{
				this.UserServerName = string.Empty;
			}
			else
			{
				this.UserServerName = string.Format(CultureInfo.InvariantCulture, "{0},{1}", new object[] { routing.ServerName, routing.Port });
			}
			this.PreRoutingServerName = preRoutingServerName;
			this.UserProtocol = "tcp";
			this.SetDerivedNames(this.UserProtocol, this.UserServerName);
		}

		// Token: 0x17000678 RID: 1656
		// (get) Token: 0x060027E8 RID: 10216 RVA: 0x0028CA40 File Offset: 0x0028BE40
		// (set) Token: 0x060027E9 RID: 10217 RVA: 0x0028CA54 File Offset: 0x0028BE54
		internal string ExtendedServerName
		{
			get
			{
				return this._extendedServerName;
			}
			set
			{
				this._extendedServerName = value;
			}
		}

		// Token: 0x17000679 RID: 1657
		// (get) Token: 0x060027EA RID: 10218 RVA: 0x0028CA68 File Offset: 0x0028BE68
		// (set) Token: 0x060027EB RID: 10219 RVA: 0x0028CA7C File Offset: 0x0028BE7C
		internal string ResolvedServerName
		{
			get
			{
				return this._resolvedServerName;
			}
			set
			{
				this._resolvedServerName = value;
			}
		}

		// Token: 0x1700067A RID: 1658
		// (get) Token: 0x060027EC RID: 10220 RVA: 0x0028CA90 File Offset: 0x0028BE90
		// (set) Token: 0x060027ED RID: 10221 RVA: 0x0028CAA4 File Offset: 0x0028BEA4
		internal string UserProtocol
		{
			get
			{
				return this._userProtocol;
			}
			set
			{
				this._userProtocol = value;
			}
		}

		// Token: 0x1700067B RID: 1659
		// (get) Token: 0x060027EE RID: 10222 RVA: 0x0028CAB8 File Offset: 0x0028BEB8
		// (set) Token: 0x060027EF RID: 10223 RVA: 0x0028CACC File Offset: 0x0028BECC
		internal string UserServerName
		{
			get
			{
				return this._userServerName;
			}
			set
			{
				this._userServerName = value;
			}
		}

		// Token: 0x060027F0 RID: 10224 RVA: 0x0028CAE0 File Offset: 0x0028BEE0
		internal void SetDerivedNames(string protocol, string serverName)
		{
			if (!ADP.IsEmpty(protocol))
			{
				this.ExtendedServerName = protocol + ":" + serverName;
			}
			else
			{
				this.ExtendedServerName = serverName;
			}
			this.ResolvedServerName = serverName;
		}

		// Token: 0x0400190A RID: 6410
		private string _extendedServerName;

		// Token: 0x0400190B RID: 6411
		private string _resolvedServerName;

		// Token: 0x0400190C RID: 6412
		private string _userProtocol;

		// Token: 0x0400190D RID: 6413
		private string _userServerName;

		// Token: 0x0400190E RID: 6414
		internal readonly string PreRoutingServerName;
	}
}
