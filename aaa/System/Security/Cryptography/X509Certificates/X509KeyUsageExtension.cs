using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x0200033C RID: 828
	public sealed class X509KeyUsageExtension : X509Extension
	{
		// Token: 0x06001A10 RID: 6672 RVA: 0x0005AA40 File Offset: 0x00059A40
		public X509KeyUsageExtension()
			: base("2.5.29.15")
		{
			this.m_decoded = true;
		}

		// Token: 0x06001A11 RID: 6673 RVA: 0x0005AA54 File Offset: 0x00059A54
		public X509KeyUsageExtension(X509KeyUsageFlags keyUsages, bool critical)
			: base("2.5.29.15", X509KeyUsageExtension.EncodeExtension(keyUsages), critical)
		{
		}

		// Token: 0x06001A12 RID: 6674 RVA: 0x0005AA68 File Offset: 0x00059A68
		public X509KeyUsageExtension(AsnEncodedData encodedKeyUsage, bool critical)
			: base("2.5.29.15", encodedKeyUsage.RawData, critical)
		{
		}

		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x06001A13 RID: 6675 RVA: 0x0005AA7C File Offset: 0x00059A7C
		public X509KeyUsageFlags KeyUsages
		{
			get
			{
				if (!this.m_decoded)
				{
					this.DecodeExtension();
				}
				return (X509KeyUsageFlags)this.m_keyUsages;
			}
		}

		// Token: 0x06001A14 RID: 6676 RVA: 0x0005AA92 File Offset: 0x00059A92
		public override void CopyFrom(AsnEncodedData asnEncodedData)
		{
			base.CopyFrom(asnEncodedData);
			this.m_decoded = false;
		}

		// Token: 0x06001A15 RID: 6677 RVA: 0x0005AAA4 File Offset: 0x00059AA4
		private void DecodeExtension()
		{
			uint num = 0U;
			SafeLocalAllocHandle safeLocalAllocHandle = null;
			if (!CAPI.DecodeObject(new IntPtr(14L), this.m_rawData, out safeLocalAllocHandle, out num))
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			CAPIBase.CRYPTOAPI_BLOB cryptoapi_BLOB = (CAPIBase.CRYPTOAPI_BLOB)Marshal.PtrToStructure(safeLocalAllocHandle.DangerousGetHandle(), typeof(CAPIBase.CRYPTOAPI_BLOB));
			if (cryptoapi_BLOB.cbData > 4U)
			{
				cryptoapi_BLOB.cbData = 4U;
			}
			byte[] array = new byte[4];
			if (cryptoapi_BLOB.pbData != IntPtr.Zero)
			{
				Marshal.Copy(cryptoapi_BLOB.pbData, array, 0, (int)cryptoapi_BLOB.cbData);
			}
			this.m_keyUsages = BitConverter.ToUInt32(array, 0);
			this.m_decoded = true;
			safeLocalAllocHandle.Dispose();
		}

		// Token: 0x06001A16 RID: 6678 RVA: 0x0005AB54 File Offset: 0x00059B54
		private unsafe static byte[] EncodeExtension(X509KeyUsageFlags keyUsages)
		{
			CAPIBase.CRYPT_BIT_BLOB crypt_BIT_BLOB = default(CAPIBase.CRYPT_BIT_BLOB);
			crypt_BIT_BLOB.cbData = 2U;
			crypt_BIT_BLOB.pbData = new IntPtr((void*)(&keyUsages));
			crypt_BIT_BLOB.cUnusedBits = 0U;
			byte[] array = null;
			if (!CAPI.EncodeObject("2.5.29.15", new IntPtr((void*)(&crypt_BIT_BLOB)), out array))
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			return array;
		}

		// Token: 0x04001B13 RID: 6931
		private uint m_keyUsages;

		// Token: 0x04001B14 RID: 6932
		private bool m_decoded;
	}
}
