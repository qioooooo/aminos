using System;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x02000340 RID: 832
	public sealed class X509SubjectKeyIdentifierExtension : X509Extension
	{
		// Token: 0x06001A27 RID: 6695 RVA: 0x0005AFF0 File Offset: 0x00059FF0
		public X509SubjectKeyIdentifierExtension()
			: base("2.5.29.14")
		{
			this.m_subjectKeyIdentifier = null;
			this.m_decoded = true;
		}

		// Token: 0x06001A28 RID: 6696 RVA: 0x0005B00B File Offset: 0x0005A00B
		public X509SubjectKeyIdentifierExtension(string subjectKeyIdentifier, bool critical)
			: base("2.5.29.14", X509SubjectKeyIdentifierExtension.EncodeExtension(subjectKeyIdentifier), critical)
		{
		}

		// Token: 0x06001A29 RID: 6697 RVA: 0x0005B01F File Offset: 0x0005A01F
		public X509SubjectKeyIdentifierExtension(byte[] subjectKeyIdentifier, bool critical)
			: base("2.5.29.14", X509SubjectKeyIdentifierExtension.EncodeExtension(subjectKeyIdentifier), critical)
		{
		}

		// Token: 0x06001A2A RID: 6698 RVA: 0x0005B033 File Offset: 0x0005A033
		public X509SubjectKeyIdentifierExtension(AsnEncodedData encodedSubjectKeyIdentifier, bool critical)
			: base("2.5.29.14", encodedSubjectKeyIdentifier.RawData, critical)
		{
		}

		// Token: 0x06001A2B RID: 6699 RVA: 0x0005B047 File Offset: 0x0005A047
		public X509SubjectKeyIdentifierExtension(PublicKey key, bool critical)
			: base("2.5.29.14", X509SubjectKeyIdentifierExtension.EncodePublicKey(key, X509SubjectKeyIdentifierHashAlgorithm.Sha1), critical)
		{
		}

		// Token: 0x06001A2C RID: 6700 RVA: 0x0005B05C File Offset: 0x0005A05C
		public X509SubjectKeyIdentifierExtension(PublicKey key, X509SubjectKeyIdentifierHashAlgorithm algorithm, bool critical)
			: base("2.5.29.14", X509SubjectKeyIdentifierExtension.EncodePublicKey(key, algorithm), critical)
		{
		}

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x06001A2D RID: 6701 RVA: 0x0005B071 File Offset: 0x0005A071
		public string SubjectKeyIdentifier
		{
			get
			{
				if (!this.m_decoded)
				{
					this.DecodeExtension();
				}
				return this.m_subjectKeyIdentifier;
			}
		}

		// Token: 0x06001A2E RID: 6702 RVA: 0x0005B087 File Offset: 0x0005A087
		public override void CopyFrom(AsnEncodedData asnEncodedData)
		{
			base.CopyFrom(asnEncodedData);
			this.m_decoded = false;
		}

		// Token: 0x06001A2F RID: 6703 RVA: 0x0005B098 File Offset: 0x0005A098
		private void DecodeExtension()
		{
			uint num = 0U;
			SafeLocalAllocHandle safeLocalAllocHandle = null;
			SafeLocalAllocHandle safeLocalAllocHandle2 = X509Utils.StringToAnsiPtr("2.5.29.14");
			if (!CAPI.DecodeObject(safeLocalAllocHandle2.DangerousGetHandle(), this.m_rawData, out safeLocalAllocHandle, out num))
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			CAPIBase.CRYPTOAPI_BLOB cryptoapi_BLOB = (CAPIBase.CRYPTOAPI_BLOB)Marshal.PtrToStructure(safeLocalAllocHandle.DangerousGetHandle(), typeof(CAPIBase.CRYPTOAPI_BLOB));
			byte[] array = CAPI.BlobToByteArray(cryptoapi_BLOB);
			this.m_subjectKeyIdentifier = X509Utils.EncodeHexString(array);
			this.m_decoded = true;
			safeLocalAllocHandle.Dispose();
			safeLocalAllocHandle2.Dispose();
		}

		// Token: 0x06001A30 RID: 6704 RVA: 0x0005B11D File Offset: 0x0005A11D
		private static byte[] EncodeExtension(string subjectKeyIdentifier)
		{
			if (subjectKeyIdentifier == null)
			{
				throw new ArgumentNullException("subjectKeyIdentifier");
			}
			return X509SubjectKeyIdentifierExtension.EncodeExtension(X509Utils.DecodeHexString(subjectKeyIdentifier));
		}

		// Token: 0x06001A31 RID: 6705 RVA: 0x0005B138 File Offset: 0x0005A138
		private unsafe static byte[] EncodeExtension(byte[] subjectKeyIdentifier)
		{
			if (subjectKeyIdentifier == null)
			{
				throw new ArgumentNullException("subjectKeyIdentifier");
			}
			if (subjectKeyIdentifier.Length == 0)
			{
				throw new ArgumentException("subjectKeyIdentifier");
			}
			byte[] array = null;
			fixed (byte* ptr = subjectKeyIdentifier)
			{
				CAPIBase.CRYPTOAPI_BLOB cryptoapi_BLOB = default(CAPIBase.CRYPTOAPI_BLOB);
				cryptoapi_BLOB.pbData = new IntPtr((void*)ptr);
				cryptoapi_BLOB.cbData = (uint)subjectKeyIdentifier.Length;
				if (!CAPI.EncodeObject("2.5.29.14", new IntPtr((void*)(&cryptoapi_BLOB)), out array))
				{
					throw new CryptographicException(Marshal.GetLastWin32Error());
				}
			}
			return array;
		}

		// Token: 0x06001A32 RID: 6706 RVA: 0x0005B1C4 File Offset: 0x0005A1C4
		private unsafe static SafeLocalAllocHandle EncodePublicKey(PublicKey key)
		{
			SafeLocalAllocHandle safeLocalAllocHandle = SafeLocalAllocHandle.InvalidHandle;
			CAPIBase.CERT_PUBLIC_KEY_INFO2* ptr = null;
			string value = key.Oid.Value;
			byte[] rawData = key.EncodedParameters.RawData;
			byte[] rawData2 = key.EncodedKeyValue.RawData;
			uint num = (uint)((long)Marshal.SizeOf(typeof(CAPIBase.CERT_PUBLIC_KEY_INFO2)) + (long)((ulong)X509Utils.AlignedLength((uint)(value.Length + 1))) + (long)((ulong)X509Utils.AlignedLength((uint)rawData.Length)) + (long)rawData2.Length);
			safeLocalAllocHandle = CAPI.LocalAlloc(64U, new IntPtr((long)((ulong)num)));
			ptr = (CAPIBase.CERT_PUBLIC_KEY_INFO2*)(void*)safeLocalAllocHandle.DangerousGetHandle();
			IntPtr intPtr = new IntPtr(ptr + (long)Marshal.SizeOf(typeof(CAPIBase.CERT_PUBLIC_KEY_INFO2)) / (long)sizeof(CAPIBase.CERT_PUBLIC_KEY_INFO2));
			IntPtr intPtr2 = new IntPtr((long)intPtr + (long)((ulong)X509Utils.AlignedLength((uint)(value.Length + 1))));
			IntPtr intPtr3 = new IntPtr((long)intPtr2 + (long)((ulong)X509Utils.AlignedLength((uint)rawData.Length)));
			ptr->Algorithm.pszObjId = intPtr;
			byte[] array = new byte[value.Length + 1];
			Encoding.ASCII.GetBytes(value, 0, value.Length, array, 0);
			Marshal.Copy(array, 0, intPtr, array.Length);
			if (rawData.Length > 0)
			{
				ptr->Algorithm.Parameters.cbData = (uint)rawData.Length;
				ptr->Algorithm.Parameters.pbData = intPtr2;
				Marshal.Copy(rawData, 0, intPtr2, rawData.Length);
			}
			ptr->PublicKey.cbData = (uint)rawData2.Length;
			ptr->PublicKey.pbData = intPtr3;
			Marshal.Copy(rawData2, 0, intPtr3, rawData2.Length);
			return safeLocalAllocHandle;
		}

		// Token: 0x06001A33 RID: 6707 RVA: 0x0005B340 File Offset: 0x0005A340
		private unsafe static byte[] EncodePublicKey(PublicKey key, X509SubjectKeyIdentifierHashAlgorithm algorithm)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			SafeLocalAllocHandle safeLocalAllocHandle = X509SubjectKeyIdentifierExtension.EncodePublicKey(key);
			CAPIBase.CERT_PUBLIC_KEY_INFO2* ptr = (CAPIBase.CERT_PUBLIC_KEY_INFO2*)(void*)safeLocalAllocHandle.DangerousGetHandle();
			byte[] array = new byte[20];
			byte[] array2 = null;
			fixed (byte* ptr2 = array)
			{
				uint num = (uint)array.Length;
				IntPtr intPtr = new IntPtr((void*)ptr2);
				try
				{
					if (algorithm == X509SubjectKeyIdentifierHashAlgorithm.Sha1 || X509SubjectKeyIdentifierHashAlgorithm.ShortSha1 == algorithm)
					{
						if (!CAPISafe.CryptHashCertificate(IntPtr.Zero, 32772U, 0U, ptr->PublicKey.pbData, ptr->PublicKey.cbData, intPtr, new IntPtr((void*)(&num))))
						{
							throw new CryptographicException(Marshal.GetHRForLastWin32Error());
						}
					}
					else
					{
						if (X509SubjectKeyIdentifierHashAlgorithm.CapiSha1 != algorithm)
						{
							throw new ArgumentException("algorithm");
						}
						if (!CAPISafe.CryptHashPublicKeyInfo(IntPtr.Zero, 32772U, 0U, 1U, new IntPtr((void*)ptr), intPtr, new IntPtr((void*)(&num))))
						{
							throw new CryptographicException(Marshal.GetHRForLastWin32Error());
						}
					}
					if (X509SubjectKeyIdentifierHashAlgorithm.ShortSha1 == algorithm)
					{
						array2 = new byte[8];
						Array.Copy(array, array.Length - 8, array2, 0, array2.Length);
						byte[] array3 = array2;
						int num2 = 0;
						array3[num2] &= 15;
						byte[] array4 = array2;
						int num3 = 0;
						array4[num3] |= 64;
					}
					else
					{
						array2 = array;
						if (array.Length > (int)num)
						{
							array2 = new byte[num];
							Array.Copy(array, 0, array2, 0, array2.Length);
						}
					}
				}
				finally
				{
					safeLocalAllocHandle.Dispose();
				}
			}
			return X509SubjectKeyIdentifierExtension.EncodeExtension(array2);
		}

		// Token: 0x04001B1F RID: 6943
		private string m_subjectKeyIdentifier;

		// Token: 0x04001B20 RID: 6944
		private bool m_decoded;
	}
}
