using System;
using System.Net.Sockets;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using System.Text;
using Microsoft.JScript.Vsa;
using Microsoft.Vsa;

namespace System.Net
{
	// Token: 0x02000003 RID: 3
	internal class VsaWebProxyScript : MarshalByRefObject, IWebProxyScript
	{
		// Token: 0x06000001 RID: 1 RVA: 0x000020D0 File Offset: 0x000010D0
		public void Close()
		{
			if (this.startupInstance != null)
			{
				try
				{
					this.startupInstance.Shutdown();
					this.startupInstance = null;
				}
				catch (Exception ex)
				{
					throw new VsaException(VsaError.EngineCannotReset, ex.ToString(), ex);
				}
			}
			this.engine.Close();
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002128 File Offset: 0x00001128
		public string Run(string url, string host)
		{
			return VsaWebProxyScript.CallMethod(this.scriptInstance, "ExecuteFindProxyForURL", new object[] { url, host }) as string;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x0000215A File Offset: 0x0000115A
		public override object InitializeLifetimeService()
		{
			return null;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002160 File Offset: 0x00001160
		[ReflectionPermission(SecurityAction.Assert, Flags = ReflectionPermissionFlag.MemberAccess)]
		private static object CallMethod(object targetObject, string name, object[] args)
		{
			if (targetObject == null || name == null)
			{
				return null;
			}
			Type type = targetObject.GetType();
			Type[] array = new Type[args.Length];
			for (int i = 0; i < args.Length; i++)
			{
				array[i] = args[i].GetType();
			}
			MethodInfo method = type.GetMethod(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod | BindingFlags.CreateInstance, Type.DefaultBinder, array, null);
			return method.Invoke(targetObject, args);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000021BC File Offset: 0x000011BC
		public bool Load(Uri engineScriptLocation, string scriptBody, Type helperType)
		{
			byte[] array;
			byte[] array2;
			return this.CompileScript(engineScriptLocation, scriptBody, helperType, out array, out array2) && this.LoadAssembly(array, array2);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000021E4 File Offset: 0x000011E4
		[PermissionSet(SecurityAction.Assert, Name = "FullTrust")]
		private bool CompileScript(Uri engineScriptLocation, string scriptBody, Type helperType, out byte[] pe, out byte[] pdb)
		{
			pe = null;
			pdb = null;
			try
			{
				this.engine = new VsaEngine();
				this.engine.RootMoniker = "pac-" + engineScriptLocation.ToString();
				this.engine.Site = new VsaWebProxyScript.VsaEngineSite(helperType);
				this.engine.InitNew();
				this.engine.RootNamespace = "__WebProxyScript";
				this.engine.SetOption("print", false);
				this.engine.SetOption("fast", false);
				this.engine.SetOption("autoref", false);
				string text = typeof(SecurityTransparentAttribute).Assembly.GetName().Name + ".dll";
				IVsaReferenceItem vsaReferenceItem = this.engine.Items.CreateItem(text, VsaItemType.Reference, VsaItemFlag.None) as IVsaReferenceItem;
				vsaReferenceItem.AssemblyName = text;
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("import System.Security;\r\n[assembly:System.Security.SecurityTransparent()]\r\nfunction isPlainHostName(hostName: String): Boolean { return __om.isPlainHostName(hostName); }\r\nfunction dnsDomainIs(host: String, domain: String): Boolean { return __om.dnsDomainIs(host, domain); }\r\nfunction localHostOrDomainIs(host: String, hostdom: String): Boolean { return __om.localHostOrDomainIs(host, hostdom); }\r\nfunction isResolvable(host: String): Boolean { return __om.isResolvable(host); }\r\nfunction isInNet(host: String, pattern: String, mask: String): Boolean { return __om.isInNet(host, pattern, mask); }\r\nfunction dnsResolve(host: String): String { return __om.dnsResolve(host); }\r\nfunction myIpAddress(): String { return __om.myIpAddress(); }\r\nfunction dnsDomainLevels(host: String): int { return __om.dnsDomainLevels(host); }\r\nfunction shExpMatch(str: String, pattern: String): Boolean { return __om.shExpMatch(str, pattern); }\r\nfunction weekdayRange(wd1: String, wd2: String, gmt: String): Boolean { return __om.weekdayRange(wd1, wd2, gmt); }\r\nfunction dateRange(day1, month1, year1, day2, month2, year2, gmt): Boolean { return true; }\r\nfunction timeRange(hour1, min1, sec1, hour2, min2, sec2, gmt): Boolean { return true; }\r\n");
				if (Socket.OSSupportsIPv6)
				{
					stringBuilder.Append("function getClientVersion(): String { return __om.getClientVersion(); }\r\nfunction sortIpAddressList(IPAddressList:String): String { return __om.sortIpAddressList(IPAddressList); }\r\nfunction isInNetEx(ipAddress:String, ipPrefix:String): Boolean { return __om.isInNetEx(ipAddress, ipPrefix); }\r\nfunction myIpAddressEx(): String { return __om.myIpAddressEx(); }\r\nfunction dnsResolveEx(host:String): String { return __om.dnsResolveEx(host); }\r\nfunction isResolvableEx(host:String): Boolean { return __om.isResolvableEx(host); }\r\nvar __RefereceOfFindProxyForURLEx = this[\"FindProxyForURLEx\"];\r\nvar bFindProxyForURLExFound : Boolean = __RefereceOfFindProxyForURLEx != null && typeof(__RefereceOfFindProxyForURLEx) == \"function\";\r\nclass __WebProxyScript { \t\r\n\t\t\t     \tfunction ExecuteFindProxyForURL(url, host): String { \r\n\t\t         \t\tif(bFindProxyForURLExFound) {\r\n\t\t         \t\t\treturn String(FindProxyForURLEx(url, host)); \r\n\t\t         \t\t}\r\n\t\t         \t\telse {\r\n\t\t\t         \t\treturn String(FindProxyForURL(url, host)); \r\n\t\t         \t\t}\r\n\t\t\t       \t} \r\n\t\t\t       }\r\n\t\t\t         ");
				}
				else
				{
					stringBuilder.Append("class __WebProxyScript { function ExecuteFindProxyForURL(url, host): String { return String(FindProxyForURL(url, host)); } }\r\n");
				}
				stringBuilder.Append("var ProxyConfig = { bindings:{} };\r\n");
				stringBuilder.Append("//@position(file=\"" + engineScriptLocation.ToString() + "\",line = 1)\n");
				stringBuilder.Append(scriptBody);
				IVsaCodeItem vsaCodeItem = this.engine.Items.CreateItem("SourceText", VsaItemType.Code, VsaItemFlag.None) as IVsaCodeItem;
				vsaCodeItem.SourceText = stringBuilder.ToString();
				this.engine.Items.CreateItem("__om", VsaItemType.AppGlobal, VsaItemFlag.None);
				if (this.engine.Compile())
				{
					this.engine.SaveCompiledState(out pe, out pdb);
					this.assembly = Assembly.Load(pe, pdb, AppDomain.CurrentDomain.Evidence);
					return true;
				}
			}
			catch
			{
			}
			return false;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000023D8 File Offset: 0x000013D8
		private bool LoadAssembly(byte[] pe, byte[] pdb)
		{
			try
			{
				Type type = this.assembly.GetType(this.engine.RootNamespace + "._Startup");
				this.startupInstance = (BaseVsaStartup)Activator.CreateInstance(type);
				this.startupInstance.SetSite(this.engine.Site);
				this.startupInstance.Startup();
				Type type2 = this.assembly.GetType(this.engine.RootNamespace + ".__WebProxyScript");
				this.scriptInstance = Activator.CreateInstance(type2);
				VsaWebProxyScript.CallMethod(this.scriptInstance, "SetEngine", new object[] { this.engine });
			}
			catch
			{
				return false;
			}
			return true;
		}

		// Token: 0x04000005 RID: 5
		private const string RootNamespace = "__WebProxyScript";

		// Token: 0x04000006 RID: 6
		private const string NetScriptSource_v4 = "import System.Security;\r\n[assembly:System.Security.SecurityTransparent()]\r\nfunction isPlainHostName(hostName: String): Boolean { return __om.isPlainHostName(hostName); }\r\nfunction dnsDomainIs(host: String, domain: String): Boolean { return __om.dnsDomainIs(host, domain); }\r\nfunction localHostOrDomainIs(host: String, hostdom: String): Boolean { return __om.localHostOrDomainIs(host, hostdom); }\r\nfunction isResolvable(host: String): Boolean { return __om.isResolvable(host); }\r\nfunction isInNet(host: String, pattern: String, mask: String): Boolean { return __om.isInNet(host, pattern, mask); }\r\nfunction dnsResolve(host: String): String { return __om.dnsResolve(host); }\r\nfunction myIpAddress(): String { return __om.myIpAddress(); }\r\nfunction dnsDomainLevels(host: String): int { return __om.dnsDomainLevels(host); }\r\nfunction shExpMatch(str: String, pattern: String): Boolean { return __om.shExpMatch(str, pattern); }\r\nfunction weekdayRange(wd1: String, wd2: String, gmt: String): Boolean { return __om.weekdayRange(wd1, wd2, gmt); }\r\nfunction dateRange(day1, month1, year1, day2, month2, year2, gmt): Boolean { return true; }\r\nfunction timeRange(hour1, min1, sec1, hour2, min2, sec2, gmt): Boolean { return true; }\r\n";

		// Token: 0x04000007 RID: 7
		private const string NetScriptSource_v6ExtClass = "function getClientVersion(): String { return __om.getClientVersion(); }\r\nfunction sortIpAddressList(IPAddressList:String): String { return __om.sortIpAddressList(IPAddressList); }\r\nfunction isInNetEx(ipAddress:String, ipPrefix:String): Boolean { return __om.isInNetEx(ipAddress, ipPrefix); }\r\nfunction myIpAddressEx(): String { return __om.myIpAddressEx(); }\r\nfunction dnsResolveEx(host:String): String { return __om.dnsResolveEx(host); }\r\nfunction isResolvableEx(host:String): Boolean { return __om.isResolvableEx(host); }\r\nvar __RefereceOfFindProxyForURLEx = this[\"FindProxyForURLEx\"];\r\nvar bFindProxyForURLExFound : Boolean = __RefereceOfFindProxyForURLEx != null && typeof(__RefereceOfFindProxyForURLEx) == \"function\";\r\nclass __WebProxyScript { \t\r\n\t\t\t     \tfunction ExecuteFindProxyForURL(url, host): String { \r\n\t\t         \t\tif(bFindProxyForURLExFound) {\r\n\t\t         \t\t\treturn String(FindProxyForURLEx(url, host)); \r\n\t\t         \t\t}\r\n\t\t         \t\telse {\r\n\t\t\t         \t\treturn String(FindProxyForURL(url, host)); \r\n\t\t         \t\t}\r\n\t\t\t       \t} \r\n\t\t\t       }\r\n\t\t\t         ";

		// Token: 0x04000008 RID: 8
		private const string NetScriptSource_v4Class = "class __WebProxyScript { function ExecuteFindProxyForURL(url, host): String { return String(FindProxyForURL(url, host)); } }\r\n";

		// Token: 0x04000009 RID: 9
		private const string NetScriptSource_bindings = "var ProxyConfig = { bindings:{} };\r\n";

		// Token: 0x0400000A RID: 10
		private static readonly Zone IntranetZone = new Zone(SecurityZone.Intranet);

		// Token: 0x0400000B RID: 11
		private VsaEngine engine;

		// Token: 0x0400000C RID: 12
		private object scriptInstance;

		// Token: 0x0400000D RID: 13
		private Assembly assembly;

		// Token: 0x0400000E RID: 14
		private BaseVsaStartup startupInstance;

		// Token: 0x02000004 RID: 4
		private class VsaEngineSite : IVsaSite
		{
			// Token: 0x0600000A RID: 10 RVA: 0x000024B9 File Offset: 0x000014B9
			internal VsaEngineSite(Type globalType)
			{
				this.m_GlobalType = globalType;
			}

			// Token: 0x0600000B RID: 11 RVA: 0x000024C8 File Offset: 0x000014C8
			public void GetCompiledState(out byte[] pe, out byte[] debugInfo)
			{
				pe = null;
				debugInfo = null;
			}

			// Token: 0x0600000C RID: 12 RVA: 0x000024D0 File Offset: 0x000014D0
			public object GetEventSourceInstance(string itemName, string eventSourceName)
			{
				return null;
			}

			// Token: 0x0600000D RID: 13 RVA: 0x000024D3 File Offset: 0x000014D3
			[ReflectionPermission(SecurityAction.Assert, Flags = ReflectionPermissionFlag.MemberAccess)]
			public object GetGlobalInstance(string name)
			{
				if (name == "__om")
				{
					return Activator.CreateInstance(this.m_GlobalType, true);
				}
				throw new VsaException(VsaError.GlobalInstanceInvalid);
			}

			// Token: 0x0600000E RID: 14 RVA: 0x000024F9 File Offset: 0x000014F9
			public void Notify(string notify, object info)
			{
			}

			// Token: 0x0600000F RID: 15 RVA: 0x000024FB File Offset: 0x000014FB
			public bool OnCompilerError(IVsaError error)
			{
				return error.Severity != 0;
			}

			// Token: 0x0400000F RID: 15
			private readonly Type m_GlobalType;
		}
	}
}
