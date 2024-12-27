using System;
using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Remoting;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using System.Threading;

namespace System.Net
{
	// Token: 0x020004B5 RID: 1205
	internal class AutoWebProxyScriptWrapper
	{
		// Token: 0x0600253F RID: 9535 RVA: 0x00094611 File Offset: 0x00093611
		static AutoWebProxyScriptWrapper()
		{
			AppDomain.CurrentDomain.DomainUnload += AutoWebProxyScriptWrapper.OnDomainUnload;
		}

		// Token: 0x06002540 RID: 9536 RVA: 0x00094640 File Offset: 0x00093640
		[ReflectionPermission(SecurityAction.Assert, Flags = ReflectionPermissionFlag.TypeInformation)]
		[ReflectionPermission(SecurityAction.Assert, Flags = ReflectionPermissionFlag.MemberAccess)]
		internal AutoWebProxyScriptWrapper()
		{
			Exception ex = null;
			if (AutoWebProxyScriptWrapper.s_ProxyScriptHelperLoadError == null && AutoWebProxyScriptWrapper.s_ProxyScriptHelperType == null)
			{
				lock (AutoWebProxyScriptWrapper.s_ProxyScriptHelperLock)
				{
					if (AutoWebProxyScriptWrapper.s_ProxyScriptHelperLoadError == null && AutoWebProxyScriptWrapper.s_ProxyScriptHelperType == null)
					{
						try
						{
							AutoWebProxyScriptWrapper.s_ProxyScriptHelperType = Type.GetType("System.Net.VsaWebProxyScript, Microsoft.JScript, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", true);
						}
						catch (Exception ex2)
						{
							ex = ex2;
						}
						if (AutoWebProxyScriptWrapper.s_ProxyScriptHelperType == null)
						{
							AutoWebProxyScriptWrapper.s_ProxyScriptHelperLoadError = ((ex == null) ? new InternalException() : ex);
						}
					}
				}
			}
			if (AutoWebProxyScriptWrapper.s_ProxyScriptHelperLoadError != null)
			{
				throw new TypeLoadException(SR.GetString("net_cannot_load_proxy_helper"), (AutoWebProxyScriptWrapper.s_ProxyScriptHelperLoadError is InternalException) ? null : AutoWebProxyScriptWrapper.s_ProxyScriptHelperLoadError);
			}
			this.CreateAppDomain();
			ex = null;
			try
			{
				ObjectHandle objectHandle = Activator.CreateInstance(this.scriptDomain, AutoWebProxyScriptWrapper.s_ProxyScriptHelperType.Assembly.FullName, AutoWebProxyScriptWrapper.s_ProxyScriptHelperType.FullName, false, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod | BindingFlags.CreateInstance, null, null, null, null, null);
				if (objectHandle != null)
				{
					this.site = (IWebProxyScript)objectHandle.Unwrap();
				}
			}
			catch (Exception ex3)
			{
				ex = ex3;
			}
			if (this.site == null)
			{
				lock (AutoWebProxyScriptWrapper.s_ProxyScriptHelperLock)
				{
					if (AutoWebProxyScriptWrapper.s_ProxyScriptHelperLoadError == null)
					{
						AutoWebProxyScriptWrapper.s_ProxyScriptHelperLoadError = ((ex == null) ? new InternalException() : ex);
					}
				}
				throw new TypeLoadException(SR.GetString("net_cannot_load_proxy_helper"), (AutoWebProxyScriptWrapper.s_ProxyScriptHelperLoadError is InternalException) ? null : AutoWebProxyScriptWrapper.s_ProxyScriptHelperLoadError);
			}
		}

		// Token: 0x06002541 RID: 9537 RVA: 0x000947C4 File Offset: 0x000937C4
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.ControlEvidence | SecurityPermissionFlag.ControlAppDomain)]
		private void CreateAppDomain()
		{
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				try
				{
				}
				finally
				{
					Monitor.Enter(AutoWebProxyScriptWrapper.s_AppDomains.SyncRoot);
					flag = true;
				}
				if (AutoWebProxyScriptWrapper.s_CleanedUp)
				{
					throw new InvalidOperationException(SR.GetString("net_cant_perform_during_shutdown"));
				}
				if (AutoWebProxyScriptWrapper.s_AppDomainInfo == null)
				{
					AutoWebProxyScriptWrapper.s_AppDomainInfo = new AppDomainSetup();
					AutoWebProxyScriptWrapper.s_AppDomainInfo.DisallowBindingRedirects = true;
					AutoWebProxyScriptWrapper.s_AppDomainInfo.DisallowCodeDownload = true;
					AutoWebProxyScriptWrapper.s_AppDomainInfo.ApplicationBase = Environment.SystemDirectory;
				}
				AppDomain appDomain = AutoWebProxyScriptWrapper.s_ExcessAppDomain;
				if (appDomain != null)
				{
					TimerThread.GetOrCreateQueue(0).CreateTimer(new TimerThread.Callback(AutoWebProxyScriptWrapper.CloseAppDomainCallback), appDomain);
					throw new InvalidOperationException(SR.GetString("net_cant_create_environment"));
				}
				this.appDomainIndex = AutoWebProxyScriptWrapper.s_NextAppDomainIndex++;
				try
				{
				}
				finally
				{
					PermissionSet permissionSet = new PermissionSet(PermissionState.None);
					permissionSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));
					PolicyLevel policyLevel = PolicyLevel.CreateAppDomainLevel();
					policyLevel.RootCodeGroup = new UnionCodeGroup(new AllMembershipCondition(), new PolicyStatement(permissionSet, PolicyStatementAttribute.Exclusive));
					Evidence evidence = new Evidence();
					evidence.AddHost(new Zone(SecurityZone.Internet));
					AutoWebProxyScriptWrapper.s_ExcessAppDomain = AppDomain.CreateDomain("WebProxyScript", evidence, AutoWebProxyScriptWrapper.s_AppDomainInfo);
					AutoWebProxyScriptWrapper.s_ExcessAppDomain.SetAppDomainPolicy(policyLevel);
					try
					{
						AutoWebProxyScriptWrapper.s_AppDomains.Add(this.appDomainIndex, AutoWebProxyScriptWrapper.s_ExcessAppDomain);
						this.scriptDomain = AutoWebProxyScriptWrapper.s_ExcessAppDomain;
					}
					finally
					{
						if (object.ReferenceEquals(this.scriptDomain, AutoWebProxyScriptWrapper.s_ExcessAppDomain))
						{
							AutoWebProxyScriptWrapper.s_ExcessAppDomain = null;
						}
						else
						{
							try
							{
								AutoWebProxyScriptWrapper.s_AppDomains.Remove(this.appDomainIndex);
							}
							finally
							{
								TimerThread.GetOrCreateQueue(0).CreateTimer(new TimerThread.Callback(AutoWebProxyScriptWrapper.CloseAppDomainCallback), AutoWebProxyScriptWrapper.s_ExcessAppDomain);
							}
						}
					}
				}
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(AutoWebProxyScriptWrapper.s_AppDomains.SyncRoot);
				}
			}
		}

		// Token: 0x06002542 RID: 9538 RVA: 0x000949F4 File Offset: 0x000939F4
		internal void Close()
		{
			this.site.Close();
			TimerThread.GetOrCreateQueue(0).CreateTimer(new TimerThread.Callback(AutoWebProxyScriptWrapper.CloseAppDomainCallback), this.appDomainIndex);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002543 RID: 9539 RVA: 0x00094A2C File Offset: 0x00093A2C
		~AutoWebProxyScriptWrapper()
		{
			if (!NclUtilities.HasShutdownStarted && this.scriptDomain != null)
			{
				TimerThread.GetOrCreateQueue(0).CreateTimer(new TimerThread.Callback(AutoWebProxyScriptWrapper.CloseAppDomainCallback), this.appDomainIndex);
			}
		}

		// Token: 0x06002544 RID: 9540 RVA: 0x00094A84 File Offset: 0x00093A84
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.ControlAppDomain)]
		private static void CloseAppDomainCallback(TimerThread.Timer timer, int timeNoticed, object context)
		{
			try
			{
				AppDomain appDomain = context as AppDomain;
				if (appDomain == null)
				{
					AutoWebProxyScriptWrapper.CloseAppDomain((int)context);
				}
				else if (object.ReferenceEquals(appDomain, AutoWebProxyScriptWrapper.s_ExcessAppDomain))
				{
					try
					{
						AppDomain.Unload(appDomain);
					}
					catch (AppDomainUnloadedException)
					{
					}
					AutoWebProxyScriptWrapper.s_ExcessAppDomain = null;
				}
			}
			catch (Exception ex)
			{
				if (NclUtilities.IsFatal(ex))
				{
					throw;
				}
			}
		}

		// Token: 0x06002545 RID: 9541 RVA: 0x00094AF4 File Offset: 0x00093AF4
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.ControlAppDomain)]
		private static void CloseAppDomain(int index)
		{
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			AppDomain appDomain;
			try
			{
				try
				{
				}
				finally
				{
					Monitor.Enter(AutoWebProxyScriptWrapper.s_AppDomains.SyncRoot);
					flag = true;
				}
				if (AutoWebProxyScriptWrapper.s_CleanedUp)
				{
					return;
				}
				appDomain = (AppDomain)AutoWebProxyScriptWrapper.s_AppDomains[index];
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(AutoWebProxyScriptWrapper.s_AppDomains.SyncRoot);
					flag = false;
				}
			}
			try
			{
				AppDomain.Unload(appDomain);
			}
			catch (AppDomainUnloadedException)
			{
			}
			finally
			{
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					try
					{
					}
					finally
					{
						Monitor.Enter(AutoWebProxyScriptWrapper.s_AppDomains.SyncRoot);
						flag = true;
					}
					AutoWebProxyScriptWrapper.s_AppDomains.Remove(index);
				}
				finally
				{
					if (flag)
					{
						Monitor.Exit(AutoWebProxyScriptWrapper.s_AppDomains.SyncRoot);
					}
				}
			}
		}

		// Token: 0x06002546 RID: 9542 RVA: 0x00094BEC File Offset: 0x00093BEC
		[ReliabilityContract(Consistency.MayCorruptProcess, Cer.MayFail)]
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.ControlAppDomain)]
		private static void OnDomainUnload(object sender, EventArgs e)
		{
			lock (AutoWebProxyScriptWrapper.s_AppDomains.SyncRoot)
			{
				if (!AutoWebProxyScriptWrapper.s_CleanedUp)
				{
					AutoWebProxyScriptWrapper.s_CleanedUp = true;
					foreach (object obj in AutoWebProxyScriptWrapper.s_AppDomains.Values)
					{
						AppDomain appDomain = (AppDomain)obj;
						try
						{
							AppDomain.Unload(appDomain);
						}
						catch
						{
						}
					}
					AutoWebProxyScriptWrapper.s_AppDomains.Clear();
					AppDomain appDomain2 = AutoWebProxyScriptWrapper.s_ExcessAppDomain;
					if (appDomain2 != null)
					{
						try
						{
							AppDomain.Unload(appDomain2);
						}
						catch
						{
						}
						AutoWebProxyScriptWrapper.s_ExcessAppDomain = null;
					}
				}
			}
		}

		// Token: 0x170007B2 RID: 1970
		// (get) Token: 0x06002547 RID: 9543 RVA: 0x00094CC0 File Offset: 0x00093CC0
		internal string ScriptBody
		{
			get
			{
				return this.scriptText;
			}
		}

		// Token: 0x170007B3 RID: 1971
		// (get) Token: 0x06002548 RID: 9544 RVA: 0x00094CC8 File Offset: 0x00093CC8
		// (set) Token: 0x06002549 RID: 9545 RVA: 0x00094CD0 File Offset: 0x00093CD0
		internal byte[] Buffer
		{
			get
			{
				return this.scriptBytes;
			}
			set
			{
				this.scriptBytes = value;
			}
		}

		// Token: 0x170007B4 RID: 1972
		// (get) Token: 0x0600254A RID: 9546 RVA: 0x00094CD9 File Offset: 0x00093CD9
		// (set) Token: 0x0600254B RID: 9547 RVA: 0x00094CE1 File Offset: 0x00093CE1
		internal DateTime LastModified
		{
			get
			{
				return this.lastModified;
			}
			set
			{
				this.lastModified = value;
			}
		}

		// Token: 0x0600254C RID: 9548 RVA: 0x00094CEA File Offset: 0x00093CEA
		internal string FindProxyForURL(string url, string host)
		{
			return this.site.Run(url, host);
		}

		// Token: 0x0600254D RID: 9549 RVA: 0x00094CF9 File Offset: 0x00093CF9
		internal bool Compile(Uri engineScriptLocation, string scriptBody, byte[] buffer)
		{
			if (this.site.Load(engineScriptLocation, scriptBody, typeof(WebProxyScriptHelper)))
			{
				this.scriptText = scriptBody;
				this.scriptBytes = buffer;
				return true;
			}
			return false;
		}

		// Token: 0x04002506 RID: 9478
		private const string c_appDomainName = "WebProxyScript";

		// Token: 0x04002507 RID: 9479
		private int appDomainIndex;

		// Token: 0x04002508 RID: 9480
		private AppDomain scriptDomain;

		// Token: 0x04002509 RID: 9481
		private IWebProxyScript site;

		// Token: 0x0400250A RID: 9482
		private static AppDomain s_ExcessAppDomain;

		// Token: 0x0400250B RID: 9483
		private static Hashtable s_AppDomains = new Hashtable();

		// Token: 0x0400250C RID: 9484
		private static bool s_CleanedUp;

		// Token: 0x0400250D RID: 9485
		private static int s_NextAppDomainIndex;

		// Token: 0x0400250E RID: 9486
		private static AppDomainSetup s_AppDomainInfo;

		// Token: 0x0400250F RID: 9487
		private static Type s_ProxyScriptHelperType;

		// Token: 0x04002510 RID: 9488
		private static Exception s_ProxyScriptHelperLoadError;

		// Token: 0x04002511 RID: 9489
		private static object s_ProxyScriptHelperLock = new object();

		// Token: 0x04002512 RID: 9490
		private string scriptText;

		// Token: 0x04002513 RID: 9491
		private byte[] scriptBytes;

		// Token: 0x04002514 RID: 9492
		private DateTime lastModified;
	}
}
