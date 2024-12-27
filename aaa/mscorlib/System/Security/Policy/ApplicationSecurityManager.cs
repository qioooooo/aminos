using System;
using System.Deployment.Internal.Isolation.Manifest;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Util;

namespace System.Security.Policy
{
	// Token: 0x02000482 RID: 1154
	[ComVisible(true)]
	public static class ApplicationSecurityManager
	{
		// Token: 0x06002E45 RID: 11845 RVA: 0x0009CF60 File Offset: 0x0009BF60
		[SecurityPermission(SecurityAction.Assert, Unrestricted = true)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.ControlEvidence | SecurityPermissionFlag.ControlPolicy)]
		public static bool DetermineApplicationTrust(ActivationContext activationContext, TrustManagerContext context)
		{
			if (activationContext == null)
			{
				throw new ArgumentNullException("activationContext");
			}
			AppDomainManager domainManager = AppDomain.CurrentDomain.DomainManager;
			ApplicationTrust applicationTrust;
			if (domainManager != null)
			{
				HostSecurityManager hostSecurityManager = domainManager.HostSecurityManager;
				if (hostSecurityManager != null && (hostSecurityManager.Flags & HostSecurityManagerOptions.HostDetermineApplicationTrust) == HostSecurityManagerOptions.HostDetermineApplicationTrust)
				{
					applicationTrust = hostSecurityManager.DetermineApplicationTrust(CmsUtils.MergeApplicationEvidence(null, activationContext.Identity, activationContext, null), null, context);
					return applicationTrust != null && applicationTrust.IsApplicationTrustedToRun;
				}
			}
			applicationTrust = ApplicationSecurityManager.DetermineApplicationTrustInternal(activationContext, context);
			return applicationTrust != null && applicationTrust.IsApplicationTrustedToRun;
		}

		// Token: 0x1700083E RID: 2110
		// (get) Token: 0x06002E46 RID: 11846 RVA: 0x0009CFD6 File Offset: 0x0009BFD6
		public static ApplicationTrustCollection UserApplicationTrusts
		{
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlPolicy)]
			get
			{
				return new ApplicationTrustCollection(true);
			}
		}

		// Token: 0x1700083F RID: 2111
		// (get) Token: 0x06002E47 RID: 11847 RVA: 0x0009CFDE File Offset: 0x0009BFDE
		public static IApplicationTrustManager ApplicationTrustManager
		{
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlPolicy)]
			get
			{
				if (ApplicationSecurityManager.m_appTrustManager == null)
				{
					ApplicationSecurityManager.m_appTrustManager = ApplicationSecurityManager.DecodeAppTrustManager();
					if (ApplicationSecurityManager.m_appTrustManager == null)
					{
						throw new PolicyException(Environment.GetResourceString("Policy_NoTrustManager"));
					}
				}
				return ApplicationSecurityManager.m_appTrustManager;
			}
		}

		// Token: 0x06002E48 RID: 11848 RVA: 0x0009D010 File Offset: 0x0009C010
		internal static ApplicationTrust DetermineApplicationTrustInternal(ActivationContext activationContext, TrustManagerContext context)
		{
			ApplicationTrustCollection applicationTrustCollection = new ApplicationTrustCollection(true);
			ApplicationTrust applicationTrust;
			if (context == null || !context.IgnorePersistedDecision)
			{
				applicationTrust = applicationTrustCollection[activationContext.Identity.FullName];
				if (applicationTrust != null)
				{
					return applicationTrust;
				}
			}
			applicationTrust = ApplicationSecurityManager.ApplicationTrustManager.DetermineApplicationTrust(activationContext, context);
			if (applicationTrust == null)
			{
				applicationTrust = new ApplicationTrust(activationContext.Identity);
			}
			applicationTrust.ApplicationIdentity = activationContext.Identity;
			if (applicationTrust.Persist)
			{
				applicationTrustCollection.Add(applicationTrust);
			}
			return applicationTrust;
		}

		// Token: 0x06002E49 RID: 11849 RVA: 0x0009D084 File Offset: 0x0009C084
		private static IApplicationTrustManager DecodeAppTrustManager()
		{
			if (File.InternalExists(ApplicationSecurityManager.s_machineConfigFile))
			{
				FileStream fileStream = new FileStream(ApplicationSecurityManager.s_machineConfigFile, FileMode.Open, FileAccess.Read);
				SecurityElement securityElement = SecurityElement.FromString(new StreamReader(fileStream).ReadToEnd());
				SecurityElement securityElement2 = securityElement.SearchForChildByTag("mscorlib");
				if (securityElement2 != null)
				{
					SecurityElement securityElement3 = securityElement2.SearchForChildByTag("security");
					if (securityElement3 != null)
					{
						SecurityElement securityElement4 = securityElement3.SearchForChildByTag("policy");
						if (securityElement4 != null)
						{
							SecurityElement securityElement5 = securityElement4.SearchForChildByTag("ApplicationSecurityManager");
							if (securityElement5 != null)
							{
								SecurityElement securityElement6 = securityElement5.SearchForChildByTag("IApplicationTrustManager");
								if (securityElement6 != null)
								{
									IApplicationTrustManager applicationTrustManager = ApplicationSecurityManager.DecodeAppTrustManagerFromElement(securityElement6);
									if (applicationTrustManager != null)
									{
										return applicationTrustManager;
									}
								}
							}
						}
					}
				}
			}
			return ApplicationSecurityManager.DecodeAppTrustManagerFromElement(ApplicationSecurityManager.CreateDefaultApplicationTrustManagerElement());
		}

		// Token: 0x06002E4A RID: 11850 RVA: 0x0009D12C File Offset: 0x0009C12C
		private static SecurityElement CreateDefaultApplicationTrustManagerElement()
		{
			SecurityElement securityElement = new SecurityElement("IApplicationTrustManager");
			securityElement.AddAttribute("class", "System.Security.Policy.TrustManager, System.Windows.Forms, Version=" + Assembly.GetExecutingAssembly().GetVersion() + ", Culture=neutral, PublicKeyToken=b77a5c561934e089");
			securityElement.AddAttribute("version", "1");
			return securityElement;
		}

		// Token: 0x06002E4B RID: 11851 RVA: 0x0009D17C File Offset: 0x0009C17C
		private static IApplicationTrustManager DecodeAppTrustManagerFromElement(SecurityElement elTrustManager)
		{
			new ReflectionPermission(ReflectionPermissionFlag.MemberAccess).Assert();
			string text = elTrustManager.Attribute("class");
			Type type = Type.GetType(text, false, false);
			if (type == null)
			{
				return null;
			}
			IApplicationTrustManager applicationTrustManager = Activator.CreateInstance(type) as IApplicationTrustManager;
			if (applicationTrustManager != null)
			{
				applicationTrustManager.FromXml(elTrustManager);
			}
			return applicationTrustManager;
		}

		// Token: 0x04001783 RID: 6019
		private static IApplicationTrustManager m_appTrustManager = null;

		// Token: 0x04001784 RID: 6020
		private static string s_machineConfigFile = Config.MachineDirectory + "applicationtrust.config";
	}
}
