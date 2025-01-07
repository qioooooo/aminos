using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Messaging;
using System.Security;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.VisualBasic.Devices;
using Microsoft.Win32.SafeHandles;

namespace Microsoft.VisualBasic.ApplicationServices
{
	[HostProtection(SecurityAction.LinkDemand, Resources = HostProtectionResource.ExternalProcessMgmt)]
	public class WindowsFormsApplicationBase : ConsoleApplicationBase
	{
		public event StartupEventHandler Startup;

		public event StartupNextInstanceEventHandler StartupNextInstance;

		public event ShutdownEventHandler Shutdown;

		public event NetworkAvailableEventHandler NetworkAvailabilityChanged
		{
			add
			{
				object networkAvailChangeLock = this.m_NetworkAvailChangeLock;
				ObjectFlowControl.CheckForSyncLockOnValueType(networkAvailChangeLock);
				lock (networkAvailChangeLock)
				{
					if (this.m_NetworkAvailabilityEventHandlers == null)
					{
						this.m_NetworkAvailabilityEventHandlers = new ArrayList();
					}
					this.m_NetworkAvailabilityEventHandlers.Add(value);
					this.m_TurnOnNetworkListener = true;
					if ((this.m_NetworkObject == null) & this.m_FinishedOnInitilaize)
					{
						this.m_NetworkObject = new Network();
						this.m_NetworkObject.NetworkAvailabilityChanged += this.NetworkAvailableEventAdaptor;
					}
				}
			}
			remove
			{
				if (this.m_NetworkAvailabilityEventHandlers != null && this.m_NetworkAvailabilityEventHandlers.Count > 0)
				{
					this.m_NetworkAvailabilityEventHandlers.Remove(value);
					if (this.m_NetworkAvailabilityEventHandlers.Count == 0)
					{
						this.m_NetworkObject.NetworkAvailabilityChanged -= this.NetworkAvailableEventAdaptor;
						if (this.m_NetworkObject != null)
						{
							this.m_NetworkObject.DisconnectListener();
							this.m_NetworkObject = null;
						}
					}
				}
			}
		}

		// Note: this method is marked as 'fire'.
		private void raise_NetworkAvailabilityChanged(object sender, NetworkAvailableEventArgs e)
		{
			if (this.m_NetworkAvailabilityEventHandlers != null)
			{
				try
				{
					foreach (object obj in this.m_NetworkAvailabilityEventHandlers)
					{
						NetworkAvailableEventHandler networkAvailableEventHandler = (NetworkAvailableEventHandler)obj;
						try
						{
							if (networkAvailableEventHandler != null)
							{
								networkAvailableEventHandler(sender, e);
							}
						}
						catch (Exception ex)
						{
							if (!this.OnUnhandledException(new UnhandledExceptionEventArgs(true, ex)))
							{
								throw;
							}
						}
					}
				}
				finally
				{
					IEnumerator enumerator;
					if (enumerator is IDisposable)
					{
						(enumerator as IDisposable).Dispose();
					}
				}
			}
		}

		public event UnhandledExceptionEventHandler UnhandledException
		{
			add
			{
				if (this.m_UnhandledExceptionHandlers == null)
				{
					this.m_UnhandledExceptionHandlers = new ArrayList();
				}
				this.m_UnhandledExceptionHandlers.Add(value);
				if (this.m_UnhandledExceptionHandlers.Count == 1)
				{
					Application.ThreadException += this.OnUnhandledExceptionEventAdaptor;
				}
			}
			remove
			{
				if (this.m_UnhandledExceptionHandlers != null && this.m_UnhandledExceptionHandlers.Count > 0)
				{
					this.m_UnhandledExceptionHandlers.Remove(value);
					if (this.m_UnhandledExceptionHandlers.Count == 0)
					{
						Application.ThreadException -= this.OnUnhandledExceptionEventAdaptor;
					}
				}
			}
		}

		// Note: this method is marked as 'fire'.
		private void raise_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			if (this.m_UnhandledExceptionHandlers != null)
			{
				this.m_ProcessingUnhandledExceptionEvent = true;
				try
				{
					foreach (object obj in this.m_UnhandledExceptionHandlers)
					{
						UnhandledExceptionEventHandler unhandledExceptionEventHandler = (UnhandledExceptionEventHandler)obj;
						if (unhandledExceptionEventHandler != null)
						{
							unhandledExceptionEventHandler(sender, e);
						}
					}
				}
				finally
				{
					IEnumerator enumerator;
					if (enumerator is IDisposable)
					{
						(enumerator as IDisposable).Dispose();
					}
				}
				this.m_ProcessingUnhandledExceptionEvent = false;
			}
		}

		public WindowsFormsApplicationBase()
			: this(AuthenticationMode.Windows)
		{
		}

		public WindowsFormsApplicationBase(AuthenticationMode authenticationMode)
		{
			this.m_MinimumSplashExposure = 2000;
			this.m_SplashLock = new object();
			this.m_NetworkAvailChangeLock = new object();
			this.m_Ok2CloseSplashScreen = true;
			this.ValidateAuthenticationModeEnumValue(authenticationMode, "authenticationMode");
			if (authenticationMode == AuthenticationMode.Windows)
			{
				try
				{
					Thread.CurrentPrincipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
				}
				catch (SecurityException ex)
				{
				}
			}
			this.m_AppContext = new WindowsFormsApplicationBase.WinFormsAppContext(this);
			new UIPermission(UIPermissionWindow.AllWindows).Assert();
			this.m_AppSyncronizationContext = AsyncOperationManager.SynchronizationContext;
			AsyncOperationManager.SynchronizationContext = new WindowsFormsSynchronizationContext();
			PermissionSet.RevertAssert();
		}

		public void Run(string[] commandLine)
		{
			base.InternalCommandLine = new ReadOnlyCollection<string>(commandLine);
			if (!this.IsSingleInstance)
			{
				this.DoApplicationModel();
			}
			else
			{
				string applicationInstanceID = this.GetApplicationInstanceID(Assembly.GetCallingAssembly());
				this.m_MemoryMappedID = applicationInstanceID + "Map";
				string text = applicationInstanceID + "Event";
				string text2 = applicationInstanceID + "Event2";
				this.m_StartNextInstanceCallback = new SendOrPostCallback(this.OnStartupNextInstanceMarshallingAdaptor);
				new SecurityPermission(SecurityPermissionFlag.ControlPrincipal).Assert();
				string name = WindowsIdentity.GetCurrent().Name;
				bool flag = Operators.CompareString(name, "", false) != 0;
				CodeAccessPermission.RevertAssert();
				bool flag2;
				if (flag)
				{
					EventWaitHandleAccessRule eventWaitHandleAccessRule = new EventWaitHandleAccessRule(name, EventWaitHandleRights.FullControl, AccessControlType.Allow);
					EventWaitHandleSecurity eventWaitHandleSecurity = new EventWaitHandleSecurity();
					eventWaitHandleSecurity.AddAccessRule(eventWaitHandleAccessRule);
					this.m_FirstInstanceSemaphore = new EventWaitHandle(false, EventResetMode.ManualReset, text, out flag2, eventWaitHandleSecurity);
					bool flag3 = false;
					EventResetMode eventResetMode = EventResetMode.AutoReset;
					string text3 = text2;
					bool flag4 = false;
					this.m_MessageRecievedSemaphore = new EventWaitHandle(flag3, eventResetMode, text3, out flag4, eventWaitHandleSecurity);
				}
				else
				{
					this.m_FirstInstanceSemaphore = new EventWaitHandle(false, EventResetMode.ManualReset, text, out flag2);
					this.m_MessageRecievedSemaphore = new EventWaitHandle(false, EventResetMode.AutoReset, text2);
				}
				if (flag2)
				{
					try
					{
						TcpChannel tcpChannel = this.RegisterChannel(flag);
						WindowsFormsApplicationBase.RemoteCommunicator remoteCommunicator = new WindowsFormsApplicationBase.RemoteCommunicator(this, this.m_MessageRecievedSemaphore);
						string text4 = applicationInstanceID + ".rem";
						new SecurityPermission(SecurityPermissionFlag.RemotingConfiguration).Assert();
						RemotingServices.Marshal(remoteCommunicator, text4);
						CodeAccessPermission.RevertAssert();
						string text5 = tcpChannel.GetUrlsForUri(text4)[0];
						this.WriteUrlToMemoryMappedFile(text5);
						this.m_FirstInstanceSemaphore.Set();
						this.DoApplicationModel();
						return;
					}
					finally
					{
						if (this.m_MessageRecievedSemaphore != null)
						{
							this.m_MessageRecievedSemaphore.Close();
						}
						if (this.m_FirstInstanceSemaphore != null)
						{
							this.m_FirstInstanceSemaphore.Close();
						}
						if (this.m_FirstInstanceMemoryMappedFileHandle != null && !this.m_FirstInstanceMemoryMappedFileHandle.IsInvalid)
						{
							this.m_FirstInstanceMemoryMappedFileHandle.Close();
						}
					}
				}
				if (!this.m_FirstInstanceSemaphore.WaitOne(1000, false))
				{
					throw new CantStartSingleInstanceException();
				}
				this.RegisterChannel(flag);
				string text6 = this.ReadUrlFromMemoryMappedFile();
				if (text6 == null)
				{
					throw new CantStartSingleInstanceException();
				}
				WindowsFormsApplicationBase.RemoteCommunicator remoteCommunicator2 = (WindowsFormsApplicationBase.RemoteCommunicator)RemotingServices.Connect(typeof(WindowsFormsApplicationBase.RemoteCommunicator), text6);
				PermissionSet permissionSet = new PermissionSet(PermissionState.None);
				permissionSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.UnmanagedCode | SecurityPermissionFlag.SerializationFormatter | SecurityPermissionFlag.ControlPrincipal));
				permissionSet.AddPermission(new DnsPermission(PermissionState.Unrestricted));
				permissionSet.AddPermission(new SocketPermission(NetworkAccess.Connect, TransportType.Tcp, this.HostName, -1));
				permissionSet.AddPermission(new EnvironmentPermission(EnvironmentPermissionAccess.Read, "USERNAME"));
				permissionSet.Assert();
				remoteCommunicator2.RunNextInstance(base.CommandLineArgs);
				PermissionSet.RevertAssert();
				if (!this.m_MessageRecievedSemaphore.WaitOne(2500, false))
				{
					throw new CantStartSingleInstanceException();
				}
			}
		}

		public FormCollection OpenForms
		{
			get
			{
				return Application.OpenForms;
			}
		}

		protected Form MainForm
		{
			get
			{
				return Interaction.IIf<Form>(this.m_AppContext != null, this.m_AppContext.MainForm, null);
			}
			set
			{
				if (value == null)
				{
					throw ExceptionUtils.GetArgumentNullException("MainForm", "General_PropertyNothing", new string[] { "MainForm" });
				}
				if (value == this.m_SplashScreen)
				{
					throw new ArgumentException(Utils.GetResourceString("AppModel_SplashAndMainFormTheSame"));
				}
				this.m_AppContext.MainForm = value;
			}
		}

		public Form SplashScreen
		{
			get
			{
				return this.m_SplashScreen;
			}
			set
			{
				if (value != null && value == this.m_AppContext.MainForm)
				{
					throw new ArgumentException(Utils.GetResourceString("AppModel_SplashAndMainFormTheSame"));
				}
				this.m_SplashScreen = value;
			}
		}

		public int MinimumSplashScreenDisplayTime
		{
			get
			{
				return this.m_MinimumSplashExposure;
			}
			set
			{
				this.m_MinimumSplashExposure = value;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected static bool UseCompatibleTextRendering
		{
			get
			{
				return false;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public ApplicationContext ApplicationContext
		{
			get
			{
				return this.m_AppContext;
			}
		}

		public bool SaveMySettingsOnExit
		{
			get
			{
				return this.m_SaveMySettingsOnExit;
			}
			set
			{
				this.m_SaveMySettingsOnExit = value;
			}
		}

		public void DoEvents()
		{
			Application.DoEvents();
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[STAThread]
		protected virtual bool OnInitialize(ReadOnlyCollection<string> commandLineArgs)
		{
			if (this.m_EnableVisualStyles)
			{
				Application.EnableVisualStyles();
			}
			if (!commandLineArgs.Contains("/nosplash") && !this.CommandLineArgs.Contains("-nosplash"))
			{
				this.ShowSplashScreen();
			}
			this.m_FinishedOnInitilaize = true;
			return true;
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual bool OnStartup(StartupEventArgs eventArgs)
		{
			eventArgs.Cancel = false;
			if (this.m_TurnOnNetworkListener & (this.m_NetworkObject == null))
			{
				this.m_NetworkObject = new Network();
				this.m_NetworkObject.NetworkAvailabilityChanged += this.NetworkAvailableEventAdaptor;
			}
			StartupEventHandler startupEvent = this.StartupEvent;
			if (startupEvent != null)
			{
				startupEvent(this, eventArgs);
			}
			return !eventArgs.Cancel;
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnStartupNextInstance(StartupNextInstanceEventArgs eventArgs)
		{
			StartupNextInstanceEventHandler startupNextInstanceEvent = this.StartupNextInstanceEvent;
			if (startupNextInstanceEvent != null)
			{
				startupNextInstanceEvent(this, eventArgs);
			}
			new UIPermission(UIPermissionWindow.AllWindows).Assert();
			if (eventArgs.BringToForeground && this.MainForm != null)
			{
				if (this.MainForm.WindowState == FormWindowState.Minimized)
				{
					this.MainForm.WindowState = FormWindowState.Normal;
				}
				this.MainForm.Activate();
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnRun()
		{
			if (this.MainForm == null)
			{
				this.OnCreateMainForm();
				if (this.MainForm == null)
				{
					throw new NoStartupFormException();
				}
				this.MainForm.Load += this.MainFormLoadingDone;
			}
			try
			{
				Application.Run(this.m_AppContext);
			}
			finally
			{
				if (this.m_NetworkObject != null)
				{
					this.m_NetworkObject.DisconnectListener();
				}
				if (this.m_FirstInstanceSemaphore != null)
				{
					this.m_FirstInstanceSemaphore.Close();
					this.m_FirstInstanceSemaphore = null;
				}
				AsyncOperationManager.SynchronizationContext = this.m_AppSyncronizationContext;
				this.m_AppSyncronizationContext = null;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnCreateSplashScreen()
		{
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnCreateMainForm()
		{
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnShutdown()
		{
			ShutdownEventHandler shutdownEvent = this.ShutdownEvent;
			if (shutdownEvent != null)
			{
				shutdownEvent(this, EventArgs.Empty);
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual bool OnUnhandledException(UnhandledExceptionEventArgs e)
		{
			if (this.m_UnhandledExceptionHandlers != null && this.m_UnhandledExceptionHandlers.Count > 0)
			{
				this.raise_UnhandledException(this, e);
				if (e.ExitApplication)
				{
					Application.Exit();
				}
				return true;
			}
			return false;
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected void ShowSplashScreen()
		{
			if (!this.m_DidSplashScreen)
			{
				this.m_DidSplashScreen = true;
				if (this.m_SplashScreen == null)
				{
					this.OnCreateSplashScreen();
				}
				if (this.m_SplashScreen != null)
				{
					if (this.m_MinimumSplashExposure > 0)
					{
						this.m_Ok2CloseSplashScreen = false;
						this.m_SplashTimer = new global::System.Timers.Timer((double)this.m_MinimumSplashExposure);
						this.m_SplashTimer.Elapsed += this.MinimumSplashExposureTimeIsUp;
						this.m_SplashTimer.AutoReset = false;
					}
					else
					{
						this.m_Ok2CloseSplashScreen = true;
					}
					Thread thread = new Thread(new ThreadStart(this.DisplaySplash));
					thread.Start();
				}
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected void HideSplashScreen()
		{
			object splashLock = this.m_SplashLock;
			ObjectFlowControl.CheckForSyncLockOnValueType(splashLock);
			lock (splashLock)
			{
				if (this.m_SplashScreen != null && !this.m_SplashScreen.IsDisposed)
				{
					WindowsFormsApplicationBase.DisposeDelegate disposeDelegate = new WindowsFormsApplicationBase.DisposeDelegate(this.m_SplashScreen.Dispose);
					this.m_SplashScreen.Invoke(disposeDelegate);
					this.m_SplashScreen = null;
				}
				if (this.MainForm != null)
				{
					new UIPermission(UIPermissionWindow.AllWindows).Assert();
					this.MainForm.Activate();
					PermissionSet.RevertAssert();
				}
			}
		}

		protected internal ShutdownMode ShutdownStyle
		{
			get
			{
				return this.m_ShutdownStyle;
			}
			set
			{
				this.ValidateShutdownModeEnumValue(value, "value");
				this.m_ShutdownStyle = value;
			}
		}

		protected bool EnableVisualStyles
		{
			get
			{
				return this.m_EnableVisualStyles;
			}
			set
			{
				this.m_EnableVisualStyles = value;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected bool IsSingleInstance
		{
			get
			{
				return this.m_IsSingleInstance;
			}
			set
			{
				this.m_IsSingleInstance = value;
			}
		}

		private void ValidateAuthenticationModeEnumValue(AuthenticationMode value, string paramName)
		{
			if (value < AuthenticationMode.Windows || value > AuthenticationMode.ApplicationDefined)
			{
				throw new InvalidEnumArgumentException(paramName, (int)value, typeof(AuthenticationMode));
			}
		}

		private void ValidateShutdownModeEnumValue(ShutdownMode value, string paramName)
		{
			if (value < ShutdownMode.AfterMainFormCloses || value > ShutdownMode.AfterAllFormsClose)
			{
				throw new InvalidEnumArgumentException(paramName, (int)value, typeof(ShutdownMode));
			}
		}

		private void DisplaySplash()
		{
			if (this.m_SplashTimer != null)
			{
				this.m_SplashTimer.Enabled = true;
			}
			Application.Run(this.m_SplashScreen);
		}

		private void MinimumSplashExposureTimeIsUp(object sender, ElapsedEventArgs e)
		{
			if (this.m_SplashTimer != null)
			{
				this.m_SplashTimer.Dispose();
				this.m_SplashTimer = null;
			}
			this.m_Ok2CloseSplashScreen = true;
		}

		private void MainFormLoadingDone(object sender, EventArgs e)
		{
			this.MainForm.Load -= this.MainFormLoadingDone;
			while (!this.m_Ok2CloseSplashScreen)
			{
				this.DoEvents();
			}
			this.HideSplashScreen();
		}

		private void OnUnhandledExceptionEventAdaptor(object sender, ThreadExceptionEventArgs e)
		{
			this.OnUnhandledException(new UnhandledExceptionEventArgs(true, e.Exception));
		}

		private void OnStartupNextInstanceMarshallingAdaptor(object args)
		{
			this.OnStartupNextInstance(new StartupNextInstanceEventArgs((ReadOnlyCollection<string>)args, true));
		}

		private void NetworkAvailableEventAdaptor(object sender, NetworkAvailableEventArgs e)
		{
			this.raise_NetworkAvailabilityChanged(sender, e);
		}

		private string HostName
		{
			get
			{
				if (string.IsNullOrEmpty(this.m_HostName))
				{
					if (Socket.SupportsIPv4)
					{
						this.m_HostName = IPAddress.Loopback.ToString();
					}
					else
					{
						if (!Socket.OSSupportsIPv6)
						{
							throw new RemotingException();
						}
						this.m_HostName = IPAddress.IPv6Loopback.ToString();
					}
				}
				return this.m_HostName;
			}
		}

		private SendOrPostCallback RunNextInstanceDelegate
		{
			get
			{
				return this.m_StartNextInstanceCallback;
			}
		}

		private string ReadUrlFromMemoryMappedFile()
		{
			string text;
			using (SafeHandleZeroOrMinusOneIsInvalid safeHandleZeroOrMinusOneIsInvalid = new SafeFileHandle(UnsafeNativeMethods.OpenFileMapping(4, false, this.m_MemoryMappedID), true))
			{
				if (safeHandleZeroOrMinusOneIsInvalid.IsInvalid)
				{
					return null;
				}
				try
				{
					HandleRef handleRef = new HandleRef(null, safeHandleZeroOrMinusOneIsInvalid.DangerousGetHandle());
					IntPtr intPtr = UnsafeNativeMethods.MapViewOfFile(handleRef, 4, 0, 0, 0);
					if (intPtr == IntPtr.Zero)
					{
						throw ExceptionUtils.GetWin32Exception("AppModel_CantGetMemoryMappedFile", new string[0]);
					}
					text = Marshal.PtrToStringUni(intPtr);
				}
				finally
				{
					IntPtr intPtr;
					if (intPtr != IntPtr.Zero)
					{
						HandleRef handleRef = new HandleRef(null, intPtr);
						UnsafeNativeMethods.UnmapViewOfFile(handleRef);
					}
				}
			}
			return text;
		}

		private void WriteUrlToMemoryMappedFile(string URL)
		{
			HandleRef handleRef = new HandleRef(null, (IntPtr)(-1));
			using (NativeTypes.SECURITY_ATTRIBUTES security_ATTRIBUTES = new NativeTypes.SECURITY_ATTRIBUTES())
			{
				security_ATTRIBUTES.bInheritHandle = false;
				bool flag;
				try
				{
					new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
					flag = NativeMethods.ConvertStringSecurityDescriptorToSecurityDescriptor("D:(A;;GA;;;CO)(A;;GR;;;AU)", 1U, ref security_ATTRIBUTES.lpSecurityDescriptor, IntPtr.Zero);
					CodeAccessPermission.RevertAssert();
				}
				catch (EntryPointNotFoundException ex)
				{
					security_ATTRIBUTES.lpSecurityDescriptor = IntPtr.Zero;
				}
				catch (DllNotFoundException ex2)
				{
					security_ATTRIBUTES.lpSecurityDescriptor = IntPtr.Zero;
				}
				if (!flag)
				{
					security_ATTRIBUTES.lpSecurityDescriptor = IntPtr.Zero;
				}
				this.m_FirstInstanceMemoryMappedFileHandle = new SafeFileHandle(UnsafeNativeMethods.CreateFileMapping(handleRef, security_ATTRIBUTES, 4, 0, checked((URL.Length + 1) * 2), this.m_MemoryMappedID), true);
				if (this.m_FirstInstanceMemoryMappedFileHandle.IsInvalid)
				{
					throw ExceptionUtils.GetWin32Exception("AppModel_CantGetMemoryMappedFile", new string[0]);
				}
			}
			try
			{
				HandleRef handleRef2 = new HandleRef(null, this.m_FirstInstanceMemoryMappedFileHandle.DangerousGetHandle());
				IntPtr intPtr = UnsafeNativeMethods.MapViewOfFile(handleRef2, 2, 0, 0, 0);
				if (intPtr == IntPtr.Zero)
				{
					throw ExceptionUtils.GetWin32Exception("AppModel_CantGetMemoryMappedFile", new string[0]);
				}
				char[] array = URL.ToCharArray();
				Marshal.Copy(array, 0, intPtr, array.Length);
			}
			finally
			{
				IntPtr intPtr;
				if (intPtr != IntPtr.Zero)
				{
					HandleRef handleRef2 = new HandleRef(null, intPtr);
					UnsafeNativeMethods.UnmapViewOfFile(handleRef2);
				}
			}
		}

		private TcpChannel RegisterChannel(bool SecureChannel)
		{
			PermissionSet permissionSet = new PermissionSet(PermissionState.None);
			permissionSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.UnmanagedCode | SecurityPermissionFlag.SerializationFormatter | SecurityPermissionFlag.ControlPrincipal));
			permissionSet.AddPermission(new SocketPermission(NetworkAccess.Accept, TransportType.Tcp, this.HostName, 0));
			permissionSet.AddPermission(new EnvironmentPermission(EnvironmentPermissionAccess.Read, "USERNAME"));
			permissionSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.RemotingConfiguration));
			permissionSet.Assert();
			IDictionary dictionary = new Hashtable(3);
			dictionary.Add("bindTo", this.HostName);
			dictionary.Add("port", 0);
			if (SecureChannel)
			{
				dictionary.Add("secure", true);
				dictionary.Add("tokenimpersonationlevel", TokenImpersonationLevel.Impersonation);
				dictionary.Add("impersonate", true);
			}
			TcpChannel tcpChannel = new TcpChannel(dictionary, null, null);
			ChannelServices.RegisterChannel(tcpChannel, false);
			PermissionSet.RevertAssert();
			return tcpChannel;
		}

		private void DoApplicationModel()
		{
			StartupEventArgs startupEventArgs = new StartupEventArgs(base.CommandLineArgs);
			if (!Debugger.IsAttached)
			{
				try
				{
					if (this.OnInitialize(base.CommandLineArgs) && this.OnStartup(startupEventArgs))
					{
						this.OnRun();
						this.OnShutdown();
					}
					return;
				}
				catch (Exception ex)
				{
					if (this.m_ProcessingUnhandledExceptionEvent)
					{
						throw;
					}
					if (!this.OnUnhandledException(new UnhandledExceptionEventArgs(true, ex)))
					{
						throw;
					}
					return;
				}
			}
			if (this.OnInitialize(base.CommandLineArgs) && this.OnStartup(startupEventArgs))
			{
				this.OnRun();
				this.OnShutdown();
			}
		}

		private string GetApplicationInstanceID(Assembly Entry)
		{
			PermissionSet permissionSet = new PermissionSet(PermissionState.None);
			permissionSet.AddPermission(new FileIOPermission(PermissionState.Unrestricted));
			permissionSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.UnmanagedCode));
			permissionSet.Assert();
			Guid typeLibGuidForAssembly = Marshal.GetTypeLibGuidForAssembly(Entry);
			string text = Entry.GetName().Version.ToString();
			string[] array = text.Split(Conversions.ToCharArrayRankOne("."));
			PermissionSet.RevertAssert();
			return typeLibGuidForAssembly.ToString() + array[0] + "." + array[1];
		}

		private const int SECOND_INSTANCE_TIMEOUT = 1000;

		private const int ATTACH_TIMEOUT = 2500;

		private ArrayList m_UnhandledExceptionHandlers;

		private bool m_ProcessingUnhandledExceptionEvent;

		private bool m_TurnOnNetworkListener;

		private bool m_FinishedOnInitilaize;

		private ArrayList m_NetworkAvailabilityEventHandlers;

		private EventWaitHandle m_FirstInstanceSemaphore;

		private EventWaitHandle m_MessageRecievedSemaphore;

		private Network m_NetworkObject;

		private string m_MemoryMappedID;

		private SafeFileHandle m_FirstInstanceMemoryMappedFileHandle;

		private bool m_IsSingleInstance;

		private ShutdownMode m_ShutdownStyle;

		private bool m_EnableVisualStyles;

		private bool m_DidSplashScreen;

		private bool m_Ok2CloseSplashScreen;

		private Form m_SplashScreen;

		private int m_MinimumSplashExposure;

		private global::System.Timers.Timer m_SplashTimer;

		private object m_SplashLock;

		private WindowsFormsApplicationBase.WinFormsAppContext m_AppContext;

		private SynchronizationContext m_AppSyncronizationContext;

		private object m_NetworkAvailChangeLock;

		private bool m_SaveMySettingsOnExit;

		private SendOrPostCallback m_StartNextInstanceCallback;

		private string m_HostName;

		private class WinFormsAppContext : ApplicationContext
		{
			public WinFormsAppContext(WindowsFormsApplicationBase App)
			{
				this.m_App = App;
			}

			protected override void OnMainFormClosed(object sender, EventArgs e)
			{
				if (this.m_App.ShutdownStyle == ShutdownMode.AfterMainFormCloses)
				{
					base.OnMainFormClosed(sender, e);
				}
				else
				{
					new UIPermission(UIPermissionWindow.AllWindows).Assert();
					FormCollection openForms = Application.OpenForms;
					PermissionSet.RevertAssert();
					if (openForms.Count > 0)
					{
						this.MainForm = openForms[0];
					}
					else
					{
						base.OnMainFormClosed(sender, e);
					}
				}
			}

			private WindowsFormsApplicationBase m_App;
		}

		private delegate void DisposeDelegate();

		private class RemoteCommunicator : MarshalByRefObject
		{
			internal RemoteCommunicator(WindowsFormsApplicationBase appObject, EventWaitHandle ConnectionMadeSemaphore)
			{
				new SecurityPermission(SecurityPermissionFlag.ControlPrincipal).Assert();
				this.m_OriginalUser = WindowsIdentity.GetCurrent();
				CodeAccessPermission.RevertAssert();
				this.m_AsyncOp = AsyncOperationManager.CreateOperation(null);
				this.m_StartNextInstanceDelegate = appObject.RunNextInstanceDelegate;
				this.m_ConnectionMadeSemaphore = ConnectionMadeSemaphore;
			}

			[OneWay]
			public void RunNextInstance(ReadOnlyCollection<string> Args)
			{
				new SecurityPermission(SecurityPermissionFlag.ControlPrincipal).Assert();
				if (this.m_OriginalUser.User != WindowsIdentity.GetCurrent().User)
				{
					return;
				}
				this.m_ConnectionMadeSemaphore.Set();
				CodeAccessPermission.RevertAssert();
				this.m_AsyncOp.Post(this.m_StartNextInstanceDelegate, Args);
			}

			public override object InitializeLifetimeService()
			{
				return null;
			}

			private SendOrPostCallback m_StartNextInstanceDelegate;

			private AsyncOperation m_AsyncOp;

			private WindowsIdentity m_OriginalUser;

			private EventWaitHandle m_ConnectionMadeSemaphore;
		}
	}
}
