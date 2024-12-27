using System;
using System.Collections;
using System.ComponentModel;
using System.Deployment.Application;
using System.Deployment.Internal.Isolation;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Windows.Forms.Layout;
using System.Windows.Forms.VisualStyles;
using Microsoft.Win32;

namespace System.Windows.Forms
{
	// Token: 0x020001DE RID: 478
	public sealed class Application
	{
		// Token: 0x0600128A RID: 4746 RVA: 0x00010E80 File Offset: 0x0000FE80
		private Application()
		{
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x0600128B RID: 4747 RVA: 0x00010E88 File Offset: 0x0000FE88
		public static bool AllowQuit
		{
			get
			{
				return Application.ThreadContext.FromCurrent().GetAllowQuit();
			}
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x0600128C RID: 4748 RVA: 0x00010E94 File Offset: 0x0000FE94
		internal static bool CanContinueIdle
		{
			get
			{
				return Application.ThreadContext.FromCurrent().ComponentManager.FContinueIdle();
			}
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x0600128D RID: 4749 RVA: 0x00010EA8 File Offset: 0x0000FEA8
		internal static bool ComCtlSupportsVisualStyles
		{
			get
			{
				if (Application.useVisualStyles && OSFeature.Feature.IsPresent(OSFeature.Themes))
				{
					return true;
				}
				IntPtr intPtr = UnsafeNativeMethods.GetModuleHandle("comctl32.dll");
				if (intPtr != IntPtr.Zero)
				{
					try
					{
						IntPtr procAddress = UnsafeNativeMethods.GetProcAddress(new HandleRef(null, intPtr), "ImageList_WriteEx");
						return procAddress != IntPtr.Zero;
					}
					catch
					{
						return false;
					}
				}
				intPtr = UnsafeNativeMethods.LoadLibrary("comctl32.dll");
				if (intPtr != IntPtr.Zero)
				{
					try
					{
						IntPtr procAddress2 = UnsafeNativeMethods.GetProcAddress(new HandleRef(null, intPtr), "ImageList_WriteEx");
						return procAddress2 != IntPtr.Zero;
					}
					finally
					{
						UnsafeNativeMethods.FreeLibrary(new HandleRef(null, intPtr));
					}
				}
				return false;
			}
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x0600128E RID: 4750 RVA: 0x00010F70 File Offset: 0x0000FF70
		public static RegistryKey CommonAppDataRegistry
		{
			get
			{
				return Registry.LocalMachine.CreateSubKey(Application.CommonAppDataRegistryKeyName);
			}
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x0600128F RID: 4751 RVA: 0x00010F84 File Offset: 0x0000FF84
		internal static string CommonAppDataRegistryKeyName
		{
			get
			{
				string text = "Software\\{0}\\{1}\\{2}";
				return string.Format(CultureInfo.CurrentCulture, text, new object[]
				{
					Application.CompanyName,
					Application.ProductName,
					Application.ProductVersion
				});
			}
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06001290 RID: 4752 RVA: 0x00010FC4 File Offset: 0x0000FFC4
		internal static bool UseEverettThreadAffinity
		{
			get
			{
				if (!Application.checkedThreadAffinity)
				{
					Application.checkedThreadAffinity = true;
					try
					{
						new RegistryPermission(PermissionState.Unrestricted).Assert();
						RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(Application.CommonAppDataRegistryKeyName);
						if (registryKey != null)
						{
							object value = registryKey.GetValue("EnableSystemEventsThreadAffinityCompatibility");
							if (value != null && (int)value != 0)
							{
								Application.useEverettThreadAffinity = true;
							}
						}
					}
					catch (SecurityException)
					{
					}
					catch (InvalidCastException)
					{
					}
				}
				return Application.useEverettThreadAffinity;
			}
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06001291 RID: 4753 RVA: 0x00011044 File Offset: 0x00010044
		public static string CommonAppDataPath
		{
			get
			{
				try
				{
					if (ApplicationDeployment.IsNetworkDeployed)
					{
						string text = AppDomain.CurrentDomain.GetData("DataDirectory") as string;
						if (text != null)
						{
							return text;
						}
					}
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsSecurityOrCriticalException(ex))
					{
						throw;
					}
				}
				return Application.GetDataPath(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData));
			}
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06001292 RID: 4754 RVA: 0x000110A4 File Offset: 0x000100A4
		public static string CompanyName
		{
			get
			{
				lock (Application.internalSyncObject)
				{
					if (Application.companyName == null)
					{
						Assembly entryAssembly = Assembly.GetEntryAssembly();
						if (entryAssembly != null)
						{
							object[] customAttributes = entryAssembly.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
							if (customAttributes != null && customAttributes.Length > 0)
							{
								Application.companyName = ((AssemblyCompanyAttribute)customAttributes[0]).Company;
							}
						}
						if (Application.companyName == null || Application.companyName.Length == 0)
						{
							Application.companyName = Application.GetAppFileVersionInfo().CompanyName;
							if (Application.companyName != null)
							{
								Application.companyName = Application.companyName.Trim();
							}
						}
						if (Application.companyName == null || Application.companyName.Length == 0)
						{
							Type appMainType = Application.GetAppMainType();
							if (appMainType != null)
							{
								string @namespace = appMainType.Namespace;
								if (!string.IsNullOrEmpty(@namespace))
								{
									int num = @namespace.IndexOf(".");
									if (num != -1)
									{
										Application.companyName = @namespace.Substring(0, num);
									}
									else
									{
										Application.companyName = @namespace;
									}
								}
								else
								{
									Application.companyName = Application.ProductName;
								}
							}
						}
					}
				}
				return Application.companyName;
			}
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06001293 RID: 4755 RVA: 0x000111B4 File Offset: 0x000101B4
		// (set) Token: 0x06001294 RID: 4756 RVA: 0x000111C0 File Offset: 0x000101C0
		public static CultureInfo CurrentCulture
		{
			get
			{
				return Thread.CurrentThread.CurrentCulture;
			}
			set
			{
				Thread.CurrentThread.CurrentCulture = value;
			}
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06001295 RID: 4757 RVA: 0x000111CD File Offset: 0x000101CD
		// (set) Token: 0x06001296 RID: 4758 RVA: 0x000111D4 File Offset: 0x000101D4
		public static InputLanguage CurrentInputLanguage
		{
			get
			{
				return InputLanguage.CurrentInputLanguage;
			}
			set
			{
				IntSecurity.AffectThreadBehavior.Demand();
				InputLanguage.CurrentInputLanguage = value;
			}
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06001297 RID: 4759 RVA: 0x000111E6 File Offset: 0x000101E6
		internal static bool CustomThreadExceptionHandlerAttached
		{
			get
			{
				return Application.ThreadContext.FromCurrent().CustomThreadExceptionHandlerAttached;
			}
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06001298 RID: 4760 RVA: 0x000111F4 File Offset: 0x000101F4
		public static string ExecutablePath
		{
			get
			{
				if (Application.executablePath == null)
				{
					Assembly entryAssembly = Assembly.GetEntryAssembly();
					if (entryAssembly == null)
					{
						StringBuilder stringBuilder = new StringBuilder(260);
						UnsafeNativeMethods.GetModuleFileName(NativeMethods.NullHandleRef, stringBuilder, stringBuilder.Capacity);
						Application.executablePath = IntSecurity.UnsafeGetFullPath(stringBuilder.ToString());
					}
					else
					{
						string escapedCodeBase = entryAssembly.EscapedCodeBase;
						Uri uri = new Uri(escapedCodeBase);
						if (uri.Scheme == "file")
						{
							Application.executablePath = NativeMethods.GetLocalPath(escapedCodeBase);
						}
						else
						{
							Application.executablePath = uri.ToString();
						}
					}
				}
				Uri uri2 = new Uri(Application.executablePath);
				if (uri2.Scheme == "file")
				{
					new FileIOPermission(FileIOPermissionAccess.PathDiscovery, Application.executablePath).Demand();
				}
				return Application.executablePath;
			}
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06001299 RID: 4761 RVA: 0x000112AC File Offset: 0x000102AC
		public static string LocalUserAppDataPath
		{
			get
			{
				try
				{
					if (ApplicationDeployment.IsNetworkDeployed)
					{
						string text = AppDomain.CurrentDomain.GetData("DataDirectory") as string;
						if (text != null)
						{
							return text;
						}
					}
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsSecurityOrCriticalException(ex))
					{
						throw;
					}
				}
				return Application.GetDataPath(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
			}
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x0600129A RID: 4762 RVA: 0x0001130C File Offset: 0x0001030C
		public static bool MessageLoop
		{
			get
			{
				return Application.ThreadContext.FromCurrent().GetMessageLoop();
			}
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x0600129B RID: 4763 RVA: 0x00011318 File Offset: 0x00010318
		public static FormCollection OpenForms
		{
			[UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.AllWindows)]
			get
			{
				return Application.OpenFormsInternal;
			}
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x0600129C RID: 4764 RVA: 0x0001131F File Offset: 0x0001031F
		internal static FormCollection OpenFormsInternal
		{
			get
			{
				if (Application.forms == null)
				{
					Application.forms = new FormCollection();
				}
				return Application.forms;
			}
		}

		// Token: 0x0600129D RID: 4765 RVA: 0x00011337 File Offset: 0x00010337
		internal static void OpenFormsInternalAdd(Form form)
		{
			Application.OpenFormsInternal.Add(form);
		}

		// Token: 0x0600129E RID: 4766 RVA: 0x00011344 File Offset: 0x00010344
		internal static void OpenFormsInternalRemove(Form form)
		{
			Application.OpenFormsInternal.Remove(form);
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x0600129F RID: 4767 RVA: 0x00011354 File Offset: 0x00010354
		public static string ProductName
		{
			get
			{
				lock (Application.internalSyncObject)
				{
					if (Application.productName == null)
					{
						Assembly entryAssembly = Assembly.GetEntryAssembly();
						if (entryAssembly != null)
						{
							object[] customAttributes = entryAssembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false);
							if (customAttributes != null && customAttributes.Length > 0)
							{
								Application.productName = ((AssemblyProductAttribute)customAttributes[0]).Product;
							}
						}
						if (Application.productName == null || Application.productName.Length == 0)
						{
							Application.productName = Application.GetAppFileVersionInfo().ProductName;
							if (Application.productName != null)
							{
								Application.productName = Application.productName.Trim();
							}
						}
						if (Application.productName == null || Application.productName.Length == 0)
						{
							Type appMainType = Application.GetAppMainType();
							if (appMainType != null)
							{
								string @namespace = appMainType.Namespace;
								if (!string.IsNullOrEmpty(@namespace))
								{
									int num = @namespace.LastIndexOf(".");
									if (num != -1 && num < @namespace.Length - 1)
									{
										Application.productName = @namespace.Substring(num + 1);
									}
									else
									{
										Application.productName = @namespace;
									}
								}
								else
								{
									Application.productName = appMainType.Name;
								}
							}
						}
					}
				}
				return Application.productName;
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x060012A0 RID: 4768 RVA: 0x00011474 File Offset: 0x00010474
		public static string ProductVersion
		{
			get
			{
				lock (Application.internalSyncObject)
				{
					if (Application.productVersion == null)
					{
						Assembly entryAssembly = Assembly.GetEntryAssembly();
						if (entryAssembly != null)
						{
							object[] customAttributes = entryAssembly.GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false);
							if (customAttributes != null && customAttributes.Length > 0)
							{
								Application.productVersion = ((AssemblyInformationalVersionAttribute)customAttributes[0]).InformationalVersion;
							}
						}
						if (Application.productVersion == null || Application.productVersion.Length == 0)
						{
							Application.productVersion = Application.GetAppFileVersionInfo().ProductVersion;
							if (Application.productVersion != null)
							{
								Application.productVersion = Application.productVersion.Trim();
							}
						}
						if (Application.productVersion == null || Application.productVersion.Length == 0)
						{
							Application.productVersion = "1.0.0.0";
						}
					}
				}
				return Application.productVersion;
			}
		}

		// Token: 0x060012A1 RID: 4769 RVA: 0x00011540 File Offset: 0x00010540
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void RegisterMessageLoop(Application.MessageLoopCallback callback)
		{
			Application.ThreadContext.FromCurrent().RegisterMessageLoop(callback);
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x060012A2 RID: 4770 RVA: 0x0001154D File Offset: 0x0001054D
		public static bool RenderWithVisualStyles
		{
			get
			{
				return Application.ComCtlSupportsVisualStyles && VisualStyleRenderer.IsSupported;
			}
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x060012A3 RID: 4771 RVA: 0x0001155D File Offset: 0x0001055D
		// (set) Token: 0x060012A4 RID: 4772 RVA: 0x0001157A File Offset: 0x0001057A
		public static string SafeTopLevelCaptionFormat
		{
			get
			{
				if (Application.safeTopLevelCaptionSuffix == null)
				{
					Application.safeTopLevelCaptionSuffix = SR.GetString("SafeTopLevelCaptionFormat");
				}
				return Application.safeTopLevelCaptionSuffix;
			}
			set
			{
				IntSecurity.WindowAdornmentModification.Demand();
				if (value == null)
				{
					value = string.Empty;
				}
				Application.safeTopLevelCaptionSuffix = value;
			}
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x060012A5 RID: 4773 RVA: 0x00011598 File Offset: 0x00010598
		public static string StartupPath
		{
			get
			{
				if (Application.startupPath == null)
				{
					StringBuilder stringBuilder = new StringBuilder(260);
					UnsafeNativeMethods.GetModuleFileName(NativeMethods.NullHandleRef, stringBuilder, stringBuilder.Capacity);
					Application.startupPath = Path.GetDirectoryName(stringBuilder.ToString());
				}
				new FileIOPermission(FileIOPermissionAccess.PathDiscovery, Application.startupPath).Demand();
				return Application.startupPath;
			}
		}

		// Token: 0x060012A6 RID: 4774 RVA: 0x000115EE File Offset: 0x000105EE
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void UnregisterMessageLoop()
		{
			Application.ThreadContext.FromCurrent().RegisterMessageLoop(null);
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x060012A7 RID: 4775 RVA: 0x000115FB File Offset: 0x000105FB
		// (set) Token: 0x060012A8 RID: 4776 RVA: 0x00011604 File Offset: 0x00010604
		public static bool UseWaitCursor
		{
			get
			{
				return Application.useWaitCursor;
			}
			set
			{
				lock (FormCollection.CollectionSyncRoot)
				{
					Application.useWaitCursor = value;
					foreach (object obj in Application.OpenFormsInternal)
					{
						Form form = (Form)obj;
						form.UseWaitCursor = Application.useWaitCursor;
					}
				}
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x060012A9 RID: 4777 RVA: 0x00011688 File Offset: 0x00010688
		public static string UserAppDataPath
		{
			get
			{
				try
				{
					if (ApplicationDeployment.IsNetworkDeployed)
					{
						string text = AppDomain.CurrentDomain.GetData("DataDirectory") as string;
						if (text != null)
						{
							return text;
						}
					}
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsSecurityOrCriticalException(ex))
					{
						throw;
					}
				}
				return Application.GetDataPath(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x060012AA RID: 4778 RVA: 0x000116E8 File Offset: 0x000106E8
		public static RegistryKey UserAppDataRegistry
		{
			get
			{
				string text = "Software\\{0}\\{1}\\{2}";
				return Registry.CurrentUser.CreateSubKey(string.Format(CultureInfo.CurrentCulture, text, new object[]
				{
					Application.CompanyName,
					Application.ProductName,
					Application.ProductVersion
				}));
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x060012AB RID: 4779 RVA: 0x00011730 File Offset: 0x00010730
		internal static bool UseVisualStyles
		{
			get
			{
				return Application.useVisualStyles;
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x060012AC RID: 4780 RVA: 0x00011737 File Offset: 0x00010737
		internal static string WindowsFormsVersion
		{
			get
			{
				return "WindowsForms10";
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x060012AD RID: 4781 RVA: 0x0001173E File Offset: 0x0001073E
		internal static string WindowMessagesVersion
		{
			get
			{
				return "WindowsForms12";
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x060012AE RID: 4782 RVA: 0x00011748 File Offset: 0x00010748
		// (set) Token: 0x060012AF RID: 4783 RVA: 0x00011768 File Offset: 0x00010768
		public static VisualStyleState VisualStyleState
		{
			get
			{
				if (!VisualStyleInformation.IsSupportedByOS)
				{
					return VisualStyleState.NoneEnabled;
				}
				return (VisualStyleState)SafeNativeMethods.GetThemeAppProperties();
			}
			set
			{
				if (VisualStyleInformation.IsSupportedByOS)
				{
					if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
					{
						throw new InvalidEnumArgumentException("value", (int)value, typeof(VisualStyleState));
					}
					SafeNativeMethods.SetThemeAppProperties((int)value);
					SafeNativeMethods.EnumThreadWindowsCallback enumThreadWindowsCallback = new SafeNativeMethods.EnumThreadWindowsCallback(Application.SendThemeChanged);
					SafeNativeMethods.EnumWindows(enumThreadWindowsCallback, IntPtr.Zero);
					GC.KeepAlive(enumThreadWindowsCallback);
				}
			}
		}

		// Token: 0x060012B0 RID: 4784 RVA: 0x000117C8 File Offset: 0x000107C8
		private static bool SendThemeChanged(IntPtr handle, IntPtr extraParameter)
		{
			int currentProcessId = SafeNativeMethods.GetCurrentProcessId();
			int num;
			SafeNativeMethods.GetWindowThreadProcessId(new HandleRef(null, handle), out num);
			if (num == currentProcessId && SafeNativeMethods.IsWindowVisible(new HandleRef(null, handle)))
			{
				Application.SendThemeChangedRecursive(handle, IntPtr.Zero);
				SafeNativeMethods.RedrawWindow(new HandleRef(null, handle), null, NativeMethods.NullHandleRef, 1157);
			}
			return true;
		}

		// Token: 0x060012B1 RID: 4785 RVA: 0x00011821 File Offset: 0x00010821
		private static bool SendThemeChangedRecursive(IntPtr handle, IntPtr lparam)
		{
			UnsafeNativeMethods.EnumChildWindows(new HandleRef(null, handle), new NativeMethods.EnumChildrenCallback(Application.SendThemeChangedRecursive), NativeMethods.NullHandleRef);
			UnsafeNativeMethods.SendMessage(new HandleRef(null, handle), 794, 0, 0);
			return true;
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060012B2 RID: 4786 RVA: 0x00011856 File Offset: 0x00010856
		// (remove) Token: 0x060012B3 RID: 4787 RVA: 0x00011863 File Offset: 0x00010863
		public static event EventHandler ApplicationExit
		{
			add
			{
				Application.AddEventHandler(Application.EVENT_APPLICATIONEXIT, value);
			}
			remove
			{
				Application.RemoveEventHandler(Application.EVENT_APPLICATIONEXIT, value);
			}
		}

		// Token: 0x060012B4 RID: 4788 RVA: 0x00011870 File Offset: 0x00010870
		private static void AddEventHandler(object key, Delegate value)
		{
			lock (Application.internalSyncObject)
			{
				if (Application.eventHandlers == null)
				{
					Application.eventHandlers = new EventHandlerList();
				}
				Application.eventHandlers.AddHandler(key, value);
			}
		}

		// Token: 0x060012B5 RID: 4789 RVA: 0x000118C0 File Offset: 0x000108C0
		private static void RemoveEventHandler(object key, Delegate value)
		{
			lock (Application.internalSyncObject)
			{
				if (Application.eventHandlers != null)
				{
					Application.eventHandlers.RemoveHandler(key, value);
				}
			}
		}

		// Token: 0x060012B6 RID: 4790 RVA: 0x00011908 File Offset: 0x00010908
		public static void AddMessageFilter(IMessageFilter value)
		{
			IntSecurity.UnmanagedCode.Demand();
			Application.ThreadContext.FromCurrent().AddMessageFilter(value);
		}

		// Token: 0x060012B7 RID: 4791 RVA: 0x00011920 File Offset: 0x00010920
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static bool FilterMessage(ref Message message)
		{
			NativeMethods.MSG msg = default(NativeMethods.MSG);
			msg.hwnd = message.HWnd;
			msg.message = message.Msg;
			msg.wParam = message.WParam;
			msg.lParam = message.LParam;
			bool flag2;
			bool flag = Application.ThreadContext.FromCurrent().ProcessFilters(ref msg, out flag2);
			if (flag2)
			{
				message.HWnd = msg.hwnd;
				message.Msg = msg.message;
				message.WParam = msg.wParam;
				message.LParam = msg.lParam;
			}
			return flag;
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x060012B8 RID: 4792 RVA: 0x000119B0 File Offset: 0x000109B0
		// (remove) Token: 0x060012B9 RID: 4793 RVA: 0x00011A04 File Offset: 0x00010A04
		public static event EventHandler Idle
		{
			add
			{
				Application.ThreadContext threadContext = Application.ThreadContext.FromCurrent();
				lock (threadContext)
				{
					Application.ThreadContext threadContext3 = threadContext;
					threadContext3.idleHandler = (EventHandler)Delegate.Combine(threadContext3.idleHandler, value);
					UnsafeNativeMethods.IMsoComponentManager componentManager = threadContext.ComponentManager;
				}
			}
			remove
			{
				Application.ThreadContext threadContext = Application.ThreadContext.FromCurrent();
				lock (threadContext)
				{
					Application.ThreadContext threadContext3 = threadContext;
					threadContext3.idleHandler = (EventHandler)Delegate.Remove(threadContext3.idleHandler, value);
				}
			}
		}

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x060012BA RID: 4794 RVA: 0x00011A50 File Offset: 0x00010A50
		// (remove) Token: 0x060012BB RID: 4795 RVA: 0x00011A9C File Offset: 0x00010A9C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static event EventHandler EnterThreadModal
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			add
			{
				Application.ThreadContext threadContext = Application.ThreadContext.FromCurrent();
				lock (threadContext)
				{
					Application.ThreadContext threadContext3 = threadContext;
					threadContext3.enterModalHandler = (EventHandler)Delegate.Combine(threadContext3.enterModalHandler, value);
				}
			}
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			remove
			{
				Application.ThreadContext threadContext = Application.ThreadContext.FromCurrent();
				lock (threadContext)
				{
					Application.ThreadContext threadContext3 = threadContext;
					threadContext3.enterModalHandler = (EventHandler)Delegate.Remove(threadContext3.enterModalHandler, value);
				}
			}
		}

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x060012BC RID: 4796 RVA: 0x00011AE8 File Offset: 0x00010AE8
		// (remove) Token: 0x060012BD RID: 4797 RVA: 0x00011B34 File Offset: 0x00010B34
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static event EventHandler LeaveThreadModal
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			add
			{
				Application.ThreadContext threadContext = Application.ThreadContext.FromCurrent();
				lock (threadContext)
				{
					Application.ThreadContext threadContext3 = threadContext;
					threadContext3.leaveModalHandler = (EventHandler)Delegate.Combine(threadContext3.leaveModalHandler, value);
				}
			}
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			remove
			{
				Application.ThreadContext threadContext = Application.ThreadContext.FromCurrent();
				lock (threadContext)
				{
					Application.ThreadContext threadContext3 = threadContext;
					threadContext3.leaveModalHandler = (EventHandler)Delegate.Remove(threadContext3.leaveModalHandler, value);
				}
			}
		}

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x060012BE RID: 4798 RVA: 0x00011B80 File Offset: 0x00010B80
		// (remove) Token: 0x060012BF RID: 4799 RVA: 0x00011BC8 File Offset: 0x00010BC8
		public static event ThreadExceptionEventHandler ThreadException
		{
			add
			{
				IntSecurity.AffectThreadBehavior.Demand();
				Application.ThreadContext threadContext = Application.ThreadContext.FromCurrent();
				lock (threadContext)
				{
					threadContext.threadExceptionHandler = value;
				}
			}
			remove
			{
				Application.ThreadContext threadContext = Application.ThreadContext.FromCurrent();
				lock (threadContext)
				{
					Application.ThreadContext threadContext3 = threadContext;
					threadContext3.threadExceptionHandler = (ThreadExceptionEventHandler)Delegate.Remove(threadContext3.threadExceptionHandler, value);
				}
			}
		}

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x060012C0 RID: 4800 RVA: 0x00011C14 File Offset: 0x00010C14
		// (remove) Token: 0x060012C1 RID: 4801 RVA: 0x00011C21 File Offset: 0x00010C21
		public static event EventHandler ThreadExit
		{
			add
			{
				Application.AddEventHandler(Application.EVENT_THREADEXIT, value);
			}
			remove
			{
				Application.RemoveEventHandler(Application.EVENT_THREADEXIT, value);
			}
		}

		// Token: 0x060012C2 RID: 4802 RVA: 0x00011C2E File Offset: 0x00010C2E
		internal static void BeginModalMessageLoop()
		{
			Application.ThreadContext.FromCurrent().BeginModalMessageLoop(null);
		}

		// Token: 0x060012C3 RID: 4803 RVA: 0x00011C3B File Offset: 0x00010C3B
		public static void DoEvents()
		{
			Application.ThreadContext.FromCurrent().RunMessageLoop(2, null);
		}

		// Token: 0x060012C4 RID: 4804 RVA: 0x00011C49 File Offset: 0x00010C49
		internal static void DoEventsModal()
		{
			Application.ThreadContext.FromCurrent().RunMessageLoop(-2, null);
		}

		// Token: 0x060012C5 RID: 4805 RVA: 0x00011C58 File Offset: 0x00010C58
		public static void EnableVisualStyles()
		{
			string text = null;
			new FileIOPermission(PermissionState.None)
			{
				AllFiles = FileIOPermissionAccess.PathDiscovery
			}.Assert();
			try
			{
				text = typeof(Application).Assembly.Location;
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			if (text != null)
			{
				Application.EnableVisualStylesInternal(text, 101);
			}
		}

		// Token: 0x060012C6 RID: 4806 RVA: 0x00011CB4 File Offset: 0x00010CB4
		private static void EnableVisualStylesInternal(string assemblyFileName, int nativeResourceID)
		{
			Application.useVisualStyles = UnsafeNativeMethods.ThemingScope.CreateActivationContext(assemblyFileName, nativeResourceID);
		}

		// Token: 0x060012C7 RID: 4807 RVA: 0x00011CC2 File Offset: 0x00010CC2
		internal static void EndModalMessageLoop()
		{
			Application.ThreadContext.FromCurrent().EndModalMessageLoop(null);
		}

		// Token: 0x060012C8 RID: 4808 RVA: 0x00011CCF File Offset: 0x00010CCF
		public static void Exit()
		{
			Application.Exit(null);
		}

		// Token: 0x060012C9 RID: 4809 RVA: 0x00011CD8 File Offset: 0x00010CD8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static void Exit(CancelEventArgs e)
		{
			Assembly entryAssembly = Assembly.GetEntryAssembly();
			Assembly callingAssembly = Assembly.GetCallingAssembly();
			if (entryAssembly == null || callingAssembly == null || !entryAssembly.Equals(callingAssembly))
			{
				IntSecurity.AffectThreadBehavior.Demand();
			}
			bool flag = Application.ExitInternal();
			if (e != null)
			{
				e.Cancel = flag;
			}
		}

		// Token: 0x060012CA RID: 4810 RVA: 0x00011D1C File Offset: 0x00010D1C
		private static bool ExitInternal()
		{
			bool flag = false;
			lock (Application.internalSyncObject)
			{
				if (Application.exiting)
				{
					return false;
				}
				Application.exiting = true;
				try
				{
					if (Application.forms != null)
					{
						foreach (object obj2 in Application.OpenFormsInternal)
						{
							Form form = (Form)obj2;
							if (form.RaiseFormClosingOnAppExit())
							{
								flag = true;
								break;
							}
						}
					}
					if (!flag)
					{
						if (Application.forms != null)
						{
							while (Application.OpenFormsInternal.Count > 0)
							{
								Application.OpenFormsInternal[0].RaiseFormClosedOnAppExit();
							}
						}
						Application.ThreadContext.ExitApplication();
					}
				}
				finally
				{
					Application.exiting = false;
				}
			}
			return flag;
		}

		// Token: 0x060012CB RID: 4811 RVA: 0x00011E04 File Offset: 0x00010E04
		public static void ExitThread()
		{
			IntSecurity.AffectThreadBehavior.Demand();
			Application.ExitThreadInternal();
		}

		// Token: 0x060012CC RID: 4812 RVA: 0x00011E18 File Offset: 0x00010E18
		private static void ExitThreadInternal()
		{
			Application.ThreadContext threadContext = Application.ThreadContext.FromCurrent();
			if (threadContext.ApplicationContext != null)
			{
				threadContext.ApplicationContext.ExitThread();
				return;
			}
			threadContext.Dispose(true);
		}

		// Token: 0x060012CD RID: 4813 RVA: 0x00011E46 File Offset: 0x00010E46
		internal static void FormActivated(bool modal, bool activated)
		{
			if (modal)
			{
				return;
			}
			Application.ThreadContext.FromCurrent().FormActivated(activated);
		}

		// Token: 0x060012CE RID: 4814 RVA: 0x00011E58 File Offset: 0x00010E58
		private static FileVersionInfo GetAppFileVersionInfo()
		{
			lock (Application.internalSyncObject)
			{
				if (Application.appFileVersion == null)
				{
					Type appMainType = Application.GetAppMainType();
					if (appMainType != null)
					{
						new FileIOPermission(PermissionState.None)
						{
							AllFiles = (FileIOPermissionAccess.Read | FileIOPermissionAccess.PathDiscovery)
						}.Assert();
						try
						{
							Application.appFileVersion = FileVersionInfo.GetVersionInfo(appMainType.Module.FullyQualifiedName);
							goto IL_005D;
						}
						finally
						{
							CodeAccessPermission.RevertAssert();
						}
					}
					Application.appFileVersion = FileVersionInfo.GetVersionInfo(Application.ExecutablePath);
				}
				IL_005D:;
			}
			return (FileVersionInfo)Application.appFileVersion;
		}

		// Token: 0x060012CF RID: 4815 RVA: 0x00011EF4 File Offset: 0x00010EF4
		private static Type GetAppMainType()
		{
			lock (Application.internalSyncObject)
			{
				if (Application.mainType == null)
				{
					Assembly entryAssembly = Assembly.GetEntryAssembly();
					if (entryAssembly != null)
					{
						Application.mainType = entryAssembly.EntryPoint.ReflectedType;
					}
				}
			}
			return Application.mainType;
		}

		// Token: 0x060012D0 RID: 4816 RVA: 0x00011F4C File Offset: 0x00010F4C
		private static Application.ThreadContext GetContextForHandle(HandleRef handle)
		{
			int num;
			int windowThreadProcessId = SafeNativeMethods.GetWindowThreadProcessId(handle, out num);
			return Application.ThreadContext.FromId(windowThreadProcessId);
		}

		// Token: 0x060012D1 RID: 4817 RVA: 0x00011F6C File Offset: 0x00010F6C
		private static string GetDataPath(string basePath)
		{
			string text = "{0}\\{1}\\{2}\\{3}";
			string text2 = Application.CompanyName;
			string text3 = Application.ProductName;
			string text4 = Application.ProductVersion;
			string text5 = string.Format(CultureInfo.CurrentCulture, text, new object[] { basePath, text2, text3, text4 });
			lock (Application.internalSyncObject)
			{
				if (!Directory.Exists(text5))
				{
					Directory.CreateDirectory(text5);
				}
			}
			return text5;
		}

		// Token: 0x060012D2 RID: 4818 RVA: 0x00011FF8 File Offset: 0x00010FF8
		private static void RaiseExit()
		{
			if (Application.eventHandlers != null)
			{
				Delegate @delegate = Application.eventHandlers[Application.EVENT_APPLICATIONEXIT];
				if (@delegate != null)
				{
					((EventHandler)@delegate)(null, EventArgs.Empty);
				}
			}
		}

		// Token: 0x060012D3 RID: 4819 RVA: 0x00012030 File Offset: 0x00011030
		private static void RaiseThreadExit()
		{
			if (Application.eventHandlers != null)
			{
				Delegate @delegate = Application.eventHandlers[Application.EVENT_THREADEXIT];
				if (@delegate != null)
				{
					((EventHandler)@delegate)(null, EventArgs.Empty);
				}
			}
		}

		// Token: 0x060012D4 RID: 4820 RVA: 0x00012068 File Offset: 0x00011068
		internal static void ParkHandle(HandleRef handle)
		{
			Application.ThreadContext contextForHandle = Application.GetContextForHandle(handle);
			if (contextForHandle != null)
			{
				contextForHandle.ParkingWindow.ParkHandle(handle);
			}
		}

		// Token: 0x060012D5 RID: 4821 RVA: 0x0001208C File Offset: 0x0001108C
		internal static void ParkHandle(CreateParams cp)
		{
			Application.ThreadContext threadContext = Application.ThreadContext.FromCurrent();
			if (threadContext != null)
			{
				cp.Parent = threadContext.ParkingWindow.Handle;
			}
		}

		// Token: 0x060012D6 RID: 4822 RVA: 0x000120B3 File Offset: 0x000110B3
		public static ApartmentState OleRequired()
		{
			return Application.ThreadContext.FromCurrent().OleRequired();
		}

		// Token: 0x060012D7 RID: 4823 RVA: 0x000120BF File Offset: 0x000110BF
		public static void OnThreadException(Exception t)
		{
			Application.ThreadContext.FromCurrent().OnThreadException(t);
		}

		// Token: 0x060012D8 RID: 4824 RVA: 0x000120CC File Offset: 0x000110CC
		internal static void UnparkHandle(HandleRef handle)
		{
			Application.ThreadContext contextForHandle = Application.GetContextForHandle(handle);
			if (contextForHandle != null)
			{
				contextForHandle.ParkingWindow.UnparkHandle(handle);
			}
		}

		// Token: 0x060012D9 RID: 4825 RVA: 0x000120F0 File Offset: 0x000110F0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void RaiseIdle(EventArgs e)
		{
			Application.ThreadContext threadContext = Application.ThreadContext.FromCurrent();
			if (threadContext.idleHandler != null)
			{
				threadContext.idleHandler(Thread.CurrentThread, e);
			}
		}

		// Token: 0x060012DA RID: 4826 RVA: 0x0001211C File Offset: 0x0001111C
		public static void RemoveMessageFilter(IMessageFilter value)
		{
			Application.ThreadContext.FromCurrent().RemoveMessageFilter(value);
		}

		// Token: 0x060012DB RID: 4827 RVA: 0x0001212C File Offset: 0x0001112C
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void Restart()
		{
			if (Assembly.GetEntryAssembly() == null)
			{
				throw new NotSupportedException(SR.GetString("RestartNotSupported"));
			}
			bool flag = false;
			Process currentProcess = Process.GetCurrentProcess();
			if (string.Equals(currentProcess.MainModule.ModuleName, "ieexec.exe", StringComparison.OrdinalIgnoreCase))
			{
				string text = string.Empty;
				new FileIOPermission(PermissionState.Unrestricted).Assert();
				try
				{
					text = Path.GetDirectoryName(typeof(object).Module.FullyQualifiedName);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				if (string.Equals(text + "\\ieexec.exe", currentProcess.MainModule.FileName, StringComparison.OrdinalIgnoreCase))
				{
					flag = true;
					Application.ExitInternal();
					string text2 = AppDomain.CurrentDomain.GetData("APP_LAUNCH_URL") as string;
					if (text2 != null)
					{
						Process.Start(currentProcess.MainModule.FileName, text2);
					}
				}
			}
			if (!flag)
			{
				if (ApplicationDeployment.IsNetworkDeployed)
				{
					string updatedApplicationFullName = ApplicationDeployment.CurrentDeployment.UpdatedApplicationFullName;
					uint hostTypeFromMetaData = (uint)Application.ClickOnceUtility.GetHostTypeFromMetaData(updatedApplicationFullName);
					Application.ExitInternal();
					UnsafeNativeMethods.CorLaunchApplication(hostTypeFromMetaData, updatedApplicationFullName, 0, null, 0, null, new UnsafeNativeMethods.PROCESS_INFORMATION());
					return;
				}
				string[] commandLineArgs = Environment.GetCommandLineArgs();
				StringBuilder stringBuilder = new StringBuilder((commandLineArgs.Length - 1) * 16);
				for (int i = 1; i < commandLineArgs.Length - 1; i++)
				{
					stringBuilder.Append('"');
					stringBuilder.Append(commandLineArgs[i]);
					stringBuilder.Append("\" ");
				}
				if (commandLineArgs.Length > 1)
				{
					stringBuilder.Append('"');
					stringBuilder.Append(commandLineArgs[commandLineArgs.Length - 1]);
					stringBuilder.Append('"');
				}
				ProcessStartInfo startInfo = Process.GetCurrentProcess().StartInfo;
				startInfo.FileName = Application.ExecutablePath;
				if (stringBuilder.Length > 0)
				{
					startInfo.Arguments = stringBuilder.ToString();
				}
				Application.ExitInternal();
				Process.Start(startInfo);
			}
		}

		// Token: 0x060012DC RID: 4828 RVA: 0x000122FC File Offset: 0x000112FC
		public static void Run()
		{
			Application.ThreadContext.FromCurrent().RunMessageLoop(-1, new ApplicationContext());
		}

		// Token: 0x060012DD RID: 4829 RVA: 0x0001230E File Offset: 0x0001130E
		public static void Run(Form mainForm)
		{
			Application.ThreadContext.FromCurrent().RunMessageLoop(-1, new ApplicationContext(mainForm));
		}

		// Token: 0x060012DE RID: 4830 RVA: 0x00012321 File Offset: 0x00011321
		public static void Run(ApplicationContext context)
		{
			Application.ThreadContext.FromCurrent().RunMessageLoop(-1, context);
		}

		// Token: 0x060012DF RID: 4831 RVA: 0x0001232F File Offset: 0x0001132F
		internal static void RunDialog(Form form)
		{
			Application.ThreadContext.FromCurrent().RunMessageLoop(4, new Application.ModalApplicationContext(form));
		}

		// Token: 0x060012E0 RID: 4832 RVA: 0x00012342 File Offset: 0x00011342
		public static void SetCompatibleTextRenderingDefault(bool defaultValue)
		{
			if (NativeWindow.AnyHandleCreated)
			{
				throw new InvalidOperationException(SR.GetString("Win32WindowAlreadyCreated"));
			}
			Control.UseCompatibleTextRenderingDefault = defaultValue;
		}

		// Token: 0x060012E1 RID: 4833 RVA: 0x00012361 File Offset: 0x00011361
		public static bool SetSuspendState(PowerState state, bool force, bool disableWakeEvent)
		{
			IntSecurity.AffectMachineState.Demand();
			return UnsafeNativeMethods.SetSuspendState(state == PowerState.Hibernate, force, disableWakeEvent);
		}

		// Token: 0x060012E2 RID: 4834 RVA: 0x00012378 File Offset: 0x00011378
		public static void SetUnhandledExceptionMode(UnhandledExceptionMode mode)
		{
			Application.SetUnhandledExceptionMode(mode, true);
		}

		// Token: 0x060012E3 RID: 4835 RVA: 0x00012381 File Offset: 0x00011381
		public static void SetUnhandledExceptionMode(UnhandledExceptionMode mode, bool threadScope)
		{
			IntSecurity.AffectThreadBehavior.Demand();
			NativeWindow.SetUnhandledExceptionModeInternal(mode, threadScope);
		}

		// Token: 0x04001028 RID: 4136
		private const string everettThreadAffinityValue = "EnableSystemEventsThreadAffinityCompatibility";

		// Token: 0x04001029 RID: 4137
		private const string IEEXEC = "ieexec.exe";

		// Token: 0x0400102A RID: 4138
		private const string CLICKONCE_APPS_DATADIRECTORY = "DataDirectory";

		// Token: 0x0400102B RID: 4139
		private static EventHandlerList eventHandlers;

		// Token: 0x0400102C RID: 4140
		private static string startupPath;

		// Token: 0x0400102D RID: 4141
		private static string executablePath;

		// Token: 0x0400102E RID: 4142
		private static object appFileVersion;

		// Token: 0x0400102F RID: 4143
		private static Type mainType;

		// Token: 0x04001030 RID: 4144
		private static string companyName;

		// Token: 0x04001031 RID: 4145
		private static string productName;

		// Token: 0x04001032 RID: 4146
		private static string productVersion;

		// Token: 0x04001033 RID: 4147
		private static string safeTopLevelCaptionSuffix;

		// Token: 0x04001034 RID: 4148
		private static bool useVisualStyles = false;

		// Token: 0x04001035 RID: 4149
		private static FormCollection forms = null;

		// Token: 0x04001036 RID: 4150
		private static object internalSyncObject = new object();

		// Token: 0x04001037 RID: 4151
		private static bool useWaitCursor = false;

		// Token: 0x04001038 RID: 4152
		private static bool useEverettThreadAffinity = false;

		// Token: 0x04001039 RID: 4153
		private static bool checkedThreadAffinity = false;

		// Token: 0x0400103A RID: 4154
		private static bool exiting;

		// Token: 0x0400103B RID: 4155
		private static readonly object EVENT_APPLICATIONEXIT = new object();

		// Token: 0x0400103C RID: 4156
		private static readonly object EVENT_THREADEXIT = new object();

		// Token: 0x020001DF RID: 479
		// (Invoke) Token: 0x060012E6 RID: 4838
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public delegate bool MessageLoopCallback();

		// Token: 0x020001E0 RID: 480
		private class ClickOnceUtility
		{
			// Token: 0x060012E9 RID: 4841 RVA: 0x000123D2 File Offset: 0x000113D2
			private ClickOnceUtility()
			{
			}

			// Token: 0x060012EA RID: 4842 RVA: 0x000123DC File Offset: 0x000113DC
			public static Application.ClickOnceUtility.HostType GetHostTypeFromMetaData(string appFullName)
			{
				Application.ClickOnceUtility.HostType hostType = Application.ClickOnceUtility.HostType.Default;
				try
				{
					IDefinitionAppId definitionAppId = IsolationInterop.AppIdAuthority.TextToDefinition(0U, appFullName);
					hostType = (Application.ClickOnceUtility.GetPropertyBoolean(definitionAppId, "IsFullTrust") ? Application.ClickOnceUtility.HostType.CorFlag : Application.ClickOnceUtility.HostType.AppLaunch);
				}
				catch
				{
				}
				return hostType;
			}

			// Token: 0x060012EB RID: 4843 RVA: 0x00012424 File Offset: 0x00011424
			private static bool GetPropertyBoolean(IDefinitionAppId appId, string propName)
			{
				string propertyString = Application.ClickOnceUtility.GetPropertyString(appId, propName);
				if (string.IsNullOrEmpty(propertyString))
				{
					return false;
				}
				bool flag;
				try
				{
					flag = Convert.ToBoolean(propertyString, CultureInfo.InvariantCulture);
				}
				catch
				{
					flag = false;
				}
				return flag;
			}

			// Token: 0x060012EC RID: 4844 RVA: 0x00012468 File Offset: 0x00011468
			private static string GetPropertyString(IDefinitionAppId appId, string propName)
			{
				byte[] deploymentProperty = IsolationInterop.UserStore.GetDeploymentProperty(Store.GetPackagePropertyFlags.Nothing, appId, Application.ClickOnceUtility.InstallReference, new Guid("2ad613da-6fdb-4671-af9e-18ab2e4df4d8"), propName);
				int num = deploymentProperty.Length;
				if (num == 0 || deploymentProperty.Length % 2 != 0 || deploymentProperty[num - 2] != 0 || deploymentProperty[num - 1] != 0)
				{
					return null;
				}
				return Encoding.Unicode.GetString(deploymentProperty, 0, num - 2);
			}

			// Token: 0x170001E5 RID: 485
			// (get) Token: 0x060012ED RID: 4845 RVA: 0x000124BF File Offset: 0x000114BF
			private static StoreApplicationReference InstallReference
			{
				get
				{
					return new StoreApplicationReference(IsolationInterop.GUID_SXS_INSTALL_REFERENCE_SCHEME_OPAQUESTRING, "{3f471841-eef2-47d6-89c0-d028f03a4ad5}", null);
				}
			}

			// Token: 0x020001E1 RID: 481
			public enum HostType
			{
				// Token: 0x0400103E RID: 4158
				Default,
				// Token: 0x0400103F RID: 4159
				AppLaunch,
				// Token: 0x04001040 RID: 4160
				CorFlag
			}
		}

		// Token: 0x020001E2 RID: 482
		private class ComponentManager : UnsafeNativeMethods.IMsoComponentManager
		{
			// Token: 0x170001E6 RID: 486
			// (get) Token: 0x060012EE RID: 4846 RVA: 0x000124D1 File Offset: 0x000114D1
			private Hashtable OleComponents
			{
				get
				{
					if (this.oleComponents == null)
					{
						this.oleComponents = new Hashtable();
						this.cookieCounter = 0;
					}
					return this.oleComponents;
				}
			}

			// Token: 0x060012EF RID: 4847 RVA: 0x000124F3 File Offset: 0x000114F3
			int UnsafeNativeMethods.IMsoComponentManager.QueryService(ref Guid guidService, ref Guid iid, out object ppvObj)
			{
				ppvObj = null;
				return -2147467262;
			}

			// Token: 0x060012F0 RID: 4848 RVA: 0x000124FD File Offset: 0x000114FD
			bool UnsafeNativeMethods.IMsoComponentManager.FDebugMessage(IntPtr hInst, int msg, IntPtr wparam, IntPtr lparam)
			{
				return true;
			}

			// Token: 0x060012F1 RID: 4849 RVA: 0x00012500 File Offset: 0x00011500
			bool UnsafeNativeMethods.IMsoComponentManager.FRegisterComponent(UnsafeNativeMethods.IMsoComponent component, NativeMethods.MSOCRINFOSTRUCT pcrinfo, out int dwComponentID)
			{
				Application.ComponentManager.ComponentHashtableEntry componentHashtableEntry = new Application.ComponentManager.ComponentHashtableEntry();
				componentHashtableEntry.component = component;
				componentHashtableEntry.componentInfo = pcrinfo;
				this.OleComponents.Add(++this.cookieCounter, componentHashtableEntry);
				dwComponentID = this.cookieCounter;
				return true;
			}

			// Token: 0x060012F2 RID: 4850 RVA: 0x0001254C File Offset: 0x0001154C
			bool UnsafeNativeMethods.IMsoComponentManager.FRevokeComponent(int dwComponentID)
			{
				Application.ComponentManager.ComponentHashtableEntry componentHashtableEntry = (Application.ComponentManager.ComponentHashtableEntry)this.OleComponents[dwComponentID];
				if (componentHashtableEntry == null)
				{
					return false;
				}
				if (componentHashtableEntry.component == this.activeComponent)
				{
					this.activeComponent = null;
				}
				if (componentHashtableEntry.component == this.trackingComponent)
				{
					this.trackingComponent = null;
				}
				this.OleComponents.Remove(dwComponentID);
				return true;
			}

			// Token: 0x060012F3 RID: 4851 RVA: 0x000125B4 File Offset: 0x000115B4
			bool UnsafeNativeMethods.IMsoComponentManager.FUpdateComponentRegistration(int dwComponentID, NativeMethods.MSOCRINFOSTRUCT info)
			{
				Application.ComponentManager.ComponentHashtableEntry componentHashtableEntry = (Application.ComponentManager.ComponentHashtableEntry)this.OleComponents[dwComponentID];
				if (componentHashtableEntry == null)
				{
					return false;
				}
				componentHashtableEntry.componentInfo = info;
				return true;
			}

			// Token: 0x060012F4 RID: 4852 RVA: 0x000125E8 File Offset: 0x000115E8
			bool UnsafeNativeMethods.IMsoComponentManager.FOnComponentActivate(int dwComponentID)
			{
				Application.ComponentManager.ComponentHashtableEntry componentHashtableEntry = (Application.ComponentManager.ComponentHashtableEntry)this.OleComponents[dwComponentID];
				if (componentHashtableEntry == null)
				{
					return false;
				}
				this.activeComponent = componentHashtableEntry.component;
				return true;
			}

			// Token: 0x060012F5 RID: 4853 RVA: 0x00012620 File Offset: 0x00011620
			bool UnsafeNativeMethods.IMsoComponentManager.FSetTrackingComponent(int dwComponentID, bool fTrack)
			{
				Application.ComponentManager.ComponentHashtableEntry componentHashtableEntry = (Application.ComponentManager.ComponentHashtableEntry)this.OleComponents[dwComponentID];
				if (componentHashtableEntry == null)
				{
					return false;
				}
				if ((componentHashtableEntry.component == this.trackingComponent) ^ fTrack)
				{
					return false;
				}
				if (fTrack)
				{
					this.trackingComponent = componentHashtableEntry.component;
				}
				else
				{
					this.trackingComponent = null;
				}
				return true;
			}

			// Token: 0x060012F6 RID: 4854 RVA: 0x00012678 File Offset: 0x00011678
			void UnsafeNativeMethods.IMsoComponentManager.OnComponentEnterState(int dwComponentID, int uStateID, int uContext, int cpicmExclude, int rgpicmExclude, int dwReserved)
			{
				this.currentState |= uStateID;
				if (uContext == 0 || uContext == 1)
				{
					foreach (object obj in this.OleComponents.Values)
					{
						Application.ComponentManager.ComponentHashtableEntry componentHashtableEntry = (Application.ComponentManager.ComponentHashtableEntry)obj;
						componentHashtableEntry.component.OnEnterState(uStateID, true);
					}
				}
			}

			// Token: 0x060012F7 RID: 4855 RVA: 0x000126F4 File Offset: 0x000116F4
			bool UnsafeNativeMethods.IMsoComponentManager.FOnComponentExitState(int dwComponentID, int uStateID, int uContext, int cpicmExclude, int rgpicmExclude)
			{
				this.currentState &= ~uStateID;
				if (uContext == 0 || uContext == 1)
				{
					foreach (object obj in this.OleComponents.Values)
					{
						Application.ComponentManager.ComponentHashtableEntry componentHashtableEntry = (Application.ComponentManager.ComponentHashtableEntry)obj;
						componentHashtableEntry.component.OnEnterState(uStateID, false);
					}
				}
				return false;
			}

			// Token: 0x060012F8 RID: 4856 RVA: 0x00012770 File Offset: 0x00011770
			bool UnsafeNativeMethods.IMsoComponentManager.FInState(int uStateID, IntPtr pvoid)
			{
				return (this.currentState & uStateID) != 0;
			}

			// Token: 0x060012F9 RID: 4857 RVA: 0x00012780 File Offset: 0x00011780
			bool UnsafeNativeMethods.IMsoComponentManager.FContinueIdle()
			{
				NativeMethods.MSG msg = default(NativeMethods.MSG);
				return !UnsafeNativeMethods.PeekMessage(ref msg, NativeMethods.NullHandleRef, 0, 0, 0);
			}

			// Token: 0x060012FA RID: 4858 RVA: 0x000127A8 File Offset: 0x000117A8
			bool UnsafeNativeMethods.IMsoComponentManager.FPushMessageLoop(int dwComponentID, int reason, int pvLoopData)
			{
				int num = this.currentState;
				bool flag = true;
				if (!this.OleComponents.ContainsKey(dwComponentID))
				{
					return false;
				}
				UnsafeNativeMethods.IMsoComponent msoComponent = this.activeComponent;
				try
				{
					NativeMethods.MSG msg = default(NativeMethods.MSG);
					NativeMethods.MSG[] array = new NativeMethods.MSG[] { msg };
					Application.ComponentManager.ComponentHashtableEntry componentHashtableEntry = (Application.ComponentManager.ComponentHashtableEntry)this.OleComponents[dwComponentID];
					if (componentHashtableEntry == null)
					{
						return false;
					}
					UnsafeNativeMethods.IMsoComponent component = componentHashtableEntry.component;
					this.activeComponent = component;
					while (flag)
					{
						UnsafeNativeMethods.IMsoComponent msoComponent2;
						if (this.trackingComponent != null)
						{
							msoComponent2 = this.trackingComponent;
						}
						else if (this.activeComponent != null)
						{
							msoComponent2 = this.activeComponent;
						}
						else
						{
							msoComponent2 = component;
						}
						bool flag2 = UnsafeNativeMethods.PeekMessage(ref msg, NativeMethods.NullHandleRef, 0, 0, 0);
						if (flag2)
						{
							array[0] = msg;
							flag = msoComponent2.FContinueMessageLoop(reason, pvLoopData, array);
							if (flag)
							{
								bool flag3;
								if (msg.hwnd != IntPtr.Zero && SafeNativeMethods.IsWindowUnicode(new HandleRef(null, msg.hwnd)))
								{
									flag3 = true;
									UnsafeNativeMethods.GetMessageW(ref msg, NativeMethods.NullHandleRef, 0, 0);
								}
								else
								{
									flag3 = false;
									UnsafeNativeMethods.GetMessageA(ref msg, NativeMethods.NullHandleRef, 0, 0);
								}
								if (msg.message == 18)
								{
									Application.ThreadContext.FromCurrent().DisposeThreadWindows();
									if (reason != -1)
									{
										UnsafeNativeMethods.PostQuitMessage((int)msg.wParam);
									}
									flag = false;
									break;
								}
								if (!msoComponent2.FPreTranslateMessage(ref msg))
								{
									UnsafeNativeMethods.TranslateMessage(ref msg);
									if (flag3)
									{
										UnsafeNativeMethods.DispatchMessageW(ref msg);
									}
									else
									{
										UnsafeNativeMethods.DispatchMessageA(ref msg);
									}
								}
							}
						}
						else
						{
							if (reason == 2 || reason == -2)
							{
								break;
							}
							bool flag4 = false;
							if (this.OleComponents != null)
							{
								foreach (object obj in this.OleComponents.Values)
								{
									Application.ComponentManager.ComponentHashtableEntry componentHashtableEntry2 = (Application.ComponentManager.ComponentHashtableEntry)obj;
									flag4 |= componentHashtableEntry2.component.FDoIdle(-1);
								}
							}
							flag = msoComponent2.FContinueMessageLoop(reason, pvLoopData, null);
							if (flag)
							{
								if (flag4)
								{
									UnsafeNativeMethods.MsgWaitForMultipleObjectsEx(0, IntPtr.Zero, 100, 255, 4);
								}
								else if (!UnsafeNativeMethods.PeekMessage(ref msg, NativeMethods.NullHandleRef, 0, 0, 0))
								{
									UnsafeNativeMethods.WaitMessage();
								}
							}
						}
					}
				}
				finally
				{
					this.currentState = num;
					this.activeComponent = msoComponent;
				}
				return !flag;
			}

			// Token: 0x060012FB RID: 4859 RVA: 0x00012A1C File Offset: 0x00011A1C
			bool UnsafeNativeMethods.IMsoComponentManager.FCreateSubComponentManager(object punkOuter, object punkServProv, ref Guid riid, out IntPtr ppvObj)
			{
				ppvObj = IntPtr.Zero;
				return false;
			}

			// Token: 0x060012FC RID: 4860 RVA: 0x00012A2B File Offset: 0x00011A2B
			bool UnsafeNativeMethods.IMsoComponentManager.FGetParentComponentManager(out UnsafeNativeMethods.IMsoComponentManager ppicm)
			{
				ppicm = null;
				return false;
			}

			// Token: 0x060012FD RID: 4861 RVA: 0x00012A34 File Offset: 0x00011A34
			bool UnsafeNativeMethods.IMsoComponentManager.FGetActiveComponent(int dwgac, UnsafeNativeMethods.IMsoComponent[] ppic, NativeMethods.MSOCRINFOSTRUCT info, int dwReserved)
			{
				UnsafeNativeMethods.IMsoComponent msoComponent = null;
				if (dwgac == 0)
				{
					msoComponent = this.activeComponent;
				}
				else if (dwgac == 1)
				{
					msoComponent = this.trackingComponent;
				}
				else if (dwgac == 2)
				{
					if (this.trackingComponent != null)
					{
						msoComponent = this.trackingComponent;
					}
					else
					{
						msoComponent = this.activeComponent;
					}
				}
				if (ppic != null)
				{
					ppic[0] = msoComponent;
				}
				if (info != null && msoComponent != null)
				{
					foreach (object obj in this.OleComponents.Values)
					{
						Application.ComponentManager.ComponentHashtableEntry componentHashtableEntry = (Application.ComponentManager.ComponentHashtableEntry)obj;
						if (componentHashtableEntry.component == msoComponent)
						{
							info = componentHashtableEntry.componentInfo;
							break;
						}
					}
				}
				return msoComponent != null;
			}

			// Token: 0x04001041 RID: 4161
			private Hashtable oleComponents;

			// Token: 0x04001042 RID: 4162
			private int cookieCounter;

			// Token: 0x04001043 RID: 4163
			private UnsafeNativeMethods.IMsoComponent activeComponent;

			// Token: 0x04001044 RID: 4164
			private UnsafeNativeMethods.IMsoComponent trackingComponent;

			// Token: 0x04001045 RID: 4165
			private int currentState;

			// Token: 0x020001E3 RID: 483
			private class ComponentHashtableEntry
			{
				// Token: 0x04001046 RID: 4166
				public UnsafeNativeMethods.IMsoComponent component;

				// Token: 0x04001047 RID: 4167
				public NativeMethods.MSOCRINFOSTRUCT componentInfo;
			}
		}

		// Token: 0x020001E4 RID: 484
		internal sealed class ThreadContext : MarshalByRefObject, UnsafeNativeMethods.IMsoComponent
		{
			// Token: 0x06001300 RID: 4864 RVA: 0x00012AFC File Offset: 0x00011AFC
			public ThreadContext()
			{
				IntPtr zero = IntPtr.Zero;
				UnsafeNativeMethods.DuplicateHandle(new HandleRef(null, SafeNativeMethods.GetCurrentProcess()), new HandleRef(null, SafeNativeMethods.GetCurrentThread()), new HandleRef(null, SafeNativeMethods.GetCurrentProcess()), ref zero, 0, false, 2);
				this.handle = zero;
				this.id = SafeNativeMethods.GetCurrentThreadId();
				this.messageLoopCount = 0;
				Application.ThreadContext.currentThreadContext = this;
				Application.ThreadContext.contextHash[this.id] = this;
			}

			// Token: 0x170001E7 RID: 487
			// (get) Token: 0x06001301 RID: 4865 RVA: 0x00012B89 File Offset: 0x00011B89
			public ApplicationContext ApplicationContext
			{
				get
				{
					return this.applicationContext;
				}
			}

			// Token: 0x170001E8 RID: 488
			// (get) Token: 0x06001302 RID: 4866 RVA: 0x00012B94 File Offset: 0x00011B94
			internal UnsafeNativeMethods.IMsoComponentManager ComponentManager
			{
				get
				{
					if (this.componentManager == null)
					{
						if (this.fetchingComponentManager)
						{
							return null;
						}
						this.fetchingComponentManager = true;
						try
						{
							UnsafeNativeMethods.IMsoComponentManager msoComponentManager = null;
							Application.OleRequired();
							IntPtr intPtr = (IntPtr)0;
							if (NativeMethods.Succeeded(UnsafeNativeMethods.CoRegisterMessageFilter(NativeMethods.NullHandleRef, ref intPtr)) && intPtr != (IntPtr)0)
							{
								IntPtr intPtr2 = (IntPtr)0;
								UnsafeNativeMethods.CoRegisterMessageFilter(new HandleRef(null, intPtr), ref intPtr2);
								object obj = Marshal.GetObjectForIUnknown(intPtr);
								Marshal.Release(intPtr);
								UnsafeNativeMethods.IOleServiceProvider oleServiceProvider = obj as UnsafeNativeMethods.IOleServiceProvider;
								if (oleServiceProvider != null)
								{
									try
									{
										IntPtr zero = IntPtr.Zero;
										Guid guid = new Guid("000C060B-0000-0000-C000-000000000046");
										Guid guid2 = new Guid("{000C0601-0000-0000-C000-000000000046}");
										int num = oleServiceProvider.QueryService(ref guid, ref guid2, out zero);
										if (NativeMethods.Succeeded(num) && zero != IntPtr.Zero)
										{
											IntPtr intPtr3;
											try
											{
												Guid guid3 = typeof(UnsafeNativeMethods.IMsoComponentManager).GUID;
												num = Marshal.QueryInterface(zero, ref guid3, out intPtr3);
											}
											finally
											{
												Marshal.Release(zero);
											}
											if (NativeMethods.Succeeded(num) && intPtr3 != IntPtr.Zero)
											{
												try
												{
													msoComponentManager = ComponentManagerBroker.GetComponentManager(intPtr3);
												}
												finally
												{
													Marshal.Release(intPtr3);
												}
											}
											if (msoComponentManager != null)
											{
												if (intPtr == zero)
												{
													obj = null;
												}
												this.externalComponentManager = true;
												AppDomain.CurrentDomain.DomainUnload += this.OnDomainUnload;
												AppDomain.CurrentDomain.ProcessExit += this.OnDomainUnload;
											}
										}
									}
									catch
									{
									}
								}
								if (obj != null && Marshal.IsComObject(obj))
								{
									Marshal.ReleaseComObject(obj);
								}
							}
							if (msoComponentManager == null)
							{
								msoComponentManager = new Application.ComponentManager();
								this.externalComponentManager = false;
							}
							if (msoComponentManager != null && this.componentID == -1)
							{
								bool flag = msoComponentManager.FRegisterComponent(this, new NativeMethods.MSOCRINFOSTRUCT
								{
									cbSize = Marshal.SizeOf(typeof(NativeMethods.MSOCRINFOSTRUCT)),
									uIdleTimeInterval = 0,
									grfcrf = 9,
									grfcadvf = 1
								}, out this.componentID);
								if (flag && !(msoComponentManager is Application.ComponentManager))
								{
									this.messageLoopCount++;
								}
								this.componentManager = msoComponentManager;
							}
						}
						finally
						{
							this.fetchingComponentManager = false;
						}
					}
					return this.componentManager;
				}
			}

			// Token: 0x170001E9 RID: 489
			// (get) Token: 0x06001303 RID: 4867 RVA: 0x00012E20 File Offset: 0x00011E20
			internal bool CustomThreadExceptionHandlerAttached
			{
				get
				{
					return this.threadExceptionHandler != null;
				}
			}

			// Token: 0x170001EA RID: 490
			// (get) Token: 0x06001304 RID: 4868 RVA: 0x00012E30 File Offset: 0x00011E30
			internal Application.ParkingWindow ParkingWindow
			{
				get
				{
					Application.ParkingWindow parkingWindow;
					lock (this)
					{
						if (this.parkingWindow == null)
						{
							IntSecurity.ManipulateWndProcAndHandles.Assert();
							try
							{
								this.parkingWindow = new Application.ParkingWindow();
							}
							finally
							{
								CodeAccessPermission.RevertAssert();
							}
						}
						parkingWindow = this.parkingWindow;
					}
					return parkingWindow;
				}
			}

			// Token: 0x170001EB RID: 491
			// (get) Token: 0x06001305 RID: 4869 RVA: 0x00012E98 File Offset: 0x00011E98
			// (set) Token: 0x06001306 RID: 4870 RVA: 0x00012EC1 File Offset: 0x00011EC1
			internal Control ActivatingControl
			{
				get
				{
					if (this.activatingControlRef != null && this.activatingControlRef.IsAlive)
					{
						return this.activatingControlRef.Target as Control;
					}
					return null;
				}
				set
				{
					if (value != null)
					{
						this.activatingControlRef = new WeakReference(value);
						return;
					}
					this.activatingControlRef = null;
				}
			}

			// Token: 0x170001EC RID: 492
			// (get) Token: 0x06001307 RID: 4871 RVA: 0x00012EDC File Offset: 0x00011EDC
			internal Control MarshalingControl
			{
				get
				{
					Control control;
					lock (this)
					{
						if (this.marshalingControl == null)
						{
							this.marshalingControl = new Application.MarshalingControl();
						}
						control = this.marshalingControl;
					}
					return control;
				}
			}

			// Token: 0x06001308 RID: 4872 RVA: 0x00012F28 File Offset: 0x00011F28
			internal void AddMessageFilter(IMessageFilter f)
			{
				if (this.messageFilters == null)
				{
					this.messageFilters = new ArrayList();
				}
				if (f != null)
				{
					this.SetState(16, false);
					if (this.messageFilters.Count > 0 && f is IMessageModifyAndFilter)
					{
						this.messageFilters.Insert(0, f);
						return;
					}
					this.messageFilters.Add(f);
				}
			}

			// Token: 0x06001309 RID: 4873 RVA: 0x00012F88 File Offset: 0x00011F88
			internal void BeginModalMessageLoop(ApplicationContext context)
			{
				bool flag = this.ourModalLoop;
				this.ourModalLoop = true;
				try
				{
					UnsafeNativeMethods.IMsoComponentManager msoComponentManager = this.ComponentManager;
					if (msoComponentManager != null)
					{
						msoComponentManager.OnComponentEnterState(this.componentID, 1, 0, 0, 0, 0);
					}
				}
				finally
				{
					this.ourModalLoop = flag;
				}
				this.DisableWindowsForModalLoop(false, context);
				this.modalCount++;
				if (this.enterModalHandler != null && this.modalCount == 1)
				{
					this.enterModalHandler(Thread.CurrentThread, EventArgs.Empty);
				}
			}

			// Token: 0x0600130A RID: 4874 RVA: 0x00013014 File Offset: 0x00012014
			internal void DisableWindowsForModalLoop(bool onlyWinForms, ApplicationContext context)
			{
				Application.ThreadWindows threadWindows = this.threadWindows;
				this.threadWindows = new Application.ThreadWindows(onlyWinForms);
				this.threadWindows.Enable(false);
				this.threadWindows.previousThreadWindows = threadWindows;
				Application.ModalApplicationContext modalApplicationContext = context as Application.ModalApplicationContext;
				if (modalApplicationContext != null)
				{
					modalApplicationContext.DisableThreadWindows(true, onlyWinForms);
				}
			}

			// Token: 0x0600130B RID: 4875 RVA: 0x00013060 File Offset: 0x00012060
			internal void Dispose(bool postQuit)
			{
				lock (this)
				{
					try
					{
						if (this.disposeCount++ == 0)
						{
							if (this.messageLoopCount > 0 && postQuit)
							{
								this.PostQuit();
							}
							else
							{
								bool flag = SafeNativeMethods.GetCurrentThreadId() == this.id;
								try
								{
									if (flag)
									{
										if (this.componentManager != null)
										{
											this.RevokeComponent();
										}
										this.DisposeThreadWindows();
										try
										{
											Application.RaiseThreadExit();
										}
										finally
										{
											if (this.GetState(1) && !this.GetState(2))
											{
												this.SetState(1, false);
												UnsafeNativeMethods.OleUninitialize();
											}
										}
									}
								}
								finally
								{
									if (this.handle != IntPtr.Zero)
									{
										UnsafeNativeMethods.CloseHandle(new HandleRef(this, this.handle));
										this.handle = IntPtr.Zero;
									}
									try
									{
										if (Application.ThreadContext.totalMessageLoopCount == 0)
										{
											Application.RaiseExit();
										}
									}
									finally
									{
										Application.ThreadContext.contextHash.Remove(this.id);
										if (Application.ThreadContext.currentThreadContext == this)
										{
											Application.ThreadContext.currentThreadContext = null;
										}
									}
								}
							}
							GC.SuppressFinalize(this);
						}
					}
					finally
					{
						this.disposeCount--;
					}
				}
			}

			// Token: 0x0600130C RID: 4876 RVA: 0x000131B4 File Offset: 0x000121B4
			private void DisposeParkingWindow()
			{
				if (this.parkingWindow != null && this.parkingWindow.IsHandleCreated)
				{
					int num;
					int windowThreadProcessId = SafeNativeMethods.GetWindowThreadProcessId(new HandleRef(this.parkingWindow, this.parkingWindow.Handle), out num);
					int currentThreadId = SafeNativeMethods.GetCurrentThreadId();
					if (windowThreadProcessId == currentThreadId)
					{
						this.parkingWindow.Destroy();
						return;
					}
					this.parkingWindow = null;
				}
			}

			// Token: 0x0600130D RID: 4877 RVA: 0x00013214 File Offset: 0x00012214
			internal void DisposeThreadWindows()
			{
				try
				{
					if (this.applicationContext != null)
					{
						this.applicationContext.Dispose();
						this.applicationContext = null;
					}
					Application.ThreadWindows threadWindows = new Application.ThreadWindows(true);
					threadWindows.Dispose();
					this.DisposeParkingWindow();
				}
				catch
				{
				}
			}

			// Token: 0x0600130E RID: 4878 RVA: 0x00013264 File Offset: 0x00012264
			internal void EnableWindowsForModalLoop(bool onlyWinForms, ApplicationContext context)
			{
				if (this.threadWindows != null)
				{
					this.threadWindows.Enable(true);
					this.threadWindows = this.threadWindows.previousThreadWindows;
				}
				Application.ModalApplicationContext modalApplicationContext = context as Application.ModalApplicationContext;
				if (modalApplicationContext != null)
				{
					modalApplicationContext.DisableThreadWindows(false, onlyWinForms);
				}
			}

			// Token: 0x0600130F RID: 4879 RVA: 0x000132A8 File Offset: 0x000122A8
			internal void EndModalMessageLoop(ApplicationContext context)
			{
				this.EnableWindowsForModalLoop(false, context);
				bool flag = this.ourModalLoop;
				this.ourModalLoop = true;
				try
				{
					UnsafeNativeMethods.IMsoComponentManager msoComponentManager = this.ComponentManager;
					if (msoComponentManager != null)
					{
						msoComponentManager.FOnComponentExitState(this.componentID, 1, 0, 0, 0);
					}
				}
				finally
				{
					this.ourModalLoop = flag;
				}
				this.modalCount--;
				if (this.leaveModalHandler != null && this.modalCount == 0)
				{
					this.leaveModalHandler(Thread.CurrentThread, EventArgs.Empty);
				}
			}

			// Token: 0x06001310 RID: 4880 RVA: 0x00013334 File Offset: 0x00012334
			internal static void ExitApplication()
			{
				Application.ThreadContext.ExitCommon(true);
			}

			// Token: 0x06001311 RID: 4881 RVA: 0x0001333C File Offset: 0x0001233C
			private static void ExitCommon(bool disposing)
			{
				lock (Application.ThreadContext.tcInternalSyncObject)
				{
					if (Application.ThreadContext.contextHash != null)
					{
						Application.ThreadContext[] array = new Application.ThreadContext[Application.ThreadContext.contextHash.Values.Count];
						Application.ThreadContext.contextHash.Values.CopyTo(array, 0);
						for (int i = 0; i < array.Length; i++)
						{
							if (array[i].ApplicationContext != null)
							{
								array[i].ApplicationContext.ExitThread();
							}
							else
							{
								array[i].Dispose(disposing);
							}
						}
					}
				}
			}

			// Token: 0x06001312 RID: 4882 RVA: 0x000133CC File Offset: 0x000123CC
			internal static void ExitDomain()
			{
				Application.ThreadContext.ExitCommon(false);
			}

			// Token: 0x06001313 RID: 4883 RVA: 0x000133D4 File Offset: 0x000123D4
			~ThreadContext()
			{
				if (this.handle != IntPtr.Zero)
				{
					UnsafeNativeMethods.CloseHandle(new HandleRef(this, this.handle));
					this.handle = IntPtr.Zero;
				}
			}

			// Token: 0x06001314 RID: 4884 RVA: 0x0001342C File Offset: 0x0001242C
			internal void FormActivated(bool activate)
			{
				if (activate)
				{
					UnsafeNativeMethods.IMsoComponentManager msoComponentManager = this.ComponentManager;
					if (msoComponentManager != null && !(msoComponentManager is Application.ComponentManager))
					{
						msoComponentManager.FOnComponentActivate(this.componentID);
					}
				}
			}

			// Token: 0x06001315 RID: 4885 RVA: 0x0001345C File Offset: 0x0001245C
			internal void TrackInput(bool track)
			{
				if (track != this.GetState(32))
				{
					UnsafeNativeMethods.IMsoComponentManager msoComponentManager = this.ComponentManager;
					if (msoComponentManager != null && !(msoComponentManager is Application.ComponentManager))
					{
						msoComponentManager.FSetTrackingComponent(this.componentID, track);
						this.SetState(32, track);
					}
				}
			}

			// Token: 0x06001316 RID: 4886 RVA: 0x000134A0 File Offset: 0x000124A0
			internal static Application.ThreadContext FromCurrent()
			{
				Application.ThreadContext threadContext = Application.ThreadContext.currentThreadContext;
				if (threadContext == null)
				{
					threadContext = new Application.ThreadContext();
				}
				return threadContext;
			}

			// Token: 0x06001317 RID: 4887 RVA: 0x000134C0 File Offset: 0x000124C0
			internal static Application.ThreadContext FromId(int id)
			{
				Application.ThreadContext threadContext = (Application.ThreadContext)Application.ThreadContext.contextHash[id];
				if (threadContext == null && id == SafeNativeMethods.GetCurrentThreadId())
				{
					threadContext = new Application.ThreadContext();
				}
				return threadContext;
			}

			// Token: 0x06001318 RID: 4888 RVA: 0x000134F5 File Offset: 0x000124F5
			internal bool GetAllowQuit()
			{
				return Application.ThreadContext.totalMessageLoopCount > 0 && Application.ThreadContext.baseLoopReason == -1;
			}

			// Token: 0x06001319 RID: 4889 RVA: 0x00013509 File Offset: 0x00012509
			internal IntPtr GetHandle()
			{
				return this.handle;
			}

			// Token: 0x0600131A RID: 4890 RVA: 0x00013511 File Offset: 0x00012511
			internal int GetId()
			{
				return this.id;
			}

			// Token: 0x0600131B RID: 4891 RVA: 0x00013519 File Offset: 0x00012519
			internal CultureInfo GetCulture()
			{
				if (this.culture == null || this.culture.LCID != SafeNativeMethods.GetThreadLocale())
				{
					this.culture = new CultureInfo(SafeNativeMethods.GetThreadLocale());
				}
				return this.culture;
			}

			// Token: 0x0600131C RID: 4892 RVA: 0x0001354B File Offset: 0x0001254B
			internal bool GetMessageLoop()
			{
				return this.GetMessageLoop(false);
			}

			// Token: 0x0600131D RID: 4893 RVA: 0x00013554 File Offset: 0x00012554
			internal bool GetMessageLoop(bool mustBeActive)
			{
				if (this.messageLoopCount > ((mustBeActive && this.externalComponentManager) ? 1 : 0))
				{
					return true;
				}
				if (this.ComponentManager != null && this.externalComponentManager)
				{
					if (!mustBeActive)
					{
						return true;
					}
					UnsafeNativeMethods.IMsoComponent[] array = new UnsafeNativeMethods.IMsoComponent[1];
					if (this.ComponentManager.FGetActiveComponent(0, array, null, 0) && array[0] == this)
					{
						return true;
					}
				}
				Application.MessageLoopCallback messageLoopCallback = this.messageLoopCallback;
				return messageLoopCallback != null && messageLoopCallback();
			}

			// Token: 0x0600131E RID: 4894 RVA: 0x000135C1 File Offset: 0x000125C1
			private bool GetState(int bit)
			{
				return (this.threadState & bit) != 0;
			}

			// Token: 0x0600131F RID: 4895 RVA: 0x000135D1 File Offset: 0x000125D1
			public override object InitializeLifetimeService()
			{
				return null;
			}

			// Token: 0x06001320 RID: 4896 RVA: 0x000135D4 File Offset: 0x000125D4
			internal bool IsValidComponentId()
			{
				return this.componentID != -1;
			}

			// Token: 0x06001321 RID: 4897 RVA: 0x000135E4 File Offset: 0x000125E4
			internal ApartmentState OleRequired()
			{
				Thread currentThread = Thread.CurrentThread;
				if (!this.GetState(1))
				{
					int num = UnsafeNativeMethods.OleInitialize();
					this.SetState(1, true);
					if (num == -2147417850)
					{
						this.SetState(2, true);
					}
				}
				if (this.GetState(2))
				{
					return ApartmentState.MTA;
				}
				return ApartmentState.STA;
			}

			// Token: 0x06001322 RID: 4898 RVA: 0x0001362A File Offset: 0x0001262A
			private void OnAppThreadExit(object sender, EventArgs e)
			{
				this.Dispose(true);
			}

			// Token: 0x06001323 RID: 4899 RVA: 0x00013633 File Offset: 0x00012633
			[PrePrepareMethod]
			private void OnDomainUnload(object sender, EventArgs e)
			{
				this.RevokeComponent();
				Application.ThreadContext.ExitDomain();
			}

			// Token: 0x06001324 RID: 4900 RVA: 0x00013640 File Offset: 0x00012640
			internal void OnThreadException(Exception t)
			{
				if (this.GetState(4))
				{
					return;
				}
				this.SetState(4, true);
				try
				{
					if (this.threadExceptionHandler != null)
					{
						this.threadExceptionHandler(Thread.CurrentThread, new ThreadExceptionEventArgs(t));
					}
					else if (SystemInformation.UserInteractive)
					{
						ThreadExceptionDialog threadExceptionDialog = new ThreadExceptionDialog(t);
						DialogResult dialogResult = DialogResult.OK;
						IntSecurity.ModifyFocus.Assert();
						try
						{
							dialogResult = threadExceptionDialog.ShowDialog();
						}
						finally
						{
							CodeAccessPermission.RevertAssert();
							threadExceptionDialog.Dispose();
						}
						DialogResult dialogResult2 = dialogResult;
						if (dialogResult2 != DialogResult.Abort)
						{
							if (dialogResult2 == DialogResult.Yes)
							{
								WarningException ex = t as WarningException;
								if (ex != null)
								{
									Help.ShowHelp(null, ex.HelpUrl, ex.HelpTopic);
								}
							}
						}
						else
						{
							Application.ExitInternal();
							new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
							Environment.Exit(0);
						}
					}
				}
				finally
				{
					this.SetState(4, false);
				}
			}

			// Token: 0x06001325 RID: 4901 RVA: 0x00013718 File Offset: 0x00012718
			internal void PostQuit()
			{
				UnsafeNativeMethods.PostThreadMessage(this.id, 18, IntPtr.Zero, IntPtr.Zero);
				this.SetState(8, true);
			}

			// Token: 0x06001326 RID: 4902 RVA: 0x0001373A File Offset: 0x0001273A
			internal void RegisterMessageLoop(Application.MessageLoopCallback callback)
			{
				this.messageLoopCallback = callback;
			}

			// Token: 0x06001327 RID: 4903 RVA: 0x00013743 File Offset: 0x00012743
			internal void RemoveMessageFilter(IMessageFilter f)
			{
				if (this.messageFilters != null)
				{
					this.SetState(16, false);
					this.messageFilters.Remove(f);
				}
			}

			// Token: 0x06001328 RID: 4904 RVA: 0x00013764 File Offset: 0x00012764
			internal void RunMessageLoop(int reason, ApplicationContext context)
			{
				IntPtr intPtr = IntPtr.Zero;
				if (Application.useVisualStyles)
				{
					intPtr = UnsafeNativeMethods.ThemingScope.Activate();
				}
				try
				{
					this.RunMessageLoopInner(reason, context);
				}
				finally
				{
					UnsafeNativeMethods.ThemingScope.Deactivate(intPtr);
				}
			}

			// Token: 0x06001329 RID: 4905 RVA: 0x000137A8 File Offset: 0x000127A8
			private void RunMessageLoopInner(int reason, ApplicationContext context)
			{
				if (reason == 4 && !SystemInformation.UserInteractive)
				{
					throw new InvalidOperationException(SR.GetString("CantShowModalOnNonInteractive"));
				}
				if (reason == -1)
				{
					this.SetState(8, false);
				}
				if (Application.ThreadContext.totalMessageLoopCount++ == 0)
				{
					Application.ThreadContext.baseLoopReason = reason;
				}
				this.messageLoopCount++;
				if (reason == -1)
				{
					if (this.messageLoopCount != 1)
					{
						throw new InvalidOperationException(SR.GetString("CantNestMessageLoops"));
					}
					this.applicationContext = context;
					this.applicationContext.ThreadExit += this.OnAppThreadExit;
					if (this.applicationContext.MainForm != null)
					{
						this.applicationContext.MainForm.Visible = true;
					}
				}
				Form form = this.currentForm;
				if (context != null)
				{
					this.currentForm = context.MainForm;
				}
				bool flag = false;
				bool flag2 = false;
				HandleRef handleRef = new HandleRef(null, IntPtr.Zero);
				if (reason == -2)
				{
					flag2 = true;
				}
				if (reason == 4 || reason == 5)
				{
					flag = true;
					bool flag3 = this.currentForm != null && this.currentForm.Enabled;
					this.BeginModalMessageLoop(context);
					handleRef = new HandleRef(null, UnsafeNativeMethods.GetWindowLong(new HandleRef(this.currentForm, this.currentForm.Handle), -8));
					if (handleRef.Handle != IntPtr.Zero)
					{
						if (SafeNativeMethods.IsWindowEnabled(handleRef))
						{
							SafeNativeMethods.EnableWindow(handleRef, false);
						}
						else
						{
							handleRef = new HandleRef(null, IntPtr.Zero);
						}
					}
					if (this.currentForm != null && this.currentForm.IsHandleCreated && SafeNativeMethods.IsWindowEnabled(new HandleRef(this.currentForm, this.currentForm.Handle)) != flag3)
					{
						SafeNativeMethods.EnableWindow(new HandleRef(this.currentForm, this.currentForm.Handle), flag3);
					}
				}
				try
				{
					if (this.messageLoopCount == 1)
					{
						WindowsFormsSynchronizationContext.InstallIfNeeded();
					}
					if (flag && this.currentForm != null)
					{
						this.currentForm.Visible = true;
					}
					if ((!flag && !flag2) || this.ComponentManager is Application.ComponentManager)
					{
						this.ComponentManager.FPushMessageLoop(this.componentID, reason, 0);
					}
					else if (reason == 2 || reason == -2)
					{
						this.LocalModalMessageLoop(null);
					}
					else
					{
						this.LocalModalMessageLoop(this.currentForm);
					}
				}
				finally
				{
					if (flag)
					{
						this.EndModalMessageLoop(context);
						if (handleRef.Handle != IntPtr.Zero)
						{
							SafeNativeMethods.EnableWindow(handleRef, true);
						}
					}
					this.currentForm = form;
					Application.ThreadContext.totalMessageLoopCount--;
					this.messageLoopCount--;
					if (this.messageLoopCount == 0)
					{
						WindowsFormsSynchronizationContext.Uninstall(false);
					}
					if (reason == -1)
					{
						this.Dispose(true);
					}
					else if (this.messageLoopCount == 0 && this.componentManager != null)
					{
						this.RevokeComponent();
					}
				}
			}

			// Token: 0x0600132A RID: 4906 RVA: 0x00013A54 File Offset: 0x00012A54
			private bool LocalModalMessageLoop(Form form)
			{
				bool flag4;
				try
				{
					NativeMethods.MSG msg = default(NativeMethods.MSG);
					bool flag = true;
					while (flag)
					{
						bool flag2 = UnsafeNativeMethods.PeekMessage(ref msg, NativeMethods.NullHandleRef, 0, 0, 0);
						if (flag2)
						{
							bool flag3;
							if (msg.hwnd != IntPtr.Zero && SafeNativeMethods.IsWindowUnicode(new HandleRef(null, msg.hwnd)))
							{
								flag3 = true;
								if (!UnsafeNativeMethods.GetMessageW(ref msg, NativeMethods.NullHandleRef, 0, 0))
								{
									continue;
								}
							}
							else
							{
								flag3 = false;
								if (!UnsafeNativeMethods.GetMessageA(ref msg, NativeMethods.NullHandleRef, 0, 0))
								{
									continue;
								}
							}
							if (!this.PreTranslateMessage(ref msg))
							{
								UnsafeNativeMethods.TranslateMessage(ref msg);
								if (flag3)
								{
									UnsafeNativeMethods.DispatchMessageW(ref msg);
								}
								else
								{
									UnsafeNativeMethods.DispatchMessageA(ref msg);
								}
							}
							if (form != null)
							{
								flag = !form.CheckCloseDialog(false);
							}
						}
						else
						{
							if (form == null)
							{
								break;
							}
							if (!UnsafeNativeMethods.PeekMessage(ref msg, NativeMethods.NullHandleRef, 0, 0, 0))
							{
								UnsafeNativeMethods.WaitMessage();
							}
						}
					}
					flag4 = flag;
				}
				catch
				{
					flag4 = false;
				}
				return flag4;
			}

			// Token: 0x0600132B RID: 4907 RVA: 0x00013B48 File Offset: 0x00012B48
			internal bool ProcessFilters(ref NativeMethods.MSG msg, out bool modified)
			{
				bool flag = false;
				modified = false;
				if (this.messageFilters != null && !this.GetState(16))
				{
					if (this.messageFilters.Count > 0)
					{
						this.messageFilterSnapshot = new IMessageFilter[this.messageFilters.Count];
						this.messageFilters.CopyTo(this.messageFilterSnapshot);
					}
					else
					{
						this.messageFilterSnapshot = null;
					}
					this.SetState(16, true);
				}
				if (this.messageFilterSnapshot != null)
				{
					int num = this.messageFilterSnapshot.Length;
					Message message = Message.Create(msg.hwnd, msg.message, msg.wParam, msg.lParam);
					for (int i = 0; i < num; i++)
					{
						IMessageFilter messageFilter = this.messageFilterSnapshot[i];
						bool flag2 = messageFilter.PreFilterMessage(ref message);
						if (messageFilter is IMessageModifyAndFilter)
						{
							msg.hwnd = message.HWnd;
							msg.message = message.Msg;
							msg.wParam = message.WParam;
							msg.lParam = message.LParam;
							modified = true;
						}
						if (flag2)
						{
							flag = true;
							break;
						}
					}
				}
				return flag;
			}

			// Token: 0x0600132C RID: 4908 RVA: 0x00013C54 File Offset: 0x00012C54
			internal bool PreTranslateMessage(ref NativeMethods.MSG msg)
			{
				bool flag = false;
				if (this.ProcessFilters(ref msg, out flag))
				{
					return true;
				}
				if (msg.message >= 256 && msg.message <= 264)
				{
					if (msg.message == 258)
					{
						int num = 21364736;
						if ((int)msg.wParam == 3 && ((int)msg.lParam & num) == num && Debugger.IsAttached)
						{
							Debugger.Break();
						}
					}
					Control control = Control.FromChildHandleInternal(msg.hwnd);
					bool flag2 = false;
					Message message = Message.Create(msg.hwnd, msg.message, msg.wParam, msg.lParam);
					if (control != null)
					{
						if (NativeWindow.WndProcShouldBeDebuggable)
						{
							if (Control.PreProcessControlMessageInternal(control, ref message) == PreProcessControlState.MessageProcessed)
							{
								flag2 = true;
								goto IL_00FF;
							}
							goto IL_00FF;
						}
						else
						{
							try
							{
								if (Control.PreProcessControlMessageInternal(control, ref message) == PreProcessControlState.MessageProcessed)
								{
									flag2 = true;
								}
								goto IL_00FF;
							}
							catch (Exception ex)
							{
								this.OnThreadException(ex);
								goto IL_00FF;
							}
						}
					}
					IntPtr ancestor = UnsafeNativeMethods.GetAncestor(new HandleRef(null, msg.hwnd), 2);
					if (ancestor != IntPtr.Zero && UnsafeNativeMethods.IsDialogMessage(new HandleRef(null, ancestor), ref msg))
					{
						return true;
					}
					IL_00FF:
					msg.wParam = message.WParam;
					msg.lParam = message.LParam;
					if (flag2)
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x0600132D RID: 4909 RVA: 0x00013D90 File Offset: 0x00012D90
			private void RevokeComponent()
			{
				if (this.componentManager != null && this.componentID != -1)
				{
					int num = this.componentID;
					UnsafeNativeMethods.IMsoComponentManager msoComponentManager = this.componentManager;
					try
					{
						msoComponentManager.FRevokeComponent(num);
						if (Marshal.IsComObject(msoComponentManager))
						{
							Marshal.ReleaseComObject(msoComponentManager);
						}
					}
					finally
					{
						this.componentManager = null;
						this.componentID = -1;
					}
				}
			}

			// Token: 0x0600132E RID: 4910 RVA: 0x00013DF4 File Offset: 0x00012DF4
			internal void SetCulture(CultureInfo culture)
			{
				if (culture != null && culture.LCID != SafeNativeMethods.GetThreadLocale())
				{
					SafeNativeMethods.SetThreadLocale(culture.LCID);
				}
			}

			// Token: 0x0600132F RID: 4911 RVA: 0x00013E12 File Offset: 0x00012E12
			private void SetState(int bit, bool value)
			{
				if (value)
				{
					this.threadState |= bit;
					return;
				}
				this.threadState &= ~bit;
			}

			// Token: 0x06001330 RID: 4912 RVA: 0x00013E35 File Offset: 0x00012E35
			bool UnsafeNativeMethods.IMsoComponent.FDebugMessage(IntPtr hInst, int msg, IntPtr wparam, IntPtr lparam)
			{
				return false;
			}

			// Token: 0x06001331 RID: 4913 RVA: 0x00013E38 File Offset: 0x00012E38
			bool UnsafeNativeMethods.IMsoComponent.FPreTranslateMessage(ref NativeMethods.MSG msg)
			{
				return this.PreTranslateMessage(ref msg);
			}

			// Token: 0x06001332 RID: 4914 RVA: 0x00013E41 File Offset: 0x00012E41
			void UnsafeNativeMethods.IMsoComponent.OnEnterState(int uStateID, bool fEnter)
			{
				if (this.ourModalLoop)
				{
					return;
				}
				if (uStateID == 1)
				{
					if (fEnter)
					{
						this.DisableWindowsForModalLoop(true, null);
						return;
					}
					this.EnableWindowsForModalLoop(true, null);
				}
			}

			// Token: 0x06001333 RID: 4915 RVA: 0x00013E64 File Offset: 0x00012E64
			void UnsafeNativeMethods.IMsoComponent.OnAppActivate(bool fActive, int dwOtherThreadID)
			{
			}

			// Token: 0x06001334 RID: 4916 RVA: 0x00013E66 File Offset: 0x00012E66
			void UnsafeNativeMethods.IMsoComponent.OnLoseActivation()
			{
			}

			// Token: 0x06001335 RID: 4917 RVA: 0x00013E68 File Offset: 0x00012E68
			void UnsafeNativeMethods.IMsoComponent.OnActivationChange(UnsafeNativeMethods.IMsoComponent component, bool fSameComponent, int pcrinfo, bool fHostIsActivating, int pchostinfo, int dwReserved)
			{
			}

			// Token: 0x06001336 RID: 4918 RVA: 0x00013E6A File Offset: 0x00012E6A
			bool UnsafeNativeMethods.IMsoComponent.FDoIdle(int grfidlef)
			{
				if (this.idleHandler != null)
				{
					this.idleHandler(Thread.CurrentThread, EventArgs.Empty);
				}
				return false;
			}

			// Token: 0x06001337 RID: 4919 RVA: 0x00013E8C File Offset: 0x00012E8C
			bool UnsafeNativeMethods.IMsoComponent.FContinueMessageLoop(int reason, int pvLoopData, NativeMethods.MSG[] msgPeeked)
			{
				bool flag = true;
				if (msgPeeked == null && this.GetState(8))
				{
					flag = false;
				}
				else
				{
					switch (reason)
					{
					case -2:
					case 2:
						if (!UnsafeNativeMethods.PeekMessage(ref this.tempMsg, NativeMethods.NullHandleRef, 0, 0, 0))
						{
							flag = false;
						}
						break;
					case 1:
					{
						int num;
						SafeNativeMethods.GetWindowThreadProcessId(new HandleRef(null, UnsafeNativeMethods.GetActiveWindow()), out num);
						if (num == SafeNativeMethods.GetCurrentProcessId())
						{
							flag = false;
						}
						break;
					}
					case 4:
					case 5:
						if (this.currentForm == null || this.currentForm.CheckCloseDialog(false))
						{
							flag = false;
						}
						break;
					}
				}
				return flag;
			}

			// Token: 0x06001338 RID: 4920 RVA: 0x00013F29 File Offset: 0x00012F29
			bool UnsafeNativeMethods.IMsoComponent.FQueryTerminate(bool fPromptUser)
			{
				return true;
			}

			// Token: 0x06001339 RID: 4921 RVA: 0x00013F2C File Offset: 0x00012F2C
			void UnsafeNativeMethods.IMsoComponent.Terminate()
			{
				if (this.messageLoopCount > 0 && !(this.ComponentManager is Application.ComponentManager))
				{
					this.messageLoopCount--;
				}
				this.Dispose(false);
			}

			// Token: 0x0600133A RID: 4922 RVA: 0x00013F59 File Offset: 0x00012F59
			IntPtr UnsafeNativeMethods.IMsoComponent.HwndGetWindow(int dwWhich, int dwReserved)
			{
				return IntPtr.Zero;
			}

			// Token: 0x04001048 RID: 4168
			private const int STATE_OLEINITIALIZED = 1;

			// Token: 0x04001049 RID: 4169
			private const int STATE_EXTERNALOLEINIT = 2;

			// Token: 0x0400104A RID: 4170
			private const int STATE_INTHREADEXCEPTION = 4;

			// Token: 0x0400104B RID: 4171
			private const int STATE_POSTEDQUIT = 8;

			// Token: 0x0400104C RID: 4172
			private const int STATE_FILTERSNAPSHOTVALID = 16;

			// Token: 0x0400104D RID: 4173
			private const int STATE_TRACKINGCOMPONENT = 32;

			// Token: 0x0400104E RID: 4174
			private const int INVALID_ID = -1;

			// Token: 0x0400104F RID: 4175
			private static Hashtable contextHash = new Hashtable();

			// Token: 0x04001050 RID: 4176
			private static object tcInternalSyncObject = new object();

			// Token: 0x04001051 RID: 4177
			private static int totalMessageLoopCount;

			// Token: 0x04001052 RID: 4178
			private static int baseLoopReason;

			// Token: 0x04001053 RID: 4179
			[ThreadStatic]
			private static Application.ThreadContext currentThreadContext;

			// Token: 0x04001054 RID: 4180
			internal ThreadExceptionEventHandler threadExceptionHandler;

			// Token: 0x04001055 RID: 4181
			internal EventHandler idleHandler;

			// Token: 0x04001056 RID: 4182
			internal EventHandler enterModalHandler;

			// Token: 0x04001057 RID: 4183
			internal EventHandler leaveModalHandler;

			// Token: 0x04001058 RID: 4184
			private ApplicationContext applicationContext;

			// Token: 0x04001059 RID: 4185
			private Application.ParkingWindow parkingWindow;

			// Token: 0x0400105A RID: 4186
			private Control marshalingControl;

			// Token: 0x0400105B RID: 4187
			private CultureInfo culture;

			// Token: 0x0400105C RID: 4188
			private ArrayList messageFilters;

			// Token: 0x0400105D RID: 4189
			private IMessageFilter[] messageFilterSnapshot;

			// Token: 0x0400105E RID: 4190
			private IntPtr handle;

			// Token: 0x0400105F RID: 4191
			private int id;

			// Token: 0x04001060 RID: 4192
			private int messageLoopCount;

			// Token: 0x04001061 RID: 4193
			private int threadState;

			// Token: 0x04001062 RID: 4194
			private int modalCount;

			// Token: 0x04001063 RID: 4195
			private WeakReference activatingControlRef;

			// Token: 0x04001064 RID: 4196
			private UnsafeNativeMethods.IMsoComponentManager componentManager;

			// Token: 0x04001065 RID: 4197
			private bool externalComponentManager;

			// Token: 0x04001066 RID: 4198
			private bool fetchingComponentManager;

			// Token: 0x04001067 RID: 4199
			private int componentID = -1;

			// Token: 0x04001068 RID: 4200
			private Form currentForm;

			// Token: 0x04001069 RID: 4201
			private Application.ThreadWindows threadWindows;

			// Token: 0x0400106A RID: 4202
			private NativeMethods.MSG tempMsg = default(NativeMethods.MSG);

			// Token: 0x0400106B RID: 4203
			private int disposeCount;

			// Token: 0x0400106C RID: 4204
			private bool ourModalLoop;

			// Token: 0x0400106D RID: 4205
			private Application.MessageLoopCallback messageLoopCallback;
		}

		// Token: 0x0200020B RID: 523
		internal sealed class MarshalingControl : Control
		{
			// Token: 0x060017D8 RID: 6104 RVA: 0x000281CA File Offset: 0x000271CA
			internal MarshalingControl()
				: base(false)
			{
				base.Visible = false;
				base.SetState2(8, false);
				base.SetTopLevel(true);
				base.CreateControl();
				this.CreateHandle();
			}

			// Token: 0x170002D1 RID: 721
			// (get) Token: 0x060017D9 RID: 6105 RVA: 0x000281F8 File Offset: 0x000271F8
			protected override CreateParams CreateParams
			{
				get
				{
					CreateParams createParams = base.CreateParams;
					if (Environment.OSVersion.Platform == PlatformID.Win32NT)
					{
						createParams.Parent = (IntPtr)NativeMethods.HWND_MESSAGE;
					}
					return createParams;
				}
			}

			// Token: 0x060017DA RID: 6106 RVA: 0x0002822A File Offset: 0x0002722A
			protected override void OnLayout(LayoutEventArgs levent)
			{
			}

			// Token: 0x060017DB RID: 6107 RVA: 0x0002822C File Offset: 0x0002722C
			protected override void OnSizeChanged(EventArgs e)
			{
			}
		}

		// Token: 0x02000211 RID: 529
		internal sealed class ParkingWindow : ContainerControl, IArrangedElement, IComponent, IDisposable
		{
			// Token: 0x06001882 RID: 6274 RVA: 0x0002B6ED File Offset: 0x0002A6ED
			public ParkingWindow()
			{
				base.SetState2(8, false);
				base.SetState(524288, true);
				this.Text = "WindowsFormsParkingWindow";
				base.Visible = false;
			}

			// Token: 0x170002F1 RID: 753
			// (get) Token: 0x06001883 RID: 6275 RVA: 0x0002B71C File Offset: 0x0002A71C
			protected override CreateParams CreateParams
			{
				get
				{
					CreateParams createParams = base.CreateParams;
					if (Environment.OSVersion.Platform == PlatformID.Win32NT)
					{
						createParams.Parent = (IntPtr)NativeMethods.HWND_MESSAGE;
					}
					return createParams;
				}
			}

			// Token: 0x06001884 RID: 6276 RVA: 0x0002B74E File Offset: 0x0002A74E
			internal override void AddReflectChild()
			{
				if (this.childCount < 0)
				{
					this.childCount = 0;
				}
				this.childCount++;
			}

			// Token: 0x06001885 RID: 6277 RVA: 0x0002B770 File Offset: 0x0002A770
			internal override void RemoveReflectChild()
			{
				this.childCount--;
				if (this.childCount < 0)
				{
					this.childCount = 0;
				}
				if (this.childCount == 0 && base.IsHandleCreated)
				{
					int num;
					int windowThreadProcessId = SafeNativeMethods.GetWindowThreadProcessId(new HandleRef(this, base.HandleInternal), out num);
					Application.ThreadContext threadContext = Application.ThreadContext.FromId(windowThreadProcessId);
					if (threadContext == null || !object.ReferenceEquals(threadContext, Application.ThreadContext.FromCurrent()))
					{
						UnsafeNativeMethods.PostMessage(new HandleRef(this, base.HandleInternal), 1025, IntPtr.Zero, IntPtr.Zero);
						return;
					}
					this.CheckDestroy();
				}
			}

			// Token: 0x06001886 RID: 6278 RVA: 0x0002B800 File Offset: 0x0002A800
			private void CheckDestroy()
			{
				if (this.childCount == 0)
				{
					IntPtr window = UnsafeNativeMethods.GetWindow(new HandleRef(this, base.Handle), 5);
					if (window == IntPtr.Zero)
					{
						this.DestroyHandle();
					}
				}
			}

			// Token: 0x06001887 RID: 6279 RVA: 0x0002B83B File Offset: 0x0002A83B
			public void Destroy()
			{
				this.DestroyHandle();
			}

			// Token: 0x06001888 RID: 6280 RVA: 0x0002B843 File Offset: 0x0002A843
			internal void ParkHandle(HandleRef handle)
			{
				if (!base.IsHandleCreated)
				{
					this.CreateHandle();
				}
				UnsafeNativeMethods.SetParent(handle, new HandleRef(this, base.Handle));
			}

			// Token: 0x06001889 RID: 6281 RVA: 0x0002B866 File Offset: 0x0002A866
			internal void UnparkHandle(HandleRef handle)
			{
				if (base.IsHandleCreated)
				{
					this.CheckDestroy();
				}
			}

			// Token: 0x0600188A RID: 6282 RVA: 0x0002B876 File Offset: 0x0002A876
			protected override void OnLayout(LayoutEventArgs levent)
			{
			}

			// Token: 0x0600188B RID: 6283 RVA: 0x0002B878 File Offset: 0x0002A878
			void IArrangedElement.PerformLayout(IArrangedElement affectedElement, string affectedProperty)
			{
			}

			// Token: 0x0600188C RID: 6284 RVA: 0x0002B87C File Offset: 0x0002A87C
			protected override void WndProc(ref Message m)
			{
				if (m.Msg != 24)
				{
					base.WndProc(ref m);
					if (m.Msg == 528)
					{
						if (NativeMethods.Util.LOWORD((int)m.WParam) == 2)
						{
							UnsafeNativeMethods.PostMessage(new HandleRef(this, base.Handle), 1025, IntPtr.Zero, IntPtr.Zero);
							return;
						}
					}
					else if (m.Msg == 1025)
					{
						this.CheckDestroy();
					}
				}
			}

			// Token: 0x040011F7 RID: 4599
			private const int WM_CHECKDESTROY = 1025;

			// Token: 0x040011F8 RID: 4600
			private int childCount;
		}

		// Token: 0x02000212 RID: 530
		private sealed class ThreadWindows
		{
			// Token: 0x0600188D RID: 6285 RVA: 0x0002B8EF File Offset: 0x0002A8EF
			internal ThreadWindows(bool onlyWinForms)
			{
				this.windows = new IntPtr[16];
				this.onlyWinForms = onlyWinForms;
				UnsafeNativeMethods.EnumThreadWindows(SafeNativeMethods.GetCurrentThreadId(), new NativeMethods.EnumThreadWindowsCallback(this.Callback), NativeMethods.NullHandleRef);
			}

			// Token: 0x0600188E RID: 6286 RVA: 0x0002B930 File Offset: 0x0002A930
			private bool Callback(IntPtr hWnd, IntPtr lparam)
			{
				if (SafeNativeMethods.IsWindowVisible(new HandleRef(null, hWnd)) && SafeNativeMethods.IsWindowEnabled(new HandleRef(null, hWnd)))
				{
					bool flag = true;
					if (this.onlyWinForms && Control.FromHandleInternal(hWnd) == null)
					{
						flag = false;
					}
					if (flag)
					{
						if (this.windowCount == this.windows.Length)
						{
							IntPtr[] array = new IntPtr[this.windowCount * 2];
							Array.Copy(this.windows, 0, array, 0, this.windowCount);
							this.windows = array;
						}
						this.windows[this.windowCount++] = hWnd;
					}
				}
				return true;
			}

			// Token: 0x0600188F RID: 6287 RVA: 0x0002B9D4 File Offset: 0x0002A9D4
			internal void Dispose()
			{
				for (int i = 0; i < this.windowCount; i++)
				{
					IntPtr intPtr = this.windows[i];
					if (UnsafeNativeMethods.IsWindow(new HandleRef(null, intPtr)))
					{
						Control control = Control.FromHandleInternal(intPtr);
						if (control != null)
						{
							control.Dispose();
						}
					}
				}
			}

			// Token: 0x06001890 RID: 6288 RVA: 0x0002BA24 File Offset: 0x0002AA24
			internal void Enable(bool state)
			{
				if (!this.onlyWinForms && !state)
				{
					this.activeHwnd = UnsafeNativeMethods.GetActiveWindow();
					Control activatingControl = Application.ThreadContext.FromCurrent().ActivatingControl;
					if (activatingControl != null)
					{
						this.focusedHwnd = activatingControl.Handle;
					}
					else
					{
						this.focusedHwnd = UnsafeNativeMethods.GetFocus();
					}
				}
				for (int i = 0; i < this.windowCount; i++)
				{
					IntPtr intPtr = this.windows[i];
					if (UnsafeNativeMethods.IsWindow(new HandleRef(null, intPtr)))
					{
						SafeNativeMethods.EnableWindow(new HandleRef(null, intPtr), state);
					}
				}
				if (!this.onlyWinForms && state)
				{
					if (this.activeHwnd != IntPtr.Zero && UnsafeNativeMethods.IsWindow(new HandleRef(null, this.activeHwnd)))
					{
						UnsafeNativeMethods.SetActiveWindow(new HandleRef(null, this.activeHwnd));
					}
					if (this.focusedHwnd != IntPtr.Zero && UnsafeNativeMethods.IsWindow(new HandleRef(null, this.focusedHwnd)))
					{
						UnsafeNativeMethods.SetFocus(new HandleRef(null, this.focusedHwnd));
					}
				}
			}

			// Token: 0x040011F9 RID: 4601
			private IntPtr[] windows;

			// Token: 0x040011FA RID: 4602
			private int windowCount;

			// Token: 0x040011FB RID: 4603
			private IntPtr activeHwnd;

			// Token: 0x040011FC RID: 4604
			private IntPtr focusedHwnd;

			// Token: 0x040011FD RID: 4605
			internal Application.ThreadWindows previousThreadWindows;

			// Token: 0x040011FE RID: 4606
			private bool onlyWinForms = true;
		}

		// Token: 0x02000214 RID: 532
		private class ModalApplicationContext : ApplicationContext
		{
			// Token: 0x060018A0 RID: 6304 RVA: 0x0002BCA2 File Offset: 0x0002ACA2
			public ModalApplicationContext(Form modalForm)
				: base(modalForm)
			{
			}

			// Token: 0x060018A1 RID: 6305 RVA: 0x0002BCAC File Offset: 0x0002ACAC
			public void DisableThreadWindows(bool disable, bool onlyWinForms)
			{
				Control control = null;
				if (base.MainForm != null && base.MainForm.IsHandleCreated)
				{
					IntPtr windowLong = UnsafeNativeMethods.GetWindowLong(new HandleRef(this, base.MainForm.Handle), -8);
					control = Control.FromHandleInternal(windowLong);
					if (control != null && control.InvokeRequired)
					{
						this.parentWindowContext = Application.GetContextForHandle(new HandleRef(this, windowLong));
					}
					else
					{
						this.parentWindowContext = null;
					}
				}
				if (this.parentWindowContext != null)
				{
					if (control == null)
					{
						control = this.parentWindowContext.ApplicationContext.MainForm;
					}
					if (disable)
					{
						control.Invoke(new Application.ModalApplicationContext.ThreadWindowCallback(this.DisableThreadWindowsCallback), new object[] { this.parentWindowContext, onlyWinForms });
						return;
					}
					control.Invoke(new Application.ModalApplicationContext.ThreadWindowCallback(this.EnableThreadWindowsCallback), new object[] { this.parentWindowContext, onlyWinForms });
				}
			}

			// Token: 0x060018A2 RID: 6306 RVA: 0x0002BD90 File Offset: 0x0002AD90
			private void DisableThreadWindowsCallback(Application.ThreadContext context, bool onlyWinForms)
			{
				context.DisableWindowsForModalLoop(onlyWinForms, this);
			}

			// Token: 0x060018A3 RID: 6307 RVA: 0x0002BD9A File Offset: 0x0002AD9A
			private void EnableThreadWindowsCallback(Application.ThreadContext context, bool onlyWinForms)
			{
				context.EnableWindowsForModalLoop(onlyWinForms, this);
			}

			// Token: 0x060018A4 RID: 6308 RVA: 0x0002BDA4 File Offset: 0x0002ADA4
			protected override void ExitThreadCore()
			{
			}

			// Token: 0x04001202 RID: 4610
			private Application.ThreadContext parentWindowContext;

			// Token: 0x02000215 RID: 533
			// (Invoke) Token: 0x060018A6 RID: 6310
			private delegate void ThreadWindowCallback(Application.ThreadContext context, bool onlyWinForms);
		}
	}
}
