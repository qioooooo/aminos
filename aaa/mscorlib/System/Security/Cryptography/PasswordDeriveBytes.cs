using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace System.Security.Cryptography
{
	// Token: 0x0200087C RID: 2172
	[ComVisible(true)]
	public class PasswordDeriveBytes : DeriveBytes
	{
		// Token: 0x17000DF1 RID: 3569
		// (get) Token: 0x06004F7B RID: 20347 RVA: 0x001154A8 File Offset: 0x001144A8
		private SafeProvHandle ProvHandle
		{
			get
			{
				if (this._safeProvHandle == null)
				{
					lock (this)
					{
						if (this._safeProvHandle == null)
						{
							SafeProvHandle safeProvHandle = Utils.AcquireProvHandle(this._cspParams);
							Thread.MemoryBarrier();
							this._safeProvHandle = safeProvHandle;
						}
					}
				}
				return this._safeProvHandle;
			}
		}

		// Token: 0x06004F7C RID: 20348 RVA: 0x00115504 File Offset: 0x00114504
		public PasswordDeriveBytes(string strPassword, byte[] rgbSalt)
			: this(strPassword, rgbSalt, new CspParameters())
		{
		}

		// Token: 0x06004F7D RID: 20349 RVA: 0x00115513 File Offset: 0x00114513
		public PasswordDeriveBytes(byte[] password, byte[] salt)
			: this(password, salt, new CspParameters())
		{
		}

		// Token: 0x06004F7E RID: 20350 RVA: 0x00115522 File Offset: 0x00114522
		public PasswordDeriveBytes(string strPassword, byte[] rgbSalt, string strHashName, int iterations)
			: this(strPassword, rgbSalt, strHashName, iterations, new CspParameters())
		{
		}

		// Token: 0x06004F7F RID: 20351 RVA: 0x00115534 File Offset: 0x00114534
		public PasswordDeriveBytes(byte[] password, byte[] salt, string hashName, int iterations)
			: this(password, salt, hashName, iterations, new CspParameters())
		{
		}

		// Token: 0x06004F80 RID: 20352 RVA: 0x00115546 File Offset: 0x00114546
		public PasswordDeriveBytes(string strPassword, byte[] rgbSalt, CspParameters cspParams)
			: this(strPassword, rgbSalt, "SHA1", 100, cspParams)
		{
		}

		// Token: 0x06004F81 RID: 20353 RVA: 0x00115558 File Offset: 0x00114558
		public PasswordDeriveBytes(byte[] password, byte[] salt, CspParameters cspParams)
			: this(password, salt, "SHA1", 100, cspParams)
		{
		}

		// Token: 0x06004F82 RID: 20354 RVA: 0x0011556A File Offset: 0x0011456A
		public PasswordDeriveBytes(string strPassword, byte[] rgbSalt, string strHashName, int iterations, CspParameters cspParams)
			: this(new UTF8Encoding(false).GetBytes(strPassword), rgbSalt, strHashName, iterations, cspParams)
		{
		}

		// Token: 0x06004F83 RID: 20355 RVA: 0x00115584 File Offset: 0x00114584
		public PasswordDeriveBytes(byte[] password, byte[] salt, string hashName, int iterations, CspParameters cspParams)
		{
			this.IterationCount = iterations;
			this.Salt = salt;
			this.HashName = hashName;
			this._password = password;
			this._cspParams = cspParams;
		}

		// Token: 0x17000DF2 RID: 3570
		// (get) Token: 0x06004F84 RID: 20356 RVA: 0x001155B1 File Offset: 0x001145B1
		// (set) Token: 0x06004F85 RID: 20357 RVA: 0x001155BC File Offset: 0x001145BC
		public string HashName
		{
			get
			{
				return this._hashName;
			}
			set
			{
				if (this._baseValue != null)
				{
					throw new CryptographicException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Cryptography_PasswordDerivedBytes_ValuesFixed"), new object[] { "HashName" }));
				}
				this._hashName = value;
				this._hash = (HashAlgorithm)CryptoConfig.CreateFromName(this._hashName);
			}
		}

		// Token: 0x17000DF3 RID: 3571
		// (get) Token: 0x06004F86 RID: 20358 RVA: 0x00115618 File Offset: 0x00114618
		// (set) Token: 0x06004F87 RID: 20359 RVA: 0x00115620 File Offset: 0x00114620
		public int IterationCount
		{
			get
			{
				return this._iterations;
			}
			set
			{
				if (this._baseValue != null)
				{
					throw new CryptographicException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Cryptography_PasswordDerivedBytes_ValuesFixed"), new object[] { "IterationCount" }));
				}
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException("value", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				this._iterations = value;
			}
		}

		// Token: 0x17000DF4 RID: 3572
		// (get) Token: 0x06004F88 RID: 20360 RVA: 0x0011567F File Offset: 0x0011467F
		// (set) Token: 0x06004F89 RID: 20361 RVA: 0x0011569C File Offset: 0x0011469C
		public byte[] Salt
		{
			get
			{
				if (this._salt == null)
				{
					return null;
				}
				return (byte[])this._salt.Clone();
			}
			set
			{
				if (this._baseValue != null)
				{
					throw new CryptographicException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Cryptography_PasswordDerivedBytes_ValuesFixed"), new object[] { "Salt" }));
				}
				if (value == null)
				{
					this._salt = null;
					return;
				}
				this._salt = (byte[])value.Clone();
			}
		}

		// Token: 0x06004F8A RID: 20362 RVA: 0x001156F8 File Offset: 0x001146F8
		[Obsolete("Rfc2898DeriveBytes replaces PasswordDeriveBytes for deriving key material from a password and is preferred in new applications.")]
		public override byte[] GetBytes(int cb)
		{
			int num = 0;
			byte[] array = new byte[cb];
			if (this._baseValue == null)
			{
				this.ComputeBaseValue();
			}
			else if (this._extra != null)
			{
				num = this._extra.Length - this._extraCount;
				if (num >= cb)
				{
					Buffer.InternalBlockCopy(this._extra, this._extraCount, array, 0, cb);
					if (num > cb)
					{
						this._extraCount += cb;
					}
					else
					{
						this._extra = null;
					}
					return array;
				}
				Buffer.InternalBlockCopy(this._extra, num, array, 0, num);
				this._extra = null;
			}
			byte[] array2 = this.ComputeBytes(cb - num);
			Buffer.InternalBlockCopy(array2, 0, array, num, cb - num);
			if (array2.Length + num > cb)
			{
				this._extra = array2;
				this._extraCount = cb - num;
			}
			return array;
		}

		// Token: 0x06004F8B RID: 20363 RVA: 0x001157B1 File Offset: 0x001147B1
		public override void Reset()
		{
			this._prefix = 0;
			this._extra = null;
			this._baseValue = null;
		}

		// Token: 0x06004F8C RID: 20364 RVA: 0x001157C8 File Offset: 0x001147C8
		public byte[] CryptDeriveKey(string algname, string alghashname, int keySize, byte[] rgbIV)
		{
			if (keySize < 0)
			{
				throw new CryptographicException(Environment.GetResourceString("Cryptography_InvalidKeySize"));
			}
			int num = X509Utils.OidToAlgId(alghashname);
			if (num == 0)
			{
				throw new CryptographicException(Environment.GetResourceString("Cryptography_PasswordDerivedBytes_InvalidAlgorithm"));
			}
			int num2 = X509Utils.OidToAlgId(algname);
			if (num2 == 0)
			{
				throw new CryptographicException(Environment.GetResourceString("Cryptography_PasswordDerivedBytes_InvalidAlgorithm"));
			}
			if (rgbIV == null)
			{
				throw new CryptographicException(Environment.GetResourceString("Cryptography_PasswordDerivedBytes_InvalidIV"));
			}
			return Utils._CryptDeriveKey(this.ProvHandle, num2, num, this._password, keySize << 16, rgbIV);
		}

		// Token: 0x06004F8D RID: 20365 RVA: 0x0011584C File Offset: 0x0011484C
		private byte[] ComputeBaseValue()
		{
			this._hash.Initialize();
			this._hash.TransformBlock(this._password, 0, this._password.Length, this._password, 0);
			if (this._salt != null)
			{
				this._hash.TransformBlock(this._salt, 0, this._salt.Length, this._salt, 0);
			}
			this._hash.TransformFinalBlock(new byte[0], 0, 0);
			this._baseValue = this._hash.Hash;
			this._hash.Initialize();
			for (int i = 1; i < this._iterations - 1; i++)
			{
				this._hash.ComputeHash(this._baseValue);
				this._baseValue = this._hash.Hash;
			}
			return this._baseValue;
		}

		// Token: 0x06004F8E RID: 20366 RVA: 0x0011591C File Offset: 0x0011491C
		private byte[] ComputeBytes(int cb)
		{
			int num = 0;
			this._hash.Initialize();
			int num2 = this._hash.HashSize / 8;
			byte[] array = new byte[(cb + num2 - 1) / num2 * num2];
			CryptoStream cryptoStream = new CryptoStream(Stream.Null, this._hash, CryptoStreamMode.Write);
			this.HashPrefix(cryptoStream);
			cryptoStream.Write(this._baseValue, 0, this._baseValue.Length);
			cryptoStream.Close();
			Buffer.InternalBlockCopy(this._hash.Hash, 0, array, num, num2);
			num += num2;
			while (cb > num)
			{
				this._hash.Initialize();
				cryptoStream = new CryptoStream(Stream.Null, this._hash, CryptoStreamMode.Write);
				this.HashPrefix(cryptoStream);
				cryptoStream.Write(this._baseValue, 0, this._baseValue.Length);
				cryptoStream.Close();
				Buffer.InternalBlockCopy(this._hash.Hash, 0, array, num, num2);
				num += num2;
			}
			return array;
		}

		// Token: 0x06004F8F RID: 20367 RVA: 0x00115A04 File Offset: 0x00114A04
		private void HashPrefix(CryptoStream cs)
		{
			int num = 0;
			byte[] array = new byte[] { 48, 48, 48 };
			if (this._prefix > 999)
			{
				throw new CryptographicException(Environment.GetResourceString("Cryptography_PasswordDerivedBytes_TooManyBytes"));
			}
			if (this._prefix >= 100)
			{
				byte[] array2 = array;
				int num2 = 0;
				array2[num2] += (byte)(this._prefix / 100);
				num++;
			}
			if (this._prefix >= 10)
			{
				byte[] array3 = array;
				int num3 = num;
				array3[num3] += (byte)(this._prefix % 100 / 10);
				num++;
			}
			if (this._prefix > 0)
			{
				byte[] array4 = array;
				int num4 = num;
				array4[num4] += (byte)(this._prefix % 10);
				num++;
				cs.Write(array, 0, num);
			}
			this._prefix++;
		}

		// Token: 0x040028C8 RID: 10440
		private int _extraCount;

		// Token: 0x040028C9 RID: 10441
		private int _prefix;

		// Token: 0x040028CA RID: 10442
		private int _iterations;

		// Token: 0x040028CB RID: 10443
		private byte[] _baseValue;

		// Token: 0x040028CC RID: 10444
		private byte[] _extra;

		// Token: 0x040028CD RID: 10445
		private byte[] _salt;

		// Token: 0x040028CE RID: 10446
		private string _hashName;

		// Token: 0x040028CF RID: 10447
		private byte[] _password;

		// Token: 0x040028D0 RID: 10448
		private HashAlgorithm _hash;

		// Token: 0x040028D1 RID: 10449
		private CspParameters _cspParams;

		// Token: 0x040028D2 RID: 10450
		private SafeProvHandle _safeProvHandle;
	}
}
