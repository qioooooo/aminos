using System;
using System.Configuration.Internal;
using System.Threading;

namespace System.Configuration
{
	// Token: 0x0200001D RID: 29
	internal sealed class ClientConfigurationSystem : IInternalConfigSystem
	{
		// Token: 0x060001A7 RID: 423 RVA: 0x0000C1B0 File Offset: 0x0000B1B0
		internal ClientConfigurationSystem()
		{
			this._configSystem = new ConfigSystem();
			IConfigSystem configSystem = this._configSystem;
			Type typeFromHandle = typeof(ClientConfigurationHost);
			object[] array = new object[2];
			configSystem.Init(typeFromHandle, array);
			this._configHost = (ClientConfigurationHost)this._configSystem.Host;
			this._configRoot = this._configSystem.Root;
			this._configRoot.ConfigRemoved += this.OnConfigRemoved;
			this._isAppConfigHttp = this._configHost.IsAppConfigHttp;
			string schemeDelimiter = Uri.SchemeDelimiter;
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x0000C240 File Offset: 0x0000B240
		private bool IsSectionUsedInInit(string configKey)
		{
			return configKey == "system.diagnostics" || (this._isAppConfigHttp && configKey.StartsWith("system.net/", StringComparison.Ordinal));
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x0000C267 File Offset: 0x0000B267
		private bool DoesSectionOnlyUseMachineConfig(string configKey)
		{
			return this._isAppConfigHttp && configKey.StartsWith("system.net/", StringComparison.Ordinal);
		}

		// Token: 0x060001AA RID: 426 RVA: 0x0000C280 File Offset: 0x0000B280
		private void EnsureInit(string configKey)
		{
			bool flag = false;
			lock (this)
			{
				if (!this._isUserConfigInited)
				{
					if (!this._isInitInProgress)
					{
						this._isInitInProgress = true;
						flag = true;
					}
					else if (!this.IsSectionUsedInInit(configKey))
					{
						Monitor.Wait(this);
					}
				}
			}
			if (flag)
			{
				try
				{
					try
					{
						this._machineConfigRecord = this._configRoot.GetConfigRecord("MACHINE");
						this._machineConfigRecord.ThrowIfInitErrors();
						this._isMachineConfigInited = true;
						if (this._isAppConfigHttp)
						{
							ConfigurationManagerHelperFactory.Instance.EnsureNetConfigLoaded();
						}
						this._configHost.RefreshConfigPaths();
						string text;
						if (this._configHost.HasLocalConfig)
						{
							text = "MACHINE/EXE/ROAMING_USER/LOCAL_USER";
						}
						else if (this._configHost.HasRoamingConfig)
						{
							text = "MACHINE/EXE/ROAMING_USER";
						}
						else
						{
							text = "MACHINE/EXE";
						}
						this._completeConfigRecord = this._configRoot.GetConfigRecord(text);
						this._completeConfigRecord.ThrowIfInitErrors();
						this._isUserConfigInited = true;
					}
					catch (Exception ex)
					{
						this._initError = new ConfigurationErrorsException(SR.GetString("Config_client_config_init_error"), ex);
						throw this._initError;
					}
					catch
					{
						this._initError = new ConfigurationErrorsException(SR.GetString("Config_client_config_init_error"));
						throw this._initError;
					}
				}
				catch
				{
					ConfigurationManager.SetInitError(this._initError);
					this._isMachineConfigInited = true;
					this._isUserConfigInited = true;
					throw;
				}
				finally
				{
					lock (this)
					{
						try
						{
							ConfigurationManager.CompleteConfigInit();
							this._isInitInProgress = false;
						}
						finally
						{
							Monitor.PulseAll(this);
						}
					}
				}
			}
		}

		// Token: 0x060001AB RID: 427 RVA: 0x0000C448 File Offset: 0x0000B448
		private void PrepareClientConfigSystem(string sectionName)
		{
			if (!this._isUserConfigInited)
			{
				this.EnsureInit(sectionName);
			}
			if (this._initError != null)
			{
				throw this._initError;
			}
		}

		// Token: 0x060001AC RID: 428 RVA: 0x0000C468 File Offset: 0x0000B468
		private void OnConfigRemoved(object sender, InternalConfigEventArgs e)
		{
			try
			{
				IInternalConfigRecord configRecord = this._configRoot.GetConfigRecord(this._completeConfigRecord.ConfigPath);
				this._completeConfigRecord = configRecord;
				this._completeConfigRecord.ThrowIfInitErrors();
			}
			catch (Exception ex)
			{
				this._initError = new ConfigurationErrorsException(SR.GetString("Config_client_config_init_error"), ex);
				ConfigurationManager.SetInitError(this._initError);
				throw this._initError;
			}
			catch
			{
				this._initError = new ConfigurationErrorsException(SR.GetString("Config_client_config_init_error"));
				ConfigurationManager.SetInitError(this._initError);
				throw this._initError;
			}
		}

		// Token: 0x060001AD RID: 429 RVA: 0x0000C510 File Offset: 0x0000B510
		object IInternalConfigSystem.GetSection(string sectionName)
		{
			this.PrepareClientConfigSystem(sectionName);
			IInternalConfigRecord internalConfigRecord = null;
			if (this.DoesSectionOnlyUseMachineConfig(sectionName))
			{
				if (this._isMachineConfigInited)
				{
					internalConfigRecord = this._machineConfigRecord;
				}
			}
			else if (this._isUserConfigInited)
			{
				internalConfigRecord = this._completeConfigRecord;
			}
			if (internalConfigRecord != null)
			{
				return internalConfigRecord.GetSection(sectionName);
			}
			return null;
		}

		// Token: 0x060001AE RID: 430 RVA: 0x0000C55B File Offset: 0x0000B55B
		void IInternalConfigSystem.RefreshConfig(string sectionName)
		{
			this.PrepareClientConfigSystem(sectionName);
			if (this._isMachineConfigInited)
			{
				this._machineConfigRecord.RefreshSection(sectionName);
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060001AF RID: 431 RVA: 0x0000C578 File Offset: 0x0000B578
		bool IInternalConfigSystem.SupportsUserConfig
		{
			get
			{
				return true;
			}
		}

		// Token: 0x04000204 RID: 516
		private const string SystemDiagnosticsConfigKey = "system.diagnostics";

		// Token: 0x04000205 RID: 517
		private const string SystemNetGroupKey = "system.net/";

		// Token: 0x04000206 RID: 518
		private IConfigSystem _configSystem;

		// Token: 0x04000207 RID: 519
		private IInternalConfigRoot _configRoot;

		// Token: 0x04000208 RID: 520
		private ClientConfigurationHost _configHost;

		// Token: 0x04000209 RID: 521
		private IInternalConfigRecord _machineConfigRecord;

		// Token: 0x0400020A RID: 522
		private IInternalConfigRecord _completeConfigRecord;

		// Token: 0x0400020B RID: 523
		private Exception _initError;

		// Token: 0x0400020C RID: 524
		private bool _isInitInProgress;

		// Token: 0x0400020D RID: 525
		private bool _isMachineConfigInited;

		// Token: 0x0400020E RID: 526
		private bool _isUserConfigInited;

		// Token: 0x0400020F RID: 527
		private bool _isAppConfigHttp;
	}
}
