using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x02000333 RID: 819
	public class X509ChainElement
	{
		// Token: 0x060019E3 RID: 6627 RVA: 0x0005A3E4 File Offset: 0x000593E4
		private X509ChainElement()
		{
		}

		// Token: 0x060019E4 RID: 6628 RVA: 0x0005A3EC File Offset: 0x000593EC
		internal unsafe X509ChainElement(IntPtr pChainElement)
		{
			CAPIBase.CERT_CHAIN_ELEMENT cert_CHAIN_ELEMENT = new CAPIBase.CERT_CHAIN_ELEMENT(Marshal.SizeOf(typeof(CAPIBase.CERT_CHAIN_ELEMENT)));
			uint num = (uint)Marshal.ReadInt32(pChainElement);
			if ((ulong)num > (ulong)((long)Marshal.SizeOf(cert_CHAIN_ELEMENT)))
			{
				num = (uint)Marshal.SizeOf(cert_CHAIN_ELEMENT);
			}
			X509Utils.memcpy(pChainElement, new IntPtr((void*)(&cert_CHAIN_ELEMENT)), num);
			this.m_certificate = new X509Certificate2(cert_CHAIN_ELEMENT.pCertContext);
			if (cert_CHAIN_ELEMENT.pwszExtendedErrorInfo == IntPtr.Zero)
			{
				this.m_description = string.Empty;
			}
			else
			{
				this.m_description = Marshal.PtrToStringUni(cert_CHAIN_ELEMENT.pwszExtendedErrorInfo);
			}
			if (cert_CHAIN_ELEMENT.dwErrorStatus == 0U)
			{
				this.m_chainStatus = new X509ChainStatus[0];
				return;
			}
			this.m_chainStatus = X509Chain.GetChainStatusInformation(cert_CHAIN_ELEMENT.dwErrorStatus);
		}

		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x060019E5 RID: 6629 RVA: 0x0005A4B8 File Offset: 0x000594B8
		public X509Certificate2 Certificate
		{
			get
			{
				return this.m_certificate;
			}
		}

		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x060019E6 RID: 6630 RVA: 0x0005A4C0 File Offset: 0x000594C0
		public X509ChainStatus[] ChainElementStatus
		{
			get
			{
				return this.m_chainStatus;
			}
		}

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x060019E7 RID: 6631 RVA: 0x0005A4C8 File Offset: 0x000594C8
		public string Information
		{
			get
			{
				return this.m_description;
			}
		}

		// Token: 0x04001AE2 RID: 6882
		private X509Certificate2 m_certificate;

		// Token: 0x04001AE3 RID: 6883
		private X509ChainStatus[] m_chainStatus;

		// Token: 0x04001AE4 RID: 6884
		private string m_description;
	}
}
