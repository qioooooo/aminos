using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using Microsoft.Win32;

namespace System.Net
{
	// Token: 0x0200039D RID: 925
	public class NetworkCredential : ICredentials, ICredentialsByHost
	{
		// Token: 0x06001CD1 RID: 7377 RVA: 0x0006DE0D File Offset: 0x0006CE0D
		public NetworkCredential()
		{
		}

		// Token: 0x06001CD2 RID: 7378 RVA: 0x0006DE1C File Offset: 0x0006CE1C
		public NetworkCredential(string userName, string password)
			: this(userName, password, string.Empty)
		{
		}

		// Token: 0x06001CD3 RID: 7379 RVA: 0x0006DE2B File Offset: 0x0006CE2B
		public NetworkCredential(string userName, string password, string domain)
			: this(userName, password, domain, true)
		{
		}

		// Token: 0x06001CD4 RID: 7380 RVA: 0x0006DE37 File Offset: 0x0006CE37
		internal NetworkCredential(string userName, string password, string domain, bool encrypt)
		{
			this.m_encrypt = encrypt;
			this.UserName = userName;
			this.Password = password;
			this.Domain = domain;
		}

		// Token: 0x06001CD5 RID: 7381 RVA: 0x0006DE64 File Offset: 0x0006CE64
		private void InitializePart1()
		{
			if (NetworkCredential.m_environmentUserNamePermission == null)
			{
				lock (NetworkCredential.lockingObject)
				{
					if (NetworkCredential.m_environmentUserNamePermission == null)
					{
						NetworkCredential.m_environmentDomainNamePermission = new EnvironmentPermission(EnvironmentPermissionAccess.Read, "USERDOMAIN");
						NetworkCredential.m_environmentUserNamePermission = new EnvironmentPermission(EnvironmentPermissionAccess.Read, "USERNAME");
					}
				}
			}
		}

		// Token: 0x06001CD6 RID: 7382 RVA: 0x0006DEC4 File Offset: 0x0006CEC4
		private void InitializePart2()
		{
			if (this.m_encrypt)
			{
				if (NetworkCredential.s_symmetricAlgorithm == null)
				{
					lock (NetworkCredential.lockingObject)
					{
						if (NetworkCredential.s_symmetricAlgorithm == null)
						{
							NetworkCredential.s_useTripleDES = this.ReadRegFips();
							SymmetricAlgorithm symmetricAlgorithm;
							if (NetworkCredential.s_useTripleDES)
							{
								symmetricAlgorithm = new TripleDESCryptoServiceProvider();
								symmetricAlgorithm.KeySize = 128;
								symmetricAlgorithm.GenerateKey();
							}
							else
							{
								NetworkCredential.s_random = new RNGCryptoServiceProvider();
								symmetricAlgorithm = Rijndael.Create();
								byte[] array = new byte[16];
								NetworkCredential.s_random.GetBytes(array);
								symmetricAlgorithm.Key = array;
							}
							NetworkCredential.s_symmetricAlgorithm = symmetricAlgorithm;
						}
					}
				}
				if (this.m_encryptionIV == null)
				{
					if (NetworkCredential.s_useTripleDES)
					{
						NetworkCredential.s_symmetricAlgorithm.GenerateIV();
						byte[] iv = NetworkCredential.s_symmetricAlgorithm.IV;
						Interlocked.CompareExchange<byte[]>(ref this.m_encryptionIV, iv, null);
						return;
					}
					byte[] array2 = new byte[16];
					NetworkCredential.s_random.GetBytes(array2);
					Interlocked.CompareExchange<byte[]>(ref this.m_encryptionIV, array2, null);
				}
			}
		}

		// Token: 0x1700059E RID: 1438
		// (get) Token: 0x06001CD7 RID: 7383 RVA: 0x0006DFC0 File Offset: 0x0006CFC0
		// (set) Token: 0x06001CD8 RID: 7384 RVA: 0x0006DFD8 File Offset: 0x0006CFD8
		public string UserName
		{
			get
			{
				this.InitializePart1();
				NetworkCredential.m_environmentUserNamePermission.Demand();
				return this.InternalGetUserName();
			}
			set
			{
				this.m_userName = this.Encrypt(value);
			}
		}

		// Token: 0x1700059F RID: 1439
		// (get) Token: 0x06001CD9 RID: 7385 RVA: 0x0006DFE7 File Offset: 0x0006CFE7
		// (set) Token: 0x06001CDA RID: 7386 RVA: 0x0006DFF9 File Offset: 0x0006CFF9
		public string Password
		{
			get
			{
				ExceptionHelper.UnmanagedPermission.Demand();
				return this.InternalGetPassword();
			}
			set
			{
				this.m_password = this.Encrypt(value);
			}
		}

		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x06001CDB RID: 7387 RVA: 0x0006E008 File Offset: 0x0006D008
		// (set) Token: 0x06001CDC RID: 7388 RVA: 0x0006E020 File Offset: 0x0006D020
		public string Domain
		{
			get
			{
				this.InitializePart1();
				NetworkCredential.m_environmentDomainNamePermission.Demand();
				return this.InternalGetDomain();
			}
			set
			{
				this.m_domain = this.Encrypt(value);
			}
		}

		// Token: 0x06001CDD RID: 7389 RVA: 0x0006E030 File Offset: 0x0006D030
		internal string InternalGetUserName()
		{
			return this.Decrypt(this.m_userName);
		}

		// Token: 0x06001CDE RID: 7390 RVA: 0x0006E04C File Offset: 0x0006D04C
		internal string InternalGetPassword()
		{
			return this.Decrypt(this.m_password);
		}

		// Token: 0x06001CDF RID: 7391 RVA: 0x0006E068 File Offset: 0x0006D068
		internal string InternalGetDomain()
		{
			return this.Decrypt(this.m_domain);
		}

		// Token: 0x06001CE0 RID: 7392 RVA: 0x0006E084 File Offset: 0x0006D084
		internal string InternalGetDomainUserName()
		{
			string text = this.InternalGetDomain();
			if (text.Length != 0)
			{
				text += "\\";
			}
			return text + this.InternalGetUserName();
		}

		// Token: 0x06001CE1 RID: 7393 RVA: 0x0006E0BA File Offset: 0x0006D0BA
		public NetworkCredential GetCredential(Uri uri, string authType)
		{
			return this;
		}

		// Token: 0x06001CE2 RID: 7394 RVA: 0x0006E0BD File Offset: 0x0006D0BD
		public NetworkCredential GetCredential(string host, int port, string authenticationType)
		{
			return this;
		}

		// Token: 0x06001CE3 RID: 7395 RVA: 0x0006E0C0 File Offset: 0x0006D0C0
		internal bool IsEqualTo(object compObject)
		{
			if (compObject == null)
			{
				return false;
			}
			if (this == compObject)
			{
				return true;
			}
			NetworkCredential networkCredential = compObject as NetworkCredential;
			return networkCredential != null && (this.InternalGetUserName() == networkCredential.InternalGetUserName() && this.InternalGetPassword() == networkCredential.InternalGetPassword()) && this.InternalGetDomain() == networkCredential.InternalGetDomain();
		}

		// Token: 0x06001CE4 RID: 7396 RVA: 0x0006E120 File Offset: 0x0006D120
		internal string Decrypt(byte[] ciphertext)
		{
			if (ciphertext == null)
			{
				return string.Empty;
			}
			if (!this.m_encrypt)
			{
				return Encoding.UTF8.GetString(ciphertext);
			}
			this.InitializePart2();
			MemoryStream memoryStream = new MemoryStream();
			CryptoStream cryptoStream = new CryptoStream(memoryStream, NetworkCredential.s_symmetricAlgorithm.CreateDecryptor(NetworkCredential.s_symmetricAlgorithm.Key, this.m_encryptionIV), CryptoStreamMode.Write);
			cryptoStream.Write(ciphertext, 0, ciphertext.Length);
			cryptoStream.FlushFinalBlock();
			byte[] array = memoryStream.ToArray();
			cryptoStream.Close();
			return Encoding.UTF8.GetString(array);
		}

		// Token: 0x06001CE5 RID: 7397 RVA: 0x0006E1A4 File Offset: 0x0006D1A4
		internal byte[] Encrypt(string text)
		{
			if (text == null || text.Length == 0)
			{
				return null;
			}
			if (!this.m_encrypt)
			{
				return Encoding.UTF8.GetBytes(text);
			}
			this.InitializePart2();
			MemoryStream memoryStream = new MemoryStream();
			CryptoStream cryptoStream = new CryptoStream(memoryStream, NetworkCredential.s_symmetricAlgorithm.CreateEncryptor(NetworkCredential.s_symmetricAlgorithm.Key, this.m_encryptionIV), CryptoStreamMode.Write);
			byte[] array = Encoding.UTF8.GetBytes(text);
			cryptoStream.Write(array, 0, array.Length);
			cryptoStream.FlushFinalBlock();
			array = memoryStream.ToArray();
			cryptoStream.Close();
			return array;
		}

		// Token: 0x06001CE6 RID: 7398 RVA: 0x0006E22C File Offset: 0x0006D22C
		[RegistryPermission(SecurityAction.Assert, Read = "HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\Lsa")]
		private bool ReadRegFips()
		{
			bool flag = false;
			bool flag2 = false;
			if (ComNetOS.IsVista)
			{
				uint num = UnsafeNclNativeMethods.BCryptGetFipsAlgorithmMode(out flag2);
				flag = num == 0U || num == 3221225524U;
			}
			else
			{
				RegistryKey registryKey = null;
				object obj = null;
				try
				{
					string text = "SYSTEM\\CurrentControlSet\\Control\\Lsa";
					registryKey = Registry.LocalMachine.OpenSubKey(text);
					if (registryKey != null)
					{
						obj = registryKey.GetValue("fipsalgorithmpolicy");
					}
					flag = true;
					if (obj != null && (int)obj == 1)
					{
						flag2 = true;
					}
				}
				catch
				{
				}
				finally
				{
					if (registryKey != null)
					{
						registryKey.Close();
					}
				}
			}
			return !flag || flag2;
		}

		// Token: 0x04001D40 RID: 7488
		private static EnvironmentPermission m_environmentUserNamePermission;

		// Token: 0x04001D41 RID: 7489
		private static EnvironmentPermission m_environmentDomainNamePermission;

		// Token: 0x04001D42 RID: 7490
		private static readonly object lockingObject = new object();

		// Token: 0x04001D43 RID: 7491
		private static SymmetricAlgorithm s_symmetricAlgorithm;

		// Token: 0x04001D44 RID: 7492
		private static RNGCryptoServiceProvider s_random;

		// Token: 0x04001D45 RID: 7493
		private static bool s_useTripleDES = false;

		// Token: 0x04001D46 RID: 7494
		private byte[] m_userName;

		// Token: 0x04001D47 RID: 7495
		private byte[] m_password;

		// Token: 0x04001D48 RID: 7496
		private byte[] m_domain;

		// Token: 0x04001D49 RID: 7497
		private byte[] m_encryptionIV;

		// Token: 0x04001D4A RID: 7498
		private bool m_encrypt = true;
	}
}
