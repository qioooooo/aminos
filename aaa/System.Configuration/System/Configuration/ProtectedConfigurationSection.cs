using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Security.Permissions;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x0200008A RID: 138
	public sealed class ProtectedConfigurationSection : ConfigurationSection
	{
		// Token: 0x0600050C RID: 1292 RVA: 0x000196E0 File Offset: 0x000186E0
		internal ProtectedConfigurationProvider GetProviderFromName(string providerName)
		{
			ProviderSettings providerSettings = this.Providers[providerName];
			if (providerSettings == null)
			{
				throw new Exception(SR.GetString("ProtectedConfigurationProvider_not_found", new object[] { providerName }));
			}
			return this.InstantiateProvider(providerSettings);
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x00019720 File Offset: 0x00018720
		internal ProtectedConfigurationProviderCollection GetAllProviders()
		{
			ProtectedConfigurationProviderCollection protectedConfigurationProviderCollection = new ProtectedConfigurationProviderCollection();
			foreach (object obj in this.Providers)
			{
				ProviderSettings providerSettings = (ProviderSettings)obj;
				protectedConfigurationProviderCollection.Add(this.InstantiateProvider(providerSettings));
			}
			return protectedConfigurationProviderCollection;
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x00019788 File Offset: 0x00018788
		[PermissionSet(SecurityAction.Assert, Unrestricted = true)]
		private ProtectedConfigurationProvider CreateAndInitializeProviderWithAssert(Type t, ProviderSettings pn)
		{
			ProtectedConfigurationProvider protectedConfigurationProvider = (ProtectedConfigurationProvider)TypeUtil.CreateInstanceWithReflectionPermission(t);
			NameValueCollection parameters = pn.Parameters;
			NameValueCollection nameValueCollection = new NameValueCollection(parameters.Count);
			foreach (object obj in parameters)
			{
				string text = (string)obj;
				nameValueCollection[text] = parameters[text];
			}
			protectedConfigurationProvider.Initialize(pn.Name, nameValueCollection);
			return protectedConfigurationProvider;
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x00019818 File Offset: 0x00018818
		private ProtectedConfigurationProvider InstantiateProvider(ProviderSettings pn)
		{
			Type typeWithReflectionPermission = TypeUtil.GetTypeWithReflectionPermission(pn.Type, true);
			if (!typeof(ProtectedConfigurationProvider).IsAssignableFrom(typeWithReflectionPermission))
			{
				throw new Exception(SR.GetString("WrongType_of_Protected_provider"));
			}
			if (!TypeUtil.IsTypeAllowedInConfig(typeWithReflectionPermission))
			{
				throw new Exception(SR.GetString("Type_from_untrusted_assembly", new object[] { typeWithReflectionPermission.FullName }));
			}
			return this.CreateAndInitializeProviderWithAssert(typeWithReflectionPermission, pn);
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x00019888 File Offset: 0x00018888
		internal static string DecryptSection(string encryptedXml, ProtectedConfigurationProvider provider)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(encryptedXml);
			XmlNode xmlNode = provider.Decrypt(xmlDocument.DocumentElement);
			return xmlNode.OuterXml;
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x000198B8 File Offset: 0x000188B8
		internal static string FormatEncryptedSection(string encryptedXml, string sectionName, string providerName)
		{
			return string.Format(CultureInfo.InvariantCulture, "<{0} {1}=\"{2}\"> {3} </{0}>", new object[] { sectionName, "configProtectionProvider", providerName, encryptedXml });
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x000198F0 File Offset: 0x000188F0
		internal static string EncryptSection(string clearXml, ProtectedConfigurationProvider provider)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.PreserveWhitespace = true;
			xmlDocument.LoadXml(clearXml);
			string name = xmlDocument.DocumentElement.Name;
			XmlNode xmlNode = provider.Encrypt(xmlDocument.DocumentElement);
			return xmlNode.OuterXml;
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x00019930 File Offset: 0x00018930
		static ProtectedConfigurationSection()
		{
			ProtectedConfigurationSection._properties.Add(ProtectedConfigurationSection._propProviders);
			ProtectedConfigurationSection._properties.Add(ProtectedConfigurationSection._propDefaultProvider);
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06000515 RID: 1301 RVA: 0x000199B1 File Offset: 0x000189B1
		protected internal override ConfigurationPropertyCollection Properties
		{
			get
			{
				return ProtectedConfigurationSection._properties;
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000516 RID: 1302 RVA: 0x000199B8 File Offset: 0x000189B8
		private ProtectedProviderSettings _Providers
		{
			get
			{
				return (ProtectedProviderSettings)base[ProtectedConfigurationSection._propProviders];
			}
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000517 RID: 1303 RVA: 0x000199CA File Offset: 0x000189CA
		[ConfigurationProperty("providers")]
		public ProviderSettingsCollection Providers
		{
			get
			{
				return this._Providers.Providers;
			}
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000518 RID: 1304 RVA: 0x000199D7 File Offset: 0x000189D7
		// (set) Token: 0x06000519 RID: 1305 RVA: 0x000199E9 File Offset: 0x000189E9
		[ConfigurationProperty("defaultProvider", DefaultValue = "RsaProtectedConfigurationProvider")]
		public string DefaultProvider
		{
			get
			{
				return (string)base[ProtectedConfigurationSection._propDefaultProvider];
			}
			set
			{
				base[ProtectedConfigurationSection._propDefaultProvider] = value;
			}
		}

		// Token: 0x0400036E RID: 878
		private const string EncryptedSectionTemplate = "<{0} {1}=\"{2}\"> {3} </{0}>";

		// Token: 0x0400036F RID: 879
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x04000370 RID: 880
		private static readonly ConfigurationProperty _propProviders = new ConfigurationProperty("providers", typeof(ProtectedProviderSettings), new ProtectedProviderSettings(), ConfigurationPropertyOptions.None);

		// Token: 0x04000371 RID: 881
		private static readonly ConfigurationProperty _propDefaultProvider = new ConfigurationProperty("defaultProvider", typeof(string), "RsaProtectedConfigurationProvider", null, ConfigurationProperty.NonEmptyStringValidator, ConfigurationPropertyOptions.None);
	}
}
