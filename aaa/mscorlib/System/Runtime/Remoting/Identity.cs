using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Threading;

namespace System.Runtime.Remoting
{
	// Token: 0x020006E7 RID: 1767
	internal class Identity
	{
		// Token: 0x17000AC5 RID: 2757
		// (get) Token: 0x06003F4A RID: 16202 RVA: 0x000D88E2 File Offset: 0x000D78E2
		internal static string ProcessIDGuid
		{
			get
			{
				return SharedStatics.Remoting_Identity_IDGuid;
			}
		}

		// Token: 0x17000AC6 RID: 2758
		// (get) Token: 0x06003F4B RID: 16203 RVA: 0x000D88E9 File Offset: 0x000D78E9
		internal static string AppDomainUniqueId
		{
			get
			{
				if (Identity.s_configuredAppDomainGuid != null)
				{
					return Identity.s_configuredAppDomainGuid;
				}
				return Identity.s_originalAppDomainGuid;
			}
		}

		// Token: 0x17000AC7 RID: 2759
		// (get) Token: 0x06003F4C RID: 16204 RVA: 0x000D88FD File Offset: 0x000D78FD
		internal static string IDGuidString
		{
			get
			{
				return Identity.s_IDGuidString;
			}
		}

		// Token: 0x06003F4D RID: 16205 RVA: 0x000D8904 File Offset: 0x000D7904
		internal static string RemoveAppNameOrAppGuidIfNecessary(string uri)
		{
			if (uri == null || uri.Length <= 1 || uri[0] != '/')
			{
				return uri;
			}
			string text;
			if (Identity.s_configuredAppDomainGuidString != null)
			{
				text = Identity.s_configuredAppDomainGuidString;
				if (uri.Length > text.Length && Identity.StringStartsWith(uri, text))
				{
					return uri.Substring(text.Length);
				}
			}
			text = Identity.s_originalAppDomainGuidString;
			if (uri.Length > text.Length && Identity.StringStartsWith(uri, text))
			{
				return uri.Substring(text.Length);
			}
			string applicationName = RemotingConfiguration.ApplicationName;
			if (applicationName != null && uri.Length > applicationName.Length + 2 && string.Compare(uri, 1, applicationName, 0, applicationName.Length, true, CultureInfo.InvariantCulture) == 0 && uri[applicationName.Length + 1] == '/')
			{
				return uri.Substring(applicationName.Length + 2);
			}
			uri = uri.Substring(1);
			return uri;
		}

		// Token: 0x06003F4E RID: 16206 RVA: 0x000D89E0 File Offset: 0x000D79E0
		private static bool StringStartsWith(string s1, string prefix)
		{
			return s1.Length >= prefix.Length && string.CompareOrdinal(s1, 0, prefix, 0, prefix.Length) == 0;
		}

		// Token: 0x17000AC8 RID: 2760
		// (get) Token: 0x06003F4F RID: 16207 RVA: 0x000D8A04 File Offset: 0x000D7A04
		internal static string ProcessGuid
		{
			get
			{
				return Identity.ProcessIDGuid;
			}
		}

		// Token: 0x06003F50 RID: 16208 RVA: 0x000D8A0B File Offset: 0x000D7A0B
		private static int GetNextSeqNum()
		{
			return SharedStatics.Remoting_Identity_GetNextSeqNum();
		}

		// Token: 0x06003F51 RID: 16209 RVA: 0x000D8A14 File Offset: 0x000D7A14
		private static byte[] GetRandomBytes()
		{
			byte[] array = new byte[18];
			Identity.s_rng.GetBytes(array);
			return array;
		}

		// Token: 0x06003F52 RID: 16210 RVA: 0x000D8A35 File Offset: 0x000D7A35
		internal Identity(string objURI, string URL)
		{
			if (URL != null)
			{
				this._flags |= 256;
				this._URL = URL;
			}
			this.SetOrCreateURI(objURI, true);
		}

		// Token: 0x06003F53 RID: 16211 RVA: 0x000D8A61 File Offset: 0x000D7A61
		internal Identity(bool bContextBound)
		{
			if (bContextBound)
			{
				this._flags |= 16;
			}
		}

		// Token: 0x17000AC9 RID: 2761
		// (get) Token: 0x06003F54 RID: 16212 RVA: 0x000D8A7B File Offset: 0x000D7A7B
		internal bool IsContextBound
		{
			get
			{
				return (this._flags & 16) == 16;
			}
		}

		// Token: 0x06003F55 RID: 16213 RVA: 0x000D8A8A File Offset: 0x000D7A8A
		internal bool IsWellKnown()
		{
			return (this._flags & 256) == 256;
		}

		// Token: 0x06003F56 RID: 16214 RVA: 0x000D8AA0 File Offset: 0x000D7AA0
		internal void SetInIDTable()
		{
			int flags;
			int num;
			do
			{
				flags = this._flags;
				num = this._flags | 4;
			}
			while (flags != Interlocked.CompareExchange(ref this._flags, num, flags));
		}

		// Token: 0x06003F57 RID: 16215 RVA: 0x000D8AD0 File Offset: 0x000D7AD0
		internal void ResetInIDTable(bool bResetURI)
		{
			int flags;
			int num;
			do
			{
				flags = this._flags;
				num = this._flags & -5;
			}
			while (flags != Interlocked.CompareExchange(ref this._flags, num, flags));
			if (bResetURI)
			{
				((ObjRef)this._objRef).URI = null;
				this._ObjURI = null;
			}
		}

		// Token: 0x06003F58 RID: 16216 RVA: 0x000D8B19 File Offset: 0x000D7B19
		internal bool IsInIDTable()
		{
			return (this._flags & 4) == 4;
		}

		// Token: 0x06003F59 RID: 16217 RVA: 0x000D8B28 File Offset: 0x000D7B28
		internal void SetFullyConnected()
		{
			int flags;
			int num;
			do
			{
				flags = this._flags;
				num = this._flags & -4;
			}
			while (flags != Interlocked.CompareExchange(ref this._flags, num, flags));
		}

		// Token: 0x06003F5A RID: 16218 RVA: 0x000D8B56 File Offset: 0x000D7B56
		internal bool IsFullyDisconnected()
		{
			return (this._flags & 1) == 1;
		}

		// Token: 0x06003F5B RID: 16219 RVA: 0x000D8B63 File Offset: 0x000D7B63
		internal bool IsRemoteDisconnected()
		{
			return (this._flags & 2) == 2;
		}

		// Token: 0x06003F5C RID: 16220 RVA: 0x000D8B70 File Offset: 0x000D7B70
		internal bool IsDisconnected()
		{
			return this.IsFullyDisconnected() || this.IsRemoteDisconnected();
		}

		// Token: 0x17000ACA RID: 2762
		// (get) Token: 0x06003F5D RID: 16221 RVA: 0x000D8B82 File Offset: 0x000D7B82
		internal string URI
		{
			get
			{
				if (this.IsWellKnown())
				{
					return this._URL;
				}
				return this._ObjURI;
			}
		}

		// Token: 0x17000ACB RID: 2763
		// (get) Token: 0x06003F5E RID: 16222 RVA: 0x000D8B99 File Offset: 0x000D7B99
		internal string ObjURI
		{
			get
			{
				return this._ObjURI;
			}
		}

		// Token: 0x17000ACC RID: 2764
		// (get) Token: 0x06003F5F RID: 16223 RVA: 0x000D8BA1 File Offset: 0x000D7BA1
		internal MarshalByRefObject TPOrObject
		{
			get
			{
				return (MarshalByRefObject)this._tpOrObject;
			}
		}

		// Token: 0x06003F60 RID: 16224 RVA: 0x000D8BAE File Offset: 0x000D7BAE
		internal object RaceSetTransparentProxy(object tpObj)
		{
			if (this._tpOrObject == null)
			{
				Interlocked.CompareExchange(ref this._tpOrObject, tpObj, null);
			}
			return this._tpOrObject;
		}

		// Token: 0x17000ACD RID: 2765
		// (get) Token: 0x06003F61 RID: 16225 RVA: 0x000D8BCC File Offset: 0x000D7BCC
		internal ObjRef ObjectRef
		{
			get
			{
				return (ObjRef)this._objRef;
			}
		}

		// Token: 0x06003F62 RID: 16226 RVA: 0x000D8BD9 File Offset: 0x000D7BD9
		internal ObjRef RaceSetObjRef(ObjRef objRefGiven)
		{
			if (this._objRef == null)
			{
				Interlocked.CompareExchange(ref this._objRef, objRefGiven, null);
			}
			return (ObjRef)this._objRef;
		}

		// Token: 0x17000ACE RID: 2766
		// (get) Token: 0x06003F63 RID: 16227 RVA: 0x000D8BFC File Offset: 0x000D7BFC
		internal IMessageSink ChannelSink
		{
			get
			{
				return (IMessageSink)this._channelSink;
			}
		}

		// Token: 0x06003F64 RID: 16228 RVA: 0x000D8C09 File Offset: 0x000D7C09
		internal IMessageSink RaceSetChannelSink(IMessageSink channelSink)
		{
			if (this._channelSink == null)
			{
				Interlocked.CompareExchange(ref this._channelSink, channelSink, null);
			}
			return (IMessageSink)this._channelSink;
		}

		// Token: 0x17000ACF RID: 2767
		// (get) Token: 0x06003F65 RID: 16229 RVA: 0x000D8C2C File Offset: 0x000D7C2C
		internal IMessageSink EnvoyChain
		{
			get
			{
				return (IMessageSink)this._envoyChain;
			}
		}

		// Token: 0x17000AD0 RID: 2768
		// (get) Token: 0x06003F66 RID: 16230 RVA: 0x000D8C39 File Offset: 0x000D7C39
		// (set) Token: 0x06003F67 RID: 16231 RVA: 0x000D8C41 File Offset: 0x000D7C41
		internal Lease Lease
		{
			get
			{
				return this._lease;
			}
			set
			{
				this._lease = value;
			}
		}

		// Token: 0x06003F68 RID: 16232 RVA: 0x000D8C4A File Offset: 0x000D7C4A
		internal IMessageSink RaceSetEnvoyChain(IMessageSink envoyChain)
		{
			if (this._envoyChain == null)
			{
				Interlocked.CompareExchange(ref this._envoyChain, envoyChain, null);
			}
			return (IMessageSink)this._envoyChain;
		}

		// Token: 0x06003F69 RID: 16233 RVA: 0x000D8C6D File Offset: 0x000D7C6D
		internal void SetOrCreateURI(string uri)
		{
			this.SetOrCreateURI(uri, false);
		}

		// Token: 0x06003F6A RID: 16234 RVA: 0x000D8C78 File Offset: 0x000D7C78
		internal void SetOrCreateURI(string uri, bool bIdCtor)
		{
			if (!bIdCtor && this._ObjURI != null)
			{
				throw new RemotingException(Environment.GetResourceString("Remoting_SetObjectUriForMarshal__UriExists"));
			}
			if (uri == null)
			{
				string text = Convert.ToBase64String(Identity.GetRandomBytes());
				this._ObjURI = string.Concat(new object[]
				{
					Identity.IDGuidString,
					text.Replace('/', '_'),
					"_",
					Identity.GetNextSeqNum(),
					".rem"
				}).ToLower(CultureInfo.InvariantCulture);
				return;
			}
			if (this is ServerIdentity)
			{
				this._ObjURI = Identity.IDGuidString + uri;
				return;
			}
			this._ObjURI = uri;
		}

		// Token: 0x06003F6B RID: 16235 RVA: 0x000D8D1F File Offset: 0x000D7D1F
		internal static string GetNewLogicalCallID()
		{
			return Identity.IDGuidString + Identity.GetNextSeqNum();
		}

		// Token: 0x06003F6C RID: 16236 RVA: 0x000D8D35 File Offset: 0x000D7D35
		[Conditional("_DEBUG")]
		internal virtual void AssertValid()
		{
			if (this.URI != null)
			{
				IdentityHolder.ResolveIdentity(this.URI);
			}
		}

		// Token: 0x06003F6D RID: 16237 RVA: 0x000D8D4C File Offset: 0x000D7D4C
		internal bool AddProxySideDynamicProperty(IDynamicProperty prop)
		{
			bool flag;
			lock (this)
			{
				if (this._dph == null)
				{
					DynamicPropertyHolder dynamicPropertyHolder = new DynamicPropertyHolder();
					lock (this)
					{
						if (this._dph == null)
						{
							this._dph = dynamicPropertyHolder;
						}
					}
				}
				flag = this._dph.AddDynamicProperty(prop);
			}
			return flag;
		}

		// Token: 0x06003F6E RID: 16238 RVA: 0x000D8DC4 File Offset: 0x000D7DC4
		internal bool RemoveProxySideDynamicProperty(string name)
		{
			bool flag;
			lock (this)
			{
				if (this._dph == null)
				{
					throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Contexts_NoProperty"), new object[] { name }));
				}
				flag = this._dph.RemoveDynamicProperty(name);
			}
			return flag;
		}

		// Token: 0x17000AD1 RID: 2769
		// (get) Token: 0x06003F6F RID: 16239 RVA: 0x000D8E30 File Offset: 0x000D7E30
		internal ArrayWithSize ProxySideDynamicSinks
		{
			get
			{
				if (this._dph == null)
				{
					return null;
				}
				return this._dph.DynamicSinks;
			}
		}

		// Token: 0x04001FD2 RID: 8146
		protected const int IDFLG_DISCONNECTED_FULL = 1;

		// Token: 0x04001FD3 RID: 8147
		protected const int IDFLG_DISCONNECTED_REM = 2;

		// Token: 0x04001FD4 RID: 8148
		protected const int IDFLG_IN_IDTABLE = 4;

		// Token: 0x04001FD5 RID: 8149
		protected const int IDFLG_CONTEXT_BOUND = 16;

		// Token: 0x04001FD6 RID: 8150
		protected const int IDFLG_WELLKNOWN = 256;

		// Token: 0x04001FD7 RID: 8151
		protected const int IDFLG_SERVER_SINGLECALL = 512;

		// Token: 0x04001FD8 RID: 8152
		protected const int IDFLG_SERVER_SINGLETON = 1024;

		// Token: 0x04001FD9 RID: 8153
		private static string s_originalAppDomainGuid = Guid.NewGuid().ToString().Replace('-', '_');

		// Token: 0x04001FDA RID: 8154
		private static string s_configuredAppDomainGuid = null;

		// Token: 0x04001FDB RID: 8155
		private static string s_originalAppDomainGuidString = "/" + Identity.s_originalAppDomainGuid.ToLower(CultureInfo.InvariantCulture) + "/";

		// Token: 0x04001FDC RID: 8156
		private static string s_configuredAppDomainGuidString = null;

		// Token: 0x04001FDD RID: 8157
		private static string s_IDGuidString = "/" + Identity.s_originalAppDomainGuid.ToLower(CultureInfo.InvariantCulture) + "/";

		// Token: 0x04001FDE RID: 8158
		private static RNGCryptoServiceProvider s_rng = new RNGCryptoServiceProvider();

		// Token: 0x04001FDF RID: 8159
		internal int _flags;

		// Token: 0x04001FE0 RID: 8160
		internal object _tpOrObject;

		// Token: 0x04001FE1 RID: 8161
		protected string _ObjURI;

		// Token: 0x04001FE2 RID: 8162
		protected string _URL;

		// Token: 0x04001FE3 RID: 8163
		internal object _objRef;

		// Token: 0x04001FE4 RID: 8164
		internal object _channelSink;

		// Token: 0x04001FE5 RID: 8165
		internal object _envoyChain;

		// Token: 0x04001FE6 RID: 8166
		internal DynamicPropertyHolder _dph;

		// Token: 0x04001FE7 RID: 8167
		internal Lease _lease;
	}
}
