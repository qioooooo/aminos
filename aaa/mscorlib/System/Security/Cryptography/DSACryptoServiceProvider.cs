using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;

namespace System.Security.Cryptography
{
	// Token: 0x02000869 RID: 2153
	[ComVisible(true)]
	public sealed class DSACryptoServiceProvider : DSA, ICspAsymmetricAlgorithm
	{
		// Token: 0x06004EE4 RID: 20196 RVA: 0x00113647 File Offset: 0x00112647
		public DSACryptoServiceProvider()
			: this(0, new CspParameters(13, null, null, DSACryptoServiceProvider.s_UseMachineKeyStore))
		{
		}

		// Token: 0x06004EE5 RID: 20197 RVA: 0x0011365E File Offset: 0x0011265E
		public DSACryptoServiceProvider(int dwKeySize)
			: this(dwKeySize, new CspParameters(13, null, null, DSACryptoServiceProvider.s_UseMachineKeyStore))
		{
		}

		// Token: 0x06004EE6 RID: 20198 RVA: 0x00113675 File Offset: 0x00112675
		public DSACryptoServiceProvider(CspParameters parameters)
			: this(0, parameters)
		{
		}

		// Token: 0x06004EE7 RID: 20199 RVA: 0x00113680 File Offset: 0x00112680
		public DSACryptoServiceProvider(int dwKeySize, CspParameters parameters)
		{
			if (dwKeySize < 0)
			{
				throw new ArgumentOutOfRangeException("dwKeySize", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			this._parameters = Utils.SaveCspParameters(CspAlgorithmType.Dss, parameters, DSACryptoServiceProvider.s_UseMachineKeyStore, ref this._randomKeyContainer);
			this.LegalKeySizesValue = new KeySizes[]
			{
				new KeySizes(512, 1024, 64)
			};
			this._dwKeySize = dwKeySize;
			this._sha1 = new SHA1CryptoServiceProvider();
			if (!this._randomKeyContainer || Environment.GetCompatibilityFlag(CompatibilityFlag.EagerlyGenerateRandomAsymmKeys))
			{
				this.GetKeyPair();
			}
		}

		// Token: 0x06004EE8 RID: 20200 RVA: 0x00113710 File Offset: 0x00112710
		private void GetKeyPair()
		{
			if (this._safeKeyHandle == null)
			{
				lock (this)
				{
					if (this._safeKeyHandle == null)
					{
						Utils.GetKeyPairHelper(CspAlgorithmType.Dss, this._parameters, this._randomKeyContainer, this._dwKeySize, ref this._safeProvHandle, ref this._safeKeyHandle);
					}
				}
			}
		}

		// Token: 0x06004EE9 RID: 20201 RVA: 0x00113774 File Offset: 0x00112774
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

		// Token: 0x17000DC8 RID: 3528
		// (get) Token: 0x06004EEA RID: 20202 RVA: 0x001137C4 File Offset: 0x001127C4
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

		// Token: 0x17000DC9 RID: 3529
		// (get) Token: 0x06004EEB RID: 20203 RVA: 0x001137EA File Offset: 0x001127EA
		[ComVisible(false)]
		public CspKeyContainerInfo CspKeyContainerInfo
		{
			get
			{
				this.GetKeyPair();
				return new CspKeyContainerInfo(this._parameters, this._randomKeyContainer);
			}
		}

		// Token: 0x17000DCA RID: 3530
		// (get) Token: 0x06004EEC RID: 20204 RVA: 0x00113804 File Offset: 0x00112804
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

		// Token: 0x17000DCB RID: 3531
		// (get) Token: 0x06004EED RID: 20205 RVA: 0x00113847 File Offset: 0x00112847
		public override string KeyExchangeAlgorithm
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000DCC RID: 3532
		// (get) Token: 0x06004EEE RID: 20206 RVA: 0x0011384A File Offset: 0x0011284A
		public override string SignatureAlgorithm
		{
			get
			{
				return "http://www.w3.org/2000/09/xmldsig#dsa-sha1";
			}
		}

		// Token: 0x17000DCD RID: 3533
		// (get) Token: 0x06004EEF RID: 20207 RVA: 0x00113851 File Offset: 0x00112851
		// (set) Token: 0x06004EF0 RID: 20208 RVA: 0x0011385B File Offset: 0x0011285B
		public static bool UseMachineKeyStore
		{
			get
			{
				return DSACryptoServiceProvider.s_UseMachineKeyStore == CspProviderFlags.UseMachineKeyStore;
			}
			set
			{
				DSACryptoServiceProvider.s_UseMachineKeyStore = (value ? CspProviderFlags.UseMachineKeyStore : CspProviderFlags.NoFlags);
			}
		}

		// Token: 0x17000DCE RID: 3534
		// (get) Token: 0x06004EF1 RID: 20209 RVA: 0x0011386C File Offset: 0x0011286C
		// (set) Token: 0x06004EF2 RID: 20210 RVA: 0x001138CC File Offset: 0x001128CC
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

		// Token: 0x06004EF3 RID: 20211 RVA: 0x00113938 File Offset: 0x00112938
		public override DSAParameters ExportParameters(bool includePrivateParameters)
		{
			this.GetKeyPair();
			if (includePrivateParameters)
			{
				KeyContainerPermission keyContainerPermission = new KeyContainerPermission(KeyContainerPermissionFlags.NoFlags);
				KeyContainerPermissionAccessEntry keyContainerPermissionAccessEntry = new KeyContainerPermissionAccessEntry(this._parameters, KeyContainerPermissionFlags.Export);
				keyContainerPermission.AccessEntries.Add(keyContainerPermissionAccessEntry);
				keyContainerPermission.Demand();
			}
			DSACspObject dsacspObject = new DSACspObject();
			int num = (includePrivateParameters ? 7 : 6);
			Utils._ExportKey(this._safeKeyHandle, num, dsacspObject);
			return DSACryptoServiceProvider.DSAObjectToStruct(dsacspObject);
		}

		// Token: 0x06004EF4 RID: 20212 RVA: 0x00113997 File Offset: 0x00112997
		[ComVisible(false)]
		public byte[] ExportCspBlob(bool includePrivateParameters)
		{
			this.GetKeyPair();
			return Utils.ExportCspBlobHelper(includePrivateParameters, this._parameters, this._safeKeyHandle);
		}

		// Token: 0x06004EF5 RID: 20213 RVA: 0x001139B4 File Offset: 0x001129B4
		public override void ImportParameters(DSAParameters parameters)
		{
			DSACspObject dsacspObject = DSACryptoServiceProvider.DSAStructToObject(parameters);
			if (this._safeKeyHandle != null && !this._safeKeyHandle.IsClosed)
			{
				this._safeKeyHandle.Dispose();
			}
			this._safeKeyHandle = SafeKeyHandle.InvalidHandle;
			if (DSACryptoServiceProvider.IsPublic(parameters))
			{
				Utils._ImportKey(Utils.StaticDssProvHandle, 8704, CspProviderFlags.NoFlags, dsacspObject, ref this._safeKeyHandle);
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
			Utils._ImportKey(this._safeProvHandle, 8704, this._parameters.Flags, dsacspObject, ref this._safeKeyHandle);
		}

		// Token: 0x06004EF6 RID: 20214 RVA: 0x00113A7C File Offset: 0x00112A7C
		[ComVisible(false)]
		public void ImportCspBlob(byte[] keyBlob)
		{
			Utils.ImportCspBlobHelper(CspAlgorithmType.Dss, keyBlob, DSACryptoServiceProvider.IsPublic(keyBlob), ref this._parameters, this._randomKeyContainer, ref this._safeProvHandle, ref this._safeKeyHandle);
		}

		// Token: 0x06004EF7 RID: 20215 RVA: 0x00113AA4 File Offset: 0x00112AA4
		public byte[] SignData(Stream inputStream)
		{
			byte[] array = this._sha1.ComputeHash(inputStream);
			return this.SignHash(array, null);
		}

		// Token: 0x06004EF8 RID: 20216 RVA: 0x00113AC8 File Offset: 0x00112AC8
		public byte[] SignData(byte[] buffer)
		{
			byte[] array = this._sha1.ComputeHash(buffer);
			return this.SignHash(array, null);
		}

		// Token: 0x06004EF9 RID: 20217 RVA: 0x00113AEC File Offset: 0x00112AEC
		public byte[] SignData(byte[] buffer, int offset, int count)
		{
			byte[] array = this._sha1.ComputeHash(buffer, offset, count);
			return this.SignHash(array, null);
		}

		// Token: 0x06004EFA RID: 20218 RVA: 0x00113B10 File Offset: 0x00112B10
		public bool VerifyData(byte[] rgbData, byte[] rgbSignature)
		{
			byte[] array = this._sha1.ComputeHash(rgbData);
			return this.VerifyHash(array, null, rgbSignature);
		}

		// Token: 0x06004EFB RID: 20219 RVA: 0x00113B33 File Offset: 0x00112B33
		public override byte[] CreateSignature(byte[] rgbHash)
		{
			return this.SignHash(rgbHash, null);
		}

		// Token: 0x06004EFC RID: 20220 RVA: 0x00113B3D File Offset: 0x00112B3D
		public override bool VerifySignature(byte[] rgbHash, byte[] rgbSignature)
		{
			return this.VerifyHash(rgbHash, null, rgbSignature);
		}

		// Token: 0x06004EFD RID: 20221 RVA: 0x00113B48 File Offset: 0x00112B48
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
			if (rgbHash.Length != this._sha1.HashSize / 8)
			{
				throw new CryptographicException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Cryptography_InvalidHashSize"), new object[]
				{
					"SHA1",
					this._sha1.HashSize / 8
				}));
			}
			this.GetKeyPair();
			if (!this.CspKeyContainerInfo.RandomlyGenerated)
			{
				KeyContainerPermission keyContainerPermission = new KeyContainerPermission(KeyContainerPermissionFlags.NoFlags);
				KeyContainerPermissionAccessEntry keyContainerPermissionAccessEntry = new KeyContainerPermissionAccessEntry(this._parameters, KeyContainerPermissionFlags.Sign);
				keyContainerPermission.AccessEntries.Add(keyContainerPermissionAccessEntry);
				keyContainerPermission.Demand();
			}
			return Utils._SignValue(this._safeKeyHandle, this._parameters.KeyNumber, 8704, num, rgbHash, 0);
		}

		// Token: 0x06004EFE RID: 20222 RVA: 0x00113C30 File Offset: 0x00112C30
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
			int num = X509Utils.OidToAlgId(str);
			if (rgbHash.Length != this._sha1.HashSize / 8)
			{
				throw new CryptographicException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Cryptography_InvalidHashSize"), new object[]
				{
					"SHA1",
					this._sha1.HashSize / 8
				}));
			}
			this.GetKeyPair();
			return Utils._VerifySign(this._safeKeyHandle, 8704, num, rgbHash, rgbSignature, 0);
		}

		// Token: 0x06004EFF RID: 20223 RVA: 0x00113CCC File Offset: 0x00112CCC
		private static DSAParameters DSAObjectToStruct(DSACspObject dsaCspObject)
		{
			return new DSAParameters
			{
				P = dsaCspObject.P,
				Q = dsaCspObject.Q,
				G = dsaCspObject.G,
				Y = dsaCspObject.Y,
				J = dsaCspObject.J,
				X = dsaCspObject.X,
				Seed = dsaCspObject.Seed,
				Counter = dsaCspObject.Counter
			};
		}

		// Token: 0x06004F00 RID: 20224 RVA: 0x00113D4C File Offset: 0x00112D4C
		private static DSACspObject DSAStructToObject(DSAParameters dsaParams)
		{
			return new DSACspObject
			{
				P = dsaParams.P,
				Q = dsaParams.Q,
				G = dsaParams.G,
				Y = dsaParams.Y,
				J = dsaParams.J,
				X = dsaParams.X,
				Seed = dsaParams.Seed,
				Counter = dsaParams.Counter
			};
		}

		// Token: 0x06004F01 RID: 20225 RVA: 0x00113DC8 File Offset: 0x00112DC8
		private static bool IsPublic(DSAParameters dsaParams)
		{
			return dsaParams.X == null;
		}

		// Token: 0x06004F02 RID: 20226 RVA: 0x00113DD4 File Offset: 0x00112DD4
		private static bool IsPublic(byte[] keyBlob)
		{
			if (keyBlob == null)
			{
				throw new ArgumentNullException("keyBlob");
			}
			return keyBlob[0] == 6 && (keyBlob[11] == 49 || keyBlob[11] == 51) && keyBlob[10] == 83 && keyBlob[9] == 83 && keyBlob[8] == 68;
		}

		// Token: 0x0400289F RID: 10399
		private int _dwKeySize;

		// Token: 0x040028A0 RID: 10400
		private CspParameters _parameters;

		// Token: 0x040028A1 RID: 10401
		private bool _randomKeyContainer;

		// Token: 0x040028A2 RID: 10402
		private SafeProvHandle _safeProvHandle;

		// Token: 0x040028A3 RID: 10403
		private SafeKeyHandle _safeKeyHandle;

		// Token: 0x040028A4 RID: 10404
		private SHA1CryptoServiceProvider _sha1;

		// Token: 0x040028A5 RID: 10405
		private static CspProviderFlags s_UseMachineKeyStore;
	}
}
