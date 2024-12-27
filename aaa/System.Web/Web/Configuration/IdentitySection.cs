using System;
using System.Configuration;
using System.Security.Permissions;
using System.Text;
using System.Web.Util;

namespace System.Web.Configuration
{
	// Token: 0x02000206 RID: 518
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class IdentitySection : ConfigurationSection
	{
		// Token: 0x06001C14 RID: 7188 RVA: 0x00080894 File Offset: 0x0007F894
		static IdentitySection()
		{
			IdentitySection._properties.Add(IdentitySection._propImpersonate);
			IdentitySection._properties.Add(IdentitySection._propUserName);
			IdentitySection._properties.Add(IdentitySection._propPassword);
		}

		// Token: 0x06001C15 RID: 7189 RVA: 0x00080938 File Offset: 0x0007F938
		protected override object GetRuntimeObject()
		{
			if (!this._credentialsValidated)
			{
				lock (this._credentialsValidatedLock)
				{
					if (!this._credentialsValidated)
					{
						this.ValidateCredentials();
						this._credentialsValidated = true;
					}
				}
			}
			return base.GetRuntimeObject();
		}

		// Token: 0x06001C16 RID: 7190 RVA: 0x00080990 File Offset: 0x0007F990
		public IdentitySection()
		{
			this.impersonateCached = false;
		}

		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x06001C17 RID: 7191 RVA: 0x000809C5 File Offset: 0x0007F9C5
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return IdentitySection._properties;
			}
		}

		// Token: 0x1700057F RID: 1407
		// (get) Token: 0x06001C18 RID: 7192 RVA: 0x000809CC File Offset: 0x0007F9CC
		// (set) Token: 0x06001C19 RID: 7193 RVA: 0x000809F9 File Offset: 0x0007F9F9
		[ConfigurationProperty("impersonate", DefaultValue = false)]
		public bool Impersonate
		{
			get
			{
				if (!this.impersonateCached)
				{
					this.impersonateCache = (bool)base[IdentitySection._propImpersonate];
					this.impersonateCached = true;
				}
				return this.impersonateCache;
			}
			set
			{
				base[IdentitySection._propImpersonate] = value;
				this.impersonateCache = value;
			}
		}

		// Token: 0x17000580 RID: 1408
		// (get) Token: 0x06001C1A RID: 7194 RVA: 0x00080A13 File Offset: 0x0007FA13
		// (set) Token: 0x06001C1B RID: 7195 RVA: 0x00080A25 File Offset: 0x0007FA25
		[ConfigurationProperty("userName", DefaultValue = "")]
		public string UserName
		{
			get
			{
				return (string)base[IdentitySection._propUserName];
			}
			set
			{
				base[IdentitySection._propUserName] = value;
			}
		}

		// Token: 0x17000581 RID: 1409
		// (get) Token: 0x06001C1C RID: 7196 RVA: 0x00080A33 File Offset: 0x0007FA33
		// (set) Token: 0x06001C1D RID: 7197 RVA: 0x00080A45 File Offset: 0x0007FA45
		[ConfigurationProperty("password", DefaultValue = "")]
		public string Password
		{
			get
			{
				return (string)base[IdentitySection._propPassword];
			}
			set
			{
				base[IdentitySection._propPassword] = value;
			}
		}

		// Token: 0x06001C1E RID: 7198 RVA: 0x00080A54 File Offset: 0x0007FA54
		protected override void Reset(ConfigurationElement parentElement)
		{
			base.Reset(parentElement);
			IdentitySection identitySection = parentElement as IdentitySection;
			if (identitySection != null)
			{
				this._impersonateTokenRef = identitySection._impersonateTokenRef;
				if (this.Impersonate)
				{
					this.UserName = null;
					this.Password = null;
					this._impersonateTokenRef = new ImpersonateTokenRef(IntPtr.Zero);
				}
				this.impersonateCached = false;
				this._credentialsValidated = false;
			}
		}

		// Token: 0x06001C1F RID: 7199 RVA: 0x00080AB4 File Offset: 0x0007FAB4
		protected override void Unmerge(ConfigurationElement sourceElement, ConfigurationElement parentElement, ConfigurationSaveMode saveMode)
		{
			base.Unmerge(sourceElement, parentElement, saveMode);
			IdentitySection identitySection = sourceElement as IdentitySection;
			if (this.Impersonate != identitySection.Impersonate)
			{
				this.Impersonate = identitySection.Impersonate;
			}
			if (this.Impersonate && (identitySection.ElementInformation.Properties[IdentitySection._propUserName.Name].IsModified || identitySection.ElementInformation.Properties[IdentitySection._propPassword.Name].IsModified))
			{
				this.UserName = identitySection.UserName;
				this.Password = identitySection.Password;
			}
		}

		// Token: 0x06001C20 RID: 7200 RVA: 0x00080B50 File Offset: 0x0007FB50
		private void ValidateCredentials()
		{
			this._username = this.UserName;
			this._password = this.Password;
			if (!HandlerBase.CheckAndReadRegistryValue(ref this._username, false))
			{
				throw new ConfigurationErrorsException(SR.GetString("Invalid_registry_config"), base.ElementInformation.Source, base.ElementInformation.LineNumber);
			}
			if (!HandlerBase.CheckAndReadRegistryValue(ref this._password, false))
			{
				throw new ConfigurationErrorsException(SR.GetString("Invalid_registry_config"), base.ElementInformation.Source, base.ElementInformation.LineNumber);
			}
			if (this._username != null && this._username.Length < 1)
			{
				this._username = null;
			}
			if (this._username != null && this.Impersonate)
			{
				if (this._password == null)
				{
					this._password = string.Empty;
				}
			}
			else if (this._password != null && this._username == null && this._password.Length > 0 && this.Impersonate)
			{
				throw new ConfigurationErrorsException(SR.GetString("Invalid_credentials"), base.ElementInformation.Properties["password"].Source, base.ElementInformation.Properties["password"].LineNumber);
			}
			if (!this.Impersonate || !(this.ImpersonateToken == IntPtr.Zero) || this._username == null)
			{
				return;
			}
			if (this.error.Length > 0)
			{
				throw new ConfigurationErrorsException(SR.GetString("Invalid_credentials_2", new object[] { this.error }), base.ElementInformation.Properties["userName"].Source, base.ElementInformation.Properties["userName"].LineNumber);
			}
			throw new ConfigurationErrorsException(SR.GetString("Invalid_credentials"), base.ElementInformation.Properties["userName"].Source, base.ElementInformation.Properties["userName"].LineNumber);
		}

		// Token: 0x06001C21 RID: 7201 RVA: 0x00080D5C File Offset: 0x0007FD5C
		private void InitializeToken()
		{
			this.error = string.Empty;
			IntPtr intPtr = IdentitySection.CreateUserToken(this._username, this._password, out this.error);
			this._impersonateTokenRef = new ImpersonateTokenRef(intPtr);
			if (!(this._impersonateTokenRef.Handle == IntPtr.Zero))
			{
				return;
			}
			if (this.error.Length > 0)
			{
				throw new ConfigurationErrorsException(SR.GetString("Invalid_credentials_2", new object[] { this.error }), base.ElementInformation.Properties["userName"].Source, base.ElementInformation.Properties["userName"].LineNumber);
			}
			throw new ConfigurationErrorsException(SR.GetString("Invalid_credentials"), base.ElementInformation.Properties["userName"].Source, base.ElementInformation.Properties["userName"].LineNumber);
		}

		// Token: 0x17000582 RID: 1410
		// (get) Token: 0x06001C22 RID: 7202 RVA: 0x00080E59 File Offset: 0x0007FE59
		internal IntPtr ImpersonateToken
		{
			get
			{
				if (this._impersonateTokenRef.Handle == IntPtr.Zero && this._username != null && this.Impersonate)
				{
					this.InitializeToken();
				}
				return this._impersonateTokenRef.Handle;
			}
		}

		// Token: 0x06001C23 RID: 7203 RVA: 0x00080E94 File Offset: 0x0007FE94
		internal static IntPtr CreateUserToken(string name, string password, out string error)
		{
			IntPtr intPtr = IntPtr.Zero;
			if (VersionInfo.ExeName == "aspnet_wp")
			{
				byte[] array = new byte[IntPtr.Size];
				byte[] bytes = Encoding.Unicode.GetBytes(name + "\t" + password);
				byte[] array2 = new byte[bytes.Length + 2];
				Buffer.BlockCopy(bytes, 0, array2, 0, bytes.Length);
				if (UnsafeNativeMethods.PMCallISAPI(IntPtr.Zero, UnsafeNativeMethods.CallISAPIFunc.GenerateToken, array2, array2.Length, array, array.Length) == 1)
				{
					long num = 0L;
					for (int i = 0; i < IntPtr.Size; i++)
					{
						num = num * 256L + (long)((ulong)array[i]);
					}
					intPtr = (IntPtr)num;
				}
			}
			if (intPtr == IntPtr.Zero)
			{
				StringBuilder stringBuilder = new StringBuilder(256);
				intPtr = UnsafeNativeMethods.CreateUserToken(name, password, 1, stringBuilder, 256);
				error = stringBuilder.ToString();
				if (intPtr != IntPtr.Zero)
				{
				}
			}
			else
			{
				error = string.Empty;
			}
			intPtr == IntPtr.Zero;
			return intPtr;
		}

		// Token: 0x17000583 RID: 1411
		// (get) Token: 0x06001C24 RID: 7204 RVA: 0x00080F90 File Offset: 0x0007FF90
		internal ContextInformation ProtectedEvaluationContext
		{
			get
			{
				return base.EvaluationContext;
			}
		}

		// Token: 0x040018AE RID: 6318
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x040018AF RID: 6319
		private static readonly ConfigurationProperty _propImpersonate = new ConfigurationProperty("impersonate", typeof(bool), false, ConfigurationPropertyOptions.None);

		// Token: 0x040018B0 RID: 6320
		private static readonly ConfigurationProperty _propUserName = new ConfigurationProperty("userName", typeof(string), string.Empty, ConfigurationPropertyOptions.None);

		// Token: 0x040018B1 RID: 6321
		private static readonly ConfigurationProperty _propPassword = new ConfigurationProperty("password", typeof(string), string.Empty, ConfigurationPropertyOptions.None);

		// Token: 0x040018B2 RID: 6322
		private ImpersonateTokenRef _impersonateTokenRef = new ImpersonateTokenRef(IntPtr.Zero);

		// Token: 0x040018B3 RID: 6323
		private string _username;

		// Token: 0x040018B4 RID: 6324
		private string _password;

		// Token: 0x040018B5 RID: 6325
		private bool impersonateCache;

		// Token: 0x040018B6 RID: 6326
		private bool impersonateCached;

		// Token: 0x040018B7 RID: 6327
		private bool _credentialsValidated;

		// Token: 0x040018B8 RID: 6328
		private object _credentialsValidatedLock = new object();

		// Token: 0x040018B9 RID: 6329
		private string error = string.Empty;
	}
}
