using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Threading;
using Microsoft.Win32;

namespace System.Security.Cryptography
{
	// Token: 0x020008A4 RID: 2212
	internal static class Utils
	{
		// Token: 0x17000E18 RID: 3608
		// (get) Token: 0x060050B5 RID: 20661 RVA: 0x001225C8 File Offset: 0x001215C8
		private static object InternalSyncObject
		{
			get
			{
				if (Utils.s_InternalSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref Utils.s_InternalSyncObject, obj, null);
				}
				return Utils.s_InternalSyncObject;
			}
		}

		// Token: 0x17000E19 RID: 3609
		// (get) Token: 0x060050B6 RID: 20662 RVA: 0x001225F4 File Offset: 0x001215F4
		internal static int DefaultRsaProviderType
		{
			get
			{
				if (Utils._defaultRsaProviderType == -1)
				{
					Utils._defaultRsaProviderType = ((Environment.OSVersion.Version.Major > 5 || (Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor >= 1)) ? 24 : 1);
				}
				return Utils._defaultRsaProviderType;
			}
		}

		// Token: 0x17000E1A RID: 3610
		// (get) Token: 0x060050B7 RID: 20663 RVA: 0x0012265C File Offset: 0x0012165C
		internal static SafeProvHandle StaticProvHandle
		{
			get
			{
				if (Utils._safeProvHandle == null)
				{
					lock (Utils.InternalSyncObject)
					{
						if (Utils._safeProvHandle == null)
						{
							SafeProvHandle safeProvHandle = Utils.AcquireProvHandle(new CspParameters(Utils.DefaultRsaProviderType));
							Thread.MemoryBarrier();
							Utils._safeProvHandle = safeProvHandle;
						}
					}
				}
				return Utils._safeProvHandle;
			}
		}

		// Token: 0x17000E1B RID: 3611
		// (get) Token: 0x060050B8 RID: 20664 RVA: 0x001226BC File Offset: 0x001216BC
		internal static SafeProvHandle StaticDssProvHandle
		{
			get
			{
				if (Utils._safeDssProvHandle == null)
				{
					lock (Utils.InternalSyncObject)
					{
						if (Utils._safeDssProvHandle == null)
						{
							SafeProvHandle safeProvHandle = Utils.AcquireProvHandle(new CspParameters(13));
							Thread.MemoryBarrier();
							Utils._safeDssProvHandle = safeProvHandle;
						}
					}
				}
				return Utils._safeDssProvHandle;
			}
		}

		// Token: 0x060050B9 RID: 20665 RVA: 0x0012271C File Offset: 0x0012171C
		internal static SafeProvHandle AcquireProvHandle(CspParameters parameters)
		{
			if (parameters == null)
			{
				parameters = new CspParameters(Utils.DefaultRsaProviderType);
			}
			SafeProvHandle safeProvHandle = SafeProvHandle.InvalidHandle;
			if (Utils.Win2KCrypto == 1)
			{
				Utils._AcquireCSP(parameters, ref safeProvHandle);
			}
			else
			{
				if (parameters.KeyContainerName == null && (parameters.Flags & CspProviderFlags.UseDefaultKeyContainer) == CspProviderFlags.NoFlags)
				{
					parameters.KeyContainerName = Utils._GetRandomKeyContainer();
				}
				safeProvHandle = Utils.CreateProvHandle(parameters, true);
			}
			return safeProvHandle;
		}

		// Token: 0x060050BA RID: 20666 RVA: 0x00122778 File Offset: 0x00121778
		internal static SafeProvHandle CreateProvHandle(CspParameters parameters, bool randomKeyContainer)
		{
			SafeProvHandle invalidHandle = SafeProvHandle.InvalidHandle;
			int num = Utils._OpenCSP(parameters, 0U, ref invalidHandle);
			KeyContainerPermission keyContainerPermission = new KeyContainerPermission(KeyContainerPermissionFlags.NoFlags);
			if (num != 0)
			{
				if ((parameters.Flags & CspProviderFlags.UseExistingKey) != CspProviderFlags.NoFlags || (num != -2146893799 && num != -2146893802))
				{
					throw new CryptographicException(num);
				}
				if (!randomKeyContainer)
				{
					KeyContainerPermissionAccessEntry keyContainerPermissionAccessEntry = new KeyContainerPermissionAccessEntry(parameters, KeyContainerPermissionFlags.Create);
					keyContainerPermission.AccessEntries.Add(keyContainerPermissionAccessEntry);
					keyContainerPermission.Demand();
				}
				Utils._CreateCSP(parameters, randomKeyContainer, ref invalidHandle);
			}
			else if (!randomKeyContainer)
			{
				KeyContainerPermissionAccessEntry keyContainerPermissionAccessEntry2 = new KeyContainerPermissionAccessEntry(parameters, KeyContainerPermissionFlags.Open);
				keyContainerPermission.AccessEntries.Add(keyContainerPermissionAccessEntry2);
				keyContainerPermission.Demand();
			}
			return invalidHandle;
		}

		// Token: 0x060050BB RID: 20667 RVA: 0x0012280C File Offset: 0x0012180C
		internal static CryptoKeySecurity GetKeySetSecurityInfo(SafeProvHandle hProv, AccessControlSections accessControlSections)
		{
			if (Utils.Win2KCrypto != 1)
			{
				throw new PlatformNotSupportedException(Environment.GetResourceString("PlatformNotSupported_RequiresNT"));
			}
			SecurityInfos securityInfos = (SecurityInfos)0;
			Privilege privilege = null;
			if ((accessControlSections & AccessControlSections.Owner) != AccessControlSections.None)
			{
				securityInfos |= SecurityInfos.Owner;
			}
			if ((accessControlSections & AccessControlSections.Group) != AccessControlSections.None)
			{
				securityInfos |= SecurityInfos.Group;
			}
			if ((accessControlSections & AccessControlSections.Access) != AccessControlSections.None)
			{
				securityInfos |= SecurityInfos.DiscretionaryAcl;
			}
			byte[] array = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			int num;
			try
			{
				if ((accessControlSections & AccessControlSections.Audit) != AccessControlSections.None)
				{
					securityInfos |= SecurityInfos.SystemAcl;
					privilege = new Privilege("SeSecurityPrivilege");
					privilege.Enable();
				}
				array = Utils._GetKeySetSecurityInfo(hProv, securityInfos, out num);
			}
			finally
			{
				if (privilege != null)
				{
					privilege.Revert();
				}
			}
			if (num == 0 && (array == null || array.Length == 0))
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NoSecurityDescriptor"));
			}
			if (num == 8)
			{
				throw new OutOfMemoryException();
			}
			if (num == 5)
			{
				throw new UnauthorizedAccessException();
			}
			if (num == 1314)
			{
				throw new PrivilegeNotHeldException("SeSecurityPrivilege");
			}
			if (num != 0)
			{
				throw new CryptographicException(num);
			}
			CommonSecurityDescriptor commonSecurityDescriptor = new CommonSecurityDescriptor(false, false, new RawSecurityDescriptor(array, 0), true);
			return new CryptoKeySecurity(commonSecurityDescriptor);
		}

		// Token: 0x060050BC RID: 20668 RVA: 0x001228FC File Offset: 0x001218FC
		internal static void SetKeySetSecurityInfo(SafeProvHandle hProv, CryptoKeySecurity cryptoKeySecurity, AccessControlSections accessControlSections)
		{
			if (Utils.Win2KCrypto != 1)
			{
				throw new PlatformNotSupportedException(Environment.GetResourceString("PlatformNotSupported_RequiresNT"));
			}
			SecurityInfos securityInfos = (SecurityInfos)0;
			Privilege privilege = null;
			if ((accessControlSections & AccessControlSections.Owner) != AccessControlSections.None && cryptoKeySecurity._securityDescriptor.Owner != null)
			{
				securityInfos |= SecurityInfos.Owner;
			}
			if ((accessControlSections & AccessControlSections.Group) != AccessControlSections.None && cryptoKeySecurity._securityDescriptor.Group != null)
			{
				securityInfos |= SecurityInfos.Group;
			}
			if ((accessControlSections & AccessControlSections.Audit) != AccessControlSections.None)
			{
				securityInfos |= SecurityInfos.SystemAcl;
			}
			if ((accessControlSections & AccessControlSections.Access) != AccessControlSections.None && cryptoKeySecurity._securityDescriptor.IsDiscretionaryAclPresent)
			{
				securityInfos |= SecurityInfos.DiscretionaryAcl;
			}
			if (securityInfos == (SecurityInfos)0)
			{
				return;
			}
			int num = 0;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				if ((securityInfos & SecurityInfos.SystemAcl) != (SecurityInfos)0)
				{
					privilege = new Privilege("SeSecurityPrivilege");
					privilege.Enable();
				}
				byte[] securityDescriptorBinaryForm = cryptoKeySecurity.GetSecurityDescriptorBinaryForm();
				if (securityDescriptorBinaryForm != null && securityDescriptorBinaryForm.Length > 0)
				{
					num = Utils._SetKeySetSecurityInfo(hProv, securityInfos, securityDescriptorBinaryForm);
				}
			}
			finally
			{
				if (privilege != null)
				{
					privilege.Revert();
				}
			}
			if (num == 5 || num == 1307 || num == 1308)
			{
				throw new UnauthorizedAccessException();
			}
			if (num == 1314)
			{
				throw new PrivilegeNotHeldException("SeSecurityPrivilege");
			}
			if (num == 6)
			{
				throw new NotSupportedException(Environment.GetResourceString("AccessControl_InvalidHandle"));
			}
			if (num != 0)
			{
				throw new CryptographicException(num);
			}
		}

		// Token: 0x060050BD RID: 20669 RVA: 0x00122A20 File Offset: 0x00121A20
		internal static byte[] ExportCspBlobHelper(bool includePrivateParameters, CspParameters parameters, SafeKeyHandle safeKeyHandle)
		{
			if (includePrivateParameters)
			{
				KeyContainerPermission keyContainerPermission = new KeyContainerPermission(KeyContainerPermissionFlags.NoFlags);
				KeyContainerPermissionAccessEntry keyContainerPermissionAccessEntry = new KeyContainerPermissionAccessEntry(parameters, KeyContainerPermissionFlags.Export);
				keyContainerPermission.AccessEntries.Add(keyContainerPermissionAccessEntry);
				keyContainerPermission.Demand();
			}
			return Utils._ExportCspBlob(safeKeyHandle, includePrivateParameters ? 7 : 6);
		}

		// Token: 0x060050BE RID: 20670 RVA: 0x00122A60 File Offset: 0x00121A60
		internal static void GetKeyPairHelper(CspAlgorithmType keyType, CspParameters parameters, bool randomKeyContainer, int dwKeySize, ref SafeProvHandle safeProvHandle, ref SafeKeyHandle safeKeyHandle)
		{
			SafeProvHandle safeProvHandle2 = Utils.CreateProvHandle(parameters, randomKeyContainer);
			if (parameters.CryptoKeySecurity != null)
			{
				KeyContainerPermission keyContainerPermission = new KeyContainerPermission(KeyContainerPermissionFlags.NoFlags);
				KeyContainerPermissionAccessEntry keyContainerPermissionAccessEntry = new KeyContainerPermissionAccessEntry(parameters, KeyContainerPermissionFlags.ChangeAcl);
				keyContainerPermission.AccessEntries.Add(keyContainerPermissionAccessEntry);
				keyContainerPermission.Demand();
				Utils.SetKeySetSecurityInfo(safeProvHandle2, parameters.CryptoKeySecurity, parameters.CryptoKeySecurity.ChangedAccessControlSections);
			}
			if (parameters.ParentWindowHandle != IntPtr.Zero)
			{
				Utils._SetProviderParameter(safeProvHandle2, parameters.KeyNumber, 10U, parameters.ParentWindowHandle);
			}
			else if (parameters.KeyPassword != null)
			{
				IntPtr intPtr = Marshal.SecureStringToCoTaskMemAnsi(parameters.KeyPassword);
				try
				{
					Utils._SetProviderParameter(safeProvHandle2, parameters.KeyNumber, 11U, intPtr);
				}
				finally
				{
					if (intPtr != IntPtr.Zero)
					{
						Marshal.ZeroFreeCoTaskMemAnsi(intPtr);
					}
				}
			}
			safeProvHandle = safeProvHandle2;
			SafeKeyHandle invalidHandle = SafeKeyHandle.InvalidHandle;
			int num = Utils._GetUserKey(safeProvHandle, parameters.KeyNumber, ref invalidHandle);
			if (num != 0)
			{
				if ((parameters.Flags & CspProviderFlags.UseExistingKey) != CspProviderFlags.NoFlags || num != -2146893811)
				{
					throw new CryptographicException(num);
				}
				Utils._GenerateKey(safeProvHandle, parameters.KeyNumber, parameters.Flags, dwKeySize, ref invalidHandle);
			}
			byte[] array = Utils._GetKeyParameter(invalidHandle, 9U);
			int num2 = (int)array[0] | ((int)array[1] << 8) | ((int)array[2] << 16) | ((int)array[3] << 24);
			if ((keyType == CspAlgorithmType.Rsa && num2 != 41984 && num2 != 9216) || (keyType == CspAlgorithmType.Dss && num2 != 8704))
			{
				invalidHandle.Dispose();
				throw new CryptographicException(Environment.GetResourceString("Cryptography_CSP_WrongKeySpec"));
			}
			safeKeyHandle = invalidHandle;
		}

		// Token: 0x060050BF RID: 20671 RVA: 0x00122BE4 File Offset: 0x00121BE4
		internal static void ImportCspBlobHelper(CspAlgorithmType keyType, byte[] keyBlob, bool publicOnly, ref CspParameters parameters, bool randomKeyContainer, ref SafeProvHandle safeProvHandle, ref SafeKeyHandle safeKeyHandle)
		{
			if (safeKeyHandle != null && !safeKeyHandle.IsClosed)
			{
				safeKeyHandle.Dispose();
			}
			safeKeyHandle = SafeKeyHandle.InvalidHandle;
			if (publicOnly)
			{
				parameters.KeyNumber = Utils._ImportCspBlob(keyBlob, (keyType == CspAlgorithmType.Dss) ? Utils.StaticDssProvHandle : Utils.StaticProvHandle, CspProviderFlags.NoFlags, ref safeKeyHandle);
				return;
			}
			KeyContainerPermission keyContainerPermission = new KeyContainerPermission(KeyContainerPermissionFlags.NoFlags);
			KeyContainerPermissionAccessEntry keyContainerPermissionAccessEntry = new KeyContainerPermissionAccessEntry(parameters, KeyContainerPermissionFlags.Import);
			keyContainerPermission.AccessEntries.Add(keyContainerPermissionAccessEntry);
			keyContainerPermission.Demand();
			if (safeProvHandle == null)
			{
				safeProvHandle = Utils.CreateProvHandle(parameters, randomKeyContainer);
			}
			parameters.KeyNumber = Utils._ImportCspBlob(keyBlob, safeProvHandle, parameters.Flags, ref safeKeyHandle);
		}

		// Token: 0x060050C0 RID: 20672 RVA: 0x00122C84 File Offset: 0x00121C84
		internal static CspParameters SaveCspParameters(CspAlgorithmType keyType, CspParameters userParameters, CspProviderFlags defaultFlags, ref bool randomKeyContainer)
		{
			CspParameters cspParameters;
			if (userParameters == null)
			{
				cspParameters = new CspParameters((keyType == CspAlgorithmType.Dss) ? 13 : Utils.DefaultRsaProviderType, null, null, defaultFlags);
			}
			else
			{
				Utils.ValidateCspFlags(userParameters.Flags);
				cspParameters = new CspParameters(userParameters);
			}
			if (cspParameters.KeyNumber == -1)
			{
				cspParameters.KeyNumber = ((keyType == CspAlgorithmType.Dss) ? 2 : 1);
			}
			else if (cspParameters.KeyNumber == 8704 || cspParameters.KeyNumber == 9216)
			{
				cspParameters.KeyNumber = 2;
			}
			else if (cspParameters.KeyNumber == 41984)
			{
				cspParameters.KeyNumber = 1;
			}
			randomKeyContainer = false;
			if (cspParameters.KeyContainerName == null && (cspParameters.Flags & CspProviderFlags.UseDefaultKeyContainer) == CspProviderFlags.NoFlags)
			{
				cspParameters.KeyContainerName = Utils._GetRandomKeyContainer();
				randomKeyContainer = true;
			}
			return cspParameters;
		}

		// Token: 0x060050C1 RID: 20673 RVA: 0x00122D34 File Offset: 0x00121D34
		private static void ValidateCspFlags(CspProviderFlags flags)
		{
			if ((flags & CspProviderFlags.UseExistingKey) != CspProviderFlags.NoFlags)
			{
				CspProviderFlags cspProviderFlags = CspProviderFlags.UseNonExportableKey | CspProviderFlags.UseArchivableKey | CspProviderFlags.UseUserProtectedKey;
				if ((flags & cspProviderFlags) != CspProviderFlags.NoFlags)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidFlag"));
				}
			}
			if ((flags & CspProviderFlags.UseUserProtectedKey) != CspProviderFlags.NoFlags)
			{
				if (!Environment.UserInteractive)
				{
					throw new InvalidOperationException(Environment.GetResourceString("Cryptography_NotInteractive"));
				}
				UIPermission uipermission = new UIPermission(UIPermissionWindow.SafeTopLevelWindows);
				uipermission.Demand();
			}
		}

		// Token: 0x17000E1C RID: 3612
		// (get) Token: 0x060050C2 RID: 20674 RVA: 0x00122D88 File Offset: 0x00121D88
		internal static RNGCryptoServiceProvider StaticRandomNumberGenerator
		{
			get
			{
				if (Utils._rng == null)
				{
					Utils._rng = new RNGCryptoServiceProvider();
				}
				return Utils._rng;
			}
		}

		// Token: 0x060050C3 RID: 20675 RVA: 0x00122DA0 File Offset: 0x00121DA0
		internal static byte[] GenerateRandom(int keySize)
		{
			byte[] array = new byte[keySize];
			Utils.StaticRandomNumberGenerator.GetBytes(array);
			return array;
		}

		// Token: 0x060050C4 RID: 20676 RVA: 0x00122DC0 File Offset: 0x00121DC0
		internal static bool HasAlgorithm(int dwCalg, int dwKeySize)
		{
			bool flag = false;
			lock (Utils.InternalSyncObject)
			{
				flag = Utils._SearchForAlgorithm(Utils.StaticProvHandle, dwCalg, dwKeySize);
			}
			return flag;
		}

		// Token: 0x17000E1D RID: 3613
		// (get) Token: 0x060050C5 RID: 20677 RVA: 0x00122E04 File Offset: 0x00121E04
		internal static int HasEnhProv
		{
			get
			{
				if (Utils.s_hasEnhProv == -1)
				{
					Utils.s_hasEnhProv = (Utils.HasAlgorithm(41984, 2048) ? 1 : 0);
				}
				return Utils.s_hasEnhProv;
			}
		}

		// Token: 0x17000E1E RID: 3614
		// (get) Token: 0x060050C6 RID: 20678 RVA: 0x00122E30 File Offset: 0x00121E30
		internal static int FipsAlgorithmPolicy
		{
			[RegistryPermission(SecurityAction.Assert, Unrestricted = true)]
			get
			{
				if (Utils.s_fipsAlgorithmPolicy == -1)
				{
					if (!Utils._GetEnforceFipsPolicySetting())
					{
						Utils.s_fipsAlgorithmPolicy = 0;
					}
					else if (Environment.OSVersion.Version.Major >= 6)
					{
						bool flag;
						uint num = Win32Native.BCryptGetFipsAlgorithmMode(out flag);
						bool flag2 = num == 0U || num == 3221225524U;
						if (!flag2 || flag)
						{
							Utils.s_fipsAlgorithmPolicy = 1;
						}
						else
						{
							Utils.s_fipsAlgorithmPolicy = 0;
						}
					}
					else
					{
						using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("System\\CurrentControlSet\\Control\\Lsa", false))
						{
							if (registryKey != null)
							{
								object value = registryKey.GetValue("FIPSAlgorithmPolicy");
								if (value != null)
								{
									Utils.s_fipsAlgorithmPolicy = (int)value;
								}
							}
						}
					}
				}
				return Utils.s_fipsAlgorithmPolicy;
			}
		}

		// Token: 0x17000E1F RID: 3615
		// (get) Token: 0x060050C7 RID: 20679 RVA: 0x00122EE8 File Offset: 0x00121EE8
		internal static int Win2KCrypto
		{
			get
			{
				if (Utils.s_win2KCrypto == -1)
				{
					Win32Native.OSVERSIONINFO osversioninfo = new Win32Native.OSVERSIONINFO();
					bool versionEx = Win32Native.GetVersionEx(osversioninfo);
					Utils.s_win2KCrypto = ((versionEx && osversioninfo.PlatformId == 2 && osversioninfo.MajorVersion >= 5) ? 1 : 0);
				}
				return Utils.s_win2KCrypto;
			}
		}

		// Token: 0x060050C8 RID: 20680 RVA: 0x00122F30 File Offset: 0x00121F30
		internal static string ObjToOidValue(object hashAlg)
		{
			if (hashAlg == null)
			{
				throw new ArgumentNullException("hashAlg");
			}
			string text = null;
			if (hashAlg is string)
			{
				text = CryptoConfig.MapNameToOID((string)hashAlg);
				if (text == null)
				{
					text = (string)hashAlg;
				}
			}
			else if (hashAlg is HashAlgorithm)
			{
				text = CryptoConfig.MapNameToOID(hashAlg.GetType().ToString());
			}
			else if (hashAlg is Type)
			{
				text = CryptoConfig.MapNameToOID(hashAlg.ToString());
			}
			if (text == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidValue"));
			}
			return text;
		}

		// Token: 0x060050C9 RID: 20681 RVA: 0x00122FB0 File Offset: 0x00121FB0
		internal static HashAlgorithm ObjToHashAlgorithm(object hashAlg)
		{
			if (hashAlg == null)
			{
				throw new ArgumentNullException("hashAlg");
			}
			HashAlgorithm hashAlgorithm = null;
			if (hashAlg is string)
			{
				hashAlgorithm = (HashAlgorithm)CryptoConfig.CreateFromName((string)hashAlg);
				if (hashAlgorithm == null)
				{
					string text = X509Utils._GetFriendlyNameFromOid((string)hashAlg);
					if (text != null)
					{
						hashAlgorithm = (HashAlgorithm)CryptoConfig.CreateFromName(text);
					}
				}
			}
			else if (hashAlg is HashAlgorithm)
			{
				hashAlgorithm = (HashAlgorithm)hashAlg;
			}
			else if (hashAlg is Type)
			{
				hashAlgorithm = (HashAlgorithm)CryptoConfig.CreateFromName(hashAlg.ToString());
			}
			if (hashAlgorithm == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidValue"));
			}
			return hashAlgorithm;
		}

		// Token: 0x060050CA RID: 20682 RVA: 0x00123044 File Offset: 0x00122044
		internal static string DiscardWhiteSpaces(string inputBuffer)
		{
			return Utils.DiscardWhiteSpaces(inputBuffer, 0, inputBuffer.Length);
		}

		// Token: 0x060050CB RID: 20683 RVA: 0x00123054 File Offset: 0x00122054
		internal static string DiscardWhiteSpaces(string inputBuffer, int inputOffset, int inputCount)
		{
			int num = 0;
			for (int i = 0; i < inputCount; i++)
			{
				if (char.IsWhiteSpace(inputBuffer[inputOffset + i]))
				{
					num++;
				}
			}
			char[] array = new char[inputCount - num];
			num = 0;
			for (int i = 0; i < inputCount; i++)
			{
				if (!char.IsWhiteSpace(inputBuffer[inputOffset + i]))
				{
					array[num++] = inputBuffer[inputOffset + i];
				}
			}
			return new string(array);
		}

		// Token: 0x060050CC RID: 20684 RVA: 0x001230C0 File Offset: 0x001220C0
		internal static int ConvertByteArrayToInt(byte[] input)
		{
			int num = 0;
			for (int i = 0; i < input.Length; i++)
			{
				num *= 256;
				num += (int)input[i];
			}
			return num;
		}

		// Token: 0x060050CD RID: 20685 RVA: 0x001230EC File Offset: 0x001220EC
		internal static byte[] ConvertIntToByteArray(int dwInput)
		{
			byte[] array = new byte[8];
			int num = 0;
			if (dwInput == 0)
			{
				return new byte[1];
			}
			int i = dwInput;
			while (i > 0)
			{
				int num2 = i % 256;
				array[num] = (byte)num2;
				i = (i - num2) / 256;
				num++;
			}
			byte[] array2 = new byte[num];
			for (int j = 0; j < num; j++)
			{
				array2[j] = array[num - j - 1];
			}
			return array2;
		}

		// Token: 0x060050CE RID: 20686 RVA: 0x00123158 File Offset: 0x00122158
		internal static void ConvertIntToByteArray(uint dwInput, ref byte[] counter)
		{
			uint num = dwInput;
			int num2 = 0;
			Array.Clear(counter, 0, counter.Length);
			if (dwInput == 0U)
			{
				return;
			}
			while (num > 0U)
			{
				uint num3 = num % 256U;
				counter[3 - num2] = (byte)num3;
				num = (num - num3) / 256U;
				num2++;
			}
		}

		// Token: 0x060050CF RID: 20687 RVA: 0x0012319C File Offset: 0x0012219C
		internal static byte[] FixupKeyParity(byte[] key)
		{
			byte[] array = new byte[key.Length];
			for (int i = 0; i < key.Length; i++)
			{
				array[i] = key[i] & 254;
				byte b = (byte)((int)(array[i] & 15) ^ (array[i] >> 4));
				byte b2 = (byte)((int)(b & 3) ^ (b >> 2));
				if ((byte)((int)(b2 & 1) ^ (b2 >> 1)) == 0)
				{
					byte[] array2 = array;
					int num = i;
					array2[num] |= 1;
				}
			}
			return array;
		}

		// Token: 0x060050D0 RID: 20688 RVA: 0x00123208 File Offset: 0x00122208
		internal unsafe static void DWORDFromLittleEndian(uint* x, int digits, byte* block)
		{
			int i = 0;
			int num = 0;
			while (i < digits)
			{
				x[i] = (uint)((int)block[num] | ((int)block[num + 1] << 8) | ((int)block[num + 2] << 16) | ((int)block[num + 3] << 24));
				i++;
				num += 4;
			}
		}

		// Token: 0x060050D1 RID: 20689 RVA: 0x00123250 File Offset: 0x00122250
		internal static void DWORDToLittleEndian(byte[] block, uint[] x, int digits)
		{
			int i = 0;
			int num = 0;
			while (i < digits)
			{
				block[num] = (byte)(x[i] & 255U);
				block[num + 1] = (byte)((x[i] >> 8) & 255U);
				block[num + 2] = (byte)((x[i] >> 16) & 255U);
				block[num + 3] = (byte)((x[i] >> 24) & 255U);
				i++;
				num += 4;
			}
		}

		// Token: 0x060050D2 RID: 20690 RVA: 0x001232B4 File Offset: 0x001222B4
		internal unsafe static void DWORDFromBigEndian(uint* x, int digits, byte* block)
		{
			int i = 0;
			int num = 0;
			while (i < digits)
			{
				x[i] = (uint)(((int)block[num] << 24) | ((int)block[num + 1] << 16) | ((int)block[num + 2] << 8) | (int)block[num + 3]);
				i++;
				num += 4;
			}
		}

		// Token: 0x060050D3 RID: 20691 RVA: 0x001232FC File Offset: 0x001222FC
		internal static void DWORDToBigEndian(byte[] block, uint[] x, int digits)
		{
			int i = 0;
			int num = 0;
			while (i < digits)
			{
				block[num] = (byte)((x[i] >> 24) & 255U);
				block[num + 1] = (byte)((x[i] >> 16) & 255U);
				block[num + 2] = (byte)((x[i] >> 8) & 255U);
				block[num + 3] = (byte)(x[i] & 255U);
				i++;
				num += 4;
			}
		}

		// Token: 0x060050D4 RID: 20692 RVA: 0x00123360 File Offset: 0x00122360
		internal unsafe static void QuadWordFromBigEndian(ulong* x, int digits, byte* block)
		{
			int i = 0;
			int num = 0;
			while (i < digits)
			{
				x[i] = ((ulong)block[num] << 56) | ((ulong)block[num + 1] << 48) | ((ulong)block[num + 2] << 40) | ((ulong)block[num + 3] << 32) | ((ulong)block[num + 4] << 24) | ((ulong)block[num + 5] << 16) | ((ulong)block[num + 6] << 8) | (ulong)block[num + 7];
				i++;
				num += 8;
			}
		}

		// Token: 0x060050D5 RID: 20693 RVA: 0x001233D8 File Offset: 0x001223D8
		internal static void QuadWordToBigEndian(byte[] block, ulong[] x, int digits)
		{
			int i = 0;
			int num = 0;
			while (i < digits)
			{
				block[num] = (byte)((x[i] >> 56) & 255UL);
				block[num + 1] = (byte)((x[i] >> 48) & 255UL);
				block[num + 2] = (byte)((x[i] >> 40) & 255UL);
				block[num + 3] = (byte)((x[i] >> 32) & 255UL);
				block[num + 4] = (byte)((x[i] >> 24) & 255UL);
				block[num + 5] = (byte)((x[i] >> 16) & 255UL);
				block[num + 6] = (byte)((x[i] >> 8) & 255UL);
				block[num + 7] = (byte)(x[i] & 255UL);
				i++;
				num += 8;
			}
		}

		// Token: 0x060050D6 RID: 20694 RVA: 0x00123490 File Offset: 0x00122490
		internal static byte[] Int(uint i)
		{
			byte[] bytes = BitConverter.GetBytes(i);
			byte[] array = new byte[]
			{
				bytes[3],
				bytes[2],
				bytes[1],
				bytes[0]
			};
			if (!BitConverter.IsLittleEndian)
			{
				return bytes;
			}
			return array;
		}

		// Token: 0x060050D7 RID: 20695 RVA: 0x001234D0 File Offset: 0x001224D0
		internal unsafe static void BlockCopy(byte* src, int srcOffset, byte* dst, int dstOffset, int count)
		{
			for (int i = 0; i < count; i++)
			{
				dst[dstOffset + i] = src[srcOffset + i];
			}
		}

		// Token: 0x060050D8 RID: 20696 RVA: 0x001234F8 File Offset: 0x001224F8
		internal unsafe static void BlockCopy(byte[] src, int srcOffset, int* dst, int dstOffset, int count)
		{
			fixed (byte* ptr = src)
			{
				Utils.BlockCopy(ptr, srcOffset, (byte*)dst, dstOffset, count);
			}
		}

		// Token: 0x060050D9 RID: 20697 RVA: 0x0012352C File Offset: 0x0012252C
		internal unsafe static void BlockCopy(int* src, int srcOffset, int[] dst, int dstOffset, int count)
		{
			fixed (int* ptr = &dst[dstOffset])
			{
				Utils.BlockCopy((byte*)(src + srcOffset), srcOffset, (byte*)ptr, 0, count);
			}
		}

		// Token: 0x060050DA RID: 20698 RVA: 0x00123558 File Offset: 0x00122558
		internal static byte[] RsaOaepEncrypt(RSA rsa, HashAlgorithm hash, PKCS1MaskGenerationMethod mgf, RandomNumberGenerator rng, byte[] data)
		{
			int num = rsa.KeySize / 8;
			int num2 = hash.HashSize / 8;
			if (data.Length + 2 + 2 * num2 > num)
			{
				throw new CryptographicException(string.Format(null, Environment.GetResourceString("Cryptography_Padding_EncDataTooBig"), new object[] { num - 2 - 2 * num2 }));
			}
			hash.ComputeHash(new byte[0]);
			byte[] array = new byte[num - num2];
			Buffer.InternalBlockCopy(hash.Hash, 0, array, 0, num2);
			array[array.Length - data.Length - 1] = 1;
			Buffer.InternalBlockCopy(data, 0, array, array.Length - data.Length, data.Length);
			byte[] array2 = new byte[num2];
			rng.GetBytes(array2);
			byte[] array3 = mgf.GenerateMask(array2, array.Length);
			for (int i = 0; i < array.Length; i++)
			{
				array[i] ^= array3[i];
			}
			array3 = mgf.GenerateMask(array, num2);
			for (int j = 0; j < array2.Length; j++)
			{
				byte[] array4 = array2;
				int num3 = j;
				array4[num3] ^= array3[j];
			}
			byte[] array5 = new byte[num];
			Buffer.InternalBlockCopy(array2, 0, array5, 0, array2.Length);
			Buffer.InternalBlockCopy(array, 0, array5, array2.Length, array.Length);
			return rsa.EncryptValue(array5);
		}

		// Token: 0x060050DB RID: 20699 RVA: 0x00123698 File Offset: 0x00122698
		internal static byte[] RsaOaepDecrypt(RSA rsa, HashAlgorithm hash, PKCS1MaskGenerationMethod mgf, byte[] encryptedData)
		{
			int num = rsa.KeySize / 8;
			byte[] array = null;
			try
			{
				array = rsa.DecryptValue(encryptedData);
			}
			catch (CryptographicException)
			{
				throw new CryptographicException(Environment.GetResourceString("Cryptography_OAEPDecoding"));
			}
			int num2 = hash.HashSize / 8;
			int num3 = num - array.Length;
			if (num3 < 0 || num3 >= num2)
			{
				throw new CryptographicException(Environment.GetResourceString("Cryptography_OAEPDecoding"));
			}
			byte[] array2 = new byte[num2];
			Buffer.InternalBlockCopy(array, 0, array2, num3, array2.Length - num3);
			byte[] array3 = new byte[array.Length - array2.Length + num3];
			Buffer.InternalBlockCopy(array, array2.Length - num3, array3, 0, array3.Length);
			byte[] array4 = mgf.GenerateMask(array3, array2.Length);
			int i;
			for (i = 0; i < array2.Length; i++)
			{
				byte[] array5 = array2;
				int num4 = i;
				array5[num4] ^= array4[i];
			}
			array4 = mgf.GenerateMask(array2, array3.Length);
			for (i = 0; i < array3.Length; i++)
			{
				array3[i] ^= array4[i];
			}
			hash.ComputeHash(new byte[0]);
			byte[] hash2 = hash.Hash;
			for (i = 0; i < num2; i++)
			{
				if (array3[i] != hash2[i])
				{
					throw new CryptographicException(Environment.GetResourceString("Cryptography_OAEPDecoding"));
				}
			}
			while (i < array3.Length && array3[i] != 1)
			{
				if (array3[i] != 0)
				{
					throw new CryptographicException(Environment.GetResourceString("Cryptography_OAEPDecoding"));
				}
				i++;
			}
			if (i == array3.Length)
			{
				throw new CryptographicException(Environment.GetResourceString("Cryptography_OAEPDecoding"));
			}
			i++;
			byte[] array6 = new byte[array3.Length - i];
			Buffer.InternalBlockCopy(array3, i, array6, 0, array6.Length);
			return array6;
		}

		// Token: 0x060050DC RID: 20700 RVA: 0x00123860 File Offset: 0x00122860
		internal static byte[] RsaPkcs1Padding(RSA rsa, byte[] oid, byte[] hash)
		{
			int num = rsa.KeySize / 8;
			byte[] array = new byte[num];
			byte[] array2 = new byte[oid.Length + 8 + hash.Length];
			array2[0] = 48;
			int num2 = array2.Length - 2;
			array2[1] = (byte)num2;
			array2[2] = 48;
			num2 = oid.Length + 2;
			array2[3] = (byte)num2;
			Buffer.InternalBlockCopy(oid, 0, array2, 4, oid.Length);
			array2[4 + oid.Length] = 5;
			array2[4 + oid.Length + 1] = 0;
			array2[4 + oid.Length + 2] = 4;
			array2[4 + oid.Length + 3] = (byte)hash.Length;
			Buffer.InternalBlockCopy(hash, 0, array2, oid.Length + 8, hash.Length);
			int num3 = num - array2.Length;
			if (num3 <= 2)
			{
				throw new CryptographicUnexpectedOperationException(Environment.GetResourceString("Cryptography_InvalidOID"));
			}
			array[0] = 0;
			array[1] = 1;
			for (int i = 2; i < num3 - 1; i++)
			{
				array[i] = byte.MaxValue;
			}
			array[num3 - 1] = 0;
			Buffer.InternalBlockCopy(array2, 0, array, num3, array2.Length);
			return array;
		}

		// Token: 0x060050DD RID: 20701 RVA: 0x00123948 File Offset: 0x00122948
		internal static bool CompareBigIntArrays(byte[] lhs, byte[] rhs)
		{
			if (lhs == null)
			{
				return rhs == null;
			}
			int i = 0;
			int num = 0;
			while (i < lhs.Length)
			{
				if (lhs[i] != 0)
				{
					break;
				}
				i++;
			}
			while (num < rhs.Length && rhs[num] == 0)
			{
				num++;
			}
			int num2 = lhs.Length - i;
			if (rhs.Length - num != num2)
			{
				return false;
			}
			for (int j = 0; j < num2; j++)
			{
				if (lhs[i + j] != rhs[num + j])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060050DE RID: 20702
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void _AcquireCSP(CspParameters param, ref SafeProvHandle hProv);

		// Token: 0x060050DF RID: 20703
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void _CreateCSP(CspParameters param, bool randomKeyContainer, ref SafeProvHandle hProv);

		// Token: 0x060050E0 RID: 20704
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void _CreateHash(SafeProvHandle hProv, int algid, ref SafeHashHandle hKey);

		// Token: 0x060050E1 RID: 20705
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern byte[] _CryptDeriveKey(SafeProvHandle hProv, int algid, int algidHash, byte[] password, int dwFlags, byte[] IV);

		// Token: 0x060050E2 RID: 20706
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int _DecryptData(SafeKeyHandle hKey, byte[] data, int ib, int cb, ref byte[] outputBuffer, int outputOffset, PaddingMode PaddingMode, bool fDone);

		// Token: 0x060050E3 RID: 20707
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern byte[] _DecryptKey(SafeKeyHandle hPubKey, byte[] key, int dwFlags);

		// Token: 0x060050E4 RID: 20708
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern byte[] _DecryptPKWin2KEnh(SafeKeyHandle hPubKey, byte[] key, bool fOAEP, out int hr);

		// Token: 0x060050E5 RID: 20709
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int _EncryptData(SafeKeyHandle hKey, byte[] data, int ib, int cb, ref byte[] outputBuffer, int outputOffset, PaddingMode PaddingMode, bool fDone);

		// Token: 0x060050E6 RID: 20710
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern byte[] _EncryptKey(SafeKeyHandle hPubKey, byte[] key);

		// Token: 0x060050E7 RID: 20711
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern byte[] _EncryptPKWin2KEnh(SafeKeyHandle hPubKey, byte[] key, bool fOAEP, out int hr);

		// Token: 0x060050E8 RID: 20712
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern byte[] _EndHash(SafeHashHandle hHash);

		// Token: 0x060050E9 RID: 20713
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern byte[] _ExportCspBlob(SafeKeyHandle hKey, int blobType);

		// Token: 0x060050EA RID: 20714
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void _ExportKey(SafeKeyHandle hKey, int blobType, object cspObject);

		// Token: 0x060050EB RID: 20715
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void _GenerateKey(SafeProvHandle hProv, int algid, CspProviderFlags flags, int keySize, ref SafeKeyHandle hKey);

		// Token: 0x060050EC RID: 20716
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void _GetBytes(SafeProvHandle hProv, byte[] randomBytes);

		// Token: 0x060050ED RID: 20717
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool _GetEnforceFipsPolicySetting();

		// Token: 0x060050EE RID: 20718
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern byte[] _GetKeyParameter(SafeKeyHandle hKey, uint paramID);

		// Token: 0x060050EF RID: 20719
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern byte[] _GetKeySetSecurityInfo(SafeProvHandle hProv, SecurityInfos securityInfo, out int error);

		// Token: 0x060050F0 RID: 20720
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void _GetNonZeroBytes(SafeProvHandle hProv, byte[] randomBytes);

		// Token: 0x060050F1 RID: 20721
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool _GetPersistKeyInCsp(SafeProvHandle hProv);

		// Token: 0x060050F2 RID: 20722
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern object _GetProviderParameter(SafeProvHandle hProv, int keyNumber, uint paramID);

		// Token: 0x060050F3 RID: 20723
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string _GetRandomKeyContainer();

		// Token: 0x060050F4 RID: 20724
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int _GetUserKey(SafeProvHandle hProv, int keyNumber, ref SafeKeyHandle hKey);

		// Token: 0x060050F5 RID: 20725
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void _HashData(SafeHashHandle hHash, byte[] data, int ibStart, int cbSize);

		// Token: 0x060050F6 RID: 20726
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void _ImportBulkKey(SafeProvHandle hProv, int algid, bool useSalt, byte[] key, ref SafeKeyHandle hKey);

		// Token: 0x060050F7 RID: 20727
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int _ImportCspBlob(byte[] keyBlob, SafeProvHandle hProv, CspProviderFlags flags, ref SafeKeyHandle hKey);

		// Token: 0x060050F8 RID: 20728
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void _ImportKey(SafeProvHandle hCSP, int keyNumber, CspProviderFlags flags, object cspObject, ref SafeKeyHandle hKey);

		// Token: 0x060050F9 RID: 20729
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int _OpenCSP(CspParameters param, uint flags, ref SafeProvHandle hProv);

		// Token: 0x060050FA RID: 20730
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool _ProduceLegacyHmacValues();

		// Token: 0x060050FB RID: 20731
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool _SearchForAlgorithm(SafeProvHandle hProv, int algID, int keyLength);

		// Token: 0x060050FC RID: 20732
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void _SetKeyParamDw(SafeKeyHandle hKey, int param, int dwValue);

		// Token: 0x060050FD RID: 20733
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void _SetKeyParamRgb(SafeKeyHandle hKey, int param, byte[] value);

		// Token: 0x060050FE RID: 20734
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int _SetKeySetSecurityInfo(SafeProvHandle hProv, SecurityInfos securityInfo, byte[] sd);

		// Token: 0x060050FF RID: 20735
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void _SetPersistKeyInCsp(SafeProvHandle hProv, bool fPersistKeyInCsp);

		// Token: 0x06005100 RID: 20736
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void _SetProviderParameter(SafeProvHandle hProv, int keyNumber, uint paramID, IntPtr pbData);

		// Token: 0x06005101 RID: 20737
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void _ShowLegacyHmacWarning();

		// Token: 0x06005102 RID: 20738
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern byte[] _SignValue(SafeKeyHandle hKey, int keyNumber, int calgKey, int calgHash, byte[] hash, int dwFlags);

		// Token: 0x06005103 RID: 20739
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool _VerifySign(SafeKeyHandle hKey, int calgKey, int calgHash, byte[] hash, byte[] signature, int dwFlags);

		// Token: 0x04002986 RID: 10630
		private static object s_InternalSyncObject;

		// Token: 0x04002987 RID: 10631
		private static int _defaultRsaProviderType = -1;

		// Token: 0x04002988 RID: 10632
		private static SafeProvHandle _safeProvHandle = null;

		// Token: 0x04002989 RID: 10633
		private static SafeProvHandle _safeDssProvHandle = null;

		// Token: 0x0400298A RID: 10634
		private static RNGCryptoServiceProvider _rng = null;

		// Token: 0x0400298B RID: 10635
		private static int s_hasEnhProv = -1;

		// Token: 0x0400298C RID: 10636
		private static int s_fipsAlgorithmPolicy = -1;

		// Token: 0x0400298D RID: 10637
		private static int s_win2KCrypto = -1;
	}
}
