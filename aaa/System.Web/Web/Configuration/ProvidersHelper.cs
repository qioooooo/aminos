using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Security.Permissions;

namespace System.Web.Configuration
{
	// Token: 0x02000235 RID: 565
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public static class ProvidersHelper
	{
		// Token: 0x06001E3F RID: 7743 RVA: 0x0008773C File Offset: 0x0008673C
		[AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Low)]
		public static ProviderBase InstantiateProvider(ProviderSettings providerSettings, Type providerType)
		{
			ProviderBase providerBase = null;
			try
			{
				string text = ((providerSettings.Type == null) ? null : providerSettings.Type.Trim());
				if (string.IsNullOrEmpty(text))
				{
					throw new ArgumentException(SR.GetString("Provider_no_type_name"));
				}
				Type type = ConfigUtil.GetType(text, "type", providerSettings, true, true);
				if (!providerType.IsAssignableFrom(type))
				{
					throw new ArgumentException(SR.GetString("Provider_must_implement_type", new object[] { providerType.ToString() }));
				}
				providerBase = (ProviderBase)HttpRuntime.CreatePublicInstance(type);
				NameValueCollection parameters = providerSettings.Parameters;
				NameValueCollection nameValueCollection = new NameValueCollection(parameters.Count, StringComparer.Ordinal);
				foreach (object obj in parameters)
				{
					string text2 = (string)obj;
					nameValueCollection[text2] = parameters[text2];
				}
				providerBase.Initialize(providerSettings.Name, nameValueCollection);
			}
			catch (Exception ex)
			{
				if (ex is ConfigurationException)
				{
					throw;
				}
				throw new ConfigurationErrorsException(ex.Message, providerSettings.ElementInformation.Properties["type"].Source, providerSettings.ElementInformation.Properties["type"].LineNumber);
			}
			return providerBase;
		}

		// Token: 0x06001E40 RID: 7744 RVA: 0x000878A0 File Offset: 0x000868A0
		[AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Low)]
		public static void InstantiateProviders(ProviderSettingsCollection configProviders, ProviderCollection providers, Type providerType)
		{
			foreach (object obj in configProviders)
			{
				ProviderSettings providerSettings = (ProviderSettings)obj;
				providers.Add(ProvidersHelper.InstantiateProvider(providerSettings, providerType));
			}
		}
	}
}
