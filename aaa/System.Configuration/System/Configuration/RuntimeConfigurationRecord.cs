using System;
using System.Configuration.Internal;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x02000092 RID: 146
	internal sealed class RuntimeConfigurationRecord : BaseConfigurationRecord
	{
		// Token: 0x06000552 RID: 1362 RVA: 0x0001A67C File Offset: 0x0001967C
		internal static IInternalConfigRecord Create(InternalConfigRoot configRoot, IInternalConfigRecord parent, string configPath)
		{
			RuntimeConfigurationRecord runtimeConfigurationRecord = new RuntimeConfigurationRecord();
			runtimeConfigurationRecord.Init(configRoot, (BaseConfigurationRecord)parent, configPath, null);
			return runtimeConfigurationRecord;
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x0001A69F File Offset: 0x0001969F
		private RuntimeConfigurationRecord()
		{
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06000554 RID: 1364 RVA: 0x0001A6A7 File Offset: 0x000196A7
		protected override SimpleBitVector32 ClassFlags
		{
			get
			{
				return RuntimeConfigurationRecord.RuntimeClassFlags;
			}
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x0001A6AE File Offset: 0x000196AE
		protected override object CreateSectionFactory(FactoryRecord factoryRecord)
		{
			return new RuntimeConfigurationRecord.RuntimeConfigurationFactory(this, factoryRecord);
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x0001A6B8 File Offset: 0x000196B8
		protected override object CreateSection(bool inputIsTrusted, FactoryRecord factoryRecord, SectionRecord sectionRecord, object parentConfig, ConfigXmlReader reader)
		{
			RuntimeConfigurationRecord.RuntimeConfigurationFactory runtimeConfigurationFactory = (RuntimeConfigurationRecord.RuntimeConfigurationFactory)factoryRecord.Factory;
			return runtimeConfigurationFactory.CreateSection(inputIsTrusted, this, factoryRecord, sectionRecord, parentConfig, reader);
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x0001A6E1 File Offset: 0x000196E1
		protected override object UseParentResult(string configKey, object parentResult, SectionRecord sectionRecord)
		{
			return parentResult;
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x0001A6E4 File Offset: 0x000196E4
		[PermissionSet(SecurityAction.Assert, Unrestricted = true)]
		private object GetRuntimeObjectWithFullTrust(ConfigurationSection section)
		{
			return section.GetRuntimeObject();
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x0001A6EC File Offset: 0x000196EC
		private object GetRuntimeObjectWithRestrictedPermissions(ConfigurationSection section)
		{
			bool flag = false;
			object runtimeObject;
			try
			{
				PermissionSet restrictedPermissions = base.GetRestrictedPermissions();
				if (restrictedPermissions != null)
				{
					restrictedPermissions.PermitOnly();
					flag = true;
				}
				runtimeObject = section.GetRuntimeObject();
			}
			finally
			{
				if (flag)
				{
					CodeAccessPermission.RevertPermitOnly();
				}
			}
			return runtimeObject;
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x0001A730 File Offset: 0x00019730
		protected override object GetRuntimeObject(object result)
		{
			ConfigurationSection configurationSection = result as ConfigurationSection;
			object obj;
			if (configurationSection == null)
			{
				obj = result;
			}
			else
			{
				try
				{
					using (base.Impersonate())
					{
						if (this._flags[8192])
						{
							obj = this.GetRuntimeObjectWithFullTrust(configurationSection);
						}
						else
						{
							obj = this.GetRuntimeObjectWithRestrictedPermissions(configurationSection);
						}
					}
				}
				catch (Exception ex)
				{
					throw new ConfigurationErrorsException(SR.GetString("Config_exception_in_config_section_handler", new object[] { configurationSection.SectionInformation.SectionName }), ex);
				}
				catch
				{
					throw new ConfigurationErrorsException(SR.GetString("Config_exception_in_config_section_handler", new object[] { configurationSection.SectionInformation.SectionName }));
				}
			}
			return obj;
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x0001A808 File Offset: 0x00019808
		[PermissionSet(SecurityAction.Assert, Unrestricted = true)]
		protected override string CallHostDecryptSection(string encryptedXml, ProtectedConfigurationProvider protectionProvider, ProtectedConfigurationSection protectedConfig)
		{
			return base.CallHostDecryptSection(encryptedXml, protectionProvider, protectedConfig);
		}

		// Token: 0x04000384 RID: 900
		private static readonly SimpleBitVector32 RuntimeClassFlags = new SimpleBitVector32(47);

		// Token: 0x02000093 RID: 147
		private class RuntimeConfigurationFactory
		{
			// Token: 0x0600055D RID: 1373 RVA: 0x0001A821 File Offset: 0x00019821
			internal RuntimeConfigurationFactory(RuntimeConfigurationRecord configRecord, FactoryRecord factoryRecord)
			{
				if (factoryRecord.IsFromTrustedConfigRecord)
				{
					this.InitWithFullTrust(configRecord, factoryRecord);
					return;
				}
				this.InitWithRestrictedPermissions(configRecord, factoryRecord);
			}

			// Token: 0x0600055E RID: 1374 RVA: 0x0001A844 File Offset: 0x00019844
			private void Init(RuntimeConfigurationRecord configRecord, FactoryRecord factoryRecord)
			{
				Type typeWithReflectionPermission = TypeUtil.GetTypeWithReflectionPermission(configRecord.Host, factoryRecord.FactoryTypeName, true);
				if (typeof(ConfigurationSection).IsAssignableFrom(typeWithReflectionPermission))
				{
					this._sectionCtor = TypeUtil.GetConstructorWithReflectionPermission(typeWithReflectionPermission, typeof(ConfigurationSection), true);
					return;
				}
				TypeUtil.VerifyAssignableType(typeof(IConfigurationSectionHandler), typeWithReflectionPermission, true);
				this._sectionHandler = (IConfigurationSectionHandler)TypeUtil.CreateInstanceWithReflectionPermission(typeWithReflectionPermission);
			}

			// Token: 0x0600055F RID: 1375 RVA: 0x0001A8B1 File Offset: 0x000198B1
			[PermissionSet(SecurityAction.Assert, Unrestricted = true)]
			private void InitWithFullTrust(RuntimeConfigurationRecord configRecord, FactoryRecord factoryRecord)
			{
				this.Init(configRecord, factoryRecord);
			}

			// Token: 0x06000560 RID: 1376 RVA: 0x0001A8BC File Offset: 0x000198BC
			private void InitWithRestrictedPermissions(RuntimeConfigurationRecord configRecord, FactoryRecord factoryRecord)
			{
				bool flag = false;
				try
				{
					PermissionSet restrictedPermissions = configRecord.GetRestrictedPermissions();
					if (restrictedPermissions != null)
					{
						restrictedPermissions.PermitOnly();
						flag = true;
					}
					this.Init(configRecord, factoryRecord);
				}
				finally
				{
					if (flag)
					{
						CodeAccessPermission.RevertPermitOnly();
					}
				}
			}

			// Token: 0x06000561 RID: 1377 RVA: 0x0001A900 File Offset: 0x00019900
			private static void CheckForLockAttributes(string sectionName, XmlNode xmlNode)
			{
				XmlAttributeCollection attributes = xmlNode.Attributes;
				if (attributes != null)
				{
					foreach (object obj in attributes)
					{
						XmlAttribute xmlAttribute = (XmlAttribute)obj;
						if (ConfigurationElement.IsLockAttributeName(xmlAttribute.Name))
						{
							throw new ConfigurationErrorsException(SR.GetString("Config_element_locking_not_supported", new object[] { sectionName }), xmlAttribute);
						}
					}
				}
				foreach (object obj2 in xmlNode.ChildNodes)
				{
					XmlNode xmlNode2 = (XmlNode)obj2;
					if (xmlNode.NodeType == XmlNodeType.Element)
					{
						RuntimeConfigurationRecord.RuntimeConfigurationFactory.CheckForLockAttributes(sectionName, xmlNode2);
					}
				}
			}

			// Token: 0x06000562 RID: 1378 RVA: 0x0001A9E0 File Offset: 0x000199E0
			private object CreateSectionImpl(RuntimeConfigurationRecord configRecord, FactoryRecord factoryRecord, SectionRecord sectionRecord, object parentConfig, ConfigXmlReader reader)
			{
				object obj;
				if (this._sectionCtor != null)
				{
					ConfigurationSection configurationSection = (ConfigurationSection)TypeUtil.InvokeCtorWithReflectionPermission(this._sectionCtor);
					configurationSection.SectionInformation.SetRuntimeConfigurationInformation(configRecord, factoryRecord, sectionRecord);
					configurationSection.CallInit();
					ConfigurationSection configurationSection2 = (ConfigurationSection)parentConfig;
					configurationSection.Reset(configurationSection2);
					if (reader != null)
					{
						configurationSection.DeserializeSection(reader);
					}
					ConfigurationErrorsException errors = configurationSection.GetErrors();
					if (errors != null)
					{
						throw errors;
					}
					configurationSection.SetReadOnly();
					configurationSection.ResetModified();
					obj = configurationSection;
				}
				else if (reader != null)
				{
					XmlNode xmlNode = ErrorInfoXmlDocument.CreateSectionXmlNode(reader);
					RuntimeConfigurationRecord.RuntimeConfigurationFactory.CheckForLockAttributes(factoryRecord.ConfigKey, xmlNode);
					object obj2 = configRecord.Host.CreateDeprecatedConfigContext(configRecord.ConfigPath);
					obj = this._sectionHandler.Create(parentConfig, obj2, xmlNode);
				}
				else
				{
					obj = null;
				}
				return obj;
			}

			// Token: 0x06000563 RID: 1379 RVA: 0x0001AA95 File Offset: 0x00019A95
			[PermissionSet(SecurityAction.Assert, Unrestricted = true)]
			private object CreateSectionWithFullTrust(RuntimeConfigurationRecord configRecord, FactoryRecord factoryRecord, SectionRecord sectionRecord, object parentConfig, ConfigXmlReader reader)
			{
				return this.CreateSectionImpl(configRecord, factoryRecord, sectionRecord, parentConfig, reader);
			}

			// Token: 0x06000564 RID: 1380 RVA: 0x0001AAA4 File Offset: 0x00019AA4
			private object CreateSectionWithRestrictedPermissions(RuntimeConfigurationRecord configRecord, FactoryRecord factoryRecord, SectionRecord sectionRecord, object parentConfig, ConfigXmlReader reader)
			{
				bool flag = false;
				object obj;
				try
				{
					PermissionSet restrictedPermissions = configRecord.GetRestrictedPermissions();
					if (restrictedPermissions != null)
					{
						restrictedPermissions.PermitOnly();
						flag = true;
					}
					obj = this.CreateSectionImpl(configRecord, factoryRecord, sectionRecord, parentConfig, reader);
				}
				finally
				{
					if (flag)
					{
						CodeAccessPermission.RevertPermitOnly();
					}
				}
				return obj;
			}

			// Token: 0x06000565 RID: 1381 RVA: 0x0001AAF0 File Offset: 0x00019AF0
			internal object CreateSection(bool inputIsTrusted, RuntimeConfigurationRecord configRecord, FactoryRecord factoryRecord, SectionRecord sectionRecord, object parentConfig, ConfigXmlReader reader)
			{
				if (inputIsTrusted)
				{
					return this.CreateSectionWithFullTrust(configRecord, factoryRecord, sectionRecord, parentConfig, reader);
				}
				return this.CreateSectionWithRestrictedPermissions(configRecord, factoryRecord, sectionRecord, parentConfig, reader);
			}

			// Token: 0x04000385 RID: 901
			private ConstructorInfo _sectionCtor;

			// Token: 0x04000386 RID: 902
			private IConfigurationSectionHandler _sectionHandler;
		}
	}
}
