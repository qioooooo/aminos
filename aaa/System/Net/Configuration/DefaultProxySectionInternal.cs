using System;
using System.Configuration;
using System.Globalization;
using System.Reflection;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading;

namespace System.Net.Configuration
{
	// Token: 0x0200064E RID: 1614
	internal sealed class DefaultProxySectionInternal
	{
		// Token: 0x060031F4 RID: 12788 RVA: 0x000D515C File Offset: 0x000D415C
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.ControlPrincipal)]
		internal DefaultProxySectionInternal(DefaultProxySection section)
		{
			if (!section.Enabled)
			{
				return;
			}
			if (section.Proxy.AutoDetect == ProxyElement.AutoDetectValues.Unspecified && section.Proxy.ScriptLocation == null && string.IsNullOrEmpty(section.Module.Type) && section.Proxy.UseSystemDefault != ProxyElement.UseSystemDefaultValues.True && section.Proxy.ProxyAddress == null && section.Proxy.BypassOnLocal == ProxyElement.BypassOnLocalValues.Unspecified && section.BypassList.Count == 0)
			{
				if (section.Proxy.UseSystemDefault == ProxyElement.UseSystemDefaultValues.False)
				{
					this.webProxy = new EmptyWebProxy();
					return;
				}
				try
				{
					using (WindowsIdentity.Impersonate(IntPtr.Zero))
					{
						this.webProxy = new WebRequest.WebProxyWrapper(new WebProxy(true));
					}
					goto IL_02E3;
				}
				catch
				{
					throw;
				}
			}
			if (!string.IsNullOrEmpty(section.Module.Type))
			{
				Type type = Type.GetType(section.Module.Type, true, true);
				if ((type.Attributes & TypeAttributes.VisibilityMask) != TypeAttributes.Public)
				{
					throw new ConfigurationErrorsException(SR.GetString("net_config_proxy_module_not_public"));
				}
				if (!typeof(IWebProxy).IsAssignableFrom(type))
				{
					throw new InvalidCastException(SR.GetString("net_invalid_cast", new object[] { type.FullName, "IWebProxy" }));
				}
				this.webProxy = (IWebProxy)Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, new object[0], CultureInfo.InvariantCulture);
			}
			else
			{
				if (section.Proxy.UseSystemDefault == ProxyElement.UseSystemDefaultValues.True && section.Proxy.AutoDetect == ProxyElement.AutoDetectValues.Unspecified && section.Proxy.ScriptLocation == null)
				{
					try
					{
						using (WindowsIdentity.Impersonate(IntPtr.Zero))
						{
							this.webProxy = new WebProxy(false);
						}
						goto IL_01DE;
					}
					catch
					{
						throw;
					}
				}
				this.webProxy = new WebProxy();
			}
			IL_01DE:
			WebProxy webProxy = this.webProxy as WebProxy;
			if (webProxy != null)
			{
				if (section.Proxy.AutoDetect != ProxyElement.AutoDetectValues.Unspecified)
				{
					webProxy.AutoDetect = section.Proxy.AutoDetect == ProxyElement.AutoDetectValues.True;
				}
				if (section.Proxy.ScriptLocation != null)
				{
					webProxy.ScriptLocation = section.Proxy.ScriptLocation;
				}
				if (section.Proxy.BypassOnLocal != ProxyElement.BypassOnLocalValues.Unspecified)
				{
					webProxy.BypassProxyOnLocal = section.Proxy.BypassOnLocal == ProxyElement.BypassOnLocalValues.True;
				}
				if (section.Proxy.ProxyAddress != null)
				{
					webProxy.Address = section.Proxy.ProxyAddress;
				}
				int count = section.BypassList.Count;
				if (count > 0)
				{
					string[] array = new string[section.BypassList.Count];
					for (int i = 0; i < count; i++)
					{
						array[i] = section.BypassList[i].Address;
					}
					webProxy.BypassList = array;
				}
				if (section.Module.Type == null)
				{
					this.webProxy = new WebRequest.WebProxyWrapper(webProxy);
				}
			}
			IL_02E3:
			if (this.webProxy != null && section.UseDefaultCredentials)
			{
				this.webProxy.Credentials = SystemNetworkCredential.defaultCredential;
			}
		}

		// Token: 0x17000B78 RID: 2936
		// (get) Token: 0x060031F5 RID: 12789 RVA: 0x000D54A0 File Offset: 0x000D44A0
		internal static object ClassSyncObject
		{
			get
			{
				if (DefaultProxySectionInternal.classSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref DefaultProxySectionInternal.classSyncObject, obj, null);
				}
				return DefaultProxySectionInternal.classSyncObject;
			}
		}

		// Token: 0x060031F6 RID: 12790 RVA: 0x000D54CC File Offset: 0x000D44CC
		internal static DefaultProxySectionInternal GetSection()
		{
			DefaultProxySectionInternal defaultProxySectionInternal;
			lock (DefaultProxySectionInternal.ClassSyncObject)
			{
				DefaultProxySection defaultProxySection = PrivilegedConfigurationManager.GetSection(ConfigurationStrings.DefaultProxySectionPath) as DefaultProxySection;
				if (defaultProxySection == null)
				{
					defaultProxySectionInternal = null;
				}
				else
				{
					try
					{
						defaultProxySectionInternal = new DefaultProxySectionInternal(defaultProxySection);
					}
					catch (Exception ex)
					{
						if (NclUtilities.IsFatal(ex))
						{
							throw;
						}
						throw new ConfigurationErrorsException(SR.GetString("net_config_proxy"), ex);
					}
					catch
					{
						throw new ConfigurationErrorsException(SR.GetString("net_config_proxy"), new Exception(SR.GetString("net_nonClsCompliantException")));
					}
				}
			}
			return defaultProxySectionInternal;
		}

		// Token: 0x17000B79 RID: 2937
		// (get) Token: 0x060031F7 RID: 12791 RVA: 0x000D5574 File Offset: 0x000D4574
		internal IWebProxy WebProxy
		{
			get
			{
				return this.webProxy;
			}
		}

		// Token: 0x04002EF1 RID: 12017
		private IWebProxy webProxy;

		// Token: 0x04002EF2 RID: 12018
		private static object classSyncObject;
	}
}
