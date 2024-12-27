using System;
using System.ComponentModel;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x02000090 RID: 144
	[EnvironmentPermission(SecurityAction.Assert, Unrestricted = true)]
	public class DirectoryContext
	{
		// Token: 0x0600047A RID: 1146 RVA: 0x000191B0 File Offset: 0x000181B0
		static DirectoryContext()
		{
			OperatingSystem osversion = Environment.OSVersion;
			if (osversion.Platform == PlatformID.Win32NT && osversion.Version.Major >= 5)
			{
				if (osversion.Version.Major == 5 && osversion.Version.Minor == 0)
				{
					DirectoryContext.w2k = true;
					DirectoryContext.dnsgetdcSupported = false;
					OSVersionInfoEx osversionInfoEx = new OSVersionInfoEx();
					if (!NativeMethods.GetVersionEx(osversionInfoEx))
					{
						int lastError = NativeMethods.GetLastError();
						throw new SystemException(Res.GetString("VersionFailure", new object[] { lastError }));
					}
					if (osversionInfoEx.servicePackMajor < 3)
					{
						DirectoryContext.serverBindSupported = false;
					}
				}
				DirectoryContext.GetLibraryHandle();
				return;
			}
			DirectoryContext.platformSupported = false;
			DirectoryContext.serverBindSupported = false;
			DirectoryContext.dnsgetdcSupported = false;
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x0600047B RID: 1147 RVA: 0x0001927D File Offset: 0x0001827D
		internal static bool ServerBindSupported
		{
			get
			{
				return DirectoryContext.serverBindSupported;
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x0600047C RID: 1148 RVA: 0x00019284 File Offset: 0x00018284
		internal static bool DnsgetdcSupported
		{
			get
			{
				return DirectoryContext.dnsgetdcSupported;
			}
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x0001928C File Offset: 0x0001828C
		internal void InitializeDirectoryContext(DirectoryContextType contextType, string name, string username, string password)
		{
			if (!DirectoryContext.platformSupported)
			{
				throw new PlatformNotSupportedException(Res.GetString("SupportedPlatforms"));
			}
			this.name = name;
			this.contextType = contextType;
			this.credential = new NetworkCredential(username, password);
			if (username == null)
			{
				this.usernameIsNull = true;
			}
			if (password == null)
			{
				this.passwordIsNull = true;
			}
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x000192E4 File Offset: 0x000182E4
		internal DirectoryContext(DirectoryContextType contextType, string name, DirectoryContext context)
		{
			this.name = name;
			this.contextType = contextType;
			if (context != null)
			{
				this.credential = context.Credential;
				this.usernameIsNull = context.usernameIsNull;
				this.passwordIsNull = context.passwordIsNull;
				return;
			}
			this.credential = new NetworkCredential(null, null, null);
			this.usernameIsNull = true;
			this.passwordIsNull = true;
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x0001934C File Offset: 0x0001834C
		internal DirectoryContext(DirectoryContext context)
		{
			this.name = context.Name;
			this.contextType = context.ContextType;
			this.credential = context.Credential;
			this.usernameIsNull = context.usernameIsNull;
			this.passwordIsNull = context.passwordIsNull;
			if (context.ContextType != DirectoryContextType.ConfigurationSet)
			{
				this.serverName = context.serverName;
			}
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x000193B0 File Offset: 0x000183B0
		[DirectoryServicesPermission(SecurityAction.Demand, Unrestricted = true)]
		public DirectoryContext(DirectoryContextType contextType)
		{
			if (contextType != DirectoryContextType.Domain && contextType != DirectoryContextType.Forest)
			{
				throw new ArgumentException(Res.GetString("OnlyDomainOrForest"), "contextType");
			}
			this.InitializeDirectoryContext(contextType, null, null, null);
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x000193E0 File Offset: 0x000183E0
		[DirectoryServicesPermission(SecurityAction.Demand, Unrestricted = true)]
		public DirectoryContext(DirectoryContextType contextType, string name)
		{
			if (contextType < DirectoryContextType.Domain || contextType > DirectoryContextType.ApplicationPartition)
			{
				throw new InvalidEnumArgumentException("contextType", (int)contextType, typeof(DirectoryContextType));
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException(Res.GetString("EmptyStringParameter"), "name");
			}
			this.InitializeDirectoryContext(contextType, name, null, null);
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x00019446 File Offset: 0x00018446
		[DirectoryServicesPermission(SecurityAction.Demand, Unrestricted = true)]
		public DirectoryContext(DirectoryContextType contextType, string username, string password)
		{
			if (contextType != DirectoryContextType.Domain && contextType != DirectoryContextType.Forest)
			{
				throw new ArgumentException(Res.GetString("OnlyDomainOrForest"), "contextType");
			}
			this.InitializeDirectoryContext(contextType, null, username, password);
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x00019474 File Offset: 0x00018474
		[DirectoryServicesPermission(SecurityAction.Demand, Unrestricted = true)]
		public DirectoryContext(DirectoryContextType contextType, string name, string username, string password)
		{
			if (contextType < DirectoryContextType.Domain || contextType > DirectoryContextType.ApplicationPartition)
			{
				throw new InvalidEnumArgumentException("contextType", (int)contextType, typeof(DirectoryContextType));
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException(Res.GetString("EmptyStringParameter"), "name");
			}
			this.InitializeDirectoryContext(contextType, name, username, password);
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000484 RID: 1156 RVA: 0x000194DB File Offset: 0x000184DB
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000485 RID: 1157 RVA: 0x000194E3 File Offset: 0x000184E3
		public string UserName
		{
			get
			{
				if (this.usernameIsNull)
				{
					return null;
				}
				return this.credential.UserName;
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000486 RID: 1158 RVA: 0x000194FA File Offset: 0x000184FA
		internal string Password
		{
			[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				if (this.passwordIsNull)
				{
					return null;
				}
				return this.credential.Password;
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000487 RID: 1159 RVA: 0x00019511 File Offset: 0x00018511
		public DirectoryContextType ContextType
		{
			get
			{
				return this.contextType;
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000488 RID: 1160 RVA: 0x00019519 File Offset: 0x00018519
		internal NetworkCredential Credential
		{
			get
			{
				return this.credential;
			}
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x00019524 File Offset: 0x00018524
		[DirectoryServicesPermission(SecurityAction.Assert, Unrestricted = true)]
		internal static bool IsContextValid(DirectoryContext context, DirectoryContextType contextType)
		{
			bool flag = false;
			if (contextType == DirectoryContextType.Domain || (contextType == DirectoryContextType.Forest && context.Name == null))
			{
				string text = context.Name;
				if (text == null)
				{
					context.serverName = DirectoryContext.GetLoggedOnDomain();
					flag = true;
				}
				else
				{
					DomainControllerInfo domainControllerInfo;
					int num = Locator.DsGetDcNameWrapper(null, text, null, 16L, out domainControllerInfo);
					if (num == 1355)
					{
						num = Locator.DsGetDcNameWrapper(null, text, null, 17L, out domainControllerInfo);
						if (num == 1355)
						{
							flag = false;
						}
						else
						{
							if (num != 0)
							{
								throw ExceptionHelper.GetExceptionFromErrorCode(num);
							}
							context.serverName = domainControllerInfo.DomainName;
							flag = true;
						}
					}
					else if (num == 1212)
					{
						flag = false;
					}
					else
					{
						if (num != 0)
						{
							throw ExceptionHelper.GetExceptionFromErrorCode(num);
						}
						context.serverName = domainControllerInfo.DomainName;
						flag = true;
					}
				}
			}
			else if (contextType == DirectoryContextType.Forest)
			{
				DomainControllerInfo domainControllerInfo2;
				int num2 = Locator.DsGetDcNameWrapper(null, context.Name, null, 80L, out domainControllerInfo2);
				if (num2 == 1355)
				{
					num2 = Locator.DsGetDcNameWrapper(null, context.Name, null, 81L, out domainControllerInfo2);
					if (num2 == 1355)
					{
						flag = false;
					}
					else
					{
						if (num2 != 0)
						{
							throw ExceptionHelper.GetExceptionFromErrorCode(num2);
						}
						context.serverName = domainControllerInfo2.DnsForestName;
						flag = true;
					}
				}
				else if (num2 == 1212)
				{
					flag = false;
				}
				else
				{
					if (num2 != 0)
					{
						throw ExceptionHelper.GetExceptionFromErrorCode(num2);
					}
					context.serverName = domainControllerInfo2.DnsForestName;
					flag = true;
				}
			}
			else if (contextType == DirectoryContextType.ApplicationPartition)
			{
				DomainControllerInfo domainControllerInfo3;
				int num3 = Locator.DsGetDcNameWrapper(null, context.Name, null, 32768L, out domainControllerInfo3);
				if (num3 == 1355)
				{
					num3 = Locator.DsGetDcNameWrapper(null, context.Name, null, 32769L, out domainControllerInfo3);
					if (num3 == 1355)
					{
						flag = false;
					}
					else
					{
						if (num3 != 0)
						{
							throw ExceptionHelper.GetExceptionFromErrorCode(num3);
						}
						flag = true;
					}
				}
				else if (num3 == 1212)
				{
					flag = false;
				}
				else
				{
					if (num3 != 0)
					{
						throw ExceptionHelper.GetExceptionFromErrorCode(num3);
					}
					flag = true;
				}
			}
			else
			{
				if (contextType == DirectoryContextType.DirectoryServer)
				{
					string text3;
					string text2 = Utils.SplitServerNameAndPortNumber(context.Name, out text3);
					DirectoryEntry directoryEntry = new DirectoryEntry("WinNT://" + text2 + ",computer", context.UserName, context.Password, Utils.DefaultAuthType);
					try
					{
						try
						{
							directoryEntry.Bind(true);
							flag = true;
						}
						catch (COMException ex)
						{
							if (ex.ErrorCode != -2147024843 && ex.ErrorCode != -2147024845 && ex.ErrorCode != -2147463168)
							{
								throw ExceptionHelper.GetExceptionFromCOMException(context, ex);
							}
							flag = false;
						}
						return flag;
					}
					finally
					{
						directoryEntry.Dispose();
					}
				}
				flag = true;
			}
			return flag;
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x000197B8 File Offset: 0x000187B8
		internal bool isRootDomain()
		{
			if (this.contextType != DirectoryContextType.Forest)
			{
				return false;
			}
			if (!this.validated)
			{
				this.contextIsValid = DirectoryContext.IsContextValid(this, DirectoryContextType.Forest);
				this.validated = true;
			}
			return this.contextIsValid;
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x000197E7 File Offset: 0x000187E7
		internal bool isDomain()
		{
			if (this.contextType != DirectoryContextType.Domain)
			{
				return false;
			}
			if (!this.validated)
			{
				this.contextIsValid = DirectoryContext.IsContextValid(this, DirectoryContextType.Domain);
				this.validated = true;
			}
			return this.contextIsValid;
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x00019815 File Offset: 0x00018815
		internal bool isNdnc()
		{
			if (this.contextType != DirectoryContextType.ApplicationPartition)
			{
				return false;
			}
			if (!this.validated)
			{
				this.contextIsValid = DirectoryContext.IsContextValid(this, DirectoryContextType.ApplicationPartition);
				this.validated = true;
			}
			return this.contextIsValid;
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x00019844 File Offset: 0x00018844
		internal bool isServer()
		{
			if (this.contextType != DirectoryContextType.DirectoryServer)
			{
				return false;
			}
			if (!this.validated)
			{
				if (DirectoryContext.w2k)
				{
					this.contextIsValid = DirectoryContext.IsContextValid(this, DirectoryContextType.DirectoryServer) && !DirectoryContext.IsContextValid(this, DirectoryContextType.Domain) && !DirectoryContext.IsContextValid(this, DirectoryContextType.ApplicationPartition);
				}
				else
				{
					this.contextIsValid = DirectoryContext.IsContextValid(this, DirectoryContextType.DirectoryServer);
				}
				this.validated = true;
			}
			return this.contextIsValid;
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x000198AC File Offset: 0x000188AC
		internal bool isADAMConfigSet()
		{
			if (this.contextType != DirectoryContextType.ConfigurationSet)
			{
				return false;
			}
			if (!this.validated)
			{
				this.contextIsValid = DirectoryContext.IsContextValid(this, DirectoryContextType.ConfigurationSet);
				this.validated = true;
			}
			return this.contextIsValid;
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x000198DC File Offset: 0x000188DC
		internal bool isCurrentForest()
		{
			bool flag = false;
			DomainControllerInfo domainControllerInfo = Locator.GetDomainControllerInfo(null, this.name, null, 1073741840L);
			string loggedOnDomain = DirectoryContext.GetLoggedOnDomain();
			DomainControllerInfo domainControllerInfo2;
			int num = Locator.DsGetDcNameWrapper(null, loggedOnDomain, null, 1073741840L, out domainControllerInfo2);
			if (num == 0)
			{
				flag = Utils.Compare(domainControllerInfo.DnsForestName, domainControllerInfo2.DnsForestName) == 0;
			}
			else if (num != 1355)
			{
				throw ExceptionHelper.GetExceptionFromErrorCode(num);
			}
			return flag;
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x00019944 File Offset: 0x00018944
		internal bool useServerBind()
		{
			return this.ContextType == DirectoryContextType.DirectoryServer || this.ContextType == DirectoryContextType.ConfigurationSet;
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x0001995C File Offset: 0x0001895C
		internal string GetServerName()
		{
			if (this.serverName == null)
			{
				switch (this.contextType)
				{
				case DirectoryContextType.Domain:
				case DirectoryContextType.Forest:
					break;
				case DirectoryContextType.DirectoryServer:
					this.serverName = this.name;
					goto IL_009D;
				case DirectoryContextType.ConfigurationSet:
				{
					AdamInstance adamInstance = ConfigurationSet.FindAnyAdamInstance(this);
					try
					{
						this.serverName = adamInstance.Name;
						goto IL_009D;
					}
					finally
					{
						adamInstance.Dispose();
					}
					break;
				}
				case DirectoryContextType.ApplicationPartition:
					this.serverName = this.name;
					goto IL_009D;
				default:
					goto IL_009D;
				}
				if (this.name == null || (this.contextType == DirectoryContextType.Forest && this.isCurrentForest()))
				{
					this.serverName = DirectoryContext.GetLoggedOnDomain();
				}
				else
				{
					this.serverName = DirectoryContext.GetDnsDomainName(this.name);
				}
			}
			IL_009D:
			return this.serverName;
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x00019A1C File Offset: 0x00018A1C
		internal static string GetLoggedOnDomain()
		{
			string text = null;
			NegotiateCallerNameRequest negotiateCallerNameRequest = new NegotiateCallerNameRequest();
			int num = Marshal.SizeOf(negotiateCallerNameRequest);
			IntPtr zero = IntPtr.Zero;
			NegotiateCallerNameResponse negotiateCallerNameResponse = new NegotiateCallerNameResponse();
			LsaLogonProcessSafeHandle lsaLogonProcessSafeHandle;
			int num2 = NativeMethods.LsaConnectUntrusted(out lsaLogonProcessSafeHandle);
			if (num2 == 0)
			{
				negotiateCallerNameRequest.messageType = 1;
				int num3;
				int num4;
				num2 = NativeMethods.LsaCallAuthenticationPackage(lsaLogonProcessSafeHandle, 0, negotiateCallerNameRequest, num, out zero, out num3, out num4);
				try
				{
					if (num2 == 0 && num4 == 0)
					{
						Marshal.PtrToStructure(zero, negotiateCallerNameResponse);
						int num5 = negotiateCallerNameResponse.callerName.IndexOf('\\');
						text = negotiateCallerNameResponse.callerName.Substring(0, num5);
					}
					else
					{
						if (num2 == -1073741756)
						{
							throw new OutOfMemoryException();
						}
						if (num2 != 0 || UnsafeNativeMethods.LsaNtStatusToWinError(num4) != 1312)
						{
							throw ExceptionHelper.GetExceptionFromErrorCode(UnsafeNativeMethods.LsaNtStatusToWinError((num2 != 0) ? num2 : num4));
						}
						WindowsIdentity current = WindowsIdentity.GetCurrent();
						int num6 = current.Name.IndexOf('\\');
						text = current.Name.Substring(0, num6);
					}
					goto IL_0109;
				}
				finally
				{
					if (zero != IntPtr.Zero)
					{
						NativeMethods.LsaFreeReturnBuffer(zero);
					}
				}
				goto IL_00ED;
				IL_0109:
				if (text != null && Utils.Compare(text, Utils.GetNtAuthorityString()) == 0)
				{
					text = DirectoryContext.GetDnsDomainName(null);
				}
				else
				{
					text = DirectoryContext.GetDnsDomainName(text);
				}
				if (text == null)
				{
					throw new ActiveDirectoryOperationException(Res.GetString("ContextNotAssociatedWithDomain"));
				}
				return text;
			}
			IL_00ED:
			if (num2 == -1073741756)
			{
				throw new OutOfMemoryException();
			}
			throw ExceptionHelper.GetExceptionFromErrorCode(UnsafeNativeMethods.LsaNtStatusToWinError(num2));
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x00019B78 File Offset: 0x00018B78
		internal static string GetDnsDomainName(string domainName)
		{
			DomainControllerInfo domainControllerInfo;
			int num = Locator.DsGetDcNameWrapper(null, domainName, null, 16L, out domainControllerInfo);
			if (num == 1355)
			{
				num = Locator.DsGetDcNameWrapper(null, domainName, null, 17L, out domainControllerInfo);
				if (num == 1355)
				{
					return null;
				}
				if (num != 0)
				{
					throw ExceptionHelper.GetExceptionFromErrorCode(num);
				}
			}
			else if (num != 0)
			{
				throw ExceptionHelper.GetExceptionFromErrorCode(num);
			}
			return domainControllerInfo.DomainName;
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x00019BD0 File Offset: 0x00018BD0
		[FileIOPermission(SecurityAction.Assert, AllFiles = FileIOPermissionAccess.PathDiscovery)]
		private static void GetLibraryHandle()
		{
			string text = Environment.SystemDirectory + "\\ntdsapi.dll";
			IntPtr intPtr = UnsafeNativeMethods.LoadLibrary(text);
			if (intPtr == (IntPtr)0)
			{
				throw ExceptionHelper.GetExceptionFromErrorCode(Marshal.GetLastWin32Error());
			}
			DirectoryContext.ADHandle = new LoadLibrarySafeHandle(intPtr);
			string text2 = Environment.CurrentDirectory + "\\ntdsapi.dll";
			intPtr = UnsafeNativeMethods.LoadLibrary(text2);
			if (!(intPtr == (IntPtr)0))
			{
				DirectoryContext.ADAMHandle = new LoadLibrarySafeHandle(intPtr);
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(Environment.SystemDirectory, 0, Environment.SystemDirectory.Length - 8);
			intPtr = UnsafeNativeMethods.LoadLibrary(stringBuilder.ToString() + "ADAM\\ntdsapi.dll");
			if (intPtr == (IntPtr)0)
			{
				DirectoryContext.ADAMHandle = DirectoryContext.ADHandle;
				return;
			}
			DirectoryContext.ADAMHandle = new LoadLibrarySafeHandle(intPtr);
		}

		// Token: 0x040003E5 RID: 997
		private string name;

		// Token: 0x040003E6 RID: 998
		private DirectoryContextType contextType;

		// Token: 0x040003E7 RID: 999
		private NetworkCredential credential;

		// Token: 0x040003E8 RID: 1000
		internal string serverName;

		// Token: 0x040003E9 RID: 1001
		internal bool usernameIsNull;

		// Token: 0x040003EA RID: 1002
		internal bool passwordIsNull;

		// Token: 0x040003EB RID: 1003
		private bool validated;

		// Token: 0x040003EC RID: 1004
		private bool contextIsValid;

		// Token: 0x040003ED RID: 1005
		private static bool platformSupported = true;

		// Token: 0x040003EE RID: 1006
		private static bool serverBindSupported = true;

		// Token: 0x040003EF RID: 1007
		private static bool dnsgetdcSupported = true;

		// Token: 0x040003F0 RID: 1008
		private static bool w2k = false;

		// Token: 0x040003F1 RID: 1009
		internal static LoadLibrarySafeHandle ADHandle;

		// Token: 0x040003F2 RID: 1010
		internal static LoadLibrarySafeHandle ADAMHandle;
	}
}
