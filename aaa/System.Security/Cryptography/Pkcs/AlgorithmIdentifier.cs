using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;

namespace System.Security.Cryptography.Pkcs
{
	// Token: 0x02000076 RID: 118
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public sealed class AlgorithmIdentifier
	{
		// Token: 0x06000178 RID: 376 RVA: 0x00007C33 File Offset: 0x00006C33
		public AlgorithmIdentifier()
		{
			this.Reset(new Oid("1.2.840.113549.3.7"), 0, new byte[0]);
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00007C52 File Offset: 0x00006C52
		public AlgorithmIdentifier(Oid oid)
		{
			this.Reset(oid, 0, new byte[0]);
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00007C68 File Offset: 0x00006C68
		public AlgorithmIdentifier(Oid oid, int keyLength)
		{
			this.Reset(oid, keyLength, new byte[0]);
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00007C7E File Offset: 0x00006C7E
		internal AlgorithmIdentifier(string oidValue)
		{
			this.Reset(new Oid(oidValue), 0, new byte[0]);
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00007C9C File Offset: 0x00006C9C
		internal AlgorithmIdentifier(CAPIBase.CERT_PUBLIC_KEY_INFO keyInfo)
		{
			SafeLocalAllocHandle safeLocalAllocHandle = CAPI.LocalAlloc(64U, new IntPtr(Marshal.SizeOf(typeof(CAPIBase.CERT_PUBLIC_KEY_INFO))));
			Marshal.StructureToPtr(keyInfo, safeLocalAllocHandle.DangerousGetHandle(), false);
			int num = (int)CAPISafe.CertGetPublicKeyLength(65537U, safeLocalAllocHandle.DangerousGetHandle());
			byte[] array = new byte[keyInfo.Algorithm.Parameters.cbData];
			if (array.Length > 0)
			{
				Marshal.Copy(keyInfo.Algorithm.Parameters.pbData, array, 0, array.Length);
			}
			Marshal.DestroyStructure(safeLocalAllocHandle.DangerousGetHandle(), typeof(CAPIBase.CERT_PUBLIC_KEY_INFO));
			safeLocalAllocHandle.Dispose();
			this.Reset(new Oid(keyInfo.Algorithm.pszObjId), num, array);
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00007D5C File Offset: 0x00006D5C
		internal AlgorithmIdentifier(CAPIBase.CRYPT_ALGORITHM_IDENTIFIER algorithmIdentifier)
		{
			int num = 0;
			uint num2 = 0U;
			SafeLocalAllocHandle invalidHandle = SafeLocalAllocHandle.InvalidHandle;
			byte[] array = new byte[0];
			uint num3 = X509Utils.OidToAlgId(algorithmIdentifier.pszObjId);
			if (num3 == 26114U)
			{
				if (algorithmIdentifier.Parameters.cbData > 0U)
				{
					if (!CAPI.DecodeObject(new IntPtr(41L), algorithmIdentifier.Parameters.pbData, algorithmIdentifier.Parameters.cbData, out invalidHandle, out num2))
					{
						throw new CryptographicException(Marshal.GetLastWin32Error());
					}
					CAPIBase.CRYPT_RC2_CBC_PARAMETERS crypt_RC2_CBC_PARAMETERS = (CAPIBase.CRYPT_RC2_CBC_PARAMETERS)Marshal.PtrToStructure(invalidHandle.DangerousGetHandle(), typeof(CAPIBase.CRYPT_RC2_CBC_PARAMETERS));
					uint dwVersion = crypt_RC2_CBC_PARAMETERS.dwVersion;
					if (dwVersion != 52U)
					{
						if (dwVersion != 58U)
						{
							if (dwVersion == 160U)
							{
								num = 40;
							}
						}
						else
						{
							num = 128;
						}
					}
					else
					{
						num = 56;
					}
					if (crypt_RC2_CBC_PARAMETERS.fIV)
					{
						array = (byte[])crypt_RC2_CBC_PARAMETERS.rgbIV.Clone();
					}
				}
			}
			else if (num3 == 26625U || num3 == 26113U || num3 == 26115U)
			{
				if (algorithmIdentifier.Parameters.cbData > 0U)
				{
					if (!CAPI.DecodeObject(new IntPtr(25L), algorithmIdentifier.Parameters.pbData, algorithmIdentifier.Parameters.cbData, out invalidHandle, out num2))
					{
						throw new CryptographicException(Marshal.GetLastWin32Error());
					}
					if (num2 > 0U)
					{
						if (num3 == 26625U)
						{
							CAPIBase.CRYPTOAPI_BLOB cryptoapi_BLOB = (CAPIBase.CRYPTOAPI_BLOB)Marshal.PtrToStructure(invalidHandle.DangerousGetHandle(), typeof(CAPIBase.CRYPTOAPI_BLOB));
							if (cryptoapi_BLOB.cbData > 0U)
							{
								array = new byte[cryptoapi_BLOB.cbData];
								Marshal.Copy(cryptoapi_BLOB.pbData, array, 0, array.Length);
							}
						}
						else
						{
							array = new byte[num2];
							Marshal.Copy(invalidHandle.DangerousGetHandle(), array, 0, array.Length);
						}
					}
				}
				if (num3 == 26625U)
				{
					num = 128 - array.Length * 8;
				}
				else if (num3 == 26113U)
				{
					num = 64;
				}
				else
				{
					num = 192;
				}
			}
			else if (algorithmIdentifier.Parameters.cbData > 0U)
			{
				array = new byte[algorithmIdentifier.Parameters.cbData];
				Marshal.Copy(algorithmIdentifier.Parameters.pbData, array, 0, array.Length);
			}
			this.Reset(new Oid(algorithmIdentifier.pszObjId), num, array);
			invalidHandle.Dispose();
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600017E RID: 382 RVA: 0x00007FA0 File Offset: 0x00006FA0
		// (set) Token: 0x0600017F RID: 383 RVA: 0x00007FA8 File Offset: 0x00006FA8
		public Oid Oid
		{
			get
			{
				return this.m_oid;
			}
			set
			{
				this.m_oid = value;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000180 RID: 384 RVA: 0x00007FB1 File Offset: 0x00006FB1
		// (set) Token: 0x06000181 RID: 385 RVA: 0x00007FB9 File Offset: 0x00006FB9
		public int KeyLength
		{
			get
			{
				return this.m_keyLength;
			}
			set
			{
				this.m_keyLength = value;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000182 RID: 386 RVA: 0x00007FC2 File Offset: 0x00006FC2
		// (set) Token: 0x06000183 RID: 387 RVA: 0x00007FCA File Offset: 0x00006FCA
		public byte[] Parameters
		{
			get
			{
				return this.m_parameters;
			}
			set
			{
				this.m_parameters = value;
			}
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00007FD3 File Offset: 0x00006FD3
		private void Reset(Oid oid, int keyLength, byte[] parameters)
		{
			this.m_oid = oid;
			this.m_keyLength = keyLength;
			this.m_parameters = parameters;
		}

		// Token: 0x0400049F RID: 1183
		private Oid m_oid;

		// Token: 0x040004A0 RID: 1184
		private int m_keyLength;

		// Token: 0x040004A1 RID: 1185
		private byte[] m_parameters;
	}
}
