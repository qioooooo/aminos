using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x0200033E RID: 830
	public sealed class X509EnhancedKeyUsageExtension : X509Extension
	{
		// Token: 0x06001A20 RID: 6688 RVA: 0x0005ADF9 File Offset: 0x00059DF9
		public X509EnhancedKeyUsageExtension()
			: base("2.5.29.37")
		{
			this.m_enhancedKeyUsages = new OidCollection();
			this.m_decoded = true;
		}

		// Token: 0x06001A21 RID: 6689 RVA: 0x0005AE18 File Offset: 0x00059E18
		public X509EnhancedKeyUsageExtension(OidCollection enhancedKeyUsages, bool critical)
			: base("2.5.29.37", X509EnhancedKeyUsageExtension.EncodeExtension(enhancedKeyUsages), critical)
		{
		}

		// Token: 0x06001A22 RID: 6690 RVA: 0x0005AE2C File Offset: 0x00059E2C
		public X509EnhancedKeyUsageExtension(AsnEncodedData encodedEnhancedKeyUsages, bool critical)
			: base("2.5.29.37", encodedEnhancedKeyUsages.RawData, critical)
		{
		}

		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x06001A23 RID: 6691 RVA: 0x0005AE40 File Offset: 0x00059E40
		public OidCollection EnhancedKeyUsages
		{
			get
			{
				if (!this.m_decoded)
				{
					this.DecodeExtension();
				}
				OidCollection oidCollection = new OidCollection();
				foreach (Oid oid in this.m_enhancedKeyUsages)
				{
					oidCollection.Add(oid);
				}
				return oidCollection;
			}
		}

		// Token: 0x06001A24 RID: 6692 RVA: 0x0005AE87 File Offset: 0x00059E87
		public override void CopyFrom(AsnEncodedData asnEncodedData)
		{
			base.CopyFrom(asnEncodedData);
			this.m_decoded = false;
		}

		// Token: 0x06001A25 RID: 6693 RVA: 0x0005AE98 File Offset: 0x00059E98
		private void DecodeExtension()
		{
			uint num = 0U;
			SafeLocalAllocHandle safeLocalAllocHandle = null;
			if (!CAPI.DecodeObject(new IntPtr(36L), this.m_rawData, out safeLocalAllocHandle, out num))
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			CAPIBase.CERT_ENHKEY_USAGE cert_ENHKEY_USAGE = (CAPIBase.CERT_ENHKEY_USAGE)Marshal.PtrToStructure(safeLocalAllocHandle.DangerousGetHandle(), typeof(CAPIBase.CERT_ENHKEY_USAGE));
			this.m_enhancedKeyUsages = new OidCollection();
			int num2 = 0;
			while ((long)num2 < (long)((ulong)cert_ENHKEY_USAGE.cUsageIdentifier))
			{
				IntPtr intPtr = Marshal.ReadIntPtr(new IntPtr((long)cert_ENHKEY_USAGE.rgpszUsageIdentifier + (long)(num2 * Marshal.SizeOf(typeof(IntPtr)))));
				string text = Marshal.PtrToStringAnsi(intPtr);
				Oid oid = new Oid(text, OidGroup.ExtensionOrAttribute, false);
				this.m_enhancedKeyUsages.Add(oid);
				num2++;
			}
			this.m_decoded = true;
			safeLocalAllocHandle.Dispose();
		}

		// Token: 0x06001A26 RID: 6694 RVA: 0x0005AF68 File Offset: 0x00059F68
		private unsafe static byte[] EncodeExtension(OidCollection enhancedKeyUsages)
		{
			if (enhancedKeyUsages == null)
			{
				throw new ArgumentNullException("enhancedKeyUsages");
			}
			SafeLocalAllocHandle safeLocalAllocHandle = X509Utils.CopyOidsToUnmanagedMemory(enhancedKeyUsages);
			byte[] array = null;
			using (safeLocalAllocHandle)
			{
				CAPIBase.CERT_ENHKEY_USAGE cert_ENHKEY_USAGE = default(CAPIBase.CERT_ENHKEY_USAGE);
				cert_ENHKEY_USAGE.cUsageIdentifier = (uint)enhancedKeyUsages.Count;
				cert_ENHKEY_USAGE.rgpszUsageIdentifier = safeLocalAllocHandle.DangerousGetHandle();
				if (!CAPI.EncodeObject("2.5.29.37", new IntPtr((void*)(&cert_ENHKEY_USAGE)), out array))
				{
					throw new CryptographicException(Marshal.GetLastWin32Error());
				}
			}
			return array;
		}

		// Token: 0x04001B19 RID: 6937
		private OidCollection m_enhancedKeyUsages;

		// Token: 0x04001B1A RID: 6938
		private bool m_decoded;
	}
}
