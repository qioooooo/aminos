using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;

namespace System.Security.Cryptography
{
	// Token: 0x02000886 RID: 2182
	[ComVisible(true)]
	public sealed class RSACryptoServiceProvider : RSA, ICspAsymmetricAlgorithm
	{
		// Token: 0x06004FCE RID: 20430 RVA: 0x00118A53 File Offset: 0x00117A53
		public RSACryptoServiceProvider()
			: this(0, new CspParameters(Utils.DefaultRsaProviderType, null, null, RSACryptoServiceProvider.s_UseMachineKeyStore), true)
		{
		}

		// Token: 0x06004FCF RID: 20431 RVA: 0x00118A6E File Offset: 0x00117A6E
		public RSACryptoServiceProvider(int dwKeySize)
			: this(dwKeySize, new CspParameters(Utils.DefaultRsaProviderType, null, null, RSACryptoServiceProvider.s_UseMachineKeyStore), false)
		{
		}

		// Token: 0x06004FD0 RID: 20432 RVA: 0x00118A89 File Offset: 0x00117A89
		public RSACryptoServiceProvider(CspParameters parameters)
			: this(0, parameters, true)
		{
		}

		// Token: 0x06004FD1 RID: 20433 RVA: 0x00118A94 File Offset: 0x00117A94
		public RSACryptoServiceProvider(int dwKeySize, CspParameters parameters)
			: this(dwKeySize, parameters, false)
		{
		}

		// Token: 0x06004FD2 RID: 20434 RVA: 0x00118AA0 File Offset: 0x00117AA0
		private RSACryptoServiceProvider(int dwKeySize, CspParameters parameters, bool useDefaultKeySize)
		{
			if (dwKeySize < 0)
			{
				throw new ArgumentOutOfRangeException("dwKeySize", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			bool flag = (parameters.Flags & (CspProviderFlags)(-2147483648)) != CspProviderFlags.NoFlags;
			parameters.Flags &= (CspProviderFlags)2147483647;
			this._parameters = Utils.SaveCspParameters(CspAlgorithmType.Rsa, parameters, RSACryptoServiceProvider.s_UseMachineKeyStore, ref this._randomKeyContainer);
			if (this._parameters.KeyNumber == 2 || Utils.HasEnhProv == 1)
			{
				this.LegalKeySizesValue = new KeySizes[]
				{
					new KeySizes(384, 16384, 8)
				};
				if (useDefaultKeySize)
				{
					this._dwKeySize = 1024;
				}
			}
			else
			{
				this.LegalKeySizesValue = new KeySizes[]
				{
					new KeySizes(384, 512, 8)
				};
				if (useDefaultKeySize)
				{
					this._dwKeySize = 512;
				}
			}
			if (!useDefaultKeySize)
			{
				this._dwKeySize = dwKeySize;
			}
			if (!this._randomKeyContainer || Environment.GetCompatibilityFlag(CompatibilityFlag.EagerlyGenerateRandomAsymmKeys))
			{
				this.GetKeyPair();
			}
			this._randomKeyContainer = this._randomKeyContainer || flag;
		}

		// Token: 0x06004FD3 RID: 20435 RVA: 0x00118BB0 File Offset: 0x00117BB0
		private void GetKeyPair()
		{
			if (this._safeKeyHandle == null)
			{
				lock (this)
				{
					if (this._safeKeyHandle == null)
					{
						Utils.GetKeyPairHelper(CspAlgorithmType.Rsa, this._parameters, this._randomKeyContainer, this._dwKeySize, ref this._safeProvHandle, ref this._safeKeyHandle);
					}
				}
			}
		}

		// Token: 0x06004FD4 RID: 20436 RVA: 0x00118C14 File Offset: 0x00117C14
		protected override void Dispose(bool disposing)
		{
			if (this._safeKeyHandle != null && !this._safeKeyHandle.IsClosed)
			{
				this._safeKeyHandle.Dispose();
			}
			if (this._safeProvHandle != null && !this._safeProvHandle.IsClosed)
			{
				this._safeProvHandle.Dispose();
			}
		}

		// Token: 0x17000DFC RID: 3580
		// (get) Token: 0x06004FD5 RID: 20437 RVA: 0x00118C64 File Offset: 0x00117C64
		[ComVisible(false)]
		public bool PublicOnly
		{
			get
			{
				this.GetKeyPair();
				byte[] array = Utils._GetKeyParameter(this._safeKeyHandle, 2U);
				return array[0] == 1;
			}
		}

		// Token: 0x17000DFD RID: 3581
		// (get) Token: 0x06004FD6 RID: 20438 RVA: 0x00118C8A File Offset: 0x00117C8A
		[ComVisible(false)]
		public CspKeyContainerInfo CspKeyContainerInfo
		{
			get
			{
				this.GetKeyPair();
				return new CspKeyContainerInfo(this._parameters, this._randomKeyContainer);
			}
		}

		// Token: 0x17000DFE RID: 3582
		// (get) Token: 0x06004FD7 RID: 20439 RVA: 0x00118CA4 File Offset: 0x00117CA4
		public override int KeySize
		{
			get
			{
				this.GetKeyPair();
				byte[] array = Utils._GetKeyParameter(this._safeKeyHandle, 1U);
				this._dwKeySize = (int)array[0] | ((int)array[1] << 8) | ((int)array[2] << 16) | ((int)array[3] << 24);
				return this._dwKeySize;
			}
		}

		// Token: 0x17000DFF RID: 3583
		// (get) Token: 0x06004FD8 RID: 20440 RVA: 0x00118CE7 File Offset: 0x00117CE7
		public override string KeyExchangeAlgorithm
		{
			get
			{
				if (this._parameters.KeyNumber == 1)
				{
					return "RSA-PKCS1-KeyEx";
				}
				return null;
			}
		}

		// Token: 0x17000E00 RID: 3584
		// (get) Token: 0x06004FD9 RID: 20441 RVA: 0x00118CFE File Offset: 0x00117CFE
		public override string SignatureAlgorithm
		{
			get
			{
				return "http://www.w3.org/2000/09/xmldsig#rsa-sha1";
			}
		}

		// Token: 0x17000E01 RID: 3585
		// (get) Token: 0x06004FDA RID: 20442 RVA: 0x00118D05 File Offset: 0x00117D05
		// (set) Token: 0x06004FDB RID: 20443 RVA: 0x00118D0F File Offset: 0x00117D0F
		public static bool UseMachineKeyStore
		{
			get
			{
				return RSACryptoServiceProvider.s_UseMachineKeyStore == CspProviderFlags.UseMachineKeyStore;
			}
			set
			{
				RSACryptoServiceProvider.s_UseMachineKeyStore = (value ? CspProviderFlags.UseMachineKeyStore : CspProviderFlags.NoFlags);
			}
		}

		// Token: 0x17000E02 RID: 3586
		// (get) Token: 0x06004FDC RID: 20444 RVA: 0x00118D20 File Offset: 0x00117D20
		// (set) Token: 0x06004FDD RID: 20445 RVA: 0x00118D80 File Offset: 0x00117D80
		public bool PersistKeyInCsp
		{
			get
			{
				if (this._safeProvHandle == null)
				{
					lock (this)
					{
						if (this._safeProvHandle == null)
						{
							this._safeProvHandle = Utils.CreateProvHandle(this._parameters, this._randomKeyContainer);
						}
					}
				}
				return Utils._GetPersistKeyInCsp(this._safeProvHandle);
			}
			set
			{
				bool persistKeyInCsp = this.PersistKeyInCsp;
				if (value == persistKeyInCsp)
				{
					return;
				}
				KeyContainerPermission keyContainerPermission = new KeyContainerPermission(KeyContainerPermissionFlags.NoFlags);
				if (!value)
				{
					KeyContainerPermissionAccessEntry keyContainerPermissionAccessEntry = new KeyContainerPermissionAccessEntry(this._parameters, KeyContainerPermissionFlags.Delete);
					keyContainerPermission.AccessEntries.Add(keyContainerPermissionAccessEntry);
				}
				else
				{
					KeyContainerPermissionAccessEntry keyContainerPermissionAccessEntry2 = new KeyContainerPermissionAccessEntry(this._parameters, KeyContainerPermissionFlags.Create);
					keyContainerPermission.AccessEntries.Add(keyContainerPermissionAccessEntry2);
				}
				keyContainerPermission.Demand();
				Utils._SetPersistKeyInCsp(this._safeProvHandle, value);
			}
		}

		// Token: 0x06004FDE RID: 20446 RVA: 0x00118DEC File Offset: 0x00117DEC
		public override RSAParameters ExportParameters(bool includePrivateParameters)
		{
			this.GetKeyPair();
			if (includePrivateParameters)
			{
				KeyContainerPermission keyContainerPermission = new KeyContainerPermission(KeyContainerPermissionFlags.NoFlags);
				KeyContainerPermissionAccessEntry keyContainerPermissionAccessEntry = new KeyContainerPermissionAccessEntry(this._parameters, KeyContainerPermissionFlags.Export);
				keyContainerPermission.AccessEntries.Add(keyContainerPermissionAccessEntry);
				keyContainerPermission.Demand();
			}
			RSACspObject rsacspObject = new RSACspObject();
			int num = (includePrivateParameters ? 7 : 6);
			Utils._ExportKey(this._safeKeyHandle, num, rsacspObject);
			return RSACryptoServiceProvider.RSAObjectToStruct(rsacspObject);
		}

		// Token: 0x06004FDF RID: 20447 RVA: 0x00118E4B File Offset: 0x00117E4B
		[ComVisible(false)]
		public byte[] ExportCspBlob(bool includePrivateParameters)
		{
			this.GetKeyPair();
			return Utils.ExportCspBlobHelper(includePrivateParameters, this._parameters, this._safeKeyHandle);
		}

		// Token: 0x06004FE0 RID: 20448 RVA: 0x00118E68 File Offset: 0x00117E68
		public override void ImportParameters(RSAParameters parameters)
		{
			RSACspObject rsacspObject = RSACryptoServiceProvider.RSAStructToObject(parameters);
			if (this._safeKeyHandle != null && !this._safeKeyHandle.IsClosed)
			{
				this._safeKeyHandle.Dispose();
			}
			this._safeKeyHandle = SafeKeyHandle.InvalidHandle;
			if (RSACryptoServiceProvider.IsPublic(parameters))
			{
				Utils._ImportKey(Utils.StaticProvHandle, 41984, CspProviderFlags.NoFlags, rsacspObject, ref this._safeKeyHandle);
				return;
			}
			KeyContainerPermission keyContainerPermission = new KeyContainerPermission(KeyContainerPermissionFlags.NoFlags);
			KeyContainerPermissionAccessEntry keyContainerPermissionAccessEntry = new KeyContainerPermissionAccessEntry(this._parameters, KeyContainerPermissionFlags.Import);
			keyContainerPermission.AccessEntries.Add(keyContainerPermissionAccessEntry);
			keyContainerPermission.Demand();
			if (this._safeProvHandle == null)
			{
				this._safeProvHandle = Utils.CreateProvHandle(this._parameters, this._randomKeyContainer);
			}
			Utils._ImportKey(this._safeProvHandle, 41984, this._parameters.Flags, rsacspObject, ref this._safeKeyHandle);
		}

		// Token: 0x06004FE1 RID: 20449 RVA: 0x00118F30 File Offset: 0x00117F30
		[ComVisible(false)]
		public void ImportCspBlob(byte[] keyBlob)
		{
			Utils.ImportCspBlobHelper(CspAlgorithmType.Rsa, keyBlob, RSACryptoServiceProvider.IsPublic(keyBlob), ref this._parameters, this._randomKeyContainer, ref this._safeProvHandle, ref this._safeKeyHandle);
		}

		// Token: 0x06004FE2 RID: 20450 RVA: 0x00118F58 File Offset: 0x00117F58
		public byte[] SignData(Stream inputStream, object halg)
		{
			string text = Utils.ObjToOidValue(halg);
			HashAlgorithm hashAlgorithm = Utils.ObjToHashAlgorithm(halg);
			byte[] array = hashAlgorithm.ComputeHash(inputStream);
			return this.SignHash(array, text);
		}

		// Token: 0x06004FE3 RID: 20451 RVA: 0x00118F84 File Offset: 0x00117F84
		public byte[] SignData(byte[] buffer, object halg)
		{
			string text = Utils.ObjToOidValue(halg);
			HashAlgorithm hashAlgorithm = Utils.ObjToHashAlgorithm(halg);
			byte[] array = hashAlgorithm.ComputeHash(buffer);
			return this.SignHash(array, text);
		}

		// Token: 0x06004FE4 RID: 20452 RVA: 0x00118FB0 File Offset: 0x00117FB0
		public byte[] SignData(byte[] buffer, int offset, int count, object halg)
		{
			string text = Utils.ObjToOidValue(halg);
			HashAlgorithm hashAlgorithm = Utils.ObjToHashAlgorithm(halg);
			byte[] array = hashAlgorithm.ComputeHash(buffer, offset, count);
			return this.SignHash(array, text);
		}

		// Token: 0x06004FE5 RID: 20453 RVA: 0x00118FE0 File Offset: 0x00117FE0
		public bool VerifyData(byte[] buffer, object halg, byte[] signature)
		{
			string text = Utils.ObjToOidValue(halg);
			HashAlgorithm hashAlgorithm = Utils.ObjToHashAlgorithm(halg);
			byte[] array = hashAlgorithm.ComputeHash(buffer);
			return this.VerifyHash(array, text, signature);
		}

		// Token: 0x06004FE6 RID: 20454 RVA: 0x0011900C File Offset: 0x0011800C
		public byte[] SignHash(byte[] rgbHash, string str)
		{
			if (rgbHash == null)
			{
				throw new ArgumentNullException("rgbHash");
			}
			if (this.PublicOnly)
			{
				throw new CryptographicException(Environment.GetResourceString("Cryptography_CSP_NoPrivateKey"));
			}
			int num = X509Utils.OidToAlgId(str);
			this.GetKeyPair();
			if (!this._randomKeyContainer)
			{
				KeyContainerPermission keyContainerPermission = new KeyContainerPermission(KeyContainerPermissionFlags.NoFlags);
				KeyContainerPermissionAccessEntry keyContainerPermissionAccessEntry = new KeyContainerPermissionAccessEntry(this._parameters, KeyContainerPermissionFlags.Sign);
				keyContainerPermission.AccessEntries.Add(keyContainerPermissionAccessEntry);
				keyContainerPermission.Demand();
			}
			return Utils._SignValue(this._safeKeyHandle, this._parameters.KeyNumber, 9216, num, rgbHash, 0);
		}

		// Token: 0x06004FE7 RID: 20455 RVA: 0x001190A0 File Offset: 0x001180A0
		public bool VerifyHash(byte[] rgbHash, string str, byte[] rgbSignature)
		{
			if (rgbHash == null)
			{
				throw new ArgumentNullException("rgbHash");
			}
			if (rgbSignature == null)
			{
				throw new ArgumentNullException("rgbSignature");
			}
			int num = X509Utils.OidToAlgId(str, OidGroup.HashAlgorithm);
			return this.VerifyHash(rgbHash, num, rgbSignature);
		}

		// Token: 0x06004FE8 RID: 20456 RVA: 0x001190DA File Offset: 0x001180DA
		internal bool VerifyHash(byte[] rgbHash, int calgHash, byte[] rgbSignature)
		{
			if (rgbHash == null)
			{
				throw new ArgumentNullException("rgbHash");
			}
			if (rgbSignature == null)
			{
				throw new ArgumentNullException("rgbSignature");
			}
			this.GetKeyPair();
			return Utils._VerifySign(this._safeKeyHandle, 9216, calgHash, rgbHash, rgbSignature, 0);
		}

		// Token: 0x06004FE9 RID: 20457 RVA: 0x00119114 File Offset: 0x00118114
		public byte[] Encrypt(byte[] rgb, bool fOAEP)
		{
			if (rgb == null)
			{
				throw new ArgumentNullException("rgb");
			}
			this.GetKeyPair();
			int num = 0;
			byte[] array;
			if (fOAEP)
			{
				if (Utils.HasEnhProv != 1 || Utils.Win2KCrypto != 1)
				{
					throw new CryptographicException(Environment.GetResourceString("Cryptography_Padding_Win2KEnhOnly"));
				}
				array = Utils._EncryptPKWin2KEnh(this._safeKeyHandle, rgb, true, out num);
				if (num != 0)
				{
					throw new CryptographicException(num);
				}
			}
			else
			{
				array = Utils._EncryptPKWin2KEnh(this._safeKeyHandle, rgb, false, out num);
				if (num != 0)
				{
					array = Utils._EncryptKey(this._safeKeyHandle, rgb);
				}
			}
			return array;
		}

		// Token: 0x06004FEA RID: 20458 RVA: 0x00119198 File Offset: 0x00118198
		public byte[] Decrypt(byte[] rgb, bool fOAEP)
		{
			if (rgb == null)
			{
				throw new ArgumentNullException("rgb");
			}
			this.GetKeyPair();
			if (rgb.Length > this.KeySize / 8)
			{
				throw new CryptographicException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Cryptography_Padding_DecDataTooBig"), new object[] { this.KeySize / 8 }));
			}
			if (!this._randomKeyContainer)
			{
				KeyContainerPermission keyContainerPermission = new KeyContainerPermission(KeyContainerPermissionFlags.NoFlags);
				KeyContainerPermissionAccessEntry keyContainerPermissionAccessEntry = new KeyContainerPermissionAccessEntry(this._parameters, KeyContainerPermissionFlags.Decrypt);
				keyContainerPermission.AccessEntries.Add(keyContainerPermissionAccessEntry);
				keyContainerPermission.Demand();
			}
			int num = 0;
			byte[] array;
			if (fOAEP)
			{
				if (Utils.HasEnhProv != 1 || Utils.Win2KCrypto != 1)
				{
					throw new CryptographicException(Environment.GetResourceString("Cryptography_Padding_Win2KEnhOnly"));
				}
				array = Utils._DecryptPKWin2KEnh(this._safeKeyHandle, rgb, true, out num);
				if (num != 0)
				{
					throw new CryptographicException(num);
				}
			}
			else
			{
				array = Utils._DecryptPKWin2KEnh(this._safeKeyHandle, rgb, false, out num);
				if (num != 0)
				{
					array = Utils._DecryptKey(this._safeKeyHandle, rgb, 0);
				}
			}
			return array;
		}

		// Token: 0x06004FEB RID: 20459 RVA: 0x00119291 File Offset: 0x00118291
		public override byte[] DecryptValue(byte[] rgb)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_Method"));
		}

		// Token: 0x06004FEC RID: 20460 RVA: 0x001192A2 File Offset: 0x001182A2
		public override byte[] EncryptValue(byte[] rgb)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_Method"));
		}

		// Token: 0x06004FED RID: 20461 RVA: 0x001192B4 File Offset: 0x001182B4
		private static RSAParameters RSAObjectToStruct(RSACspObject rsaCspObject)
		{
			return new RSAParameters
			{
				Exponent = rsaCspObject.Exponent,
				Modulus = rsaCspObject.Modulus,
				P = rsaCspObject.P,
				Q = rsaCspObject.Q,
				DP = rsaCspObject.DP,
				DQ = rsaCspObject.DQ,
				InverseQ = rsaCspObject.InverseQ,
				D = rsaCspObject.D
			};
		}

		// Token: 0x06004FEE RID: 20462 RVA: 0x00119334 File Offset: 0x00118334
		private static RSACspObject RSAStructToObject(RSAParameters rsaParams)
		{
			return new RSACspObject
			{
				Exponent = rsaParams.Exponent,
				Modulus = rsaParams.Modulus,
				P = rsaParams.P,
				Q = rsaParams.Q,
				DP = rsaParams.DP,
				DQ = rsaParams.DQ,
				InverseQ = rsaParams.InverseQ,
				D = rsaParams.D
			};
		}

		// Token: 0x06004FEF RID: 20463 RVA: 0x001193B0 File Offset: 0x001183B0
		private static bool IsPublic(RSAParameters rsaParams)
		{
			return rsaParams.P == null;
		}

		// Token: 0x06004FF0 RID: 20464 RVA: 0x001193BC File Offset: 0x001183BC
		private static bool IsPublic(byte[] keyBlob)
		{
			if (keyBlob == null)
			{
				throw new ArgumentNullException("keyBlob");
			}
			return keyBlob[0] == 6 && keyBlob[11] == 49 && keyBlob[10] == 65 && keyBlob[9] == 83 && keyBlob[8] == 82;
		}

		// Token: 0x040028F5 RID: 10485
		private const uint RandomKeyContainerFlag = 2147483648U;

		// Token: 0x040028F6 RID: 10486
		private int _dwKeySize;

		// Token: 0x040028F7 RID: 10487
		private CspParameters _parameters;

		// Token: 0x040028F8 RID: 10488
		private bool _randomKeyContainer;

		// Token: 0x040028F9 RID: 10489
		private SafeProvHandle _safeProvHandle;

		// Token: 0x040028FA RID: 10490
		private SafeKeyHandle _safeKeyHandle;

		// Token: 0x040028FB RID: 10491
		private static CspProviderFlags s_UseMachineKeyStore;
	}
}
