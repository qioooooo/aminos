using System;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Permissions;

namespace System.Security.Cryptography
{
	// Token: 0x02000876 RID: 2166
	[ComVisible(true)]
	public sealed class CspKeyContainerInfo
	{
		// Token: 0x06004F49 RID: 20297 RVA: 0x00114A25 File Offset: 0x00113A25
		private CspKeyContainerInfo()
		{
		}

		// Token: 0x06004F4A RID: 20298 RVA: 0x00114A30 File Offset: 0x00113A30
		internal CspKeyContainerInfo(CspParameters parameters, bool randomKeyContainer)
		{
			KeyContainerPermission keyContainerPermission = new KeyContainerPermission(KeyContainerPermissionFlags.NoFlags);
			KeyContainerPermissionAccessEntry keyContainerPermissionAccessEntry = new KeyContainerPermissionAccessEntry(parameters, KeyContainerPermissionFlags.Open);
			keyContainerPermission.AccessEntries.Add(keyContainerPermissionAccessEntry);
			keyContainerPermission.Demand();
			this.m_parameters = new CspParameters(parameters);
			if (this.m_parameters.KeyNumber == -1)
			{
				if (this.m_parameters.ProviderType == 1 || this.m_parameters.ProviderType == 24)
				{
					this.m_parameters.KeyNumber = 1;
				}
				else if (this.m_parameters.ProviderType == 13)
				{
					this.m_parameters.KeyNumber = 2;
				}
			}
			this.m_randomKeyContainer = randomKeyContainer;
		}

		// Token: 0x06004F4B RID: 20299 RVA: 0x00114ACC File Offset: 0x00113ACC
		public CspKeyContainerInfo(CspParameters parameters)
			: this(parameters, false)
		{
		}

		// Token: 0x17000DDD RID: 3549
		// (get) Token: 0x06004F4C RID: 20300 RVA: 0x00114AD6 File Offset: 0x00113AD6
		public bool MachineKeyStore
		{
			get
			{
				return (this.m_parameters.Flags & CspProviderFlags.UseMachineKeyStore) == CspProviderFlags.UseMachineKeyStore;
			}
		}

		// Token: 0x17000DDE RID: 3550
		// (get) Token: 0x06004F4D RID: 20301 RVA: 0x00114AEB File Offset: 0x00113AEB
		public string ProviderName
		{
			get
			{
				return this.m_parameters.ProviderName;
			}
		}

		// Token: 0x17000DDF RID: 3551
		// (get) Token: 0x06004F4E RID: 20302 RVA: 0x00114AF8 File Offset: 0x00113AF8
		public int ProviderType
		{
			get
			{
				return this.m_parameters.ProviderType;
			}
		}

		// Token: 0x17000DE0 RID: 3552
		// (get) Token: 0x06004F4F RID: 20303 RVA: 0x00114B05 File Offset: 0x00113B05
		public string KeyContainerName
		{
			get
			{
				return this.m_parameters.KeyContainerName;
			}
		}

		// Token: 0x17000DE1 RID: 3553
		// (get) Token: 0x06004F50 RID: 20304 RVA: 0x00114B14 File Offset: 0x00113B14
		public string UniqueKeyContainerName
		{
			get
			{
				if (Utils.Win2KCrypto == 0)
				{
					throw new NotSupportedException(Environment.GetResourceString("NotSupported_Method"));
				}
				SafeProvHandle invalidHandle = SafeProvHandle.InvalidHandle;
				int num = Utils._OpenCSP(this.m_parameters, 64U, ref invalidHandle);
				if (num != 0)
				{
					throw new CryptographicException(Environment.GetResourceString("Cryptography_CSP_NotFound"));
				}
				string text = (string)Utils._GetProviderParameter(invalidHandle, this.m_parameters.KeyNumber, 8U);
				invalidHandle.Dispose();
				return text;
			}
		}

		// Token: 0x17000DE2 RID: 3554
		// (get) Token: 0x06004F51 RID: 20305 RVA: 0x00114B80 File Offset: 0x00113B80
		public KeyNumber KeyNumber
		{
			get
			{
				return (KeyNumber)this.m_parameters.KeyNumber;
			}
		}

		// Token: 0x17000DE3 RID: 3555
		// (get) Token: 0x06004F52 RID: 20306 RVA: 0x00114B90 File Offset: 0x00113B90
		public bool Exportable
		{
			get
			{
				if (Utils.Win2KCrypto == 0)
				{
					throw new NotSupportedException(Environment.GetResourceString("NotSupported_Method"));
				}
				if (this.HardwareDevice)
				{
					return false;
				}
				SafeProvHandle invalidHandle = SafeProvHandle.InvalidHandle;
				int num = Utils._OpenCSP(this.m_parameters, 64U, ref invalidHandle);
				if (num != 0)
				{
					throw new CryptographicException(Environment.GetResourceString("Cryptography_CSP_NotFound"));
				}
				byte[] array = (byte[])Utils._GetProviderParameter(invalidHandle, this.m_parameters.KeyNumber, 3U);
				invalidHandle.Dispose();
				return array[0] == 1;
			}
		}

		// Token: 0x17000DE4 RID: 3556
		// (get) Token: 0x06004F53 RID: 20307 RVA: 0x00114C0C File Offset: 0x00113C0C
		public bool HardwareDevice
		{
			get
			{
				SafeProvHandle invalidHandle = SafeProvHandle.InvalidHandle;
				CspParameters cspParameters = new CspParameters(this.m_parameters);
				cspParameters.KeyContainerName = null;
				cspParameters.Flags = (((cspParameters.Flags & CspProviderFlags.UseMachineKeyStore) != CspProviderFlags.NoFlags) ? CspProviderFlags.UseMachineKeyStore : CspProviderFlags.NoFlags);
				uint num = 0U;
				if (Utils.Win2KCrypto == 1)
				{
					num |= 4026531840U;
				}
				int num2 = Utils._OpenCSP(cspParameters, num, ref invalidHandle);
				if (num2 != 0)
				{
					throw new CryptographicException(Environment.GetResourceString("Cryptography_CSP_NotFound"));
				}
				byte[] array = (byte[])Utils._GetProviderParameter(invalidHandle, cspParameters.KeyNumber, 5U);
				invalidHandle.Dispose();
				return array[0] == 1;
			}
		}

		// Token: 0x17000DE5 RID: 3557
		// (get) Token: 0x06004F54 RID: 20308 RVA: 0x00114C98 File Offset: 0x00113C98
		public bool Removable
		{
			get
			{
				SafeProvHandle invalidHandle = SafeProvHandle.InvalidHandle;
				CspParameters cspParameters = new CspParameters(this.m_parameters);
				cspParameters.KeyContainerName = null;
				cspParameters.Flags = (((cspParameters.Flags & CspProviderFlags.UseMachineKeyStore) != CspProviderFlags.NoFlags) ? CspProviderFlags.UseMachineKeyStore : CspProviderFlags.NoFlags);
				uint num = 0U;
				if (Utils.Win2KCrypto == 1)
				{
					num |= 4026531840U;
				}
				int num2 = Utils._OpenCSP(cspParameters, num, ref invalidHandle);
				if (num2 != 0)
				{
					throw new CryptographicException(Environment.GetResourceString("Cryptography_CSP_NotFound"));
				}
				byte[] array = (byte[])Utils._GetProviderParameter(invalidHandle, cspParameters.KeyNumber, 4U);
				invalidHandle.Dispose();
				return array[0] == 1;
			}
		}

		// Token: 0x17000DE6 RID: 3558
		// (get) Token: 0x06004F55 RID: 20309 RVA: 0x00114D24 File Offset: 0x00113D24
		public bool Accessible
		{
			get
			{
				if (Utils.Win2KCrypto == 0)
				{
					throw new NotSupportedException(Environment.GetResourceString("NotSupported_Method"));
				}
				SafeProvHandle invalidHandle = SafeProvHandle.InvalidHandle;
				int num = Utils._OpenCSP(this.m_parameters, 64U, ref invalidHandle);
				if (num != 0)
				{
					return false;
				}
				byte[] array = (byte[])Utils._GetProviderParameter(invalidHandle, this.m_parameters.KeyNumber, 6U);
				invalidHandle.Dispose();
				return array[0] == 1;
			}
		}

		// Token: 0x17000DE7 RID: 3559
		// (get) Token: 0x06004F56 RID: 20310 RVA: 0x00114D88 File Offset: 0x00113D88
		public bool Protected
		{
			get
			{
				if (this.HardwareDevice)
				{
					return true;
				}
				if (Utils.Win2KCrypto == 0)
				{
					throw new NotSupportedException(Environment.GetResourceString("NotSupported_Method"));
				}
				SafeProvHandle invalidHandle = SafeProvHandle.InvalidHandle;
				int num = Utils._OpenCSP(this.m_parameters, 64U, ref invalidHandle);
				if (num != 0)
				{
					throw new CryptographicException(Environment.GetResourceString("Cryptography_CSP_NotFound"));
				}
				byte[] array = (byte[])Utils._GetProviderParameter(invalidHandle, this.m_parameters.KeyNumber, 7U);
				invalidHandle.Dispose();
				return array[0] == 1;
			}
		}

		// Token: 0x17000DE8 RID: 3560
		// (get) Token: 0x06004F57 RID: 20311 RVA: 0x00114E04 File Offset: 0x00113E04
		public CryptoKeySecurity CryptoKeySecurity
		{
			get
			{
				if (Utils.Win2KCrypto == 0)
				{
					throw new NotSupportedException(Environment.GetResourceString("NotSupported_Method"));
				}
				KeyContainerPermission keyContainerPermission = new KeyContainerPermission(KeyContainerPermissionFlags.NoFlags);
				KeyContainerPermissionAccessEntry keyContainerPermissionAccessEntry = new KeyContainerPermissionAccessEntry(this.m_parameters, KeyContainerPermissionFlags.ViewAcl | KeyContainerPermissionFlags.ChangeAcl);
				keyContainerPermission.AccessEntries.Add(keyContainerPermissionAccessEntry);
				keyContainerPermission.Demand();
				SafeProvHandle invalidHandle = SafeProvHandle.InvalidHandle;
				int num = Utils._OpenCSP(this.m_parameters, 64U, ref invalidHandle);
				if (num != 0)
				{
					throw new CryptographicException(Environment.GetResourceString("Cryptography_CSP_NotFound"));
				}
				CryptoKeySecurity keySetSecurityInfo;
				using (invalidHandle)
				{
					keySetSecurityInfo = Utils.GetKeySetSecurityInfo(invalidHandle, AccessControlSections.All);
				}
				return keySetSecurityInfo;
			}
		}

		// Token: 0x17000DE9 RID: 3561
		// (get) Token: 0x06004F58 RID: 20312 RVA: 0x00114EAC File Offset: 0x00113EAC
		public bool RandomlyGenerated
		{
			get
			{
				return this.m_randomKeyContainer;
			}
		}

		// Token: 0x040028BB RID: 10427
		private CspParameters m_parameters;

		// Token: 0x040028BC RID: 10428
		private bool m_randomKeyContainer;
	}
}
