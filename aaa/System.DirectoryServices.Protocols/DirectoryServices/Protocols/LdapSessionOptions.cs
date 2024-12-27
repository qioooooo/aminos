using System;
using System.Collections;
using System.ComponentModel;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x0200008B RID: 139
	public class LdapSessionOptions
	{
		// Token: 0x060002D5 RID: 725 RVA: 0x0000DF08 File Offset: 0x0000CF08
		internal LdapSessionOptions(LdapConnection connection)
		{
			this.connection = connection;
			this.queryDelegate = new QUERYFORCONNECTIONInternal(this.ProcessQueryConnection);
			this.notifiyDelegate = new NOTIFYOFNEWCONNECTIONInternal(this.ProcessNotifyConnection);
			this.dereferenceDelegate = new DEREFERENCECONNECTIONInternal(this.ProcessDereferenceConnection);
			this.serverCertificateRoutine = new VERIFYSERVERCERT(this.ProcessServerCertificate);
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060002D6 RID: 726 RVA: 0x0000DF78 File Offset: 0x0000CF78
		// (set) Token: 0x060002D7 RID: 727 RVA: 0x0000DF95 File Offset: 0x0000CF95
		public ReferralChasingOptions ReferralChasing
		{
			get
			{
				int intValueHelper = this.GetIntValueHelper(LdapOption.LDAP_OPT_REFERRALS);
				if (intValueHelper == 1)
				{
					return ReferralChasingOptions.All;
				}
				return (ReferralChasingOptions)intValueHelper;
			}
			set
			{
				if ((value & ~(ReferralChasingOptions.Subordinate | ReferralChasingOptions.External)) != ReferralChasingOptions.None)
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ReferralChasingOptions));
				}
				this.SetIntValueHelper(LdapOption.LDAP_OPT_REFERRALS, (int)value);
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060002D8 RID: 728 RVA: 0x0000DFBC File Offset: 0x0000CFBC
		// (set) Token: 0x060002D9 RID: 729 RVA: 0x0000DFDC File Offset: 0x0000CFDC
		public bool SecureSocketLayer
		{
			get
			{
				int intValueHelper = this.GetIntValueHelper(LdapOption.LDAP_OPT_SSL);
				return intValueHelper == 1;
			}
			set
			{
				int num;
				if (value)
				{
					num = 1;
				}
				else
				{
					num = 0;
				}
				this.SetIntValueHelper(LdapOption.LDAP_OPT_SSL, num);
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060002DA RID: 730 RVA: 0x0000DFFB File Offset: 0x0000CFFB
		// (set) Token: 0x060002DB RID: 731 RVA: 0x0000E005 File Offset: 0x0000D005
		public int ReferralHopLimit
		{
			get
			{
				return this.GetIntValueHelper(LdapOption.LDAP_OPT_REFERRAL_HOP_LIMIT);
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException(Res.GetString("ValidValue"), "value");
				}
				this.SetIntValueHelper(LdapOption.LDAP_OPT_REFERRAL_HOP_LIMIT, value);
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060002DC RID: 732 RVA: 0x0000E029 File Offset: 0x0000D029
		// (set) Token: 0x060002DD RID: 733 RVA: 0x0000E033 File Offset: 0x0000D033
		public int ProtocolVersion
		{
			get
			{
				return this.GetIntValueHelper(LdapOption.LDAP_OPT_VERSION);
			}
			set
			{
				this.SetIntValueHelper(LdapOption.LDAP_OPT_VERSION, value);
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060002DE RID: 734 RVA: 0x0000E03E File Offset: 0x0000D03E
		// (set) Token: 0x060002DF RID: 735 RVA: 0x0000E049 File Offset: 0x0000D049
		public string HostName
		{
			get
			{
				return this.GetStringValueHelper(LdapOption.LDAP_OPT_HOST_NAME, false);
			}
			set
			{
				this.SetStringValueHelper(LdapOption.LDAP_OPT_HOST_NAME, value);
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060002E0 RID: 736 RVA: 0x0000E054 File Offset: 0x0000D054
		// (set) Token: 0x060002E1 RID: 737 RVA: 0x0000E05F File Offset: 0x0000D05F
		public string DomainName
		{
			get
			{
				return this.GetStringValueHelper(LdapOption.LDAP_OPT_DNSDOMAIN_NAME, true);
			}
			set
			{
				this.SetStringValueHelper(LdapOption.LDAP_OPT_DNSDOMAIN_NAME, value);
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060002E2 RID: 738 RVA: 0x0000E06C File Offset: 0x0000D06C
		// (set) Token: 0x060002E3 RID: 739 RVA: 0x0000E084 File Offset: 0x0000D084
		public LocatorFlags LocatorFlag
		{
			get
			{
				int intValueHelper = this.GetIntValueHelper(LdapOption.LDAP_OPT_GETDSNAME_FLAGS);
				return (LocatorFlags)intValueHelper;
			}
			set
			{
				this.SetIntValueHelper(LdapOption.LDAP_OPT_GETDSNAME_FLAGS, (int)value);
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060002E4 RID: 740 RVA: 0x0000E090 File Offset: 0x0000D090
		public bool HostReachable
		{
			get
			{
				int intValueHelper = this.GetIntValueHelper(LdapOption.LDAP_OPT_HOST_REACHABLE);
				return intValueHelper == 1;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060002E5 RID: 741 RVA: 0x0000E0B0 File Offset: 0x0000D0B0
		// (set) Token: 0x060002E6 RID: 742 RVA: 0x0000E0D4 File Offset: 0x0000D0D4
		public TimeSpan PingKeepAliveTimeout
		{
			get
			{
				int intValueHelper = this.GetIntValueHelper(LdapOption.LDAP_OPT_PING_KEEP_ALIVE);
				return new TimeSpan((long)intValueHelper * 10000000L);
			}
			set
			{
				if (value < TimeSpan.Zero)
				{
					throw new ArgumentException(Res.GetString("NoNegativeTime"), "value");
				}
				if (value.TotalSeconds > 2147483647.0)
				{
					throw new ArgumentException(Res.GetString("TimespanExceedMax"), "value");
				}
				int num = (int)(value.Ticks / 10000000L);
				this.SetIntValueHelper(LdapOption.LDAP_OPT_PING_KEEP_ALIVE, num);
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060002E7 RID: 743 RVA: 0x0000E143 File Offset: 0x0000D143
		// (set) Token: 0x060002E8 RID: 744 RVA: 0x0000E14D File Offset: 0x0000D14D
		public int PingLimit
		{
			get
			{
				return this.GetIntValueHelper(LdapOption.LDAP_OPT_PING_LIMIT);
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException(Res.GetString("ValidValue"), "value");
				}
				this.SetIntValueHelper(LdapOption.LDAP_OPT_PING_LIMIT, value);
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060002E9 RID: 745 RVA: 0x0000E174 File Offset: 0x0000D174
		// (set) Token: 0x060002EA RID: 746 RVA: 0x0000E198 File Offset: 0x0000D198
		public TimeSpan PingWaitTimeout
		{
			get
			{
				int intValueHelper = this.GetIntValueHelper(LdapOption.LDAP_OPT_PING_WAIT_TIME);
				return new TimeSpan((long)intValueHelper * 10000L);
			}
			set
			{
				if (value < TimeSpan.Zero)
				{
					throw new ArgumentException(Res.GetString("NoNegativeTime"), "value");
				}
				if (value.TotalMilliseconds > 2147483647.0)
				{
					throw new ArgumentException(Res.GetString("TimespanExceedMax"), "value");
				}
				int num = (int)(value.Ticks / 10000L);
				this.SetIntValueHelper(LdapOption.LDAP_OPT_PING_WAIT_TIME, num);
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060002EB RID: 747 RVA: 0x0000E208 File Offset: 0x0000D208
		// (set) Token: 0x060002EC RID: 748 RVA: 0x0000E228 File Offset: 0x0000D228
		public bool AutoReconnect
		{
			get
			{
				int intValueHelper = this.GetIntValueHelper(LdapOption.LDAP_OPT_AUTO_RECONNECT);
				return intValueHelper == 1;
			}
			set
			{
				int num;
				if (value)
				{
					num = 1;
				}
				else
				{
					num = 0;
				}
				this.SetIntValueHelper(LdapOption.LDAP_OPT_AUTO_RECONNECT, num);
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060002ED RID: 749 RVA: 0x0000E24A File Offset: 0x0000D24A
		// (set) Token: 0x060002EE RID: 750 RVA: 0x0000E257 File Offset: 0x0000D257
		public int SspiFlag
		{
			get
			{
				return this.GetIntValueHelper(LdapOption.LDAP_OPT_SSPI_FLAGS);
			}
			set
			{
				this.SetIntValueHelper(LdapOption.LDAP_OPT_SSPI_FLAGS, value);
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060002EF RID: 751 RVA: 0x0000E268 File Offset: 0x0000D268
		public SecurityPackageContextConnectionInformation SslInformation
		{
			get
			{
				if (this.connection.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				SecurityPackageContextConnectionInformation securityPackageContextConnectionInformation = new SecurityPackageContextConnectionInformation();
				int num = Wldap32.ldap_get_option_secInfo(this.connection.ldapHandle, LdapOption.LDAP_OPT_SSL_INFO, securityPackageContextConnectionInformation);
				ErrorChecking.CheckAndSetLdapError(num);
				return securityPackageContextConnectionInformation;
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060002F0 RID: 752 RVA: 0x0000E2B8 File Offset: 0x0000D2B8
		public object SecurityContext
		{
			get
			{
				if (this.connection.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				SecurityHandle securityHandle = default(SecurityHandle);
				int num = Wldap32.ldap_get_option_sechandle(this.connection.ldapHandle, LdapOption.LDAP_OPT_SECURITY_CONTEXT, ref securityHandle);
				ErrorChecking.CheckAndSetLdapError(num);
				return securityHandle;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060002F1 RID: 753 RVA: 0x0000E310 File Offset: 0x0000D310
		// (set) Token: 0x060002F2 RID: 754 RVA: 0x0000E330 File Offset: 0x0000D330
		public bool Signing
		{
			get
			{
				int intValueHelper = this.GetIntValueHelper(LdapOption.LDAP_OPT_SIGN);
				return intValueHelper == 1;
			}
			set
			{
				int num;
				if (value)
				{
					num = 1;
				}
				else
				{
					num = 0;
				}
				this.SetIntValueHelper(LdapOption.LDAP_OPT_SIGN, num);
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060002F3 RID: 755 RVA: 0x0000E354 File Offset: 0x0000D354
		// (set) Token: 0x060002F4 RID: 756 RVA: 0x0000E374 File Offset: 0x0000D374
		public bool Sealing
		{
			get
			{
				int intValueHelper = this.GetIntValueHelper(LdapOption.LDAP_OPT_ENCRYPT);
				return intValueHelper == 1;
			}
			set
			{
				int num;
				if (value)
				{
					num = 1;
				}
				else
				{
					num = 0;
				}
				this.SetIntValueHelper(LdapOption.LDAP_OPT_ENCRYPT, num);
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060002F5 RID: 757 RVA: 0x0000E396 File Offset: 0x0000D396
		// (set) Token: 0x060002F6 RID: 758 RVA: 0x0000E3A4 File Offset: 0x0000D3A4
		public string SaslMethod
		{
			get
			{
				return this.GetStringValueHelper(LdapOption.LDAP_OPT_SASL_METHOD, true);
			}
			set
			{
				this.SetStringValueHelper(LdapOption.LDAP_OPT_SASL_METHOD, value);
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060002F7 RID: 759 RVA: 0x0000E3B4 File Offset: 0x0000D3B4
		// (set) Token: 0x060002F8 RID: 760 RVA: 0x0000E3D4 File Offset: 0x0000D3D4
		public bool RootDseCache
		{
			get
			{
				int intValueHelper = this.GetIntValueHelper(LdapOption.LDAP_OPT_ROOTDSE_CACHE);
				return intValueHelper == 1;
			}
			set
			{
				int num;
				if (value)
				{
					num = 1;
				}
				else
				{
					num = 0;
				}
				this.SetIntValueHelper(LdapOption.LDAP_OPT_ROOTDSE_CACHE, num);
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060002F9 RID: 761 RVA: 0x0000E3F8 File Offset: 0x0000D3F8
		// (set) Token: 0x060002FA RID: 762 RVA: 0x0000E418 File Offset: 0x0000D418
		public bool TcpKeepAlive
		{
			get
			{
				int intValueHelper = this.GetIntValueHelper(LdapOption.LDAP_OPT_TCP_KEEPALIVE);
				return intValueHelper == 1;
			}
			set
			{
				int num;
				if (value)
				{
					num = 1;
				}
				else
				{
					num = 0;
				}
				this.SetIntValueHelper(LdapOption.LDAP_OPT_TCP_KEEPALIVE, num);
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060002FB RID: 763 RVA: 0x0000E438 File Offset: 0x0000D438
		// (set) Token: 0x060002FC RID: 764 RVA: 0x0000E45C File Offset: 0x0000D45C
		public TimeSpan SendTimeout
		{
			get
			{
				int intValueHelper = this.GetIntValueHelper(LdapOption.LDAP_OPT_SEND_TIMEOUT);
				return new TimeSpan((long)intValueHelper * 10000000L);
			}
			set
			{
				if (value < TimeSpan.Zero)
				{
					throw new ArgumentException(Res.GetString("NoNegativeTime"), "value");
				}
				if (value.TotalSeconds > 2147483647.0)
				{
					throw new ArgumentException(Res.GetString("TimespanExceedMax"), "value");
				}
				int num = (int)(value.Ticks / 10000000L);
				this.SetIntValueHelper(LdapOption.LDAP_OPT_SEND_TIMEOUT, num);
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060002FD RID: 765 RVA: 0x0000E4CB File Offset: 0x0000D4CB
		// (set) Token: 0x060002FE RID: 766 RVA: 0x0000E4F4 File Offset: 0x0000D4F4
		public ReferralCallback ReferralCallback
		{
			get
			{
				if (this.connection.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				return this.callbackRoutine;
			}
			set
			{
				if (this.connection.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				ReferralCallback referralCallback = new ReferralCallback();
				if (value != null)
				{
					referralCallback.QueryForConnection = value.QueryForConnection;
					referralCallback.NotifyNewConnection = value.NotifyNewConnection;
					referralCallback.DereferenceConnection = value.DereferenceConnection;
				}
				else
				{
					referralCallback.QueryForConnection = null;
					referralCallback.NotifyNewConnection = null;
					referralCallback.DereferenceConnection = null;
				}
				this.ProcessCallBackRoutine(referralCallback);
				this.callbackRoutine = value;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060002FF RID: 767 RVA: 0x0000E571 File Offset: 0x0000D571
		// (set) Token: 0x06000300 RID: 768 RVA: 0x0000E598 File Offset: 0x0000D598
		public QueryClientCertificateCallback QueryClientCertificate
		{
			get
			{
				if (this.connection.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				return this.clientCertificateDelegate;
			}
			set
			{
				if (this.connection.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				if (value != null)
				{
					int num = Wldap32.ldap_set_option_clientcert(this.connection.ldapHandle, LdapOption.LDAP_OPT_CLIENT_CERTIFICATE, this.connection.clientCertificateRoutine);
					if (num != 0)
					{
						if (Utility.IsLdapError((LdapError)num))
						{
							string text = LdapErrorMappings.MapResultCode(num);
							throw new LdapException(num, text);
						}
						throw new LdapException(num);
					}
					else
					{
						this.connection.automaticBind = false;
					}
				}
				this.clientCertificateDelegate = value;
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000301 RID: 769 RVA: 0x0000E61B File Offset: 0x0000D61B
		// (set) Token: 0x06000302 RID: 770 RVA: 0x0000E644 File Offset: 0x0000D644
		public VerifyServerCertificateCallback VerifyServerCertificate
		{
			get
			{
				if (this.connection.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				return this.serverCertificateDelegate;
			}
			set
			{
				if (this.connection.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				if (value != null)
				{
					int num = Wldap32.ldap_set_option_servercert(this.connection.ldapHandle, LdapOption.LDAP_OPT_SERVER_CERTIFICATE, this.serverCertificateRoutine);
					ErrorChecking.CheckAndSetLdapError(num);
				}
				this.serverCertificateDelegate = value;
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000303 RID: 771 RVA: 0x0000E69B File Offset: 0x0000D69B
		internal string ServerErrorMessage
		{
			get
			{
				return this.GetStringValueHelper(LdapOption.LDAP_OPT_SERVER_ERROR, true);
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000304 RID: 772 RVA: 0x0000E6A6 File Offset: 0x0000D6A6
		// (set) Token: 0x06000305 RID: 773 RVA: 0x0000E6AF File Offset: 0x0000D6AF
		internal DereferenceAlias DerefAlias
		{
			get
			{
				return (DereferenceAlias)this.GetIntValueHelper(LdapOption.LDAP_OPT_DEREF);
			}
			set
			{
				this.SetIntValueHelper(LdapOption.LDAP_OPT_DEREF, (int)value);
			}
		}

		// Token: 0x170000CE RID: 206
		// (set) Token: 0x06000306 RID: 774 RVA: 0x0000E6B9 File Offset: 0x0000D6B9
		internal bool FQDN
		{
			set
			{
				this.SetIntValueHelper(LdapOption.LDAP_OPT_AREC_EXCLUSIVE, 1);
			}
		}

		// Token: 0x06000307 RID: 775 RVA: 0x0000E6C8 File Offset: 0x0000D6C8
		public void FastConcurrentBind()
		{
			if (this.connection.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			int num = 1;
			this.ProtocolVersion = 3;
			int num2 = Wldap32.ldap_set_option_int(this.connection.ldapHandle, LdapOption.LDAP_OPT_FAST_CONCURRENT_BIND, ref num);
			if (num2 == 89 && !Utility.IsWin2k3AboveOS)
			{
				throw new PlatformNotSupportedException(Res.GetString("ConcurrentBindNotSupport"));
			}
			ErrorChecking.CheckAndSetLdapError(num2);
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0000E734 File Offset: 0x0000D734
		public unsafe void StartTransportLayerSecurity(DirectoryControlCollection controls)
		{
			IntPtr intPtr = (IntPtr)0;
			LdapControl[] array = null;
			IntPtr intPtr2 = (IntPtr)0;
			LdapControl[] array2 = null;
			IntPtr intPtr3 = (IntPtr)0;
			IntPtr intPtr4 = (IntPtr)0;
			int num = 0;
			Uri[] array3 = null;
			if (Utility.IsWin2kOS)
			{
				throw new PlatformNotSupportedException(Res.GetString("TLSNotSupported"));
			}
			if (this.connection.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			try
			{
				IntPtr intPtr5 = (IntPtr)0;
				IntPtr intPtr6 = (IntPtr)0;
				array = this.connection.BuildControlArray(controls, true);
				int num2 = Marshal.SizeOf(typeof(LdapControl));
				if (array != null)
				{
					intPtr = Utility.AllocHGlobalIntPtrArray(array.Length + 1);
					for (int i = 0; i < array.Length; i++)
					{
						intPtr5 = Marshal.AllocHGlobal(num2);
						Marshal.StructureToPtr(array[i], intPtr5, false);
						intPtr6 = (IntPtr)((long)intPtr + (long)(Marshal.SizeOf(typeof(IntPtr)) * i));
						Marshal.WriteIntPtr(intPtr6, intPtr5);
					}
					intPtr6 = (IntPtr)((long)intPtr + (long)(Marshal.SizeOf(typeof(IntPtr)) * array.Length));
					Marshal.WriteIntPtr(intPtr6, (IntPtr)0);
				}
				array2 = this.connection.BuildControlArray(controls, false);
				if (array2 != null)
				{
					intPtr2 = Utility.AllocHGlobalIntPtrArray(array2.Length + 1);
					for (int j = 0; j < array2.Length; j++)
					{
						intPtr5 = Marshal.AllocHGlobal(num2);
						Marshal.StructureToPtr(array2[j], intPtr5, false);
						intPtr6 = (IntPtr)((long)intPtr2 + (long)(Marshal.SizeOf(typeof(IntPtr)) * j));
						Marshal.WriteIntPtr(intPtr6, intPtr5);
					}
					intPtr6 = (IntPtr)((long)intPtr2 + (long)(Marshal.SizeOf(typeof(IntPtr)) * array2.Length));
					Marshal.WriteIntPtr(intPtr6, (IntPtr)0);
				}
				int num3 = Wldap32.ldap_start_tls(this.connection.ldapHandle, ref num, ref intPtr3, intPtr, intPtr2);
				if (intPtr3 != (IntPtr)0 && Wldap32.ldap_parse_result_referral(this.connection.ldapHandle, intPtr3, (IntPtr)0, (IntPtr)0, (IntPtr)0, ref intPtr4, (IntPtr)0, 0) == 0 && intPtr4 != (IntPtr)0)
				{
					char** ptr = (char**)(void*)intPtr4;
					char* ptr2 = *(IntPtr*)(ptr + 0 / sizeof(char*));
					int num4 = 0;
					ArrayList arrayList = new ArrayList();
					while (ptr2 != null)
					{
						string text = Marshal.PtrToStringUni((IntPtr)((void*)ptr2));
						arrayList.Add(text);
						num4++;
						ptr2 = *(IntPtr*)(ptr + (IntPtr)num4 * (IntPtr)sizeof(char*) / (IntPtr)sizeof(char*));
					}
					if (intPtr4 != (IntPtr)0)
					{
						Wldap32.ldap_value_free(intPtr4);
						intPtr4 = (IntPtr)0;
					}
					if (arrayList.Count > 0)
					{
						array3 = new Uri[arrayList.Count];
						for (int k = 0; k < arrayList.Count; k++)
						{
							array3[k] = new Uri((string)arrayList[k]);
						}
					}
				}
				if (num3 != 0)
				{
					string text2 = Res.GetString("DefaultLdapError");
					if (Utility.IsResultCode((ResultCode)num3))
					{
						if (num3 == 80)
						{
							num3 = num;
						}
						text2 = OperationErrorMappings.MapResultCode(num3);
						throw new TlsOperationException(new ExtendedResponse(null, null, (ResultCode)num3, text2, array3)
						{
							name = "1.3.6.1.4.1.1466.20037"
						});
					}
					if (Utility.IsLdapError((LdapError)num3))
					{
						text2 = LdapErrorMappings.MapResultCode(num3);
						throw new LdapException(num3, text2);
					}
				}
			}
			finally
			{
				if (intPtr != (IntPtr)0)
				{
					for (int l = 0; l < array.Length; l++)
					{
						IntPtr intPtr7 = Marshal.ReadIntPtr(intPtr, Marshal.SizeOf(typeof(IntPtr)) * l);
						if (intPtr7 != (IntPtr)0)
						{
							Marshal.FreeHGlobal(intPtr7);
						}
					}
					Marshal.FreeHGlobal(intPtr);
				}
				if (array != null)
				{
					for (int m = 0; m < array.Length; m++)
					{
						if (array[m].ldctl_oid != (IntPtr)0)
						{
							Marshal.FreeHGlobal(array[m].ldctl_oid);
						}
						if (array[m].ldctl_value != null && array[m].ldctl_value.bv_val != (IntPtr)0)
						{
							Marshal.FreeHGlobal(array[m].ldctl_value.bv_val);
						}
					}
				}
				if (intPtr2 != (IntPtr)0)
				{
					for (int n = 0; n < array2.Length; n++)
					{
						IntPtr intPtr8 = Marshal.ReadIntPtr(intPtr2, Marshal.SizeOf(typeof(IntPtr)) * n);
						if (intPtr8 != (IntPtr)0)
						{
							Marshal.FreeHGlobal(intPtr8);
						}
					}
					Marshal.FreeHGlobal(intPtr2);
				}
				if (array2 != null)
				{
					for (int num5 = 0; num5 < array2.Length; num5++)
					{
						if (array2[num5].ldctl_oid != (IntPtr)0)
						{
							Marshal.FreeHGlobal(array2[num5].ldctl_oid);
						}
						if (array2[num5].ldctl_value != null && array2[num5].ldctl_value.bv_val != (IntPtr)0)
						{
							Marshal.FreeHGlobal(array2[num5].ldctl_value.bv_val);
						}
					}
				}
				if (intPtr4 != (IntPtr)0)
				{
					Wldap32.ldap_value_free(intPtr4);
				}
			}
		}

		// Token: 0x06000309 RID: 777 RVA: 0x0000EC58 File Offset: 0x0000DC58
		public void StopTransportLayerSecurity()
		{
			if (Utility.IsWin2kOS)
			{
				throw new PlatformNotSupportedException(Res.GetString("TLSNotSupported"));
			}
			if (this.connection.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			if (Wldap32.ldap_stop_tls(this.connection.ldapHandle) == 0)
			{
				throw new TlsOperationException(null, Res.GetString("TLSStopFailure"));
			}
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0000ECC0 File Offset: 0x0000DCC0
		private int GetIntValueHelper(LdapOption option)
		{
			if (this.connection.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			int num = 0;
			int num2 = Wldap32.ldap_get_option_int(this.connection.ldapHandle, option, ref num);
			ErrorChecking.CheckAndSetLdapError(num2);
			return num;
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0000ED08 File Offset: 0x0000DD08
		private void SetIntValueHelper(LdapOption option, int value)
		{
			if (this.connection.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			int num = value;
			int num2 = Wldap32.ldap_set_option_int(this.connection.ldapHandle, option, ref num);
			ErrorChecking.CheckAndSetLdapError(num2);
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0000ED50 File Offset: 0x0000DD50
		private string GetStringValueHelper(LdapOption option, bool releasePtr)
		{
			if (this.connection.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			IntPtr intPtr = new IntPtr(0);
			int num = Wldap32.ldap_get_option_ptr(this.connection.ldapHandle, option, ref intPtr);
			ErrorChecking.CheckAndSetLdapError(num);
			string text = null;
			if (intPtr != (IntPtr)0)
			{
				text = Marshal.PtrToStringUni(intPtr);
			}
			if (releasePtr)
			{
				Wldap32.ldap_memfree(intPtr);
			}
			return text;
		}

		// Token: 0x0600030D RID: 781 RVA: 0x0000EDC4 File Offset: 0x0000DDC4
		private void SetStringValueHelper(LdapOption option, string value)
		{
			if (this.connection.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			IntPtr intPtr = new IntPtr(0);
			if (value != null)
			{
				intPtr = Marshal.StringToHGlobalUni(value);
			}
			try
			{
				int num = Wldap32.ldap_set_option_ptr(this.connection.ldapHandle, option, ref intPtr);
				ErrorChecking.CheckAndSetLdapError(num);
			}
			finally
			{
				if (intPtr != (IntPtr)0)
				{
					Marshal.FreeHGlobal(intPtr);
				}
			}
		}

		// Token: 0x0600030E RID: 782 RVA: 0x0000EE48 File Offset: 0x0000DE48
		private void ProcessCallBackRoutine(ReferralCallback tempCallback)
		{
			LdapReferralCallback ldapReferralCallback = default(LdapReferralCallback);
			ldapReferralCallback.sizeofcallback = Marshal.SizeOf(typeof(LdapReferralCallback));
			ldapReferralCallback.query = ((tempCallback.QueryForConnection == null) ? null : this.queryDelegate);
			ldapReferralCallback.notify = ((tempCallback.NotifyNewConnection == null) ? null : this.notifiyDelegate);
			ldapReferralCallback.dereference = ((tempCallback.DereferenceConnection == null) ? null : this.dereferenceDelegate);
			int num = Wldap32.ldap_set_option_referral(this.connection.ldapHandle, LdapOption.LDAP_OPT_REFERRAL_CALLBACK, ref ldapReferralCallback);
			ErrorChecking.CheckAndSetLdapError(num);
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0000EED8 File Offset: 0x0000DED8
		private int ProcessQueryConnection(IntPtr PrimaryConnection, IntPtr ReferralFromConnection, IntPtr NewDNPtr, string HostName, int PortNumber, SEC_WINNT_AUTH_IDENTITY_EX SecAuthIdentity, Luid CurrentUserToken, ref IntPtr ConnectionToUse)
		{
			ConnectionToUse = (IntPtr)0;
			string text = null;
			if (this.callbackRoutine.QueryForConnection != null)
			{
				if (NewDNPtr != (IntPtr)0)
				{
					text = Marshal.PtrToStringUni(NewDNPtr);
				}
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(HostName);
				stringBuilder.Append(":");
				stringBuilder.Append(PortNumber);
				LdapDirectoryIdentifier ldapDirectoryIdentifier = new LdapDirectoryIdentifier(stringBuilder.ToString());
				NetworkCredential networkCredential = this.ProcessSecAuthIdentity(SecAuthIdentity);
				LdapConnection ldapConnection = null;
				if (ReferralFromConnection != (IntPtr)0)
				{
					lock (LdapConnection.objectLock)
					{
						WeakReference weakReference = (WeakReference)LdapConnection.handleTable[ReferralFromConnection];
						if (weakReference != null && weakReference.IsAlive)
						{
							ldapConnection = (LdapConnection)weakReference.Target;
						}
						else
						{
							if (weakReference != null)
							{
								LdapConnection.handleTable.Remove(ReferralFromConnection);
							}
							ldapConnection = new LdapConnection((LdapDirectoryIdentifier)this.connection.Directory, this.connection.GetCredential(), this.connection.AuthType, ReferralFromConnection);
							LdapConnection.handleTable.Add(ReferralFromConnection, new WeakReference(ldapConnection));
						}
					}
				}
				long num = (long)((ulong)CurrentUserToken.LowPart + (ulong)((ulong)((long)CurrentUserToken.HighPart) << 32));
				LdapConnection ldapConnection2 = this.callbackRoutine.QueryForConnection(this.connection, ldapConnection, text, ldapDirectoryIdentifier, networkCredential, num);
				if (ldapConnection2 != null)
				{
					ConnectionToUse = ldapConnection2.ldapHandle;
				}
				return 0;
			}
			return 1;
		}

		// Token: 0x06000310 RID: 784 RVA: 0x0000F068 File Offset: 0x0000E068
		private bool ProcessNotifyConnection(IntPtr PrimaryConnection, IntPtr ReferralFromConnection, IntPtr NewDNPtr, string HostName, IntPtr NewConnection, int PortNumber, SEC_WINNT_AUTH_IDENTITY_EX SecAuthIdentity, Luid CurrentUser, int ErrorCodeFromBind)
		{
			string text = null;
			if (NewConnection != (IntPtr)0 && this.callbackRoutine.NotifyNewConnection != null)
			{
				if (NewDNPtr != (IntPtr)0)
				{
					text = Marshal.PtrToStringUni(NewDNPtr);
				}
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(HostName);
				stringBuilder.Append(":");
				stringBuilder.Append(PortNumber);
				LdapDirectoryIdentifier ldapDirectoryIdentifier = new LdapDirectoryIdentifier(stringBuilder.ToString());
				NetworkCredential networkCredential = this.ProcessSecAuthIdentity(SecAuthIdentity);
				LdapConnection ldapConnection = null;
				LdapConnection ldapConnection2 = null;
				lock (LdapConnection.objectLock)
				{
					if (ReferralFromConnection != (IntPtr)0)
					{
						WeakReference weakReference = (WeakReference)LdapConnection.handleTable[ReferralFromConnection];
						if (weakReference != null && weakReference.IsAlive)
						{
							ldapConnection2 = (LdapConnection)weakReference.Target;
						}
						else
						{
							if (weakReference != null)
							{
								LdapConnection.handleTable.Remove(ReferralFromConnection);
							}
							ldapConnection2 = new LdapConnection((LdapDirectoryIdentifier)this.connection.Directory, this.connection.GetCredential(), this.connection.AuthType, ReferralFromConnection);
							LdapConnection.handleTable.Add(ReferralFromConnection, new WeakReference(ldapConnection2));
						}
					}
					if (NewConnection != (IntPtr)0)
					{
						WeakReference weakReference = (WeakReference)LdapConnection.handleTable[NewConnection];
						if (weakReference != null && weakReference.IsAlive)
						{
							ldapConnection = (LdapConnection)weakReference.Target;
						}
						else
						{
							if (weakReference != null)
							{
								LdapConnection.handleTable.Remove(NewConnection);
							}
							ldapConnection = new LdapConnection(ldapDirectoryIdentifier, networkCredential, this.connection.AuthType, NewConnection);
							LdapConnection.handleTable.Add(NewConnection, new WeakReference(ldapConnection));
						}
					}
				}
				long num = (long)((ulong)CurrentUser.LowPart + (ulong)((ulong)((long)CurrentUser.HighPart) << 32));
				bool flag = this.callbackRoutine.NotifyNewConnection(this.connection, ldapConnection2, text, ldapDirectoryIdentifier, ldapConnection, networkCredential, num, ErrorCodeFromBind);
				if (flag)
				{
					ldapConnection.needDispose = true;
				}
				return flag;
			}
			return false;
		}

		// Token: 0x06000311 RID: 785 RVA: 0x0000F294 File Offset: 0x0000E294
		private int ProcessDereferenceConnection(IntPtr PrimaryConnection, IntPtr ConnectionToDereference)
		{
			if (ConnectionToDereference != (IntPtr)0 && this.callbackRoutine.DereferenceConnection != null)
			{
				WeakReference weakReference = null;
				lock (LdapConnection.objectLock)
				{
					weakReference = (WeakReference)LdapConnection.handleTable[ConnectionToDereference];
				}
				LdapConnection ldapConnection;
				if (weakReference == null || !weakReference.IsAlive)
				{
					ldapConnection = new LdapConnection((LdapDirectoryIdentifier)this.connection.Directory, this.connection.GetCredential(), this.connection.AuthType, ConnectionToDereference);
				}
				else
				{
					ldapConnection = (LdapConnection)weakReference.Target;
				}
				this.callbackRoutine.DereferenceConnection(this.connection, ldapConnection);
			}
			return 1;
		}

		// Token: 0x06000312 RID: 786 RVA: 0x0000F360 File Offset: 0x0000E360
		private NetworkCredential ProcessSecAuthIdentity(SEC_WINNT_AUTH_IDENTITY_EX SecAuthIdentit)
		{
			if (SecAuthIdentit == null)
			{
				return new NetworkCredential();
			}
			string user = SecAuthIdentit.user;
			string domain = SecAuthIdentit.domain;
			string password = SecAuthIdentit.password;
			return new NetworkCredential(user, password, domain);
		}

		// Token: 0x06000313 RID: 787 RVA: 0x0000F394 File Offset: 0x0000E394
		private bool ProcessServerCertificate(IntPtr Connection, IntPtr pServerCert)
		{
			bool flag = true;
			if (this.serverCertificateDelegate != null)
			{
				IntPtr intPtr = (IntPtr)0;
				X509Certificate x509Certificate = null;
				try
				{
					intPtr = Marshal.ReadIntPtr(pServerCert);
					x509Certificate = new X509Certificate(intPtr);
				}
				finally
				{
					Wldap32.CertFreeCRLContext(intPtr);
				}
				flag = this.serverCertificateDelegate(this.connection, x509Certificate);
			}
			return flag;
		}

		// Token: 0x0400029D RID: 669
		private LdapConnection connection;

		// Token: 0x0400029E RID: 670
		private ReferralCallback callbackRoutine = new ReferralCallback();

		// Token: 0x0400029F RID: 671
		internal QueryClientCertificateCallback clientCertificateDelegate;

		// Token: 0x040002A0 RID: 672
		private VerifyServerCertificateCallback serverCertificateDelegate;

		// Token: 0x040002A1 RID: 673
		private QUERYFORCONNECTIONInternal queryDelegate;

		// Token: 0x040002A2 RID: 674
		private NOTIFYOFNEWCONNECTIONInternal notifiyDelegate;

		// Token: 0x040002A3 RID: 675
		private DEREFERENCECONNECTIONInternal dereferenceDelegate;

		// Token: 0x040002A4 RID: 676
		private VERIFYSERVERCERT serverCertificateRoutine;
	}
}
