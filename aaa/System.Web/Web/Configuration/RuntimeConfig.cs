using System;
using System.Collections;
using System.Configuration;
using System.Configuration.Internal;
using System.Net.Configuration;
using System.Security.Permissions;
using System.Web.Hosting;

namespace System.Web.Configuration
{
	// Token: 0x020001B9 RID: 441
	internal class RuntimeConfig
	{
		// Token: 0x06001940 RID: 6464 RVA: 0x0007889C File Offset: 0x0007789C
		internal static RuntimeConfig GetConfig()
		{
			if (!HttpConfigurationSystem.UseHttpConfigurationSystem)
			{
				return RuntimeConfig.GetClientRuntimeConfig();
			}
			HttpContext httpContext = HttpContext.Current;
			if (httpContext != null)
			{
				return RuntimeConfig.GetConfig(httpContext);
			}
			return RuntimeConfig.GetAppConfig();
		}

		// Token: 0x06001941 RID: 6465 RVA: 0x000788CB File Offset: 0x000778CB
		internal static RuntimeConfig GetConfig(HttpContext context)
		{
			if (!HttpConfigurationSystem.UseHttpConfigurationSystem)
			{
				return RuntimeConfig.GetClientRuntimeConfig();
			}
			return context.GetRuntimeConfig();
		}

		// Token: 0x06001942 RID: 6466 RVA: 0x000788E0 File Offset: 0x000778E0
		internal static RuntimeConfig GetConfig(HttpContext context, VirtualPath path)
		{
			if (!HttpConfigurationSystem.UseHttpConfigurationSystem)
			{
				return RuntimeConfig.GetClientRuntimeConfig();
			}
			return context.GetRuntimeConfig(path);
		}

		// Token: 0x06001943 RID: 6467 RVA: 0x000788F6 File Offset: 0x000778F6
		internal static RuntimeConfig GetConfig(string path)
		{
			return RuntimeConfig.GetConfig(VirtualPath.CreateNonRelativeAllowNull(path));
		}

		// Token: 0x06001944 RID: 6468 RVA: 0x00078903 File Offset: 0x00077903
		internal static RuntimeConfig GetConfig(VirtualPath path)
		{
			if (!HttpConfigurationSystem.UseHttpConfigurationSystem)
			{
				return RuntimeConfig.GetClientRuntimeConfig();
			}
			return CachedPathData.GetVirtualPathData(path, true).RuntimeConfig;
		}

		// Token: 0x06001945 RID: 6469 RVA: 0x0007891E File Offset: 0x0007791E
		internal static RuntimeConfig GetAppConfig()
		{
			if (!HttpConfigurationSystem.UseHttpConfigurationSystem)
			{
				return RuntimeConfig.GetClientRuntimeConfig();
			}
			return CachedPathData.GetApplicationPathData().RuntimeConfig;
		}

		// Token: 0x06001946 RID: 6470 RVA: 0x00078937 File Offset: 0x00077937
		internal static RuntimeConfig GetRootWebConfig()
		{
			if (!HttpConfigurationSystem.UseHttpConfigurationSystem)
			{
				return RuntimeConfig.GetClientRuntimeConfig();
			}
			return CachedPathData.GetRootWebPathData().RuntimeConfig;
		}

		// Token: 0x06001947 RID: 6471 RVA: 0x00078950 File Offset: 0x00077950
		internal static RuntimeConfig GetMachineConfig()
		{
			if (!HttpConfigurationSystem.UseHttpConfigurationSystem)
			{
				return RuntimeConfig.GetClientRuntimeConfig();
			}
			return CachedPathData.GetMachinePathData().RuntimeConfig;
		}

		// Token: 0x06001948 RID: 6472 RVA: 0x0007896C File Offset: 0x0007796C
		internal static RuntimeConfig GetLKGConfig(HttpContext context)
		{
			RuntimeConfig runtimeConfig = null;
			bool flag = false;
			try
			{
				runtimeConfig = RuntimeConfig.GetConfig(context);
				flag = true;
			}
			catch
			{
			}
			if (!flag)
			{
				runtimeConfig = RuntimeConfig.GetLKGRuntimeConfig(context.Request.FilePathObject);
			}
			return runtimeConfig.RuntimeConfigLKG;
		}

		// Token: 0x06001949 RID: 6473 RVA: 0x000789B8 File Offset: 0x000779B8
		internal static RuntimeConfig GetAppLKGConfig()
		{
			RuntimeConfig runtimeConfig = null;
			bool flag = false;
			try
			{
				runtimeConfig = RuntimeConfig.GetAppConfig();
				flag = true;
			}
			catch
			{
			}
			if (!flag)
			{
				runtimeConfig = RuntimeConfig.GetLKGRuntimeConfig(global::System.Web.Hosting.HostingEnvironment.ApplicationVirtualPathObject);
			}
			return runtimeConfig.RuntimeConfigLKG;
		}

		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x0600194A RID: 6474 RVA: 0x000789FC File Offset: 0x000779FC
		internal ConnectionStringsSection ConnectionStrings
		{
			get
			{
				return (ConnectionStringsSection)this.GetSection("connectionStrings", typeof(ConnectionStringsSection), RuntimeConfig.ResultsIndex.ConnectionStrings);
			}
		}

		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x0600194B RID: 6475 RVA: 0x00078A19 File Offset: 0x00077A19
		internal SmtpSection Smtp
		{
			get
			{
				return (SmtpSection)this.GetSection("system.net/mailSettings/smtp", typeof(SmtpSection));
			}
		}

		// Token: 0x1700048C RID: 1164
		// (get) Token: 0x0600194C RID: 6476 RVA: 0x00078A35 File Offset: 0x00077A35
		internal AnonymousIdentificationSection AnonymousIdentification
		{
			get
			{
				return (AnonymousIdentificationSection)this.GetSection("system.web/anonymousIdentification", typeof(AnonymousIdentificationSection));
			}
		}

		// Token: 0x1700048D RID: 1165
		// (get) Token: 0x0600194D RID: 6477 RVA: 0x00078A51 File Offset: 0x00077A51
		internal ProtocolsSection Protocols
		{
			get
			{
				return (ProtocolsSection)this.GetSection("system.web/protocols", typeof(ProtocolsSection));
			}
		}

		// Token: 0x1700048E RID: 1166
		// (get) Token: 0x0600194E RID: 6478 RVA: 0x00078A6D File Offset: 0x00077A6D
		internal AuthenticationSection Authentication
		{
			get
			{
				return (AuthenticationSection)this.GetSection("system.web/authentication", typeof(AuthenticationSection), RuntimeConfig.ResultsIndex.Authentication);
			}
		}

		// Token: 0x1700048F RID: 1167
		// (get) Token: 0x0600194F RID: 6479 RVA: 0x00078A8A File Offset: 0x00077A8A
		internal AuthorizationSection Authorization
		{
			get
			{
				return (AuthorizationSection)this.GetSection("system.web/authorization", typeof(AuthorizationSection), RuntimeConfig.ResultsIndex.Authorization);
			}
		}

		// Token: 0x17000490 RID: 1168
		// (get) Token: 0x06001950 RID: 6480 RVA: 0x00078AA7 File Offset: 0x00077AA7
		internal HttpCapabilitiesEvaluator BrowserCaps
		{
			get
			{
				return (HttpCapabilitiesEvaluator)this.GetHandlerSection("system.web/browserCaps", typeof(HttpCapabilitiesEvaluator), RuntimeConfig.ResultsIndex.BrowserCaps);
			}
		}

		// Token: 0x17000491 RID: 1169
		// (get) Token: 0x06001951 RID: 6481 RVA: 0x00078AC4 File Offset: 0x00077AC4
		internal ClientTargetSection ClientTarget
		{
			get
			{
				return (ClientTargetSection)this.GetSection("system.web/clientTarget", typeof(ClientTargetSection), RuntimeConfig.ResultsIndex.ClientTarget);
			}
		}

		// Token: 0x17000492 RID: 1170
		// (get) Token: 0x06001952 RID: 6482 RVA: 0x00078AE1 File Offset: 0x00077AE1
		internal CompilationSection Compilation
		{
			get
			{
				return (CompilationSection)this.GetSection("system.web/compilation", typeof(CompilationSection), RuntimeConfig.ResultsIndex.Compilation);
			}
		}

		// Token: 0x17000493 RID: 1171
		// (get) Token: 0x06001953 RID: 6483 RVA: 0x00078AFE File Offset: 0x00077AFE
		internal CustomErrorsSection CustomErrors
		{
			get
			{
				return (CustomErrorsSection)this.GetSection("system.web/customErrors", typeof(CustomErrorsSection));
			}
		}

		// Token: 0x17000494 RID: 1172
		// (get) Token: 0x06001954 RID: 6484 RVA: 0x00078B1A File Offset: 0x00077B1A
		internal GlobalizationSection Globalization
		{
			get
			{
				return (GlobalizationSection)this.GetSection("system.web/globalization", typeof(GlobalizationSection), RuntimeConfig.ResultsIndex.Globalization);
			}
		}

		// Token: 0x17000495 RID: 1173
		// (get) Token: 0x06001955 RID: 6485 RVA: 0x00078B37 File Offset: 0x00077B37
		internal DeploymentSection Deployment
		{
			get
			{
				return (DeploymentSection)this.GetSection("system.web/deployment", typeof(DeploymentSection));
			}
		}

		// Token: 0x17000496 RID: 1174
		// (get) Token: 0x06001956 RID: 6486 RVA: 0x00078B53 File Offset: 0x00077B53
		internal HealthMonitoringSection HealthMonitoring
		{
			get
			{
				return (HealthMonitoringSection)this.GetSection("system.web/healthMonitoring", typeof(HealthMonitoringSection));
			}
		}

		// Token: 0x17000497 RID: 1175
		// (get) Token: 0x06001957 RID: 6487 RVA: 0x00078B6F File Offset: 0x00077B6F
		internal HostingEnvironmentSection HostingEnvironment
		{
			get
			{
				return (HostingEnvironmentSection)this.GetSection("system.web/hostingEnvironment", typeof(HostingEnvironmentSection));
			}
		}

		// Token: 0x17000498 RID: 1176
		// (get) Token: 0x06001958 RID: 6488 RVA: 0x00078B8B File Offset: 0x00077B8B
		internal HttpCookiesSection HttpCookies
		{
			get
			{
				return (HttpCookiesSection)this.GetSection("system.web/httpCookies", typeof(HttpCookiesSection), RuntimeConfig.ResultsIndex.HttpCookies);
			}
		}

		// Token: 0x17000499 RID: 1177
		// (get) Token: 0x06001959 RID: 6489 RVA: 0x00078BA8 File Offset: 0x00077BA8
		internal HttpHandlersSection HttpHandlers
		{
			get
			{
				return (HttpHandlersSection)this.GetSection("system.web/httpHandlers", typeof(HttpHandlersSection), RuntimeConfig.ResultsIndex.HttpHandlers);
			}
		}

		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x0600195A RID: 6490 RVA: 0x00078BC6 File Offset: 0x00077BC6
		internal HttpModulesSection HttpModules
		{
			get
			{
				return (HttpModulesSection)this.GetSection("system.web/httpModules", typeof(HttpModulesSection), RuntimeConfig.ResultsIndex.HttpModules);
			}
		}

		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x0600195B RID: 6491 RVA: 0x00078BE4 File Offset: 0x00077BE4
		internal HttpRuntimeSection HttpRuntime
		{
			get
			{
				return (HttpRuntimeSection)this.GetSection("system.web/httpRuntime", typeof(HttpRuntimeSection), RuntimeConfig.ResultsIndex.HttpRuntime);
			}
		}

		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x0600195C RID: 6492 RVA: 0x00078C02 File Offset: 0x00077C02
		internal IdentitySection Identity
		{
			get
			{
				return (IdentitySection)this.GetSection("system.web/identity", typeof(IdentitySection), RuntimeConfig.ResultsIndex.Identity);
			}
		}

		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x0600195D RID: 6493 RVA: 0x00078C20 File Offset: 0x00077C20
		internal MachineKeySection MachineKey
		{
			get
			{
				return (MachineKeySection)this.GetSection("system.web/machineKey", typeof(MachineKeySection), RuntimeConfig.ResultsIndex.MachineKey);
			}
		}

		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x0600195E RID: 6494 RVA: 0x00078C3E File Offset: 0x00077C3E
		internal MembershipSection Membership
		{
			get
			{
				return (MembershipSection)this.GetSection("system.web/membership", typeof(MembershipSection), RuntimeConfig.ResultsIndex.Membership);
			}
		}

		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x0600195F RID: 6495 RVA: 0x00078C5C File Offset: 0x00077C5C
		internal PagesSection Pages
		{
			get
			{
				return (PagesSection)this.GetSection("system.web/pages", typeof(PagesSection), RuntimeConfig.ResultsIndex.Pages);
			}
		}

		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x06001960 RID: 6496 RVA: 0x00078C7A File Offset: 0x00077C7A
		internal ProcessModelSection ProcessModel
		{
			get
			{
				return (ProcessModelSection)this.GetSection("system.web/processModel", typeof(ProcessModelSection));
			}
		}

		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x06001961 RID: 6497 RVA: 0x00078C96 File Offset: 0x00077C96
		internal ProfileSection Profile
		{
			get
			{
				return (ProfileSection)this.GetSection("system.web/profile", typeof(ProfileSection), RuntimeConfig.ResultsIndex.Profile);
			}
		}

		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x06001962 RID: 6498 RVA: 0x00078CB4 File Offset: 0x00077CB4
		internal RoleManagerSection RoleManager
		{
			get
			{
				return (RoleManagerSection)this.GetSection("system.web/roleManager", typeof(RoleManagerSection));
			}
		}

		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x06001963 RID: 6499 RVA: 0x00078CD0 File Offset: 0x00077CD0
		internal SecurityPolicySection SecurityPolicy
		{
			get
			{
				return (SecurityPolicySection)this.GetSection("system.web/securityPolicy", typeof(SecurityPolicySection));
			}
		}

		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x06001964 RID: 6500 RVA: 0x00078CEC File Offset: 0x00077CEC
		internal SessionPageStateSection SessionPageState
		{
			get
			{
				return (SessionPageStateSection)this.GetSection("system.web/sessionPageState", typeof(SessionPageStateSection), RuntimeConfig.ResultsIndex.SessionPageState);
			}
		}

		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x06001965 RID: 6501 RVA: 0x00078D0A File Offset: 0x00077D0A
		internal SessionStateSection SessionState
		{
			get
			{
				return (SessionStateSection)this.GetSection("system.web/sessionState", typeof(SessionStateSection));
			}
		}

		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x06001966 RID: 6502 RVA: 0x00078D26 File Offset: 0x00077D26
		internal SiteMapSection SiteMap
		{
			get
			{
				return (SiteMapSection)this.GetSection("system.web/siteMap", typeof(SiteMapSection));
			}
		}

		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x06001967 RID: 6503 RVA: 0x00078D42 File Offset: 0x00077D42
		internal TraceSection Trace
		{
			get
			{
				return (TraceSection)this.GetSection("system.web/trace", typeof(TraceSection));
			}
		}

		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x06001968 RID: 6504 RVA: 0x00078D5E File Offset: 0x00077D5E
		internal TrustSection Trust
		{
			get
			{
				return (TrustSection)this.GetSection("system.web/trust", typeof(TrustSection));
			}
		}

		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x06001969 RID: 6505 RVA: 0x00078D7A File Offset: 0x00077D7A
		internal UrlMappingsSection UrlMappings
		{
			get
			{
				return (UrlMappingsSection)this.GetSection("system.web/urlMappings", typeof(UrlMappingsSection), RuntimeConfig.ResultsIndex.UrlMappings);
			}
		}

		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x0600196A RID: 6506 RVA: 0x00078D98 File Offset: 0x00077D98
		internal Hashtable WebControls
		{
			get
			{
				return (Hashtable)this.GetSection("system.web/webControls", typeof(Hashtable), RuntimeConfig.ResultsIndex.WebControls);
			}
		}

		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x0600196B RID: 6507 RVA: 0x00078DB6 File Offset: 0x00077DB6
		internal WebPartsSection WebParts
		{
			get
			{
				return (WebPartsSection)this.GetSection("system.web/webParts", typeof(WebPartsSection), RuntimeConfig.ResultsIndex.WebParts);
			}
		}

		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x0600196C RID: 6508 RVA: 0x00078DD4 File Offset: 0x00077DD4
		internal XhtmlConformanceSection XhtmlConformance
		{
			get
			{
				return (XhtmlConformanceSection)this.GetSection("system.web/xhtmlConformance", typeof(XhtmlConformanceSection), RuntimeConfig.ResultsIndex.XhtmlConformance);
			}
		}

		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x0600196D RID: 6509 RVA: 0x00078DF2 File Offset: 0x00077DF2
		internal CacheSection Cache
		{
			get
			{
				return (CacheSection)this.GetSection("system.web/caching/cache", typeof(CacheSection));
			}
		}

		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x0600196E RID: 6510 RVA: 0x00078E0E File Offset: 0x00077E0E
		internal OutputCacheSection OutputCache
		{
			get
			{
				return (OutputCacheSection)this.GetSection("system.web/caching/outputCache", typeof(OutputCacheSection), RuntimeConfig.ResultsIndex.OutputCache);
			}
		}

		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x0600196F RID: 6511 RVA: 0x00078E2C File Offset: 0x00077E2C
		internal OutputCacheSettingsSection OutputCacheSettings
		{
			get
			{
				return (OutputCacheSettingsSection)this.GetSection("system.web/caching/outputCacheSettings", typeof(OutputCacheSettingsSection), RuntimeConfig.ResultsIndex.OutputCacheSettings);
			}
		}

		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x06001970 RID: 6512 RVA: 0x00078E4A File Offset: 0x00077E4A
		internal SqlCacheDependencySection SqlCacheDependency
		{
			get
			{
				return (SqlCacheDependencySection)this.GetSection("system.web/caching/sqlCacheDependency", typeof(SqlCacheDependencySection));
			}
		}

		// Token: 0x06001971 RID: 6513 RVA: 0x00078E66 File Offset: 0x00077E66
		static RuntimeConfig()
		{
			RuntimeConfig.GetErrorRuntimeConfig();
		}

		// Token: 0x06001972 RID: 6514 RVA: 0x00078E78 File Offset: 0x00077E78
		internal RuntimeConfig(IInternalConfigRecord configRecord)
			: this(configRecord, false)
		{
		}

		// Token: 0x06001973 RID: 6515 RVA: 0x00078E84 File Offset: 0x00077E84
		protected RuntimeConfig(IInternalConfigRecord configRecord, bool permitNull)
		{
			this._configRecord = configRecord;
			this._permitNull = permitNull;
			this._results = new object[24];
			for (int i = 0; i < this._results.Length; i++)
			{
				this._results[i] = RuntimeConfig.s_unevaluatedResult;
			}
		}

		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x06001974 RID: 6516 RVA: 0x00078ED4 File Offset: 0x00077ED4
		private RuntimeConfigLKG RuntimeConfigLKG
		{
			get
			{
				if (this._runtimeConfigLKG == null)
				{
					lock (this)
					{
						if (this._runtimeConfigLKG == null)
						{
							this._runtimeConfigLKG = new RuntimeConfigLKG(this._configRecord);
						}
					}
				}
				return this._runtimeConfigLKG;
			}
		}

		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x06001975 RID: 6517 RVA: 0x00078F2C File Offset: 0x00077F2C
		internal IInternalConfigRecord ConfigRecord
		{
			get
			{
				return this._configRecord;
			}
		}

		// Token: 0x06001976 RID: 6518 RVA: 0x00078F34 File Offset: 0x00077F34
		private static RuntimeConfig GetClientRuntimeConfig()
		{
			if (RuntimeConfig.s_clientRuntimeConfig == null)
			{
				RuntimeConfig.s_clientRuntimeConfig = new ClientRuntimeConfig();
			}
			return RuntimeConfig.s_clientRuntimeConfig;
		}

		// Token: 0x06001977 RID: 6519 RVA: 0x00078F4C File Offset: 0x00077F4C
		private static RuntimeConfig GetNullRuntimeConfig()
		{
			if (RuntimeConfig.s_nullRuntimeConfig == null)
			{
				RuntimeConfig.s_nullRuntimeConfig = new NullRuntimeConfig();
			}
			return RuntimeConfig.s_nullRuntimeConfig;
		}

		// Token: 0x06001978 RID: 6520 RVA: 0x00078F64 File Offset: 0x00077F64
		internal static RuntimeConfig GetErrorRuntimeConfig()
		{
			if (RuntimeConfig.s_errorRuntimeConfig == null)
			{
				RuntimeConfig.s_errorRuntimeConfig = new ErrorRuntimeConfig();
			}
			return RuntimeConfig.s_errorRuntimeConfig;
		}

		// Token: 0x06001979 RID: 6521 RVA: 0x00078F7C File Offset: 0x00077F7C
		[ConfigurationPermission(SecurityAction.Assert, Unrestricted = true)]
		protected virtual object GetSectionObject(string sectionName)
		{
			return this._configRecord.GetSection(sectionName);
		}

		// Token: 0x0600197A RID: 6522 RVA: 0x00078F8C File Offset: 0x00077F8C
		private object GetHandlerSection(string sectionName, Type type, RuntimeConfig.ResultsIndex index)
		{
			object obj = this._results[(int)index];
			if (obj != RuntimeConfig.s_unevaluatedResult)
			{
				return obj;
			}
			obj = this.GetSectionObject(sectionName);
			if (obj != null && obj.GetType() != type)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_unable_to_get_section", new object[] { sectionName }));
			}
			if (index != RuntimeConfig.ResultsIndex.UNUSED)
			{
				this._results[(int)index] = obj;
			}
			return obj;
		}

		// Token: 0x0600197B RID: 6523 RVA: 0x00078FE9 File Offset: 0x00077FE9
		private object GetSection(string sectionName, Type type)
		{
			return this.GetSection(sectionName, type, RuntimeConfig.ResultsIndex.UNUSED);
		}

		// Token: 0x0600197C RID: 6524 RVA: 0x00078FF4 File Offset: 0x00077FF4
		private object GetSection(string sectionName, Type type, RuntimeConfig.ResultsIndex index)
		{
			object obj = this._results[(int)index];
			if (obj != RuntimeConfig.s_unevaluatedResult)
			{
				return obj;
			}
			obj = this.GetSectionObject(sectionName);
			if (obj == null)
			{
				if (!this._permitNull)
				{
					throw new ConfigurationErrorsException(SR.GetString("Config_unable_to_get_section", new object[] { sectionName }));
				}
			}
			else if (obj.GetType() != type)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_unable_to_get_section", new object[] { sectionName }));
			}
			if (index != RuntimeConfig.ResultsIndex.UNUSED)
			{
				this._results[(int)index] = obj;
			}
			return obj;
		}

		// Token: 0x0600197D RID: 6525 RVA: 0x00079078 File Offset: 0x00078078
		private static RuntimeConfig GetLKGRuntimeConfig(VirtualPath path)
		{
			try
			{
				path = path.Parent;
				goto IL_0028;
			}
			catch
			{
				path = global::System.Web.Hosting.HostingEnvironment.ApplicationVirtualPathObject;
				goto IL_0028;
			}
			try
			{
				IL_0014:
				return RuntimeConfig.GetConfig(path);
			}
			catch
			{
				path = path.Parent;
			}
			IL_0028:
			if (!(path != null))
			{
				try
				{
					return RuntimeConfig.GetRootWebConfig();
				}
				catch
				{
				}
				try
				{
					return RuntimeConfig.GetMachineConfig();
				}
				catch
				{
				}
				return RuntimeConfig.GetNullRuntimeConfig();
			}
			goto IL_0014;
		}

		// Token: 0x04001727 RID: 5927
		private static RuntimeConfig s_clientRuntimeConfig;

		// Token: 0x04001728 RID: 5928
		private static RuntimeConfig s_nullRuntimeConfig;

		// Token: 0x04001729 RID: 5929
		private static RuntimeConfig s_errorRuntimeConfig;

		// Token: 0x0400172A RID: 5930
		private static object s_unevaluatedResult = new object();

		// Token: 0x0400172B RID: 5931
		private object[] _results;

		// Token: 0x0400172C RID: 5932
		private RuntimeConfigLKG _runtimeConfigLKG;

		// Token: 0x0400172D RID: 5933
		protected IInternalConfigRecord _configRecord;

		// Token: 0x0400172E RID: 5934
		private bool _permitNull;

		// Token: 0x020001BA RID: 442
		internal enum ResultsIndex
		{
			// Token: 0x04001730 RID: 5936
			UNUSED,
			// Token: 0x04001731 RID: 5937
			Authentication,
			// Token: 0x04001732 RID: 5938
			Authorization,
			// Token: 0x04001733 RID: 5939
			BrowserCaps,
			// Token: 0x04001734 RID: 5940
			ClientTarget,
			// Token: 0x04001735 RID: 5941
			Compilation,
			// Token: 0x04001736 RID: 5942
			ConnectionStrings,
			// Token: 0x04001737 RID: 5943
			Globalization,
			// Token: 0x04001738 RID: 5944
			HttpCookies,
			// Token: 0x04001739 RID: 5945
			HttpHandlers,
			// Token: 0x0400173A RID: 5946
			HttpModules,
			// Token: 0x0400173B RID: 5947
			HttpRuntime,
			// Token: 0x0400173C RID: 5948
			Identity,
			// Token: 0x0400173D RID: 5949
			MachineKey,
			// Token: 0x0400173E RID: 5950
			Membership,
			// Token: 0x0400173F RID: 5951
			OutputCache,
			// Token: 0x04001740 RID: 5952
			OutputCacheSettings,
			// Token: 0x04001741 RID: 5953
			Pages,
			// Token: 0x04001742 RID: 5954
			Profile,
			// Token: 0x04001743 RID: 5955
			SessionPageState,
			// Token: 0x04001744 RID: 5956
			WebControls,
			// Token: 0x04001745 RID: 5957
			WebParts,
			// Token: 0x04001746 RID: 5958
			UrlMappings,
			// Token: 0x04001747 RID: 5959
			XhtmlConformance,
			// Token: 0x04001748 RID: 5960
			SIZE
		}
	}
}
