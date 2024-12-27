using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Management
{
	// Token: 0x02000034 RID: 52
	public class ConnectionOptions : ManagementOptions
	{
		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600018D RID: 397 RVA: 0x00008A87 File Offset: 0x00007A87
		// (set) Token: 0x0600018E RID: 398 RVA: 0x00008A9D File Offset: 0x00007A9D
		public string Locale
		{
			get
			{
				if (this.locale == null)
				{
					return string.Empty;
				}
				return this.locale;
			}
			set
			{
				if (this.locale != value)
				{
					this.locale = value;
					base.FireIdentifierChanged();
				}
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600018F RID: 399 RVA: 0x00008ABA File Offset: 0x00007ABA
		// (set) Token: 0x06000190 RID: 400 RVA: 0x00008AC2 File Offset: 0x00007AC2
		public string Username
		{
			get
			{
				return this.username;
			}
			set
			{
				if (this.username != value)
				{
					this.username = value;
					base.FireIdentifierChanged();
				}
			}
		}

		// Token: 0x17000047 RID: 71
		// (set) Token: 0x06000191 RID: 401 RVA: 0x00008AE0 File Offset: 0x00007AE0
		public string Password
		{
			set
			{
				if (value == null)
				{
					if (this.securePassword != null)
					{
						this.securePassword.Dispose();
						this.securePassword = null;
						base.FireIdentifierChanged();
					}
					return;
				}
				if (this.securePassword == null)
				{
					this.securePassword = new SecureString();
					for (int i = 0; i < value.Length; i++)
					{
						this.securePassword.AppendChar(value[i]);
					}
					return;
				}
				SecureString secureString = new SecureString();
				for (int j = 0; j < value.Length; j++)
				{
					secureString.AppendChar(value[j]);
				}
				this.securePassword.Clear();
				this.securePassword = secureString.Copy();
				base.FireIdentifierChanged();
				secureString.Dispose();
			}
		}

		// Token: 0x17000048 RID: 72
		// (set) Token: 0x06000192 RID: 402 RVA: 0x00008B90 File Offset: 0x00007B90
		public SecureString SecurePassword
		{
			set
			{
				if (value == null)
				{
					if (this.securePassword != null)
					{
						this.securePassword.Dispose();
						this.securePassword = null;
						base.FireIdentifierChanged();
					}
					return;
				}
				if (this.securePassword == null)
				{
					this.securePassword = value.Copy();
					return;
				}
				this.securePassword.Clear();
				this.securePassword = value.Copy();
				base.FireIdentifierChanged();
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000193 RID: 403 RVA: 0x00008BF3 File Offset: 0x00007BF3
		// (set) Token: 0x06000194 RID: 404 RVA: 0x00008C09 File Offset: 0x00007C09
		public string Authority
		{
			get
			{
				if (this.authority == null)
				{
					return string.Empty;
				}
				return this.authority;
			}
			set
			{
				if (this.authority != value)
				{
					this.authority = value;
					base.FireIdentifierChanged();
				}
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000195 RID: 405 RVA: 0x00008C26 File Offset: 0x00007C26
		// (set) Token: 0x06000196 RID: 406 RVA: 0x00008C2E File Offset: 0x00007C2E
		public ImpersonationLevel Impersonation
		{
			get
			{
				return this.impersonation;
			}
			set
			{
				if (this.impersonation != value)
				{
					this.impersonation = value;
					base.FireIdentifierChanged();
				}
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000197 RID: 407 RVA: 0x00008C46 File Offset: 0x00007C46
		// (set) Token: 0x06000198 RID: 408 RVA: 0x00008C4E File Offset: 0x00007C4E
		public AuthenticationLevel Authentication
		{
			get
			{
				return this.authentication;
			}
			set
			{
				if (this.authentication != value)
				{
					this.authentication = value;
					base.FireIdentifierChanged();
				}
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000199 RID: 409 RVA: 0x00008C66 File Offset: 0x00007C66
		// (set) Token: 0x0600019A RID: 410 RVA: 0x00008C6E File Offset: 0x00007C6E
		public bool EnablePrivileges
		{
			get
			{
				return this.enablePrivileges;
			}
			set
			{
				if (this.enablePrivileges != value)
				{
					this.enablePrivileges = value;
					base.FireIdentifierChanged();
				}
			}
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00008C88 File Offset: 0x00007C88
		public ConnectionOptions()
			: this(null, null, null, null, ImpersonationLevel.Impersonate, AuthenticationLevel.Unchanged, false, null, ManagementOptions.InfiniteTimeout)
		{
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00008CA8 File Offset: 0x00007CA8
		public ConnectionOptions(string locale, string username, string password, string authority, ImpersonationLevel impersonation, AuthenticationLevel authentication, bool enablePrivileges, ManagementNamedValueCollection context, TimeSpan timeout)
			: base(context, timeout)
		{
			if (locale != null)
			{
				this.locale = locale;
			}
			this.username = username;
			this.enablePrivileges = enablePrivileges;
			if (password != null)
			{
				this.securePassword = new SecureString();
				for (int i = 0; i < password.Length; i++)
				{
					this.securePassword.AppendChar(password[i]);
				}
			}
			if (authority != null)
			{
				this.authority = authority;
			}
			if (impersonation != ImpersonationLevel.Default)
			{
				this.impersonation = impersonation;
			}
			if (authentication != AuthenticationLevel.Default)
			{
				this.authentication = authentication;
			}
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00008D30 File Offset: 0x00007D30
		public ConnectionOptions(string locale, string username, SecureString password, string authority, ImpersonationLevel impersonation, AuthenticationLevel authentication, bool enablePrivileges, ManagementNamedValueCollection context, TimeSpan timeout)
			: base(context, timeout)
		{
			if (locale != null)
			{
				this.locale = locale;
			}
			this.username = username;
			this.enablePrivileges = enablePrivileges;
			if (password != null)
			{
				this.securePassword = password.Copy();
			}
			if (authority != null)
			{
				this.authority = authority;
			}
			if (impersonation != ImpersonationLevel.Default)
			{
				this.impersonation = impersonation;
			}
			if (authentication != AuthenticationLevel.Default)
			{
				this.authentication = authentication;
			}
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00008D94 File Offset: 0x00007D94
		public override object Clone()
		{
			ManagementNamedValueCollection managementNamedValueCollection = null;
			if (base.Context != null)
			{
				managementNamedValueCollection = base.Context.Clone();
			}
			return new ConnectionOptions(this.locale, this.username, this.GetSecurePassword(), this.authority, this.impersonation, this.authentication, this.enablePrivileges, managementNamedValueCollection, base.Timeout);
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00008DF0 File Offset: 0x00007DF0
		internal IntPtr GetPassword()
		{
			if (this.securePassword != null)
			{
				try
				{
					return Marshal.SecureStringToBSTR(this.securePassword);
				}
				catch (OutOfMemoryException)
				{
					return IntPtr.Zero;
				}
			}
			return IntPtr.Zero;
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00008E34 File Offset: 0x00007E34
		internal SecureString GetSecurePassword()
		{
			if (this.securePassword != null)
			{
				return this.securePassword.Copy();
			}
			return null;
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00008E4B File Offset: 0x00007E4B
		internal ConnectionOptions(ManagementNamedValueCollection context, TimeSpan timeout, int flags)
			: base(context, timeout, flags)
		{
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00008E56 File Offset: 0x00007E56
		internal ConnectionOptions(ManagementNamedValueCollection context)
			: base(context, ManagementOptions.InfiniteTimeout)
		{
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00008E64 File Offset: 0x00007E64
		internal static ConnectionOptions _Clone(ConnectionOptions options)
		{
			return ConnectionOptions._Clone(options, null);
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00008E70 File Offset: 0x00007E70
		internal static ConnectionOptions _Clone(ConnectionOptions options, IdentifierChangedEventHandler handler)
		{
			ConnectionOptions connectionOptions;
			if (options != null)
			{
				connectionOptions = new ConnectionOptions(options.Context, options.Timeout, options.Flags);
				connectionOptions.locale = options.locale;
				connectionOptions.username = options.username;
				connectionOptions.enablePrivileges = options.enablePrivileges;
				if (options.securePassword != null)
				{
					connectionOptions.securePassword = options.securePassword.Copy();
				}
				else
				{
					connectionOptions.securePassword = null;
				}
				if (options.authority != null)
				{
					connectionOptions.authority = options.authority;
				}
				if (options.impersonation != ImpersonationLevel.Default)
				{
					connectionOptions.impersonation = options.impersonation;
				}
				if (options.authentication != AuthenticationLevel.Default)
				{
					connectionOptions.authentication = options.authentication;
				}
			}
			else
			{
				connectionOptions = new ConnectionOptions();
			}
			if (handler != null)
			{
				connectionOptions.IdentifierChanged += handler;
			}
			else if (options != null)
			{
				connectionOptions.IdentifierChanged += options.HandleIdentifierChange;
			}
			return connectionOptions;
		}

		// Token: 0x0400013F RID: 319
		internal const string DEFAULTLOCALE = null;

		// Token: 0x04000140 RID: 320
		internal const string DEFAULTAUTHORITY = null;

		// Token: 0x04000141 RID: 321
		internal const ImpersonationLevel DEFAULTIMPERSONATION = ImpersonationLevel.Impersonate;

		// Token: 0x04000142 RID: 322
		internal const AuthenticationLevel DEFAULTAUTHENTICATION = AuthenticationLevel.Unchanged;

		// Token: 0x04000143 RID: 323
		internal const bool DEFAULTENABLEPRIVILEGES = false;

		// Token: 0x04000144 RID: 324
		private string locale;

		// Token: 0x04000145 RID: 325
		private string username;

		// Token: 0x04000146 RID: 326
		private SecureString securePassword;

		// Token: 0x04000147 RID: 327
		private string authority;

		// Token: 0x04000148 RID: 328
		private ImpersonationLevel impersonation;

		// Token: 0x04000149 RID: 329
		private AuthenticationLevel authentication;

		// Token: 0x0400014A RID: 330
		private bool enablePrivileges;
	}
}
