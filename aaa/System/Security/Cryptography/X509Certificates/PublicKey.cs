using System;
using System.IO;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x02000326 RID: 806
	public sealed class PublicKey
	{
		// Token: 0x0600193C RID: 6460 RVA: 0x00055E48 File Offset: 0x00054E48
		private PublicKey()
		{
		}

		// Token: 0x0600193D RID: 6461 RVA: 0x00055E50 File Offset: 0x00054E50
		public PublicKey(Oid oid, AsnEncodedData parameters, AsnEncodedData keyValue)
		{
			this.m_oid = new Oid(oid);
			this.m_encodedParameters = new AsnEncodedData(parameters);
			this.m_encodedKeyValue = new AsnEncodedData(keyValue);
		}

		// Token: 0x0600193E RID: 6462 RVA: 0x00055E7C File Offset: 0x00054E7C
		internal PublicKey(PublicKey publicKey)
		{
			this.m_oid = new Oid(publicKey.m_oid);
			this.m_encodedParameters = new AsnEncodedData(publicKey.m_encodedParameters);
			this.m_encodedKeyValue = new AsnEncodedData(publicKey.m_encodedKeyValue);
		}

		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x0600193F RID: 6463 RVA: 0x00055EB7 File Offset: 0x00054EB7
		internal uint AlgorithmId
		{
			get
			{
				if (this.m_aiPubKey == 0U)
				{
					this.m_aiPubKey = X509Utils.OidToAlgId(this.m_oid.Value);
				}
				return this.m_aiPubKey;
			}
		}

		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x06001940 RID: 6464 RVA: 0x00055EDD File Offset: 0x00054EDD
		private byte[] CspBlobData
		{
			get
			{
				if (this.m_cspBlobData == null)
				{
					PublicKey.DecodePublicKeyObject(this.AlgorithmId, this.m_encodedKeyValue.RawData, this.m_encodedParameters.RawData, out this.m_cspBlobData);
				}
				return this.m_cspBlobData;
			}
		}

		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x06001941 RID: 6465 RVA: 0x00055F14 File Offset: 0x00054F14
		public AsymmetricAlgorithm Key
		{
			get
			{
				if (this.m_key == null)
				{
					uint algorithmId = this.AlgorithmId;
					if (algorithmId != 8704U)
					{
						if (algorithmId != 9216U && algorithmId != 41984U)
						{
							throw new NotSupportedException(SR.GetString("NotSupported_KeyAlgorithm"));
						}
						RSACryptoServiceProvider rsacryptoServiceProvider = new RSACryptoServiceProvider();
						rsacryptoServiceProvider.ImportCspBlob(this.CspBlobData);
						this.m_key = rsacryptoServiceProvider;
					}
					else
					{
						DSACryptoServiceProvider dsacryptoServiceProvider = new DSACryptoServiceProvider();
						dsacryptoServiceProvider.ImportCspBlob(this.CspBlobData);
						this.m_key = dsacryptoServiceProvider;
					}
				}
				return this.m_key;
			}
		}

		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x06001942 RID: 6466 RVA: 0x00055F94 File Offset: 0x00054F94
		public Oid Oid
		{
			get
			{
				return new Oid(this.m_oid);
			}
		}

		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x06001943 RID: 6467 RVA: 0x00055FA1 File Offset: 0x00054FA1
		public AsnEncodedData EncodedKeyValue
		{
			get
			{
				return this.m_encodedKeyValue;
			}
		}

		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x06001944 RID: 6468 RVA: 0x00055FA9 File Offset: 0x00054FA9
		public AsnEncodedData EncodedParameters
		{
			get
			{
				return this.m_encodedParameters;
			}
		}

		// Token: 0x06001945 RID: 6469 RVA: 0x00055FB4 File Offset: 0x00054FB4
		private static void DecodePublicKeyObject(uint aiPubKey, byte[] encodedKeyValue, byte[] encodedParameters, out byte[] decodedData)
		{
			decodedData = null;
			IntPtr zero = IntPtr.Zero;
			if (aiPubKey <= 9216U)
			{
				if (aiPubKey == 8704U)
				{
					zero = new IntPtr(38L);
					goto IL_0081;
				}
				if (aiPubKey != 9216U)
				{
					goto IL_0071;
				}
			}
			else if (aiPubKey != 41984U)
			{
				switch (aiPubKey)
				{
				case 43521U:
				case 43522U:
					throw new NotSupportedException(SR.GetString("NotSupported_KeyAlgorithm"));
				default:
					goto IL_0071;
				}
			}
			zero = new IntPtr(19L);
			goto IL_0081;
			IL_0071:
			throw new NotSupportedException(SR.GetString("NotSupported_KeyAlgorithm"));
			IL_0081:
			SafeLocalAllocHandle safeLocalAllocHandle = null;
			uint num = 0U;
			if (!CAPI.DecodeObject(zero, encodedKeyValue, out safeLocalAllocHandle, out num))
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			if ((int)zero == 19)
			{
				decodedData = new byte[num];
				Marshal.Copy(safeLocalAllocHandle.DangerousGetHandle(), decodedData, 0, decodedData.Length);
			}
			else if ((int)zero == 38)
			{
				SafeLocalAllocHandle safeLocalAllocHandle2 = null;
				uint num2 = 0U;
				if (!CAPI.DecodeObject(new IntPtr(39L), encodedParameters, out safeLocalAllocHandle2, out num2))
				{
					throw new CryptographicException(Marshal.GetLastWin32Error());
				}
				decodedData = PublicKey.ConstructDSSPubKeyCspBlob(safeLocalAllocHandle, safeLocalAllocHandle2);
				safeLocalAllocHandle2.Dispose();
			}
			safeLocalAllocHandle.Dispose();
		}

		// Token: 0x06001946 RID: 6470 RVA: 0x000560D0 File Offset: 0x000550D0
		private static byte[] ConstructDSSPubKeyCspBlob(SafeLocalAllocHandle decodedKeyValue, SafeLocalAllocHandle decodedParameters)
		{
			CAPIBase.CRYPTOAPI_BLOB cryptoapi_BLOB = (CAPIBase.CRYPTOAPI_BLOB)Marshal.PtrToStructure(decodedKeyValue.DangerousGetHandle(), typeof(CAPIBase.CRYPTOAPI_BLOB));
			CAPIBase.CERT_DSS_PARAMETERS cert_DSS_PARAMETERS = (CAPIBase.CERT_DSS_PARAMETERS)Marshal.PtrToStructure(decodedParameters.DangerousGetHandle(), typeof(CAPIBase.CERT_DSS_PARAMETERS));
			uint cbData = cert_DSS_PARAMETERS.p.cbData;
			if (cbData == 0U)
			{
				throw new CryptographicException(-2146893803);
			}
			uint num = 16U + cbData + 20U + cbData + cbData + 24U;
			MemoryStream memoryStream = new MemoryStream((int)num);
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			binaryWriter.Write(6);
			binaryWriter.Write(2);
			binaryWriter.Write(0);
			binaryWriter.Write(8704U);
			binaryWriter.Write(827544388U);
			binaryWriter.Write(cbData * 8U);
			byte[] array = new byte[cert_DSS_PARAMETERS.p.cbData];
			Marshal.Copy(cert_DSS_PARAMETERS.p.pbData, array, 0, array.Length);
			binaryWriter.Write(array);
			uint num2 = cert_DSS_PARAMETERS.q.cbData;
			if (num2 == 0U || num2 > 20U)
			{
				throw new CryptographicException(-2146893803);
			}
			byte[] array2 = new byte[cert_DSS_PARAMETERS.q.cbData];
			Marshal.Copy(cert_DSS_PARAMETERS.q.pbData, array2, 0, array2.Length);
			binaryWriter.Write(array2);
			if (20U > num2)
			{
				binaryWriter.Write(new byte[20U - num2]);
			}
			num2 = cert_DSS_PARAMETERS.g.cbData;
			if (num2 == 0U || num2 > cbData)
			{
				throw new CryptographicException(-2146893803);
			}
			byte[] array3 = new byte[cert_DSS_PARAMETERS.g.cbData];
			Marshal.Copy(cert_DSS_PARAMETERS.g.pbData, array3, 0, array3.Length);
			binaryWriter.Write(array3);
			if (cbData > num2)
			{
				binaryWriter.Write(new byte[cbData - num2]);
			}
			num2 = cryptoapi_BLOB.cbData;
			if (num2 == 0U || num2 > cbData)
			{
				throw new CryptographicException(-2146893803);
			}
			byte[] array4 = new byte[cryptoapi_BLOB.cbData];
			Marshal.Copy(cryptoapi_BLOB.pbData, array4, 0, array4.Length);
			binaryWriter.Write(array4);
			if (cbData > num2)
			{
				binaryWriter.Write(new byte[cbData - num2]);
			}
			binaryWriter.Write(uint.MaxValue);
			binaryWriter.Write(new byte[20]);
			return memoryStream.ToArray();
		}

		// Token: 0x04001A97 RID: 6807
		private AsnEncodedData m_encodedKeyValue;

		// Token: 0x04001A98 RID: 6808
		private AsnEncodedData m_encodedParameters;

		// Token: 0x04001A99 RID: 6809
		private Oid m_oid;

		// Token: 0x04001A9A RID: 6810
		private uint m_aiPubKey;

		// Token: 0x04001A9B RID: 6811
		private byte[] m_cspBlobData;

		// Token: 0x04001A9C RID: 6812
		private AsymmetricAlgorithm m_key;
	}
}
