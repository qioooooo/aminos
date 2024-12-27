using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x02000075 RID: 117
	[TypeConverter(typeof(ManagementScopeConverter))]
	public class ManagementScope : ICloneable
	{
		// Token: 0x1400000C RID: 12
		// (add) Token: 0x06000320 RID: 800 RVA: 0x0000CEA9 File Offset: 0x0000BEA9
		// (remove) Token: 0x06000321 RID: 801 RVA: 0x0000CEC2 File Offset: 0x0000BEC2
		internal event IdentifierChangedEventHandler IdentifierChanged;

		// Token: 0x06000322 RID: 802
		[DllImport("rpcrt4.dll")]
		private static extern int RpcMgmtEnableIdleCleanup();

		// Token: 0x06000323 RID: 803 RVA: 0x0000CEDB File Offset: 0x0000BEDB
		private void FireIdentifierChanged()
		{
			if (this.IdentifierChanged != null)
			{
				this.IdentifierChanged(this, null);
			}
		}

		// Token: 0x06000324 RID: 804 RVA: 0x0000CEF2 File Offset: 0x0000BEF2
		private void HandleIdentifierChange(object sender, IdentifierChangedEventArgs args)
		{
			this.wbemServices = null;
			this.FireIdentifierChanged();
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000325 RID: 805 RVA: 0x0000CF01 File Offset: 0x0000BF01
		// (set) Token: 0x06000326 RID: 806 RVA: 0x0000CF0C File Offset: 0x0000BF0C
		private ManagementPath prvpath
		{
			get
			{
				return this.validatedPath;
			}
			set
			{
				if (value != null)
				{
					string path = value.Path;
					if (!ManagementPath.IsValidNamespaceSyntax(path))
					{
						ManagementException.ThrowWithExtendedInfo(ManagementStatus.InvalidNamespace);
					}
				}
				this.validatedPath = value;
			}
		}

		// Token: 0x06000327 RID: 807 RVA: 0x0000CF3C File Offset: 0x0000BF3C
		internal IWbemServices GetIWbemServices()
		{
			IWbemServices wbemServices = this.wbemServices;
			if (CompatSwitches.AllowIManagementObjectQI)
			{
				IntPtr iunknownForObject = Marshal.GetIUnknownForObject(this.wbemServices);
				object objectForIUnknown = Marshal.GetObjectForIUnknown(iunknownForObject);
				Marshal.Release(iunknownForObject);
				if (!object.ReferenceEquals(objectForIUnknown, this.wbemServices))
				{
					SecurityHandler securityHandler = this.GetSecurityHandler();
					securityHandler.SecureIUnknown(objectForIUnknown);
					wbemServices = (IWbemServices)objectForIUnknown;
					securityHandler.Secure(wbemServices);
				}
			}
			return wbemServices;
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000328 RID: 808 RVA: 0x0000CF9C File Offset: 0x0000BF9C
		public bool IsConnected
		{
			get
			{
				return null != this.wbemServices;
			}
		}

		// Token: 0x06000329 RID: 809 RVA: 0x0000CFAA File Offset: 0x0000BFAA
		internal ManagementScope(ManagementPath path, IWbemServices wbemServices, ConnectionOptions options)
		{
			if (path != null)
			{
				this.Path = path;
			}
			if (options != null)
			{
				this.Options = options;
			}
			this.wbemServices = wbemServices;
		}

		// Token: 0x0600032A RID: 810 RVA: 0x0000CFCD File Offset: 0x0000BFCD
		internal ManagementScope(ManagementPath path, ManagementScope scope)
			: this(path, (scope != null) ? scope.options : null)
		{
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0000CFE2 File Offset: 0x0000BFE2
		internal static ManagementScope _Clone(ManagementScope scope)
		{
			return ManagementScope._Clone(scope, null);
		}

		// Token: 0x0600032C RID: 812 RVA: 0x0000CFEC File Offset: 0x0000BFEC
		internal static ManagementScope _Clone(ManagementScope scope, IdentifierChangedEventHandler handler)
		{
			ManagementScope managementScope = new ManagementScope(null, null, null);
			if (handler != null)
			{
				managementScope.IdentifierChanged = handler;
			}
			else if (scope != null)
			{
				managementScope.IdentifierChanged = new IdentifierChangedEventHandler(scope.HandleIdentifierChange);
			}
			if (scope == null)
			{
				managementScope.prvpath = ManagementPath._Clone(ManagementPath.DefaultPath, new IdentifierChangedEventHandler(managementScope.HandleIdentifierChange));
				managementScope.IsDefaulted = true;
				managementScope.wbemServices = null;
				managementScope.options = null;
			}
			else
			{
				if (scope.prvpath == null)
				{
					managementScope.prvpath = ManagementPath._Clone(ManagementPath.DefaultPath, new IdentifierChangedEventHandler(managementScope.HandleIdentifierChange));
					managementScope.IsDefaulted = true;
				}
				else
				{
					managementScope.prvpath = ManagementPath._Clone(scope.prvpath, new IdentifierChangedEventHandler(managementScope.HandleIdentifierChange));
					managementScope.IsDefaulted = scope.IsDefaulted;
				}
				managementScope.wbemServices = scope.wbemServices;
				if (scope.options != null)
				{
					managementScope.options = ConnectionOptions._Clone(scope.options, new IdentifierChangedEventHandler(managementScope.HandleIdentifierChange));
				}
			}
			return managementScope;
		}

		// Token: 0x0600032D RID: 813 RVA: 0x0000D0E4 File Offset: 0x0000C0E4
		public ManagementScope()
			: this(new ManagementPath(ManagementPath.DefaultPath.Path))
		{
			this.IsDefaulted = true;
		}

		// Token: 0x0600032E RID: 814 RVA: 0x0000D102 File Offset: 0x0000C102
		public ManagementScope(ManagementPath path)
			: this(path, null)
		{
		}

		// Token: 0x0600032F RID: 815 RVA: 0x0000D10C File Offset: 0x0000C10C
		public ManagementScope(string path)
			: this(new ManagementPath(path), null)
		{
		}

		// Token: 0x06000330 RID: 816 RVA: 0x0000D11B File Offset: 0x0000C11B
		public ManagementScope(string path, ConnectionOptions options)
			: this(new ManagementPath(path), options)
		{
		}

		// Token: 0x06000331 RID: 817 RVA: 0x0000D12C File Offset: 0x0000C12C
		public ManagementScope(ManagementPath path, ConnectionOptions options)
		{
			if (path != null)
			{
				this.prvpath = ManagementPath._Clone(path, new IdentifierChangedEventHandler(this.HandleIdentifierChange));
			}
			else
			{
				this.prvpath = ManagementPath._Clone(null);
			}
			if (options != null)
			{
				this.options = ConnectionOptions._Clone(options, new IdentifierChangedEventHandler(this.HandleIdentifierChange));
			}
			else
			{
				this.options = null;
			}
			this.IsDefaulted = false;
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000332 RID: 818 RVA: 0x0000D194 File Offset: 0x0000C194
		// (set) Token: 0x06000333 RID: 819 RVA: 0x0000D1CC File Offset: 0x0000C1CC
		public ConnectionOptions Options
		{
			get
			{
				if (this.options == null)
				{
					return this.options = ConnectionOptions._Clone(null, new IdentifierChangedEventHandler(this.HandleIdentifierChange));
				}
				return this.options;
			}
			set
			{
				if (value != null)
				{
					if (this.options != null)
					{
						this.options.IdentifierChanged -= this.HandleIdentifierChange;
					}
					this.options = ConnectionOptions._Clone(value, new IdentifierChangedEventHandler(this.HandleIdentifierChange));
					this.HandleIdentifierChange(this, null);
					return;
				}
				throw new ArgumentNullException("value");
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000334 RID: 820 RVA: 0x0000D228 File Offset: 0x0000C228
		// (set) Token: 0x06000335 RID: 821 RVA: 0x0000D254 File Offset: 0x0000C254
		public ManagementPath Path
		{
			get
			{
				if (this.prvpath == null)
				{
					return this.prvpath = ManagementPath._Clone(null);
				}
				return this.prvpath;
			}
			set
			{
				if (value != null)
				{
					if (this.prvpath != null)
					{
						this.prvpath.IdentifierChanged -= this.HandleIdentifierChange;
					}
					this.IsDefaulted = false;
					this.prvpath = ManagementPath._Clone(value, new IdentifierChangedEventHandler(this.HandleIdentifierChange));
					this.HandleIdentifierChange(this, null);
					return;
				}
				throw new ArgumentNullException("value");
			}
		}

		// Token: 0x06000336 RID: 822 RVA: 0x0000D2B5 File Offset: 0x0000C2B5
		public ManagementScope Clone()
		{
			return ManagementScope._Clone(this);
		}

		// Token: 0x06000337 RID: 823 RVA: 0x0000D2BD File Offset: 0x0000C2BD
		object ICloneable.Clone()
		{
			return this.Clone();
		}

		// Token: 0x06000338 RID: 824 RVA: 0x0000D2C5 File Offset: 0x0000C2C5
		public void Connect()
		{
			this.Initialize();
		}

		// Token: 0x06000339 RID: 825 RVA: 0x0000D2D0 File Offset: 0x0000C2D0
		internal void Initialize()
		{
			if (this.prvpath == null)
			{
				throw new InvalidOperationException();
			}
			if (!this.IsConnected)
			{
				lock (this)
				{
					if (!this.IsConnected)
					{
						if (!MTAHelper.IsNoContextMTA())
						{
							new ThreadDispatch(new ThreadDispatch.ThreadWorkerMethodWithParam(this.InitializeGuts))
							{
								Parameter = this
							}.Start();
						}
						else
						{
							this.InitializeGuts(this);
						}
					}
				}
			}
		}

		// Token: 0x0600033A RID: 826 RVA: 0x0000D34C File Offset: 0x0000C34C
		private void InitializeGuts(object o)
		{
			ManagementScope managementScope = (ManagementScope)o;
			IWbemLocator wbemLocator = (IWbemLocator)new WbemLocator();
			IntPtr zero = IntPtr.Zero;
			if (managementScope.options == null)
			{
				managementScope.Options = new ConnectionOptions();
			}
			string text = managementScope.prvpath.GetNamespacePath(8);
			if (text == null || text.Length == 0)
			{
				bool flag;
				text = managementScope.prvpath.SetNamespacePath(ManagementPath.DefaultPath.Path, out flag);
			}
			SecurityHandler securityHandler = this.GetSecurityHandler();
			int num = 0;
			managementScope.wbemServices = null;
			if (Environment.OSVersion.Platform == PlatformID.Win32NT && ((Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor >= 1) || Environment.OSVersion.Version.Major >= 6))
			{
				managementScope.options.Flags |= 128;
			}
			try
			{
				num = this.GetSecuredConnectHandler().ConnectNSecureIWbemServices(text, ref managementScope.wbemServices);
			}
			catch (COMException ex)
			{
				ManagementException.ThrowWithExtendedInfo(ex);
			}
			finally
			{
				securityHandler.Reset();
			}
			if (((long)num & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
			{
				ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
				return;
			}
			if (((long)num & (long)((ulong)(-2147483648))) != 0L)
			{
				Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
			}
		}

		// Token: 0x0600033B RID: 827 RVA: 0x0000D4A0 File Offset: 0x0000C4A0
		internal SecurityHandler GetSecurityHandler()
		{
			return new SecurityHandler(this);
		}

		// Token: 0x0600033C RID: 828 RVA: 0x0000D4A8 File Offset: 0x0000C4A8
		internal SecuredConnectHandler GetSecuredConnectHandler()
		{
			return new SecuredConnectHandler(this);
		}

		// Token: 0x0600033D RID: 829 RVA: 0x0000D4B0 File Offset: 0x0000C4B0
		internal SecuredIEnumWbemClassObjectHandler GetSecuredIEnumWbemClassObjectHandler(IEnumWbemClassObject pEnumWbemClassObject)
		{
			return new SecuredIEnumWbemClassObjectHandler(this, pEnumWbemClassObject);
		}

		// Token: 0x0600033E RID: 830 RVA: 0x0000D4B9 File Offset: 0x0000C4B9
		internal SecurityIWbemServicesHandler GetSecurityIWbemServicesHandler(IWbemServices pWbemServiecs)
		{
			return new SecurityIWbemServicesHandler(this, pWbemServiecs);
		}

		// Token: 0x040001C0 RID: 448
		private ManagementPath validatedPath;

		// Token: 0x040001C1 RID: 449
		private IWbemServices wbemServices;

		// Token: 0x040001C2 RID: 450
		private ConnectionOptions options;

		// Token: 0x040001C4 RID: 452
		internal bool IsDefaulted;
	}
}
