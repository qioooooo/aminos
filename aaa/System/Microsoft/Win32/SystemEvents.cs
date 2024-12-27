using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;

namespace Microsoft.Win32
{
	// Token: 0x020002A4 RID: 676
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public sealed class SystemEvents
	{
		// Token: 0x06001637 RID: 5687 RVA: 0x000464E5 File Offset: 0x000454E5
		private SystemEvents()
		{
		}

		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x06001638 RID: 5688 RVA: 0x000464F0 File Offset: 0x000454F0
		private static bool UserInteractive
		{
			get
			{
				if (Environment.OSVersion.Platform == PlatformID.Win32NT)
				{
					IntPtr intPtr = IntPtr.Zero;
					intPtr = UnsafeNativeMethods.GetProcessWindowStation();
					if (intPtr != IntPtr.Zero && SystemEvents.processWinStation != intPtr)
					{
						SystemEvents.isUserInteractive = true;
						int num = 0;
						NativeMethods.USEROBJECTFLAGS userobjectflags = new NativeMethods.USEROBJECTFLAGS();
						if (UnsafeNativeMethods.GetUserObjectInformation(new HandleRef(null, intPtr), 1, userobjectflags, Marshal.SizeOf(userobjectflags), ref num) && (userobjectflags.dwFlags & 1) == 0)
						{
							SystemEvents.isUserInteractive = false;
						}
						SystemEvents.processWinStation = intPtr;
					}
				}
				else
				{
					SystemEvents.isUserInteractive = true;
				}
				return SystemEvents.isUserInteractive;
			}
		}

		// Token: 0x14000030 RID: 48
		// (add) Token: 0x06001639 RID: 5689 RVA: 0x00046579 File Offset: 0x00045579
		// (remove) Token: 0x0600163A RID: 5690 RVA: 0x00046586 File Offset: 0x00045586
		public static event EventHandler DisplaySettingsChanging
		{
			add
			{
				SystemEvents.AddEventHandler(SystemEvents.OnDisplaySettingsChangingEvent, value);
			}
			remove
			{
				SystemEvents.RemoveEventHandler(SystemEvents.OnDisplaySettingsChangingEvent, value);
			}
		}

		// Token: 0x14000031 RID: 49
		// (add) Token: 0x0600163B RID: 5691 RVA: 0x00046593 File Offset: 0x00045593
		// (remove) Token: 0x0600163C RID: 5692 RVA: 0x000465A0 File Offset: 0x000455A0
		public static event EventHandler DisplaySettingsChanged
		{
			add
			{
				SystemEvents.AddEventHandler(SystemEvents.OnDisplaySettingsChangedEvent, value);
			}
			remove
			{
				SystemEvents.RemoveEventHandler(SystemEvents.OnDisplaySettingsChangedEvent, value);
			}
		}

		// Token: 0x14000032 RID: 50
		// (add) Token: 0x0600163D RID: 5693 RVA: 0x000465AD File Offset: 0x000455AD
		// (remove) Token: 0x0600163E RID: 5694 RVA: 0x000465BA File Offset: 0x000455BA
		public static event EventHandler EventsThreadShutdown
		{
			add
			{
				SystemEvents.AddEventHandler(SystemEvents.OnEventsThreadShutdownEvent, value);
			}
			remove
			{
				SystemEvents.RemoveEventHandler(SystemEvents.OnEventsThreadShutdownEvent, value);
			}
		}

		// Token: 0x14000033 RID: 51
		// (add) Token: 0x0600163F RID: 5695 RVA: 0x000465C7 File Offset: 0x000455C7
		// (remove) Token: 0x06001640 RID: 5696 RVA: 0x000465D4 File Offset: 0x000455D4
		public static event EventHandler InstalledFontsChanged
		{
			add
			{
				SystemEvents.AddEventHandler(SystemEvents.OnInstalledFontsChangedEvent, value);
			}
			remove
			{
				SystemEvents.RemoveEventHandler(SystemEvents.OnInstalledFontsChangedEvent, value);
			}
		}

		// Token: 0x14000034 RID: 52
		// (add) Token: 0x06001641 RID: 5697 RVA: 0x000465E1 File Offset: 0x000455E1
		// (remove) Token: 0x06001642 RID: 5698 RVA: 0x000465F5 File Offset: 0x000455F5
		[Obsolete("This event has been deprecated. http://go.microsoft.com/fwlink/?linkid=14202")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static event EventHandler LowMemory
		{
			add
			{
				SystemEvents.EnsureSystemEvents(true, true);
				SystemEvents.AddEventHandler(SystemEvents.OnLowMemoryEvent, value);
			}
			remove
			{
				SystemEvents.RemoveEventHandler(SystemEvents.OnLowMemoryEvent, value);
			}
		}

		// Token: 0x14000035 RID: 53
		// (add) Token: 0x06001643 RID: 5699 RVA: 0x00046602 File Offset: 0x00045602
		// (remove) Token: 0x06001644 RID: 5700 RVA: 0x0004660F File Offset: 0x0004560F
		public static event EventHandler PaletteChanged
		{
			add
			{
				SystemEvents.AddEventHandler(SystemEvents.OnPaletteChangedEvent, value);
			}
			remove
			{
				SystemEvents.RemoveEventHandler(SystemEvents.OnPaletteChangedEvent, value);
			}
		}

		// Token: 0x14000036 RID: 54
		// (add) Token: 0x06001645 RID: 5701 RVA: 0x0004661C File Offset: 0x0004561C
		// (remove) Token: 0x06001646 RID: 5702 RVA: 0x00046630 File Offset: 0x00045630
		public static event PowerModeChangedEventHandler PowerModeChanged
		{
			add
			{
				SystemEvents.EnsureSystemEvents(true, true);
				SystemEvents.AddEventHandler(SystemEvents.OnPowerModeChangedEvent, value);
			}
			remove
			{
				SystemEvents.RemoveEventHandler(SystemEvents.OnPowerModeChangedEvent, value);
			}
		}

		// Token: 0x14000037 RID: 55
		// (add) Token: 0x06001647 RID: 5703 RVA: 0x0004663D File Offset: 0x0004563D
		// (remove) Token: 0x06001648 RID: 5704 RVA: 0x00046651 File Offset: 0x00045651
		public static event SessionEndedEventHandler SessionEnded
		{
			add
			{
				SystemEvents.EnsureSystemEvents(true, false);
				SystemEvents.AddEventHandler(SystemEvents.OnSessionEndedEvent, value);
			}
			remove
			{
				SystemEvents.RemoveEventHandler(SystemEvents.OnSessionEndedEvent, value);
			}
		}

		// Token: 0x14000038 RID: 56
		// (add) Token: 0x06001649 RID: 5705 RVA: 0x0004665E File Offset: 0x0004565E
		// (remove) Token: 0x0600164A RID: 5706 RVA: 0x00046672 File Offset: 0x00045672
		public static event SessionEndingEventHandler SessionEnding
		{
			add
			{
				SystemEvents.EnsureSystemEvents(true, false);
				SystemEvents.AddEventHandler(SystemEvents.OnSessionEndingEvent, value);
			}
			remove
			{
				SystemEvents.RemoveEventHandler(SystemEvents.OnSessionEndingEvent, value);
			}
		}

		// Token: 0x14000039 RID: 57
		// (add) Token: 0x0600164B RID: 5707 RVA: 0x0004667F File Offset: 0x0004567F
		// (remove) Token: 0x0600164C RID: 5708 RVA: 0x00046698 File Offset: 0x00045698
		public static event SessionSwitchEventHandler SessionSwitch
		{
			add
			{
				SystemEvents.EnsureSystemEvents(true, true);
				SystemEvents.EnsureRegisteredSessionNotification();
				SystemEvents.AddEventHandler(SystemEvents.OnSessionSwitchEvent, value);
			}
			remove
			{
				SystemEvents.RemoveEventHandler(SystemEvents.OnSessionSwitchEvent, value);
			}
		}

		// Token: 0x1400003A RID: 58
		// (add) Token: 0x0600164D RID: 5709 RVA: 0x000466A5 File Offset: 0x000456A5
		// (remove) Token: 0x0600164E RID: 5710 RVA: 0x000466B9 File Offset: 0x000456B9
		public static event EventHandler TimeChanged
		{
			add
			{
				SystemEvents.EnsureSystemEvents(true, false);
				SystemEvents.AddEventHandler(SystemEvents.OnTimeChangedEvent, value);
			}
			remove
			{
				SystemEvents.RemoveEventHandler(SystemEvents.OnTimeChangedEvent, value);
			}
		}

		// Token: 0x1400003B RID: 59
		// (add) Token: 0x0600164F RID: 5711 RVA: 0x000466C6 File Offset: 0x000456C6
		// (remove) Token: 0x06001650 RID: 5712 RVA: 0x000466DA File Offset: 0x000456DA
		public static event TimerElapsedEventHandler TimerElapsed
		{
			add
			{
				SystemEvents.EnsureSystemEvents(true, false);
				SystemEvents.AddEventHandler(SystemEvents.OnTimerElapsedEvent, value);
			}
			remove
			{
				SystemEvents.RemoveEventHandler(SystemEvents.OnTimerElapsedEvent, value);
			}
		}

		// Token: 0x1400003C RID: 60
		// (add) Token: 0x06001651 RID: 5713 RVA: 0x000466E7 File Offset: 0x000456E7
		// (remove) Token: 0x06001652 RID: 5714 RVA: 0x000466F4 File Offset: 0x000456F4
		public static event UserPreferenceChangedEventHandler UserPreferenceChanged
		{
			add
			{
				SystemEvents.AddEventHandler(SystemEvents.OnUserPreferenceChangedEvent, value);
			}
			remove
			{
				SystemEvents.RemoveEventHandler(SystemEvents.OnUserPreferenceChangedEvent, value);
			}
		}

		// Token: 0x1400003D RID: 61
		// (add) Token: 0x06001653 RID: 5715 RVA: 0x00046701 File Offset: 0x00045701
		// (remove) Token: 0x06001654 RID: 5716 RVA: 0x0004670E File Offset: 0x0004570E
		public static event UserPreferenceChangingEventHandler UserPreferenceChanging
		{
			add
			{
				SystemEvents.AddEventHandler(SystemEvents.OnUserPreferenceChangingEvent, value);
			}
			remove
			{
				SystemEvents.RemoveEventHandler(SystemEvents.OnUserPreferenceChangingEvent, value);
			}
		}

		// Token: 0x06001655 RID: 5717 RVA: 0x0004671C File Offset: 0x0004571C
		private static void AddEventHandler(object key, Delegate value)
		{
			lock (SystemEvents.eventLockObject)
			{
				if (SystemEvents._handlers == null)
				{
					SystemEvents._handlers = new Dictionary<object, List<SystemEvents.SystemEventInvokeInfo>>();
					SystemEvents.EnsureSystemEvents(false, false);
				}
				List<SystemEvents.SystemEventInvokeInfo> list;
				if (!SystemEvents._handlers.TryGetValue(key, out list))
				{
					list = new List<SystemEvents.SystemEventInvokeInfo>();
					SystemEvents._handlers[key] = list;
				}
				else
				{
					list = SystemEvents._handlers[key];
				}
				list.Add(new SystemEvents.SystemEventInvokeInfo(value));
			}
		}

		// Token: 0x06001656 RID: 5718 RVA: 0x000467A4 File Offset: 0x000457A4
		private int ConsoleHandlerProc(int signalType)
		{
			switch (signalType)
			{
			case 5:
				this.OnSessionEnded((IntPtr)1, (IntPtr)int.MinValue);
				break;
			case 6:
				this.OnSessionEnded((IntPtr)1, (IntPtr)0);
				break;
			}
			return 0;
		}

		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x06001657 RID: 5719 RVA: 0x000467F0 File Offset: 0x000457F0
		private NativeMethods.WNDCLASS WndClass
		{
			get
			{
				if (SystemEvents.staticwndclass == null)
				{
					IntPtr moduleHandle = UnsafeNativeMethods.GetModuleHandle(null);
					SystemEvents.className = string.Format(CultureInfo.InvariantCulture, ".NET-BroadcastEventWindow.{0}.{1}.{2}", new object[]
					{
						"2.0.0.0",
						Convert.ToString(AppDomain.CurrentDomain.GetHashCode(), 16),
						SystemEvents.domainQualifier
					});
					SystemEvents.staticwndclass = new NativeMethods.WNDCLASS();
					SystemEvents.staticwndclass.hbrBackground = (IntPtr)6;
					SystemEvents.staticwndclass.style = 0;
					this.windowProc = new NativeMethods.WndProc(this.WindowProc);
					SystemEvents.staticwndclass.lpszClassName = SystemEvents.className;
					SystemEvents.staticwndclass.lpfnWndProc = this.windowProc;
					SystemEvents.staticwndclass.hInstance = moduleHandle;
				}
				return SystemEvents.staticwndclass;
			}
		}

		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x06001658 RID: 5720 RVA: 0x000468BC File Offset: 0x000458BC
		private IntPtr DefWndProc
		{
			get
			{
				if (SystemEvents.defWindowProc == IntPtr.Zero)
				{
					string text = ((Marshal.SystemDefaultCharSize == 1) ? "DefWindowProcA" : "DefWindowProcW");
					SystemEvents.defWindowProc = UnsafeNativeMethods.GetProcAddress(new HandleRef(this, UnsafeNativeMethods.GetModuleHandle("user32.dll")), text);
				}
				return SystemEvents.defWindowProc;
			}
		}

		// Token: 0x06001659 RID: 5721 RVA: 0x0004690F File Offset: 0x0004590F
		private void BumpQualifier()
		{
			SystemEvents.staticwndclass = null;
			SystemEvents.domainQualifier++;
		}

		// Token: 0x0600165A RID: 5722 RVA: 0x00046924 File Offset: 0x00045924
		private IntPtr CreateBroadcastWindow()
		{
			NativeMethods.WNDCLASS_I wndclass_I = new NativeMethods.WNDCLASS_I();
			IntPtr moduleHandle = UnsafeNativeMethods.GetModuleHandle(null);
			if (!UnsafeNativeMethods.GetClassInfo(new HandleRef(this, moduleHandle), this.WndClass.lpszClassName, wndclass_I))
			{
				if (UnsafeNativeMethods.RegisterClass(this.WndClass) == 0)
				{
					this.windowProc = null;
					return IntPtr.Zero;
				}
			}
			else if (wndclass_I.lpfnWndProc == this.DefWndProc)
			{
				short num = 0;
				if (UnsafeNativeMethods.UnregisterClass(this.WndClass.lpszClassName, new HandleRef(null, UnsafeNativeMethods.GetModuleHandle(null))) != 0)
				{
					num = UnsafeNativeMethods.RegisterClass(this.WndClass);
				}
				if (num == 0)
				{
					do
					{
						this.BumpQualifier();
					}
					while (UnsafeNativeMethods.RegisterClass(this.WndClass) == 0 && Marshal.GetLastWin32Error() == 1410);
				}
			}
			return UnsafeNativeMethods.CreateWindowEx(0, this.WndClass.lpszClassName, this.WndClass.lpszClassName, int.MinValue, 0, 0, 0, 0, NativeMethods.NullHandleRef, NativeMethods.NullHandleRef, new HandleRef(this, moduleHandle), null);
		}

		// Token: 0x0600165B RID: 5723 RVA: 0x00046A10 File Offset: 0x00045A10
		public static IntPtr CreateTimer(int interval)
		{
			if (interval <= 0)
			{
				throw new ArgumentException(SR.GetString("InvalidLowBoundArgument", new object[]
				{
					"interval",
					interval.ToString(Thread.CurrentThread.CurrentCulture),
					"0"
				}));
			}
			SystemEvents.EnsureSystemEvents(true, true);
			IntPtr intPtr = UnsafeNativeMethods.SendMessage(new HandleRef(SystemEvents.systemEvents, SystemEvents.systemEvents.windowHandle), 1025, (IntPtr)interval, IntPtr.Zero);
			if (intPtr == IntPtr.Zero)
			{
				throw new ExternalException(SR.GetString("ErrorCreateTimer"));
			}
			return intPtr;
		}

		// Token: 0x0600165C RID: 5724 RVA: 0x00046AAC File Offset: 0x00045AAC
		private void Dispose()
		{
			if (this.windowHandle != IntPtr.Zero)
			{
				if (SystemEvents.registeredSessionNotification)
				{
					UnsafeNativeMethods.WTSUnRegisterSessionNotification(new HandleRef(SystemEvents.systemEvents, SystemEvents.systemEvents.windowHandle));
				}
				IntPtr intPtr = this.windowHandle;
				this.windowHandle = IntPtr.Zero;
				HandleRef handleRef = new HandleRef(this, intPtr);
				if (UnsafeNativeMethods.IsWindow(handleRef) && this.DefWndProc != IntPtr.Zero)
				{
					UnsafeNativeMethods.SetWindowLong(handleRef, -4, new HandleRef(this, this.DefWndProc));
					UnsafeNativeMethods.SetClassLong(handleRef, -24, this.DefWndProc);
				}
				if (UnsafeNativeMethods.IsWindow(handleRef) && !UnsafeNativeMethods.DestroyWindow(handleRef))
				{
					UnsafeNativeMethods.PostMessage(handleRef, 16, IntPtr.Zero, IntPtr.Zero);
				}
				else
				{
					IntPtr moduleHandle = UnsafeNativeMethods.GetModuleHandle(null);
					UnsafeNativeMethods.UnregisterClass(SystemEvents.className, new HandleRef(this, moduleHandle));
				}
			}
			if (this.consoleHandler != null)
			{
				UnsafeNativeMethods.SetConsoleCtrlHandler(this.consoleHandler, 0);
				this.consoleHandler = null;
			}
		}

		// Token: 0x0600165D RID: 5725 RVA: 0x00046BA4 File Offset: 0x00045BA4
		private static void EnsureSystemEvents(bool requireHandle, bool throwOnRefusal)
		{
			if (SystemEvents.systemEvents == null)
			{
				lock (SystemEvents.procLockObject)
				{
					if (SystemEvents.systemEvents == null)
					{
						if (Thread.GetDomain().GetData(".appDomain") != null)
						{
							if (throwOnRefusal)
							{
								throw new InvalidOperationException(SR.GetString("ErrorSystemEventsNotSupported"));
							}
						}
						else
						{
							if (!SystemEvents.UserInteractive || Thread.CurrentThread.GetApartmentState() == ApartmentState.STA)
							{
								SystemEvents.systemEvents = new SystemEvents();
								SystemEvents.systemEvents.Initialize();
							}
							else
							{
								SystemEvents.eventWindowReady = new ManualResetEvent(false);
								SystemEvents.systemEvents = new SystemEvents();
								SystemEvents.windowThread = new Thread(new ThreadStart(SystemEvents.systemEvents.WindowThreadProc));
								SystemEvents.windowThread.IsBackground = true;
								SystemEvents.windowThread.Name = ".NET SystemEvents";
								SystemEvents.windowThread.Start();
								SystemEvents.eventWindowReady.WaitOne();
							}
							if (requireHandle && SystemEvents.systemEvents.windowHandle == IntPtr.Zero)
							{
								throw new ExternalException(SR.GetString("ErrorCreateSystemEvents"));
							}
							SystemEvents.startupRecreates = false;
						}
					}
				}
			}
		}

		// Token: 0x0600165E RID: 5726 RVA: 0x00046CCC File Offset: 0x00045CCC
		private static void EnsureRegisteredSessionNotification()
		{
			if (!SystemEvents.registeredSessionNotification)
			{
				IntPtr intPtr = SafeNativeMethods.LoadLibrary("wtsapi32.dll");
				if (intPtr != IntPtr.Zero)
				{
					UnsafeNativeMethods.WTSRegisterSessionNotification(new HandleRef(SystemEvents.systemEvents, SystemEvents.systemEvents.windowHandle), 0);
					SystemEvents.registeredSessionNotification = true;
					SafeNativeMethods.FreeLibrary(new HandleRef(null, intPtr));
				}
			}
		}

		// Token: 0x0600165F RID: 5727 RVA: 0x00046D28 File Offset: 0x00045D28
		private UserPreferenceCategory GetUserPreferenceCategory(int msg, IntPtr wParam, IntPtr lParam)
		{
			UserPreferenceCategory userPreferenceCategory = UserPreferenceCategory.General;
			if (msg == 26)
			{
				if (lParam != IntPtr.Zero && Marshal.PtrToStringAuto(lParam).Equals("Policy"))
				{
					userPreferenceCategory = UserPreferenceCategory.Policy;
				}
				else if (lParam != IntPtr.Zero && Marshal.PtrToStringAuto(lParam).Equals("intl"))
				{
					userPreferenceCategory = UserPreferenceCategory.Locale;
				}
				else
				{
					int num = (int)wParam;
					if (num <= 113)
					{
						switch (num)
						{
						case 4:
						case 29:
						case 30:
						case 32:
						case 33:
						case 93:
						case 96:
							break;
						case 5:
						case 7:
						case 8:
						case 9:
						case 10:
						case 12:
						case 14:
						case 16:
						case 18:
						case 22:
						case 25:
						case 27:
						case 31:
						case 35:
						case 36:
						case 38:
						case 39:
						case 40:
						case 41:
						case 43:
						case 45:
						case 48:
						case 49:
						case 50:
						case 52:
						case 54:
						case 56:
						case 58:
						case 60:
						case 62:
						case 64:
						case 66:
						case 68:
						case 70:
						case 72:
						case 74:
						case 78:
						case 79:
						case 80:
						case 83:
						case 84:
						case 89:
						case 90:
						case 92:
						case 94:
						case 95:
							return userPreferenceCategory;
						case 6:
						case 37:
						case 42:
						case 44:
						case 73:
						case 76:
						case 77:
							goto IL_02FC;
						case 11:
						case 23:
						case 69:
						case 91:
							return UserPreferenceCategory.Keyboard;
						case 13:
						case 24:
						case 26:
						case 34:
						case 46:
						case 88:
							return UserPreferenceCategory.Icon;
						case 15:
						case 17:
						case 97:
							return UserPreferenceCategory.Screensaver;
						case 19:
						case 20:
						case 21:
						case 47:
						case 75:
						case 87:
							return UserPreferenceCategory.Desktop;
						case 28:
							goto IL_02EE;
						case 51:
						case 53:
						case 55:
						case 57:
						case 59:
						case 61:
						case 63:
						case 65:
						case 67:
						case 71:
							return UserPreferenceCategory.Accessibility;
						case 81:
						case 82:
						case 85:
						case 86:
							return UserPreferenceCategory.Power;
						default:
							switch (num)
							{
							case 101:
							case 103:
							case 105:
								break;
							case 102:
							case 104:
							case 106:
								return userPreferenceCategory;
							case 107:
								goto IL_02EE;
							default:
								switch (num)
								{
								case 111:
									goto IL_02FC;
								case 112:
									return userPreferenceCategory;
								case 113:
									break;
								default:
									return userPreferenceCategory;
								}
								break;
							}
							break;
						}
					}
					else if (num <= 4123)
					{
						switch (num)
						{
						case 4097:
						case 4101:
						case 4103:
						case 4105:
						case 4107:
						case 4109:
							goto IL_02FC;
						case 4098:
						case 4100:
						case 4102:
						case 4104:
						case 4106:
						case 4108:
						case 4110:
							return userPreferenceCategory;
						case 4099:
							goto IL_02EE;
						case 4111:
							break;
						default:
							switch (num)
							{
							case 4115:
							case 4117:
								goto IL_02EE;
							case 4116:
							case 4118:
							case 4120:
							case 4122:
								return userPreferenceCategory;
							case 4119:
							case 4121:
							case 4123:
								break;
							default:
								return userPreferenceCategory;
							}
							break;
						}
					}
					else
					{
						if (num == 4159)
						{
							goto IL_02FC;
						}
						switch (num)
						{
						case 8193:
						case 8195:
						case 8197:
						case 8199:
							goto IL_02FC;
						case 8194:
						case 8196:
						case 8198:
							return userPreferenceCategory;
						default:
							return userPreferenceCategory;
						}
					}
					return UserPreferenceCategory.Mouse;
					IL_02EE:
					return UserPreferenceCategory.Menu;
					IL_02FC:
					userPreferenceCategory = UserPreferenceCategory.Window;
				}
			}
			else if (msg == 21)
			{
				userPreferenceCategory = UserPreferenceCategory.Color;
			}
			return userPreferenceCategory;
		}

		// Token: 0x06001660 RID: 5728 RVA: 0x00047040 File Offset: 0x00046040
		private void Initialize()
		{
			this.consoleHandler = new NativeMethods.ConHndlr(this.ConsoleHandlerProc);
			if (!UnsafeNativeMethods.SetConsoleCtrlHandler(this.consoleHandler, 1))
			{
				this.consoleHandler = null;
			}
			this.windowHandle = this.CreateBroadcastWindow();
			AppDomain.CurrentDomain.ProcessExit += SystemEvents.Shutdown;
			AppDomain.CurrentDomain.DomainUnload += SystemEvents.Shutdown;
		}

		// Token: 0x06001661 RID: 5729 RVA: 0x000470AC File Offset: 0x000460AC
		private void InvokeMarshaledCallbacks()
		{
			Delegate @delegate = null;
			lock (SystemEvents.threadCallbackList)
			{
				if (SystemEvents.threadCallbackList.Count > 0)
				{
					@delegate = (Delegate)SystemEvents.threadCallbackList.Dequeue();
				}
				goto IL_0094;
			}
			try
			{
				IL_0034:
				EventHandler eventHandler = @delegate as EventHandler;
				if (eventHandler != null)
				{
					eventHandler(null, EventArgs.Empty);
				}
				else
				{
					@delegate.DynamicInvoke(new object[0]);
				}
			}
			catch (Exception)
			{
			}
			lock (SystemEvents.threadCallbackList)
			{
				if (SystemEvents.threadCallbackList.Count > 0)
				{
					@delegate = (Delegate)SystemEvents.threadCallbackList.Dequeue();
				}
				else
				{
					@delegate = null;
				}
			}
			IL_0094:
			if (@delegate == null)
			{
				return;
			}
			goto IL_0034;
		}

		// Token: 0x06001662 RID: 5730 RVA: 0x00047178 File Offset: 0x00046178
		public static void InvokeOnEventsThread(Delegate method)
		{
			SystemEvents.EnsureSystemEvents(true, true);
			if (SystemEvents.threadCallbackList == null)
			{
				lock (SystemEvents.eventLockObject)
				{
					if (SystemEvents.threadCallbackList == null)
					{
						SystemEvents.threadCallbackList = new Queue();
						SystemEvents.threadCallbackMessage = SafeNativeMethods.RegisterWindowMessage("SystemEventsThreadCallbackMessage");
					}
				}
			}
			lock (SystemEvents.threadCallbackList)
			{
				SystemEvents.threadCallbackList.Enqueue(method);
			}
			UnsafeNativeMethods.PostMessage(new HandleRef(SystemEvents.systemEvents, SystemEvents.systemEvents.windowHandle), SystemEvents.threadCallbackMessage, IntPtr.Zero, IntPtr.Zero);
		}

		// Token: 0x06001663 RID: 5731 RVA: 0x00047230 File Offset: 0x00046230
		public static void KillTimer(IntPtr timerId)
		{
			SystemEvents.EnsureSystemEvents(true, true);
			if (SystemEvents.systemEvents.windowHandle != IntPtr.Zero && (int)UnsafeNativeMethods.SendMessage(new HandleRef(SystemEvents.systemEvents, SystemEvents.systemEvents.windowHandle), 1026, timerId, IntPtr.Zero) == 0)
			{
				throw new ExternalException(SR.GetString("ErrorKillTimer"));
			}
		}

		// Token: 0x06001664 RID: 5732 RVA: 0x00047298 File Offset: 0x00046298
		private IntPtr OnCreateTimer(IntPtr wParam)
		{
			IntPtr intPtr = (IntPtr)SystemEvents.randomTimerId.Next();
			IntPtr intPtr2 = UnsafeNativeMethods.SetTimer(new HandleRef(this, this.windowHandle), new HandleRef(this, intPtr), (int)wParam, NativeMethods.NullHandleRef);
			if (!(intPtr2 == IntPtr.Zero))
			{
				return intPtr;
			}
			return IntPtr.Zero;
		}

		// Token: 0x06001665 RID: 5733 RVA: 0x000472F0 File Offset: 0x000462F0
		private void OnDisplaySettingsChanging()
		{
			SystemEvents.RaiseEvent(SystemEvents.OnDisplaySettingsChangingEvent, new object[]
			{
				this,
				EventArgs.Empty
			});
		}

		// Token: 0x06001666 RID: 5734 RVA: 0x0004731C File Offset: 0x0004631C
		private void OnDisplaySettingsChanged()
		{
			SystemEvents.RaiseEvent(SystemEvents.OnDisplaySettingsChangedEvent, new object[]
			{
				this,
				EventArgs.Empty
			});
		}

		// Token: 0x06001667 RID: 5735 RVA: 0x00047348 File Offset: 0x00046348
		private void OnGenericEvent(object eventKey)
		{
			SystemEvents.RaiseEvent(eventKey, new object[]
			{
				this,
				EventArgs.Empty
			});
		}

		// Token: 0x06001668 RID: 5736 RVA: 0x00047370 File Offset: 0x00046370
		private void OnShutdown(object eventKey)
		{
			SystemEvents.RaiseEvent(false, eventKey, new object[]
			{
				this,
				EventArgs.Empty
			});
		}

		// Token: 0x06001669 RID: 5737 RVA: 0x00047398 File Offset: 0x00046398
		private bool OnKillTimer(IntPtr wParam)
		{
			return UnsafeNativeMethods.KillTimer(new HandleRef(this, this.windowHandle), new HandleRef(this, wParam));
		}

		// Token: 0x0600166A RID: 5738 RVA: 0x000473C0 File Offset: 0x000463C0
		private void OnPowerModeChanged(IntPtr wParam)
		{
			PowerModes powerModes;
			switch ((int)wParam)
			{
			case 4:
			case 5:
				powerModes = PowerModes.Suspend;
				break;
			case 6:
			case 7:
			case 8:
				powerModes = PowerModes.Resume;
				break;
			case 9:
			case 10:
			case 11:
				powerModes = PowerModes.StatusChange;
				break;
			default:
				return;
			}
			SystemEvents.RaiseEvent(SystemEvents.OnPowerModeChangedEvent, new object[]
			{
				this,
				new PowerModeChangedEventArgs(powerModes)
			});
		}

		// Token: 0x0600166B RID: 5739 RVA: 0x00047428 File Offset: 0x00046428
		private void OnSessionEnded(IntPtr wParam, IntPtr lParam)
		{
			if (wParam != (IntPtr)0)
			{
				SessionEndReasons sessionEndReasons = SessionEndReasons.SystemShutdown;
				if (((int)(long)lParam & -2147483648) != 0)
				{
					sessionEndReasons = SessionEndReasons.Logoff;
				}
				SessionEndedEventArgs sessionEndedEventArgs = new SessionEndedEventArgs(sessionEndReasons);
				SystemEvents.RaiseEvent(SystemEvents.OnSessionEndedEvent, new object[] { this, sessionEndedEventArgs });
			}
		}

		// Token: 0x0600166C RID: 5740 RVA: 0x00047478 File Offset: 0x00046478
		private int OnSessionEnding(IntPtr lParam)
		{
			SessionEndReasons sessionEndReasons = SessionEndReasons.SystemShutdown;
			if (((long)lParam & -2147483648L) != 0L)
			{
				sessionEndReasons = SessionEndReasons.Logoff;
			}
			SessionEndingEventArgs sessionEndingEventArgs = new SessionEndingEventArgs(sessionEndReasons);
			SystemEvents.RaiseEvent(SystemEvents.OnSessionEndingEvent, new object[] { this, sessionEndingEventArgs });
			return sessionEndingEventArgs.Cancel ? 0 : 1;
		}

		// Token: 0x0600166D RID: 5741 RVA: 0x000474CC File Offset: 0x000464CC
		private void OnSessionSwitch(int wParam)
		{
			SessionSwitchEventArgs sessionSwitchEventArgs = new SessionSwitchEventArgs((SessionSwitchReason)wParam);
			SystemEvents.RaiseEvent(SystemEvents.OnSessionSwitchEvent, new object[] { this, sessionSwitchEventArgs });
		}

		// Token: 0x0600166E RID: 5742 RVA: 0x000474FC File Offset: 0x000464FC
		private void OnThemeChanged()
		{
			SystemEvents.RaiseEvent(SystemEvents.OnUserPreferenceChangingEvent, new object[]
			{
				this,
				new UserPreferenceChangingEventArgs(UserPreferenceCategory.VisualStyle)
			});
			UserPreferenceCategory userPreferenceCategory = UserPreferenceCategory.Window;
			SystemEvents.RaiseEvent(SystemEvents.OnUserPreferenceChangedEvent, new object[]
			{
				this,
				new UserPreferenceChangedEventArgs(userPreferenceCategory)
			});
			userPreferenceCategory = UserPreferenceCategory.VisualStyle;
			SystemEvents.RaiseEvent(SystemEvents.OnUserPreferenceChangedEvent, new object[]
			{
				this,
				new UserPreferenceChangedEventArgs(userPreferenceCategory)
			});
		}

		// Token: 0x0600166F RID: 5743 RVA: 0x00047570 File Offset: 0x00046570
		private void OnUserPreferenceChanged(int msg, IntPtr wParam, IntPtr lParam)
		{
			UserPreferenceCategory userPreferenceCategory = this.GetUserPreferenceCategory(msg, wParam, lParam);
			SystemEvents.RaiseEvent(SystemEvents.OnUserPreferenceChangedEvent, new object[]
			{
				this,
				new UserPreferenceChangedEventArgs(userPreferenceCategory)
			});
		}

		// Token: 0x06001670 RID: 5744 RVA: 0x000475A8 File Offset: 0x000465A8
		private void OnUserPreferenceChanging(int msg, IntPtr wParam, IntPtr lParam)
		{
			UserPreferenceCategory userPreferenceCategory = this.GetUserPreferenceCategory(msg, wParam, lParam);
			SystemEvents.RaiseEvent(SystemEvents.OnUserPreferenceChangingEvent, new object[]
			{
				this,
				new UserPreferenceChangingEventArgs(userPreferenceCategory)
			});
		}

		// Token: 0x06001671 RID: 5745 RVA: 0x000475E0 File Offset: 0x000465E0
		private void OnTimerElapsed(IntPtr wParam)
		{
			SystemEvents.RaiseEvent(SystemEvents.OnTimerElapsedEvent, new object[]
			{
				this,
				new TimerElapsedEventArgs(wParam)
			});
		}

		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x06001672 RID: 5746 RVA: 0x0004760C File Offset: 0x0004660C
		internal static bool UseEverettThreadAffinity
		{
			get
			{
				if (!SystemEvents.checkedThreadAffinity)
				{
					lock (SystemEvents.eventLockObject)
					{
						if (!SystemEvents.checkedThreadAffinity)
						{
							SystemEvents.checkedThreadAffinity = true;
							string text = "Software\\{0}\\{1}\\{2}";
							try
							{
								new RegistryPermission(PermissionState.Unrestricted).Assert();
								RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(string.Format(CultureInfo.CurrentCulture, text, new object[]
								{
									SystemEvents.CompanyNameInternal,
									SystemEvents.ProductNameInternal,
									SystemEvents.ProductVersionInternal
								}));
								if (registryKey != null)
								{
									object value = registryKey.GetValue("EnableSystemEventsThreadAffinityCompatibility");
									if (value != null && (int)value != 0)
									{
										SystemEvents.useEverettThreadAffinity = true;
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
					}
				}
				return SystemEvents.useEverettThreadAffinity;
			}
		}

		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x06001673 RID: 5747 RVA: 0x000476E8 File Offset: 0x000466E8
		private static string CompanyNameInternal
		{
			get
			{
				string text = null;
				Assembly entryAssembly = Assembly.GetEntryAssembly();
				if (entryAssembly != null)
				{
					object[] customAttributes = entryAssembly.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
					if (customAttributes != null && customAttributes.Length > 0)
					{
						text = ((AssemblyCompanyAttribute)customAttributes[0]).Company;
					}
				}
				if (text == null || text.Length == 0)
				{
					text = SystemEvents.GetAppFileVersionInfo().CompanyName;
					if (text != null)
					{
						text = text.Trim();
					}
				}
				if (text == null || text.Length == 0)
				{
					Type appMainType = SystemEvents.GetAppMainType();
					if (appMainType != null)
					{
						string @namespace = appMainType.Namespace;
						if (!string.IsNullOrEmpty(@namespace))
						{
							int num = @namespace.IndexOf(".", StringComparison.Ordinal);
							if (num != -1)
							{
								text = @namespace.Substring(0, num);
							}
							else
							{
								text = @namespace;
							}
						}
						else
						{
							text = SystemEvents.ProductNameInternal;
						}
					}
				}
				return text;
			}
		}

		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x06001674 RID: 5748 RVA: 0x0004779C File Offset: 0x0004679C
		private static string ProductNameInternal
		{
			get
			{
				string text = null;
				Assembly entryAssembly = Assembly.GetEntryAssembly();
				if (entryAssembly != null)
				{
					object[] customAttributes = entryAssembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false);
					if (customAttributes != null && customAttributes.Length > 0)
					{
						text = ((AssemblyProductAttribute)customAttributes[0]).Product;
					}
				}
				if (text == null || text.Length == 0)
				{
					text = SystemEvents.GetAppFileVersionInfo().ProductName;
					if (text != null)
					{
						text = text.Trim();
					}
				}
				if (text == null || text.Length == 0)
				{
					Type appMainType = SystemEvents.GetAppMainType();
					if (appMainType != null)
					{
						string @namespace = appMainType.Namespace;
						if (!string.IsNullOrEmpty(@namespace))
						{
							int num = @namespace.LastIndexOf(".", StringComparison.Ordinal);
							if (num != -1 && num < @namespace.Length - 1)
							{
								text = @namespace.Substring(num + 1);
							}
							else
							{
								text = @namespace;
							}
						}
						else
						{
							text = appMainType.Name;
						}
					}
				}
				return text;
			}
		}

		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x06001675 RID: 5749 RVA: 0x00047860 File Offset: 0x00046860
		private static string ProductVersionInternal
		{
			get
			{
				string text = null;
				Assembly entryAssembly = Assembly.GetEntryAssembly();
				if (entryAssembly != null)
				{
					object[] customAttributes = entryAssembly.GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false);
					if (customAttributes != null && customAttributes.Length > 0)
					{
						text = ((AssemblyInformationalVersionAttribute)customAttributes[0]).InformationalVersion;
					}
				}
				if (text == null || text.Length == 0)
				{
					text = SystemEvents.GetAppFileVersionInfo().ProductVersion;
					if (text != null)
					{
						text = text.Trim();
					}
				}
				if (text == null || text.Length == 0)
				{
					text = "1.0.0.0";
				}
				return text;
			}
		}

		// Token: 0x06001676 RID: 5750 RVA: 0x000478D4 File Offset: 0x000468D4
		private static FileVersionInfo GetAppFileVersionInfo()
		{
			if (SystemEvents.appFileVersion == null)
			{
				Type appMainType = SystemEvents.GetAppMainType();
				if (appMainType != null)
				{
					new FileIOPermission(PermissionState.None)
					{
						AllFiles = (FileIOPermissionAccess.Read | FileIOPermissionAccess.PathDiscovery)
					}.Assert();
					try
					{
						SystemEvents.appFileVersion = FileVersionInfo.GetVersionInfo(appMainType.Module.FullyQualifiedName);
						goto IL_0051;
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
				}
				SystemEvents.appFileVersion = FileVersionInfo.GetVersionInfo(SystemEvents.ExecutablePath);
			}
			IL_0051:
			return (FileVersionInfo)SystemEvents.appFileVersion;
		}

		// Token: 0x06001677 RID: 5751 RVA: 0x0004794C File Offset: 0x0004694C
		private static Type GetAppMainType()
		{
			if (SystemEvents.mainType == null)
			{
				Assembly entryAssembly = Assembly.GetEntryAssembly();
				if (entryAssembly != null)
				{
					SystemEvents.mainType = entryAssembly.EntryPoint.ReflectedType;
				}
			}
			return SystemEvents.mainType;
		}

		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x06001678 RID: 5752 RVA: 0x00047980 File Offset: 0x00046980
		private static string ExecutablePath
		{
			get
			{
				if (SystemEvents.executablePath == null)
				{
					Assembly entryAssembly = Assembly.GetEntryAssembly();
					if (entryAssembly == null)
					{
						StringBuilder stringBuilder = new StringBuilder(260);
						UnsafeNativeMethods.GetModuleFileName(NativeMethods.NullHandleRef, stringBuilder, stringBuilder.Capacity);
						SystemEvents.executablePath = IntSecurity.UnsafeGetFullPath(stringBuilder.ToString());
					}
					else
					{
						string escapedCodeBase = entryAssembly.EscapedCodeBase;
						Uri uri = new Uri(escapedCodeBase);
						if (uri.Scheme == "file")
						{
							SystemEvents.executablePath = NativeMethods.GetLocalPath(escapedCodeBase);
						}
						else
						{
							SystemEvents.executablePath = uri.ToString();
						}
					}
				}
				Uri uri2 = new Uri(SystemEvents.executablePath);
				if (uri2.Scheme == "file")
				{
					new FileIOPermission(FileIOPermissionAccess.PathDiscovery, SystemEvents.executablePath).Demand();
				}
				return SystemEvents.executablePath;
			}
		}

		// Token: 0x06001679 RID: 5753 RVA: 0x00047A38 File Offset: 0x00046A38
		private static void RaiseEvent(object key, params object[] args)
		{
			SystemEvents.RaiseEvent(true, key, args);
		}

		// Token: 0x0600167A RID: 5754 RVA: 0x00047A44 File Offset: 0x00046A44
		private static void RaiseEvent(bool checkFinalization, object key, params object[] args)
		{
			if (checkFinalization && AppDomain.CurrentDomain.IsFinalizingForUnload())
			{
				return;
			}
			SystemEvents.SystemEventInvokeInfo[] array = null;
			lock (SystemEvents.eventLockObject)
			{
				if (SystemEvents._handlers != null && SystemEvents._handlers.ContainsKey(key))
				{
					List<SystemEvents.SystemEventInvokeInfo> list = SystemEvents._handlers[key];
					if (list != null)
					{
						array = list.ToArray();
					}
				}
			}
			if (array != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					try
					{
						SystemEvents.SystemEventInvokeInfo systemEventInvokeInfo = array[i];
						systemEventInvokeInfo.Invoke(checkFinalization, args);
						array[i] = null;
					}
					catch (Exception)
					{
					}
				}
				lock (SystemEvents.eventLockObject)
				{
					List<SystemEvents.SystemEventInvokeInfo> list2 = null;
					foreach (SystemEvents.SystemEventInvokeInfo systemEventInvokeInfo2 in array)
					{
						if (systemEventInvokeInfo2 != null)
						{
							if (list2 == null && !SystemEvents._handlers.TryGetValue(key, out list2))
							{
								break;
							}
							list2.Remove(systemEventInvokeInfo2);
						}
					}
				}
			}
		}

		// Token: 0x0600167B RID: 5755 RVA: 0x00047B48 File Offset: 0x00046B48
		private static void RemoveEventHandler(object key, Delegate value)
		{
			lock (SystemEvents.eventLockObject)
			{
				if (SystemEvents._handlers != null && SystemEvents._handlers.ContainsKey(key))
				{
					List<SystemEvents.SystemEventInvokeInfo> list = SystemEvents._handlers[key];
					list.Remove(new SystemEvents.SystemEventInvokeInfo(value));
				}
			}
		}

		// Token: 0x0600167C RID: 5756 RVA: 0x00047BA8 File Offset: 0x00046BA8
		private static void Startup()
		{
			if (SystemEvents.startupRecreates)
			{
				SystemEvents.EnsureSystemEvents(false, false);
			}
		}

		// Token: 0x0600167D RID: 5757 RVA: 0x00047BB8 File Offset: 0x00046BB8
		private static void Shutdown()
		{
			if (SystemEvents.systemEvents != null && SystemEvents.systemEvents.windowHandle != IntPtr.Zero)
			{
				lock (SystemEvents.procLockObject)
				{
					if (SystemEvents.systemEvents != null)
					{
						SystemEvents.startupRecreates = true;
						if (SystemEvents.windowThread != null)
						{
							SystemEvents.eventThreadTerminated = new ManualResetEvent(false);
							UnsafeNativeMethods.PostMessage(new HandleRef(SystemEvents.systemEvents, SystemEvents.systemEvents.windowHandle), 18, IntPtr.Zero, IntPtr.Zero);
							SystemEvents.eventThreadTerminated.WaitOne();
							SystemEvents.windowThread.Join();
						}
						else
						{
							SystemEvents.systemEvents.Dispose();
							SystemEvents.systemEvents = null;
						}
					}
				}
			}
		}

		// Token: 0x0600167E RID: 5758 RVA: 0x00047C7C File Offset: 0x00046C7C
		[PrePrepareMethod]
		private static void Shutdown(object sender, EventArgs e)
		{
			SystemEvents.Shutdown();
		}

		// Token: 0x0600167F RID: 5759 RVA: 0x00047C84 File Offset: 0x00046C84
		private IntPtr WindowProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam)
		{
			if (msg <= 785)
			{
				if (msg <= 65)
				{
					if (msg <= 22)
					{
						if (msg == 17)
						{
							return (IntPtr)this.OnSessionEnding(lParam);
						}
						switch (msg)
						{
						case 21:
							break;
						case 22:
							this.OnSessionEnded(wParam, lParam);
							goto IL_02D6;
						default:
							goto IL_02BF;
						}
					}
					else
					{
						switch (msg)
						{
						case 26:
						{
							IntPtr intPtr = lParam;
							if (lParam != IntPtr.Zero)
							{
								string text = Marshal.PtrToStringAuto(lParam);
								if (text != null)
								{
									intPtr = Marshal.StringToHGlobalAuto(text);
								}
							}
							UnsafeNativeMethods.PostMessage(new HandleRef(this, this.windowHandle), 8192 + msg, wParam, intPtr);
							goto IL_02D6;
						}
						case 27:
						case 28:
							goto IL_02BF;
						case 29:
						case 30:
							break;
						default:
							if (msg != 65)
							{
								goto IL_02BF;
							}
							break;
						}
					}
				}
				else if (msg <= 275)
				{
					if (msg != 126 && msg != 275)
					{
						goto IL_02BF;
					}
				}
				else
				{
					if (msg == 536)
					{
						this.OnPowerModeChanged(wParam);
						goto IL_02D6;
					}
					if (msg == 689)
					{
						this.OnSessionSwitch((int)wParam);
						goto IL_02D6;
					}
					if (msg != 785)
					{
						goto IL_02BF;
					}
				}
			}
			else if (msg <= 8222)
			{
				if (msg > 1026)
				{
					if (msg != 8213)
					{
						switch (msg)
						{
						case 8218:
							try
							{
								this.OnUserPreferenceChanging(msg - 8192, wParam, lParam);
								this.OnUserPreferenceChanged(msg - 8192, wParam, lParam);
								goto IL_02D6;
							}
							finally
							{
								try
								{
									if (lParam != IntPtr.Zero)
									{
										Marshal.FreeHGlobal(lParam);
									}
								}
								catch (Exception)
								{
								}
							}
							break;
						case 8219:
						case 8220:
							goto IL_02BF;
						case 8221:
							this.OnGenericEvent(SystemEvents.OnInstalledFontsChangedEvent);
							goto IL_02D6;
						case 8222:
							this.OnGenericEvent(SystemEvents.OnTimeChangedEvent);
							goto IL_02D6;
						default:
							goto IL_02BF;
						}
					}
					this.OnUserPreferenceChanging(msg - 8192, wParam, lParam);
					this.OnUserPreferenceChanged(msg - 8192, wParam, lParam);
					goto IL_02D6;
				}
				if (msg != 794)
				{
					switch (msg)
					{
					case 1025:
						return this.OnCreateTimer(wParam);
					case 1026:
						return (IntPtr)(this.OnKillTimer(wParam) ? 1 : 0);
					default:
						goto IL_02BF;
					}
				}
			}
			else if (msg <= 8318)
			{
				if (msg == 8257)
				{
					this.OnGenericEvent(SystemEvents.OnLowMemoryEvent);
					goto IL_02D6;
				}
				if (msg != 8318)
				{
					goto IL_02BF;
				}
				this.OnDisplaySettingsChanging();
				this.OnDisplaySettingsChanged();
				goto IL_02D6;
			}
			else
			{
				if (msg == 8467)
				{
					this.OnTimerElapsed(wParam);
					goto IL_02D6;
				}
				if (msg == 8977)
				{
					this.OnGenericEvent(SystemEvents.OnPaletteChangedEvent);
					goto IL_02D6;
				}
				if (msg != 8986)
				{
					goto IL_02BF;
				}
				this.OnThemeChanged();
				goto IL_02D6;
			}
			UnsafeNativeMethods.PostMessage(new HandleRef(this, this.windowHandle), 8192 + msg, wParam, lParam);
			goto IL_02D6;
			IL_02BF:
			if (msg == SystemEvents.threadCallbackMessage && msg != 0)
			{
				this.InvokeMarshaledCallbacks();
				return IntPtr.Zero;
			}
			IL_02D6:
			return UnsafeNativeMethods.DefWindowProc(hWnd, msg, wParam, lParam);
		}

		// Token: 0x06001680 RID: 5760 RVA: 0x00047F90 File Offset: 0x00046F90
		private void WindowThreadProc()
		{
			try
			{
				this.Initialize();
				SystemEvents.eventWindowReady.Set();
				if (this.windowHandle != IntPtr.Zero)
				{
					NativeMethods.MSG msg = default(NativeMethods.MSG);
					bool flag = true;
					while (flag)
					{
						int num = UnsafeNativeMethods.MsgWaitForMultipleObjectsEx(0, IntPtr.Zero, 100, 255, 4);
						if (num == 258)
						{
							Thread.Sleep(1);
						}
						else
						{
							while (UnsafeNativeMethods.PeekMessage(ref msg, NativeMethods.NullHandleRef, 0, 0, 1))
							{
								if (msg.message == 18)
								{
									flag = false;
									break;
								}
								UnsafeNativeMethods.TranslateMessage(ref msg);
								UnsafeNativeMethods.DispatchMessage(ref msg);
							}
						}
					}
				}
				this.OnShutdown(SystemEvents.OnEventsThreadShutdownEvent);
			}
			catch (Exception ex)
			{
				SystemEvents.eventWindowReady.Set();
				if (!(ex is ThreadInterruptedException))
				{
					ThreadAbortException ex2 = ex as ThreadAbortException;
				}
			}
			this.Dispose();
			if (SystemEvents.eventThreadTerminated != null)
			{
				SystemEvents.eventThreadTerminated.Set();
			}
		}

		// Token: 0x040015C3 RID: 5571
		private const string everettThreadAffinityValue = "EnableSystemEventsThreadAffinityCompatibility";

		// Token: 0x040015C4 RID: 5572
		private static readonly object eventLockObject = new object();

		// Token: 0x040015C5 RID: 5573
		private static readonly object procLockObject = new object();

		// Token: 0x040015C6 RID: 5574
		private static SystemEvents systemEvents;

		// Token: 0x040015C7 RID: 5575
		private static Thread windowThread;

		// Token: 0x040015C8 RID: 5576
		private static ManualResetEvent eventWindowReady;

		// Token: 0x040015C9 RID: 5577
		private static Random randomTimerId = new Random();

		// Token: 0x040015CA RID: 5578
		private static bool startupRecreates;

		// Token: 0x040015CB RID: 5579
		private static bool registeredSessionNotification = false;

		// Token: 0x040015CC RID: 5580
		private static int domainQualifier;

		// Token: 0x040015CD RID: 5581
		private static NativeMethods.WNDCLASS staticwndclass;

		// Token: 0x040015CE RID: 5582
		private static IntPtr defWindowProc;

		// Token: 0x040015CF RID: 5583
		private static string className = null;

		// Token: 0x040015D0 RID: 5584
		private static Queue threadCallbackList;

		// Token: 0x040015D1 RID: 5585
		private static int threadCallbackMessage = 0;

		// Token: 0x040015D2 RID: 5586
		private static ManualResetEvent eventThreadTerminated;

		// Token: 0x040015D3 RID: 5587
		private static bool checkedThreadAffinity = false;

		// Token: 0x040015D4 RID: 5588
		private static bool useEverettThreadAffinity = false;

		// Token: 0x040015D5 RID: 5589
		private IntPtr windowHandle;

		// Token: 0x040015D6 RID: 5590
		private NativeMethods.WndProc windowProc;

		// Token: 0x040015D7 RID: 5591
		private NativeMethods.ConHndlr consoleHandler;

		// Token: 0x040015D8 RID: 5592
		private static readonly object OnUserPreferenceChangingEvent = new object();

		// Token: 0x040015D9 RID: 5593
		private static readonly object OnUserPreferenceChangedEvent = new object();

		// Token: 0x040015DA RID: 5594
		private static readonly object OnSessionEndingEvent = new object();

		// Token: 0x040015DB RID: 5595
		private static readonly object OnSessionEndedEvent = new object();

		// Token: 0x040015DC RID: 5596
		private static readonly object OnPowerModeChangedEvent = new object();

		// Token: 0x040015DD RID: 5597
		private static readonly object OnLowMemoryEvent = new object();

		// Token: 0x040015DE RID: 5598
		private static readonly object OnDisplaySettingsChangingEvent = new object();

		// Token: 0x040015DF RID: 5599
		private static readonly object OnDisplaySettingsChangedEvent = new object();

		// Token: 0x040015E0 RID: 5600
		private static readonly object OnInstalledFontsChangedEvent = new object();

		// Token: 0x040015E1 RID: 5601
		private static readonly object OnTimeChangedEvent = new object();

		// Token: 0x040015E2 RID: 5602
		private static readonly object OnTimerElapsedEvent = new object();

		// Token: 0x040015E3 RID: 5603
		private static readonly object OnPaletteChangedEvent = new object();

		// Token: 0x040015E4 RID: 5604
		private static readonly object OnEventsThreadShutdownEvent = new object();

		// Token: 0x040015E5 RID: 5605
		private static readonly object OnSessionSwitchEvent = new object();

		// Token: 0x040015E6 RID: 5606
		private static Dictionary<object, List<SystemEvents.SystemEventInvokeInfo>> _handlers;

		// Token: 0x040015E7 RID: 5607
		private static IntPtr processWinStation = IntPtr.Zero;

		// Token: 0x040015E8 RID: 5608
		private static bool isUserInteractive = false;

		// Token: 0x040015E9 RID: 5609
		private static object appFileVersion;

		// Token: 0x040015EA RID: 5610
		private static Type mainType;

		// Token: 0x040015EB RID: 5611
		private static string executablePath = null;

		// Token: 0x020002A5 RID: 677
		private class SystemEventInvokeInfo
		{
			// Token: 0x06001682 RID: 5762 RVA: 0x00048163 File Offset: 0x00047163
			public SystemEventInvokeInfo(Delegate d)
			{
				this._delegate = d;
				this._syncContext = AsyncOperationManager.SynchronizationContext;
			}

			// Token: 0x06001683 RID: 5763 RVA: 0x00048180 File Offset: 0x00047180
			public void Invoke(bool checkFinalization, params object[] args)
			{
				try
				{
					if (this._syncContext == null || SystemEvents.UseEverettThreadAffinity)
					{
						this.InvokeCallback(args);
					}
					else
					{
						this._syncContext.Send(new SendOrPostCallback(this.InvokeCallback), args);
					}
				}
				catch (InvalidAsynchronousStateException)
				{
					if (!checkFinalization || !AppDomain.CurrentDomain.IsFinalizingForUnload())
					{
						this.InvokeCallback(args);
					}
				}
			}

			// Token: 0x06001684 RID: 5764 RVA: 0x000481E8 File Offset: 0x000471E8
			private void InvokeCallback(object arg)
			{
				this._delegate.DynamicInvoke((object[])arg);
			}

			// Token: 0x06001685 RID: 5765 RVA: 0x000481FC File Offset: 0x000471FC
			public override bool Equals(object other)
			{
				SystemEvents.SystemEventInvokeInfo systemEventInvokeInfo = other as SystemEvents.SystemEventInvokeInfo;
				return systemEventInvokeInfo != null && systemEventInvokeInfo._delegate.Equals(this._delegate);
			}

			// Token: 0x06001686 RID: 5766 RVA: 0x00048226 File Offset: 0x00047226
			public override int GetHashCode()
			{
				return base.GetHashCode();
			}

			// Token: 0x040015EC RID: 5612
			private SynchronizationContext _syncContext;

			// Token: 0x040015ED RID: 5613
			private Delegate _delegate;
		}
	}
}
