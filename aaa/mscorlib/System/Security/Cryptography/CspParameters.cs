using System;
using System.Runtime.InteropServices;
using System.Security.AccessControl;

namespace System.Security.Cryptography
{
	// Token: 0x0200085D RID: 2141
	[ComVisible(true)]
	public sealed class CspParameters
	{
		// Token: 0x17000DB1 RID: 3505
		// (get) Token: 0x06004E7B RID: 20091 RVA: 0x001109C5 File Offset: 0x0010F9C5
		// (set) Token: 0x06004E7C RID: 20092 RVA: 0x001109D0 File Offset: 0x0010F9D0
		public CspProviderFlags Flags
		{
			get
			{
				return (CspProviderFlags)this.m_flags;
			}
			set
			{
				uint num = 2147483775U;
				if ((value & (CspProviderFlags)(~(CspProviderFlags)num)) != CspProviderFlags.NoFlags)
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_EnumIllegalVal", new object[] { (int)value }), "value");
				}
				this.m_flags = (uint)value;
			}
		}

		// Token: 0x17000DB2 RID: 3506
		// (get) Token: 0x06004E7D RID: 20093 RVA: 0x00110A18 File Offset: 0x0010FA18
		// (set) Token: 0x06004E7E RID: 20094 RVA: 0x00110A20 File Offset: 0x0010FA20
		public CryptoKeySecurity CryptoKeySecurity
		{
			get
			{
				return this.m_cryptoKeySecurity;
			}
			set
			{
				this.m_cryptoKeySecurity = value;
			}
		}

		// Token: 0x17000DB3 RID: 3507
		// (get) Token: 0x06004E7F RID: 20095 RVA: 0x00110A29 File Offset: 0x0010FA29
		// (set) Token: 0x06004E80 RID: 20096 RVA: 0x00110A31 File Offset: 0x0010FA31
		public SecureString KeyPassword
		{
			get
			{
				return this.m_keyPassword;
			}
			set
			{
				this.m_keyPassword = value;
				this.m_parentWindowHandle = IntPtr.Zero;
			}
		}

		// Token: 0x17000DB4 RID: 3508
		// (get) Token: 0x06004E81 RID: 20097 RVA: 0x00110A45 File Offset: 0x0010FA45
		// (set) Token: 0x06004E82 RID: 20098 RVA: 0x00110A4D File Offset: 0x0010FA4D
		public IntPtr ParentWindowHandle
		{
			get
			{
				return this.m_parentWindowHandle;
			}
			set
			{
				this.m_parentWindowHandle = value;
				this.m_keyPassword = null;
			}
		}

		// Token: 0x06004E83 RID: 20099 RVA: 0x00110A5D File Offset: 0x0010FA5D
		public CspParameters()
			: this(Utils.DefaultRsaProviderType, null, null)
		{
		}

		// Token: 0x06004E84 RID: 20100 RVA: 0x00110A6C File Offset: 0x0010FA6C
		public CspParameters(int dwTypeIn)
			: this(dwTypeIn, null, null)
		{
		}

		// Token: 0x06004E85 RID: 20101 RVA: 0x00110A77 File Offset: 0x0010FA77
		public CspParameters(int dwTypeIn, string strProviderNameIn)
			: this(dwTypeIn, strProviderNameIn, null)
		{
		}

		// Token: 0x06004E86 RID: 20102 RVA: 0x00110A82 File Offset: 0x0010FA82
		public CspParameters(int dwTypeIn, string strProviderNameIn, string strContainerNameIn)
			: this(dwTypeIn, strProviderNameIn, strContainerNameIn, CspProviderFlags.NoFlags)
		{
		}

		// Token: 0x06004E87 RID: 20103 RVA: 0x00110A8E File Offset: 0x0010FA8E
		public CspParameters(int providerType, string providerName, string keyContainerName, CryptoKeySecurity cryptoKeySecurity, SecureString keyPassword)
			: this(providerType, providerName, keyContainerName)
		{
			this.m_cryptoKeySecurity = cryptoKeySecurity;
			this.m_keyPassword = keyPassword;
		}

		// Token: 0x06004E88 RID: 20104 RVA: 0x00110AA9 File Offset: 0x0010FAA9
		public CspParameters(int providerType, string providerName, string keyContainerName, CryptoKeySecurity cryptoKeySecurity, IntPtr parentWindowHandle)
			: this(providerType, providerName, keyContainerName)
		{
			this.m_cryptoKeySecurity = cryptoKeySecurity;
			this.m_parentWindowHandle = parentWindowHandle;
		}

		// Token: 0x06004E89 RID: 20105 RVA: 0x00110AC4 File Offset: 0x0010FAC4
		internal CspParameters(int providerType, string providerName, string keyContainerName, CspProviderFlags flags)
		{
			this.ProviderType = providerType;
			this.ProviderName = providerName;
			this.KeyContainerName = keyContainerName;
			this.KeyNumber = -1;
			this.Flags = flags;
		}

		// Token: 0x06004E8A RID: 20106 RVA: 0x00110AF0 File Offset: 0x0010FAF0
		internal CspParameters(CspParameters parameters)
		{
			this.ProviderType = parameters.ProviderType;
			this.ProviderName = parameters.ProviderName;
			this.KeyContainerName = parameters.KeyContainerName;
			this.KeyNumber = parameters.KeyNumber;
			this.Flags = parameters.Flags;
			this.m_cryptoKeySecurity = parameters.m_cryptoKeySecurity;
			this.m_keyPassword = parameters.m_keyPassword;
			this.m_parentWindowHandle = parameters.m_parentWindowHandle;
		}

		// Token: 0x04002864 RID: 10340
		public int ProviderType;

		// Token: 0x04002865 RID: 10341
		public string ProviderName;

		// Token: 0x04002866 RID: 10342
		public string KeyContainerName;

		// Token: 0x04002867 RID: 10343
		public int KeyNumber;

		// Token: 0x04002868 RID: 10344
		private uint m_flags;

		// Token: 0x04002869 RID: 10345
		private CryptoKeySecurity m_cryptoKeySecurity;

		// Token: 0x0400286A RID: 10346
		private SecureString m_keyPassword;

		// Token: 0x0400286B RID: 10347
		private IntPtr m_parentWindowHandle;
	}
}
