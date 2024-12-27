using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace System.Security.Permissions
{
	// Token: 0x02000648 RID: 1608
	[ComVisible(true)]
	[Serializable]
	public sealed class KeyContainerPermissionAccessEntry
	{
		// Token: 0x06003A6F RID: 14959 RVA: 0x000C6212 File Offset: 0x000C5212
		internal KeyContainerPermissionAccessEntry(KeyContainerPermissionAccessEntry accessEntry)
			: this(accessEntry.KeyStore, accessEntry.ProviderName, accessEntry.ProviderType, accessEntry.KeyContainerName, accessEntry.KeySpec, accessEntry.Flags)
		{
		}

		// Token: 0x06003A70 RID: 14960 RVA: 0x000C623E File Offset: 0x000C523E
		public KeyContainerPermissionAccessEntry(string keyContainerName, KeyContainerPermissionFlags flags)
			: this(null, null, -1, keyContainerName, -1, flags)
		{
		}

		// Token: 0x06003A71 RID: 14961 RVA: 0x000C624C File Offset: 0x000C524C
		public KeyContainerPermissionAccessEntry(CspParameters parameters, KeyContainerPermissionFlags flags)
			: this(((parameters.Flags & CspProviderFlags.UseMachineKeyStore) == CspProviderFlags.UseMachineKeyStore) ? "Machine" : "User", parameters.ProviderName, parameters.ProviderType, parameters.KeyContainerName, parameters.KeyNumber, flags)
		{
		}

		// Token: 0x06003A72 RID: 14962 RVA: 0x000C6284 File Offset: 0x000C5284
		public KeyContainerPermissionAccessEntry(string keyStore, string providerName, int providerType, string keyContainerName, int keySpec, KeyContainerPermissionFlags flags)
		{
			this.m_providerName = ((providerName == null) ? "*" : providerName);
			this.m_providerType = providerType;
			this.m_keyContainerName = ((keyContainerName == null) ? "*" : keyContainerName);
			this.m_keySpec = keySpec;
			this.KeyStore = keyStore;
			this.Flags = flags;
		}

		// Token: 0x170009E3 RID: 2531
		// (get) Token: 0x06003A73 RID: 14963 RVA: 0x000C62D9 File Offset: 0x000C52D9
		// (set) Token: 0x06003A74 RID: 14964 RVA: 0x000C62E4 File Offset: 0x000C52E4
		public string KeyStore
		{
			get
			{
				return this.m_keyStore;
			}
			set
			{
				if (KeyContainerPermissionAccessEntry.IsUnrestrictedEntry(value, this.ProviderName, this.ProviderType, this.KeyContainerName, this.KeySpec))
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_InvalidAccessEntry"));
				}
				if (value == null)
				{
					this.m_keyStore = "*";
					return;
				}
				if (value != "User" && value != "Machine" && value != "*")
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidKeyStore", new object[] { value }), "value");
				}
				this.m_keyStore = value;
			}
		}

		// Token: 0x170009E4 RID: 2532
		// (get) Token: 0x06003A75 RID: 14965 RVA: 0x000C637F File Offset: 0x000C537F
		// (set) Token: 0x06003A76 RID: 14966 RVA: 0x000C6388 File Offset: 0x000C5388
		public string ProviderName
		{
			get
			{
				return this.m_providerName;
			}
			set
			{
				if (KeyContainerPermissionAccessEntry.IsUnrestrictedEntry(this.KeyStore, value, this.ProviderType, this.KeyContainerName, this.KeySpec))
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_InvalidAccessEntry"));
				}
				if (value == null)
				{
					this.m_providerName = "*";
					return;
				}
				this.m_providerName = value;
			}
		}

		// Token: 0x170009E5 RID: 2533
		// (get) Token: 0x06003A77 RID: 14967 RVA: 0x000C63DB File Offset: 0x000C53DB
		// (set) Token: 0x06003A78 RID: 14968 RVA: 0x000C63E3 File Offset: 0x000C53E3
		public int ProviderType
		{
			get
			{
				return this.m_providerType;
			}
			set
			{
				if (KeyContainerPermissionAccessEntry.IsUnrestrictedEntry(this.KeyStore, this.ProviderName, value, this.KeyContainerName, this.KeySpec))
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_InvalidAccessEntry"));
				}
				this.m_providerType = value;
			}
		}

		// Token: 0x170009E6 RID: 2534
		// (get) Token: 0x06003A79 RID: 14969 RVA: 0x000C641C File Offset: 0x000C541C
		// (set) Token: 0x06003A7A RID: 14970 RVA: 0x000C6424 File Offset: 0x000C5424
		public string KeyContainerName
		{
			get
			{
				return this.m_keyContainerName;
			}
			set
			{
				if (KeyContainerPermissionAccessEntry.IsUnrestrictedEntry(this.KeyStore, this.ProviderName, this.ProviderType, value, this.KeySpec))
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_InvalidAccessEntry"));
				}
				if (value == null)
				{
					this.m_keyContainerName = "*";
					return;
				}
				this.m_keyContainerName = value;
			}
		}

		// Token: 0x170009E7 RID: 2535
		// (get) Token: 0x06003A7B RID: 14971 RVA: 0x000C6477 File Offset: 0x000C5477
		// (set) Token: 0x06003A7C RID: 14972 RVA: 0x000C647F File Offset: 0x000C547F
		public int KeySpec
		{
			get
			{
				return this.m_keySpec;
			}
			set
			{
				if (KeyContainerPermissionAccessEntry.IsUnrestrictedEntry(this.KeyStore, this.ProviderName, this.ProviderType, this.KeyContainerName, value))
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_InvalidAccessEntry"));
				}
				this.m_keySpec = value;
			}
		}

		// Token: 0x170009E8 RID: 2536
		// (get) Token: 0x06003A7D RID: 14973 RVA: 0x000C64B8 File Offset: 0x000C54B8
		// (set) Token: 0x06003A7E RID: 14974 RVA: 0x000C64C0 File Offset: 0x000C54C0
		public KeyContainerPermissionFlags Flags
		{
			get
			{
				return this.m_flags;
			}
			set
			{
				KeyContainerPermission.VerifyFlags(value);
				this.m_flags = value;
			}
		}

		// Token: 0x06003A7F RID: 14975 RVA: 0x000C64D0 File Offset: 0x000C54D0
		public override bool Equals(object o)
		{
			KeyContainerPermissionAccessEntry keyContainerPermissionAccessEntry = o as KeyContainerPermissionAccessEntry;
			return keyContainerPermissionAccessEntry != null && !(keyContainerPermissionAccessEntry.m_keyStore != this.m_keyStore) && !(keyContainerPermissionAccessEntry.m_providerName != this.m_providerName) && keyContainerPermissionAccessEntry.m_providerType == this.m_providerType && !(keyContainerPermissionAccessEntry.m_keyContainerName != this.m_keyContainerName) && keyContainerPermissionAccessEntry.m_keySpec == this.m_keySpec;
		}

		// Token: 0x06003A80 RID: 14976 RVA: 0x000C654C File Offset: 0x000C554C
		public override int GetHashCode()
		{
			int num = 0;
			num |= (this.m_keyStore.GetHashCode() & 255) << 24;
			num |= (this.m_providerName.GetHashCode() & 255) << 16;
			num |= (this.m_providerType & 15) << 12;
			num |= (this.m_keyContainerName.GetHashCode() & 255) << 4;
			return num | (this.m_keySpec & 15);
		}

		// Token: 0x06003A81 RID: 14977 RVA: 0x000C65BC File Offset: 0x000C55BC
		internal bool IsSubsetOf(KeyContainerPermissionAccessEntry target)
		{
			return (!(target.m_keyStore != "*") || !(this.m_keyStore != target.m_keyStore)) && (!(target.m_providerName != "*") || !(this.m_providerName != target.m_providerName)) && (target.m_providerType == -1 || this.m_providerType == target.m_providerType) && (!(target.m_keyContainerName != "*") || !(this.m_keyContainerName != target.m_keyContainerName)) && (target.m_keySpec == -1 || this.m_keySpec == target.m_keySpec);
		}

		// Token: 0x06003A82 RID: 14978 RVA: 0x000C6674 File Offset: 0x000C5674
		internal static bool IsUnrestrictedEntry(string keyStore, string providerName, int providerType, string keyContainerName, int keySpec)
		{
			return (!(keyStore != "*") || keyStore == null) && (!(providerName != "*") || providerName == null) && providerType == -1 && (!(keyContainerName != "*") || keyContainerName == null) && keySpec == -1;
		}

		// Token: 0x04001E29 RID: 7721
		private string m_keyStore;

		// Token: 0x04001E2A RID: 7722
		private string m_providerName;

		// Token: 0x04001E2B RID: 7723
		private int m_providerType;

		// Token: 0x04001E2C RID: 7724
		private string m_keyContainerName;

		// Token: 0x04001E2D RID: 7725
		private int m_keySpec;

		// Token: 0x04001E2E RID: 7726
		private KeyContainerPermissionFlags m_flags;
	}
}
