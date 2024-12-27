using System;
using System.Globalization;
using System.Security;
using System.Security.Permissions;
using System.Threading;

namespace System.Net
{
	// Token: 0x02000440 RID: 1088
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public sealed class SocketPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x06002221 RID: 8737 RVA: 0x00086B78 File Offset: 0x00085B78
		public SocketPermissionAttribute(SecurityAction action)
			: base(action)
		{
		}

		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x06002222 RID: 8738 RVA: 0x00086B81 File Offset: 0x00085B81
		// (set) Token: 0x06002223 RID: 8739 RVA: 0x00086B8C File Offset: 0x00085B8C
		public string Access
		{
			get
			{
				return this.m_access;
			}
			set
			{
				if (this.m_access != null)
				{
					throw new ArgumentException(SR.GetString("net_perm_attrib_multi", new object[] { "Access", value }), "value");
				}
				this.m_access = value;
			}
		}

		// Token: 0x17000742 RID: 1858
		// (get) Token: 0x06002224 RID: 8740 RVA: 0x00086BD1 File Offset: 0x00085BD1
		// (set) Token: 0x06002225 RID: 8741 RVA: 0x00086BDC File Offset: 0x00085BDC
		public string Host
		{
			get
			{
				return this.m_host;
			}
			set
			{
				if (this.m_host != null)
				{
					throw new ArgumentException(SR.GetString("net_perm_attrib_multi", new object[] { "Host", value }), "value");
				}
				this.m_host = value;
			}
		}

		// Token: 0x17000743 RID: 1859
		// (get) Token: 0x06002226 RID: 8742 RVA: 0x00086C21 File Offset: 0x00085C21
		// (set) Token: 0x06002227 RID: 8743 RVA: 0x00086C2C File Offset: 0x00085C2C
		public string Transport
		{
			get
			{
				return this.m_transport;
			}
			set
			{
				if (this.m_transport != null)
				{
					throw new ArgumentException(SR.GetString("net_perm_attrib_multi", new object[] { "Transport", value }), "value");
				}
				this.m_transport = value;
			}
		}

		// Token: 0x17000744 RID: 1860
		// (get) Token: 0x06002228 RID: 8744 RVA: 0x00086C71 File Offset: 0x00085C71
		// (set) Token: 0x06002229 RID: 8745 RVA: 0x00086C7C File Offset: 0x00085C7C
		public string Port
		{
			get
			{
				return this.m_port;
			}
			set
			{
				if (this.m_port != null)
				{
					throw new ArgumentException(SR.GetString("net_perm_attrib_multi", new object[] { "Port", value }), "value");
				}
				this.m_port = value;
			}
		}

		// Token: 0x0600222A RID: 8746 RVA: 0x00086CC4 File Offset: 0x00085CC4
		public override IPermission CreatePermission()
		{
			SocketPermission socketPermission;
			if (base.Unrestricted)
			{
				socketPermission = new SocketPermission(PermissionState.Unrestricted);
			}
			else
			{
				socketPermission = new SocketPermission(PermissionState.None);
				if (this.m_access == null)
				{
					throw new ArgumentException(SR.GetString("net_perm_attrib_count", new object[] { "Access" }));
				}
				if (this.m_host == null)
				{
					throw new ArgumentException(SR.GetString("net_perm_attrib_count", new object[] { "Host" }));
				}
				if (this.m_transport == null)
				{
					throw new ArgumentException(SR.GetString("net_perm_attrib_count", new object[] { "Transport" }));
				}
				if (this.m_port == null)
				{
					throw new ArgumentException(SR.GetString("net_perm_attrib_count", new object[] { "Port" }));
				}
				this.ParseAddPermissions(socketPermission);
			}
			return socketPermission;
		}

		// Token: 0x0600222B RID: 8747 RVA: 0x00086D9C File Offset: 0x00085D9C
		private void ParseAddPermissions(SocketPermission perm)
		{
			NetworkAccess networkAccess;
			if (string.Compare(this.m_access, "Connect", StringComparison.OrdinalIgnoreCase) == 0)
			{
				networkAccess = NetworkAccess.Connect;
			}
			else
			{
				if (string.Compare(this.m_access, "Accept", StringComparison.OrdinalIgnoreCase) != 0)
				{
					throw new ArgumentException(SR.GetString("net_perm_invalid_val", new object[] { "Access", this.m_access }));
				}
				networkAccess = NetworkAccess.Accept;
			}
			TransportType transportType;
			try
			{
				transportType = (TransportType)Enum.Parse(typeof(TransportType), this.m_transport, true);
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				throw new ArgumentException(SR.GetString("net_perm_invalid_val", new object[] { "Transport", this.m_transport }), ex);
			}
			catch
			{
				throw new ArgumentException(SR.GetString("net_perm_invalid_val", new object[] { "Transport", this.m_transport }), new Exception(SR.GetString("net_nonClsCompliantException")));
			}
			if (string.Compare(this.m_port, "All", StringComparison.OrdinalIgnoreCase) == 0)
			{
				this.m_port = "-1";
			}
			int num;
			try
			{
				num = int.Parse(this.m_port, NumberFormatInfo.InvariantInfo);
			}
			catch (Exception ex2)
			{
				if (ex2 is ThreadAbortException || ex2 is StackOverflowException || ex2 is OutOfMemoryException)
				{
					throw;
				}
				throw new ArgumentException(SR.GetString("net_perm_invalid_val", new object[] { "Port", this.m_port }), ex2);
			}
			catch
			{
				throw new ArgumentException(SR.GetString("net_perm_invalid_val", new object[] { "Port", this.m_port }), new Exception(SR.GetString("net_nonClsCompliantException")));
			}
			if (!ValidationHelper.ValidateTcpPort(num) && num != -1)
			{
				throw new ArgumentOutOfRangeException(SR.GetString("net_perm_invalid_val", new object[] { "Port", this.m_port }));
			}
			perm.AddPermission(networkAccess, transportType, this.m_host, num);
		}

		// Token: 0x0400220B RID: 8715
		private const string strAccess = "Access";

		// Token: 0x0400220C RID: 8716
		private const string strConnect = "Connect";

		// Token: 0x0400220D RID: 8717
		private const string strAccept = "Accept";

		// Token: 0x0400220E RID: 8718
		private const string strHost = "Host";

		// Token: 0x0400220F RID: 8719
		private const string strTransport = "Transport";

		// Token: 0x04002210 RID: 8720
		private const string strPort = "Port";

		// Token: 0x04002211 RID: 8721
		private string m_access;

		// Token: 0x04002212 RID: 8722
		private string m_host;

		// Token: 0x04002213 RID: 8723
		private string m_port;

		// Token: 0x04002214 RID: 8724
		private string m_transport;
	}
}
