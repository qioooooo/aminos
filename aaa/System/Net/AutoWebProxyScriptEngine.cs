using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading;

namespace System.Net
{
	// Token: 0x020004AF RID: 1199
	internal class AutoWebProxyScriptEngine
	{
		// Token: 0x060024F0 RID: 9456 RVA: 0x00092514 File Offset: 0x00091514
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.ControlPrincipal)]
		internal AutoWebProxyScriptEngine(WebProxy proxy, bool useRegistry)
		{
			this.webProxy = proxy;
			this.m_UseRegistry = useRegistry;
			this.m_AutoDetector = AutoWebProxyScriptEngine.AutoDetector.CurrentAutoDetector;
			this.m_NetworkChangeStatus = this.m_AutoDetector.NetworkChangeStatus;
			SafeRegistryHandle.RegOpenCurrentUser(131097U, out this.hkcu);
			if (this.m_UseRegistry)
			{
				this.ListenForRegistry();
				this.m_Identity = WindowsIdentity.GetCurrent();
			}
			this.webProxyFinder = new HybridWebProxyFinder(this);
		}

		// Token: 0x060024F1 RID: 9457 RVA: 0x00092588 File Offset: 0x00091588
		private void EnterLock(ref int syncStatus)
		{
			if (syncStatus == 0)
			{
				lock (this)
				{
					if (syncStatus != 4)
					{
						syncStatus = 1;
						while (this.m_LockHeld)
						{
							Monitor.Wait(this);
							if (syncStatus == 4)
							{
								Monitor.Pulse(this);
								return;
							}
						}
						syncStatus = 2;
						this.m_LockHeld = true;
					}
				}
			}
		}

		// Token: 0x060024F2 RID: 9458 RVA: 0x000925EC File Offset: 0x000915EC
		private void ExitLock(ref int syncStatus)
		{
			if (syncStatus != 0 && syncStatus != 4)
			{
				lock (this)
				{
					this.m_LockHeld = false;
					if (syncStatus == 3)
					{
						this.webProxyFinder.Reset();
						syncStatus = 4;
					}
					else
					{
						syncStatus = 0;
					}
					Monitor.Pulse(this);
				}
			}
		}

		// Token: 0x060024F3 RID: 9459 RVA: 0x00092648 File Offset: 0x00091648
		internal void Abort(ref int syncStatus)
		{
			lock (this)
			{
				switch (syncStatus)
				{
				case 0:
					syncStatus = 4;
					break;
				case 1:
					syncStatus = 4;
					Monitor.PulseAll(this);
					break;
				case 2:
					syncStatus = 3;
					this.webProxyFinder.Abort();
					break;
				}
			}
		}

		// Token: 0x170007A3 RID: 1955
		// (get) Token: 0x060024F4 RID: 9460 RVA: 0x000926AC File Offset: 0x000916AC
		// (set) Token: 0x060024F5 RID: 9461 RVA: 0x000926B4 File Offset: 0x000916B4
		internal bool AutomaticallyDetectSettings
		{
			get
			{
				return this.automaticallyDetectSettings;
			}
			set
			{
				if (this.automaticallyDetectSettings != value)
				{
					this.automaticallyDetectSettings = value;
					this.webProxyFinder.Reset();
				}
			}
		}

		// Token: 0x170007A4 RID: 1956
		// (get) Token: 0x060024F6 RID: 9462 RVA: 0x000926D1 File Offset: 0x000916D1
		// (set) Token: 0x060024F7 RID: 9463 RVA: 0x000926D9 File Offset: 0x000916D9
		internal Uri AutomaticConfigurationScript
		{
			get
			{
				return this.automaticConfigurationScript;
			}
			set
			{
				if (!object.Equals(this.automaticConfigurationScript, value))
				{
					this.automaticConfigurationScript = value;
					this.webProxyFinder.Reset();
				}
			}
		}

		// Token: 0x170007A5 RID: 1957
		// (get) Token: 0x060024F8 RID: 9464 RVA: 0x000926FB File Offset: 0x000916FB
		internal ICredentials Credentials
		{
			get
			{
				return this.webProxy.Credentials;
			}
		}

		// Token: 0x060024F9 RID: 9465 RVA: 0x00092708 File Offset: 0x00091708
		internal bool GetProxies(Uri destination, out IList<string> proxyList)
		{
			int num = 0;
			return this.GetProxies(destination, out proxyList, ref num);
		}

		// Token: 0x060024FA RID: 9466 RVA: 0x00092724 File Offset: 0x00091724
		internal bool GetProxies(Uri destination, out IList<string> proxyList, ref int syncStatus)
		{
			proxyList = null;
			this.CheckForChanges(ref syncStatus);
			if (!this.webProxyFinder.IsValid)
			{
				return false;
			}
			bool flag;
			try
			{
				this.EnterLock(ref syncStatus);
				if (syncStatus != 2)
				{
					flag = false;
				}
				else
				{
					flag = this.webProxyFinder.GetProxies(destination, out proxyList);
				}
			}
			finally
			{
				this.ExitLock(ref syncStatus);
			}
			return flag;
		}

		// Token: 0x060024FB RID: 9467 RVA: 0x00092784 File Offset: 0x00091784
		internal WebProxyData GetWebProxyData()
		{
			WebProxyDataBuilder webProxyDataBuilder;
			if (ComNetOS.IsWin7)
			{
				webProxyDataBuilder = new WinHttpWebProxyBuilder();
			}
			else
			{
				webProxyDataBuilder = new RegBlobWebProxyDataBuilder(this.m_AutoDetector.Connectoid, this.hkcu);
			}
			return webProxyDataBuilder.Build();
		}

		// Token: 0x060024FC RID: 9468 RVA: 0x000927C0 File Offset: 0x000917C0
		internal void Close()
		{
			if (this.m_AutoDetector != null)
			{
				int num = 0;
				try
				{
					this.EnterLock(ref num);
					if (this.m_AutoDetector != null)
					{
						this.registrySuppress = true;
						if (this.registryChangeEventPolicy != null)
						{
							this.registryChangeEventPolicy.Close();
							this.registryChangeEventPolicy = null;
						}
						if (this.registryChangeEventLM != null)
						{
							this.registryChangeEventLM.Close();
							this.registryChangeEventLM = null;
						}
						if (this.registryChangeEvent != null)
						{
							this.registryChangeEvent.Close();
							this.registryChangeEvent = null;
						}
						if (this.regKeyPolicy != null && !this.regKeyPolicy.IsInvalid)
						{
							this.regKeyPolicy.Close();
						}
						if (this.regKeyLM != null && !this.regKeyLM.IsInvalid)
						{
							this.regKeyLM.Close();
						}
						if (this.regKey != null && !this.regKey.IsInvalid)
						{
							this.regKey.Close();
						}
						if (this.hkcu != null)
						{
							this.hkcu.RegCloseKey();
							this.hkcu = null;
						}
						if (this.m_Identity != null)
						{
							this.m_Identity.Dispose();
							this.m_Identity = null;
						}
						this.webProxyFinder.Dispose();
						this.m_AutoDetector = null;
					}
				}
				finally
				{
					this.ExitLock(ref num);
				}
			}
		}

		// Token: 0x060024FD RID: 9469 RVA: 0x00092910 File Offset: 0x00091910
		internal void ListenForRegistry()
		{
			if (!this.registrySuppress)
			{
				if (this.registryChangeEvent == null)
				{
					this.ListenForRegistryHelper(ref this.regKey, ref this.registryChangeEvent, IntPtr.Zero, "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Internet Settings\\Connections");
				}
				if (this.registryChangeEventLM == null)
				{
					this.ListenForRegistryHelper(ref this.regKeyLM, ref this.registryChangeEventLM, UnsafeNclNativeMethods.RegistryHelper.HKEY_LOCAL_MACHINE, "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Internet Settings\\Connections");
				}
				if (this.registryChangeEventPolicy == null)
				{
					this.ListenForRegistryHelper(ref this.regKeyPolicy, ref this.registryChangeEventPolicy, UnsafeNclNativeMethods.RegistryHelper.HKEY_LOCAL_MACHINE, "SOFTWARE\\Policies\\Microsoft\\Windows\\CurrentVersion\\Internet Settings");
				}
				if (this.registryChangeEvent == null && this.registryChangeEventLM == null && this.registryChangeEventPolicy == null)
				{
					this.registrySuppress = true;
				}
			}
		}

		// Token: 0x060024FE RID: 9470 RVA: 0x000929B4 File Offset: 0x000919B4
		private void ListenForRegistryHelper(ref SafeRegistryHandle key, ref AutoResetEvent changeEvent, IntPtr baseKey, string subKey)
		{
			uint num = 0U;
			if (key == null || key.IsInvalid)
			{
				if (baseKey == IntPtr.Zero)
				{
					if (this.hkcu != null)
					{
						num = this.hkcu.RegOpenKeyEx(subKey, 0U, 131097U, out key);
					}
					else
					{
						num = 1168U;
					}
				}
				else
				{
					num = SafeRegistryHandle.RegOpenKeyEx(baseKey, subKey, 0U, 131097U, out key);
				}
				if (num == 0U)
				{
					changeEvent = new AutoResetEvent(false);
				}
			}
			if (num == 0U)
			{
				num = key.RegNotifyChangeKeyValue(true, 4U, changeEvent.SafeWaitHandle, true);
			}
			if (num != 0U)
			{
				if (key != null && !key.IsInvalid)
				{
					try
					{
						num = key.RegCloseKey();
					}
					catch (Exception ex)
					{
						if (NclUtilities.IsFatal(ex))
						{
							throw;
						}
					}
				}
				key = null;
				if (changeEvent != null)
				{
					changeEvent.Close();
					changeEvent = null;
				}
			}
		}

		// Token: 0x060024FF RID: 9471 RVA: 0x00092A7C File Offset: 0x00091A7C
		private void RegistryChanged()
		{
			if (Logging.On)
			{
				Logging.PrintWarning(Logging.Web, SR.GetString("net_log_proxy_system_setting_update"));
			}
			WebProxyData webProxyData;
			using (this.m_Identity.Impersonate())
			{
				webProxyData = this.GetWebProxyData();
			}
			this.webProxy.Update(webProxyData);
		}

		// Token: 0x06002500 RID: 9472 RVA: 0x00092AE0 File Offset: 0x00091AE0
		private void ConnectoidChanged()
		{
			if (Logging.On)
			{
				Logging.PrintWarning(Logging.Web, SR.GetString("net_log_proxy_update_due_to_ip_config_change"));
			}
			this.m_AutoDetector = AutoWebProxyScriptEngine.AutoDetector.CurrentAutoDetector;
			if (this.m_UseRegistry)
			{
				WebProxyData webProxyData;
				using (this.m_Identity.Impersonate())
				{
					webProxyData = this.GetWebProxyData();
				}
				this.webProxy.Update(webProxyData);
			}
			if (this.automaticallyDetectSettings)
			{
				this.webProxyFinder.Reset();
			}
		}

		// Token: 0x06002501 RID: 9473 RVA: 0x00092B6C File Offset: 0x00091B6C
		internal void CheckForChanges()
		{
			int num = 0;
			this.CheckForChanges(ref num);
		}

		// Token: 0x06002502 RID: 9474 RVA: 0x00092B84 File Offset: 0x00091B84
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.ControlPrincipal)]
		private void CheckForChanges(ref int syncStatus)
		{
			try
			{
				bool flag = AutoWebProxyScriptEngine.AutoDetector.CheckForNetworkChanges(ref this.m_NetworkChangeStatus);
				bool flag2 = false;
				if (!flag)
				{
					if (!this.needConnectoidUpdate)
					{
						goto IL_0053;
					}
				}
				try
				{
					this.EnterLock(ref syncStatus);
					if (flag || this.needConnectoidUpdate)
					{
						this.needConnectoidUpdate = syncStatus != 2;
						if (!this.needConnectoidUpdate)
						{
							this.ConnectoidChanged();
							flag2 = true;
						}
					}
				}
				finally
				{
					this.ExitLock(ref syncStatus);
				}
				IL_0053:
				if (this.m_UseRegistry)
				{
					bool flag3 = false;
					AutoResetEvent autoResetEvent = this.registryChangeEvent;
					if (!this.registryChangeDeferred)
					{
						if (!(flag3 = autoResetEvent != null && autoResetEvent.WaitOne(0, false)))
						{
							goto IL_0100;
						}
					}
					try
					{
						this.EnterLock(ref syncStatus);
						if (flag3 || this.registryChangeDeferred)
						{
							this.registryChangeDeferred = syncStatus != 2;
							if (!this.registryChangeDeferred && this.registryChangeEvent != null)
							{
								try
								{
									using (this.m_Identity.Impersonate())
									{
										this.ListenForRegistryHelper(ref this.regKey, ref this.registryChangeEvent, IntPtr.Zero, "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Internet Settings\\Connections");
									}
								}
								catch
								{
									throw;
								}
								this.needRegistryUpdate = true;
							}
						}
					}
					finally
					{
						this.ExitLock(ref syncStatus);
					}
					IL_0100:
					flag3 = false;
					autoResetEvent = this.registryChangeEventLM;
					if (!this.registryChangeLMDeferred)
					{
						if (!(flag3 = autoResetEvent != null && autoResetEvent.WaitOne(0, false)))
						{
							goto IL_01A0;
						}
					}
					try
					{
						this.EnterLock(ref syncStatus);
						if (flag3 || this.registryChangeLMDeferred)
						{
							this.registryChangeLMDeferred = syncStatus != 2;
							if (!this.registryChangeLMDeferred && this.registryChangeEventLM != null)
							{
								try
								{
									using (this.m_Identity.Impersonate())
									{
										this.ListenForRegistryHelper(ref this.regKeyLM, ref this.registryChangeEventLM, UnsafeNclNativeMethods.RegistryHelper.HKEY_LOCAL_MACHINE, "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Internet Settings\\Connections");
									}
								}
								catch
								{
									throw;
								}
								this.needRegistryUpdate = true;
							}
						}
					}
					finally
					{
						this.ExitLock(ref syncStatus);
					}
					IL_01A0:
					flag3 = false;
					autoResetEvent = this.registryChangeEventPolicy;
					if (!this.registryChangePolicyDeferred)
					{
						if (!(flag3 = autoResetEvent != null && autoResetEvent.WaitOne(0, false)))
						{
							goto IL_0240;
						}
					}
					try
					{
						this.EnterLock(ref syncStatus);
						if (flag3 || this.registryChangePolicyDeferred)
						{
							this.registryChangePolicyDeferred = syncStatus != 2;
							if (!this.registryChangePolicyDeferred && this.registryChangeEventPolicy != null)
							{
								try
								{
									using (this.m_Identity.Impersonate())
									{
										this.ListenForRegistryHelper(ref this.regKeyPolicy, ref this.registryChangeEventPolicy, UnsafeNclNativeMethods.RegistryHelper.HKEY_LOCAL_MACHINE, "SOFTWARE\\Policies\\Microsoft\\Windows\\CurrentVersion\\Internet Settings");
									}
								}
								catch
								{
									throw;
								}
								this.needRegistryUpdate = true;
							}
						}
					}
					finally
					{
						this.ExitLock(ref syncStatus);
					}
					IL_0240:
					if (this.needRegistryUpdate)
					{
						try
						{
							this.EnterLock(ref syncStatus);
							if (this.needRegistryUpdate && syncStatus == 2)
							{
								this.needRegistryUpdate = false;
								if (!flag2)
								{
									this.RegistryChanged();
								}
							}
						}
						finally
						{
							this.ExitLock(ref syncStatus);
						}
					}
				}
			}
			catch (ObjectDisposedException)
			{
			}
		}

		// Token: 0x040024E1 RID: 9441
		private bool automaticallyDetectSettings;

		// Token: 0x040024E2 RID: 9442
		private Uri automaticConfigurationScript;

		// Token: 0x040024E3 RID: 9443
		private WebProxy webProxy;

		// Token: 0x040024E4 RID: 9444
		private IWebProxyFinder webProxyFinder;

		// Token: 0x040024E5 RID: 9445
		private bool m_LockHeld;

		// Token: 0x040024E6 RID: 9446
		private bool m_UseRegistry;

		// Token: 0x040024E7 RID: 9447
		private int m_NetworkChangeStatus;

		// Token: 0x040024E8 RID: 9448
		private AutoWebProxyScriptEngine.AutoDetector m_AutoDetector;

		// Token: 0x040024E9 RID: 9449
		private SafeRegistryHandle hkcu;

		// Token: 0x040024EA RID: 9450
		private WindowsIdentity m_Identity;

		// Token: 0x040024EB RID: 9451
		private SafeRegistryHandle regKey;

		// Token: 0x040024EC RID: 9452
		private SafeRegistryHandle regKeyLM;

		// Token: 0x040024ED RID: 9453
		private SafeRegistryHandle regKeyPolicy;

		// Token: 0x040024EE RID: 9454
		private AutoResetEvent registryChangeEvent;

		// Token: 0x040024EF RID: 9455
		private AutoResetEvent registryChangeEventLM;

		// Token: 0x040024F0 RID: 9456
		private AutoResetEvent registryChangeEventPolicy;

		// Token: 0x040024F1 RID: 9457
		private bool registryChangeDeferred;

		// Token: 0x040024F2 RID: 9458
		private bool registryChangeLMDeferred;

		// Token: 0x040024F3 RID: 9459
		private bool registryChangePolicyDeferred;

		// Token: 0x040024F4 RID: 9460
		private bool needRegistryUpdate;

		// Token: 0x040024F5 RID: 9461
		private bool needConnectoidUpdate;

		// Token: 0x040024F6 RID: 9462
		private bool registrySuppress;

		// Token: 0x020004B0 RID: 1200
		private static class SyncStatus
		{
			// Token: 0x040024F7 RID: 9463
			internal const int Unlocked = 0;

			// Token: 0x040024F8 RID: 9464
			internal const int Locking = 1;

			// Token: 0x040024F9 RID: 9465
			internal const int LockOwner = 2;

			// Token: 0x040024FA RID: 9466
			internal const int AbortedLocked = 3;

			// Token: 0x040024FB RID: 9467
			internal const int Aborted = 4;
		}

		// Token: 0x020004B1 RID: 1201
		private class AutoDetector
		{
			// Token: 0x06002504 RID: 9476 RVA: 0x00092F3C File Offset: 0x00091F3C
			private static void Initialize()
			{
				if (!AutoWebProxyScriptEngine.AutoDetector.s_Initialized)
				{
					lock (AutoWebProxyScriptEngine.AutoDetector.s_LockObject)
					{
						if (!AutoWebProxyScriptEngine.AutoDetector.s_Initialized)
						{
							AutoWebProxyScriptEngine.AutoDetector.s_CurrentAutoDetector = new AutoWebProxyScriptEngine.AutoDetector(UnsafeNclNativeMethods.RasHelper.GetCurrentConnectoid(), 1);
							if (NetworkChange.CanListenForNetworkChanges)
							{
								AutoWebProxyScriptEngine.AutoDetector.s_AddressChange = new NetworkAddressChangePolled();
							}
							if (UnsafeNclNativeMethods.RasHelper.RasSupported)
							{
								AutoWebProxyScriptEngine.AutoDetector.s_RasHelper = new UnsafeNclNativeMethods.RasHelper();
							}
							AutoWebProxyScriptEngine.AutoDetector.s_CurrentVersion = 1;
							AutoWebProxyScriptEngine.AutoDetector.s_Initialized = true;
						}
					}
				}
			}

			// Token: 0x06002505 RID: 9477 RVA: 0x00092FC4 File Offset: 0x00091FC4
			internal static bool CheckForNetworkChanges(ref int changeStatus)
			{
				AutoWebProxyScriptEngine.AutoDetector.Initialize();
				AutoWebProxyScriptEngine.AutoDetector.CheckForChanges();
				int num = changeStatus;
				changeStatus = AutoWebProxyScriptEngine.AutoDetector.s_CurrentVersion;
				return num != changeStatus;
			}

			// Token: 0x06002506 RID: 9478 RVA: 0x00092FF0 File Offset: 0x00091FF0
			private static void CheckForChanges()
			{
				bool flag = false;
				if (AutoWebProxyScriptEngine.AutoDetector.s_RasHelper != null && AutoWebProxyScriptEngine.AutoDetector.s_RasHelper.HasChanged)
				{
					AutoWebProxyScriptEngine.AutoDetector.s_RasHelper.Reset();
					flag = true;
				}
				if (AutoWebProxyScriptEngine.AutoDetector.s_AddressChange != null && AutoWebProxyScriptEngine.AutoDetector.s_AddressChange.CheckAndReset())
				{
					flag = true;
				}
				if (flag)
				{
					Interlocked.Increment(ref AutoWebProxyScriptEngine.AutoDetector.s_CurrentVersion);
					AutoWebProxyScriptEngine.AutoDetector.s_CurrentAutoDetector = new AutoWebProxyScriptEngine.AutoDetector(UnsafeNclNativeMethods.RasHelper.GetCurrentConnectoid(), AutoWebProxyScriptEngine.AutoDetector.s_CurrentVersion);
				}
			}

			// Token: 0x170007A6 RID: 1958
			// (get) Token: 0x06002507 RID: 9479 RVA: 0x00093057 File Offset: 0x00092057
			internal static AutoWebProxyScriptEngine.AutoDetector CurrentAutoDetector
			{
				get
				{
					AutoWebProxyScriptEngine.AutoDetector.Initialize();
					return AutoWebProxyScriptEngine.AutoDetector.s_CurrentAutoDetector;
				}
			}

			// Token: 0x06002508 RID: 9480 RVA: 0x00093065 File Offset: 0x00092065
			private AutoDetector(string connectoid, int currentVersion)
			{
				this.m_Connectoid = connectoid;
				this.m_CurrentVersion = currentVersion;
			}

			// Token: 0x170007A7 RID: 1959
			// (get) Token: 0x06002509 RID: 9481 RVA: 0x0009307B File Offset: 0x0009207B
			internal string Connectoid
			{
				get
				{
					return this.m_Connectoid;
				}
			}

			// Token: 0x170007A8 RID: 1960
			// (get) Token: 0x0600250A RID: 9482 RVA: 0x00093083 File Offset: 0x00092083
			internal int NetworkChangeStatus
			{
				get
				{
					return this.m_CurrentVersion;
				}
			}

			// Token: 0x040024FC RID: 9468
			private static NetworkAddressChangePolled s_AddressChange;

			// Token: 0x040024FD RID: 9469
			private static UnsafeNclNativeMethods.RasHelper s_RasHelper;

			// Token: 0x040024FE RID: 9470
			private static int s_CurrentVersion;

			// Token: 0x040024FF RID: 9471
			private static volatile AutoWebProxyScriptEngine.AutoDetector s_CurrentAutoDetector;

			// Token: 0x04002500 RID: 9472
			private static volatile bool s_Initialized;

			// Token: 0x04002501 RID: 9473
			private static object s_LockObject = new object();

			// Token: 0x04002502 RID: 9474
			private readonly string m_Connectoid;

			// Token: 0x04002503 RID: 9475
			private readonly int m_CurrentVersion;
		}
	}
}
