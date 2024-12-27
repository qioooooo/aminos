using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Internal;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using Microsoft.Win32;

namespace System.Windows.Forms
{
	// Token: 0x020001ED RID: 493
	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class NativeWindow : MarshalByRefObject, IWin32Window
	{
		// Token: 0x060016BA RID: 5818 RVA: 0x000214D8 File Offset: 0x000204D8
		static NativeWindow()
		{
			EventHandler eventHandler = new EventHandler(NativeWindow.OnShutdown);
			AppDomain.CurrentDomain.ProcessExit += eventHandler;
			AppDomain.CurrentDomain.DomainUnload += eventHandler;
			int num = NativeWindow.primes[4];
			NativeWindow.hashBuckets = new NativeWindow.HandleBucket[num];
			NativeWindow.hashLoadSize = (int)(0.72f * (float)num);
			if (NativeWindow.hashLoadSize >= num)
			{
				NativeWindow.hashLoadSize = num - 1;
			}
			NativeWindow.hashForIdHandle = new Dictionary<short, IntPtr>();
			NativeWindow.hashForHandleId = new Dictionary<IntPtr, short>();
		}

		// Token: 0x060016BB RID: 5819 RVA: 0x0002158A File Offset: 0x0002058A
		public NativeWindow()
		{
			this.weakThisPtr = new WeakReference(this);
		}

		// Token: 0x060016BC RID: 5820 RVA: 0x000215A0 File Offset: 0x000205A0
		~NativeWindow()
		{
			this.ForceExitMessageLoop();
		}

		// Token: 0x060016BD RID: 5821 RVA: 0x000215CC File Offset: 0x000205CC
		internal void ForceExitMessageLoop()
		{
			IntPtr intPtr;
			bool flag;
			lock (this)
			{
				intPtr = this.handle;
				flag = this.ownHandle;
			}
			if (this.handle != IntPtr.Zero)
			{
				if (UnsafeNativeMethods.IsWindow(new HandleRef(null, this.handle)))
				{
					int num;
					int windowThreadProcessId = SafeNativeMethods.GetWindowThreadProcessId(new HandleRef(null, this.handle), out num);
					Application.ThreadContext threadContext = Application.ThreadContext.FromId(windowThreadProcessId);
					IntPtr intPtr2 = ((threadContext == null) ? IntPtr.Zero : threadContext.GetHandle());
					if (intPtr2 != IntPtr.Zero)
					{
						int num2 = 0;
						SafeNativeMethods.GetExitCodeThread(new HandleRef(null, intPtr2), out num2);
						if (!AppDomain.CurrentDomain.IsFinalizingForUnload() && num2 == 259)
						{
							IntPtr intPtr3;
							UnsafeNativeMethods.SendMessageTimeout(new HandleRef(null, this.handle), NativeMethods.WM_UIUNSUBCLASS, IntPtr.Zero, IntPtr.Zero, 2, 100, out intPtr3) == IntPtr.Zero;
						}
					}
				}
				if (this.handle != IntPtr.Zero)
				{
					this.ReleaseHandle(true);
				}
			}
			if (intPtr != IntPtr.Zero && flag)
			{
				UnsafeNativeMethods.PostMessage(new HandleRef(this, intPtr), 16, 0, 0);
			}
		}

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x060016BE RID: 5822 RVA: 0x00021708 File Offset: 0x00020708
		internal static bool AnyHandleCreated
		{
			get
			{
				return NativeWindow.anyHandleCreated;
			}
		}

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x060016BF RID: 5823 RVA: 0x0002170F File Offset: 0x0002070F
		public IntPtr Handle
		{
			get
			{
				return this.handle;
			}
		}

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x060016C0 RID: 5824 RVA: 0x00021717 File Offset: 0x00020717
		internal NativeWindow PreviousWindow
		{
			get
			{
				return this.previousWindow;
			}
		}

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x060016C1 RID: 5825 RVA: 0x0002171F File Offset: 0x0002071F
		internal static IntPtr UserDefindowProc
		{
			get
			{
				return NativeWindow.userDefWindowProc;
			}
		}

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x060016C2 RID: 5826 RVA: 0x00021728 File Offset: 0x00020728
		private static int WndProcFlags
		{
			get
			{
				int num = (int)NativeWindow.wndProcFlags;
				if (num == 0)
				{
					if (NativeWindow.userSetProcFlags != 0)
					{
						num = (int)NativeWindow.userSetProcFlags;
					}
					else if (NativeWindow.userSetProcFlagsForApp != 0)
					{
						num = (int)NativeWindow.userSetProcFlagsForApp;
					}
					else if (!Application.CustomThreadExceptionHandlerAttached)
					{
						if (Debugger.IsAttached)
						{
							num |= 4;
						}
						else
						{
							num = NativeWindow.AdjustWndProcFlagsFromRegistry(num);
							if ((num & 2) != 0)
							{
								num = NativeWindow.AdjustWndProcFlagsFromMetadata(num);
								if ((num & 16) != 0)
								{
									if ((num & 8) != 0)
									{
										num = NativeWindow.AdjustWndProcFlagsFromConfig(num);
									}
									else
									{
										num |= 4;
									}
								}
							}
						}
					}
					num |= 1;
					NativeWindow.wndProcFlags = (byte)num;
				}
				return num;
			}
		}

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x060016C3 RID: 5827 RVA: 0x000217A7 File Offset: 0x000207A7
		internal static bool WndProcShouldBeDebuggable
		{
			get
			{
				return (NativeWindow.WndProcFlags & 4) != 0;
			}
		}

		// Token: 0x060016C4 RID: 5828 RVA: 0x000217B8 File Offset: 0x000207B8
		private static void AddWindowToTable(IntPtr handle, NativeWindow window)
		{
			lock (NativeWindow.internalSyncObject)
			{
				if (NativeWindow.handleCount >= NativeWindow.hashLoadSize)
				{
					NativeWindow.ExpandTable();
				}
				NativeWindow.anyHandleCreated = true;
				NativeWindow.anyHandleCreatedInApp = true;
				uint num2;
				uint num3;
				uint num = NativeWindow.InitHash(handle, NativeWindow.hashBuckets.Length, out num2, out num3);
				int num4 = 0;
				int num5 = -1;
				GCHandle gchandle = GCHandle.Alloc(window, GCHandleType.Weak);
				int num6;
				for (;;)
				{
					num6 = (int)(num2 % (uint)NativeWindow.hashBuckets.Length);
					if (num5 == -1 && NativeWindow.hashBuckets[num6].handle == new IntPtr(-1) && NativeWindow.hashBuckets[num6].hash_coll < 0)
					{
						num5 = num6;
					}
					if (NativeWindow.hashBuckets[num6].handle == IntPtr.Zero || (NativeWindow.hashBuckets[num6].handle == new IntPtr(-1) && ((long)NativeWindow.hashBuckets[num6].hash_coll & (long)((ulong)(-2147483648))) == 0L))
					{
						break;
					}
					if ((long)(NativeWindow.hashBuckets[num6].hash_coll & 2147483647) == (long)((ulong)num) && handle == NativeWindow.hashBuckets[num6].handle)
					{
						goto Block_11;
					}
					if (num5 == -1)
					{
						NativeWindow.HandleBucket[] array = NativeWindow.hashBuckets;
						int num7 = num6;
						array[num7].hash_coll = array[num7].hash_coll | int.MinValue;
					}
					num2 += num3;
					if (++num4 >= NativeWindow.hashBuckets.Length)
					{
						goto Block_14;
					}
				}
				if (num5 != -1)
				{
					num6 = num5;
				}
				NativeWindow.hashBuckets[num6].window = gchandle;
				NativeWindow.hashBuckets[num6].handle = handle;
				NativeWindow.HandleBucket[] array2 = NativeWindow.hashBuckets;
				int num8 = num6;
				array2[num8].hash_coll = array2[num8].hash_coll | (int)num;
				NativeWindow.handleCount++;
				return;
				Block_11:
				GCHandle window2 = NativeWindow.hashBuckets[num6].window;
				if (window2.IsAllocated)
				{
					window.previousWindow = (NativeWindow)window2.Target;
					window.previousWindow.nextWindow = window;
					window2.Free();
				}
				NativeWindow.hashBuckets[num6].window = gchandle;
				return;
				Block_14:
				if (num5 != -1)
				{
					NativeWindow.hashBuckets[num5].window = gchandle;
					NativeWindow.hashBuckets[num5].handle = handle;
					NativeWindow.HandleBucket[] array3 = NativeWindow.hashBuckets;
					int num9 = num5;
					array3[num9].hash_coll = array3[num9].hash_coll | (int)num;
					NativeWindow.handleCount++;
				}
			}
		}

		// Token: 0x060016C5 RID: 5829 RVA: 0x00021A44 File Offset: 0x00020A44
		internal static void AddWindowToIDTable(object wrapper, IntPtr handle)
		{
			NativeWindow.hashForIdHandle[NativeWindow.globalID] = handle;
			NativeWindow.hashForHandleId[handle] = NativeWindow.globalID;
			UnsafeNativeMethods.SetWindowLong(new HandleRef(wrapper, handle), -12, new HandleRef(wrapper, (IntPtr)((int)NativeWindow.globalID)));
			NativeWindow.globalID += 1;
		}

		// Token: 0x060016C6 RID: 5830 RVA: 0x00021A9D File Offset: 0x00020A9D
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static int AdjustWndProcFlagsFromConfig(int wndProcFlags)
		{
			if (WindowsFormsSection.GetSection().JitDebugging)
			{
				wndProcFlags |= 4;
			}
			return wndProcFlags;
		}

		// Token: 0x060016C7 RID: 5831 RVA: 0x00021AB4 File Offset: 0x00020AB4
		private static int AdjustWndProcFlagsFromRegistry(int wndProcFlags)
		{
			new RegistryPermission(PermissionState.Unrestricted).Assert();
			try
			{
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\.NETFramework");
				if (registryKey == null)
				{
					return wndProcFlags;
				}
				try
				{
					object value = registryKey.GetValue("DbgJITDebugLaunchSetting");
					if (value != null)
					{
						int num = 0;
						try
						{
							num = (int)value;
						}
						catch (InvalidCastException)
						{
							num = 1;
						}
						if (num != 1)
						{
							wndProcFlags |= 2;
							wndProcFlags |= 8;
						}
					}
					else if (registryKey.GetValue("DbgManagedDebugger") != null)
					{
						wndProcFlags |= 2;
						wndProcFlags |= 8;
					}
				}
				finally
				{
					registryKey.Close();
				}
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			return wndProcFlags;
		}

		// Token: 0x060016C8 RID: 5832 RVA: 0x00021B68 File Offset: 0x00020B68
		private static int AdjustWndProcFlagsFromMetadata(int wndProcFlags)
		{
			if ((wndProcFlags & 2) != 0)
			{
				Assembly entryAssembly = Assembly.GetEntryAssembly();
				if (entryAssembly != null && Attribute.IsDefined(entryAssembly, typeof(DebuggableAttribute)))
				{
					Attribute[] customAttributes = Attribute.GetCustomAttributes(entryAssembly, typeof(DebuggableAttribute));
					if (customAttributes.Length > 0)
					{
						DebuggableAttribute debuggableAttribute = (DebuggableAttribute)customAttributes[0];
						if (debuggableAttribute.IsJITTrackingEnabled)
						{
							wndProcFlags |= 16;
						}
					}
				}
			}
			return wndProcFlags;
		}

		// Token: 0x060016C9 RID: 5833 RVA: 0x00021BC4 File Offset: 0x00020BC4
		public void AssignHandle(IntPtr handle)
		{
			this.AssignHandle(handle, true);
		}

		// Token: 0x060016CA RID: 5834 RVA: 0x00021BD0 File Offset: 0x00020BD0
		internal void AssignHandle(IntPtr handle, bool assignUniqueID)
		{
			lock (this)
			{
				this.CheckReleased();
				this.handle = handle;
				if (NativeWindow.userDefWindowProc == IntPtr.Zero)
				{
					string text = ((Marshal.SystemDefaultCharSize == 1) ? "DefWindowProcA" : "DefWindowProcW");
					NativeWindow.userDefWindowProc = UnsafeNativeMethods.GetProcAddress(new HandleRef(null, UnsafeNativeMethods.GetModuleHandle("user32.dll")), text);
					if (NativeWindow.userDefWindowProc == IntPtr.Zero)
					{
						throw new Win32Exception();
					}
				}
				this.defWindowProc = UnsafeNativeMethods.GetWindowLong(new HandleRef(this, handle), -4);
				if (NativeWindow.WndProcShouldBeDebuggable)
				{
					this.windowProc = new NativeMethods.WndProc(this.DebuggableCallback);
				}
				else
				{
					this.windowProc = new NativeMethods.WndProc(this.Callback);
				}
				NativeWindow.AddWindowToTable(handle, this);
				UnsafeNativeMethods.SetWindowLong(new HandleRef(this, handle), -4, this.windowProc);
				this.windowProcPtr = UnsafeNativeMethods.GetWindowLong(new HandleRef(this, handle), -4);
				if (assignUniqueID && ((int)(long)UnsafeNativeMethods.GetWindowLong(new HandleRef(this, handle), -16) & 1073741824) != 0 && (int)(long)UnsafeNativeMethods.GetWindowLong(new HandleRef(this, handle), -12) == 0)
				{
					UnsafeNativeMethods.SetWindowLong(new HandleRef(this, handle), -12, new HandleRef(this, handle));
				}
				if (this.suppressedGC)
				{
					new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
					try
					{
						GC.ReRegisterForFinalize(this);
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
					this.suppressedGC = false;
				}
				this.OnHandleChange();
			}
		}

		// Token: 0x060016CB RID: 5835 RVA: 0x00021D74 File Offset: 0x00020D74
		private IntPtr Callback(IntPtr hWnd, int msg, IntPtr wparam, IntPtr lparam)
		{
			Message message = Message.Create(hWnd, msg, wparam, lparam);
			try
			{
				if (this.weakThisPtr.IsAlive && this.weakThisPtr.Target != null)
				{
					this.WndProc(ref message);
				}
				else
				{
					this.DefWndProc(ref message);
				}
			}
			catch (Exception ex)
			{
				this.OnThreadException(ex);
			}
			finally
			{
				if (msg == 130)
				{
					this.ReleaseHandle(false);
				}
				if (msg == NativeMethods.WM_UIUNSUBCLASS)
				{
					this.ReleaseHandle(true);
				}
			}
			return message.Result;
		}

		// Token: 0x060016CC RID: 5836 RVA: 0x00021E08 File Offset: 0x00020E08
		private void CheckReleased()
		{
			if (this.handle != IntPtr.Zero)
			{
				throw new InvalidOperationException(SR.GetString("HandleAlreadyExists"));
			}
		}

		// Token: 0x060016CD RID: 5837 RVA: 0x00021E2C File Offset: 0x00020E2C
		public virtual void CreateHandle(CreateParams cp)
		{
			IntSecurity.CreateAnyWindow.Demand();
			if ((cp.Style & 1073741824) != 1073741824 || cp.Parent == IntPtr.Zero)
			{
				IntSecurity.TopLevelWindow.Demand();
			}
			lock (this)
			{
				this.CheckReleased();
				NativeWindow.WindowClass windowClass = NativeWindow.WindowClass.Create(cp.ClassName, cp.ClassStyle);
				lock (NativeWindow.createWindowSyncObject)
				{
					if (!(this.handle != IntPtr.Zero))
					{
						windowClass.targetWindow = this;
						IntPtr moduleHandle = UnsafeNativeMethods.GetModuleHandle(null);
						IntPtr intPtr = IntPtr.Zero;
						int num = 0;
						try
						{
							if (cp.Caption != null && cp.Caption.Length > 32767)
							{
								cp.Caption = cp.Caption.Substring(0, 32767);
							}
							intPtr = UnsafeNativeMethods.CreateWindowEx(cp.ExStyle, windowClass.windowClassName, cp.Caption, cp.Style, cp.X, cp.Y, cp.Width, cp.Height, new HandleRef(cp, cp.Parent), NativeMethods.NullHandleRef, new HandleRef(null, moduleHandle), cp.Param);
							num = Marshal.GetLastWin32Error();
						}
						catch (NullReferenceException ex)
						{
							throw new OutOfMemoryException(SR.GetString("ErrorCreatingHandle"), ex);
						}
						windowClass.targetWindow = null;
						if (intPtr == IntPtr.Zero)
						{
							throw new Win32Exception(num, SR.GetString("ErrorCreatingHandle"));
						}
						this.ownHandle = true;
						global::System.Internal.HandleCollector.Add(intPtr, NativeMethods.CommonHandles.Window);
					}
				}
			}
		}

		// Token: 0x060016CE RID: 5838 RVA: 0x00022008 File Offset: 0x00021008
		private IntPtr DebuggableCallback(IntPtr hWnd, int msg, IntPtr wparam, IntPtr lparam)
		{
			Message message = Message.Create(hWnd, msg, wparam, lparam);
			try
			{
				if (this.weakThisPtr.IsAlive && this.weakThisPtr.Target != null)
				{
					this.WndProc(ref message);
				}
				else
				{
					this.DefWndProc(ref message);
				}
			}
			finally
			{
				if (msg == 130)
				{
					this.ReleaseHandle(false);
				}
				if (msg == NativeMethods.WM_UIUNSUBCLASS)
				{
					this.ReleaseHandle(true);
				}
			}
			return message.Result;
		}

		// Token: 0x060016CF RID: 5839 RVA: 0x00022084 File Offset: 0x00021084
		public void DefWndProc(ref Message m)
		{
			if (this.previousWindow != null)
			{
				m.Result = this.previousWindow.Callback(m.HWnd, m.Msg, m.WParam, m.LParam);
				return;
			}
			if (this.defWindowProc == IntPtr.Zero)
			{
				m.Result = UnsafeNativeMethods.DefWindowProc(m.HWnd, m.Msg, m.WParam, m.LParam);
				return;
			}
			m.Result = UnsafeNativeMethods.CallWindowProc(this.defWindowProc, m.HWnd, m.Msg, m.WParam, m.LParam);
		}

		// Token: 0x060016D0 RID: 5840 RVA: 0x00022124 File Offset: 0x00021124
		public virtual void DestroyHandle()
		{
			lock (this)
			{
				if (this.handle != IntPtr.Zero)
				{
					if (!UnsafeNativeMethods.DestroyWindow(new HandleRef(this, this.handle)))
					{
						this.UnSubclass();
						UnsafeNativeMethods.PostMessage(new HandleRef(this, this.handle), 16, 0, 0);
					}
					this.handle = IntPtr.Zero;
					this.ownHandle = false;
				}
				new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
				try
				{
					GC.SuppressFinalize(this);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				this.suppressedGC = true;
			}
		}

		// Token: 0x060016D1 RID: 5841 RVA: 0x000221D4 File Offset: 0x000211D4
		private static void ExpandTable()
		{
			int num = NativeWindow.hashBuckets.Length;
			int prime = NativeWindow.GetPrime(1 + num * 2);
			NativeWindow.HandleBucket[] array = new NativeWindow.HandleBucket[prime];
			for (int i = 0; i < num; i++)
			{
				NativeWindow.HandleBucket handleBucket = NativeWindow.hashBuckets[i];
				if (handleBucket.handle != IntPtr.Zero && handleBucket.handle != new IntPtr(-1))
				{
					uint num2 = (uint)(handleBucket.hash_coll & int.MaxValue);
					uint num3 = 1U + ((num2 >> 5) + 1U) % (uint)(array.Length - 1);
					int num4;
					for (;;)
					{
						num4 = (int)(num2 % (uint)array.Length);
						if (array[num4].handle == IntPtr.Zero || array[num4].handle == new IntPtr(-1))
						{
							break;
						}
						NativeWindow.HandleBucket[] array2 = array;
						int num5 = num4;
						array2[num5].hash_coll = array2[num5].hash_coll | int.MinValue;
						num2 += num3;
					}
					array[num4].window = handleBucket.window;
					array[num4].handle = handleBucket.handle;
					NativeWindow.HandleBucket[] array3 = array;
					int num6 = num4;
					array3[num6].hash_coll = array3[num6].hash_coll | (handleBucket.hash_coll & int.MaxValue);
				}
			}
			NativeWindow.hashBuckets = array;
			NativeWindow.hashLoadSize = (int)(0.72f * (float)prime);
			if (NativeWindow.hashLoadSize >= prime)
			{
				NativeWindow.hashLoadSize = prime - 1;
			}
		}

		// Token: 0x060016D2 RID: 5842 RVA: 0x0002233A File Offset: 0x0002133A
		public static NativeWindow FromHandle(IntPtr handle)
		{
			if (handle != IntPtr.Zero && NativeWindow.handleCount > 0)
			{
				return NativeWindow.GetWindowFromTable(handle);
			}
			return null;
		}

		// Token: 0x060016D3 RID: 5843 RVA: 0x0002235C File Offset: 0x0002135C
		private static int GetPrime(int minSize)
		{
			if (minSize < 0)
			{
				throw new OutOfMemoryException();
			}
			for (int i = 0; i < NativeWindow.primes.Length; i++)
			{
				int num = NativeWindow.primes[i];
				if (num >= minSize)
				{
					return num;
				}
			}
			for (int j = (minSize - 2) | 1; j < 2147483647; j += 2)
			{
				bool flag = true;
				if ((j & 1) != 0)
				{
					int num2 = (int)Math.Sqrt((double)j);
					for (int k = 3; k < num2; k += 2)
					{
						if (j % k == 0)
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						return j;
					}
				}
				else if (j == 2)
				{
					return j;
				}
			}
			return minSize;
		}

		// Token: 0x060016D4 RID: 5844 RVA: 0x000223E0 File Offset: 0x000213E0
		private static NativeWindow GetWindowFromTable(IntPtr handle)
		{
			NativeWindow.HandleBucket[] array = NativeWindow.hashBuckets;
			int num = 0;
			uint num3;
			uint num4;
			uint num2 = NativeWindow.InitHash(handle, array.Length, out num3, out num4);
			NativeWindow.HandleBucket handleBucket;
			for (;;)
			{
				int num5 = (int)(num3 % (uint)array.Length);
				handleBucket = array[num5];
				if (handleBucket.handle == IntPtr.Zero)
				{
					break;
				}
				if ((long)(handleBucket.hash_coll & 2147483647) == (long)((ulong)num2) && handle == handleBucket.handle && handleBucket.window.IsAllocated)
				{
					goto Block_4;
				}
				num3 += num4;
				if (handleBucket.hash_coll >= 0 || ++num >= array.Length)
				{
					goto IL_009F;
				}
			}
			return null;
			Block_4:
			return (NativeWindow)handleBucket.window.Target;
			IL_009F:
			return null;
		}

		// Token: 0x060016D5 RID: 5845 RVA: 0x00022490 File Offset: 0x00021490
		internal IntPtr GetHandleFromID(short id)
		{
			IntPtr zero;
			if (NativeWindow.hashForIdHandle == null || !NativeWindow.hashForIdHandle.TryGetValue(id, out zero))
			{
				zero = IntPtr.Zero;
			}
			return zero;
		}

		// Token: 0x060016D6 RID: 5846 RVA: 0x000224BC File Offset: 0x000214BC
		private static uint InitHash(IntPtr handle, int hashsize, out uint seed, out uint incr)
		{
			uint num = (uint)(handle.GetHashCode() & int.MaxValue);
			seed = num;
			incr = 1U + ((seed >> 5) + 1U) % (uint)(hashsize - 1);
			return num;
		}

		// Token: 0x060016D7 RID: 5847 RVA: 0x000224EF File Offset: 0x000214EF
		protected virtual void OnHandleChange()
		{
		}

		// Token: 0x060016D8 RID: 5848 RVA: 0x000224F4 File Offset: 0x000214F4
		[PrePrepareMethod]
		private static void OnShutdown(object sender, EventArgs e)
		{
			if (NativeWindow.handleCount > 0)
			{
				lock (NativeWindow.internalSyncObject)
				{
					for (int i = 0; i < NativeWindow.hashBuckets.Length; i++)
					{
						NativeWindow.HandleBucket handleBucket = NativeWindow.hashBuckets[i];
						if (handleBucket.handle != IntPtr.Zero && handleBucket.handle != new IntPtr(-1))
						{
							HandleRef handleRef = new HandleRef(handleBucket, handleBucket.handle);
							UnsafeNativeMethods.SetWindowLong(handleRef, -4, new HandleRef(null, NativeWindow.userDefWindowProc));
							UnsafeNativeMethods.SetClassLong(handleRef, -24, NativeWindow.userDefWindowProc);
							UnsafeNativeMethods.PostMessage(handleRef, 16, 0, 0);
							if (handleBucket.window.IsAllocated)
							{
								NativeWindow nativeWindow = (NativeWindow)handleBucket.window.Target;
								if (nativeWindow != null)
								{
									nativeWindow.handle = IntPtr.Zero;
								}
							}
							handleBucket.window.Free();
						}
						NativeWindow.hashBuckets[i].handle = IntPtr.Zero;
						NativeWindow.hashBuckets[i].hash_coll = 0;
					}
					NativeWindow.handleCount = 0;
				}
			}
			NativeWindow.WindowClass.DisposeCache();
		}

		// Token: 0x060016D9 RID: 5849 RVA: 0x00022634 File Offset: 0x00021634
		protected virtual void OnThreadException(Exception e)
		{
		}

		// Token: 0x060016DA RID: 5850 RVA: 0x00022636 File Offset: 0x00021636
		public virtual void ReleaseHandle()
		{
			this.ReleaseHandle(true);
		}

		// Token: 0x060016DB RID: 5851 RVA: 0x00022640 File Offset: 0x00021640
		private void ReleaseHandle(bool handleValid)
		{
			if (this.handle != IntPtr.Zero)
			{
				lock (this)
				{
					if (this.handle != IntPtr.Zero)
					{
						if (handleValid)
						{
							this.UnSubclass();
						}
						NativeWindow.RemoveWindowFromTable(this.handle, this);
						if (this.ownHandle)
						{
							global::System.Internal.HandleCollector.Remove(this.handle, NativeMethods.CommonHandles.Window);
							this.ownHandle = false;
						}
						this.handle = IntPtr.Zero;
						if (this.weakThisPtr.IsAlive && this.weakThisPtr.Target != null)
						{
							this.OnHandleChange();
							new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
							try
							{
								GC.SuppressFinalize(this);
							}
							finally
							{
								CodeAccessPermission.RevertAssert();
							}
							this.suppressedGC = true;
						}
					}
				}
			}
		}

		// Token: 0x060016DC RID: 5852 RVA: 0x00022724 File Offset: 0x00021724
		private static void RemoveWindowFromTable(IntPtr handle, NativeWindow window)
		{
			lock (NativeWindow.internalSyncObject)
			{
				uint num2;
				uint num3;
				uint num = NativeWindow.InitHash(handle, NativeWindow.hashBuckets.Length, out num2, out num3);
				int num4 = 0;
				NativeWindow nativeWindow = window.PreviousWindow;
				int num5;
				for (;;)
				{
					num5 = (int)(num2 % (uint)NativeWindow.hashBuckets.Length);
					NativeWindow.HandleBucket handleBucket = NativeWindow.hashBuckets[num5];
					if ((long)(handleBucket.hash_coll & 2147483647) == (long)((ulong)num) && handle == handleBucket.handle)
					{
						break;
					}
					num2 += num3;
					if (NativeWindow.hashBuckets[num5].hash_coll >= 0 || ++num4 >= NativeWindow.hashBuckets.Length)
					{
						goto IL_01ED;
					}
				}
				bool flag = window.nextWindow == null;
				bool flag2 = NativeWindow.IsRootWindowInListWithChildren(window);
				if (window.previousWindow != null)
				{
					window.previousWindow.nextWindow = window.nextWindow;
				}
				if (window.nextWindow != null)
				{
					window.nextWindow.defWindowProc = window.defWindowProc;
					window.nextWindow.previousWindow = window.previousWindow;
				}
				window.nextWindow = null;
				window.previousWindow = null;
				if (flag2)
				{
					if (NativeWindow.hashBuckets[num5].window.IsAllocated)
					{
						NativeWindow.hashBuckets[num5].window.Free();
					}
					NativeWindow.hashBuckets[num5].window = GCHandle.Alloc(nativeWindow, GCHandleType.Weak);
				}
				else if (flag)
				{
					NativeWindow.HandleBucket[] array = NativeWindow.hashBuckets;
					int num6 = num5;
					array[num6].hash_coll = array[num6].hash_coll & int.MinValue;
					if (NativeWindow.hashBuckets[num5].hash_coll != 0)
					{
						NativeWindow.hashBuckets[num5].handle = new IntPtr(-1);
					}
					else
					{
						NativeWindow.hashBuckets[num5].handle = IntPtr.Zero;
					}
					if (NativeWindow.hashBuckets[num5].window.IsAllocated)
					{
						NativeWindow.hashBuckets[num5].window.Free();
					}
					NativeWindow.handleCount--;
				}
				IL_01ED:;
			}
		}

		// Token: 0x060016DD RID: 5853 RVA: 0x00022944 File Offset: 0x00021944
		private static bool IsRootWindowInListWithChildren(NativeWindow window)
		{
			return window.PreviousWindow != null && window.nextWindow == null;
		}

		// Token: 0x060016DE RID: 5854 RVA: 0x0002295C File Offset: 0x0002195C
		internal static void RemoveWindowFromIDTable(IntPtr handle)
		{
			short num = NativeWindow.hashForHandleId[handle];
			NativeWindow.hashForHandleId.Remove(handle);
			NativeWindow.hashForIdHandle.Remove(num);
		}

		// Token: 0x060016DF RID: 5855 RVA: 0x00022990 File Offset: 0x00021990
		internal static void SetUnhandledExceptionModeInternal(UnhandledExceptionMode mode, bool threadScope)
		{
			if (!threadScope && NativeWindow.anyHandleCreatedInApp)
			{
				throw new InvalidOperationException(SR.GetString("ApplicationCannotChangeApplicationExceptionMode"));
			}
			if (threadScope && NativeWindow.anyHandleCreated)
			{
				throw new InvalidOperationException(SR.GetString("ApplicationCannotChangeThreadExceptionMode"));
			}
			switch (mode)
			{
			case UnhandledExceptionMode.Automatic:
				if (threadScope)
				{
					NativeWindow.userSetProcFlags = 0;
					return;
				}
				NativeWindow.userSetProcFlagsForApp = 0;
				return;
			case UnhandledExceptionMode.ThrowException:
				if (threadScope)
				{
					NativeWindow.userSetProcFlags = 5;
					return;
				}
				NativeWindow.userSetProcFlagsForApp = 5;
				return;
			case UnhandledExceptionMode.CatchException:
				if (threadScope)
				{
					NativeWindow.userSetProcFlags = 1;
					return;
				}
				NativeWindow.userSetProcFlagsForApp = 1;
				return;
			default:
				throw new InvalidEnumArgumentException("mode", (int)mode, typeof(UnhandledExceptionMode));
			}
		}

		// Token: 0x060016E0 RID: 5856 RVA: 0x00022A30 File Offset: 0x00021A30
		private void UnSubclass()
		{
			bool flag = !this.weakThisPtr.IsAlive || this.weakThisPtr.Target == null;
			HandleRef handleRef = new HandleRef(this, this.handle);
			IntPtr windowLong = UnsafeNativeMethods.GetWindowLong(new HandleRef(this, this.handle), -4);
			if (!(this.windowProcPtr == windowLong))
			{
				if (this.nextWindow == null || this.nextWindow.defWindowProc != this.windowProcPtr)
				{
					UnsafeNativeMethods.SetWindowLong(handleRef, -4, new HandleRef(this, NativeWindow.userDefWindowProc));
				}
				return;
			}
			if (this.previousWindow == null)
			{
				UnsafeNativeMethods.SetWindowLong(handleRef, -4, new HandleRef(this, this.defWindowProc));
				return;
			}
			if (flag)
			{
				UnsafeNativeMethods.SetWindowLong(handleRef, -4, new HandleRef(this, NativeWindow.userDefWindowProc));
				return;
			}
			UnsafeNativeMethods.SetWindowLong(handleRef, -4, this.previousWindow.windowProc);
		}

		// Token: 0x060016E1 RID: 5857 RVA: 0x00022B0B File Offset: 0x00021B0B
		protected virtual void WndProc(ref Message m)
		{
			this.DefWndProc(ref m);
		}

		// Token: 0x0400113E RID: 4414
		private const int InitializedFlags = 1;

		// Token: 0x0400113F RID: 4415
		private const int DebuggerPresent = 2;

		// Token: 0x04001140 RID: 4416
		private const int UseDebuggableWndProc = 4;

		// Token: 0x04001141 RID: 4417
		private const int LoadConfigSettings = 8;

		// Token: 0x04001142 RID: 4418
		private const int AssemblyIsDebuggable = 16;

		// Token: 0x04001143 RID: 4419
		private const float hashLoadFactor = 0.72f;

		// Token: 0x04001144 RID: 4420
		private static readonly TraceSwitch WndProcChoice;

		// Token: 0x04001145 RID: 4421
		private static readonly int[] primes = new int[]
		{
			11, 17, 23, 29, 37, 47, 59, 71, 89, 107,
			131, 163, 197, 239, 293, 353, 431, 521, 631, 761,
			919, 1103, 1327, 1597, 1931, 2333, 2801, 3371, 4049, 4861,
			5839, 7013, 8419, 10103, 12143, 14591, 17519, 21023, 25229, 30293,
			36353, 43627, 52361, 62851, 75431, 90523, 108631, 130363, 156437, 187751,
			225307, 270371, 324449, 389357, 467237, 560689, 672827, 807403, 968897, 1162687,
			1395263, 1674319, 2009191, 2411033, 2893249, 3471899, 4166287, 4999559, 5999471, 7199369
		};

		// Token: 0x04001146 RID: 4422
		[ThreadStatic]
		private static bool anyHandleCreated;

		// Token: 0x04001147 RID: 4423
		private static bool anyHandleCreatedInApp;

		// Token: 0x04001148 RID: 4424
		private static int handleCount;

		// Token: 0x04001149 RID: 4425
		private static int hashLoadSize;

		// Token: 0x0400114A RID: 4426
		private static NativeWindow.HandleBucket[] hashBuckets;

		// Token: 0x0400114B RID: 4427
		private static IntPtr userDefWindowProc;

		// Token: 0x0400114C RID: 4428
		[ThreadStatic]
		private static byte wndProcFlags = 0;

		// Token: 0x0400114D RID: 4429
		[ThreadStatic]
		private static byte userSetProcFlags = 0;

		// Token: 0x0400114E RID: 4430
		private static byte userSetProcFlagsForApp;

		// Token: 0x0400114F RID: 4431
		private static short globalID = 1;

		// Token: 0x04001150 RID: 4432
		private static Dictionary<short, IntPtr> hashForIdHandle;

		// Token: 0x04001151 RID: 4433
		private static Dictionary<IntPtr, short> hashForHandleId;

		// Token: 0x04001152 RID: 4434
		private static object internalSyncObject = new object();

		// Token: 0x04001153 RID: 4435
		private static object createWindowSyncObject = new object();

		// Token: 0x04001154 RID: 4436
		private IntPtr handle;

		// Token: 0x04001155 RID: 4437
		private NativeMethods.WndProc windowProc;

		// Token: 0x04001156 RID: 4438
		private IntPtr windowProcPtr;

		// Token: 0x04001157 RID: 4439
		private IntPtr defWindowProc;

		// Token: 0x04001158 RID: 4440
		private bool suppressedGC;

		// Token: 0x04001159 RID: 4441
		private bool ownHandle;

		// Token: 0x0400115A RID: 4442
		private NativeWindow previousWindow;

		// Token: 0x0400115B RID: 4443
		private NativeWindow nextWindow;

		// Token: 0x0400115C RID: 4444
		private WeakReference weakThisPtr;

		// Token: 0x020001EE RID: 494
		private struct HandleBucket
		{
			// Token: 0x0400115D RID: 4445
			public IntPtr handle;

			// Token: 0x0400115E RID: 4446
			public GCHandle window;

			// Token: 0x0400115F RID: 4447
			public int hash_coll;
		}

		// Token: 0x020001EF RID: 495
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		private class WindowClass
		{
			// Token: 0x060016E2 RID: 5858 RVA: 0x00022B14 File Offset: 0x00021B14
			internal WindowClass(string className, int classStyle)
			{
				this.className = className;
				this.classStyle = classStyle;
				this.RegisterClass();
			}

			// Token: 0x060016E3 RID: 5859 RVA: 0x00022B30 File Offset: 0x00021B30
			public IntPtr Callback(IntPtr hWnd, int msg, IntPtr wparam, IntPtr lparam)
			{
				UnsafeNativeMethods.SetWindowLong(new HandleRef(null, hWnd), -4, new HandleRef(this, this.defWindowProc));
				this.targetWindow.AssignHandle(hWnd);
				return this.targetWindow.Callback(hWnd, msg, wparam, lparam);
			}

			// Token: 0x060016E4 RID: 5860 RVA: 0x00022B6C File Offset: 0x00021B6C
			internal static NativeWindow.WindowClass Create(string className, int classStyle)
			{
				NativeWindow.WindowClass windowClass2;
				lock (NativeWindow.WindowClass.wcInternalSyncObject)
				{
					NativeWindow.WindowClass windowClass = NativeWindow.WindowClass.cache;
					if (className == null)
					{
						while (windowClass != null)
						{
							if (windowClass.className == null && windowClass.classStyle == classStyle)
							{
								break;
							}
							windowClass = windowClass.next;
						}
					}
					else
					{
						while (windowClass != null && !className.Equals(windowClass.className))
						{
							windowClass = windowClass.next;
						}
					}
					if (windowClass == null)
					{
						windowClass = new NativeWindow.WindowClass(className, classStyle);
						windowClass.next = NativeWindow.WindowClass.cache;
						NativeWindow.WindowClass.cache = windowClass;
					}
					else if (!windowClass.registered)
					{
						windowClass.RegisterClass();
					}
					windowClass2 = windowClass;
				}
				return windowClass2;
			}

			// Token: 0x060016E5 RID: 5861 RVA: 0x00022C10 File Offset: 0x00021C10
			internal static void DisposeCache()
			{
				lock (NativeWindow.WindowClass.wcInternalSyncObject)
				{
					for (NativeWindow.WindowClass windowClass = NativeWindow.WindowClass.cache; windowClass != null; windowClass = windowClass.next)
					{
						windowClass.UnregisterClass();
					}
				}
			}

			// Token: 0x060016E6 RID: 5862 RVA: 0x00022C5C File Offset: 0x00021C5C
			private string GetFullClassName(string className)
			{
				StringBuilder stringBuilder = new StringBuilder(50);
				stringBuilder.Append(Application.WindowsFormsVersion);
				stringBuilder.Append('.');
				stringBuilder.Append(className);
				stringBuilder.Append(".app.");
				stringBuilder.Append(NativeWindow.WindowClass.domainQualifier);
				stringBuilder.Append('.');
				stringBuilder.Append(Convert.ToString(AppDomain.CurrentDomain.GetHashCode(), 16));
				return stringBuilder.ToString();
			}

			// Token: 0x060016E7 RID: 5863 RVA: 0x00022CD0 File Offset: 0x00021CD0
			private void RegisterClass()
			{
				NativeMethods.WNDCLASS_D wndclass_D = new NativeMethods.WNDCLASS_D();
				if (NativeWindow.userDefWindowProc == IntPtr.Zero)
				{
					string text = ((Marshal.SystemDefaultCharSize == 1) ? "DefWindowProcA" : "DefWindowProcW");
					NativeWindow.userDefWindowProc = UnsafeNativeMethods.GetProcAddress(new HandleRef(null, UnsafeNativeMethods.GetModuleHandle("user32.dll")), text);
					if (NativeWindow.userDefWindowProc == IntPtr.Zero)
					{
						throw new Win32Exception();
					}
				}
				string text2;
				if (this.className == null)
				{
					wndclass_D.hbrBackground = UnsafeNativeMethods.GetStockObject(5);
					wndclass_D.style = this.classStyle;
					this.defWindowProc = NativeWindow.userDefWindowProc;
					text2 = "Window." + Convert.ToString(this.classStyle, 16);
					this.hashCode = 0;
				}
				else
				{
					NativeMethods.WNDCLASS_I wndclass_I = new NativeMethods.WNDCLASS_I();
					bool classInfo = UnsafeNativeMethods.GetClassInfo(NativeMethods.NullHandleRef, this.className, wndclass_I);
					int lastWin32Error = Marshal.GetLastWin32Error();
					if (!classInfo)
					{
						throw new Win32Exception(lastWin32Error, SR.GetString("InvalidWndClsName"));
					}
					wndclass_D.style = wndclass_I.style;
					wndclass_D.cbClsExtra = wndclass_I.cbClsExtra;
					wndclass_D.cbWndExtra = wndclass_I.cbWndExtra;
					wndclass_D.hIcon = wndclass_I.hIcon;
					wndclass_D.hCursor = wndclass_I.hCursor;
					wndclass_D.hbrBackground = wndclass_I.hbrBackground;
					wndclass_D.lpszMenuName = Marshal.PtrToStringAuto(wndclass_I.lpszMenuName);
					text2 = this.className;
					this.defWindowProc = wndclass_I.lpfnWndProc;
					this.hashCode = this.className.GetHashCode();
				}
				this.windowClassName = this.GetFullClassName(text2);
				this.windowProc = new NativeMethods.WndProc(this.Callback);
				wndclass_D.lpfnWndProc = this.windowProc;
				wndclass_D.hInstance = UnsafeNativeMethods.GetModuleHandle(null);
				wndclass_D.lpszClassName = this.windowClassName;
				short num = UnsafeNativeMethods.RegisterClass(wndclass_D);
				if (num == 0)
				{
					int lastWin32Error2 = Marshal.GetLastWin32Error();
					if (lastWin32Error2 == 1410)
					{
						NativeMethods.WNDCLASS_I wndclass_I2 = new NativeMethods.WNDCLASS_I();
						bool classInfo2 = UnsafeNativeMethods.GetClassInfo(new HandleRef(null, UnsafeNativeMethods.GetModuleHandle(null)), this.windowClassName, wndclass_I2);
						if (classInfo2 && wndclass_I2.lpfnWndProc == NativeWindow.UserDefindowProc)
						{
							if (UnsafeNativeMethods.UnregisterClass(this.windowClassName, new HandleRef(null, UnsafeNativeMethods.GetModuleHandle(null))))
							{
								num = UnsafeNativeMethods.RegisterClass(wndclass_D);
							}
							else
							{
								do
								{
									NativeWindow.WindowClass.domainQualifier++;
									this.windowClassName = this.GetFullClassName(text2);
									wndclass_D.lpszClassName = this.windowClassName;
									num = UnsafeNativeMethods.RegisterClass(wndclass_D);
								}
								while (num == 0 && Marshal.GetLastWin32Error() == 1410);
							}
						}
					}
					if (num == 0)
					{
						this.windowProc = null;
						throw new Win32Exception(lastWin32Error2);
					}
				}
				this.registered = true;
			}

			// Token: 0x060016E8 RID: 5864 RVA: 0x00022F57 File Offset: 0x00021F57
			private void UnregisterClass()
			{
				if (this.registered && UnsafeNativeMethods.UnregisterClass(this.windowClassName, new HandleRef(null, UnsafeNativeMethods.GetModuleHandle(null))))
				{
					this.windowProc = null;
					this.registered = false;
				}
			}

			// Token: 0x04001160 RID: 4448
			internal static NativeWindow.WindowClass cache;

			// Token: 0x04001161 RID: 4449
			internal NativeWindow.WindowClass next;

			// Token: 0x04001162 RID: 4450
			internal string className;

			// Token: 0x04001163 RID: 4451
			internal int classStyle;

			// Token: 0x04001164 RID: 4452
			internal string windowClassName;

			// Token: 0x04001165 RID: 4453
			internal int hashCode;

			// Token: 0x04001166 RID: 4454
			internal IntPtr defWindowProc;

			// Token: 0x04001167 RID: 4455
			internal NativeMethods.WndProc windowProc;

			// Token: 0x04001168 RID: 4456
			internal bool registered;

			// Token: 0x04001169 RID: 4457
			internal NativeWindow targetWindow;

			// Token: 0x0400116A RID: 4458
			private static object wcInternalSyncObject = new object();

			// Token: 0x0400116B RID: 4459
			private static int domainQualifier = 0;
		}
	}
}
