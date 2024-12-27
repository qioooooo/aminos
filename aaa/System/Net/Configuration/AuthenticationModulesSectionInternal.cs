using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;

namespace System.Net.Configuration
{
	// Token: 0x02000644 RID: 1604
	internal sealed class AuthenticationModulesSectionInternal
	{
		// Token: 0x060031A9 RID: 12713 RVA: 0x000D47C4 File Offset: 0x000D37C4
		internal AuthenticationModulesSectionInternal(AuthenticationModulesSection section)
		{
			if (section.AuthenticationModules.Count > 0)
			{
				this.authenticationModules = new List<Type>(section.AuthenticationModules.Count);
				foreach (object obj in section.AuthenticationModules)
				{
					AuthenticationModuleElement authenticationModuleElement = (AuthenticationModuleElement)obj;
					Type type = null;
					try
					{
						type = Type.GetType(authenticationModuleElement.Type, true, true);
						if (!typeof(IAuthenticationModule).IsAssignableFrom(type))
						{
							throw new InvalidCastException(SR.GetString("net_invalid_cast", new object[] { type.FullName, "IAuthenticationModule" }));
						}
					}
					catch (Exception ex)
					{
						if (NclUtilities.IsFatal(ex))
						{
							throw;
						}
						throw new ConfigurationErrorsException(SR.GetString("net_config_authenticationmodules"), ex);
					}
					catch
					{
						throw new ConfigurationErrorsException(SR.GetString("net_config_authenticationmodules"), new Exception(SR.GetString("net_nonClsCompliantException")));
					}
					this.authenticationModules.Add(type);
				}
			}
		}

		// Token: 0x17000B58 RID: 2904
		// (get) Token: 0x060031AA RID: 12714 RVA: 0x000D4900 File Offset: 0x000D3900
		internal List<Type> AuthenticationModules
		{
			get
			{
				List<Type> list = this.authenticationModules;
				if (list == null)
				{
					list = new List<Type>(0);
				}
				return list;
			}
		}

		// Token: 0x17000B59 RID: 2905
		// (get) Token: 0x060031AB RID: 12715 RVA: 0x000D4920 File Offset: 0x000D3920
		internal static object ClassSyncObject
		{
			get
			{
				if (AuthenticationModulesSectionInternal.classSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref AuthenticationModulesSectionInternal.classSyncObject, obj, null);
				}
				return AuthenticationModulesSectionInternal.classSyncObject;
			}
		}

		// Token: 0x060031AC RID: 12716 RVA: 0x000D494C File Offset: 0x000D394C
		internal static AuthenticationModulesSectionInternal GetSection()
		{
			AuthenticationModulesSectionInternal authenticationModulesSectionInternal;
			lock (AuthenticationModulesSectionInternal.ClassSyncObject)
			{
				AuthenticationModulesSection authenticationModulesSection = PrivilegedConfigurationManager.GetSection(ConfigurationStrings.AuthenticationModulesSectionPath) as AuthenticationModulesSection;
				if (authenticationModulesSection == null)
				{
					authenticationModulesSectionInternal = null;
				}
				else
				{
					authenticationModulesSectionInternal = new AuthenticationModulesSectionInternal(authenticationModulesSection);
				}
			}
			return authenticationModulesSectionInternal;
		}

		// Token: 0x04002E96 RID: 11926
		private List<Type> authenticationModules;

		// Token: 0x04002E97 RID: 11927
		private static object classSyncObject;
	}
}
