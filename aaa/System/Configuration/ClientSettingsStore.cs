using System;
using System.Collections;
using System.Configuration.Internal;
using System.IO;
using System.Security.Permissions;

namespace System.Configuration
{
	// Token: 0x020006E7 RID: 1767
	internal sealed class ClientSettingsStore
	{
		// Token: 0x06003689 RID: 13961 RVA: 0x000E8C54 File Offset: 0x000E7C54
		private Configuration GetUserConfig(bool isRoaming)
		{
			ConfigurationUserLevel configurationUserLevel = (isRoaming ? ConfigurationUserLevel.PerUserRoaming : ConfigurationUserLevel.PerUserRoamingAndLocal);
			return ClientSettingsStore.ClientSettingsConfigurationHost.OpenExeConfiguration(configurationUserLevel);
		}

		// Token: 0x0600368A RID: 13962 RVA: 0x000E8C74 File Offset: 0x000E7C74
		private ClientSettingsSection GetConfigSection(Configuration config, string sectionName, bool declare)
		{
			string text = "userSettings/" + sectionName;
			ClientSettingsSection clientSettingsSection = null;
			if (config != null)
			{
				clientSettingsSection = config.GetSection(text) as ClientSettingsSection;
				if (clientSettingsSection == null && declare)
				{
					this.DeclareSection(config, sectionName);
					clientSettingsSection = config.GetSection(text) as ClientSettingsSection;
				}
			}
			return clientSettingsSection;
		}

		// Token: 0x0600368B RID: 13963 RVA: 0x000E8CBC File Offset: 0x000E7CBC
		private void DeclareSection(Configuration config, string sectionName)
		{
			if (config.GetSectionGroup("userSettings") == null)
			{
				ConfigurationSectionGroup configurationSectionGroup = new UserSettingsGroup();
				config.SectionGroups.Add("userSettings", configurationSectionGroup);
			}
			ConfigurationSectionGroup sectionGroup = config.GetSectionGroup("userSettings");
			if (sectionGroup != null && sectionGroup.Sections[sectionName] == null)
			{
				ConfigurationSection configurationSection = new ClientSettingsSection();
				configurationSection.SectionInformation.AllowExeDefinition = ConfigurationAllowExeDefinition.MachineToLocalUser;
				configurationSection.SectionInformation.RequirePermission = false;
				sectionGroup.Sections.Add(sectionName, configurationSection);
			}
		}

		// Token: 0x0600368C RID: 13964 RVA: 0x000E8D40 File Offset: 0x000E7D40
		internal IDictionary ReadSettings(string sectionName, bool isUserScoped)
		{
			IDictionary dictionary = new Hashtable();
			if (isUserScoped && !ConfigurationManagerInternalFactory.Instance.SupportsUserConfig)
			{
				return dictionary;
			}
			string text = (isUserScoped ? "userSettings/" : "applicationSettings/");
			ConfigurationManager.RefreshSection(text + sectionName);
			ClientSettingsSection clientSettingsSection = ConfigurationManager.GetSection(text + sectionName) as ClientSettingsSection;
			if (clientSettingsSection != null)
			{
				foreach (object obj in clientSettingsSection.Settings)
				{
					SettingElement settingElement = (SettingElement)obj;
					dictionary[settingElement.Name] = new StoredSetting(settingElement.SerializeAs, settingElement.Value.ValueXml);
				}
			}
			return dictionary;
		}

		// Token: 0x0600368D RID: 13965 RVA: 0x000E8E0C File Offset: 0x000E7E0C
		internal static IDictionary ReadSettingsFromFile(string configFileName, string sectionName, bool isUserScoped)
		{
			IDictionary dictionary = new Hashtable();
			if (isUserScoped && !ConfigurationManagerInternalFactory.Instance.SupportsUserConfig)
			{
				return dictionary;
			}
			string text = (isUserScoped ? "userSettings/" : "applicationSettings/");
			ExeConfigurationFileMap exeConfigurationFileMap = new ExeConfigurationFileMap();
			ConfigurationUserLevel configurationUserLevel = (isUserScoped ? ConfigurationUserLevel.PerUserRoaming : ConfigurationUserLevel.None);
			if (isUserScoped)
			{
				exeConfigurationFileMap.ExeConfigFilename = ConfigurationManagerInternalFactory.Instance.ApplicationConfigUri;
				exeConfigurationFileMap.RoamingUserConfigFilename = configFileName;
			}
			else
			{
				exeConfigurationFileMap.ExeConfigFilename = configFileName;
			}
			Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(exeConfigurationFileMap, configurationUserLevel);
			ClientSettingsSection clientSettingsSection = configuration.GetSection(text + sectionName) as ClientSettingsSection;
			if (clientSettingsSection != null)
			{
				foreach (object obj in clientSettingsSection.Settings)
				{
					SettingElement settingElement = (SettingElement)obj;
					dictionary[settingElement.Name] = new StoredSetting(settingElement.SerializeAs, settingElement.Value.ValueXml);
				}
			}
			return dictionary;
		}

		// Token: 0x0600368E RID: 13966 RVA: 0x000E8F10 File Offset: 0x000E7F10
		internal ConnectionStringSettingsCollection ReadConnectionStrings()
		{
			return PrivilegedConfigurationManager.ConnectionStrings;
		}

		// Token: 0x0600368F RID: 13967 RVA: 0x000E8F18 File Offset: 0x000E7F18
		internal void RevertToParent(string sectionName, bool isRoaming)
		{
			if (!ConfigurationManagerInternalFactory.Instance.SupportsUserConfig)
			{
				throw new ConfigurationErrorsException(SR.GetString("UserSettingsNotSupported"));
			}
			Configuration userConfig = this.GetUserConfig(isRoaming);
			ClientSettingsSection configSection = this.GetConfigSection(userConfig, sectionName, false);
			if (configSection != null)
			{
				configSection.SectionInformation.RevertToParent();
				userConfig.Save();
			}
		}

		// Token: 0x06003690 RID: 13968 RVA: 0x000E8F68 File Offset: 0x000E7F68
		internal void WriteSettings(string sectionName, bool isRoaming, IDictionary newSettings)
		{
			if (!ConfigurationManagerInternalFactory.Instance.SupportsUserConfig)
			{
				throw new ConfigurationErrorsException(SR.GetString("UserSettingsNotSupported"));
			}
			Configuration userConfig = this.GetUserConfig(isRoaming);
			ClientSettingsSection configSection = this.GetConfigSection(userConfig, sectionName, true);
			if (configSection != null)
			{
				SettingElementCollection settings = configSection.Settings;
				foreach (object obj in newSettings)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					SettingElement settingElement = settings.Get((string)dictionaryEntry.Key);
					if (settingElement == null)
					{
						settingElement = new SettingElement();
						settingElement.Name = (string)dictionaryEntry.Key;
						settings.Add(settingElement);
					}
					StoredSetting storedSetting = (StoredSetting)dictionaryEntry.Value;
					settingElement.SerializeAs = storedSetting.SerializeAs;
					settingElement.Value.ValueXml = storedSetting.Value;
				}
				try
				{
					userConfig.Save();
					return;
				}
				catch (ConfigurationErrorsException ex)
				{
					throw new ConfigurationErrorsException(SR.GetString("SettingsSaveFailed", new object[] { ex.Message }), ex);
				}
			}
			throw new ConfigurationErrorsException(SR.GetString("SettingsSaveFailedNoSection"));
		}

		// Token: 0x04003188 RID: 12680
		private const string ApplicationSettingsGroupName = "applicationSettings";

		// Token: 0x04003189 RID: 12681
		private const string UserSettingsGroupName = "userSettings";

		// Token: 0x0400318A RID: 12682
		private const string ApplicationSettingsGroupPrefix = "applicationSettings/";

		// Token: 0x0400318B RID: 12683
		private const string UserSettingsGroupPrefix = "userSettings/";

		// Token: 0x020006E8 RID: 1768
		private sealed class ClientSettingsConfigurationHost : DelegatingConfigHost
		{
			// Token: 0x17000C9D RID: 3229
			// (get) Token: 0x06003692 RID: 13970 RVA: 0x000E90B8 File Offset: 0x000E80B8
			private IInternalConfigClientHost ClientHost
			{
				get
				{
					return (IInternalConfigClientHost)base.Host;
				}
			}

			// Token: 0x17000C9E RID: 3230
			// (get) Token: 0x06003693 RID: 13971 RVA: 0x000E90C5 File Offset: 0x000E80C5
			internal static IInternalConfigConfigurationFactory ConfigFactory
			{
				get
				{
					if (ClientSettingsStore.ClientSettingsConfigurationHost.s_configFactory == null)
					{
						ClientSettingsStore.ClientSettingsConfigurationHost.s_configFactory = (IInternalConfigConfigurationFactory)TypeUtil.CreateInstanceWithReflectionPermission("System.Configuration.Internal.InternalConfigConfigurationFactory,System.Configuration, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
					}
					return ClientSettingsStore.ClientSettingsConfigurationHost.s_configFactory;
				}
			}

			// Token: 0x06003694 RID: 13972 RVA: 0x000E90E7 File Offset: 0x000E80E7
			private ClientSettingsConfigurationHost()
			{
			}

			// Token: 0x06003695 RID: 13973 RVA: 0x000E90EF File Offset: 0x000E80EF
			public override void Init(IInternalConfigRoot configRoot, params object[] hostInitParams)
			{
			}

			// Token: 0x06003696 RID: 13974 RVA: 0x000E90F4 File Offset: 0x000E80F4
			public override void InitForConfiguration(ref string locationSubPath, out string configPath, out string locationConfigPath, IInternalConfigRoot configRoot, params object[] hostInitConfigurationParams)
			{
				ConfigurationUserLevel configurationUserLevel = (ConfigurationUserLevel)hostInitConfigurationParams[0];
				base.Host = (IInternalConfigHost)TypeUtil.CreateInstanceWithReflectionPermission("System.Configuration.ClientConfigurationHost,System.Configuration, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
				ConfigurationUserLevel configurationUserLevel2 = configurationUserLevel;
				string text;
				if (configurationUserLevel2 != ConfigurationUserLevel.None)
				{
					if (configurationUserLevel2 != ConfigurationUserLevel.PerUserRoaming)
					{
						if (configurationUserLevel2 != ConfigurationUserLevel.PerUserRoamingAndLocal)
						{
							throw new ArgumentException(SR.GetString("UnknownUserLevel"));
						}
						text = this.ClientHost.GetLocalUserConfigPath();
					}
					else
					{
						text = this.ClientHost.GetRoamingUserConfigPath();
					}
				}
				else
				{
					text = this.ClientHost.GetExeConfigPath();
				}
				base.Host.InitForConfiguration(ref locationSubPath, out configPath, out locationConfigPath, configRoot, new object[] { null, null, text });
			}

			// Token: 0x06003697 RID: 13975 RVA: 0x000E918C File Offset: 0x000E818C
			private bool IsKnownConfigFile(string filename)
			{
				return string.Equals(filename, ConfigurationManagerInternalFactory.Instance.MachineConfigPath, StringComparison.OrdinalIgnoreCase) || string.Equals(filename, ConfigurationManagerInternalFactory.Instance.ApplicationConfigUri, StringComparison.OrdinalIgnoreCase) || string.Equals(filename, ConfigurationManagerInternalFactory.Instance.ExeLocalConfigPath, StringComparison.OrdinalIgnoreCase) || string.Equals(filename, ConfigurationManagerInternalFactory.Instance.ExeRoamingConfigPath, StringComparison.OrdinalIgnoreCase);
			}

			// Token: 0x06003698 RID: 13976 RVA: 0x000E91E8 File Offset: 0x000E81E8
			internal static Configuration OpenExeConfiguration(ConfigurationUserLevel userLevel)
			{
				return ClientSettingsStore.ClientSettingsConfigurationHost.ConfigFactory.Create(typeof(ClientSettingsStore.ClientSettingsConfigurationHost), new object[] { userLevel });
			}

			// Token: 0x06003699 RID: 13977 RVA: 0x000E921A File Offset: 0x000E821A
			public override Stream OpenStreamForRead(string streamName)
			{
				if (this.IsKnownConfigFile(streamName))
				{
					return base.Host.OpenStreamForRead(streamName, true);
				}
				return base.Host.OpenStreamForRead(streamName);
			}

			// Token: 0x0600369A RID: 13978 RVA: 0x000E9240 File Offset: 0x000E8240
			public override Stream OpenStreamForWrite(string streamName, string templateStreamName, ref object writeContext)
			{
				Stream stream;
				if (string.Equals(streamName, ConfigurationManagerInternalFactory.Instance.ExeLocalConfigPath, StringComparison.OrdinalIgnoreCase))
				{
					stream = new ClientSettingsStore.QuotaEnforcedStream(base.Host.OpenStreamForWrite(streamName, templateStreamName, ref writeContext, true), false);
				}
				else if (string.Equals(streamName, ConfigurationManagerInternalFactory.Instance.ExeRoamingConfigPath, StringComparison.OrdinalIgnoreCase))
				{
					stream = new ClientSettingsStore.QuotaEnforcedStream(base.Host.OpenStreamForWrite(streamName, templateStreamName, ref writeContext, true), true);
				}
				else
				{
					stream = base.Host.OpenStreamForWrite(streamName, templateStreamName, ref writeContext);
				}
				return stream;
			}

			// Token: 0x0600369B RID: 13979 RVA: 0x000E92B8 File Offset: 0x000E82B8
			public override void WriteCompleted(string streamName, bool success, object writeContext)
			{
				if (string.Equals(streamName, ConfigurationManagerInternalFactory.Instance.ExeLocalConfigPath, StringComparison.OrdinalIgnoreCase) || string.Equals(streamName, ConfigurationManagerInternalFactory.Instance.ExeRoamingConfigPath, StringComparison.OrdinalIgnoreCase))
				{
					base.Host.WriteCompleted(streamName, success, writeContext, true);
					return;
				}
				base.Host.WriteCompleted(streamName, success, writeContext);
			}

			// Token: 0x0400318C RID: 12684
			private const string ClientConfigurationHostTypeName = "System.Configuration.ClientConfigurationHost,System.Configuration, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";

			// Token: 0x0400318D RID: 12685
			private const string InternalConfigConfigurationFactoryTypeName = "System.Configuration.Internal.InternalConfigConfigurationFactory,System.Configuration, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";

			// Token: 0x0400318E RID: 12686
			private static IInternalConfigConfigurationFactory s_configFactory;
		}

		// Token: 0x020006E9 RID: 1769
		private sealed class QuotaEnforcedStream : Stream
		{
			// Token: 0x0600369C RID: 13980 RVA: 0x000E9309 File Offset: 0x000E8309
			internal QuotaEnforcedStream(Stream originalStream, bool isRoaming)
			{
				this._originalStream = originalStream;
				this._isRoaming = isRoaming;
			}

			// Token: 0x17000C9F RID: 3231
			// (get) Token: 0x0600369D RID: 13981 RVA: 0x000E931F File Offset: 0x000E831F
			public override bool CanRead
			{
				get
				{
					return this._originalStream.CanRead;
				}
			}

			// Token: 0x17000CA0 RID: 3232
			// (get) Token: 0x0600369E RID: 13982 RVA: 0x000E932C File Offset: 0x000E832C
			public override bool CanWrite
			{
				get
				{
					return this._originalStream.CanWrite;
				}
			}

			// Token: 0x17000CA1 RID: 3233
			// (get) Token: 0x0600369F RID: 13983 RVA: 0x000E9339 File Offset: 0x000E8339
			public override bool CanSeek
			{
				get
				{
					return this._originalStream.CanSeek;
				}
			}

			// Token: 0x17000CA2 RID: 3234
			// (get) Token: 0x060036A0 RID: 13984 RVA: 0x000E9346 File Offset: 0x000E8346
			public override long Length
			{
				get
				{
					return this._originalStream.Length;
				}
			}

			// Token: 0x17000CA3 RID: 3235
			// (get) Token: 0x060036A1 RID: 13985 RVA: 0x000E9353 File Offset: 0x000E8353
			// (set) Token: 0x060036A2 RID: 13986 RVA: 0x000E9360 File Offset: 0x000E8360
			public override long Position
			{
				get
				{
					return this._originalStream.Position;
				}
				set
				{
					if (value < 0L)
					{
						throw new ArgumentOutOfRangeException("value", SR.GetString("PositionOutOfRange"));
					}
					this.Seek(value, SeekOrigin.Begin);
				}
			}

			// Token: 0x060036A3 RID: 13987 RVA: 0x000E9385 File Offset: 0x000E8385
			public override void Close()
			{
				this._originalStream.Close();
			}

			// Token: 0x060036A4 RID: 13988 RVA: 0x000E9392 File Offset: 0x000E8392
			protected override void Dispose(bool disposing)
			{
				if (disposing && this._originalStream != null)
				{
					((IDisposable)this._originalStream).Dispose();
					this._originalStream = null;
				}
				base.Dispose(disposing);
			}

			// Token: 0x060036A5 RID: 13989 RVA: 0x000E93B8 File Offset: 0x000E83B8
			public override void Flush()
			{
				this._originalStream.Flush();
			}

			// Token: 0x060036A6 RID: 13990 RVA: 0x000E93C8 File Offset: 0x000E83C8
			public override void SetLength(long value)
			{
				long length = this._originalStream.Length;
				this.EnsureQuota(Math.Max(length, value));
				this._originalStream.SetLength(value);
			}

			// Token: 0x060036A7 RID: 13991 RVA: 0x000E93FC File Offset: 0x000E83FC
			public override int Read(byte[] buffer, int offset, int count)
			{
				return this._originalStream.Read(buffer, offset, count);
			}

			// Token: 0x060036A8 RID: 13992 RVA: 0x000E940C File Offset: 0x000E840C
			public override int ReadByte()
			{
				return this._originalStream.ReadByte();
			}

			// Token: 0x060036A9 RID: 13993 RVA: 0x000E941C File Offset: 0x000E841C
			public override long Seek(long offset, SeekOrigin origin)
			{
				if (!this.CanSeek)
				{
					throw new NotSupportedException();
				}
				long length = this._originalStream.Length;
				long num;
				switch (origin)
				{
				case SeekOrigin.Begin:
					num = offset;
					break;
				case SeekOrigin.Current:
					num = this._originalStream.Position + offset;
					break;
				case SeekOrigin.End:
					num = length + offset;
					break;
				default:
					throw new ArgumentException(SR.GetString("UnknownSeekOrigin"), "origin");
				}
				this.EnsureQuota(Math.Max(length, num));
				return this._originalStream.Seek(offset, origin);
			}

			// Token: 0x060036AA RID: 13994 RVA: 0x000E94A4 File Offset: 0x000E84A4
			public override void Write(byte[] buffer, int offset, int count)
			{
				if (!this.CanWrite)
				{
					throw new NotSupportedException();
				}
				long length = this._originalStream.Length;
				long num = (this._originalStream.CanSeek ? (this._originalStream.Position + (long)count) : (this._originalStream.Length + (long)count));
				this.EnsureQuota(Math.Max(length, num));
				this._originalStream.Write(buffer, offset, count);
			}

			// Token: 0x060036AB RID: 13995 RVA: 0x000E9514 File Offset: 0x000E8514
			public override void WriteByte(byte value)
			{
				if (!this.CanWrite)
				{
					throw new NotSupportedException();
				}
				long length = this._originalStream.Length;
				long num = (this._originalStream.CanSeek ? (this._originalStream.Position + 1L) : (this._originalStream.Length + 1L));
				this.EnsureQuota(Math.Max(length, num));
				this._originalStream.WriteByte(value);
			}

			// Token: 0x060036AC RID: 13996 RVA: 0x000E9580 File Offset: 0x000E8580
			public override IAsyncResult BeginRead(byte[] buffer, int offset, int numBytes, AsyncCallback userCallback, object stateObject)
			{
				return this._originalStream.BeginRead(buffer, offset, numBytes, userCallback, stateObject);
			}

			// Token: 0x060036AD RID: 13997 RVA: 0x000E9594 File Offset: 0x000E8594
			public override int EndRead(IAsyncResult asyncResult)
			{
				return this._originalStream.EndRead(asyncResult);
			}

			// Token: 0x060036AE RID: 13998 RVA: 0x000E95A4 File Offset: 0x000E85A4
			public override IAsyncResult BeginWrite(byte[] buffer, int offset, int numBytes, AsyncCallback userCallback, object stateObject)
			{
				if (!this.CanWrite)
				{
					throw new NotSupportedException();
				}
				long length = this._originalStream.Length;
				long num = (this._originalStream.CanSeek ? (this._originalStream.Position + (long)numBytes) : (this._originalStream.Length + (long)numBytes));
				this.EnsureQuota(Math.Max(length, num));
				return this._originalStream.BeginWrite(buffer, offset, numBytes, userCallback, stateObject);
			}

			// Token: 0x060036AF RID: 13999 RVA: 0x000E9616 File Offset: 0x000E8616
			public override void EndWrite(IAsyncResult asyncResult)
			{
				this._originalStream.EndWrite(asyncResult);
			}

			// Token: 0x060036B0 RID: 14000 RVA: 0x000E9624 File Offset: 0x000E8624
			private void EnsureQuota(long size)
			{
				new IsolatedStorageFilePermission(PermissionState.None)
				{
					UserQuota = size,
					UsageAllowed = (this._isRoaming ? IsolatedStorageContainment.DomainIsolationByRoamingUser : IsolatedStorageContainment.DomainIsolationByUser)
				}.Demand();
			}

			// Token: 0x0400318F RID: 12687
			private Stream _originalStream;

			// Token: 0x04003190 RID: 12688
			private bool _isRoaming;
		}
	}
}
