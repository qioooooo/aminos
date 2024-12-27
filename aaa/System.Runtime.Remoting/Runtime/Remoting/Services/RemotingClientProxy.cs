using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;

namespace System.Runtime.Remoting.Services
{
	// Token: 0x020000B6 RID: 182
	[ComVisible(true)]
	public abstract class RemotingClientProxy : Component
	{
		// Token: 0x06000576 RID: 1398 RVA: 0x0001FE18 File Offset: 0x0001EE18
		protected void ConfigureProxy(Type type, string url)
		{
			lock (this)
			{
				this._type = type;
				this.Url = url;
			}
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x0001FE54 File Offset: 0x0001EE54
		protected void ConnectProxy()
		{
			lock (this)
			{
				this._tp = null;
				this._tp = Activator.GetObject(this._type, this._url);
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000578 RID: 1400 RVA: 0x0001FEA0 File Offset: 0x0001EEA0
		// (set) Token: 0x06000579 RID: 1401 RVA: 0x0001FEBC File Offset: 0x0001EEBC
		public bool AllowAutoRedirect
		{
			get
			{
				return (bool)ChannelServices.GetChannelSinkProperties(this._tp)["allowautoredirect"];
			}
			set
			{
				ChannelServices.GetChannelSinkProperties(this._tp)["allowautoredirect"] = value;
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x0600057A RID: 1402 RVA: 0x0001FED9 File Offset: 0x0001EED9
		public object Cookies
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x0600057B RID: 1403 RVA: 0x0001FEDC File Offset: 0x0001EEDC
		// (set) Token: 0x0600057C RID: 1404 RVA: 0x0001FEDF File Offset: 0x0001EEDF
		public bool EnableCookies
		{
			get
			{
				return false;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x0600057D RID: 1405 RVA: 0x0001FEE6 File Offset: 0x0001EEE6
		// (set) Token: 0x0600057E RID: 1406 RVA: 0x0001FF02 File Offset: 0x0001EF02
		public bool PreAuthenticate
		{
			get
			{
				return (bool)ChannelServices.GetChannelSinkProperties(this._tp)["preauthenticate"];
			}
			set
			{
				ChannelServices.GetChannelSinkProperties(this._tp)["preauthenticate"] = value;
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x0600057F RID: 1407 RVA: 0x0001FF1F File Offset: 0x0001EF1F
		// (set) Token: 0x06000580 RID: 1408 RVA: 0x0001FF27 File Offset: 0x0001EF27
		public string Path
		{
			get
			{
				return this.Url;
			}
			set
			{
				this.Url = value;
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000581 RID: 1409 RVA: 0x0001FF30 File Offset: 0x0001EF30
		// (set) Token: 0x06000582 RID: 1410 RVA: 0x0001FF4C File Offset: 0x0001EF4C
		public int Timeout
		{
			get
			{
				return (int)ChannelServices.GetChannelSinkProperties(this._tp)["timeout"];
			}
			set
			{
				ChannelServices.GetChannelSinkProperties(this._tp)["timeout"] = value;
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000583 RID: 1411 RVA: 0x0001FF69 File Offset: 0x0001EF69
		// (set) Token: 0x06000584 RID: 1412 RVA: 0x0001FF74 File Offset: 0x0001EF74
		public string Url
		{
			get
			{
				return this._url;
			}
			set
			{
				lock (this)
				{
					this._url = value;
				}
				this.ConnectProxy();
				ChannelServices.GetChannelSinkProperties(this._tp)["url"] = value;
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000585 RID: 1413 RVA: 0x0001FFC8 File Offset: 0x0001EFC8
		// (set) Token: 0x06000586 RID: 1414 RVA: 0x0001FFCF File Offset: 0x0001EFCF
		public string UserAgent
		{
			get
			{
				return HttpClientTransportSink.UserAgent;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000587 RID: 1415 RVA: 0x0001FFD6 File Offset: 0x0001EFD6
		// (set) Token: 0x06000588 RID: 1416 RVA: 0x0001FFF2 File Offset: 0x0001EFF2
		public string Username
		{
			get
			{
				return (string)ChannelServices.GetChannelSinkProperties(this._tp)["username"];
			}
			set
			{
				ChannelServices.GetChannelSinkProperties(this._tp)["username"] = value;
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000589 RID: 1417 RVA: 0x0002000A File Offset: 0x0001F00A
		// (set) Token: 0x0600058A RID: 1418 RVA: 0x00020026 File Offset: 0x0001F026
		public string Password
		{
			get
			{
				return (string)ChannelServices.GetChannelSinkProperties(this._tp)["password"];
			}
			set
			{
				ChannelServices.GetChannelSinkProperties(this._tp)["password"] = value;
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x0600058B RID: 1419 RVA: 0x0002003E File Offset: 0x0001F03E
		// (set) Token: 0x0600058C RID: 1420 RVA: 0x0002005A File Offset: 0x0001F05A
		public string Domain
		{
			get
			{
				return (string)ChannelServices.GetChannelSinkProperties(this._tp)["domain"];
			}
			set
			{
				ChannelServices.GetChannelSinkProperties(this._tp)["domain"] = value;
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x0600058D RID: 1421 RVA: 0x00020072 File Offset: 0x0001F072
		// (set) Token: 0x0600058E RID: 1422 RVA: 0x0002008E File Offset: 0x0001F08E
		public string ProxyName
		{
			get
			{
				return (string)ChannelServices.GetChannelSinkProperties(this._tp)["proxyname"];
			}
			set
			{
				ChannelServices.GetChannelSinkProperties(this._tp)["Proxyname"] = value;
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x0600058F RID: 1423 RVA: 0x000200A6 File Offset: 0x0001F0A6
		// (set) Token: 0x06000590 RID: 1424 RVA: 0x000200C2 File Offset: 0x0001F0C2
		public int ProxyPort
		{
			get
			{
				return (int)ChannelServices.GetChannelSinkProperties(this._tp)["proxyport"];
			}
			set
			{
				ChannelServices.GetChannelSinkProperties(this._tp)["proxyport"] = value;
			}
		}

		// Token: 0x040004AB RID: 1195
		protected Type _type;

		// Token: 0x040004AC RID: 1196
		protected object _tp;

		// Token: 0x040004AD RID: 1197
		protected string _url;
	}
}
