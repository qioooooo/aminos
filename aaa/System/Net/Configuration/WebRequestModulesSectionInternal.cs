using System;
using System.Collections;
using System.Configuration;
using System.Threading;

namespace System.Net.Configuration
{
	// Token: 0x02000670 RID: 1648
	internal sealed class WebRequestModulesSectionInternal
	{
		// Token: 0x060032EB RID: 13035 RVA: 0x000D790C File Offset: 0x000D690C
		internal WebRequestModulesSectionInternal(WebRequestModulesSection section)
		{
			if (section.WebRequestModules.Count > 0)
			{
				this.webRequestModules = new ArrayList(section.WebRequestModules.Count);
				foreach (object obj in section.WebRequestModules)
				{
					WebRequestModuleElement webRequestModuleElement = (WebRequestModuleElement)obj;
					try
					{
						this.webRequestModules.Add(new WebRequestPrefixElement(webRequestModuleElement.Prefix, webRequestModuleElement.Type));
					}
					catch (Exception ex)
					{
						if (NclUtilities.IsFatal(ex))
						{
							throw;
						}
						throw new ConfigurationErrorsException(SR.GetString("net_config_webrequestmodules"), ex);
					}
					catch
					{
						throw new ConfigurationErrorsException(ConfigurationStrings.WebRequestModulesSectionPath, new Exception(SR.GetString("net_nonClsCompliantException")));
					}
				}
			}
		}

		// Token: 0x17000BF4 RID: 3060
		// (get) Token: 0x060032EC RID: 13036 RVA: 0x000D79FC File Offset: 0x000D69FC
		internal static object ClassSyncObject
		{
			get
			{
				if (WebRequestModulesSectionInternal.classSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref WebRequestModulesSectionInternal.classSyncObject, obj, null);
				}
				return WebRequestModulesSectionInternal.classSyncObject;
			}
		}

		// Token: 0x060032ED RID: 13037 RVA: 0x000D7A28 File Offset: 0x000D6A28
		internal static WebRequestModulesSectionInternal GetSection()
		{
			WebRequestModulesSectionInternal webRequestModulesSectionInternal;
			lock (WebRequestModulesSectionInternal.ClassSyncObject)
			{
				WebRequestModulesSection webRequestModulesSection = PrivilegedConfigurationManager.GetSection(ConfigurationStrings.WebRequestModulesSectionPath) as WebRequestModulesSection;
				if (webRequestModulesSection == null)
				{
					webRequestModulesSectionInternal = null;
				}
				else
				{
					webRequestModulesSectionInternal = new WebRequestModulesSectionInternal(webRequestModulesSection);
				}
			}
			return webRequestModulesSectionInternal;
		}

		// Token: 0x17000BF5 RID: 3061
		// (get) Token: 0x060032EE RID: 13038 RVA: 0x000D7A7C File Offset: 0x000D6A7C
		internal ArrayList WebRequestModules
		{
			get
			{
				ArrayList arrayList = this.webRequestModules;
				if (arrayList == null)
				{
					arrayList = new ArrayList(0);
				}
				return arrayList;
			}
		}

		// Token: 0x04002F71 RID: 12145
		private static object classSyncObject;

		// Token: 0x04002F72 RID: 12146
		private ArrayList webRequestModules;
	}
}
