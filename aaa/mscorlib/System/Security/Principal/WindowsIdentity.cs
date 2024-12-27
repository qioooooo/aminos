using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace System.Security.Principal
{
	// Token: 0x020004B7 RID: 1207
	[ComVisible(true)]
	[Serializable]
	public class WindowsIdentity : IIdentity, ISerializable, IDeserializationCallback, IDisposable
	{
		// Token: 0x0600309F RID: 12447 RVA: 0x000A783F File Offset: 0x000A683F
		private WindowsIdentity()
		{
		}

		// Token: 0x060030A0 RID: 12448 RVA: 0x000A7859 File Offset: 0x000A6859
		internal WindowsIdentity(SafeTokenHandle safeTokenHandle)
			: this(safeTokenHandle.DangerousGetHandle())
		{
			GC.KeepAlive(safeTokenHandle);
		}

		// Token: 0x060030A1 RID: 12449 RVA: 0x000A786D File Offset: 0x000A686D
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlPrincipal)]
		public WindowsIdentity(IntPtr userToken)
			: this(userToken, null, -1)
		{
		}

		// Token: 0x060030A2 RID: 12450 RVA: 0x000A7878 File Offset: 0x000A6878
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlPrincipal)]
		public WindowsIdentity(IntPtr userToken, string type)
			: this(userToken, type, -1)
		{
		}

		// Token: 0x060030A3 RID: 12451 RVA: 0x000A7883 File Offset: 0x000A6883
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlPrincipal)]
		public WindowsIdentity(IntPtr userToken, string type, WindowsAccountType acctType)
			: this(userToken, type, -1)
		{
		}

		// Token: 0x060030A4 RID: 12452 RVA: 0x000A788E File Offset: 0x000A688E
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlPrincipal)]
		public WindowsIdentity(IntPtr userToken, string type, WindowsAccountType acctType, bool isAuthenticated)
			: this(userToken, type, isAuthenticated ? 1 : 0)
		{
		}

		// Token: 0x060030A5 RID: 12453 RVA: 0x000A78A0 File Offset: 0x000A68A0
		private WindowsIdentity(IntPtr userToken, string authType, int isAuthenticated)
		{
			this.CreateFromToken(userToken);
			this.m_authType = authType;
			this.m_isAuthenticated = isAuthenticated;
		}

		// Token: 0x060030A6 RID: 12454 RVA: 0x000A78D0 File Offset: 0x000A68D0
		private void CreateFromToken(IntPtr userToken)
		{
			if (userToken == IntPtr.Zero)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_TokenZero"));
			}
			uint num = (uint)Marshal.SizeOf(typeof(uint));
			Win32Native.GetTokenInformation(userToken, 8U, SafeLocalAllocHandle.InvalidHandle, 0U, out num);
			if (Marshal.GetLastWin32Error() == 6)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidImpersonationToken"));
			}
			if (!Win32Native.DuplicateHandle(Win32Native.GetCurrentProcess(), userToken, Win32Native.GetCurrentProcess(), ref this.m_safeTokenHandle, 0U, true, 2U))
			{
				throw new SecurityException(Win32Native.GetMessage(Marshal.GetLastWin32Error()));
			}
		}

		// Token: 0x060030A7 RID: 12455 RVA: 0x000A795D File Offset: 0x000A695D
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlPrincipal)]
		public WindowsIdentity(string sUserPrincipalName)
			: this(sUserPrincipalName, null)
		{
		}

		// Token: 0x060030A8 RID: 12456 RVA: 0x000A7967 File Offset: 0x000A6967
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlPrincipal)]
		public WindowsIdentity(string sUserPrincipalName, string type)
		{
			this.m_safeTokenHandle = WindowsIdentity.KerbS4ULogon(sUserPrincipalName);
		}

		// Token: 0x060030A9 RID: 12457 RVA: 0x000A798D File Offset: 0x000A698D
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlPrincipal)]
		public WindowsIdentity(SerializationInfo info, StreamingContext context)
			: this(info)
		{
		}

		// Token: 0x060030AA RID: 12458 RVA: 0x000A7998 File Offset: 0x000A6998
		private WindowsIdentity(SerializationInfo info)
		{
			IntPtr intPtr = (IntPtr)info.GetValue("m_userToken", typeof(IntPtr));
			if (intPtr != IntPtr.Zero)
			{
				this.CreateFromToken(intPtr);
			}
		}

		// Token: 0x060030AB RID: 12459 RVA: 0x000A79EC File Offset: 0x000A69EC
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("m_userToken", this.m_safeTokenHandle.DangerousGetHandle());
		}

		// Token: 0x060030AC RID: 12460 RVA: 0x000A7A09 File Offset: 0x000A6A09
		void IDeserializationCallback.OnDeserialization(object sender)
		{
		}

		// Token: 0x060030AD RID: 12461 RVA: 0x000A7A0B File Offset: 0x000A6A0B
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlPrincipal)]
		public static WindowsIdentity GetCurrent()
		{
			return WindowsIdentity.GetCurrentInternal(TokenAccessLevels.MaximumAllowed, false);
		}

		// Token: 0x060030AE RID: 12462 RVA: 0x000A7A18 File Offset: 0x000A6A18
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlPrincipal)]
		public static WindowsIdentity GetCurrent(bool ifImpersonating)
		{
			return WindowsIdentity.GetCurrentInternal(TokenAccessLevels.MaximumAllowed, ifImpersonating);
		}

		// Token: 0x060030AF RID: 12463 RVA: 0x000A7A25 File Offset: 0x000A6A25
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlPrincipal)]
		public static WindowsIdentity GetCurrent(TokenAccessLevels desiredAccess)
		{
			return WindowsIdentity.GetCurrentInternal(desiredAccess, false);
		}

		// Token: 0x060030B0 RID: 12464 RVA: 0x000A7A2E File Offset: 0x000A6A2E
		public static WindowsIdentity GetAnonymous()
		{
			return new WindowsIdentity();
		}

		// Token: 0x170008A3 RID: 2211
		// (get) Token: 0x060030B1 RID: 12465 RVA: 0x000A7A38 File Offset: 0x000A6A38
		public string AuthenticationType
		{
			get
			{
				if (this.m_safeTokenHandle.IsInvalid)
				{
					return string.Empty;
				}
				if (this.m_authType != null)
				{
					return this.m_authType;
				}
				Win32Native.LUID logonAuthId = WindowsIdentity.GetLogonAuthId(this.m_safeTokenHandle);
				if (logonAuthId.LowPart == 998U)
				{
					return string.Empty;
				}
				SafeLsaReturnBufferHandle invalidHandle = SafeLsaReturnBufferHandle.InvalidHandle;
				int num = Win32Native.LsaGetLogonSessionData(ref logonAuthId, ref invalidHandle);
				if (num < 0)
				{
					throw WindowsIdentity.GetExceptionFromNtStatus(num);
				}
				string text = Marshal.PtrToStringUni(((Win32Native.SECURITY_LOGON_SESSION_DATA)Marshal.PtrToStructure(invalidHandle.DangerousGetHandle(), typeof(Win32Native.SECURITY_LOGON_SESSION_DATA))).AuthenticationPackage.Buffer);
				invalidHandle.Dispose();
				return text;
			}
		}

		// Token: 0x170008A4 RID: 2212
		// (get) Token: 0x060030B2 RID: 12466 RVA: 0x000A7AD8 File Offset: 0x000A6AD8
		[ComVisible(false)]
		public TokenImpersonationLevel ImpersonationLevel
		{
			get
			{
				if (this.m_safeTokenHandle.IsInvalid)
				{
					return TokenImpersonationLevel.Anonymous;
				}
				uint num = 0U;
				SafeLocalAllocHandle tokenInformation = WindowsIdentity.GetTokenInformation(this.m_safeTokenHandle, TokenInformationClass.TokenType, out num);
				int num2 = Marshal.ReadInt32(tokenInformation.DangerousGetHandle());
				if (num2 == 1)
				{
					return TokenImpersonationLevel.None;
				}
				SafeLocalAllocHandle tokenInformation2 = WindowsIdentity.GetTokenInformation(this.m_safeTokenHandle, TokenInformationClass.TokenImpersonationLevel, out num);
				num2 = Marshal.ReadInt32(tokenInformation2.DangerousGetHandle());
				tokenInformation.Dispose();
				tokenInformation2.Dispose();
				return num2 + TokenImpersonationLevel.Anonymous;
			}
		}

		// Token: 0x170008A5 RID: 2213
		// (get) Token: 0x060030B3 RID: 12467 RVA: 0x000A7B44 File Offset: 0x000A6B44
		public virtual bool IsAuthenticated
		{
			get
			{
				if (!WindowsIdentity.RunningOnWin2K)
				{
					return false;
				}
				if (this.m_isAuthenticated == -1)
				{
					WindowsPrincipal windowsPrincipal = new WindowsPrincipal(this);
					SecurityIdentifier securityIdentifier = new SecurityIdentifier(IdentifierAuthority.NTAuthority, new int[] { 11 });
					this.m_isAuthenticated = (windowsPrincipal.IsInRole(securityIdentifier) ? 1 : 0);
				}
				return this.m_isAuthenticated == 1;
			}
		}

		// Token: 0x170008A6 RID: 2214
		// (get) Token: 0x060030B4 RID: 12468 RVA: 0x000A7B9C File Offset: 0x000A6B9C
		public virtual bool IsGuest
		{
			get
			{
				if (this.m_safeTokenHandle.IsInvalid)
				{
					return false;
				}
				SecurityIdentifier securityIdentifier = new SecurityIdentifier(IdentifierAuthority.NTAuthority, new int[] { 32, 501 });
				return this.User == securityIdentifier;
			}
		}

		// Token: 0x170008A7 RID: 2215
		// (get) Token: 0x060030B5 RID: 12469 RVA: 0x000A7BE4 File Offset: 0x000A6BE4
		public virtual bool IsSystem
		{
			get
			{
				if (this.m_safeTokenHandle.IsInvalid)
				{
					return false;
				}
				SecurityIdentifier securityIdentifier = new SecurityIdentifier(IdentifierAuthority.NTAuthority, new int[] { 18 });
				return this.User == securityIdentifier;
			}
		}

		// Token: 0x170008A8 RID: 2216
		// (get) Token: 0x060030B6 RID: 12470 RVA: 0x000A7C24 File Offset: 0x000A6C24
		public virtual bool IsAnonymous
		{
			get
			{
				if (this.m_safeTokenHandle.IsInvalid)
				{
					return true;
				}
				SecurityIdentifier securityIdentifier = new SecurityIdentifier(IdentifierAuthority.NTAuthority, new int[] { 7 });
				return this.User == securityIdentifier;
			}
		}

		// Token: 0x170008A9 RID: 2217
		// (get) Token: 0x060030B7 RID: 12471 RVA: 0x000A7C60 File Offset: 0x000A6C60
		public virtual string Name
		{
			get
			{
				return this.GetName();
			}
		}

		// Token: 0x060030B8 RID: 12472 RVA: 0x000A7C68 File Offset: 0x000A6C68
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal string GetName()
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			if (this.m_safeTokenHandle.IsInvalid)
			{
				return string.Empty;
			}
			if (this.m_name == null)
			{
				using (WindowsIdentity.SafeImpersonate(SafeTokenHandle.InvalidHandle, null, ref stackCrawlMark))
				{
					NTAccount ntaccount = this.User.Translate(typeof(NTAccount)) as NTAccount;
					this.m_name = ntaccount.ToString();
				}
			}
			return this.m_name;
		}

		// Token: 0x170008AA RID: 2218
		// (get) Token: 0x060030B9 RID: 12473 RVA: 0x000A7CEC File Offset: 0x000A6CEC
		[ComVisible(false)]
		public SecurityIdentifier Owner
		{
			get
			{
				if (this.m_safeTokenHandle.IsInvalid)
				{
					return null;
				}
				if (this.m_owner == null)
				{
					uint num = 0U;
					SafeLocalAllocHandle tokenInformation = WindowsIdentity.GetTokenInformation(this.m_safeTokenHandle, TokenInformationClass.TokenOwner, out num);
					this.m_owner = new SecurityIdentifier(Marshal.ReadIntPtr(tokenInformation.DangerousGetHandle()), true);
					tokenInformation.Dispose();
				}
				return this.m_owner;
			}
		}

		// Token: 0x170008AB RID: 2219
		// (get) Token: 0x060030BA RID: 12474 RVA: 0x000A7D4C File Offset: 0x000A6D4C
		[ComVisible(false)]
		public SecurityIdentifier User
		{
			get
			{
				if (this.m_safeTokenHandle.IsInvalid)
				{
					return null;
				}
				if (this.m_user == null)
				{
					uint num = 0U;
					SafeLocalAllocHandle tokenInformation = WindowsIdentity.GetTokenInformation(this.m_safeTokenHandle, TokenInformationClass.TokenUser, out num);
					this.m_user = new SecurityIdentifier(Marshal.ReadIntPtr(tokenInformation.DangerousGetHandle()), true);
					tokenInformation.Dispose();
				}
				return this.m_user;
			}
		}

		// Token: 0x170008AC RID: 2220
		// (get) Token: 0x060030BB RID: 12475 RVA: 0x000A7DAC File Offset: 0x000A6DAC
		public IdentityReferenceCollection Groups
		{
			get
			{
				if (this.m_safeTokenHandle.IsInvalid)
				{
					return null;
				}
				if (this.m_groups == null)
				{
					IdentityReferenceCollection identityReferenceCollection = new IdentityReferenceCollection();
					uint num = 0U;
					using (SafeLocalAllocHandle tokenInformation = WindowsIdentity.GetTokenInformation(this.m_safeTokenHandle, TokenInformationClass.TokenGroups, out num))
					{
						int num2 = Marshal.ReadInt32(tokenInformation.DangerousGetHandle());
						IntPtr intPtr = new IntPtr((long)tokenInformation.DangerousGetHandle() + (long)Marshal.OffsetOf(typeof(Win32Native.TOKEN_GROUPS), "Groups"));
						for (int i = 0; i < num2; i++)
						{
							Win32Native.SID_AND_ATTRIBUTES sid_AND_ATTRIBUTES = (Win32Native.SID_AND_ATTRIBUTES)Marshal.PtrToStructure(intPtr, typeof(Win32Native.SID_AND_ATTRIBUTES));
							uint num3 = 3221225492U;
							if ((sid_AND_ATTRIBUTES.Attributes & num3) == 4U)
							{
								identityReferenceCollection.Add(new SecurityIdentifier(sid_AND_ATTRIBUTES.Sid, true));
							}
							intPtr = new IntPtr((long)intPtr + (long)Marshal.SizeOf(typeof(Win32Native.SID_AND_ATTRIBUTES)));
						}
					}
					Interlocked.CompareExchange(ref this.m_groups, identityReferenceCollection, null);
				}
				return this.m_groups as IdentityReferenceCollection;
			}
		}

		// Token: 0x170008AD RID: 2221
		// (get) Token: 0x060030BC RID: 12476 RVA: 0x000A7EC8 File Offset: 0x000A6EC8
		public virtual IntPtr Token
		{
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				return this.m_safeTokenHandle.DangerousGetHandle();
			}
		}

		// Token: 0x060030BD RID: 12477 RVA: 0x000A7ED8 File Offset: 0x000A6ED8
		[MethodImpl(MethodImplOptions.NoInlining)]
		public virtual WindowsImpersonationContext Impersonate()
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return this.Impersonate(ref stackCrawlMark);
		}

		// Token: 0x060030BE RID: 12478 RVA: 0x000A7EF0 File Offset: 0x000A6EF0
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlPrincipal)]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static WindowsImpersonationContext Impersonate(IntPtr userToken)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			if (userToken == IntPtr.Zero)
			{
				return WindowsIdentity.SafeImpersonate(SafeTokenHandle.InvalidHandle, null, ref stackCrawlMark);
			}
			WindowsIdentity windowsIdentity = new WindowsIdentity(userToken);
			return windowsIdentity.Impersonate(ref stackCrawlMark);
		}

		// Token: 0x060030BF RID: 12479 RVA: 0x000A7F2C File Offset: 0x000A6F2C
		internal WindowsImpersonationContext Impersonate(ref StackCrawlMark stackMark)
		{
			if (!WindowsIdentity.RunningOnWin2K)
			{
				return new WindowsImpersonationContext(SafeTokenHandle.InvalidHandle, WindowsIdentity.GetCurrentThreadWI(), false, null);
			}
			if (this.m_safeTokenHandle.IsInvalid)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_AnonymousCannotImpersonate"));
			}
			return WindowsIdentity.SafeImpersonate(this.m_safeTokenHandle, this, ref stackMark);
		}

		// Token: 0x060030C0 RID: 12480 RVA: 0x000A7F7C File Offset: 0x000A6F7C
		[ComVisible(false)]
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && this.m_safeTokenHandle != null && !this.m_safeTokenHandle.IsClosed)
			{
				this.m_safeTokenHandle.Dispose();
			}
			this.m_name = null;
			this.m_owner = null;
			this.m_user = null;
		}

		// Token: 0x060030C1 RID: 12481 RVA: 0x000A7FB6 File Offset: 0x000A6FB6
		[ComVisible(false)]
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x170008AE RID: 2222
		// (get) Token: 0x060030C2 RID: 12482 RVA: 0x000A7FBF File Offset: 0x000A6FBF
		internal SafeTokenHandle TokenHandle
		{
			get
			{
				return this.m_safeTokenHandle;
			}
		}

		// Token: 0x060030C3 RID: 12483 RVA: 0x000A7FC8 File Offset: 0x000A6FC8
		internal static WindowsImpersonationContext SafeImpersonate(SafeTokenHandle userToken, WindowsIdentity wi, ref StackCrawlMark stackMark)
		{
			if (!WindowsIdentity.RunningOnWin2K)
			{
				return new WindowsImpersonationContext(SafeTokenHandle.InvalidHandle, WindowsIdentity.GetCurrentThreadWI(), false, null);
			}
			int num = 0;
			bool flag;
			SafeTokenHandle currentToken = WindowsIdentity.GetCurrentToken(TokenAccessLevels.MaximumAllowed, false, out flag, out num);
			if (currentToken == null || currentToken.IsInvalid)
			{
				throw new SecurityException(Win32Native.GetMessage(num));
			}
			FrameSecurityDescriptor securityObjectForFrame = SecurityRuntime.GetSecurityObjectForFrame(ref stackMark, true);
			if (securityObjectForFrame == null && SecurityManager._IsSecurityOn())
			{
				throw new SecurityException(Environment.GetResourceString("ExecutionEngine_MissingSecurityDescriptor"));
			}
			WindowsImpersonationContext windowsImpersonationContext = new WindowsImpersonationContext(currentToken, WindowsIdentity.GetCurrentThreadWI(), flag, securityObjectForFrame);
			if (userToken.IsInvalid)
			{
				num = Win32.RevertToSelf();
				if (num < 0)
				{
					throw new SecurityException(Win32Native.GetMessage(num));
				}
				WindowsIdentity.UpdateThreadWI(wi);
				securityObjectForFrame.SetTokenHandles(currentToken, (wi == null) ? null : wi.TokenHandle);
			}
			else
			{
				num = Win32.RevertToSelf();
				if (num < 0)
				{
					throw new SecurityException(Win32Native.GetMessage(num));
				}
				num = Win32.ImpersonateLoggedOnUser(userToken);
				if (num < 0)
				{
					windowsImpersonationContext.Undo();
					throw new SecurityException(Environment.GetResourceString("Argument_ImpersonateUser"));
				}
				WindowsIdentity.UpdateThreadWI(wi);
				securityObjectForFrame.SetTokenHandles(currentToken, (wi == null) ? null : wi.TokenHandle);
			}
			return windowsImpersonationContext;
		}

		// Token: 0x060030C4 RID: 12484 RVA: 0x000A80D4 File Offset: 0x000A70D4
		internal static WindowsIdentity GetCurrentThreadWI()
		{
			return SecurityContext.GetCurrentWI(Thread.CurrentThread.GetExecutionContextNoCreate());
		}

		// Token: 0x060030C5 RID: 12485 RVA: 0x000A80E8 File Offset: 0x000A70E8
		internal static void UpdateThreadWI(WindowsIdentity wi)
		{
			SecurityContext securityContext = SecurityContext.GetCurrentSecurityContextNoCreate();
			if (wi != null && securityContext == null)
			{
				securityContext = new SecurityContext();
				Thread.CurrentThread.ExecutionContext.SecurityContext = securityContext;
			}
			if (securityContext != null)
			{
				securityContext.WindowsIdentity = wi;
			}
		}

		// Token: 0x060030C6 RID: 12486 RVA: 0x000A8124 File Offset: 0x000A7124
		internal static WindowsIdentity GetCurrentInternal(TokenAccessLevels desiredAccess, bool threadOnly)
		{
			WindowsIdentity windowsIdentity = null;
			if (!WindowsIdentity.RunningOnWin2K)
			{
				if (!threadOnly)
				{
					windowsIdentity = new WindowsIdentity();
					windowsIdentity.m_name = string.Empty;
				}
				return windowsIdentity;
			}
			int num = 0;
			bool flag;
			SafeTokenHandle currentToken = WindowsIdentity.GetCurrentToken(desiredAccess, threadOnly, out flag, out num);
			if (currentToken != null && !currentToken.IsInvalid)
			{
				windowsIdentity = new WindowsIdentity();
				windowsIdentity.m_safeTokenHandle.Dispose();
				windowsIdentity.m_safeTokenHandle = currentToken;
				return windowsIdentity;
			}
			if (threadOnly && !flag)
			{
				return windowsIdentity;
			}
			throw new SecurityException(Win32Native.GetMessage(num));
		}

		// Token: 0x170008AF RID: 2223
		// (get) Token: 0x060030C7 RID: 12487 RVA: 0x000A8198 File Offset: 0x000A7198
		internal static bool RunningOnWin2K
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
			get
			{
				if (WindowsIdentity.s_runningOnWin2K == -1)
				{
					Win32Native.OSVERSIONINFO osversioninfo = new Win32Native.OSVERSIONINFO();
					bool versionEx = Win32Native.GetVersionEx(osversioninfo);
					WindowsIdentity.s_runningOnWin2K = ((versionEx && osversioninfo.PlatformId == 2 && osversioninfo.MajorVersion >= 5) ? 1 : 0);
				}
				return WindowsIdentity.s_runningOnWin2K == 1;
			}
		}

		// Token: 0x060030C8 RID: 12488 RVA: 0x000A81E0 File Offset: 0x000A71E0
		private static int GetHRForWin32Error(int dwLastError)
		{
			if (((long)dwLastError & (long)((ulong)(-2147483648))) == (long)((ulong)(-2147483648)))
			{
				return dwLastError;
			}
			return (dwLastError & 65535) | -2147024896;
		}

		// Token: 0x060030C9 RID: 12489 RVA: 0x000A8204 File Offset: 0x000A7204
		private static Exception GetExceptionFromNtStatus(int status)
		{
			if (status == -1073741790)
			{
				return new UnauthorizedAccessException();
			}
			if (status == -1073741670 || status == -1073741801)
			{
				return new OutOfMemoryException();
			}
			int num = Win32Native.LsaNtStatusToWinError(status);
			return new SecurityException(Win32Native.GetMessage(num));
		}

		// Token: 0x060030CA RID: 12490 RVA: 0x000A8248 File Offset: 0x000A7248
		private static SafeTokenHandle GetCurrentToken(TokenAccessLevels desiredAccess, bool threadOnly, out bool isImpersonating, out int hr)
		{
			isImpersonating = true;
			SafeTokenHandle safeTokenHandle = WindowsIdentity.GetCurrentThreadToken(desiredAccess, out hr);
			if (safeTokenHandle == null && hr == WindowsIdentity.GetHRForWin32Error(1008))
			{
				isImpersonating = false;
				if (!threadOnly)
				{
					safeTokenHandle = WindowsIdentity.GetCurrentProcessToken(desiredAccess, out hr);
				}
			}
			return safeTokenHandle;
		}

		// Token: 0x060030CB RID: 12491 RVA: 0x000A8280 File Offset: 0x000A7280
		private static SafeTokenHandle GetCurrentProcessToken(TokenAccessLevels desiredAccess, out int hr)
		{
			hr = 0;
			SafeTokenHandle invalidHandle = SafeTokenHandle.InvalidHandle;
			if (!Win32Native.OpenProcessToken(Win32Native.GetCurrentProcess(), desiredAccess, ref invalidHandle))
			{
				hr = WindowsIdentity.GetHRForWin32Error(Marshal.GetLastWin32Error());
			}
			return invalidHandle;
		}

		// Token: 0x060030CC RID: 12492 RVA: 0x000A82B4 File Offset: 0x000A72B4
		internal static SafeTokenHandle GetCurrentThreadToken(TokenAccessLevels desiredAccess, out int hr)
		{
			SafeTokenHandle safeTokenHandle;
			hr = Win32.OpenThreadToken(desiredAccess, WinSecurityContext.Both, out safeTokenHandle);
			return safeTokenHandle;
		}

		// Token: 0x060030CD RID: 12493 RVA: 0x000A82D0 File Offset: 0x000A72D0
		private static Win32Native.LUID GetLogonAuthId(SafeTokenHandle safeTokenHandle)
		{
			uint num = 0U;
			SafeLocalAllocHandle tokenInformation = WindowsIdentity.GetTokenInformation(safeTokenHandle, TokenInformationClass.TokenStatistics, out num);
			Win32Native.TOKEN_STATISTICS token_STATISTICS = (Win32Native.TOKEN_STATISTICS)Marshal.PtrToStructure(tokenInformation.DangerousGetHandle(), typeof(Win32Native.TOKEN_STATISTICS));
			tokenInformation.Dispose();
			return token_STATISTICS.AuthenticationId;
		}

		// Token: 0x060030CE RID: 12494 RVA: 0x000A8314 File Offset: 0x000A7314
		private static SafeLocalAllocHandle GetTokenInformation(SafeTokenHandle tokenHandle, TokenInformationClass tokenInformationClass, out uint dwLength)
		{
			SafeLocalAllocHandle safeLocalAllocHandle = SafeLocalAllocHandle.InvalidHandle;
			dwLength = (uint)Marshal.SizeOf(typeof(uint));
			bool tokenInformation = Win32Native.GetTokenInformation(tokenHandle, (uint)tokenInformationClass, safeLocalAllocHandle, 0U, out dwLength);
			int lastWin32Error = Marshal.GetLastWin32Error();
			int num = lastWin32Error;
			if (num == 6)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidImpersonationToken"));
			}
			if (num != 24 && num != 122)
			{
				throw new SecurityException(Win32Native.GetMessage(lastWin32Error));
			}
			IntPtr intPtr = new IntPtr((long)((ulong)dwLength));
			safeLocalAllocHandle.Dispose();
			safeLocalAllocHandle = Win32Native.LocalAlloc(0, intPtr);
			if (safeLocalAllocHandle == null || safeLocalAllocHandle.IsInvalid)
			{
				throw new OutOfMemoryException();
			}
			if (!Win32Native.GetTokenInformation(tokenHandle, (uint)tokenInformationClass, safeLocalAllocHandle, dwLength, out dwLength))
			{
				throw new SecurityException(Win32Native.GetMessage(Marshal.GetLastWin32Error()));
			}
			return safeLocalAllocHandle;
		}

		// Token: 0x060030CF RID: 12495 RVA: 0x000A83CC File Offset: 0x000A73CC
		private unsafe static SafeTokenHandle KerbS4ULogon(string upn)
		{
			byte[] array = new byte[] { 67, 76, 82 };
			IntPtr intPtr = new IntPtr((long)((ulong)(array.Length + 1)));
			SafeLocalAllocHandle safeLocalAllocHandle = Win32Native.LocalAlloc(64, intPtr);
			Marshal.Copy(array, 0, safeLocalAllocHandle.DangerousGetHandle(), array.Length);
			Win32Native.UNICODE_INTPTR_STRING unicode_INTPTR_STRING = new Win32Native.UNICODE_INTPTR_STRING(array.Length, array.Length + 1, safeLocalAllocHandle.DangerousGetHandle());
			SafeLsaLogonProcessHandle invalidHandle = SafeLsaLogonProcessHandle.InvalidHandle;
			Privilege privilege = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			int num;
			try
			{
				try
				{
					privilege = new Privilege("SeTcbPrivilege");
					privilege.Enable();
				}
				catch (PrivilegeNotHeldException)
				{
				}
				IntPtr zero = IntPtr.Zero;
				num = Win32Native.LsaRegisterLogonProcess(ref unicode_INTPTR_STRING, ref invalidHandle, ref zero);
				if (5 == Win32Native.LsaNtStatusToWinError(num))
				{
					num = Win32Native.LsaConnectUntrusted(ref invalidHandle);
				}
			}
			catch
			{
				if (privilege != null)
				{
					privilege.Revert();
				}
				throw;
			}
			finally
			{
				if (privilege != null)
				{
					privilege.Revert();
				}
			}
			if (num < 0)
			{
				throw WindowsIdentity.GetExceptionFromNtStatus(num);
			}
			byte[] array2 = new byte["Kerberos".Length + 1];
			Encoding.ASCII.GetBytes("Kerberos", 0, "Kerberos".Length, array2, 0);
			intPtr = new IntPtr((long)((ulong)array2.Length));
			SafeLocalAllocHandle safeLocalAllocHandle2 = Win32Native.LocalAlloc(0, intPtr);
			if (safeLocalAllocHandle2 == null || safeLocalAllocHandle2.IsInvalid)
			{
				throw new OutOfMemoryException();
			}
			Marshal.Copy(array2, 0, safeLocalAllocHandle2.DangerousGetHandle(), array2.Length);
			Win32Native.UNICODE_INTPTR_STRING unicode_INTPTR_STRING2 = new Win32Native.UNICODE_INTPTR_STRING("Kerberos".Length, "Kerberos".Length + 1, safeLocalAllocHandle2.DangerousGetHandle());
			uint num2 = 0U;
			num = Win32Native.LsaLookupAuthenticationPackage(invalidHandle, ref unicode_INTPTR_STRING2, ref num2);
			if (num < 0)
			{
				throw WindowsIdentity.GetExceptionFromNtStatus(num);
			}
			Win32Native.TOKEN_SOURCE token_SOURCE = default(Win32Native.TOKEN_SOURCE);
			if (!Win32Native.AllocateLocallyUniqueId(ref token_SOURCE.SourceIdentifier))
			{
				throw new SecurityException(Win32Native.GetMessage(Marshal.GetLastWin32Error()));
			}
			token_SOURCE.Name = new char[8];
			token_SOURCE.Name[0] = 'C';
			token_SOURCE.Name[1] = 'L';
			token_SOURCE.Name[2] = 'R';
			uint num3 = 0U;
			SafeLsaReturnBufferHandle invalidHandle2 = SafeLsaReturnBufferHandle.InvalidHandle;
			Win32Native.LUID luid = default(Win32Native.LUID);
			Win32Native.QUOTA_LIMITS quota_LIMITS = default(Win32Native.QUOTA_LIMITS);
			int num4 = 0;
			SafeTokenHandle invalidHandle3 = SafeTokenHandle.InvalidHandle;
			int num5 = Marshal.SizeOf(typeof(Win32Native.KERB_S4U_LOGON)) + 2 * (upn.Length + 1);
			byte[] array3 = new byte[num5];
			fixed (byte* ptr = array3)
			{
				byte[] array4 = new byte[2 * (upn.Length + 1)];
				Encoding.Unicode.GetBytes(upn, 0, upn.Length, array4, 0);
				Buffer.BlockCopy(array4, 0, array3, Marshal.SizeOf(typeof(Win32Native.KERB_S4U_LOGON)), array4.Length);
				Win32Native.KERB_S4U_LOGON* ptr2 = (Win32Native.KERB_S4U_LOGON*)ptr;
				ptr2->MessageType = 12U;
				ptr2->Flags = 0U;
				ptr2->ClientUpn.Length = (ushort)(2 * upn.Length);
				ptr2->ClientUpn.MaxLength = (ushort)(2 * (upn.Length + 1));
				ptr2->ClientUpn.Buffer = new IntPtr((void*)(ptr2 + 1));
				num = Win32Native.LsaLogonUser(invalidHandle, ref unicode_INTPTR_STRING, 3U, num2, new IntPtr((void*)ptr), (uint)array3.Length, IntPtr.Zero, ref token_SOURCE, ref invalidHandle2, ref num3, ref luid, ref invalidHandle3, ref quota_LIMITS, ref num4);
			}
			if (num == -1073741714 && num4 < 0)
			{
				num = num4;
			}
			if (num < 0)
			{
				throw WindowsIdentity.GetExceptionFromNtStatus(num);
			}
			if (num4 < 0)
			{
				throw WindowsIdentity.GetExceptionFromNtStatus(num4);
			}
			invalidHandle2.Dispose();
			safeLocalAllocHandle.Dispose();
			safeLocalAllocHandle2.Dispose();
			invalidHandle.Dispose();
			return invalidHandle3;
		}

		// Token: 0x04001851 RID: 6225
		private string m_name;

		// Token: 0x04001852 RID: 6226
		private SecurityIdentifier m_owner;

		// Token: 0x04001853 RID: 6227
		private SecurityIdentifier m_user;

		// Token: 0x04001854 RID: 6228
		private object m_groups;

		// Token: 0x04001855 RID: 6229
		private SafeTokenHandle m_safeTokenHandle = SafeTokenHandle.InvalidHandle;

		// Token: 0x04001856 RID: 6230
		private string m_authType;

		// Token: 0x04001857 RID: 6231
		private int m_isAuthenticated = -1;

		// Token: 0x04001858 RID: 6232
		private static int s_runningOnWin2K = -1;
	}
}
