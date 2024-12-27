using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Net.NetworkInformation;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text.RegularExpressions;

namespace System.Net
{
	// Token: 0x020004AC RID: 1196
	[Serializable]
	public class WebProxy : IAutoWebProxy, IWebProxy, ISerializable
	{
		// Token: 0x060024A1 RID: 9377 RVA: 0x0009108E File Offset: 0x0009008E
		public WebProxy()
			: this(null, false, null, null)
		{
		}

		// Token: 0x060024A2 RID: 9378 RVA: 0x0009109A File Offset: 0x0009009A
		public WebProxy(Uri Address)
			: this(Address, false, null, null)
		{
		}

		// Token: 0x060024A3 RID: 9379 RVA: 0x000910A6 File Offset: 0x000900A6
		public WebProxy(Uri Address, bool BypassOnLocal)
			: this(Address, BypassOnLocal, null, null)
		{
		}

		// Token: 0x060024A4 RID: 9380 RVA: 0x000910B2 File Offset: 0x000900B2
		public WebProxy(Uri Address, bool BypassOnLocal, string[] BypassList)
			: this(Address, BypassOnLocal, BypassList, null)
		{
		}

		// Token: 0x060024A5 RID: 9381 RVA: 0x000910BE File Offset: 0x000900BE
		internal WebProxy(Hashtable proxyHostAddresses, bool BypassOnLocal, string[] BypassList)
			: this(null, BypassOnLocal, BypassList, null)
		{
			this._ProxyHostAddresses = proxyHostAddresses;
			if (this._ProxyHostAddresses != null)
			{
				this._ProxyAddress = (Uri)proxyHostAddresses["http"];
			}
		}

		// Token: 0x060024A6 RID: 9382 RVA: 0x000910EF File Offset: 0x000900EF
		public WebProxy(Uri Address, bool BypassOnLocal, string[] BypassList, ICredentials Credentials)
		{
			this._ProxyAddress = Address;
			this._BypassOnLocal = BypassOnLocal;
			if (BypassList != null)
			{
				this._BypassList = new ArrayList(BypassList);
				this.UpdateRegExList(true);
			}
			this._Credentials = Credentials;
			this.m_EnableAutoproxy = true;
		}

		// Token: 0x060024A7 RID: 9383 RVA: 0x0009112A File Offset: 0x0009012A
		public WebProxy(string Host, int Port)
			: this(new Uri("http://" + Host + ":" + Port.ToString(CultureInfo.InvariantCulture)), false, null, null)
		{
		}

		// Token: 0x060024A8 RID: 9384 RVA: 0x00091156 File Offset: 0x00090156
		public WebProxy(string Address)
			: this(WebProxy.CreateProxyUri(Address), false, null, null)
		{
		}

		// Token: 0x060024A9 RID: 9385 RVA: 0x00091167 File Offset: 0x00090167
		public WebProxy(string Address, bool BypassOnLocal)
			: this(WebProxy.CreateProxyUri(Address), BypassOnLocal, null, null)
		{
		}

		// Token: 0x060024AA RID: 9386 RVA: 0x00091178 File Offset: 0x00090178
		public WebProxy(string Address, bool BypassOnLocal, string[] BypassList)
			: this(WebProxy.CreateProxyUri(Address), BypassOnLocal, BypassList, null)
		{
		}

		// Token: 0x060024AB RID: 9387 RVA: 0x00091189 File Offset: 0x00090189
		public WebProxy(string Address, bool BypassOnLocal, string[] BypassList, ICredentials Credentials)
			: this(WebProxy.CreateProxyUri(Address), BypassOnLocal, BypassList, Credentials)
		{
		}

		// Token: 0x17000796 RID: 1942
		// (get) Token: 0x060024AC RID: 9388 RVA: 0x0009119B File Offset: 0x0009019B
		// (set) Token: 0x060024AD RID: 9389 RVA: 0x000911A9 File Offset: 0x000901A9
		public Uri Address
		{
			get
			{
				this.CheckForChanges();
				return this._ProxyAddress;
			}
			set
			{
				this._UseRegistry = false;
				this.DeleteScriptEngine();
				this._ProxyHostAddresses = null;
				this._ProxyAddress = value;
			}
		}

		// Token: 0x17000797 RID: 1943
		// (set) Token: 0x060024AE RID: 9390 RVA: 0x000911C6 File Offset: 0x000901C6
		internal bool AutoDetect
		{
			set
			{
				if (this.ScriptEngine == null)
				{
					this.ScriptEngine = new AutoWebProxyScriptEngine(this, false);
				}
				this.ScriptEngine.AutomaticallyDetectSettings = value;
			}
		}

		// Token: 0x17000798 RID: 1944
		// (set) Token: 0x060024AF RID: 9391 RVA: 0x000911E9 File Offset: 0x000901E9
		internal Uri ScriptLocation
		{
			set
			{
				if (this.ScriptEngine == null)
				{
					this.ScriptEngine = new AutoWebProxyScriptEngine(this, false);
				}
				this.ScriptEngine.AutomaticConfigurationScript = value;
			}
		}

		// Token: 0x17000799 RID: 1945
		// (get) Token: 0x060024B0 RID: 9392 RVA: 0x0009120C File Offset: 0x0009020C
		// (set) Token: 0x060024B1 RID: 9393 RVA: 0x0009121A File Offset: 0x0009021A
		public bool BypassProxyOnLocal
		{
			get
			{
				this.CheckForChanges();
				return this._BypassOnLocal;
			}
			set
			{
				this._UseRegistry = false;
				this.DeleteScriptEngine();
				this._BypassOnLocal = value;
			}
		}

		// Token: 0x1700079A RID: 1946
		// (get) Token: 0x060024B2 RID: 9394 RVA: 0x00091230 File Offset: 0x00090230
		// (set) Token: 0x060024B3 RID: 9395 RVA: 0x00091265 File Offset: 0x00090265
		public string[] BypassList
		{
			get
			{
				this.CheckForChanges();
				if (this._BypassList == null)
				{
					this._BypassList = new ArrayList();
				}
				return (string[])this._BypassList.ToArray(typeof(string));
			}
			set
			{
				this._UseRegistry = false;
				this.DeleteScriptEngine();
				this._BypassList = new ArrayList(value);
				this.UpdateRegExList(true);
			}
		}

		// Token: 0x1700079B RID: 1947
		// (get) Token: 0x060024B4 RID: 9396 RVA: 0x00091287 File Offset: 0x00090287
		// (set) Token: 0x060024B5 RID: 9397 RVA: 0x0009128F File Offset: 0x0009028F
		public ICredentials Credentials
		{
			get
			{
				return this._Credentials;
			}
			set
			{
				this._Credentials = value;
			}
		}

		// Token: 0x1700079C RID: 1948
		// (get) Token: 0x060024B6 RID: 9398 RVA: 0x00091298 File Offset: 0x00090298
		// (set) Token: 0x060024B7 RID: 9399 RVA: 0x000912AA File Offset: 0x000902AA
		public bool UseDefaultCredentials
		{
			get
			{
				return this.Credentials is SystemNetworkCredential;
			}
			set
			{
				this._Credentials = (value ? CredentialCache.DefaultCredentials : null);
			}
		}

		// Token: 0x1700079D RID: 1949
		// (get) Token: 0x060024B8 RID: 9400 RVA: 0x000912BD File Offset: 0x000902BD
		public ArrayList BypassArrayList
		{
			get
			{
				this.CheckForChanges();
				if (this._BypassList == null)
				{
					this._BypassList = new ArrayList();
				}
				return this._BypassList;
			}
		}

		// Token: 0x060024B9 RID: 9401 RVA: 0x000912DE File Offset: 0x000902DE
		internal void CheckForChanges()
		{
			if (this.ScriptEngine != null)
			{
				this.ScriptEngine.CheckForChanges();
			}
		}

		// Token: 0x060024BA RID: 9402 RVA: 0x000912F4 File Offset: 0x000902F4
		public Uri GetProxy(Uri destination)
		{
			if (destination == null)
			{
				throw new ArgumentNullException("destination");
			}
			Uri uri;
			if (this.GetProxyAuto(destination, out uri))
			{
				return uri;
			}
			if (this.IsBypassedManual(destination))
			{
				return destination;
			}
			Hashtable proxyHostAddresses = this._ProxyHostAddresses;
			Uri uri2 = ((proxyHostAddresses != null) ? (proxyHostAddresses[destination.Scheme] as Uri) : this._ProxyAddress);
			if (!(uri2 != null))
			{
				return destination;
			}
			return uri2;
		}

		// Token: 0x060024BB RID: 9403 RVA: 0x0009135D File Offset: 0x0009035D
		private static Uri CreateProxyUri(string address)
		{
			if (address == null)
			{
				return null;
			}
			if (address.IndexOf("://") == -1)
			{
				address = "http://" + address;
			}
			return new Uri(address);
		}

		// Token: 0x060024BC RID: 9404 RVA: 0x00091388 File Offset: 0x00090388
		private void UpdateRegExList(bool canThrow)
		{
			Regex[] array = null;
			ArrayList bypassList = this._BypassList;
			try
			{
				if (bypassList != null && bypassList.Count > 0)
				{
					array = new Regex[bypassList.Count];
					for (int i = 0; i < bypassList.Count; i++)
					{
						array[i] = new Regex((string)bypassList[i], RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
					}
				}
			}
			catch
			{
				if (!canThrow)
				{
					this._RegExBypassList = null;
					return;
				}
				throw;
			}
			this._RegExBypassList = array;
		}

		// Token: 0x060024BD RID: 9405 RVA: 0x00091408 File Offset: 0x00090408
		private bool IsMatchInBypassList(Uri input)
		{
			this.UpdateRegExList(false);
			if (this._RegExBypassList == null)
			{
				return false;
			}
			string text = input.Scheme + "://" + input.Host + ((!input.IsDefaultPort) ? (":" + input.Port) : "");
			for (int i = 0; i < this._BypassList.Count; i++)
			{
				if (this._RegExBypassList[i].IsMatch(text))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060024BE RID: 9406 RVA: 0x0009148C File Offset: 0x0009048C
		private bool IsLocal(Uri host)
		{
			string host2 = host.Host;
			int num = -1;
			bool flag = true;
			bool flag2 = false;
			for (int i = 0; i < host2.Length; i++)
			{
				if (host2[i] == '.')
				{
					if (num == -1)
					{
						num = i;
						if (!flag)
						{
							break;
						}
					}
				}
				else
				{
					if (host2[i] == ':')
					{
						flag2 = true;
						flag = false;
						break;
					}
					if (host2[i] < '0' || host2[i] > '9')
					{
						flag = false;
						if (num != -1)
						{
							break;
						}
					}
				}
			}
			if (num == -1 && !flag2)
			{
				return true;
			}
			if (!flag)
			{
				if (!flag2)
				{
					goto IL_009D;
				}
			}
			try
			{
				IPAddress ipaddress = IPAddress.Parse(host2);
				if (IPAddress.IsLoopback(ipaddress))
				{
					return true;
				}
				return NclUtilities.IsAddressLocal(ipaddress);
			}
			catch (FormatException)
			{
			}
			IL_009D:
			string text = "." + IPGlobalProperties.InternalGetIPGlobalProperties().DomainName;
			return text != null && text.Length == host2.Length - num && string.Compare(text, 0, host2, num, text.Length, StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x060024BF RID: 9407 RVA: 0x0009158C File Offset: 0x0009058C
		private bool IsLocalInProxyHash(Uri host)
		{
			Hashtable proxyHostAddresses = this._ProxyHostAddresses;
			if (proxyHostAddresses != null)
			{
				Uri uri = (Uri)proxyHostAddresses[host.Scheme];
				if (uri == null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060024C0 RID: 9408 RVA: 0x000915C4 File Offset: 0x000905C4
		public bool IsBypassed(Uri host)
		{
			if (host == null)
			{
				throw new ArgumentNullException("host");
			}
			bool flag;
			if (this.IsBypassedAuto(host, out flag))
			{
				return flag;
			}
			return this.IsBypassedManual(host);
		}

		// Token: 0x060024C1 RID: 9409 RVA: 0x000915FC File Offset: 0x000905FC
		private bool IsBypassedManual(Uri host)
		{
			return host.IsLoopback || (this._ProxyAddress == null && this._ProxyHostAddresses == null) || (this._BypassOnLocal && this.IsLocal(host)) || this.IsMatchInBypassList(host) || this.IsLocalInProxyHash(host);
		}

		// Token: 0x060024C2 RID: 9410 RVA: 0x0009164C File Offset: 0x0009064C
		[Obsolete("This method has been deprecated. Please use the proxy selected for you by default. http://go.microsoft.com/fwlink/?linkid=14202")]
		public static WebProxy GetDefaultProxy()
		{
			ExceptionHelper.WebPermissionUnrestricted.Demand();
			return new WebProxy(true);
		}

		// Token: 0x060024C3 RID: 9411 RVA: 0x00091660 File Offset: 0x00090660
		protected WebProxy(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			bool flag = false;
			try
			{
				flag = serializationInfo.GetBoolean("_UseRegistry");
			}
			catch
			{
			}
			if (flag)
			{
				ExceptionHelper.WebPermissionUnrestricted.Demand();
				this.UnsafeUpdateFromRegistry();
				return;
			}
			this._ProxyAddress = (Uri)serializationInfo.GetValue("_ProxyAddress", typeof(Uri));
			this._BypassOnLocal = serializationInfo.GetBoolean("_BypassOnLocal");
			this._BypassList = (ArrayList)serializationInfo.GetValue("_BypassList", typeof(ArrayList));
			try
			{
				this.UseDefaultCredentials = serializationInfo.GetBoolean("_UseDefaultCredentials");
			}
			catch
			{
			}
		}

		// Token: 0x060024C4 RID: 9412 RVA: 0x00091720 File Offset: 0x00090720
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter, SerializationFormatter = true)]
		void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			this.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x060024C5 RID: 9413 RVA: 0x0009172C File Offset: 0x0009072C
		[SecurityPermission(SecurityAction.LinkDemand, SerializationFormatter = true)]
		protected virtual void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			serializationInfo.AddValue("_BypassOnLocal", this._BypassOnLocal);
			serializationInfo.AddValue("_ProxyAddress", this._ProxyAddress);
			serializationInfo.AddValue("_BypassList", this._BypassList);
			serializationInfo.AddValue("_UseDefaultCredentials", this.UseDefaultCredentials);
			if (this._UseRegistry)
			{
				serializationInfo.AddValue("_UseRegistry", true);
			}
		}

		// Token: 0x1700079E RID: 1950
		// (get) Token: 0x060024C6 RID: 9414 RVA: 0x00091791 File Offset: 0x00090791
		// (set) Token: 0x060024C7 RID: 9415 RVA: 0x00091799 File Offset: 0x00090799
		internal AutoWebProxyScriptEngine ScriptEngine
		{
			get
			{
				return this.m_ScriptEngine;
			}
			set
			{
				this.m_ScriptEngine = value;
			}
		}

		// Token: 0x060024C8 RID: 9416 RVA: 0x000917A2 File Offset: 0x000907A2
		internal WebProxy(bool enableAutoproxy)
		{
			this.m_EnableAutoproxy = enableAutoproxy;
			this.UnsafeUpdateFromRegistry();
		}

		// Token: 0x060024C9 RID: 9417 RVA: 0x000917B7 File Offset: 0x000907B7
		internal void DeleteScriptEngine()
		{
			if (this.ScriptEngine != null)
			{
				this.ScriptEngine.Close();
				this.ScriptEngine = null;
			}
		}

		// Token: 0x060024CA RID: 9418 RVA: 0x000917D4 File Offset: 0x000907D4
		internal void UnsafeUpdateFromRegistry()
		{
			this._UseRegistry = true;
			this.ScriptEngine = new AutoWebProxyScriptEngine(this, true);
			WebProxyData webProxyData = this.ScriptEngine.GetWebProxyData();
			this.Update(webProxyData);
		}

		// Token: 0x060024CB RID: 9419 RVA: 0x00091808 File Offset: 0x00090808
		internal void Update(WebProxyData webProxyData)
		{
			lock (this)
			{
				this._BypassOnLocal = webProxyData.bypassOnLocal;
				this._ProxyAddress = webProxyData.proxyAddress;
				this._BypassList = webProxyData.bypassList;
				this.ScriptEngine.AutomaticallyDetectSettings = this.m_EnableAutoproxy && webProxyData.automaticallyDetectSettings;
				this.ScriptEngine.AutomaticConfigurationScript = (this.m_EnableAutoproxy ? webProxyData.scriptLocation : null);
			}
		}

		// Token: 0x060024CC RID: 9420 RVA: 0x00091894 File Offset: 0x00090894
		ProxyChain IAutoWebProxy.GetProxies(Uri destination)
		{
			if (destination == null)
			{
				throw new ArgumentNullException("destination");
			}
			return new ProxyScriptChain(this, destination);
		}

		// Token: 0x060024CD RID: 9421 RVA: 0x000918B4 File Offset: 0x000908B4
		private bool GetProxyAuto(Uri destination, out Uri proxyUri)
		{
			proxyUri = null;
			if (this.ScriptEngine == null)
			{
				return false;
			}
			IList<string> list = null;
			if (!this.ScriptEngine.GetProxies(destination, out list))
			{
				return false;
			}
			if (list.Count > 0)
			{
				if (WebProxy.AreAllBypassed(list, true))
				{
					proxyUri = destination;
				}
				else
				{
					proxyUri = WebProxy.ProxyUri(list[0]);
				}
			}
			return true;
		}

		// Token: 0x060024CE RID: 9422 RVA: 0x00091908 File Offset: 0x00090908
		private bool IsBypassedAuto(Uri destination, out bool isBypassed)
		{
			isBypassed = true;
			if (this.ScriptEngine == null)
			{
				return false;
			}
			IList<string> list;
			if (!this.ScriptEngine.GetProxies(destination, out list))
			{
				return false;
			}
			if (list.Count == 0)
			{
				isBypassed = false;
			}
			else
			{
				isBypassed = WebProxy.AreAllBypassed(list, true);
			}
			return true;
		}

		// Token: 0x060024CF RID: 9423 RVA: 0x0009194C File Offset: 0x0009094C
		internal Uri[] GetProxiesAuto(Uri destination, ref int syncStatus)
		{
			if (this.ScriptEngine == null)
			{
				return null;
			}
			IList<string> list = null;
			if (!this.ScriptEngine.GetProxies(destination, out list, ref syncStatus))
			{
				return null;
			}
			Uri[] array;
			if (list.Count == 0)
			{
				array = new Uri[0];
			}
			else if (WebProxy.AreAllBypassed(list, false))
			{
				Uri[] array2 = new Uri[1];
				array = array2;
			}
			else
			{
				array = new Uri[list.Count];
				for (int i = 0; i < list.Count; i++)
				{
					array[i] = WebProxy.ProxyUri(list[i]);
				}
			}
			return array;
		}

		// Token: 0x060024D0 RID: 9424 RVA: 0x000919CC File Offset: 0x000909CC
		internal void AbortGetProxiesAuto(ref int syncStatus)
		{
			if (this.ScriptEngine != null)
			{
				this.ScriptEngine.Abort(ref syncStatus);
			}
		}

		// Token: 0x060024D1 RID: 9425 RVA: 0x000919E4 File Offset: 0x000909E4
		internal Uri GetProxyAutoFailover(Uri destination)
		{
			if (this.IsBypassedManual(destination))
			{
				return null;
			}
			Uri uri = this._ProxyAddress;
			Hashtable proxyHostAddresses = this._ProxyHostAddresses;
			if (proxyHostAddresses != null)
			{
				uri = proxyHostAddresses[destination.Scheme] as Uri;
			}
			return uri;
		}

		// Token: 0x060024D2 RID: 9426 RVA: 0x00091A20 File Offset: 0x00090A20
		private static bool AreAllBypassed(IEnumerable<string> proxies, bool checkFirstOnly)
		{
			bool flag = true;
			foreach (string text in proxies)
			{
				flag = string.IsNullOrEmpty(text);
				if (checkFirstOnly || !flag)
				{
					break;
				}
			}
			return flag;
		}

		// Token: 0x060024D3 RID: 9427 RVA: 0x00091A74 File Offset: 0x00090A74
		private static Uri ProxyUri(string proxyName)
		{
			if (proxyName != null && proxyName.Length != 0)
			{
				return new Uri("http://" + proxyName);
			}
			return null;
		}

		// Token: 0x040024CD RID: 9421
		private bool _UseRegistry;

		// Token: 0x040024CE RID: 9422
		private bool _BypassOnLocal;

		// Token: 0x040024CF RID: 9423
		private bool m_EnableAutoproxy;

		// Token: 0x040024D0 RID: 9424
		private Uri _ProxyAddress;

		// Token: 0x040024D1 RID: 9425
		private ArrayList _BypassList;

		// Token: 0x040024D2 RID: 9426
		private ICredentials _Credentials;

		// Token: 0x040024D3 RID: 9427
		private Regex[] _RegExBypassList;

		// Token: 0x040024D4 RID: 9428
		private Hashtable _ProxyHostAddresses;

		// Token: 0x040024D5 RID: 9429
		private AutoWebProxyScriptEngine m_ScriptEngine;
	}
}
